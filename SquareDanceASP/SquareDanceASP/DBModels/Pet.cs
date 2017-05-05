using SquareDanceASP.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SquareDanceASP.DBModels
{
    [Table("Pet")]
    public class Pet
    {
        public Pet()
        {

        }
        public Pet(PetModel model)
        {
            PetImages = new List<PetImage>();

            Id = model.Id;
            UserId = model.UserId;
            Name = model.Name;
            Sex = model.Sex;
            Description = model.Description;
            Breed = model.Breed;
            Weight = model.Weight;
            Years = model.Years;
            Months = model.Months;
            Spayed = model.Spayed;
            Microchipped = model.Microchipped;
            WellDogs = model.WellDogs;
            WellDogDetail = model.WellDogDetail;
            WellCats = model.WellCats;
            WellCatDetail = model.WellCatDetail;
            WellChild = model.WellChild;
            WellChildDetail = model.WellChildDetail;
            HouseTrained = model.HouseTrained;
            HouseTrainedDetail = model.HouseTrainedDetail;
            SpecialRequirement = model.SpecialRequirement;
        }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Breed { get; set; }
        public decimal Weight { get; set; }
        public int Years { get; set; }
        public int Months { get; set; }
        public string Sex { get; set; }
        public bool Spayed { get; set; }
        public bool Microchipped { get; set; }
        public bool WellDogs { get; set; }
        public string WellDogDetail { get; set; }
        public bool WellCats { get; set; }
        public string WellCatDetail { get; set; }
        public bool WellChild { get; set; }
        public string WellChildDetail { get; set; }
        public bool HouseTrained { get; set; }
        public string HouseTrainedDetail { get; set; }
        public string SpecialRequirement { get; set; }
    
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<PetImage> PetImages { get; set; }
    }
}