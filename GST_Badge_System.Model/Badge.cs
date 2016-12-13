using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GST_Badge_System.Model
{
    public class Badge
    {
        [Key]
        public int Badge_Id { get; set; }

        [Required]
        public string Badge_Name { get; set; }

        [Required]
        public string Badge_Descript { get; set; }
        public string Badge_Notes { get; set; }
        public DateTime Badge_ActivateDate { get; set; }
        public DateTime Badge_RetireDate { get; set; }
        public string Badge_Image { get; set; }

        public BadgeStatus Badge_Status { get; set; }
        public BadgeType BadgeType_Object { get; set; }
        public BadgeGiveType BadgeGiveType_Object { get; set; }

        public int BadgeType { get; set; }
        public int BadgeGiveType { get; set; }

        public int BadgeStatus { get; set; }
    }
}
