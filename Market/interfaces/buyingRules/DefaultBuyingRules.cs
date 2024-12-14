namespace Market.buyingRules;

public class DefaultBuyingRules : IBuyingRules
{
    public bool CanBuyLuxury(double productPrice, double budget)
    {
        return productPrice <= 0.4 * budget;
    }

    public bool CanBuyEssential(double productPrice, double budget)
    {
        return productPrice <= budget;
    }
}
