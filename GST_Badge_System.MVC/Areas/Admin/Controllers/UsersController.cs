using GST_Badge_System.DAO;
using GST_Badge_System.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GST_Badge_System.MVC.Areas.Admin.Controllers
{
    public class UsersController : Controller
    {
        // GET: Admin/Users
        public ActionResult Index()
        {
            string user_role = "";
            if (User.Identity.IsAuthenticated)
            {
                var test = User.Identity.Name;
                // We need to know who this is, so that we can determin which role this user has
                User user = new UserDAO()[User.Identity.Name];
                user_role = user.User_Type;
                ViewBag.username = user.User_Name;
            }
            ViewBag.role = user_role;

            return View();
        }

        public ActionResult LoadUsersData()
        {
            UserDAO usersdao = new UserDAO();
            List<User> users = usersdao.getDetailedList();
            return Json(new { data = users }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int ID)
        {
            UserDAO usersdao = new UserDAO();
            User user = usersdao.findPersonGivenId(ID);
            return View(user);
        }

        [HttpPost]
        public ActionResult Edit (User user)
        {
            UserDAO usersdao = new UserDAO();
            usersdao.updateUser(user);
            return RedirectToAction("Index");
        }

        public ActionResult Details (int ID)
        {
            /*
            UserDAO usersdao = new UserDAO();
            User user = usersdao.findPersonGivenId(ID);
            return View(user);
            */
            return RedirectToAction("Tree");
        }

        public ActionResult Delete (int ID)
        {
            UserDAO usersdao = new UserDAO();
            User user = usersdao.findPersonGivenId(ID);
            return View(user);
        }

        [HttpPost]
        public ActionResult Delete(int Id, User user)
        {
            UserDAO usersdao = new UserDAO();
            usersdao.deleteRecord(Id, user);
            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(User user)
        {
            UserDAO usersdao = new UserDAO();
            usersdao.addUser(user);
            return RedirectToAction("Index");
        }

        public ActionResult Tree()
        {
            return View();
        }

        public ActionResult Reports()
        {
            return View();
        }
    }
}