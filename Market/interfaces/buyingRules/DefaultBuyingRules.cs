namespace Market.buyingRules;

public class DefaultBuyingRules : IBuyingRules
{
    public bool CanBuyLuxury(double productPrice, double avgProductPrice, double budget)
    {
        Console.WriteLine("Can buy lux product price: " + productPrice + ", avg: " + avgProductPrice + ", budget: " + budget + " " +
                          (productPrice <= 0.8 * budget && productPrice <= avgProductPrice));
        return productPrice <= 0.8 * budget && productPrice <= avgProductPrice;
    }

    public bool CanBuyEssential(double productPrice, double avgProductPrice, double budget)
    {
        Console.WriteLine("Can buy esential product price: " + productPrice + ", avg: " + avgProductPrice + ", budget: " + budget + " " +
                          (productPrice <= budget && productPrice <= 1.2 * avgProductPrice));
        return productPrice <= budget && productPrice <= 1.1 * avgProductPrice;
    }
}
