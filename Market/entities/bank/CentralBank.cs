using Market.entities.calculateInfaltion;
using Market.publishers;
using Market.visitors;

namespace Market.entities;

public class CentralBank : Entity, IObserver<Seller>
{
    private BankSalesHistory _bankSalesHistory;
    private ICalculateInflation _calculateInflation { get; set; }
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

    public CentralBank(double inflationRate, double reqTaxAmount, ICalculateInflation calculateInflation)
    {
        CentralBankProvider = new CentralBankProvider();
        _inflationRate = inflationRate;
        _reqTaxAmount = reqTaxAmount;
        _bankSalesHistory = new BankSalesHistory();
        _calculateInflation = calculateInflation;
    }

    public void IntroduceNewInflationRate()
    {
        InflationRate = CalculateInflationRate();
    }

    private double CalculateInflationRate()
    {
        return _calculateInflation.CalculateInflationRate(_bankSalesHistory, _inflationRate, _reqTaxAmount, Turn);
    }

    public void SubscribeSeller(IObservable<Seller> observable)
    {
        _unsubscribers.Add(observable.Subscribe(this));
    }

    public void OnCompleted()
    {
    }

    public void OnError(Exception error)
    {
    }

    public void OnNext(Seller value)
    {
        _bankSalesHistory.RegisterIncome(Turn, value);
    }

    public override void Accept(IVisitor visitor)
    {
        visitor.VisitCentralBank(this);
    }
}