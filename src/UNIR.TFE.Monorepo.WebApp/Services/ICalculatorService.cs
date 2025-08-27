namespace UNIR.TFE.Monorepo.WebApp.Services
{
    public interface ICalculatorService
    {
        decimal Addition(decimal a, decimal b);
        decimal Subtraction(decimal a, decimal b);
        decimal Multiplication(decimal a, decimal b);
        decimal Division(decimal a, decimal b);
    }
}
