namespace Market.entities.calculateInfaltion;

public class DefaultInflationCalculator : ICalculateInflation
{
    public double CalculateInflationRate(BankSalesHistory bankSalesHistory, double actualInflationRate, double reqTaxAmount,
        int turn)
    {
        if (!bankSalesHistory.HasTurnData(turn - 1))
        {
            Console.WriteLine($"Brak danych do obliczenia inflacji {actualInflationRate}");
            return actualInflationRate;
        }

        var lastTurnover = bankSalesHistory.GetFinancialTurnover(turn - 1);
        var bankIncome = lastTurnover * actualInflationRate;
        Console.WriteLine($"przychod bank {bankIncome} a wymaga {reqTaxAmount}");

        double inflationFactor = reqTaxAmount / bankIncome;
        if (inflationFactor < 1 / 30.0 || inflationFactor > 30)
        {
            return actualInflationRate * inflationFactor;
        }

        if (bankIncome < reqTaxAmount)
        {
            Console.WriteLine($"new inflation rate {actualInflationRate + 0.02}");
            return actualInflationRate += 0.02;
        }

        Console.WriteLine($"new inflation rate {actualInflationRate - 0.01}");
        return actualInflationRate -= 0.01;
    }
}