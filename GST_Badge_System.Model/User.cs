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
        public string User_Name { get; set; }
        public string User_Email { get; set; }
        public string User_Type { get; set; }
        public List<BadgeTransaction> sentBadges { get; set; }
        public List<BadgeTransaction> receivedBadges { get; set; }

        public int totalReceived { get; set; }
        public int totalSent { get; set; }
    }
}
