namespace UNIR.TFE.Monorepo.WebApp.Services.Impl
{
    public class CalculatorService : ICalculatorService
    {
        public int Addition(int a, int b)
        {
            return a + b;
        }

        public int Division(int a, int b)
        {
            if (b==0)
            {
                throw new InvalidOperationException("División por cero no está permitida.");
            }
            return a / b;
        }

        public int Multiplication(int a, int b)
        {
            return a * b;
        }

        public int Subtraction(int a, int b)
        {
            return a - b;
        }
    }
}
