using Market.buyingRules;
using Market.publishers;
using Market.visitors;

namespace Market.entities;

public class Buyer : IEntity, IObserver<Seller>, IObserver<CentralBank>
{
    public List<string> Needs { get; private set; } = new List<string>();
    public double Budget { get; set; }
    public double InflationRate { get; private set; }
    private List<IDisposable> _subscriptions = new List<IDisposable>();
    private readonly IBuyingRules _buyingRules;

    public Buyer(double initialBudget, List<string> needs, IBuyingRules buyingRules)
    {
        Budget = initialBudget;
        Needs = needs;
        _buyingRules = buyingRules;
    }

    public void Accept(IVisitor visitor)
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

    public void UnsubscribeAll()
    {
        foreach (var subscription in _subscriptions)
        {
            subscription.Dispose();
        }
        _subscriptions.Clear();
    }

    public void OnCompleted() { }

    public void OnError(Exception error)
    {
        Console.WriteLine($"Error: {error.Message}");
    }

    public void OnNext(Seller seller)
    {
        Console.WriteLine($"Buyer notified about seller: {seller.GetType().Name}");
        TryPurchaseProduct(seller);
    }

    public void OnNext(CentralBank bank)
    {
        InflationRate = bank.InflationRate;
        Console.WriteLine($"Updated inflation rate: {InflationRate}");
    }

    private void TryPurchaseProduct(Seller seller)
    {
        foreach (var product in seller.Products)
        {
            bool canBuy = product.Type switch
            {
                ProductType.Luxory => _buyingRules.CanBuyLuxury(product.Price, Budget),
                ProductType.Essential => _buyingRules.CanBuyEssential(product.Price, Budget),
                _ => false
            };

            if (canBuy && Needs.Contains(product.Name))
            {
                Console.WriteLine($"Buying product: {product.Name}");
                Budget -= product.Price;
                seller.BuyProduct(product.Name);
                Needs.Remove(product.Name);
                break; //1 naraz
            }
        }
    }
}
