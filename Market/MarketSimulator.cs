using Market.entities;
using Market.publishers;
using Market.visitors;

namespace Market;

public class MarketSimulator
{
    public static int Turn = 0;
    List<Seller> Sellers;
    List<Buyer> Buyers;
    CentralBank CentralBank;
    private UpdateVisitor UpdateVisitor = new UpdateVisitor();
    private int _prevIncome = 0;

    public MarketSimulator(List<Seller> sellers, List<Buyer> buyers, CentralBank centralBank)
    {
        Sellers = sellers;
        Buyers = buyers;
        CentralBank = centralBank;
        
        sellers.ForEach(s => s.Subscribe(CentralBank.CentralBankProvider));
        buyers.ForEach(b => b.Subscribe(CentralBank.CentralBankProvider));
        
        sellers.ForEach(s => CentralBank.SubscribeSeller(s.ProductProvider));
        buyers.ForEach(b => Sellers.ForEach(s => b.Subscribe(s.ProductProvider)));
    }

    
    public void SimulateTurn()
    {
        CentralBank.IntroduceNewInflationRate();
        
        Sellers.ForEach(UpdateVisitor.VisitSeller);
        
        Turn++;
    }
}