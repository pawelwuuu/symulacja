using Market.buyingRules;
using NUnit.Framework;

namespace Market.Tests
{
    [TestFixture]
    public class DefaultBuyingRulesTests
    {
        private DefaultBuyingRules _buyingRules;

        [SetUp]
        public void SetUp()
        {
            _buyingRules = new DefaultBuyingRules();
        }

        [Test]
        [TestCase(100, 120, 200, ExpectedResult = true)]  // Produkt tańszy niż 80% budżetu i poniżej średniej ceny
        [TestCase(180, 150, 200, ExpectedResult = false)] // Produkt przekracza budżet
        [TestCase(100, 80, 120, ExpectedResult = false)]  // Produkt droższy niż średnia cena
        [TestCase(160, 200, 250, ExpectedResult = true)]  // Produkt tańszy niż 80% budżetu i średnia cena
        public bool CanBuyLuxury_ShouldReturnCorrectResult(double productPrice, double avgProductPrice, double budget)
        {
            return _buyingRules.CanBuyLuxury(productPrice, avgProductPrice, budget);
        }

        [Test]
        [TestCase(100, 105, 150, ExpectedResult = true)]   // Produkt tańszy niż budżet i 1.1 * średnia cena
        [TestCase(200, 150, 180, ExpectedResult = false)] // Produkt przekracza budżet
        [TestCase(120, 100, 130, ExpectedResult = false)] // Produkt przekracza 1.1 * średnią cenę
        [TestCase(90, 100, 150, ExpectedResult = true)]   // Produkt tańszy niż budżet i 1.1 * średnia cena
        public bool CanBuyEssential_ShouldReturnCorrectResult(double productPrice, double avgProductPrice, double budget)
        {
            return _buyingRules.CanBuyEssential(productPrice, avgProductPrice, budget);
        }
    }
}