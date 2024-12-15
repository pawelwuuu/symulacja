using NUnit.Framework;
using Market.entities.marginCalculation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Market.Tests
{
    [TestFixture]
    public class DefaultMarginCalculatorTests
    {
        private DefaultMarginCalculator _marginCalculator;

        [SetUp]
        public void Setup()
        {
            _marginCalculator = new DefaultMarginCalculator();
        }

        [Test]
        public void CalculateMargin_NotEnoughSalesHistory_ReturnsCurrentMargin()
        {
            // Historia sprzedaży zawiera tylko dwie rundy, więc marża powinna zostać zachowana.
            var salesHistory = new Dictionary<int, List<double>>
            {
                { 1, new List<double> { 100 } },
                { 2, new List<double> { 150 } }
            };

            var currentMargin = 0.2;
            var currentTurn = 3;

            var result = _marginCalculator.CalculateMargin(salesHistory, currentMargin, currentTurn);
            
            Assert.That(result, Is.EqualTo(currentMargin));
        }

        [Test]
        public void CalculateMargin_LastRoundHigherThanAverage_IncreasesMargin()
        {

            var salesHistory = new Dictionary<int, List<double>>
            {
                { 1, new List<double> { 100 } },
                { 2, new List<double> { 150 } },
                { 3, new List<double> { 130 } },
                { 4, new List<double> { 180 } }
            };

            var currentMargin = 0.2;
            var currentTurn = 4;

            var result = _marginCalculator.CalculateMargin(salesHistory, currentMargin, currentTurn);

            // Ponieważ sprzedaż w rundzie 4 (180) jest wyższa niż średnia z rundy 3 i 2 (średnia to 135),
            // marża powinna zostać zwiększona o 0.05.
            Assert.That(result, Is.EqualTo(0.25));
        }

        [Test]
        public void CalculateMargin_LastRoundLowerThanAverage_DecreasesMargin()
        {
            // Historia sprzedaży z 4 rundami, ostatnia sprzedaż niższa niż średnia z dwóch poprzednich rund.
            var salesHistory = new Dictionary<int, List<double>>
            {
                { 1, new List<double> { 100 } },
                { 2, new List<double> { 150 } },
                { 3, new List<double> { 90 } },
                { 4, new List<double> { 80 } }
            };

            var currentMargin = 0.2;
            var currentTurn = 4;

            var result = _marginCalculator.CalculateMargin(salesHistory, currentMargin, currentTurn);
            
            Assert.That(result, Is.EqualTo(0.18).Within(0.001));
        }

        [Test]
        public void CalculateMargin_MarginExceedsUpperLimit_LimitsMarginTo0_5()
        {
            var salesHistory = new Dictionary<int, List<double>>
            {
                { 1, new List<double> { 100 } },
                { 2, new List<double> { 150 } },
                { 3, new List<double> { 170 } },
                { 4, new List<double> { 250 } }
            };

            var currentMargin = 0.49;
            var currentTurn = 4;

            var result = _marginCalculator.CalculateMargin(salesHistory, currentMargin, currentTurn);
            
            Assert.That(result, Is.EqualTo(0.5).Within(0.001));
        }

        [Test]
        public void CalculateMargin_MarginBelowLowerLimit_LimitsMarginTo0_1()
        {
            // Historia sprzedaży z 4 rundami, sprzedaż w ostatniej rundzie powoduje zmniejszenie marży poniżej 0.1.
            var salesHistory = new Dictionary<int, List<double>>
            {
                { 1, new List<double> { 100 } },
                { 2, new List<double> { 150 } },
                { 3, new List<double> { 120 } },
                { 4, new List<double> { 90 } }
            };

            var currentMargin = 0.1; // Marża na dolnej granicy
            var currentTurn = 4;

            var result = _marginCalculator.CalculateMargin(salesHistory, currentMargin, currentTurn);

            // Marża nie powinna spaść poniżej 0.1
            Assert.That(result, Is.EqualTo(0.1));
        }
    }
}
