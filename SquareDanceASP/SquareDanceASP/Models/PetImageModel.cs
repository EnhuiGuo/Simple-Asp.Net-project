using SquareDanceASP.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SquareDanceASP.Models
{
    public class PetImageModel
    {
        public PetImageModel() { }
        public PetImageModel(PetImage petImage)
        {
            Id = petImage.Id;
            PetId = petImage.PetId;
            Name = petImage.Name;
            Description = petImage.Description;
            Path = petImage.Path;
        }
        public Guid Id { get; set; }
        public Guid PetId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }
        public string Data { get; set; }
    }
}