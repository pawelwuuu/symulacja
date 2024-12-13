using Market.entities;

namespace Market.publishers;

public class CentralBankUnSubscriber : IDisposable
{
    private List<IObserver<CentralBank>> lstObservers;
    private IObserver<CentralBank> observer;

    public CentralBankUnSubscriber(List<IObserver<CentralBank>> observersCollection,
        IObserver<CentralBank> observer)
    {
        this.lstObservers = observersCollection;
        this.observer = observer;
    }

    public void Dispose()
    {
        lstObservers.Remove(this.observer);
    }
}