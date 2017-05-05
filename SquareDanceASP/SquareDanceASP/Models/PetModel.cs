using SquareDanceASP.DBModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SquareDanceASP.Models
{
    public class PetModel
    {
        public PetModel()
        {
            PetImages = new List<PetImageModel>();
        }
        public PetModel(Pet pet)
        {
            PetImages = new List<PetImageModel>();

            Id = pet.Id;
            UserId = pet.UserId;
            Name = pet.Name;
            Sex = pet.Sex;
            Description = pet.Description;
            Breed = pet.Breed;
            Weight = pet.Weight;
            Years = pet.Years;
            Months = pet.Months;
            Spayed = pet.Spayed;
            Microchipped = pet.Microchipped;
            WellDogs = pet.WellDogs;
            WellDogDetail = pet.WellDogDetail;
            WellChild = pet.WellChild;
            WellChildDetail = pet.WellChildDetail;
            WellCats = pet.WellCats;
            WellCatDetail = pet.WellCatDetail;
            HouseTrained = pet.HouseTrained;
            HouseTrainedDetail = pet.HouseTrainedDetail;
            SpecialRequirement = pet.SpecialRequirement;

            if (pet.PetImages != null && pet.PetImages.Count > 0)
            {
                foreach (var petImage in pet.PetImages)
                {
                    var petImageModel = new PetImageModel(petImage);
                    PetImages.Add(petImageModel);
                }
            }
        }
        public Guid Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Display(Name = "昵称")]
        [Required(ErrorMessage = "昵称不能为空。")]
        public string Name { get; set; }
    
        public string Description { get; set; }

        [Display(Name = "品种")]
        [Required(ErrorMessage = "品种不能为空")]
        public string Breed { get; set; }

        [Required]
        [Display(Name = "体重")]
        [Range(1, int.MaxValue, ErrorMessage = "体重至少大于{1}")]
        public decimal Weight { get; set; }

        [Required]
        [Display(Name = "年龄")]
        public int Years { get; set; }

        public int Months { get; set; }

        [Required]
        [Display(Name = "性别")]
        public string Sex { get; set; }
        [Display(Name = "被阉割了么？")]
        public bool Spayed { get; set; }
        [Display(Name = "植入晶片么？")]
        public bool Microchipped { get; set; }
        [Display(Name = "和别的狗狗能和平相处？")]
        public bool WellDogs { get; set; }
        public string WellDogDetail { get; set; }
        [Display(Name = "和猫咪能和平相处？")]
        public bool WellCats { get; set; }
        public string WellCatDetail { get; set; }
        [Display(Name = "和小孩能和平相处？")]
        public bool WellChild { get; set; }
        public string WellChildDetail { get; set; }
        [Display(Name = "受过家庭训练么？")]
        public bool HouseTrained { get; set; }
        public string HouseTrainedDetail { get; set; }
        [Display(Name = "特殊需求")]
        public string SpecialRequirement { get; set; }
        public List<PetImageModel> PetImages { get; set; }
    }
}