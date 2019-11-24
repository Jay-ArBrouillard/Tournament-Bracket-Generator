using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TBG.Business;
using TBG.Business.Controllers;

namespace TBG.UnitTests
{
    /// <summary>
    /// Summary description for TournamentUnitTests
    /// </summary>
    [TestClass]
    public class TournamentUnitTests
    {
        [TestMethod]
        public void ValidateEntryFee_AlphabetInput()
        {
            //Arrange
            TournamentController tournamentController = new TournamentController();

            //Act
            bool valid = tournamentController.validateEntryFee("abc");

            //Assert
            Assert.IsFalse(valid);
        }

        [TestMethod]
        public void ValidateEntryFee_SymbolInput()
        {
            //Arrange
            TournamentController tournamentController = new TournamentController();

            //Act
            bool valid = tournamentController.validateEntryFee("!0");

            //Assert
            Assert.IsFalse(valid);
        }

        [TestMethod]
        public void ValidateEntryFee_ValidIntegerInput()
        {
            //Arrange
            TournamentController tournamentController = new TournamentController();

            //Act
            bool valid = tournamentController.validateEntryFee("100");

            //Assert
            Assert.IsTrue(valid);
        }


    }
}
