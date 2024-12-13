using Market.publishers;
using Market.visitors;

namespace Market.entities;

public class Seller : IEntity, IObserver<CentralBank>
{
    public readonly Dictionary<int, List<double>>SalesHistory = new Dictionary<int, List<double>>(); // turn -> List<price>
    public List<Product> Products { get; private set; }
    public double Margin { get; set; } = 0.2;
    public double InflationRate { get; private set; }
    private IDisposable _unsubscriber;
    public ProductPriceProvider ProductProvider { get; private set; }

    public Seller(List<Product> products, double margin)
    {
        Products = products;
        Margin = margin;
        ProductProvider = new ProductPriceProvider();
    }
    
    public void AdjustMargin(int currentRound)
    {
        int previousRoundSales = SalesHistory.ContainsKey(currentRound - 1) ? SalesHistory[currentRound - 1].Count : 0;
        int twoRoundsAgoSales = SalesHistory.ContainsKey(currentRound - 2) ? SalesHistory[currentRound - 2].Count : 0;
        
        if (previousRoundSales > twoRoundsAgoSales)
        {
            Margin = Math.Min(0.5, Margin + 0.02); // Zwiększ marżę, ale nie powyżej 50%
            Console.WriteLine($"Powiekszam marze: {Margin}");
        }
        else if (previousRoundSales < twoRoundsAgoSales)
        {
            Margin = Math.Max(0.1, Margin - 0.02); // Obniż marżę, ale nie poniżej 10%
            Console.WriteLine($"Obnizam marze: {Margin}");
        }
        else
        {
            Console.WriteLine($"Marza stoi w miejscu: {Margin}");
        }
    }
    
    public void BuyProduct(string productName)
    {
        var product = Products.FirstOrDefault(p => p.Name == productName);
        
        if (product == null)
            throw new Exception($"Product '{productName}' was not found");

        if (!SalesHistory.ContainsKey(MarketSimulator.Turn))
        {
            SalesHistory.Add(MarketSimulator.Turn, new List<double>());
        }
        
        SalesHistory[MarketSimulator.Turn].Add(product.Price);
        Products.Remove(product);
        ProductProvider.NotifyBank(this);
    }

    public void Accept(IVisitor visitor)
    {
        visitor.VisitSeller(this);
    }

    public void Subscribe(IObservable<CentralBank> observable)
    {
        _unsubscriber = observable.Subscribe(this);
    }

    public void OnCompleted()
    {
        throw new NotImplementedException();
    }

    public void OnError(Exception error)
    {
        throw new NotImplementedException();
    }

    public void OnNext(CentralBank value)
    {
        InflationRate = value.InflationRate;
    }
}