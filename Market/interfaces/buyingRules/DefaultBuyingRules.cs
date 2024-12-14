namespace Market.buyingRules;

public class DefaultBuyingRules : IBuyingRules
{
    public bool CanBuyLuxury(double productPrice, double avgProductPrice, double budget)
    {
        return productPrice <= 0.8 * budget && productPrice <= avgProductPrice;
    }

    public bool CanBuyEssential(double productPrice, double avgProductPrice, double budget)
    {
        return productPrice <= budget && productPrice <= 1.2 * avgProductPrice;
    }
}
