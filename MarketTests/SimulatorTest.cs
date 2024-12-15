using Market.buyingRules;
using Market.entities;
using Market.entities.buyingStrategy;
using Market.entities.calculateInfaltion;
using Market.entities.marginCalculation;
using Market.visitors;

namespace Market.Tests;

[TestFixture]
public class SimulatorTest
{
    private List<Seller> sellers;
    private List<Buyer> buyers;
    private List<Product> products;
    private List<Product> products2;
    private UpdateVisitor updateVisitor;
    private MarketSimulator marketSimulator;
    private CentralBank centralBank;

    [SetUp]
    public void Setup()
    {
        // Setup common dependencies
        var buyingStrategy = new DefaultBuyingStrategy();
        products = new List<Product>
        {
            new Product("Piwo", 4.5, 0, ProductType.Essential, 4),
            new Product("Piwo1", 5.2, 0, ProductType.Essential, 4),
            new Product("TV1", 5.3, 0, ProductType.Luxory, 4)
        };
        products2 = new List<Product>
        {
            new Product("Piwo1", 5, 0, ProductType.Essential, 1),
            new Product("TV", 4.8, 0, ProductType.Luxory, 4)
        };

        sellers = new List<Seller>
        {
            new Seller(products, 0.3, new DefaultMarginCalculator()),
            new Seller(products2, 0.2, new DefaultMarginCalculator())
        };

        buyers = new List<Buyer>
        {
            new Buyer(new DefaultBuyingRules(), buyingStrategy),
            new Buyer(new DefaultBuyingRules(), buyingStrategy),
            new Buyer(new DefaultBuyingRules(), buyingStrategy)
        };

        var allProducts = products.Concat(products2).ToList();
        updateVisitor = new UpdateVisitor(allProducts, 9, 24);
        centralBank = new CentralBank(0.02, 3.6, new DefaultInflationCalculator());

        marketSimulator = new MarketSimulator(sellers, buyers, centralBank, updateVisitor);
    }

    [Test]
    public void Simulate100Turn_ShouldRunWithoutErrors()
    {
        for (int i = 0; i < 100; i++)
        {
            marketSimulator.SimulateTurn();
        }
        
        Assert.Pass("Simulation ran successfully without errors.");
    }

    [Test]
    public void Simulate100Turn_ShouldHaveCorrectMargins()
    {
        for (int i = 0; i < 100; i++)
        {
            marketSimulator.SimulateTurn();
        }

        foreach (var seller in marketSimulator.Sellers)
        {
            Assert.Less(seller.Margin, 0.5, "Margin is greater than 50%");
            Assert.Greater(seller.Margin, 0.1, "Margin is lower than 10%");
        }
    }

    [Test]
    public void Simulate100Turn_ShouldHaveCorrectInflation()
    {
        for (int i = 0; i < 100; i++)
        {
            marketSimulator.SimulateTurn();
        }

        Assert.Less(marketSimulator.CentralBank.InflationRate, 0.5, "Inflation is greater than 20%");
        Assert.Greater(marketSimulator.CentralBank.InflationRate, 0.02, "Inflation is lower than 2%");
    }
    
    [Test]
    public void Simulate100Turn_ShouldHaveCorrectInflationWithBadInflation()
    {
        for (int i = 0; i < 100; i++)
        {
            if (i == 50)
            {
                marketSimulator.CentralBank.InflationRate = 3000;
            }
            marketSimulator.SimulateTurn();
        }

        Assert.Less(marketSimulator.CentralBank.InflationRate, 0.5, "Inflation is greater than 20%");
        Assert.Greater(marketSimulator.CentralBank.InflationRate, 0.02, "Inflation is lower than 2%");
    }
}