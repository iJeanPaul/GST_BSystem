using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GST_Badge_System.DAO;

namespace GST_Badge_System.Test
{
    [TestClass]
    public class TestBadgeDAO
    {
        [TestMethod]
        public void TestImportBadges()
        {
            int expected = 15;

            DAO.BadgeDAO badgedao = new DAO.BadgeDAO();
            int actual = badgedao.ImportBadges(GetDirectory.getFilePath() + @"\GST_Badge_System.DAO\Data\Staff-Student.csv").Count;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestListBadges()
        {
            int expected = 46;

            DAO.BadgeDAO bdao = new DAO.BadgeDAO();
            int actual = bdao.list().Count;

            Assert.AreEqual(expected, actual);
        }
    }
}
