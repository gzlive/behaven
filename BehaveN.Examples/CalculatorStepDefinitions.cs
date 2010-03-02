using SharpTestsEx;

namespace BehaveN.Examples
{
    public class CalculatorStepDefinitions
    {
        private Calculator _calculator;
        private int _result;

        public void Given_a_new_calculator()
        {
            _calculator = new Calculator();
        }

        public void When_adding_arg1_and_arg2(int a, int b)
        {
            _result = _calculator.Add(a, b);
        }

        public void When_dividing_arg1_by_arg2(int a, int b)
        {
            _result = _calculator.Divide(a, b);
        }

        public void Then_the_result_should_be_arg1(int n)
        {
            _result.Should().Be(n);
        }
    }
}
