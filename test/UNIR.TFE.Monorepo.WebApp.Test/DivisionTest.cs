using UNIR.TFE.Monorepo.WebApp.Services;
using UNIR.TFE.Monorepo.WebApp.Services.Impl;
using Xunit;

namespace UNIR.TFE.Monorepo.WebApp.Test
{
    public class DivisionTest
    {
        private readonly ICalculatorService _sut;

        // EXACTAMENTE 10 000 casos en el dataset masivo
        private const int BULK_CASES_COUNT = 10_000;

        public DivisionTest()
        {
            _sut = new CalculatorService();
        }

        // ---------------------------
        // Básicos (opcionales; quítalos si quieres EXACTAMENTE 10 000 en total)
        // ---------------------------
        [Fact]
        public void Division_ShouldThrow_OnDivideByZero()
        {
            Assert.Throws<DivideByZeroException>(() => _sut.Division(1, 0));
            Assert.Throws<DivideByZeroException>(() => _sut.Division(0, 0));
        }

        [Theory]
        [InlineData(1, 2, 0.5)]
        [InlineData(10, 4, 2.5)]
        [InlineData(2500, 0.004, 625000)]
        [InlineData(0.0008, 0.0002, 4)]
        [InlineData(-5, -2.5, 2)]
        [InlineData(0, 7, 0)]
        public void Division_ShouldReturn_CorrectQuotient_ForGivenOperands(decimal a, decimal b, decimal expected)
        {
            var actual = _sut.Division(a, b);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(12345.6789)]
        [InlineData(-9876.54321)]
        [InlineData(1)]
        public void Division_Identities(decimal a)
        {
            Assert.Equal(a, _sut.Division(a, 1)); // a/1 = a
            Assert.Equal(0, _sut.Division(0, a)); // 0/a = 0
            if (a != 0) Assert.Equal(1, _sut.Division(a, a)); // a/a = 1
        }

        // ---------------------------
        // Generadores seguros
        // ---------------------------

        // Devuelve un decimal en [-9999.999, 9999.999] con 0..3 decimales
        private static decimal NextDecimal(Random rng)
        {
            var sign = rng.Next(0, 2) == 0 ? 1m : -1m;
            int magnitude = rng.Next(0, 10_000_000); // 0..9,999,999
            int scale = rng.Next(0, 4);              // 0..3 decimales
            decimal divisor = 1m;
            for (int i = 0; i < scale; i++) divisor *= 10m;
            return sign * (magnitude / divisor);
        }

        // Devuelve un decimal con |x| >= minAbs y x != 0
        private static decimal NextNonZeroDecimal(Random rng, decimal minAbs)
        {
            decimal x;
            do
            {
                x = NextDecimal(rng);
                if (x < 0m && -x < minAbs) x = -minAbs;
                else if (x > 0m && x < minAbs) x = minAbs;
            } while (x == 0m);
            return x;
        }

        // ---------------------------
        // Dataset MASIVO (10 000 casos que no fallan)
        // ---------------------------

        public static IEnumerable<object[]> BulkDivisionCases()
        {
            var rng = new Random(424242);             // seed fija → reproducible
            const decimal MinAbsDen = 0.001m;         // evita divisores muy pequeños
            for (int i = 0; i < BULK_CASES_COUNT; i++)
            {
                decimal a = NextDecimal(rng);                     // numerador libre (puede ser 0)
                decimal b = NextNonZeroDecimal(rng, MinAbsDen);   // denominador seguro (|b|≥0.001)
                decimal expected = a / b;                         // mismo operador decimal del runtime
                yield return new object[] { a, b, expected };
            }
        }

        [Trait("size", "bulk10k")]
        [Theory]
        [MemberData(nameof(BulkDivisionCases))]
        public void Division_BulkRandomizedDataset_ReturnsCorrectQuotient(decimal a, decimal b, decimal expected)
        {
            var result = _sut.Division(a, b);
            Assert.Equal(expected, result);
        }
    }
}
