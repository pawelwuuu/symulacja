using Market.buyingRules;
using Market.entities;
using Market.entities.buyingStrategy;

namespace Market.Tests
{
    [TestFixture]
    public class DefaultBuyingStrategyTests
    {
        private DefaultBuyingStrategy _buyingStrategy;
        private DefaultBuyingRules _buyingRules;
        private Buyer _buyer;
        private Seller _seller;

        [SetUp]
        public void SetUp()
        {
            _buyingRules = new DefaultBuyingRules();
            _buyingStrategy = new DefaultBuyingStrategy();

            _buyer = new Buyer(_buyingRules, _buyingStrategy)
            {
                Budget = 1000,
            };

            _seller = new Seller(
                new List<Product>
                {
                    new Product("LuxuryCar", 500, 600, ProductType.Luxory, 5),
                    new Product("Bread", 2, 3, ProductType.Essential, 10)
                },
                margin: 0.2,
                marginCalculator: null
            );
        }

        [Test]
        public void TryBuyProductFromSeller_BuyerCanAffordEssentialProduct_PurchasesProduct()
        {
            _buyer.Needs.Add("Bread");
            _buyer.BuyerNeedsHistory.RegisterNeedPrice("Bread", 3.5);
            _buyer.BuyerNeedsHistory.RegisterNeedPrice("LuxuryCar", 3.5);
            
            
            Console.WriteLine(_buyer.BuyerNeedsHistory.GetAverageNeedPrice("Bread"));
            
            _buyingStrategy.TryBuyProductFromSeller(_seller, _buyer);

            Assert.That(_buyer.Budget, Is.EqualTo(997));
            Assert.That(_seller.Products.Find(p => p.Name == "Bread")?.Quantity, Is.EqualTo(9));
        }

        [Test]
        public void TryBuyProductFromSeller_BuyerCannotAffordLuxuryProduct_DoesNotPurchaseProduct()
        {
            _buyer.Needs.Add("LuxuryCar");
            _buyer.BuyerNeedsHistory.RegisterNeedPrice("Bread", 3.5);
            _buyer.BuyerNeedsHistory.RegisterNeedPrice("LuxuryCar", 550);

            _buyingStrategy.TryBuyProductFromSeller(_seller, _buyer);

            Assert.That(_buyer.Budget, Is.EqualTo(1000)); // Budget unchanged
            Assert.That(_seller.Products.Find(p => p.Name == "LuxuryCar")?.Quantity, Is.EqualTo(5)); // Quantity unchanged
        }

        [Test]
        public void TryBuyProductFromSeller_ProductNotInNeeds_DoesNotPurchaseProduct()
        {
            _buyer.BuyerNeedsHistory.RegisterNeedPrice("LuxuryCar", 550);
            _buyer.BuyerNeedsHistory.RegisterNeedPrice("Bread", 3.5);

            _buyingStrategy.TryBuyProductFromSeller(_seller, _buyer);

            Assert.That(_buyer.Budget, Is.EqualTo(1000)); // Budget unchanged
            Assert.That(_seller.Products.Find(p => p.Name == "LuxuryCar")?.Quantity, Is.EqualTo(5)); // Quantity unchanged
        }

        [Test]
        public void TryBuyProductFromSeller_MultipleProducts_BuyerPurchasesEssentialOnly()
        {
            _buyer.Needs.Add("Bread");
            _buyer.BuyerNeedsHistory.RegisterNeedPrice("Bread", 3);
            _buyer.Needs.Add("LuxuryCar");
            _buyer.BuyerNeedsHistory.RegisterNeedPrice("LuxuryCar", 550);

            _buyingStrategy.TryBuyProductFromSeller(_seller, _buyer);

            Assert.That(_buyer.Budget, Is.EqualTo(997)); // Budget reduced only by the essential product price
            Assert.That(_seller.Products.Find(p => p.Name == "Bread")?.Quantity, Is.EqualTo(9)); // Bread quantity reduced
            Assert.That(_seller.Products.Find(p => p.Name == "LuxuryCar")?.Quantity, Is.EqualTo(5)); // LuxuryCar quantity unchanged
        }

        [Test]
        public void TryBuyProductFromSeller_BuyerNeedsNotRegistered_ThrowsException()
        {
            _buyer.Needs.Add("LuxuryCar");

            Assert.Throws<KeyNotFoundException>(() =>
                _buyingStrategy.TryBuyProductFromSeller(_seller, _buyer));
        }
    }
}
