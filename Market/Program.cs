using Market;
using Market.buyingRules;
using Market.entities;
using Market.entities.buyingStrategy;
using Market.entities.marginCalculation;
using Market.visitors;

public class Program
{
    public static void Main(string[] args)
    {
        List<Seller> sellers = new List<Seller>();
        List<Buyer> buyers = new List<Buyer>();

        var buyingStrategy = new DefaultBuyingStrategy();

        List<Product> products = new List<Product>();
        List<Product> products2 = new List<Product>();
        products.Add(new Product("Piwo", 4.5, 0, ProductType.Essential, 4));
        products2.Add(new Product("Piwo1", 5, 0, ProductType.Essential, 1));
        products.Add(new Product("Piwo1", 5.2, 0, ProductType.Essential, 4));
        products.Add(new Product("TV1", 5.3, 0, ProductType.Luxory, 4));
        products2.Add(new Product("TV", 4.8, 0, ProductType.Luxory, 4));
        
        sellers.Add(new Seller(products, 0.3, new DefaultMarginCalculator()));
        sellers.Add(new Seller(products2, 0.2, new DefaultMarginCalculator()));
        buyers.Add(new Buyer(new DefaultBuyingRules(), buyingStrategy));
        buyers.Add(new Buyer(new DefaultBuyingRules(), buyingStrategy));
        buyers.Add(new Buyer(new DefaultBuyingRules(), buyingStrategy));
        // buyers.Add(new Buyer(new DefaultBuyingRules()));
        // buyers.Add(new Buyer(new DefaultBuyingRules()));
        // buyers.Add(new Buyer(new DefaultBuyingRules()));
        // buyers.Add(new Buyer(new DefaultBuyingRules()));

        var p = products.Concat(products2).ToList();
        p.ForEach(p => Console.WriteLine(p.Name));
        
        var visitor = new UpdateVisitor(p);

    var market = new MarketSimulator(sellers, buyers, new CentralBank(0.02, 3.6), visitor);

    for (int i = 0; i < 100; i++)
    {
        market.SimulateTurn();
    }
    }
}