using Market.publishers;
using Market.visitors;

namespace Market.entities;

public class CentralBank : IObserver<Seller>
{
    private BankSalesHistory _bankSalesHistory;
    public CentralBankProvider CentralBankProvider { get; private set; }
    public List<IDisposable> _unsubscribers = new List<IDisposable>();
    private double _reqTaxAmount;
    private double _taxPercentage;
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

    public CentralBank(double inflationRate, double reqTaxAmount, double taxPercentage)
    {
        CentralBankProvider = new CentralBankProvider();
        _inflationRate = inflationRate;
        _reqTaxAmount = reqTaxAmount;
        _taxPercentage = taxPercentage;
        _bankSalesHistory = new BankSalesHistory();
    }

    public void IntroduceNewInflationRate()
    {
        InflationRate = CalculateInflationRate();
        Console.WriteLine($"nowa inflacja: {InflationRate}");
    }
    
    private double CalculateInflationRate()
    {
        if (_bankSalesHistory.HasTurnData(MarketSimulator.Turn - 1))
            return _reqTaxAmount / (_taxPercentage * _bankSalesHistory.GetFinancialTurnover(MarketSimulator.Turn - 1)) - 1;

        return _inflationRate;
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
        _bankSalesHistory.RegisterIncome(MarketSimulator.Turn, value);
    }
}
