using Market;
using Market.buyingRules;
using Market.entities;
using Market.visitors;

public class Program
{
    public static void Main(string[] args)
    {
        List<Seller> sellers = new List<Seller>();
        List<Buyer> buyers = new List<Buyer>();

        List<Product> products = new List<Product>();
        products.Add(new Product("Piwo", 7, 0, ProductType.Essential, 2));
        products.Add(new Product("Chleb", 3.5, 0, ProductType.Essential, 2));
        products.Add(new Product("TV", 5, 0, ProductType.Luxory, 2));
        products.Add(new Product("Gra", 6, 0, ProductType.Luxory, 2));

        sellers.Add(new Seller(products, 0.2));
        sellers.Add(new Seller(products, 0.32));
        // sellers.Add(new Seller());
        buyers.Add(new Buyer(new DefaultBuyingRules()));
        buyers.Add(new Buyer(new DefaultBuyingRules()));

        var visitor = new UpdateVisitor(products);

    var market = new MarketSimulator(sellers, buyers, new CentralBank(0.02, 3.65), visitor);

    for (int i = 0; i < 100; i++)
    {
        market.SimulateTurn();
    }
    }
}