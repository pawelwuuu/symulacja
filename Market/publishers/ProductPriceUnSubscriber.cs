using Market.entities;

namespace Market.publishers;

public class ProductPriceUnSubscriber : IDisposable
{
    private List<IObserver<Seller>> lstObservers;
    private IObserver<Seller> observer;

    public ProductPriceUnSubscriber(List<IObserver<Seller>> observersCollection,
        IObserver<Seller> observer)
    {
        this.lstObservers = observersCollection;
        this.observer = observer;
    }

    public void Dispose()
    {
        lstObservers.Remove(this.observer);
    }
}