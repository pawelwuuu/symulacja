namespace Market.entities;

public class BuyerNeedsHistory
{
    private Dictionary<string, Queue<double>> _buyerNeedsHistory = new();

    public void RegisterNeedPrice(string name, double price)
    {
        if (!_buyerNeedsHistory.ContainsKey(name))
            _buyerNeedsHistory.Add(name, new Queue<double>());
        
        _buyerNeedsHistory[name].Enqueue(price);
        
        if (_buyerNeedsHistory[name].Count > 2)
            _buyerNeedsHistory[name].Dequeue();
    }

    public double GetAverageNeedPrice(string name)
    {
        if (!_buyerNeedsHistory.ContainsKey(name))
            throw new KeyNotFoundException("There is no need price for this buyer");
        return _buyerNeedsHistory[name].Average();
    }
}