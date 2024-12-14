using Market.publishers;
using Market.visitors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Market.entities;

public class Seller : Entity, IVisitable, IObserver<CentralBank>
{
    public readonly Dictionary<int, List<double>> SalesHistory = new Dictionary<int, List<double>>();
    public List<Product> Products { get; private set; }
    public double Margin { get; set; } = 0.2;
    public double InflationRate { get; private set; }
    private IDisposable _unsubscriber;
    public ProductPriceProvider ProductProvider { get; private set; }

    public Seller(List<Product> products, double margin)
    {
        Products = products;
        Margin = margin;
        ProductProvider = new ProductPriceProvider();
    }
    
    public void AdjustMargin()
{

            if (base.Turn < 5){ //Handle the beginning rounds, wait till SalesHistory reaches appropriate size for doing averages
            return;
          }

        var averageOfLast5Turns = GetAverageSalesFromLast5Turns();

       if (averageOfLast5Turns > SalesHistory[base.Turn - 1].Sum()) // previous round less that  last 5 average, lets slightly raise the margins to generate higher income from same sales.
         {
            Margin = Math.Min(0.5, Margin + 0.01); //  Increase Margin, slow upward change of margins
              Console.WriteLine($"Increasing Margin: {Margin}");
         }

       else  if (averageOfLast5Turns <  SalesHistory[base.Turn - 1].Sum()) //decrease when round sales was lower from averages in longer horizon
         {
               Margin = Math.Max(0.05, Margin - 0.05); //fast down-speed adjustments of  margin
               Console.WriteLine($"Decreasing Margin: {Margin}");
          }


   }
private double GetAverageSalesFromLast5Turns() // Method for getting sales average of last 5 rounds to calculate better change
    {
      var lastTurnsSales =   SalesHistory
            .Where(kvp => kvp.Key > base.Turn - 5 && kvp.Key < base.Turn)// take all rounds starting form before last 5th round, going towards round before now (exclude now).
             .Select(kvp=> kvp.Value.Sum())
            .ToList();// convert rounds into sums and finally collect last turn sums into list.

       return lastTurnsSales.Count > 0 ?  lastTurnsSales.Average():0;  // do averaging in final part, but avoid errors when sales for less thatn 5 round are present

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
        
        SalesHistory[base.Turn].Add(product.Price);
        product.Quantity -= 1;
        ProductProvider.NotifyBank(this);
    }

    public void Accept(IVisitor visitor)
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