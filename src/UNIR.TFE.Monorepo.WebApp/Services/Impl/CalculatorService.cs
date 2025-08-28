namespace UNIR.TFE.Monorepo.WebApp.Services.Impl
{
    public class CalculatorService : ICalculatorService
    {
        public decimal Addition(decimal a, decimal b)
        {
            return a + b;
        }

        public decimal Division(decimal a, decimal b) => a / b;

        public decimal Multiplication(decimal a, decimal b)
        {
            return a * b;
        }

        public decimal Subtraction(decimal a, decimal b)
        {
            return a - b;
        }
    }
}
