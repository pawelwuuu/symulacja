using Market.publishers;
using Market.visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using Market.entities.marginCalculation;

namespace Market.entities;

public class Seller : Entity, IVisitable, IObserver<CentralBank>
{
    public readonly Dictionary<int, List<double>> SalesHistory = new Dictionary<int, List<double>>(); //turn -> List<price>
    public List<Product> Products { get; private set; }
    public double Margin { get; set; } = 0.2;
    public double InflationRate { get; private set; }
    private IDisposable _unsubscriber;
    public ProductPriceProvider ProductProvider { get; private set; }
    private IMarginCalculator _marginCalculator;

    public Seller(List<Product> products, double margin, IMarginCalculator marginCalculator)
    {
        Products = products;
        Margin = margin;
        _marginCalculator = marginCalculator;
        ProductProvider = new ProductPriceProvider();
    }

    public void AdjustMargin()
    {
        _marginCalculator.CalculateMargin(SalesHistory, Margin, Turn);
    }

    public void BuyProduct(string productName)
    {
        var product = Products.First(p => p.Name == productName);

        if (!SalesHistory.ContainsKey(base.Turn))
        {
            SalesHistory.Add(base.Turn, new List<double>());
        }

        if (product.Quantity == 0)
            return;

        SalesHistory[Turn].Add(product.Price);
        product.Quantity -= 1;
        ProductProvider.NotifyBank(this);
    }

    public override void Accept(IVisitor visitor)
    {
        visitor.VisitSeller(this);
    }

    public void Subscribe(IObservable<CentralBank> observable)
    {
        _unsubscriber = observable.Subscribe(this);
    }

    public void OnCompleted()
    {
        throw new NotImplementedException();
    }

    public void OnError(Exception error)
    {
        throw new NotImplementedException();
    }

    public void OnNext(CentralBank value)
    {
        InflationRate = value.InflationRate;
    }
}