using System;
using System.Net.Mail;

namespace GST_Badge_System.Model
{
    public class MailHelper
    {
        public static void SendBadgeNotification(string sender, string receiver)    // add badge as a param
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress(sender);
                mail.To.Add(receiver);
                mail.Subject = "Test Mail - 1";
                mail.Body = "Mail with attachment";

                //System.Net.Mail.Attachment attachment;
                //string uriPath = "https://assets.goodstatic.com/s3/magazine/others/meta/goodlogosquare.png";
                //string localPath = new Uri(uriPath).LocalPath;
                //attachment = new System.Net.Mail.Attachment(localPath);
                //mail.Attachments.Add(attachment);

                
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Port = 585;
                SmtpServer.Credentials = new System.Net.NetworkCredential(sender, "anonym0u$1");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);

                Console.WriteLine("The message was sent!!!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
