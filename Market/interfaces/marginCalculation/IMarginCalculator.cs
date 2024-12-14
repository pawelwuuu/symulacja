namespace Market.entities.marginCalculation;

public interface IMarginCalculator
{
    public double CalculateMargin(Dictionary<int, List<double>> salesHistory, double currentMargin, int currentTurn);
}