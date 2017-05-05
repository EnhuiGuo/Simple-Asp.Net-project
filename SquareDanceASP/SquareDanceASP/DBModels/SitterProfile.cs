using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SquareDanceASP.DBModels
{
    [Table("SitterProfile")]
    public class SitterProfile
    {
        [Key]
        public string UserId { get; set; }
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
        public virtual Sitter Sitter { get; set; }
    }
}