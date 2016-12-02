using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GST_Badge_System.DAO;

namespace GST_Badge_System.Test
{
    [TestClass]
    public class TestBadgeStatusDAO
    {
        [TestMethod]
        public void TestRetrieveBadgeTypes()
        {
            BadgeStatusDAO bsdao = new BadgeStatusDAO();
            int expected = 2;
            int actual = bsdao.retrieveBadgeTypes().Count;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestAddBadgeStatus()
        {
            BadgeStatusDAO bsdao = new BadgeStatusDAO();
            bsdao.addBadgeStatus(new Model.BadgeStatus { BS_Name = "Name", BS_Descript = "Testing" });

            int expected = 3;
            int actual = bsdao.retrieveBadgeTypes().Count;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestDeleteBadgeStatus()
        {
            BadgeStatusDAO bsdao = new BadgeStatusDAO();
            bsdao.deleteBadgeStatus("Name");

            int expected = 2;
            int actual = bsdao.retrieveBadgeTypes().Count;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestUpdateBadgeStatus()
        {
            BadgeStatusDAO bsdao = new BadgeStatusDAO();
            bsdao.updateBadgeStatus(new Model.BadgeStatus { BS_Name = "Active", BS_Descript = "This badge is currently active and can be used" }, "Active");

            string expected = "Active";
            string actual = bsdao.retrieveBadgeTypes()[0].BS_Name;

            Assert.AreEqual(expected, actual);
        }
    }
}
