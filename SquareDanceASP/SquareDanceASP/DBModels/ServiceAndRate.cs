using SquareDanceASP.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SquareDanceASP.DBModels
{
    [Table("ServiceAndRate")]
    public class ServiceAndRate
    {
        public ServiceAndRate() { }
        public ServiceAndRate(ServiceModel model)
        {
            UserId = model.UserId;
            DogBoarding = model.DogBoarding;
            HouseSitting = model.HouseSitting;
            DropInVisits = model.DropInVisits;
            DogWalking = model.DogWalking;
            DoggyDayCare = model.DoggyDayCare;
        }
        [Key]
        public string UserId { get; set; }
        public bool DogBoarding { get; set; }
        public double DogBoardingFee { get; set; }
        public bool HouseSitting { get; set; }
        public double HouseSittingFee { get; set; }
        public bool DropInVisits { get; set; }
        public double DropInVisitsFee { get; set; }
        public bool DogWalking { get; set; }
        public double DogWalkingFee { get; set; }
        public bool DoggyDayCare { get; set; }
        public double DoggyDayCareFee { get; set; }
        public int DayCareDogs { get; set; }
        public bool FullTimeWeek { get; set; }
        public string PottyBreaks { get; set; }
        public bool Flexible { get; set; }
        public string DogWalkingTime { get; set; }
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

        [ForeignKey("UserId")]
        public virtual Sitter Sitter { get; set; }
    }
}