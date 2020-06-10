using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using Stocks.BuisnessLayer;
using Stocks.Shared.TestHelpers.Interface;

namespace Stocks.BusinessLayer.Tests
{
    [TestFixture]
    public class StockManagerTests
    {
        private StockManager _stockManager;
        private Mock<IFile> _fileMock;

        [SetUp]
        public void Setup()
        {
            _stockManager = new StockManager();
            _fileMock = new Mock<IFile>();

            _stockManager.SetIFile(_fileMock.Object);

            _fileMock.Setup(s => s.WriteAllText(It.IsAny<string>(), It.IsAny<string>())).Callback<string, string>(
                (path, json) => _fileMock.Setup(s => s.ReadAllText(path)).Returns(json));
        }

        [Test]
        public void ItemsHaveASellInValue()
        {
            // Arrange
            _fileMock.Setup(s => s.ReadAllText("DataFiles\\ItemsInStock.json")).Returns("[{\"Id\":0,\"Name\":\"Aged Brie\",\"QualityModifier\":1}]");
            _fileMock.Setup(s => s.ReadAllText("DataFiles\\Stocks.json")).Returns(string.Empty);
            _stockManager.ParseInput(new List<string> { "Aged Brie, 2, 1" });
            var stockItem = _stockManager.GetStocks().First();

            // Act
            var sellIn = (stockItem.SellByDates - new DateTime(2020, 06, 10)).Value.Days;

            // Assert
            Assert.That(sellIn, Is.EqualTo(2));
        }

        [Test]
        public void ItemsHaveAQualityValue()
        {
            // Arrange
            _fileMock.Setup(s => s.ReadAllText("DataFiles\\ItemsInStock.json")).Returns("[{\"Id\":0,\"Name\":\"Aged Brie\",\"QualityModifier\":1}]");
            _fileMock.Setup(s => s.ReadAllText("DataFiles\\Stocks.json")).Returns(string.Empty);

            _stockManager.ParseInput(new List<string> { "Aged Brie, 2, 1" });

            // Act
            var stockItem = _stockManager.GetStocks().First(); 

            // Assert
            Assert.That(stockItem.Quality, Is.EqualTo(1));
        }

        [Test]
        public void ItemsGoDownInQualityAfterDay()
        {
            // Arrange
            _fileMock.Setup(s => s.ReadAllText("DataFiles\\ChangingQualityModifiers.json")).Returns("");
            _fileMock.Setup(s => s.ReadAllText("DataFiles\\ItemsInStock.json")).Returns("[{\"Id\":0,\"Name\":\"Frozen Items\",\"QualityModifier\":-1}]");
            _fileMock.Setup(s => s.ReadAllText("DataFiles\\Stocks.json")).Returns(string.Empty);

            _stockManager.ParseInput(new List<string> { "Frozen Items, 2, 3" });

            var previousQuality = _stockManager.GetStocks().First().Quality;

            // Act
            _stockManager.ProgressDay();
            var currentQuality = _stockManager.GetStocks().First().Quality;

            // Assert
            Assert.That(currentQuality, Is.EqualTo(previousQuality - 1));
        }

        [Test]
        public void ItemsGoDownTwiceAsFastAfterTheirExpriyDate()
        {
            // Arrange
            _fileMock.Setup(s => s.ReadAllText("DataFiles\\ChangingQualityModifiers.json")).Returns("");
            _fileMock.Setup(s => s.ReadAllText("DataFiles\\ItemsInStock.json")).Returns("[{\"Id\":0,\"Name\":\"Frozen Items\",\"QualityModifier\":-1}]");
            _fileMock.Setup(s => s.ReadAllText("DataFiles\\Stocks.json")).Returns(string.Empty);

            _stockManager.ParseInput(new List<string> { "Frozen Items, -1, 3" });

            var previousQuality = _stockManager.GetStocks().First().Quality;

            // Act
            _stockManager.ProgressDay();
            var currentQuality = _stockManager.GetStocks().First().Quality;

            // Assert
            Assert.That(currentQuality, Is.EqualTo(previousQuality - 2));
        }

        [Test]
        public void QualityIsNeverNegative()
        {
            // Arrange
            _fileMock.Setup(s => s.ReadAllText("DataFiles\\ChangingQualityModifiers.json")).Returns("");
            _fileMock.Setup(s => s.ReadAllText("DataFiles\\ItemsInStock.json")).Returns("[{\"Id\":0,\"Name\":\"Aged Brie\",\"QualityModifier\":-1}]");
            _fileMock.Setup(s => s.ReadAllText("DataFiles\\Stocks.json")).Returns(string.Empty);

            _stockManager.ParseInput(new List<string> { "Aged Brie, 2, -20" });

            var previousQuality = _stockManager.GetStocks().First().Quality;

            // Act
            _stockManager.ProgressDay();
            var currentQuality = _stockManager.GetStocks().First().Quality;

            // Assert
            Assert.That(currentQuality, Is.EqualTo(0));
        }

