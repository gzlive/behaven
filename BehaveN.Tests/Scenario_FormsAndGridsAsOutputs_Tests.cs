using System.Collections.Generic;
using NUnit.Framework;
using SharpTestsEx;

namespace BehaveN.Tests
{
    [TestFixture]
    public class Scenario_FormsAndGridsAsOutputs_Tests : BaseScenarioTests
    {
        [Test]
        public void it_passes_when_all_of_the_properties_are_correct()
        {
            ExecuteText("Scenario: Form passed",
                        "Then the object should look like this",
                        "  : String Property : foo",
                        "  :    Int Property : 1");

            TheScenario.Passed.Should().Be.True();
        }

        [Test]
        public void it_shows_all_the_values_that_are_not_correct_on_the_form()
        {
            ExecuteText("Scenario: Form failed",
                        "Then the object should look like this",
                        "  : String Property : baz",
                        "  :    Int Property : 3");

            TheScenario.Passed.Should().Be.False();
            ((Form)TheScenario.Steps[0].Block).GetValue(0).Should().Be("baz (was foo)");
            ((Form)TheScenario.Steps[0].Block).GetValue(1).Should().Be("3 (was 1)");
        }

        [Test]
        public void it_fails_when_the_form_has_an_unknown_property()
        {
            ExecuteText("Scenario: Form passed",
                        "Then the object should look like this",
                        "  :  String Property : foo",
                        "  :     Int Property : 1",
                        "  : Unknown Property : xxx");

            TheScenario.Passed.Should().Be.False();
            ((Form)TheScenario.Steps[0].Block).GetValue(2).Should().Be("xxx (unknown)");
        }

        [Test]
        public void it_fails_when_the_grid_has_an_unknown_property()
        {
            ExecuteText("Scenario: Grid failed",
                        "Then the objects should look like this",
                        "  | String Property | Int Property | Unknown Property |",
                        "  |             foo |            1 |              xxx |",
                        "  |             bar |            2 |              xxx |");

            TheScenario.Passed.Should().Be.False();
            ((Grid)TheScenario.Steps[0].Block).GetValue(0, 2).Should().Be("xxx (unknown)");
            ((Grid)TheScenario.Steps[0].Block).GetValue(1, 2).Should().Be("xxx (unknown)");
        }

        [Test]
        public void it_shows_all_the_values_that_are_not_correct_on_the_grid()
        {
            ExecuteText("Scenario: Grid failed",
                        "Then the objects should look like this",
                        "  | String Property | Int Property |",
                        "  |             baz |            3 |",
                        "  |            quux |            4 |");

            TheScenario.Passed.Should().Be.False();
            ((Grid)TheScenario.Steps[0].Block).GetValue(0, 0).Should().Be("baz (was foo)");
            ((Grid)TheScenario.Steps[0].Block).GetValue(0, 1).Should().Be("3 (was 1)");
            ((Grid)TheScenario.Steps[0].Block).GetValue(1, 0).Should().Be("quux (was bar)");
            ((Grid)TheScenario.Steps[0].Block).GetValue(1, 1).Should().Be("4 (was 2)");
        }

        [Test]
        public void it_shows_unexpected_rows()
        {
            ExecuteText("Scenario: Grid unexpected",
                        "Then the objects should look like this",
                        "  | String Property | Int Property |",
                        "  |             foo |            1 |");

            TheScenario.Passed.Should().Be.False();
            ((Grid)TheScenario.Steps[0].Block).GetValue(0, 0).Should().Be("foo");
            ((Grid)TheScenario.Steps[0].Block).GetValue(0, 1).Should().Be("1");
            ((Grid)TheScenario.Steps[0].Block).GetValue(1, 0).Should().Be("(unexpected) bar");
            ((Grid)TheScenario.Steps[0].Block).GetValue(1, 1).Should().Be("2");
        }

        [Test]
        public void it_shows_missing_rows()
        {
            ExecuteText("Scenario: Grid missing",
                        "Then the objects should look like this",
                        "  | String Property | Int Property |",
                        "  |             foo |            1 |",
                        "  |             bar |            2 |",
                        "  |             baz |            3 |");

            TheScenario.Passed.Should().Be.False();
            ((Grid)TheScenario.Steps[0].Block).GetValue(0, 0).Should().Be("foo");
            ((Grid)TheScenario.Steps[0].Block).GetValue(0, 1).Should().Be("1");
            ((Grid)TheScenario.Steps[0].Block).GetValue(1, 0).Should().Be("bar");
            ((Grid)TheScenario.Steps[0].Block).GetValue(1, 1).Should().Be("2");
            ((Grid)TheScenario.Steps[0].Block).GetValue(2, 0).Should().Be("(missing) baz");
            ((Grid)TheScenario.Steps[0].Block).GetValue(2, 1).Should().Be("3");
        }

        public void then_the_object_should_look_like_this(out MyObject theObject)
        {
            theObject = new MyObject { StringProperty = "foo", IntProperty = 1 };
        }

        public void then_the_objects_should_look_like_this(out List<MyObject> theObjects)
        {
            theObjects = new List<MyObject>
                             {
                                 new MyObject { StringProperty = "foo", IntProperty = 1 },
                                 new MyObject { StringProperty = "bar", IntProperty = 2 },
                             };
        }
    }
}
