using System.Collections.Generic;
using NUnit.Framework;

namespace BehaveN.Tests
{
    [TestFixture]
    public class Scenario_FormsAndGridsAsOutputs_Tests : BaseTests
    {
        [Test]
        public void it_passes_when_all_of_the_properties_are_correct()
        {
            VerifyText("Then the object should look like this",
                       "  : String Property : foo",
                       "  :    Int Property : 1");

            ShouldHavePassed();
            OutputShouldBe("  Then the object should look like this",
                           "    : String Property : foo",
                           "    :    Int Property : 1");
        }

        [Test]
        public void it_shows_all_the_values_that_are_not_correct_on_the_form()
        {
            VerifyText("Then the object should look like this",
                       "  : String Property : baz",
                       "  :    Int Property : 3");

            ShouldHaveFailed();
            OutputShouldBe("! Then the object should look like this",
                           "    : String Property : baz (was foo)",
                           "    :    Int Property : 3 (was 1)");
        }

        [Test]
        public void it_shows_all_the_values_that_are_not_correct_on_the_grid()
        {
            VerifyText("Then the objects should look like this",
                       "  | String Property | Int Property |",
                       "  |             baz |            3 |",
                       "  |            quux |            4 |");

            ShouldHaveFailed();
            OutputShouldBe("! Then the objects should look like this",
                           "    | String Property | Int Property |",
                           "    |   baz (was foo) |    3 (was 1) |",
                           "    |  quux (was bar) |    4 (was 2) |");
        }

        [Test]
        public void it_shows_extra_rows()
        {
            VerifyText("Then the objects should look like this",
                       "  | String Property | Int Property |",
                       "  |             foo |            1 |");

            ShouldHaveFailed();
            OutputShouldBe("! Then the objects should look like this",
                           "    |  String Property | Int Property |",
                           "    |              foo |            1 |",
                           "    | (unexpected) bar |            2 |");
        }

        [Test]
        public void it_shows_missing_rows()
        {
            VerifyText("Then the objects should look like this",
                       "  | String Property | Int Property |",
                       "  |             foo |            1 |",
                       "  |             bar |            2 |",
                       "  |             baz |            3 |");

            ShouldHaveFailed();
            OutputShouldBe("! Then the objects should look like this",
                           "    | String Property | Int Property |",
                           "    |             foo |            1 |",
                           "    |             bar |            2 |",
                           "    |   (missing) baz |            3 |");
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
