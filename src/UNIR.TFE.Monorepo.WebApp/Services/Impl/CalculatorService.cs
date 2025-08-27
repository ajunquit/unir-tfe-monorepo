namespace UNIR.TFE.Monorepo.WebApp.Services.Impl
{
    public class CalculatorService : ICalculatorService
    {
        public decimal Addition(decimal a, decimal b)
        {
            return a + b;
        }

        public decimal Division(decimal a, decimal b)
        {
            if (b==0)
            {
                throw new InvalidOperationException("División por cero no está permitida.");
            }
            return a / b;
        }

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
