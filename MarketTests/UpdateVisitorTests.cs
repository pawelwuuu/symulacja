using NUnit.Framework;
using Market.entities;
using Market.visitors;
using System.Collections.Generic;
using Market.buyingRules;
using Market.entities.buyingStrategy;
using Market.entities.calculateInfaltion;
using Market.entities.marginCalculation;

namespace Market.Tests
{
    [TestFixture]
    public class UpdateVisitorTests
    {
        [Test]
        public void VisitSeller_ShouldUpdateProductPricesCorrectly()
        {
            var product1 = new Product("Laptop", 1000, 1500, ProductType.Essential, 10);
            var product2 = new Product("Phone", 500, 800, ProductType.Luxory, 5);
            var products = new List<Product> { product1, product2 };

            var updateVisitor = new UpdateVisitor(products, 9, 24);
            var seller = new Seller(products, 0.2, new DefaultMarginCalculator());
            
            updateVisitor.VisitSeller(seller);
            
            Assert.That(product1.Price, Is.EqualTo(1200).Within(0.001));  // 1000 * (1 + 0.2) * (1 + 0) (brak inflacji)
            Assert.That(product2.Price, Is.EqualTo(600).Within(0.001));   // 500 * (1 + 0.2) * (1 + 0)
        }

        [Test]
        public void VisitBuyer_ShouldIncreaseBudgetWithInflation()
        {
            var product1 = new Product("Laptop", 1000, 1500, ProductType.Essential, 10);
            var product2 = new Product("Phone", 500, 800, ProductType.Luxory, 5);
            var products = new List<Product> { product1, product2 };

            var updateVisitor = new UpdateVisitor(products, 9 ,24);
            var buyer = new Buyer(new DefaultBuyingRules(), new DefaultBuyingStrategy()) { Budget = 1000 };
            
            updateVisitor.VisitBuyer(buyer);
            
            Assert.IsTrue(buyer.Budget > 1000); // Oczekiwany wzrost budżetu, ponieważ inflacja jest dodatnia
        }

        [Test]
        public void VisitCentralBank_ShouldUpdateInflationRate()
        {
            var centralBank = new CentralBank(0.05, 100000, new DefaultInflationCalculator());
            var updateVisitor = new UpdateVisitor(new List<Product>(), 9, 24);

            updateVisitor.VisitCentralBank(centralBank);
            
            Assert.AreEqual(0.05 + 0.03, centralBank.InflationRate, 2);  // Oczekiwane zwiększenie inflacji, ponieważ poprzedni obrót był niewystarczający
        }

        [Test]
        public void UpdateBuyerNeeds_ShouldAddNewNeeds()
        {
            var product1 = new Product("Laptop", 1000, 1500, ProductType.Essential, 10);
            var product2 = new Product("Phone", 500, 800, ProductType.Luxory, 5);
            var products = new List<Product> { product1, product2 };

            var buyer = new Buyer(new DefaultBuyingRules(), new DefaultBuyingStrategy());
            var updateVisitor = new UpdateVisitor(products, 9, 24);
            
            updateVisitor.UpdateBuyerNeeds(buyer);
            
            Assert.Contains("Laptop", buyer.Needs);
            Assert.Contains("Phone", buyer.Needs);
        }
    }
}
