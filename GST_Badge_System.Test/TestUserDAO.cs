using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GST_Badge_System.DAO;

namespace GST_Badge_System.Test
{
    [TestClass]
    public class TestUserDAO
    {
        [TestMethod]
        public void TestImportUsers()
        {
            // Arrange
            int expecited = 31;

            // Act
            UserDAO userdao = new UserDAO();
            int actual = userdao.importUsers().Count;

            // Assert
            Assert.AreEqual(expecited, actual);
        }

        [TestMethod]
        public void TestUploadUsers()
        {
            int expected = 1;

            UserDAO userdao = new UserDAO();
            int actual = userdao.uploadUsers();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestListUsers()
        {
            string expected = "Andy Harbert";

            UserDAO userdao = new UserDAO();
            string actual = userdao.list()[userdao.list().Count - 1].User_Name;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestRetrieveUserReceivedBadges()
        {
            string expected = "Olivier Tuyishime";

            UserDAO userdao = new UserDAO();
            string actual = userdao.retrieveUserReceivedBadges("1")[0].Sender_obj.User_Name;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestRetrieveUserSentBadges()
        {
            string expected = "Jesus Arredondo";

            UserDAO userdao = new UserDAO();
            string actual = userdao.retrieveUserSentBadges("58")[0].Receiver_obj.User_Name;

            Assert.AreEqual(expected, actual);
        }
    }
}
