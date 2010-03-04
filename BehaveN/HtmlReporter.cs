using System.Collections.Generic;
using System.IO;

namespace BehaveN
{
    /// <summary>
    /// Represents a reporter that outputs HTML.
    /// </summary>
    public class HtmlReporter : Reporter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlReporter"/> class.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public HtmlReporter(TextWriter writer)
        {
            _writer = writer;
        }

        private TextWriter _writer;

        /// <summary>
        /// Reports the specifications file.
        /// </summary>
        /// <param name="specificationsFile">The specifications file.</param>
        public override void ReportSpecificationsFile(SpecificationsFile specificationsFile)
        {
            foreach (Scenario scenario in specificationsFile.Scenarios)
            {
                ReportScenario(scenario);

                WriteDivider();
            }

            ReportUndefinedSteps(specificationsFile.GetUndefinedSteps());
        }

        /// <summary>
        /// Reports the scenario.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        public override void ReportScenario(Scenario scenario)
        {
            _writer.WriteLine("<h2>Scenario: {0}</h2>", scenario.Name);

            foreach (Step step in scenario.Steps)
            {
                switch (step.Result)
                {
                    case StepResult.Passed:
                        ReportPassed(step);
                        break;
                    case StepResult.Failed:
                        ReportFailed(step);
                        break;
                    case StepResult.Undefined:
                        ReportUndefined(step);
                        break;
                    case StepResult.Pending:
                        ReportPending(step);
                        break;
                    case StepResult.Skipped:
                        ReportSkipped(step);
                        break;
                }

                if (step.Block != null)
                    ReportBlock(step.Block);
            }

            ReportException(scenario);
        }

        private void WriteDivider()
        {
            _writer.WriteLine("<hr />");
        }

        private void ReportException(Scenario scenario)
        {
            if (scenario.Exception != null)
            {
                _writer.WriteLine("<p style='color: red'>{0}</p>", scenario.Exception.Message);

                if (!string.IsNullOrEmpty(scenario.Exception.StackTrace))
                {
                    _writer.WriteLine("<pre>{0}</pre>", scenario.Exception.StackTrace);
                }
            }
        }

        /// <summary>
        /// Reports the undefined steps.
        /// </summary>
        /// <param name="undefinedSteps">The undefined steps.</param>
        public override void ReportUndefinedSteps(ICollection<Step> undefinedSteps)
        {
            if (undefinedSteps.Count > 0)
            {
                _writer.WriteLine("<p>Your undefined steps can be defined with the following code:</p>");

                foreach (Step undefinedStep in undefinedSteps)
                {
                    ReportUndefinedStep(undefinedStep);
                }
            }
        }

        private void ReportUndefined(Step step)
        {
            ReportStatus(step, "red");
        }

        private void ReportPending(Step step)
        {
            ReportStatus(step, "red");
        }

        private void ReportPassed(Step step)
        {
            ReportStatus(step, "green");
        }

        private void ReportFailed(Step step)
        {
            ReportStatus(step, "red");
        }

        private void ReportSkipped(Step step)
        {
            ReportStatus(step, "yellow");
        }

        private void ReportStatus(Step step, string color)
        {
            _writer.WriteLine("<div style='color: {1}'>{0}</div>", step.Text, color);
        }

        private void ReportBlock(IBlock block)
        {
            // TODO: Figure out if formatting should be done here or in the block object.
            //_writer.Write(block.Format());
        }

        private void ReportUndefinedStep(Step undefinedStep)
        {
            string code = UndefinedStepDefinitionHelper.GetUndefinedStepCode(undefinedStep);

            _writer.WriteLine("<pre>{0}</pre>", code);
        }
    }
}
