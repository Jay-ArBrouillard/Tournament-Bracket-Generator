using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TBG.Business;
using TBG.Business.Controllers;
using TBG.Core.Interfaces;

namespace TBG.UnitTests
{
    [TestClass]
    public class PrizeUnitTests
    {
        [TestMethod]
        public void TestValidatePrize_DefaultInput()
        {
            //Arrange
            PrizeController prizeController = new PrizeController();
            String name = "Champion, runner-up, etc...";
            String inPercentage = "ex) .25, .5, .33, etc...";

            //Act
            IPrize valid = prizeController.ValidatePrize(name, inPercentage);

            //Assert
            Assert.IsNull(valid);
        }

        [TestMethod]
        public void TestValidatePrize_EmptyInput()
        {
            //Arrange
            PrizeController prizeController = new PrizeController();

            //Act
            IPrize valid = prizeController.ValidatePrize("", "");

            //Assert
            Assert.IsNull(valid);
        }

        [TestMethod]
        public void TestValidatePrize_NullInput()
        {
            //Arrange
            PrizeController prizeController = new PrizeController();
            String name = null;
            String inPercentage = null;

            //Act
            IPrize valid = prizeController.ValidatePrize(name, inPercentage);

            //Assert
            Assert.IsNull(valid);
        }

        [TestMethod]
        public void TestValidatePrize_PrizeNameWithNumbers()
        {
            //Arrange
            PrizeController prizeController = new PrizeController();
            String name = "TestPrizeName123456789";
            String inPercentage = "12%";

            //Act
            IPrize valid = prizeController.ValidatePrize(name, inPercentage);

            //Assert
            Assert.IsNotNull(valid);
        }

        [TestMethod]
        public void TestValidatePrize_PrizeNameWithNumbersAndSpaces()
        {
            //Arrange
            PrizeController prizeController = new PrizeController();
            String name = "Test Prize Name 123456789";
            String inPercentage = "12%";

            //Act
            IPrize valid = prizeController.ValidatePrize(name, inPercentage);

            //Assert
            Assert.IsNotNull(valid);
        }

        [TestMethod]
        public void TestValidatePrize_Numbers_Spaces_Symbols()
        {
            //Arrange
            PrizeController prizeController = new PrizeController();
            String name = "Test-Prize-Name 123456789 &%$#@.";
            String inPercentage = "12%";

            //Act
            IPrize valid = prizeController.ValidatePrize(name, inPercentage);

            //Assert
            Assert.IsNotNull(valid);
        }

        [TestMethod]
        public void TestValidatePrize_NoPercentSignInteger()
        {
            //Arrange
            PrizeController prizeController = new PrizeController();
            String name = "Test-Prize-Name";
            String inPercentage = "10";

            //Act
            IPrize valid = prizeController.ValidatePrize(name, inPercentage);

            //Assert
            Assert.IsNotNull(valid);
        }

        [TestMethod]
        public void TestValidatePrize_NoPercentSignDouble()
        {
            //Arrange
            PrizeController prizeController = new PrizeController();
            String name = "Test-Prize-Name";
            String inPercentage = "10.99";

            //Act
            IPrize valid = prizeController.ValidatePrize(name, inPercentage);

            //Assert
            Assert.IsNotNull(valid);
        }

        [TestMethod]
        public void TestValidatePrize_NoPercentSignDoubleAndSpaces()
        {
            //Arrange
            PrizeController prizeController = new PrizeController();
            String name = "Test-Prize-Name";
            String inPercentage = "10.99 ";

            //Act
            IPrize valid = prizeController.ValidatePrize(name, inPercentage);

            //Assert
            Assert.IsNotNull(valid);
        }

        [TestMethod]
        public void TestValidatePrize_PercentWithCharacters()
        {
            //Arrange
            PrizeController prizeController = new PrizeController();
            String name = "Test-Prize-Name";
            String inPercentage = "12xxxx%";

            //Act
            IPrize valid = prizeController.ValidatePrize(name, inPercentage);

            //Assert
            Assert.IsNull(valid);
        }

        [TestMethod]
        public void TestValidatePrize_IntegerPercent()
        {
            //Arrange
            PrizeController prizeController = new PrizeController();
            String name = "Test-Prize-Name";
            String inPercentage = "12%";

            //Act
            IPrize valid = prizeController.ValidatePrize(name, inPercentage);

            //Assert
            Assert.IsNotNull(valid);
        }

        [TestMethod]
        public void TestValidatePrize_IntegerPercentAbove100()  //Should we allow over 100%?
        {
            //Arrange
            PrizeController prizeController = new PrizeController();
            String name = "Test-Prize-Name";
            String inPercentage = "120%";

            //Act
            IPrize valid = prizeController.ValidatePrize(name, inPercentage);

            //Assert
            Assert.IsNull(valid);
        }

        [TestMethod]
        public void TestValidatePrize_DoublePercent()
        {
            //Arrange
            PrizeController prizeController = new PrizeController();
            String name = "Test-Prize-Name";
            String inPercentage = "12.55%";

            //Act
            IPrize valid = prizeController.ValidatePrize(name, inPercentage);

            //Assert
            Assert.IsNotNull(valid);
        }

        [TestMethod]
        public void TestValidatePrize_DoublePercentAbove100() //Should we allow over 100% Input?
        {
            //Arrange
            PrizeController prizeController = new PrizeController();
            String name = "Test-Prize-Name";
            String inPercentage = "120.55%";

            //Act
            IPrize valid = prizeController.ValidatePrize(name, inPercentage);

            //Assert
            Assert.IsNull(valid);
        }
    }
}
