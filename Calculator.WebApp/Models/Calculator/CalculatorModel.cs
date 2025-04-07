namespace Calculator.WebApp.Models.Calculator
{
    public class CalculatorModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int Number1 { get; set; }
        public int Number2 { get; set; }
        public int Result { get; set; }
        public string Operation { get; set; } = "Addition"; // Default operation
    }
}
