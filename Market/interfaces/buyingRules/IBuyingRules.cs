namespace Market.buyingRules;

public interface IBuyingRules
{
    bool CanBuyLuxury(double productPrice, double budget);
    bool CanBuyEssential(double productPrice, double budget);
}
