using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GST_Badge_System.Model
{
    public class User
    {
        [Key]
        public int User_Id { get; set; }

        [Required(ErrorMessage = "The user name is required")]
        [Display(Name = "User Full Name:")]
        public string User_Name { get; set; }

        [Display(Name = "Email Address:")]
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string User_Email { get; set; }

        [Required(ErrorMessage = "The type is required")]
        [Display(Name = "Type (stu or adm):")]
        public string User_Type { get; set; }
        public List<BadgeTransaction> sentBadges { get; set; }
        public List<BadgeTransaction> receivedBadges { get; set; }

        public int totalReceived { get; set; }
        public int totalSent { get; set; }
    }
}
