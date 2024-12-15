namespace Market.entities.calculateInfaltion;

public interface ICalculateInflation
{
    double CalculateInflationRate(BankSalesHistory bankSalesHistory, double actualInflationRate, double reqTaxAmount, int turn);
}