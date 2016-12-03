using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;
using GST_Badge_System.Model;

namespace GST_Badge_System.MVC.Helpers
{
    public class EmailSender
    {
        SmtpClient client;
        MailAddress from;
        public EmailSender()
        {
            client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;

            // TODO: We need to use a differnt email here, maybe!!
            var credential = new NetworkCredential
            {
                UserName = "eamail",  // replace with valid value
                Password = "password"  // replace with valid value
            };
            client.Credentials = credential;
            from = new MailAddress("email", "GST Notifier");
        }

        /*
         * This method will send a notification email after a badge transaction has been created
         */
        public void sendEmail(string badgeDirection, string sender, string reciever, Badge badge, string comment) 
        {
            MailAddress to;
            MailMessage message;

            string messageBody = "";
            if (badgeDirection.Equals("sending"))
            {
                to = new MailAddress(sender);
                message = new MailMessage(from, to);

                messageBody = "<p>Dear {0},<br><br>You just sent a badge through GST system. See Details below:<br><br>" +
                    "<p>Receiver: {1}<br>Badge Type: {2}<br>Badge Description: {3}<br>Comment: {4}<br><img src='{5}'/><br></p>Thanks.<br></p>";
                message.Body = string.Format(messageBody, sender, reciever, badge.Badge_Name, badge.Badge_Descript, comment, badge.Badge_Image);
                message.Subject = string.Format("Notification: Badge Sent to {0}", reciever);
            }
            else
            {
                to = new MailAddress(reciever);
                message = new MailMessage(from, to);
                messageBody = "<p>Dear {0},<br><br>You just recieved a badge through GST system. See Details below:<br><br>" +
                    "<p>Sender: {1}<br>Badge Type: {2}<br>Badge Description: {3}<br>Comment: {4}<br><img src='{5}'/><br></p>Thanks.<br></p>";
                message.Body = string.Format(messageBody, reciever, sender, badge.Badge_Name, badge.Badge_Descript, comment, badge.Badge_Image);
                message.Subject = string.Format("Notification: Badge Received from {0}", sender);
            }
            message.IsBodyHtml = true;
            try
            {
                client.Send(message);
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }
    }
}