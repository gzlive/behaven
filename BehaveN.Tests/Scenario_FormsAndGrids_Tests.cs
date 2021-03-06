using System.Collections.Generic;
using NUnit.Framework;
using SharpTestsEx;

namespace BehaveN.Tests
{
    [TestFixture]
    public class Scenario_FormsAndGrids_Tests : BaseScenarioTests
    {
        [SetUp]
        public void SetUp()
        {
            _object = null;
            _objects = null;
        }

        private MyObject _object;
        private List<MyObject> _objects;

        [Test]
        public void it_converts_a_form_into_a_single_object()
        {
            ExecuteText("Scenario: Form",
                        "Given an object",
                        "  : String Property : foo",
                        "  :    Int Property : 1");

            _object.Should().Not.Be.Null();
            _object.StringProperty.Should().Be("foo");
            _object.IntProperty.Should().Be(1);
        }

        [Test]
        public void it_converts_a_grid_into_a_list_of_objects()
        {
            ExecuteText("Scenario: Grid",
                        "Given a list of objects",
                        "  | String Property | Int Property |",
                        "  |             foo |            1 |",
                        "  |             bar |            2 |");

            _objects.Should().Not.Be.Null();
            _objects.Count.Should().Be(2);
            _objects[0].StringProperty.Should().Be("foo");
            _objects[0].IntProperty.Should().Be(1);
            _objects[1].StringProperty.Should().Be("bar");
            _objects[1].IntProperty.Should().Be(2);
        }

        [Test]
        public void it_fails_when_a_form_property_cannot_be_set()
        {
            ExecuteText("Scenario: Form",
                        "Given an object",
                        "  :  String Property : foo",
                        "  :     Int Property : 1",
                        "  : Missing Property : xxx");

            TheScenario.Steps[0].Result.Should().Be(StepResult.Failed);
            _object.Should().Be.Null();
        }

        [Test]
        public void it_fails_when_a_grid_property_cannot_be_set()
        {
            ExecuteText("Scenario: Grid",
                        "Given a list of objects",
                        "  | String Property | Int Property | Unknown Property |",
                        "  |             foo |            1 |              xxx |",
                        "  |             bar |            2 |              xxx |");

            TheScenario.Steps[0].Result.Should().Be(StepResult.Failed);
            _objects.Should().Be.Null();
        }

        public void given_an_object(MyObject theObject)
        {
            _object = theObject;
        }

        public void given_a_list_of_objects(List<MyObject> theObjects)
        {
            _objects = theObjects;
        }
    }

    public class MyObject
    {
        public string StringProperty { get; set; }
        public int IntProperty { get; set; }
    }
}
