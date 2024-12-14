using Market.publishers;
using Market.visitors;

namespace Market.entities;

public class CentralBank : Entity, IObserver<Seller>
{
    private BankSalesHistory _bankSalesHistory;
    public CentralBankProvider CentralBankProvider { get; private set; }
    public List<IDisposable> _unsubscribers = new List<IDisposable>();
    private double _reqTaxAmount;
    private double _inflationRate;
    public double InflationRate
    {
        get { return _inflationRate; }
        set
        {
            _inflationRate = value;
            CentralBankProvider.DataChanged(this);
        }
    }

    public CentralBank(double inflationRate, double reqTaxAmount)
    {
        CentralBankProvider = new CentralBankProvider();
        _inflationRate = inflationRate;
        _reqTaxAmount = reqTaxAmount;
        _bankSalesHistory = new BankSalesHistory();
    }

    public void IntroduceNewInflationRate()
    {
        InflationRate = CalculateInflationRate();
        Console.WriteLine($"nowa inflacja: {InflationRate}");
    }
    
    private double CalculateInflationRate()
    {
        if (!_bankSalesHistory.HasTurnData(Turn - 1))
            return _inflationRate + 0.005;

        var lastTurnover = _bankSalesHistory.GetFinancialTurnover(Turn - 1);
        var bankIncome = lastTurnover * _inflationRate;
        
        return Math.Min(Math.Max(-0.045, bankIncome/_reqTaxAmount), _inflationRate);
    }

    public void SubscribeSeller(IObservable<Seller> observable)
    {
        _unsubscribers.Add(observable.Subscribe(this));
    }

    public void OnCompleted()
    { }

    public void OnError(Exception error)
    { }

    public void OnNext(Seller value)
    {
        _bankSalesHistory.RegisterIncome(Turn, value);
    }
}
