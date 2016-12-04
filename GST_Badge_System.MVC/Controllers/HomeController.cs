using GST_Badge_System.DAO;
using GST_Badge_System.Model;
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
    }
}