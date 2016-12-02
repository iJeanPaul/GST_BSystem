using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GST_Badge_System.DAO;

namespace GST_Badge_System.Test
{
    [TestClass]
    public class TestBadgeGiveTypeDAO
    {
        [TestMethod]
        public void TestRetrieveBadgeGiveTypes()
        {
            int expected = 4;

            BadgeGiveTypeDAO bgtdao = new BadgeGiveTypeDAO();
            int actual = bgtdao.retrieveBadgeGiveTypes().Count;

            Assert.AreEqual(expected, actual);
        }
    }
}
