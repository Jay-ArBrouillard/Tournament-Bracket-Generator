using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TBG.Business;
using TBG.Business.Controllers;
using TBG.Business.Models;
using TBG.Core.Interfaces;
using TBG.Data.Classes;

namespace TBG.UnitTests
{
    [TestClass]
    public class PersonUnitTests
    {
        #region VALIDATE PERSON
        [TestMethod]
        public void TestValidatePerson_EmptyPerson()
        {
            //Arrange
            PersonController personController = new PersonController();
            DatabaseProvider databaseProvider = new DatabaseProvider();
            IPerson thisPerson = new Person();
            IPerson thatPerson = databaseProvider.getPersonByUniqueIdentifiers(thisPerson.FirstName, thisPerson.LastName, thisPerson.Email);

            //Act
            bool valid = personController.validatePerson(thisPerson, thatPerson);

            //Assert
            Assert.IsFalse(valid);
        }

        [TestMethod]
        public void TestValidatePerson_EmptyFirstName()
        {
            //Arrange
            PersonController personController = new PersonController();
            DatabaseProvider databaseProvider = new DatabaseProvider();
            IPerson thisPerson = new Person()
            {
                LastName = "TestLastName",
                Email = "TestEmail@Email.com",
                Phone = "111-111-1111"
            };
            IPerson thatPerson = databaseProvider.getPersonByUniqueIdentifiers(thisPerson.FirstName, thisPerson.LastName, thisPerson.Email);

            //Act
            bool valid = personController.validatePerson(thisPerson, thatPerson);

            //Assert
            Assert.IsFalse(valid);
        }

        [TestMethod]
        public void TestValidatePerson_EmptyLastName()
        {
            //Arrange
            PersonController personController = new PersonController();
            DatabaseProvider databaseProvider = new DatabaseProvider();
            IPerson thisPerson = new Person()
            {
                FirstName = "TestFirstName",
                Email = "TestEmail@Email.com",
                Phone = "111-111-1111"
            };
            IPerson thatPerson = databaseProvider.getPersonByUniqueIdentifiers(thisPerson.FirstName, thisPerson.LastName, thisPerson.Email);

            //Act
            bool valid = personController.validatePerson(thisPerson, thatPerson);

            //Assert
            Assert.IsFalse(valid);
        }

        [TestMethod]
        public void TestValidatePerson_EmptyEmail()
        {
            //Arrange
            PersonController personController = new PersonController();
            DatabaseProvider databaseProvider = new DatabaseProvider();
            IPerson thisPerson = new Person()
            {
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                Phone = "111-111-1111"
            };
            IPerson thatPerson = databaseProvider.getPersonByUniqueIdentifiers(thisPerson.FirstName, thisPerson.LastName, thisPerson.Email);

            //Act
            bool valid = personController.validatePerson(thisPerson, thatPerson);

            //Assert
            Assert.IsFalse(valid);
        }

        [TestMethod]
        public void TestValidatePerson_EmptyPhone()
        {
            //Arrange
            PersonController personController = new PersonController();
            DatabaseProvider databaseProvider = new DatabaseProvider();
            IPerson thisPerson = new Person()
            {
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                Email = "TestEmail@Email.com"
            };
            IPerson thatPerson = databaseProvider.getPersonByUniqueIdentifiers(thisPerson.FirstName, thisPerson.LastName, thisPerson.Email);

            //Act
            bool valid = personController.validatePerson(thisPerson, thatPerson);

            //Assert
            Assert.IsFalse(valid);
        }

        [TestMethod]
        public void TestValidatePerson_PersonAlreadyExists()
        {
            //Arrange
            PersonController personController = new PersonController();
            IPerson thisPerson = new Person()
            {
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                Email = "TestEmail@Email.com"
            };

            IPerson thatPerson = new Person()
            {
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                Email = "TestEmail@Email.com"
            };

            //Act
            bool valid = personController.validatePerson(thisPerson, thatPerson);

            //Assert
            Assert.IsFalse(valid);
        }

        [TestMethod]
        public void TestValidatePerson_ValidPerson()
        {
            RemovePerson();

            //Arrange
            PersonController personController = new PersonController();
            DatabaseProvider databaseProvider = new DatabaseProvider();
            IPerson thisPerson = new Person()
            {
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                Email = "TestEmail@Email.com",
                Phone = "111-111-1111"
            };
            IPerson thatPerson = databaseProvider.getPersonByUniqueIdentifiers(thisPerson.FirstName, thisPerson.LastName, thisPerson.Email);

            //Act
            bool valid = personController.validatePerson(thisPerson, thatPerson);

            //Assert
            Assert.IsTrue(valid);

            RemovePerson();
        }

        private void RemovePerson()
        {
            DatabaseProvider databaseProvider = new DatabaseProvider();
            IPerson thisPerson = databaseProvider.getPersonByUniqueIdentifiers("TestFirstName", "TestLastName", "TestEmail@Email.com");

            if (thisPerson != null)
            {
                databaseProvider.deletePerson(thisPerson);
            }
        }
        #endregion

