namespace Market.entities.marginCalculation;

public class DefaultMarginCalculator : IMarginCalculator
{
    public double CalculateMargin(Dictionary<int, List<double>> salesHistory, double currentMargin, int currentTurn)
    {
        if (salesHistory.Count < 4)
        {
            Console.WriteLine($"Brak historii zakupów, skip marza: {currentMargin}");
            return currentMargin;
        }

        var threeRoundsAgo = salesHistory.ContainsKey(currentTurn - 3) ? salesHistory[currentTurn - 3].Sum() : 0;
        var twoRoundsAgo = salesHistory.ContainsKey(currentTurn - 2) ? salesHistory[currentTurn - 2].Sum() : 0;
        var roundAgo = salesHistory.ContainsKey(currentTurn - 1) ? salesHistory[currentTurn - 1].Sum() : 0;

        if (roundAgo > (threeRoundsAgo + twoRoundsAgo) / 2)
        {
            currentMargin += 0.05;
        }
        else
        {
            currentMargin -= 0.02;
        }
        
        Console.WriteLine($"Margin: {Math.Min(Math.Max(currentMargin, 0.1), 0.5)}");
        return Math.Min(Math.Max(currentMargin, 0.1), 0.5);
    }
}