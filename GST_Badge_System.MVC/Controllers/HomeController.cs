using GST_Badge_System.DAO;
using GST_Badge_System.Model;
using GST_Badge_System.MVC.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GST_Badge_System.MVC.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            string user_role = "";
            if (User.Identity.IsAuthenticated)
            {
                var test = User.Identity.Name;
                // We need to know who this is, so that we can determin which role this user has
                // User user = new UserDAO()[User.Identity.Name];
                User user = new UserDAO().getUserAndAllDetails(User.Identity.Name);
                user_role = user.User_Type;
                ViewBag.username = user.User_Name;
                ViewBag.user = user;
            }
            ViewBag.role = user_role;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public ActionResult SendBadge()
        {
            List<SelectListItem> listItems_users = new List<SelectListItem>();
            UserDAO userdao = new UserDAO();
            var users = userdao.list();
            foreach (User user in users)
            {
                listItems_users.Add(new SelectListItem() { Value = user.User_Name, Text = user.User_Email });
            }
            ViewBag.usersListItems = new SelectList(listItems_users, "Text", "Value");

            BadgeDAO badgedao = new BadgeDAO();
            List<SelectListItem> listItems_badges = new List<SelectListItem>();
            var badges = badgedao.list();
            foreach (Badge badge in badges)
            {
                listItems_badges.Add(new SelectListItem() { Value = badge.Badge_Name, Text = badge.Badge_Id.ToString() });
            }
            ViewBag.badgesListItems = new SelectList(listItems_badges, "Text", "Value");

            return View();
        }

        [HttpPost]
        public ActionResult SendBadge(string selectedUserID, string selectedBadgeID, string comment)
        {
            BadgeTransactionDAO bt = new BadgeTransactionDAO();

            try
            {
                // TODO: we need to use the currently logged user instead of the hardcoded Andy
                bt.addBadgeTransaction("jeanpaul.iradukunda@eagles.oc.edu", selectedUserID, Convert.ToInt32(selectedBadgeID), comment);
                string senderEmail = "jeanpaul.iradukunda@eagles.oc.edu";   // TODO: this does not have to be hardcoded
                var badgesender = new UserDAO()[senderEmail];
                string receiverEmail = selectedUserID;
                var badgeReceiver = new UserDAO()[receiverEmail];
                string bt_comment = comment;
                // var badge = new BadgeDAO()[selectedBadgeID];
                var badgedao = new BadgeDAO();
                var badge = badgedao.findBadgeGivenId(Convert.ToInt32(selectedBadgeID));

                EmailSender emailSender = new EmailSender();
                emailSender.sendEmail("sending", senderEmail, receiverEmail, badge, comment);
                emailSender.sendEmail("receiving", senderEmail, receiverEmail, badge, comment);
            }
            catch (Exception e)
            {

                throw new Exception(e.ToString());
            }
            return RedirectToAction("Index");
        }

        public ActionResult BadgeDetails(int ID)
        {
            BadgeTransaction bt = new BadgeTransactionDAO().getBTGivenId(ID);
            return View(bt);
        }

    }
}