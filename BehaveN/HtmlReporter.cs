using System;
using System.Collections.Generic;
using System.IO;

namespace BehaveN
{
    /// <summary>
    /// Represents a reporter that outputs HTML.
    /// </summary>
    public class HtmlReporter : Reporter, IBlockReporter<Form>, IBlockReporter<Grid>
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

            _writer.Flush();
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

            _writer.Flush();
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

                _writer.Flush();
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
            ReportStatus(step, "grey");
        }

        private void ReportStatus(Step step, string color)
        {
            _writer.WriteLine("<div style='color: {1}'>{0}</div>", step.Text, color);
        }

        private void ReportBlock(IBlock block)
        {
            block.ReportTo(this);
        }

        private void ReportUndefinedStep(Step undefinedStep)
        {
            string code = UndefinedStepDefinitionHelper.GetUndefinedStepCode(undefinedStep);

            _writer.WriteLine("<pre>{0}</pre>", Escape(code));
        }

        private static string Escape(string text)
        {
            return text.Replace("&", "&amp;").Replace("<", "&lt;");
        }

        void IBlockReporter<Form>.ReportBlock(Form block)
        {
            _writer.WriteLine("<table border='1'>");

            for (int i = 0; i < block.Size; i++)
            {
                string label = block.GetLabel(i);
                string value = block.GetValue(i);
                _writer.WriteLine("<tr><th>{0}</th><td>{1}</td></tr>", label, value);
            }

            _writer.WriteLine("</table>");
        }

        void IBlockReporter<Grid>.ReportBlock(Grid block)
        {
            _writer.WriteLine("<table border='1'>");

            _writer.WriteLine("<tr>");

            for (int i = 0; i < block.ColumnCount; i++)
            {
                string header = block.GetHeader(i);
                _writer.WriteLine("<th>{0}</th>", header);
            }

            _writer.WriteLine("</tr>");

            for (int i = 0; i < block.RowCount; i++)
            {
                _writer.WriteLine("<tr>");

                for (int j = 0; j < block.ColumnCount; j++)
                {
                    string value = block.GetValue(i, j);
                    _writer.WriteLine("<td>{0}</td>", value);
                }

                _writer.WriteLine("</tr>");
            }

            _writer.WriteLine("</table>");
        }
    }
}
