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
    }
    
    private double CalculateInflationRate()
    {
        if (!_bankSalesHistory.HasTurnData(Turn - 1))
        {
            Console.WriteLine($"Brak danych do obliczenia inflacji {InflationRate}");
            return _inflationRate;
        }

        var lastTurnover = _bankSalesHistory.GetFinancialTurnover(Turn - 1);
        var bankIncome = lastTurnover * _inflationRate;

        if (bankIncome < _reqTaxAmount)
        {
            Console.WriteLine($"new inflation rate {InflationRate + 0.01}");
            return InflationRate += 0.03;
        }
        else
        {
            Console.WriteLine($"new inflation rate {InflationRate - 0.01}");
            return InflationRate -= 0.01;
        }
        
        // Console.WriteLine($"nowa inflacja: {InflationRate + Math.Min(Math.Max(-0.045, _reqTaxAmount/bankIncome), 0.07)} poprzedni obrot ${lastTurnover} {bankIncome/_reqTaxAmount} {_reqTaxAmount}");
        
        
        
        return InflationRate + Math.Min(Math.Max(-0.045, _reqTaxAmount/bankIncome), 0.07);
        // return bankIncome / _reqTaxAmount;
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

    public override void Accept(IVisitor visitor)
    {
        visitor.VisitCentralBank(this);
    }
}
