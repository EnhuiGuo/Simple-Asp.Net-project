using SquareDanceASP.DBModels;
using SquareDanceASP.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace SquareDanceASP.Controllers
{
    public class HomeController : Controller
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ActionResult Index()
        {
            UserListModel model = new UserListModel();
            var _db = new SquareDanceDb();

            try
            {
                var users = _db.Users.ToList();

                foreach (var user in users)
                {
                    var userModel = new UserProfileModel(user);

                    model.Users.Add(userModel);
                }
            }
            catch(Exception e)
            {
                logger.Error("An error occurred while Index. Error: " + e);
            }
            finally
            {
                _db.Dispose();
            }

            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}