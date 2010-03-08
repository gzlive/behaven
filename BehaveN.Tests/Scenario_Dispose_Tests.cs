using System;
using NUnit.Framework;
using SharpTestsEx;

namespace BehaveN.Tests
{
    [TestFixture]
    public class Scenario_Dispose_Tests : BaseScenarioTests
    {
        [Test]
        public void it_calls_dispose_at_the_end_of_a_passing_test()
        {
            TheSpecificationsFile.StepDefinitions.UseStepDefinitionsFromType<MyDisposableStepDefinitions>();
            MyDisposableStepDefinitions.DisposeInvoked = false;

            ExecuteText("Scenario: Dispose after passing",
                        "Given foo");

            MyDisposableStepDefinitions.DisposeInvoked.Should().Be.True();
        }

        [Test]
        public void it_calls_dispose_at_the_end_of_a_failing_test()
        {
            TheSpecificationsFile.StepDefinitions.UseStepDefinitionsFromType<MyDisposableStepDefinitions>();
            MyDisposableStepDefinitions.DisposeInvoked = false;

            ExecuteText("Scenario: Dispose after failing",
                        "Given foo",
                        "When failing");

            MyDisposableStepDefinitions.DisposeInvoked.Should().Be.True();
        }
    }

    public class MyDisposableStepDefinitions : IDisposable
    {
        public void given_foo()
        {
        }

        public void when_failing()
        {
            throw new Exception("fail.");
        }

        public static bool DisposeInvoked { get; set; }

        public void Dispose()
        {
            DisposeInvoked = true;
        }
    }
}
