using Market;
using Market.buyingRules;
using Market.entities;

public class Program
{
    public static void Main(string[] args)
    {
        List<Seller> sellers = new List<Seller>();
        List<Buyer> buyers = new List<Buyer>();

        List<Product> products = new List<Product>();
        products.Add(new Product("Piwo", 3.5, 0, ProductType.Essential));
        products.Add(new Product("Piwo", 3.5, 0, ProductType.Essential));
        products.Add(new Product("Chleb", 5, 0, ProductType.Essential));
        products.Add(new Product("TV", 1000, 0, ProductType.Luxory));
        products.Add(new Product("TV", 1000, 0, ProductType.Luxory));
        products.Add(new Product("TV", 1000, 0, ProductType.Luxory));

        sellers.Add(new Seller(products, 0.2));
        // sellers.Add(new Seller());
        buyers.Add(new Buyer(15500, new List<string>{"Piwo", "TV"}, new DefaultBuyingRules()));
        buyers.Add(new Buyer(15500, new List<string>{"Piwo", "TV"}, new DefaultBuyingRules()));

    var market = new MarketSimulator(sellers, buyers, new CentralBank(0.02, 150, 0.1));
        market.SimulateTurn();
        market.SimulateTurn();
        market.SimulateTurn();
        market.SimulateTurn();
        market.SimulateTurn();
    }
}