        #region VALIDATE EMAIL
        [TestMethod]
        public void TestValidateEmail_EmptyEmail()
        {
            //Arrange
            PersonController personController = new PersonController();

            //Act
            bool valid = personController.isValidEmail("");

            //Assert
            Assert.IsFalse(valid);
        }

        [TestMethod]
        public void TestValidateEmail_NullEmail()
        {
            //Arrange
            PersonController personController = new PersonController();
            String email = null;

            //Act
            bool valid = personController.isValidEmail(email);

            //Assert
            Assert.IsFalse(valid);
        }

        [TestMethod]
        public void TestValidateEmail_CorrectEmail()
        {
            //Arrange
            PersonController personController = new PersonController();
            String email = "Test@Test.com";

            //Act
            bool valid = personController.isValidEmail(email);

            //Assert
            Assert.IsTrue(valid);
        }
        #endregion

        #region VALIDATE PHONE NUMBER 
        [TestMethod]
        public void TestisValidPhoneNumber_MultipleCorrectNumbers()
        {
            //Arrange
            PersonController personController = new PersonController();
            String phone1 = "(111)1111111";
            String phone2 = "(111) 1111111";
            String phone3 = "(111)111-1111";
            String phone4 = "(111) 111-1111";
            String phone5 = "1111111111";
            String phone6 = "111-111-1111";

            //Act
            bool valid1 = personController.isValidPhoneNumber(phone1);
            bool valid2 = personController.isValidPhoneNumber(phone2);
            bool valid3 = personController.isValidPhoneNumber(phone3);
            bool valid4 = personController.isValidPhoneNumber(phone4);
            bool valid5 = personController.isValidPhoneNumber(phone5);
            bool valid6 = personController.isValidPhoneNumber(phone6);

            //Assert
            Assert.IsTrue(valid1);
            Assert.IsTrue(valid2);
            Assert.IsTrue(valid3);
            Assert.IsTrue(valid4);
            Assert.IsTrue(valid5);
            Assert.IsTrue(valid6);
        }

        [TestMethod]
        public void TestisValidPhoneNumber_MultipleIncorrectNumbers()
        {
            //Arrange
            PersonController personController = new PersonController();
            String phone1 = "(xxx)xxxxxxx";
            String phone2 = "(xxx) xxxxxxx";
            String phone3 = "(xxx)xxx-xxxx";
            String phone4 = "(xxx) xxx-xxxx";
            String phone5 = "xxxxxxxxxx";
            String phone6 = "xxx-xxx-xxxx";
            String phone7 = "";
            String phone8 = "123-1x2-1234";
            String phone9 = "123456789)";
            String phone10 = "(111) 1111 111";

            //Act
            bool valid1 = personController.isValidPhoneNumber(phone1);
            bool valid2 = personController.isValidPhoneNumber(phone2);
            bool valid3 = personController.isValidPhoneNumber(phone3);
            bool valid4 = personController.isValidPhoneNumber(phone4);
            bool valid5 = personController.isValidPhoneNumber(phone5);
            bool valid6 = personController.isValidPhoneNumber(phone6);
            bool valid7 = personController.isValidPhoneNumber(phone7);
            bool valid8 = personController.isValidPhoneNumber(phone8);
            bool valid9 = personController.isValidPhoneNumber(phone9);
            bool valid10 = personController.isValidPhoneNumber(phone10);

            //Assert
            Assert.IsFalse(valid1);
            Assert.IsFalse(valid2);
            Assert.IsFalse(valid3);
            Assert.IsFalse(valid4);
            Assert.IsFalse(valid5);
            Assert.IsFalse(valid6);
            Assert.IsFalse(valid7);
            Assert.IsFalse(valid8);
            Assert.IsFalse(valid9);
            Assert.IsFalse(valid10);
        }
        #endregion

        #region VALIDATE WIN LOSSES
        [TestMethod]
        public void TestValidateWinLoss_InvalidWins()
        {
            //Arrange
            PersonController personController = new PersonController();
            string wins = "x";
            string losses = "0";

            //Act
            bool valid = personController.validateWinLoss(wins, losses);

            //Assert
            Assert.IsFalse(valid);
        }

        [TestMethod]
        public void TestValidateWinLoss_InvalidLosses()
        {
            //Arrange
            PersonController personController = new PersonController();
            string wins = "0";
            string losses = "x";

            //Act
            bool valid = personController.validateWinLoss(wins, losses);

            //Assert
            Assert.IsFalse(valid);
        }

        [TestMethod]
        public void TestValidateWinLoss_validWinLosses()
        {
            //Arrange
            PersonController personController = new PersonController();
            string wins = "0";
            string losses = "0";

            //Act
            bool valid = personController.validateWinLoss(wins, losses);

            //Assert
            Assert.IsTrue(valid);
        }
        #endregion
    }
}
