using GST_Badge_System.DAO;
using GST_Badge_System.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GST_Badge_System.MVC.Areas.Admin.Controllers
{
    public class BadgesController : Controller
    {
        // GET: Admin/Badges
        public ActionResult Index()
        {
            string user_role = "";
            if (User.Identity.IsAuthenticated)
            {
                var test = User.Identity.Name;
                // We need to know who this is, so that we can determine which role this user has
                User user = new UserDAO()[User.Identity.Name];
                user_role = user.User_Type;
                ViewBag.username = user.User_Name;
            }
            ViewBag.role = user_role;

            // TODO: get a list of all badges in the badge bank
            // TODO: We need to this only when the logged in user is an admin
            BadgeDAO badgedao = new BadgeDAO();
            List<Badge> badges = badgedao.list();
            UserDAO userdao = new UserDAO();
            List<User> users = userdao.list();
            return View();
        }

       public ActionResult LoadBadgesData()
        {
            BadgeDAO badgedao = new BadgeDAO();
            List<Badge> badges = badgedao.list();

            return Json(new { data = badges }, JsonRequestBehavior.AllowGet);
        }

        // GET: Edit
        public ActionResult Edit (int ID)
        {
            // get the current badge
            BadgeDAO badgedao = new BadgeDAO();
            Model.Badge badgeToEdit = badgedao.findBadgeGivenId(ID);
            List<SelectListItem>[] result = new List<SelectListItem>[3];
            this.getDropdownValues(result);
            ViewBag.badgesTypeListItems = new SelectList(result[0], "Text", "Value", badgeToEdit.BadgeType);
            ViewBag.badgesGiveTypeListItems = new SelectList(result[1], "Text", "Value", badgeToEdit.BadgeGiveType);
            ViewBag.statusListItems = new SelectList(result[2], "Text", "Value", badgeToEdit.BadgeStatus);
            return View(badgeToEdit);
        }

        [HttpPost]
        public ActionResult Edit(Badge badge, string selectedBadgeType, string selectedBadgeGiveType, string selectedStatus)
        {
            if (ModelState.IsValid)
            {
                badge.BadgeType = Convert.ToInt32(selectedBadgeType);
                badge.BadgeGiveType = Convert.ToInt32(selectedBadgeGiveType);
                badge.BadgeStatus = Convert.ToInt32(selectedStatus);
                BadgeDAO badgedao = new BadgeDAO();
                badgedao.update(badge);

                return RedirectToAction("Index");
            }
            else
            {   
                // get the current badge
                BadgeDAO badgedao = new BadgeDAO();
                Model.Badge badgeToEdit = badgedao.findBadgeGivenId(badge.Badge_Id);
                List<SelectListItem>[] result = new List<SelectListItem>[3];
                this.getDropdownValues(result);
                ViewBag.badgesTypeListItems = new SelectList(result[0], "Text", "Value", badgeToEdit.BadgeType);
                ViewBag.badgesGiveTypeListItems = new SelectList(result[1], "Text", "Value", badgeToEdit.BadgeGiveType);
                ViewBag.statusListItems = new SelectList(result[2], "Text", "Value", badgeToEdit.BadgeStatus);
                return View(badgeToEdit);
            }
        }

        [HttpGet]
        public ActionResult Details(int Id)
        {
            BadgeDAO badgedao = new BadgeDAO();
            Badge badgeForDetails = badgedao.findBadgeGivenId(Id);
            return View(badgeForDetails);
        }

        [HttpGet]
        public ActionResult Delete(int Id)
        {
            BadgeDAO badgedao = new BadgeDAO();
            Badge badgeToDelete = badgedao.findBadgeGivenId(Id);
            return View(badgeToDelete);
        }

        [HttpPost]
        public ActionResult Delete(int Id, Badge badge)
        {
            BadgeDAO badgedao = new BadgeDAO();
            badgedao.delete(Id,badge);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Create()
        {
            List<SelectListItem>[] result = new List<SelectListItem>[3];
            this.getDropdownValues(result);
            ViewBag.badgesTypeListItems = new SelectList(result[0], "Text", "Value");
            ViewBag.badgesGiveTypeListItems = new SelectList(result[1], "Text", "Value");
            ViewBag.statusListItems = new SelectList(result[2], "Text", "Value");

            return View();
        }

        [HttpPost]
        public ActionResult Create(Badge badge, string selectedBadgeType, string selectedBadgeGiveType, string selectedStatus)
        {
            // bool validSelection = true;

            // TODO: need to think about a better validation way for dropdowns!!!
            if (string.IsNullOrEmpty(selectedBadgeType))
            {
                selectedBadgeType = "3";
            }
            if (string.IsNullOrEmpty(selectedBadgeGiveType))
            {
                selectedBadgeGiveType = "4";
            }
            if (string.IsNullOrEmpty(selectedStatus))
            {
                selectedStatus = "1";
            }

            if (ModelState.IsValid)
            {
                badge.BadgeType = Convert.ToInt32(selectedBadgeType);
                badge.BadgeGiveType = Convert.ToInt32(selectedBadgeGiveType);
                badge.BadgeStatus = Convert.ToInt32(selectedStatus);
                BadgeDAO badgedao = new BadgeDAO();
                badgedao.create(badge);
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Create");
            }

        }

        public void getDropdownValues(List<SelectListItem>[] result)
        {
            
            // first element
            BadgeTypeDAO badgetypedao = new BadgeTypeDAO();
            List<SelectListItem> listItems_badgetypes = new List<SelectListItem>();
            var badgestypes = badgetypedao.retrieveBadgeTypes();
            foreach (BadgeType badgetype in badgestypes)
            {
                listItems_badgetypes.Add(new SelectListItem() { Value = badgetype.BT_Name, Text = badgetype.BT_Id.ToString() });
            }
            result[0] = listItems_badgetypes;

            // TODO: get a list of badge give type - we actually do not need this since will know who is sending to who
            BadgeGiveTypeDAO badgegivetypedao = new BadgeGiveTypeDAO();
            List<SelectListItem> listItems_badgegivetypes = new List<SelectListItem>();
            var badgegivetypes = badgegivetypedao.retrieveBadgeGiveTypes();
            foreach (BadgeGiveType bgt in badgegivetypes)
            {
                listItems_badgegivetypes.Add(new SelectListItem() { Value = bgt.BGT_Name, Text = bgt.BGT_Id.ToString() });
            }
            result[1] = listItems_badgegivetypes;

            // get alist of badge statuses
            BadgeStatusDAO badgestatusdao = new BadgeStatusDAO();
            List<SelectListItem> listItems_badgestatus = new List<SelectListItem>();
            var badgestatuses = badgestatusdao.retrieveBadgeTypes();
            foreach (BadgeStatus bs in badgestatuses)
            {
                listItems_badgestatus.Add(new SelectListItem() { Value = bs.BS_Name, Text = bs.BS_Id.ToString() });
            }
            result[2] = listItems_badgestatus;
        }
    }
}