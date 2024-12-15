using Market.entities;

namespace Market.visitors;

public class UpdateVisitor : IVisitor
{
    private Random _random = new Random();
    private readonly List<Product> essentialProducts;
    private readonly List<Product> luxuryProducts;
    public int LowerBoundSalary { get; set; }
    public int UpperBoundSalary { get; set; }
    public UpdateVisitor(List<Product> products, int lowerBoundSalary, int upperBoundSalary)
    {
        essentialProducts = products.FindAll(p => p.Type == ProductType.Essential);
        luxuryProducts = products.FindAll(p => p.Type == ProductType.Luxory);
        LowerBoundSalary = lowerBoundSalary;
        UpperBoundSalary = upperBoundSalary;
    }
    
    public void VisitSeller(Seller seller)
    {
        seller.AdjustMargin();
        foreach (var product in seller.Products)
        {
            product.Price = product.ProductionCost * (1 + seller.Margin) * (1 + seller.InflationRate);
        }
        seller.Products.ForEach(p => p.Quantity = p.QuantityConstant);
        seller.ProductProvider.NotifyBuyers(seller);
    }

    public void VisitBuyer(Buyer buyer)
    {
        buyer.Budget += _random.Next(LowerBoundSalary, UpperBoundSalary) * (1 + buyer.InflationRate * 0.9);
        // buyer.Budget += _random.Next(12, 24) * (1 + buyer.InflationRate * 0.9);
        
        if (buyer.Needs.Count == 0)
            UpdateBuyerNeeds(buyer);
    }

    public void VisitCentralBank(CentralBank centralBank)
    {
        centralBank.IntroduceNewInflationRate();
    }

    public void UpdateBuyerNeeds(Buyer buyer)
    {
        int essentialNeeds = 1;
        int luxuryNeeds = 1;
        
        var selectedEssentialProducts = essentialProducts.OrderBy(x => _random.Next()).Take(essentialNeeds).ToList();
        var selectedLuxuryProducts = luxuryProducts.OrderBy(x => _random.Next()).Take(luxuryNeeds).ToList();
        
        foreach (var product in selectedEssentialProducts)
        {
            if (!buyer.Needs.Contains(product.Name))
            {
                buyer.Needs.Add(product.Name);
            }
        }

        foreach (var product in selectedLuxuryProducts)
        {
            if (!buyer.Needs.Contains(product.Name))
            {
                buyer.Needs.Add(product.Name);
            }
        }
    }
}