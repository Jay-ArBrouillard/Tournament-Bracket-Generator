using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TBG.Business;
using TBG.Core.Interfaces;
using TBG.Data.Classes;

namespace TBG.UnitTests
{
    [TestClass]
    public class TeamUnitTests
    {
        [TestMethod]
        public void TestValidateTeam_NullTeam()
        {
            //Arrange
            TeamController teamController = new TeamController();
            DatabaseProvider databaseProvider = new DatabaseProvider();
            ITeam thisTeam = new Team();
            ITeam existingTeam = databaseProvider.getTeam(thisTeam.TeamName);

            //Act
            bool valid = teamController.validateTeam(thisTeam, existingTeam);

            //Assert
            Assert.IsFalse(valid);
        }

        [TestMethod]
        public void TestValidateTeam_ExistingTeam()
        {
            //Arrange
            TeamController teamController = new TeamController();
            DatabaseProvider databaseProvider = new DatabaseProvider();
            IPerson person = new Person();
            List<IPerson> persons = new List<IPerson>()
            {
                person
            };
            ITeam thisTeam = new Team()
            {
                TeamName = "Test Team",
                TeamMembers = persons
            };
            ITeam existingTeam = databaseProvider.getTeam(thisTeam.TeamName);

            //Act
            bool valid = teamController.validateTeam(thisTeam, existingTeam);

            //Assert
            Assert.IsFalse(valid);
        }

        [TestMethod]
        public void TestValidateTeam_NoPlayerTeam()
        {
            //Arrange
            TeamController teamController = new TeamController();
            DatabaseProvider databaseProvider = new DatabaseProvider();
            List<IPerson> persons = new List<IPerson>();
            ITeam thisTeam = new Team()
            {
                TeamName = "teamabc123xyz",
                TeamMembers = persons
            };
            ITeam existingTeam = databaseProvider.getTeam(thisTeam.TeamName);

            //Act
            bool valid = teamController.validateTeam(thisTeam, existingTeam);

            //Assert
            Assert.IsFalse(valid);
        }

        [TestMethod]
        public void TestValidateTeam_ValidTeam()
        {
            //Arrange
            TeamController teamController = new TeamController();
            DatabaseProvider databaseProvider = new DatabaseProvider();
            IPerson person = new Person();
            List<IPerson> persons = new List<IPerson>()
            {
                person
            };
            ITeam thisTeam = new Team()
            {
                TeamName = "teamabc123xyz",
                TeamMembers = persons
            };
            ITeam existingTeam = databaseProvider.getTeam(thisTeam.TeamName);

            //Act
            bool valid = teamController.validateTeam(thisTeam, existingTeam);

            //Assert
            Assert.IsTrue(valid);
        }
    }
}
