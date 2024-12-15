using Market.entities;
using Market.publishers;
using Market.visitors;

namespace Market;

public class MarketSimulator
{
    public List<Seller> Sellers;
    public List<Buyer> Buyers;
    public CentralBank CentralBank;
    private IVisitor _visitor;
    private int _prevIncome = 0;

    public MarketSimulator(List<Seller> sellers, List<Buyer> buyers, CentralBank centralBank, IVisitor visitor)
    {
        Sellers = sellers;
        Buyers = buyers;
        CentralBank = centralBank;
        _visitor = visitor;
        
        sellers.ForEach(s => s.Subscribe(CentralBank.CentralBankProvider));
        buyers.ForEach(b => b.Subscribe(CentralBank.CentralBankProvider));
        
        sellers.ForEach(s => CentralBank.SubscribeSeller(s.ProductProvider));
        buyers.ForEach(b => Sellers.ForEach(s => b.Subscribe(s.ProductProvider)));
    }

    public void NextTurn()
    {
        Sellers.ForEach(s => s.NextTurn());
        Buyers.ForEach(b => b.NextTurn());
        CentralBank.NextTurn();
        Console.WriteLine("-----------------------------------\n");
    }
    
    public void SimulateTurn()
    {
        CentralBank.Accept(_visitor);
        Buyers.ForEach(b => b.Accept(_visitor));
        Sellers.ForEach(s => s.Accept(_visitor));
        
        NextTurn();
    }
}