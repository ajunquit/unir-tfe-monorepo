using UNIR.TFE.Monorepo.WebApp.Services;
using UNIR.TFE.Monorepo.WebApp.Services.Impl;
using Xunit;

namespace UNIR.TFE.Monorepo.WebApp.Test
{
    public class SubtractionTest
    {
        private readonly ICalculatorService _sut;

        // <<< NUEVO >>>: cantidad de casos masivos para alcanzar ~12k en total
        // Ajusta este valor si cambias otras teor�as para mantenerte cerca de 12k.
        private const int BULK_CASES_COUNT = 5000;

        public SubtractionTest()
        {
            _sut = new CalculatorService();
        }

        // Pruebas b�sicas y casos extremos
        [Theory]
        [InlineData(1, 2, -1)]
        [InlineData(-1, 1, -2)]
        [InlineData(0, 0, 0)]
        [InlineData(123.45, 54.55, 68.90)]
        [InlineData(-5, -7, 2)]
        [InlineData(999999999, 1, 999999998)]
        public void Subtraction_ShouldReturn_CorrectDifference_ForGivenOperands(decimal operandA, decimal operandB, decimal expected)
        {
            var actual = _sut.Subtraction(operandA, operandB);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Subtraction_ShouldHandle_MaxDecimalValues()
        {
            decimal maxValue = decimal.MaxValue;
            var result = _sut.Subtraction(maxValue, 0);
            Assert.Equal(maxValue, result);
        }

        [Fact]
        public void Subtraction_ShouldHandle_MinDecimalValues()
        {
            decimal minValue = decimal.MinValue;
            var result = _sut.Subtraction(minValue, 0);
            Assert.Equal(minValue, result);
        }

        // Generaci�n de pruebas con n�meros secuenciales
        public static IEnumerable<object[]> SequentialNumbers()
        {
            for (int i = -500; i <= 500; i++)
            {
                yield return new object[] { i, i, 0 };        // i - i = 0
                yield return new object[] { i, -i, i - (-i) }; // = 2i
                yield return new object[] { i, 0, i };         // i - 0 = i
            }
        }

        [Theory]
        [MemberData(nameof(SequentialNumbers))]
        public void Subtraction_WithSequentialNumbers_ReturnsCorrectResult(decimal a, decimal b, decimal expected)
        {
            var result = _sut.Subtraction(a, b);
            Assert.Equal(expected, result);
        }

        // Pruebas con n�meros decimales espec�ficos
        public static IEnumerable<object[]> DecimalTestCases()
        {
            var testCases = new[]
            {
                (0.1m, 0.2m, -0.1m),
                (1.111m, 2.222m, -1.111m),
                (99.99m, 0.01m, 99.98m),
                (123.456m, 789.012m, -665.556m),
                (-45.67m, 45.67m, -91.34m),
                (1000.001m, 0.999m, 999.002m),
                (0.0001m, 0.0001m, 0.0000m),
                (999.999m, 0.001m, 999.998m)
            };

            foreach (var (a, b, expected) in testCases)
                yield return new object[] { a, b, expected };
        }

        [Theory]
        [MemberData(nameof(DecimalTestCases))]
        public void Subtraction_WithDecimalNumbers_ReturnsPreciseResult(decimal a, decimal b, decimal expected)
        {
            var result = _sut.Subtraction(a, b);
            Assert.Equal(expected, result);
        }

        // Pruebas con n�meros grandes
        public static IEnumerable<object[]> LargeNumbersTestCases()
        {
            var largeNumbers = new[]
            {
                1000000m,
                5000000m,
                10000000m,
                50000000m,
                100000000m,
                500000000m,
                1000000000m,
                5000000000m,
                10000000000m,
                50000000000m
            };

            foreach (var number in largeNumbers)
            {
                yield return new object[] { number, number, 0m };                // n - n = 0
                yield return new object[] { number, 0m, number };                 // n - 0 = n
                yield return new object[] { -number, number, -2m * number };      // (-n) - n = -2n
            }
        }

        [Theory]
        [MemberData(nameof(LargeNumbersTestCases))]
        public void Subtraction_WithLargeNumbers_ReturnsCorrectResult(decimal a, decimal b, decimal expected)
        {
            var result = _sut.Subtraction(a, b);
            Assert.Equal(expected, result);
        }

        // Propiedad antisim�trica: a - b = -(b - a)
        public static IEnumerable<object[]> AntisymmetricPropertyTestCases()
        {
            var random = new Random(20250825); // seed fija para repetibilidad
            for (int i = 0; i < 100; i++)
            {
                decimal a = (decimal)(random.NextDouble() * 1000 - 500);
                decimal b = (decimal)(random.NextDouble() * 1000 - 500);
                yield return new object[] { a, b };
            }
        }

        [Theory]
        [MemberData(nameof(AntisymmetricPropertyTestCases))]
        public void Subtraction_ShouldBeAntisymmetric(decimal a, decimal b)
        {
            var result1 = _sut.Subtraction(a, b);
            var result2 = _sut.Subtraction(b, a);
            Assert.Equal(result1, -result2);
        }

        // Transformaci�n v�lida: (a - b) - c = a - (b + c)
        public static IEnumerable<object[]> AssociativeLikeTransformTestCases()
        {
            var random = new Random(20250826); // seed fija
            for (int i = 0; i < 100; i++)
            {
                decimal a = (decimal)(random.NextDouble() * 1000 - 500);
                decimal b = (decimal)(random.NextDouble() * 1000 - 500);
                decimal c = (decimal)(random.NextDouble() * 1000 - 500);
                yield return new object[] { a, b, c };
            }
        }

        [Theory]
        [MemberData(nameof(AssociativeLikeTransformTestCases))]
        public void Subtraction_ShouldRespect_SubtractionTransform(decimal a, decimal b, decimal c)
        {
            var left = _sut.Subtraction(_sut.Subtraction(a, b), c);    // (a - b) - c
            var right = _sut.Subtraction(a, b + c);                // a - (b + c)
            Assert.Equal(left, right);
        }

        // Elemento neutro (a - 0 = a) y "negaci�n" (0 - a = -a)
        public static IEnumerable<object[]> IdentityElementTestCases()
        {
            var random = new Random(20250827); // seed fija
            for (int i = 0; i < 100; i++)
            {
                decimal a = (decimal)(random.NextDouble() * 2000 - 1000);
                yield return new object[] { a };
            }
        }

        [Theory]
        [MemberData(nameof(IdentityElementTestCases))]
        public void Subtraction_WithZeroOnRight_ShouldReturnSameNumber(decimal a)
        {
            var result = _sut.Subtraction(a, 0);
            Assert.Equal(a, result);
        }

        [Theory]
        [MemberData(nameof(IdentityElementTestCases))]
        public void Subtraction_WithZeroOnLeft_ShouldReturnNegated(decimal a)
        {
            var result = _sut.Subtraction(0, a);
            Assert.Equal(-a, result);
        }

        // Inverso aditivo bajo resta directa (a - a = 0)
        [Theory]
        [MemberData(nameof(IdentityElementTestCases))]
        public void Subtraction_WithSameOperands_ShouldReturnZero(decimal a)
        {
            var result = _sut.Subtraction(a, a);
            Assert.Equal(0, result);
        }

        // Pruebas de rendimiento con m�ltiples operaciones
        [Fact]
        public void Subtraction_ShouldHandle_MultipleOperationsCorrectly()
        {
            decimal result = 0;
            decimal expected = 0;

            for (int i = 1; i <= 1000; i++)
            {
                result = _sut.Subtraction(result, 1); // restar 1
                expected -= 1;
            }

            Assert.Equal(expected, result);
        }

        // Pruebas de precisi�n decimal
        public static IEnumerable<object[]> PrecisionTestCases()
        {
            return new[]
            {
                new object[] { 0.0000000000000000000000000001m, 0.0000000000000000000000000001m, 0.0000000000000000000000000000m },
                new object[] { 0.0000000000000000000000000001m, 0.0000000000000000000000000002m, -0.0000000000000000000000000001m },
                new object[] { 1.2345678901234567890123456789m, 2.3456789012345678901234567890m, -1.1111110111111111011111111101m }
            };
        }

        [Theory]
        [MemberData(nameof(PrecisionTestCases))]
        public void Subtraction_WithHighPrecisionNumbers_MaintainsPrecision(decimal a, decimal b, decimal expected)
        {
            var result = _sut.Subtraction(a, b);
            Assert.Equal(expected, result);
        }

        // =========================
        // <<< NUEVO >>> 8 546 CASOS
        // =========================

        // Genera decimales seguros con escalas 0..3 y magnitudes acotadas.
        private static decimal NextDecimal(Random rng)
        {
            var sign = rng.Next(0, 2) == 0 ? 1m : -1m;
            int magnitude = rng.Next(0, 10_000_000); // 0 .. 9,999,999
            int scale = rng.Next(0, 4);              // 0..3 decimales

            decimal divisor = 1m;
            for (int i = 0; i < scale; i++) divisor *= 10m;

            return sign * (magnitude / divisor);
        }

        public static IEnumerable<object[]> BulkSubtractionCases()
        {
            var rng = new Random(424242); // seed fija para repetibilidad
            for (int i = 0; i < BULK_CASES_COUNT; i++)
            {
                decimal a = NextDecimal(rng);
                decimal b = NextDecimal(rng);

                decimal expected = a - b;
                yield return new object[] { a, b, expected };
            }
        }

        [Trait("size", "bulk")]
        [Theory]
        [MemberData(nameof(BulkSubtractionCases))]
        public void Subtraction_BulkRandomizedDataset_ReturnsCorrectDifference(decimal a, decimal b, decimal expected)
        {
            var result = _sut.Subtraction(a, b);
            Assert.Equal(expected, result);
        }
    }
}
