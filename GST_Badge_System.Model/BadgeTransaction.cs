using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GST_Badge_System.Model
{
    public class BadgeTransaction
    {
        // We don't really need these first two objects, i added them to get around
        // casting exceptions when i modified this file to work with the new getDetailedList, getUserReceivedBadges, getUserSentBadges
        // i  change their previous names (Sender and Reciever) because thay matched the database fields
        public User Sender_obj { get; set; }
        public User Receiver_obj { get; set; }

        public int Sender { get; set; }
        public int Reciever { get; set; }
        public int Badge_Id { get; set; }

        [Key]
        public int bt_id { get; set; }
        public User Sender_Object { get; set; }
        public User Receiver_Object { get; set; }

        public Badge Badge { get; set; }
        public string Badge_Comment { get; set; }
        public DateTime BTrans_Date { get; set; }
    }
}
