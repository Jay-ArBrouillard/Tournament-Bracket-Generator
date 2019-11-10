using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TBG.Business;
using TBG.Core.Interfaces;
using TBG.Data.Classes;
using TBG.Data.Entities;

namespace TBG.UnitTests
{
    [TestClass]
    public class LoginControllerTests
    {
        [TestMethod]
        public void TestValidateLogin_ValidUser()
        {
            //Arrange
            LoginController loginController = new LoginController();
            DatabaseProvider databaseProvider = new DatabaseProvider();
            IUser thisUser = new User()
            {
                UserName = "Username",
                Password = "Password"
            };
            IUser thatUser = databaseProvider.getUser("Username");

            //Act
            bool valid = loginController.validateLogin(thisUser, thatUser);

            //Assert
            Assert.IsTrue(valid);
        }

        [TestMethod]
        public void TestValidateLogin_InValidUser()
        {
            //Arrange
            LoginController loginController = new LoginController();
            DatabaseProvider databaseProvider = new DatabaseProvider();
            IUser thisUser = new User()
            {
                UserName = "Username",
                Password = "Password"
            };
            IUser thatUser = databaseProvider.getUser("-1");

            //Act
            bool valid = loginController.validateLogin(thisUser, thatUser);

            //Assert
            Assert.IsFalse(valid);
        }

        [TestMethod]
        public void TestValidateRegister_UserExists()
        {
            //Arrange
            LoginController loginController = new LoginController();
            DatabaseProvider databaseProvider = new DatabaseProvider();
            IUser thisUser = new User()
            {
                UserName = "Username",
                Password = "Password"
            };
            IUser thatUser = databaseProvider.getUser("Username");

            //Act
            bool valid = loginController.validateRegister(thisUser, thatUser);

            //Assert
            Assert.IsFalse(valid);
        }

        [TestMethod]
        public void TestValidateRegister_ValidUser()
        {
            RemoveUser();

            //Arrange
            LoginController loginController = new LoginController();
            DatabaseProvider databaseProvider = new DatabaseProvider();
            IUser thisUser = new User()
            {
                UserName = "TestUser123456789",
                Password = "Password123456789"
            };
            IUser thatUser = databaseProvider.getUser("TestUser123456789");

            //Act
            bool valid = loginController.validateRegister(thisUser, thatUser);

            //Assert
            Assert.IsTrue(valid);

            RemoveUser();
        }

        [TestMethod]
        public void TestValidateRegister_EmptyUser()
        {
            RemoveUser();

            //Arrange
            LoginController loginController = new LoginController();
            DatabaseProvider databaseProvider = new DatabaseProvider();
            IUser thisUser = new User()
            {
                UserName = "",
                Password = "Password123456789"
            };
            IUser thatUser = databaseProvider.getUser("TestUser123456789");

            //Act
            bool valid = loginController.validateRegister(thisUser, thatUser);

            //Assert
            Assert.IsFalse(valid);
        }

        [TestMethod]
        public void TestValidateRegister_NullUser()
        {
            RemoveUser();

            //Arrange
            LoginController loginController = new LoginController();
            DatabaseProvider databaseProvider = new DatabaseProvider();
            IUser thisUser = new User()
            {
                Password = "Password123456789"
            };
            IUser thatUser = databaseProvider.getUser("TestUser123456789");

            //Act
            bool valid = loginController.validateRegister(thisUser, thatUser);

            //Assert
            Assert.IsFalse(valid);
        }

        private void RemoveUser()
        {
            DatabaseProvider databaseProvider = new DatabaseProvider();
            IUser thisUser = databaseProvider.getUser("TestUser123456789");

            if (thisUser != null)
            {
                databaseProvider.deleteUser(thisUser);
            }
        }
    }
}
