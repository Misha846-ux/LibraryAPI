using Calculator;

namespace Books.Test
{
    public class BookTest
    {
        [Fact]
        public void CalculatorSum_FiveAndTwo_ReturnSeven()
        {
            int a = 5;
            int b = 2;

            int result = 999;
            int answer = Calc.Sum(a, b);

            Assert.Equal(result, answer);
        }
    }
}