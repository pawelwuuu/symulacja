namespace Market.entities;

public class BankSalesHistory
{
    Dictionary<int, Dictionary<Seller, double>> bankSalesHistory = new();  // turn -> <Seller, sallesSum>

    public void RegisterIncome(int turn, Seller seller)
    {
        if (!bankSalesHistory.ContainsKey(turn))
        {
            bankSalesHistory.Add(turn, new Dictionary<Seller, double>());
        }

        bankSalesHistory[turn][seller] = seller.SalesHistory[turn].Sum();
    }

    public double GetFinancialTurnover(int turn)
    {
        return bankSalesHistory[turn].Values.Sum();;
    }

    public bool HasTurnData(int turn)
    {
        return bankSalesHistory.ContainsKey(turn);
    }
}