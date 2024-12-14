namespace Market.entities;

public enum ProductType
{
    Essential,
    Luxory
}

public class Product
{
    public string Name { get; set; }
    public double ProductionCost { get; set; }
    public double Price { get; set; }
    public int Quantity { get; set; }
    public int QuantityConstant { get; private set; }
    public ProductType Type { get; set; }

    public Product(string name, double productionCost, double price, ProductType type, int quantity)
    {
        Name = name;
        ProductionCost = productionCost;
        Price = price;
        Type = type;
        Quantity = quantity;
        QuantityConstant = quantity;
    }
}