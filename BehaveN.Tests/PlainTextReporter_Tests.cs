using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using SharpTestsEx;

namespace BehaveN.Tests
{
    [TestFixture]
    public class PlainTextReporter_Tests
    {
        [SetUp]
        public void SetUp()
        {
            sw = new StringWriter();
            reporter = new PlainTextReporter(sw);
            s = new Scenario();
        }

        private StringWriter sw;
        private Reporter reporter;
        private Scenario s;

        [Test]
        public void it_reports_the_scenario_name()
        {
            s.Name = "My scenario";

            ScenarioReportShouldBe("Scenario: My scenario",
                                   "");
        }

        [Test]
        public void it_reports_passed_steps_with_a_leading_space()
        {
            s.Name = "My scenario";
            s.Steps.Add(new Step { Text = "Given passed", Result = StepResult.Passed });

            ScenarioReportShouldBe("Scenario: My scenario",
                                   "",
                                   "  Given passed");
        }

        [Test]
        public void it_reports_failed_steps_with_a_leading_bang()
        {
            s.Name = "My scenario";
            s.Steps.Add(new Step { Text = "Given failed", Result = StepResult.Failed });

            ScenarioReportShouldBe("Scenario: My scenario",
                                   "",
                                   "! Given failed");
        }

        [Test]
        public void it_reports_undefined_steps_with_a_leading_question_mark()
        {
            s.Name = "My scenario";
            s.Steps.Add(new Step { Text = "Given undefined", Result = StepResult.Undefined });

            ScenarioReportShouldBe("Scenario: My scenario",
                                   "",
                                   "? Given undefined");
        }

        [Test]
        public void it_reports_pending_steps_with_a_leading_asterisk()
        {
            s.Name = "My scenario";
            s.Steps.Add(new Step { Text = "Given pending", Result = StepResult.Pending });

            ScenarioReportShouldBe("Scenario: My scenario",
                                   "",
                                   "* Given pending");
        }

        [Test]
        public void it_reports_skipped_steps_with_a_leading_dash()
        {
            s.Name = "My scenario";
            s.Steps.Add(new Step { Text = "Given skipped", Result = StepResult.Skipped });

            ScenarioReportShouldBe("Scenario: My scenario",
                                   "",
                                   "- Given skipped");
        }

        [Test]
        public void it_suggests_methods_for_undefined_steps()
        {
            var undefinedSteps = new List<Step>
                                     {
                                         new Step { Keyword = "given", Text = "Given undefined", Result = StepResult.Undefined }
                                     };

            ReportUndefinedStepsOutputShouldBe(undefinedSteps, "Your undefined steps can be defined with the following code:",
                                                               "",
                                                               "public void given_undefined()",
                                                               "{",
                                                               "    throw new NotImplementedException();",
                                                               "}");
        }

        [Test]
        public void it_does_not_suggest_methods_beginning_with_and_for_undefined_steps()
        {
            var undefinedSteps = new List<Step>
                                     {
                                         new Step { Keyword = "given", Text = "Given undefined", Result = StepResult.Undefined },
                                         new Step { Keyword = "given", Text = "And another undefined", Result = StepResult.Undefined }
                                     };

            ReportUndefinedStepsOutputShouldBe(undefinedSteps, "Your undefined steps can be defined with the following code:",
                                                               "",
                                                               "public void given_undefined()",
                                                               "{",
                                                               "    throw new NotImplementedException();",
                                                               "}",
                                                               "",
                                                               "public void given_another_undefined()",
                                                               "{",
                                                               "    throw new NotImplementedException();",
                                                               "}");
        }

        private void ScenarioReportShouldBe(params string[] lines)
        {
            reporter.ReportScenario(s);

            string actual = sw.GetStringBuilder().ToString();
            string expected = string.Join("\r\n", lines) + "\r\n\r\n";

            actual.Should().Be(expected);
        }

        private void ReportUndefinedStepsOutputShouldBe(ICollection<Step> steps, params string[] lines)
        {
            reporter.ReportUndefinedSteps(steps);

            string actual = sw.GetStringBuilder().ToString();
            string expected = string.Join("\r\n", lines) + "\r\n\r\n";

            actual.Should().Be(expected);
        }
    }
}
