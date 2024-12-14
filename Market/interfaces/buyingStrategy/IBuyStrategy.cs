namespace Market.entities.buyingStrategy;

public interface IBuyStrategy
{
    public void TryBuyProductFromSeller(Seller seller, Buyer buyer);
}