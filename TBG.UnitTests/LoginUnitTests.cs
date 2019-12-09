using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TBG.Business;
using TBG.Business.Controllers;
using TBG.Core.Interfaces;
using TBG.Data.Classes;
using TBG.Data.Entities;

namespace TBG.UnitTests
{
    [TestClass]
    public class LoginControllerTests
    {
        private const string LoginUser = "Username";
        private const string LoginPassword = "Password";
        private const string RegisterUser = "TestUser123456789";
        private const string RegisterPassword = "Password123456789";

        [TestMethod]
        public void TestValidateLogin_ValidUser()
        {
            //Arrange
            LoginController loginController = new LoginController();
            DatabaseProvider databaseProvider = new DatabaseProvider();

            IUser thatUser = databaseProvider.getUser("Username");

            //Act
            var valid = loginController.validateLogin(LoginUser, LoginPassword, thatUser);

            //Assert
            Assert.IsTrue(valid != null);
        }

        [TestMethod]
        public void TestValidateLogin_InValidUser()
        {
            //Arrange
            LoginController loginController = new LoginController();
            DatabaseProvider databaseProvider = new DatabaseProvider();
            IUser thatUser = databaseProvider.getUser("-1");

            //Act
            var valid = loginController.validateLogin(LoginUser, LoginPassword, thatUser);

            //Assert
            Assert.IsTrue(valid == null);
        }

        [TestMethod]
        public void TestValidateRegister_UserExists()
        {
            //Arrange
            LoginController loginController = new LoginController();
            DatabaseProvider databaseProvider = new DatabaseProvider();
            IUser thatUser = databaseProvider.getUser("Username");

            //Act
            var valid = loginController.validateRegister(LoginUser, LoginPassword, thatUser);

            //Assert
            Assert.IsTrue(valid == null);
        }

        [TestMethod]
        public void TestValidateRegister_ValidUser()
        {
            RemoveUser();

            //Arrange
            LoginController loginController = new LoginController();
            DatabaseProvider databaseProvider = new DatabaseProvider();

            IUser thatUser = databaseProvider.getUser(RegisterUser);

            //Act
            var valid = loginController.validateRegister(RegisterUser, RegisterPassword, thatUser);

            //Assert
            Assert.IsTrue(valid != null);

            RemoveUser();
        }

        [TestMethod]
        public void TestValidateRegister_EmptyUser()
        {
            RemoveUser();

            //Arrange
            LoginController loginController = new LoginController();
            DatabaseProvider databaseProvider = new DatabaseProvider();

            IUser thatUser = databaseProvider.getUser(RegisterUser);

            //Act
            var valid = loginController.validateRegister("", RegisterPassword, thatUser);

            //Assert
            Assert.IsTrue(valid == null);
        }

        private void RemoveUser()
        {
            DatabaseProvider databaseProvider = new DatabaseProvider();
            IUser thisUser = databaseProvider.getUser(RegisterUser);

            if (thisUser != null)
            {
                databaseProvider.deleteUser(thisUser);
            }
        }
    }
}
