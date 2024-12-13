using Market.entities;

namespace Market.visitors;

public class UpdateVisitor : IVisitor
{
    public void VisitSeller(Seller seller)
    {
        seller.AdjustMargin(MarketSimulator.Turn);
        foreach (var product in seller.Products)
        {
            product.Price = product.ProductionCost * (1 + seller.Margin) * (1 + seller.InflationRate);
            Console.WriteLine(product.Price);
        }
        seller.ProductProvider.NotifyBuyers(seller);
    }

    public void VisitBuyer(Buyer buyer)
    {
        buyer.Budget += buyer.Budget * buyer.InflationRate;
    }
}