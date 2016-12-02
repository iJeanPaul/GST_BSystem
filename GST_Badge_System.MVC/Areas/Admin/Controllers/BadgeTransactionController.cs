using GST_Badge_System.DAO;
using GST_Badge_System.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GST_Badge_System.MVC.Areas.Admin.Controllers
{
    public class BadgeTransactionController : Controller
    {
        // GET: Admin/BadgeTransaction
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoadTransactionsData()
        {
            BadgeTransactionDAO dtdao = new BadgeTransactionDAO();
            var transactions = dtdao.getAllBadgeTransactions();

            return Json(new { data = transactions }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Create()
        {
            List<SelectListItem> listItems_users = new List<SelectListItem>();
            UserDAO userdao = new UserDAO();
            var users = userdao.list();
            foreach (User user in users)
            {
                listItems_users.Add(new SelectListItem() { Value = user.User_Name, Text = user.User_Email});
            }
            ViewBag.usersListItems = new SelectList(listItems_users, "Text", "Value");

            BadgeDAO badgedao = new BadgeDAO();
            List<SelectListItem> listItems_badges = new List<SelectListItem>();
            var badges = badgedao.list();
            foreach (Badge badge in badges)
            {
                listItems_badges.Add(new SelectListItem() { Value = badge.Badge_Name, Text = badge.Badge_Name });
            }
            ViewBag.badgesListItems = new SelectList(listItems_badges, "Text", "Value");

            return View();
        }

        [HttpPost]
        public ActionResult Create(string selectedUserID, string selectedBadgeID, string comment)
        {
            BadgeTransactionDAO bt = new BadgeTransactionDAO();

            // TODO: we need to use the currently logged user instead of the hardcoded Andy
            bt.addBadgeTransaction("andy.harbert@oc.edu", selectedUserID, selectedBadgeID, comment);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int ID)
        {
            BadgeTransactionDAO dtdao = new BadgeTransactionDAO();
            BadgeTransaction currentTransaction = dtdao.getBTGivenId(ID);

            List<SelectListItem> listItems_users = new List<SelectListItem>();
            UserDAO userdao = new UserDAO();
            var users = userdao.list();
            foreach (User user in users)
            {
                listItems_users.Add(new SelectListItem() { Value = user.User_Name, Text = user.User_Email });
            }

            User sender = userdao.findPersonGivenId(currentTransaction.Reciever);
            ViewBag.usersListItems = new SelectList(listItems_users, "Text", "Value", sender.User_Email);

            BadgeDAO badgedao = new BadgeDAO();
            List<SelectListItem> listItems_badges = new List<SelectListItem>();
            var badges = badgedao.list();
            foreach (Badge badge in badges)
            {
                // TODO: We need to make the field we set the text to, since there could be badges with the same name!!!
                listItems_badges.Add(new SelectListItem() { Value = badge.Badge_Name, Text = badge.Badge_Name });
            }

            Badge current_badge = badgedao.findBadgeGivenId(currentTransaction.Badge_Id);
            ViewBag.badgesListItems = new SelectList(listItems_badges, "Text", "Value", current_badge.Badge_Name);

            return View(currentTransaction);
        }

        [HttpPost]
        public ActionResult Edit(int Id, string selectedUserID, string selectedBadgeID, string comment)
        {
            BadgeTransactionDAO btdao = new BadgeTransactionDAO();
            btdao.updateBadgeTransaction(Id, selectedUserID, selectedBadgeID, comment);
            return RedirectToAction("Index");
        }

        public ActionResult Details(int ID)
        {
            BadgeTransactionDAO btdao = new BadgeTransactionDAO();
            return View(btdao.getBTGivenId(ID));
        }

        public ActionResult Delete(int ID)
        {
            BadgeTransactionDAO btdao = new BadgeTransactionDAO();
            return View(btdao.getBTGivenId(ID));
        }

        [HttpPost]
        public ActionResult Delete(int Id, BadgeTransaction bt)
        {
            BadgeTransactionDAO btdao = new BadgeTransactionDAO();
            btdao.deleteBadgeTransaction(Id);
            return RedirectToAction("Index");
        }
    }
}