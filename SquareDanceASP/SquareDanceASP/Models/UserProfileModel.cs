using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SquareDanceASP.Models
{
    public class UserProfileModel
    {
        public UserProfileModel()
        {
            Pets = new List<PetModel>();
        }

        public UserProfileModel(ApplicationUser user)
        {
            Pets = new List<PetModel>();

            Address = user.Address;
            Name = user.Name;
            UserId = user.Id;
            Phone = user.PhoneNumber;
            ConnectEmail = user.Email;
            ProfileImagePath = user.ProfileImagePath;
            WeChat = user.WeChat;

            if (user.Pets != null && user.Pets.Count() > 0)
            {
                foreach (var pet in user.Pets)
                {
                    var petModel = new PetModel(pet);

                    Pets.Add(petModel);
                }
            }
        }
        public string UserId { get; set; }
        [Display(Name = "名字")]
        [Required(ErrorMessage = "名字不能为空。")]
        public string Name { get; set; }
        [Display(Name = "地址")]
        [Required(ErrorMessage = "地址不能为空。")]
        public string Address { get; set; }
        [Display(Name = "电话")]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "电话不能为空。")]
        public string Phone { get; set; }
        [Display(Name = "邮箱")]
        [EmailAddress(ErrorMessage = "不是正确的邮箱地址")]
        public string ConnectEmail { get; set; }
        [Display(Name = "网址")]
        public string WebAddress { get; set; }
        [Display(Name = "头像")]
        public string ProfileImagePath { get; set; }
        [Display(Name = "微信")]
        public string WeChat { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public bool Sitter { get; set; }
        public List<PetModel> Pets { get; set; }
    }

    public class UserListModel
    {
        public UserListModel()
        {
            Users = new List<UserProfileModel>();
        }
        public List<UserProfileModel> Users { get; set; }
    }
}