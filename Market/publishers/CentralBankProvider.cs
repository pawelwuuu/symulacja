using Market.entities;

namespace Market.publishers;

public class CentralBankProvider : IObservable<CentralBank>
{
    private readonly List<IObserver<CentralBank>> _observers = [];
    
    public IDisposable Subscribe(IObserver<CentralBank> observer)
    {
        if (!_observers.Contains(observer))
        {
            _observers.Add(observer);
        }
        return new CentralBankUnSubscriber(_observers, observer);
    }
    
    public void DataChanged(CentralBank bank)
    {
        foreach (var obs in _observers)
        {
            obs.OnNext(bank);
        }
    }
}