using Calculator.WebApp.Services;
using Calculator.WebApp.Services.Impl;

namespace Calculator.WebApp.Test
{
    public class CalculatorServiceTests
    {
        private readonly ICalculatorService _calculatorService;

        public CalculatorServiceTests()
        {
            _calculatorService = new CalculatorService();
        }

        [Theory]
        [InlineData(1, 2, 3)]
        [InlineData(0, 0, 0)]
        [InlineData(-1, -1, -2)]
        [InlineData(int.MaxValue, 1, int.MinValue)] // Overflow case
        public void Addition_ShouldReturnCorrectResult(int a, int b, int expected)
        {
            // Act
            var result = _calculatorService.Addition(a, b);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(10, 2, 5)]
        [InlineData(0, 1, 0)]
        [InlineData(-10, 2, -5)]
        [InlineData(10, -2, -5)]
        public void Division_ShouldReturnCorrectResult_WhenDivisorIsNotZero(int a, int b, int expected)
        {
            // Act
            var result = _calculatorService.Division(a, b);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(10, 0)]
        [InlineData(-5, 0)]
        [InlineData(0, 0)]
        public void Division_ShouldThrowInvalidOperationException_WhenDivisorIsZero(int a, int b)
        {
            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => _calculatorService.Division(a, b));
            Assert.Equal("División por cero no está permitida.", exception.Message);
        }

        [Theory]
        [InlineData(2, 3, 6)]
        [InlineData(0, 5, 0)]
        [InlineData(-2, 3, -6)]
        [InlineData(-2, -3, 6)]
        [InlineData(int.MaxValue, 2, -2)] // Overflow case
        public void Multiplication_ShouldReturnCorrectResult(int a, int b, int expected)
        {
            // Act
            var result = _calculatorService.Multiplication(a, b);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(5, 3, 2)]
        [InlineData(0, 0, 0)]
        [InlineData(-1, -1, 0)]
        [InlineData(int.MinValue, 1, int.MaxValue)] // Overflow case
        public void Subtraction_ShouldReturnCorrectResult(int a, int b, int expected)
        {
            // Act
            var result = _calculatorService.Subtraction(a, b);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}