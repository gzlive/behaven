using NUnit.Framework;
using SharpTestsEx;

namespace BehaveN.Tests
{
    [TestFixture]
    public class Scenario_Context_Tests : BaseTests
    {
        [Test]
        public void it_passes_when_all_of_the_outputs_are_correct()
        {
            s.UseStepDefinitionsFromType<MyGivenStepDefinitonsThatRequireMyContext>();
            s.UseStepDefinitionsFromType<MyThenStepDefinitonsThatRequireMyContext>();

            VerifyText("Given this foo",
                       "Then the value should be foo");

            ShouldHavePassed();
        }
    }

    public class MyGivenStepDefinitonsThatRequireMyContext
    {
        private readonly MyContext myContext;

        public MyGivenStepDefinitonsThatRequireMyContext(MyContext myContext)
        {
            this.myContext = myContext;
        }

        public void Given_this_value(string value)
        {
            this.myContext.MyString = value;
        }
    }

    public class MyThenStepDefinitonsThatRequireMyContext
    {
        private readonly MyContext myContext;

        public MyThenStepDefinitonsThatRequireMyContext(MyContext myContext)
        {
            this.myContext = myContext;
        }

        public void Then_the_value_should_be_this(string @this)
        {
            this.myContext.MyString.Should().Be(@this);
        }
    }

    public class MyContext
    {
        public string MyString { get; set; }
    }
}
