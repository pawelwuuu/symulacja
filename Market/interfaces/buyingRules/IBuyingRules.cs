namespace Market.buyingRules;

public interface IBuyingRules
{
    bool CanBuyLuxury(double productPrice, double avgProductPrice, double budget);
    bool CanBuyEssential(double productPrice, double avgProductPrice, double budget);
}
