using Market.entities;

namespace Market.publishers;

public class ProductPriceProvider : IObservable<Seller>
{
    private readonly List<IObserver<Seller>> _observers = [];
    
    public IDisposable Subscribe(IObserver<Seller> observer)
    {
        if (!_observers.Contains(observer))
        {
            _observers.Add(observer);
        }
        return new ProductPriceUnSubscriber(_observers, observer);
    }
    
    public void NotifyBuyers(Seller seller)
    {
        foreach (var obs in _observers)
        {
            if (obs is not CentralBank)
            {
                obs.OnNext(seller);
            }
        }
    }

    public void NotifyBank(Seller seller)
    {
        foreach (var observer in _observers)
        {
            if (observer is CentralBank)
            {
                observer.OnNext(seller);
            }
        }
    }
}