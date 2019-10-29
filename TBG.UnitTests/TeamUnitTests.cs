using System;
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
        public void TestValidateTeam_EmptyTeam()
        {
            //Arrange
            TeamController teamController = new TeamController();
            DatabaseProvider databaseProvider = new DatabaseProvider();
            ITeam thisTeam = new Team();
            ITeam existingTeam = databaseProvider.getTeam(thisTeam.TeamName);

            //Act
            bool valid = teamController.validateTeam(thisPerson, thatPerson);

            //Assert
            Assert.IsFalse(valid);
        }
    }
}
