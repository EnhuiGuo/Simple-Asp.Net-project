using GoogleMaps.LocationServices;
using Microsoft.AspNet.Identity;
using SquareDanceASP.DBModels;
using SquareDanceASP.Models;
using System;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web.Mvc;

namespace SquareDanceASP.Controllers
{
    public class SitterController : Controller
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        // GET: Sitter
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search(string address)
        {
            var _db = new SquareDanceDb();
            var model = new SitterSearchModel();
            try
            {
                if (address != null)
                {
                    var point = new GoogleLocationService().GetLatLongFromAddress((!string.IsNullOrEmpty(address) && address != null) ? address : "");

                    if (point != null)
                    {
                        model.AddressLatitude = point.Latitude;
                        model.AddressLongitude = point.Longitude;

                        var coord = DbGeography.FromText($"POINT({point.Longitude} {point.Latitude})");

                        if (coord != null)
                        {
                            var sitters = _db.Sitter.OrderBy(x => x.User.Location.Distance(coord)).Take(20).ToList();

                            foreach (var sitter in sitters)
                            {
                                var sitterModel = new SitterModel(sitter);
                                model.Sitters.Add(sitterModel);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                logger.Error("An error occurred while Index. Error: " + e);
            }
            finally
            {
                _db.Dispose();
            }
            return View(model);
        }

        public ActionResult Detail(string sitterId)
        {
            var _db = new SquareDanceDb();
            try
            {
                var sitter = _db.Sitter.Find(sitterId);
                var serviceAndRate = _db.ServiceAndRate.Find(sitterId);
                var sitterProfile = _db.SitterProfile.Find(sitterId);
                var model = new SitterDetailModel(sitter);

                if (serviceAndRate != null)
                {
                    model.Rate = new RateModel(serviceAndRate);
                    model.Service = new ServiceModel(serviceAndRate);
                    model.ServiceOptions = new ServiceOptionsModel(serviceAndRate);
                    model.PetPreferences = new PetPreferencesModel(serviceAndRate);

                }

                if (sitterProfile != null)
                {
                    model.SitterProfile = new SitterProfileModel(sitterProfile);
                }

                return View(model);
            }
            catch (Exception e)
            {
                logger.Error("An error occurred while Detail. Error: " + e);

                return View("Search");
            }
            finally
            {
                _db.Dispose();
            }
        }

        public ActionResult Services()
        {
            var _db = new SquareDanceDb();
            var model = new ServiceModel();
            try
            {
                model.UserId = User.Identity.GetUserId();
                var services = _db.ServiceAndRate.Find(model.UserId);
                if (services != null)
                {
                    model = new ServiceModel(services);
                }
            }
            catch (Exception e)
            {
                logger.Error("An error occurred while Services. Error: " + e);
            }
            finally
            {
                _db.Dispose();
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Services(ServiceModel model)
        {
            if (ModelState.IsValid)
            {
                var _db = new SquareDanceDb();
                try
                {
                    var sitter = _db.Sitter.Find(model.UserId);
                    if (sitter == null)
                    {
                        var newSitter = new Sitter();
                        {
                            newSitter.UserId = model.UserId;
                        }
                        _db.Sitter.Add(newSitter);
                        _db.SaveChanges();
                    }

                    var services = _db.ServiceAndRate.Find(model.UserId);
                    if (services == null)
                    {
                        var newServices = new ServiceAndRate(model);
                        _db.ServiceAndRate.Add(newServices);
                        _db.SaveChanges();
                    }
                    else
                    {
                        services.DogBoarding = model.DogBoarding;
                        services.HouseSitting = model.HouseSitting;
                        services.DropInVisits = model.DropInVisits;
                        services.DogWalking = model.DogWalking;
                        services.DoggyDayCare = model.DoggyDayCare;
                        _db.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    logger.Error("An error occurred while SaveServices. Error: " + e);
                }
                finally
                {
                    _db.Dispose();
                }

                return RedirectToAction("Rates", "Sitter");
            }
            return View(model);
        }

        public ActionResult Rates()
        {
            var _db = new SquareDanceDb();
            var model = new RateModel();
            try
            {
                var userId = User.Identity.GetUserId();
                var rates = _db.ServiceAndRate.Find(userId);
                if (rates != null)
                {
                    model = new RateModel(rates);
                }
            }
            catch (Exception e)
            {
                logger.Error("An error occurred while Rates. Error: " + e);
            }
            finally
            {
                _db.Dispose();
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Rates(RateModel model)
        {
            if (ModelState.IsValid)
            {
                var _db = new SquareDanceDb();
                try
                {
                    var userId = User.Identity.GetUserId();
                    var rates = _db.ServiceAndRate.Find(userId);
                    if (rates != null)
                    {
                        rates.DogBoardingFee = model.DogBoardingFee;
                        rates.DoggyDayCareFee = model.DoggyDayCareFee;
                        rates.DogWalkingFee = model.DogWalkingFee;
                        rates.DropInVisitsFee = model.DropInVisitsFee;
                        rates.HouseSittingFee = model.HouseSittingFee;
                        _db.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    logger.Error("An error occurred while SaveRates. Error: " + e);
                }
                finally
                {
                    _db.Dispose();
                }

                return RedirectToAction("ServiceOptions", "Sitter");
            }

            return View(model);
        }

        public ActionResult ServiceOptions()
        {
            var _db = new SquareDanceDb();
            var model = new ServiceOptionsModel();
            try
            {
                var userId = User.Identity.GetUserId();
                var serviceOptions = _db.ServiceAndRate.Find(userId);
                if (serviceOptions != null)
                {
                    model = new ServiceOptionsModel(serviceOptions);
                }
            }
            catch (Exception e)
            {
                logger.Error("An error occurred while ServiceOptions. Error: " + e);
            }
            finally
            {
                _db.Dispose();
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult ServiceOptions(ServiceOptionsModel model)
        {
            if (ModelState.IsValid)
            {
                var _db = new SquareDanceDb();
                try
                {
                    var userId = User.Identity.GetUserId();
                    var serviceOptions = _db.ServiceAndRate.Find(userId);
                    if (serviceOptions != null)
                    {
                        serviceOptions.DayCareDogs = model.DayCareDogs;
                        serviceOptions.FullTimeWeek = model.FullTimeWeek;
                        serviceOptions.PottyBreaks = model.PottyBreaks;
                        serviceOptions.Flexible = model.Flexible;
                        serviceOptions.DropInVisits = model.DropInVisits;
                        serviceOptions.DogWalkingTime = model.DogWalkingTime;
                        _db.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    logger.Error("An error occurred while SaveServiceOption. Error: " + e);
                }
                finally
                {
                    _db.Dispose();
                }
                return RedirectToAction("PetPreferences", "Sitter");
            }

            return View(model);
        }

        public ActionResult PetPreferences()
        {
            var model = new PetPreferencesModel();
            var _db = new SquareDanceDb();
            try
            {
                var userId = User.Identity.GetUserId();
                var petPreferences = _db.ServiceAndRate.Find(userId);
                if (petPreferences != null)
                {
                    model = new PetPreferencesModel(petPreferences);
                }
            }
            catch (Exception e)
            {
                logger.Error("An error occurred while PetPreferences. Error: " + e);
            }
            finally
            {
                _db.Dispose();
            }
            return View(model);
        }

        public ActionResult SavePetPreferences(PetPreferencesModel model)
        {
            var _db = new SquareDanceDb();
            try
            {
                var userId = User.Identity.GetUserId();
                var petPreferences = _db.ServiceAndRate.Find(userId);
                if (petPreferences != null)
                {
                    petPreferences.BoardingSmallDog = model.BoardingSmallDog;
                    petPreferences.BoardingMediumDog = model.BoardingMediumDog;
                    petPreferences.BoardingLargeDog = model.BoardingLargeDog;
                    petPreferences.BoardingGiantDog = model.BoardingGiantDog;
                    petPreferences.BoardingUnderOne = model.BoardingUnderOne;
                    petPreferences.HostDifferentFamily = model.HostDifferentFamily;
                    petPreferences.HostMaleNotNeutered = model.HostMaleNotNeutered;
                    petPreferences.HostFemaleNotSpayed = model.HostFemaleNotSpayed;
                    petPreferences.HostNeedCrateTrained = model.HostNeedCrateTrained;
                    petPreferences.HouseSmallDog = model.HouseSmallDog;
                    petPreferences.HouseMediumDog = model.HouseMediumDog;
                    petPreferences.HouseLargeDog = model.HouseLargeDog;
                    petPreferences.HouseGiantDog = model.HouseGiantDog;
                    petPreferences.HouseUnderOne = model.HouseUnderOne;
                    _db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                logger.Error("An error occurred while SavePetPreferences. Error: " + e);
            }
            finally
            {
                _db.Dispose();
            }
            return RedirectToAction("UserProfile", "Account");
        }
    }
}