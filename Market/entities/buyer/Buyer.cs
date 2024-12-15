using Market.buyingRules;
using Market.entities.buyingStrategy;
using Market.visitors;

namespace Market.entities;

public class Buyer : Entity, IVisitable, IObserver<Seller>, IObserver<CentralBank>
{
    public BuyerNeedsHistory BuyerNeedsHistory;
    public List<string> Needs { get; } = [];
    public double Budget { get; set; }
    public double InflationRate { get; private set; }
    private readonly List<IDisposable> _subscriptions = new();
    public readonly IBuyingRules BuyingRules;
    public readonly IBuyStrategy BuyStrategy;

    public Buyer(IBuyingRules buyingRules, IBuyStrategy buyStrategy, int buyerHistoryLength = 3)
    {
        BuyingRules = buyingRules;
        BuyerNeedsHistory = new BuyerNeedsHistory(buyerHistoryLength);
        BuyStrategy = buyStrategy;
    }

    public override void Accept(IVisitor visitor)
    {
        visitor.VisitBuyer(this);
    }

    public void Subscribe(IObservable<Seller> seller)
    {
        _subscriptions.Add(seller.Subscribe(this));
    }

    public void Subscribe(IObservable<CentralBank> centralBank)
    {
        _subscriptions.Add(centralBank.Subscribe(this));
    }

    public void OnCompleted() { }

    public void OnError(Exception error)
    {
        Console.WriteLine($"Error: {error.Message}");
    }

    public void OnNext(Seller seller)
    {
        foreach (var product in seller.Products)
        {
            BuyerNeedsHistory.RegisterNeedPrice(product.Name, product.Price);
        }
        
        TryPurchaseProduct(seller);
    }

    public void OnNext(CentralBank bank)
    {
        InflationRate = bank.InflationRate;
    }

    private void TryPurchaseProduct(Seller seller)
    {
        BuyStrategy.TryBuyProductFromSeller(seller, this);
    }
}
