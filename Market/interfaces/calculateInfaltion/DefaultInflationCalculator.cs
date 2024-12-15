namespace Market.entities.calculateInfaltion;

public class DefaultInflationCalculator : ICalculateInflation
{
    public double CalculateInflationRate(BankSalesHistory bankSalesHistory, double actualInflationRate, double reqTaxAmount,
        int turn)
    {
        if (!bankSalesHistory.HasTurnData(turn - 1))
        {
            return actualInflationRate;
        }

        var lastTurnover = bankSalesHistory.GetFinancialTurnover(turn - 1);
        var bankIncome = lastTurnover * actualInflationRate;

        double inflationFactor = reqTaxAmount / bankIncome;
        if (inflationFactor < 1 / 30.0 || inflationFactor > 30)
        {
            return actualInflationRate * inflationFactor;
        }

        if (bankIncome < reqTaxAmount)
        {
            return actualInflationRate += 0.02;
        }
        
        return actualInflationRate -= 0.01;
    }
}