using SquareDanceASP.DBModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SquareDanceASP.Models
{
    public class SitterModel : UserProfileModel
    {
        public SitterModel() { }
        public SitterModel(Sitter sitter)
        {
            UserId = sitter.UserId;
            Name = sitter.User.Name;
            Address = sitter.User.Address;
            Phone = sitter.User.PhoneNumber;
            WeChat = sitter.User.WeChat;
            Years = sitter.Years;
            Latitude = sitter.User.Location.Latitude;
            Longitude = sitter.User.Location.Longitude;
            EmergencyContactName = sitter.EmergencyContactName;
            EmergencyContactPhoneNumber = sitter.EmergencyContactPhoneNumber;
        }

        public string Message { get; set; }
        public string EmergencyContactName { get; set; }
        public string EmergencyContactPhoneNumber { get; set; }
        public int Years { get; set; }
    }

    public class SitterDetailModel
    {
        public SitterDetailModel()
        {
            Sitter = new SitterModel();
            PetImages = new List<PetImageModel>();
        }

        public SitterDetailModel(Sitter sitter)
        {
            Sitter = new SitterModel();
            PetImages = new List<PetImageModel>();

            this.Sitter = new SitterModel(sitter);

            if (sitter.User.Pets != null)
            {
                foreach (var pet in sitter.User.Pets)
                {
                    if (pet.PetImages != null)
                    {
                        foreach (var petImage in pet.PetImages)
                        {
                            var petImageModel = new PetImageModel(petImage);
                            this.PetImages.Add(petImageModel);
                        }
                    }
                }
            }
        }

        public SitterModel Sitter { get; set; }
        public ServiceModel Service { get; set; }
        public RateModel Rate { get; set; }
        public ServiceOptionsModel ServiceOptions { get; set; }
        public PetPreferencesModel PetPreferences { get; set; }
        public SitterProfileModel SitterProfile { get; set; }
        public List<PetImageModel> PetImages { get; set; }
    }

    public class ServiceModel
    {
        public ServiceModel() { }
        public ServiceModel(ServiceAndRate dbModel)
        {
            UserId = dbModel.UserId;
            DogBoarding = dbModel.DogBoarding;
            HouseSitting = dbModel.HouseSitting;
            DropInVisits = dbModel.DropInVisits;
            DogWalking = dbModel.DogWalking;
            DoggyDayCare = dbModel.DoggyDayCare;
        }

        public string UserId { get; set; }
        public bool DogBoarding { get; set; }
        public bool HouseSitting { get; set; }
        public bool DropInVisits { get; set; }
        public bool DogWalking { get; set; }
        public bool DoggyDayCare { get; set; }
    }

    public class RateModel
    {
        public RateModel() { }
        public RateModel(ServiceAndRate dbModel)
        {
            UserId = dbModel.UserId;
            DogBoardingFee = dbModel.DogBoardingFee;
            DoggyDayCareFee = dbModel.DoggyDayCareFee;
            DogWalkingFee = dbModel.DogWalkingFee;
            DropInVisitsFee = dbModel.DropInVisitsFee;
            HouseSittingFee = dbModel.HouseSittingFee;
        }

        public string UserId { get; set; }

        [Required(ErrorMessage = "不能为空。")]
        [Range(0, int.MaxValue, ErrorMessage = "别太贪心哦!")]
        public double DogBoardingFee { get; set; }

        [Required(ErrorMessage = "不能为空。")]
        [Range(0, int.MaxValue, ErrorMessage = "别太贪心哦!")]
        public double HouseSittingFee { get; set; }

        [Required(ErrorMessage = "不能为空。")]
        [Range(0, int.MaxValue, ErrorMessage = "别太贪心哦!")]
        public double DropInVisitsFee { get; set; }

        [Required(ErrorMessage = "不能为空。")]
        [Range(0, int.MaxValue, ErrorMessage = "别太贪心哦!")]
        public double DogWalkingFee { get; set; }

        [Required(ErrorMessage = "不能为空。")]
        [Range(0, int.MaxValue, ErrorMessage = "别太贪心哦!")]
        public double DoggyDayCareFee { get; set; }
    }

    public class ServiceOptionsModel
    {
        public ServiceOptionsModel() { }
        public ServiceOptionsModel(ServiceAndRate dbModel)
        {
            UserId = dbModel.UserId;
            DayCareDogs = dbModel.DayCareDogs;
            FullTimeWeek = dbModel.FullTimeWeek;
            PottyBreaks = dbModel.PottyBreaks;
            Flexible = dbModel.Flexible;
            DropInVisits = dbModel.DropInVisits;
            DogWalkingTime = dbModel.DogWalkingTime;
        }

        public string UserId { get; set; }
        public bool DropInVisits { get; set; }
        public bool DoggyDayCare { get; set; }
        public int DayCareDogs { get; set; }
        public bool FullTimeWeek { get; set; }
        public string PottyBreaks { get; set; }
        public bool Flexible { get; set; }
        public string DogWalkingTime { get; set; }
    }

    public class PetPreferencesModel
    {
        public PetPreferencesModel() { }
        public PetPreferencesModel(ServiceAndRate dbModel)
        {
            UserId = dbModel.UserId;
            BoardingSmallDog = dbModel.BoardingSmallDog;
            BoardingMediumDog = dbModel.BoardingMediumDog;
            BoardingLargeDog = dbModel.BoardingLargeDog;
            BoardingGiantDog = dbModel.BoardingGiantDog;
            BoardingUnderOne = dbModel.BoardingUnderOne;
            HostDifferentFamily = dbModel.HostDifferentFamily;
            HostMaleNotNeutered = dbModel.HostMaleNotNeutered;
            HostFemaleNotSpayed = dbModel.HostFemaleNotSpayed;
            HostNeedCrateTrained = dbModel.HostNeedCrateTrained;
            HouseSmallDog = dbModel.HouseSmallDog;
            HouseMediumDog = dbModel.HouseMediumDog;
            HouseLargeDog = dbModel.HouseLargeDog;
            HouseGiantDog = dbModel.HouseGiantDog;
            HouseUnderOne = dbModel.HouseUnderOne;
        }
        public string UserId { get; set; }
        public bool BoardingSmallDog { get; set; }
        public bool BoardingMediumDog { get; set; }
        public bool BoardingLargeDog { get; set; }
        public bool BoardingGiantDog { get; set; }
        public bool BoardingUnderOne { get; set; }
        public bool HostDifferentFamily { get; set; }
        public bool HostMaleNotNeutered { get; set; }
        public bool HostFemaleNotSpayed { get; set; }
        public bool HostNeedCrateTrained { get; set; }
        public bool HouseSmallDog { get; set; }
        public bool HouseMediumDog { get; set; }
        public bool HouseLargeDog { get; set; }
        public bool HouseGiantDog { get; set; }
        public bool HouseUnderOne { get; set; }
    }

    public class SitterImageModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }
    }

    public class SitterProfileModel
    {
        public SitterProfileModel() { }
        public SitterProfileModel(SitterProfile dbModel)
        {
            //details
            LiveCondition = dbModel.LiveCondition;
            AnySmoker = dbModel.AnySmoker;
            HaveChildren = dbModel.HaveChildren;
            HaveCats = dbModel.HaveCats;
            CagedPets = dbModel.CagedPets;
            SittingFurniture = dbModel.SittingFurniture;
            DogExperience = dbModel.DogExperience;
            Describe = dbModel.Describe;
            DogCPR = dbModel.DogCPR;
            OralMedication = dbModel.OralMedication;
            InjectedMedication = dbModel.InjectedMedication;
            SeniorDogExperience = dbModel.SeniorDogExperience;
            ExericiseForHighEnergyDog = dbModel.ExericiseForHighEnergyDog;
        }
        public string LiveCondition { get; set; }
        public bool AnySmoker { get; set; }
        public bool HaveChildren { get; set; }
        public bool HaveCats { get; set; }
        public bool CagedPets { get; set; }
        public bool SittingFurniture { get; set; }
        public bool SittingBed { get; set; }
        public int DogExperience { get; set; }
        public string Describe { get; set; }
        public bool DogCPR { get; set; }
        public bool OralMedication { get; set; }
        public bool InjectedMedication { get; set; }
        public bool SeniorDogExperience { get; set; }
        public bool ExericiseForHighEnergyDog { get; set; }
    }

    public class SitterSearchModel
    {
        public SitterSearchModel()
        {
            Sitters = new List<SitterModel>();
        }
        public List<SitterModel> Sitters { get; set; }
        public double? AddressLatitude { get; set; }
        public double? AddressLongitude { get; set; }
    }
}