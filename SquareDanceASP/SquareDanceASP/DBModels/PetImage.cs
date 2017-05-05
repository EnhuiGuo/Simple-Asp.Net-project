using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SquareDanceASP.DBModels
{
    [Table("PetImage")]
    public class PetImage
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid PetId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }

        [ForeignKey("PetId")]
        public virtual Pet Pet { get; set; }
    }
}