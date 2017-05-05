using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SquareDanceASP.Models;
using SquareDanceASP.DBModels;
using System;
using System.IO;
using SquareDanceASP.Attribute;
using System.Net;
using GoogleMaps.LocationServices;
using System.Data.Entity.Spatial;

namespace SquareDanceASP.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                };
                //if (model.Sex == Enum.Enum.Sex.Male.ToString())
                //{
                //    user.Gender = 1;
                //}
                //else
                //{
                //    user.Gender = 2;
                //}
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        public ActionResult UserProfile()
        {
            var _db = new SquareDanceDb();

            var model = new UserProfileModel();
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                model = new UserProfileModel(user);
                var sitter = _db.Sitter.Find(user.Id);

                if (sitter != null)
                {
                    model.Sitter = true;
                }
            }
            return View(model);
        }

        public ActionResult _UserProfileEdit()
        {
            var model = new UserProfileModel();
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                model = new UserProfileModel(user);
            }
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult UserProfileEdit(UserProfileModel model)
        {
            var user = UserManager.FindById(model.UserId);
            if (user != null)
            {
                var point = new GoogleLocationService().GetLatLongFromAddress((!string.IsNullOrEmpty(model.Address)) ? model.Address : "");

                user.Name = model.Name;
                user.Address = model.Address;
                user.ConnectEmail = model.ConnectEmail;
                user.PhoneNumber = model.Phone;
                user.WeChat = model.WeChat;

                if (point != null)
                {
                    user.Location = DbGeography.FromText($"POINT({point.Longitude} {point.Latitude})");
                }

                var result = UserManager.Update(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("UserProfile");
                }
            }
            return View(model);
        }

        public ActionResult _AddItem(UserProfileModel model)
        {
            return PartialView(model);
        }

        public ActionResult AddDog()
        {
            var model = new PetModel();
            model.UserId = User.Identity.GetUserId();
            return View(model);
        }

        [HttpPost]
        [ValidateAjax]
        public JsonResult AddDog(PetModel model)
        {
            var _db = new SquareDanceDb();
            try
            {
                var newDog = new Pet(model);

                foreach (string file in Request.Files)
                {
                    var fileContent = Request.Files[file];
                    if (fileContent.ContentLength > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + fileContent.FileName;
                        var path = Path.Combine(Server.MapPath("~/Pic/"), fileName);
                        fileContent.SaveAs(path);
                        var petImage = new PetImage();
                        {
                            petImage.PetId = newDog.Id;
                            petImage.Path = Path.Combine("http://www.liar114.com/Pic/", fileName);
                            petImage.Name = fileName;
                            petImage.Description = "asfasd";
                        }
                        newDog.PetImages.Add(petImage);
                    }
                }

                newDog = _db.Pet.Add(newDog);
                _db.SaveChanges();

            }
            catch (Exception e)
            {
                logger.Error("An error occurred while UploadDog. Error: " + e);
            }
            finally
            {
                _db.Dispose();
            }

            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteImage(Guid id)
        {
            var _db = new SquareDanceDb();
            try
            {
                var image = _db.PetImage.Find(id);
                _db.PetImage.Remove(image);
                _db.SaveChanges();

                var path = Path.Combine(Server.MapPath("~/Pic/"), image.Name);

                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }

                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                logger.Error("An error occurred while DeleteImage. Error: " + e);

                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                _db.Dispose();
            }
        }

        public ActionResult Details()
        {
            var _db = new SquareDanceDb();
            var model = new SitterProfileModel();
            try
            {
                var userId = User.Identity.GetUserId();
                var details = _db.SitterProfile.Find(userId);
                model = new SitterProfileModel(details);
            }
            catch (Exception e)
            {
                logger.Error("An error occurred while Details. Error: " + e);
            }
            finally
            {
                _db.Dispose();
            }
            return View(model);
        }

        public ActionResult SaveDetails(SitterProfileModel model)
        {
            var _db = new SquareDanceDb();
            try
            {
                var userId = User.Identity.GetUserId();
                var details = _db.SitterProfile.Find(userId);
                if (details != null)
                {
                    details.LiveCondition = model.LiveCondition;
                    details.AnySmoker = model.AnySmoker;
                    details.HaveChildren = model.HaveChildren;
                    details.HaveCats = model.HaveCats;
                    details.CagedPets = model.CagedPets;
                    details.SittingFurniture = model.SittingFurniture;
                    details.DogExperience = model.DogExperience;
                    details.Describe = model.Describe;
                    details.DogCPR = model.DogCPR;
                    details.OralMedication = model.OralMedication;
                    details.InjectedMedication = model.InjectedMedication;
                    details.SeniorDogExperience = model.SeniorDogExperience;
                    details.ExericiseForHighEnergyDog = model.ExericiseForHighEnergyDog;
                    _db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                logger.Error("An error occurred while SaveDetails. Error: " + e);
            }
            finally
            {
                _db.Dispose();
            }
            return RedirectToAction("PetPreferences", "Account");
        }

        [HttpPost]
        public ActionResult _ItemImage(PetImageModel model)
        {
            return PartialView(model);
        }

        [HttpGet]
        public ActionResult PetDetail(Guid petId)
        {
            var model = new PetModel();

            try
            {
                var _db = new SquareDanceDb();
                var pet = _db.Pet.Find(petId);
                model = new PetModel(pet);
            }
            catch (Exception e)
            {
                logger.Error("An error occurred while PetDetail. Error: " + e);
            }

            return View(model);
        }

        public ActionResult _PetImagesDetail(Guid id)
        {
            var _db = new SquareDanceDb();
            var model = new PetModel();
            try
            {
                var pet = _db.Pet.Find(id);
                model = new PetModel(pet);
            }
            catch (Exception e)
            {
                logger.Error("An error occurred while AddImagesToPet. Error: " + e);
            }
            finally
            {
                _db.Dispose();
            }

            return PartialView(model);
        }

        [HttpPost]
        public JsonResult AddImagesToPet(Guid Id)
        {
            var _db = new SquareDanceDb();
            try
            {
                foreach (string file in Request.Files)
                {
                    var fileContent = Request.Files[file];
                    if (fileContent.ContentLength > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + fileContent.FileName;
                        var path = Path.Combine(Server.MapPath("~/Pic/"), fileName);
                        fileContent.SaveAs(path);
                        var petImage = new PetImage();
                        {
                            petImage.PetId = Id;
                            petImage.Path = Path.Combine("http://www.liar114.com/Pic/", fileName);
                            petImage.Name = fileName;
                            petImage.Description = "asfasd";
                        }
                        _db.PetImage.Add(petImage);
                    }
                }
                _db.SaveChanges();

                return Json(_PetImagesDetail(Id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                logger.Error("An error occurred while AddImagesToPet. Error: " + e);

                Response.StatusCode = (int)HttpStatusCode.BadRequest;

                return Json("错误", JsonRequestBehavior.AllowGet);
            }
            finally
            {
                _db.Dispose();   
            }
        }

        public ActionResult _EditPet(PetModel model)
        {
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult EditPet(PetModel model)
        {
            var _db = new SquareDanceDb();
            try
            {
                var pet = _db.Pet.Find(model.Id);
                var updatedPet = new Pet(model);
                _db.Entry(pet).CurrentValues.SetValues(updatedPet);
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                logger.Error("An error occurred while EditPet. Error: " + e);
            }
            finally
            {
                _db.Dispose();
            }
            return RedirectToAction("PetDetail", new { petId = model.Id });
        }

        [HttpPost]
        public ActionResult UpdateProfileImage()
        {
            var _db = new SquareDanceDb();

            try
            {
                foreach (string file in Request.Files)
                {
                    var fileContent = Request.Files[file];

                    if (fileContent.ContentLength > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + fileContent.FileName;
                        var path = Path.Combine(Server.MapPath("~/Pic/"), fileName);
                        fileContent.SaveAs(path);

                        var userId = User.Identity.GetUserId();
                        var user = _db.Users.Find(userId);
                        if (user != null)
                        {
                            var deletePath = user.ProfileImagePath;

                            user.ProfileImagePath = Path.Combine("http://www.liar114.com/Pic/", fileName);
                            _db.SaveChanges();

                            if (!string.IsNullOrEmpty(deletePath))
                            {
                                deletePath = Path.Combine(Server.MapPath($"~{deletePath}"));

                                if (System.IO.File.Exists(deletePath))
                                {
                                    System.IO.File.Delete(deletePath);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                logger.Error("An error occurred while UpdateProfileImage. Error: " + e);
            }
            finally
            {
                _db.Dispose();
            }

            return RedirectToAction("UserProfile");
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("Register", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}