        [Test]
        public void QualityIsNeverOverFifty()
        {
            // Arrange
            _fileMock.Setup(s => s.ReadAllText("DataFiles\\ChangingQualityModifiers.json")).Returns("");
            _fileMock.Setup(s => s.ReadAllText("DataFiles\\ItemsInStock.json")).Returns("[{\"Id\":0,\"Name\":\"Aged Brie\",\"QualityModifier\":-1}]");
            _fileMock.Setup(s => s.ReadAllText("DataFiles\\Stocks.json")).Returns(string.Empty);

            _stockManager.ParseInput(new List<string> { "Aged Brie, 2, 55" });

            var previousQuality = _stockManager.GetStocks().First().Quality;

            // Act
            _stockManager.ProgressDay();
            var currentQuality = _stockManager.GetStocks().First().Quality;

            // Assert
            Assert.That(currentQuality, Is.EqualTo(50));
        }

        [Test]
        public void AgedBrieIncreaseInQualityTheOlderItGets()
        {
            // Arrange
            _fileMock.Setup(s => s.ReadAllText("DataFiles\\ChangingQualityModifiers.json")).Returns("");
            _fileMock.Setup(s => s.ReadAllText("DataFiles\\ItemsInStock.json")).Returns("[{\"Id\":0,\"Name\":\"Aged Brie\",\"QualityModifier\":1}]");
            _fileMock.Setup(s => s.ReadAllText("DataFiles\\Stocks.json")).Returns(string.Empty);

            _stockManager.ParseInput(new List<string> { "Aged Brie, 2, 1" });

            var previousQuality = _stockManager.GetStocks().First().Quality;

            // Act
            _stockManager.ProgressDay();
            var currentQuality = _stockManager.GetStocks().First().Quality;

            // Assert
            Assert.That(currentQuality, Is.GreaterThan(previousQuality));
        }

        [Test]
        public void FrozenItemsDecreaseQualityByOne()
        {
            // Arrange
            _fileMock.Setup(s => s.ReadAllText("DataFiles\\ChangingQualityModifiers.json")).Returns("");
            _fileMock.Setup(s => s.ReadAllText("DataFiles\\ItemsInStock.json")).Returns("[{\"Id\":0,\"Name\":\"Frozen Items\",\"QualityModifier\":-1}]");
            _fileMock.Setup(s => s.ReadAllText("DataFiles\\Stocks.json")).Returns(string.Empty);

            _stockManager.ParseInput(new List<string> { "Frozen Items, 2, 3" });

            var previousQuality = _stockManager.GetStocks().First().Quality;

            // Act
            _stockManager.ProgressDay();
            var currentQuality = _stockManager.GetStocks().First().Quality;

            // Assert
            Assert.That(currentQuality, Is.EqualTo(previousQuality - 1));
        }

        [Test]
        public void FreshItemsDecreaseTwiceAsFastAsFrozen()
        {
            // Arrange
            _fileMock.Setup(s => s.ReadAllText("DataFiles\\ChangingQualityModifiers.json")).Returns("");
            _fileMock.Setup(s => s.ReadAllText("DataFiles\\ItemsInStock.json")).Returns("[{\"Id\":0,\"Name\":\"Fresh Items\",\"QualityModifier\":-2}]");
            _fileMock.Setup(s => s.ReadAllText("DataFiles\\Stocks.json")).Returns(string.Empty);

            _stockManager.ParseInput(new List<string> { "Fresh Items, 2, 3" });

            var previousQuality = _stockManager.GetStocks().First().Quality;

            // Act
            _stockManager.ProgressDay();
            var currentQuality = _stockManager.GetStocks().First().Quality;

            // Assert
            Assert.That(currentQuality, Is.EqualTo(previousQuality - 2));
        }

        [Test]
        public void SoapHasNoSellByDateOrDecreaseInQuality()
        {
            // Arrange
            _fileMock.Setup(s => s.ReadAllText("DataFiles\\ChangingQualityModifiers.json")).Returns("");
            _fileMock.Setup(s => s.ReadAllText("DataFiles\\ItemsInStock.json")).Returns("[{\"Id\":0,\"Name\":\"Soap\",\"QualityModifier\":0}]");
            _fileMock.Setup(s => s.ReadAllText("DataFiles\\Stocks.json")).Returns(string.Empty);

            _stockManager.ParseInput(new List<string> { "Soap, 2, 1" });

            var previousQuality = _stockManager.GetStocks().First().Quality;

            // Act
            _stockManager.ProgressDay();
            var currentQuality = _stockManager.GetStocks().First().Quality;

            // Assert
            Assert.That(currentQuality, Is.EqualTo(previousQuality));
        }

        [Test]
        public void ChristmasCrackersIncreaseAsTheSellByGetsCloser()
        {
            // Arrange
            _fileMock.Setup(s => s.ReadAllText("DataFiles\\ChangingQualityModifiers.json")).Returns("");
            _fileMock.Setup(s => s.ReadAllText("DataFiles\\ItemsInStock.json")).Returns("[{\"Id\":0,\"Name\":\"Christmas Crackers\",\"QualityModifier\":-1}]");
            _fileMock.Setup(s => s.ReadAllText("DataFiles\\Stocks.json")).Returns(string.Empty);

            _stockManager.ParseInput(new List<string> { "Christmas Crackers, 2, 1" });

            var previousQuality = _stockManager.GetStocks().First().Quality;

            // Act
            _stockManager.ProgressDay();
            var currentQuality = _stockManager.GetStocks().First().Quality;

            // Assert
            Assert.That(currentQuality, Is.EqualTo(previousQuality - 1));
        }
    }
}
