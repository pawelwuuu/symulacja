using Market.buyingRules;

namespace Market.entities.buyingStrategy;

public class DefaultBuyingStrategy : IBuyStrategy
{
    public void TryBuyProductFromSeller(Seller seller, Buyer buyer)
    {
        foreach (var product in seller.Products.OrderBy(p => p.Type != ProductType.Essential))
        {
            bool canBuy = product.Type switch
            {
                ProductType.Luxory => buyer.BuyingRules.CanBuyLuxury(product.Price, buyer.BuyerNeedsHistory.GetAverageNeedPrice(product.Name), buyer.Budget),
                ProductType.Essential => buyer.BuyingRules.CanBuyEssential(product.Price, buyer.BuyerNeedsHistory.GetAverageNeedPrice(product.Name), buyer.Budget),
                _ => false
            };

            if (canBuy && buyer.Needs.Contains(product.Name))
            {
                Console.WriteLine($"Buying product: {product.Name}");
                buyer.Budget -= product.Price;
                seller.BuyProduct(product.Name);
            }
        }
    }
}