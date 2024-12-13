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
    public ProductType Type { get; set; }

    public Product(string name, double productionCost, double price, ProductType type)
    {
        Name = name;
        ProductionCost = productionCost;
        Price = price;
        Type = type;
    }
}