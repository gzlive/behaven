using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace BehaveN
{
    /// <summary>
    /// Represents a plain text reporter.
    /// </summary>
    public class PlainTextReporter : Reporter
    {
        /// <summary>
        /// The symbol for an undefined step.
        /// </summary>
        public const string Undefined = "?";

        /// <summary>
        /// The symbol for a pending step.
        /// </summary>
        public const string Pending = "*";

        /// <summary>
        /// The symbol for a passed step.
        /// </summary>
        public const string Passed = " ";

        /// <summary>
        /// The symbol for a failed step.
        /// </summary>
        public const string Failed = "!";

        /// <summary>
        /// The symbol for a skipped step.
        /// </summary>
        public const string Skipped = "-";

        /// <summary>
        /// Initializes a new instance of the <see cref="PlainTextReporter"/> class.
        /// </summary>
        public PlainTextReporter() : this(Console.Out)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlainTextReporter"/> class.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public PlainTextReporter(TextWriter writer)
        {
            _writer = writer;
        }

        private TextWriter _writer;
        private int _scenarioNumber;

        public override void Begin()
        {
        }

        public override void ReportScenario(Scenario scenario)
        {
            if (_scenarioNumber > 0)
            {
                WriteBorder();
            }

            _writer.WriteLine("Scenario: {0}", scenario.Name);
            _writer.WriteLine();

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

            _writer.WriteLine();

            ReportException(scenario);

            _scenarioNumber++;
        }

        private void WriteBorder()
        {
            _writer.WriteLine("---");
            _writer.WriteLine();
        }

        private void ReportException(Scenario scenario)
        {
            if (scenario.Exception != null)
            {
                _writer.WriteLine(scenario.Exception.Message);

                if (!string.IsNullOrEmpty(scenario.Exception.StackTrace))
                {
                    _writer.WriteLine();
                    _writer.WriteLine(GetStackTraceThatIsClickableInOutputWindow(scenario.Exception));
                }

                _writer.WriteLine();
            }
        }

        private string GetStackTraceThatIsClickableInOutputWindow(Exception e)
        {
            return Regex.Replace(e.StackTrace, @"  at (.+) in (.+):line (\d+)", "$2($3): $1");
        }

        public override void ReportUndefinedSteps(List<Step> undefinedSteps)
        {
            if (undefinedSteps.Count > 0)
            {
                _writer.WriteLine("Your undefined steps can be defined with the following code:");
                _writer.WriteLine();

                foreach (Step undefinedStep in undefinedSteps)
                {
                    ReportUndefinedStep(undefinedStep);
                }
            }
        }

        public override void End()
        {
        }

        private void ReportUndefined(Step step)
        {
            ReportStatus(step, Undefined);
        }

        private void ReportPending(Step step)
        {
            ReportStatus(step, Pending);
        }

        private void ReportPassed(Step step)
        {
            ReportStatus(step, Passed);
        }

        private void ReportFailed(Step step)
        {
            ReportStatus(step, Failed);
        }

        private void ReportSkipped(Step step)
        {
            ReportStatus(step, Skipped);
        }

        private void ReportStatus(Step step, string status)
        {
            _writer.WriteLine(status + " " + step.Text);
        }

        private void ReportBlock(IBlock block)
        {
            _writer.Write(block.Format());
        }

        private void ReportUndefinedStep(Step undefinedStep)
        {
            string methodName = GetMethodName(undefinedStep.Text);
            string parameters = GetParameters(undefinedStep.Text, undefinedStep.Block);

            _writer.WriteLine("public void {0}({1})", methodName, parameters);
            _writer.WriteLine("{");
            _writer.WriteLine("    throw new NotImplementedException();");
            _writer.WriteLine("}");
            _writer.WriteLine();
        }

        private string GetMethodName(string description)
        {
            StringBuilder sb = new StringBuilder();

            int i = 1;

            foreach (string part in Split(description))
            {
                if (sb.Length > 0)
                {
                    sb.Append("_");
                }

                if (IsInteger(part) || IsDecimal(part) || IsString(part))
                {
                    sb.AppendFormat("x{0}", i++);
                }
                else
                {
                    sb.Append(part);
                }
            }

            return sb.ToString().ToLowerInvariant();
        }

        private string GetParameters(string description, IBlock block)
        {
            List<string> parameters = new List<string>();

            int i = 1;

            foreach (string part in Split(description))
            {
                if (IsInteger(part))
                {
                    parameters.Add(string.Format("int x{0}", i++));
                }
                else if (IsDecimal(part))
                {
                    parameters.Add(string.Format("decimal x{0}", i++));
                }
                else if (IsString(part))
                {
                    parameters.Add(string.Format("string x{0}", i++));
                }
            }

            if (block != null)
            {
                parameters.Add(string.Format("{0} {1}", block.GetType().Name, block.GetType().Name.ToLower()));
            }

            return string.Join(", ", parameters.ToArray());
        }

        private string[] Split(string description)
        {
            List<string> parts = new List<string>();

            bool quoted = false;
            int start = 0;

            for (int i = 0; i < description.Length; i++)
            {
                char c = description[i];

                if (c == '\"') quoted = !quoted;

                if (!quoted && c == ' ')
                {
                    if (start < i)
                        parts.Add(description.Substring(start, i - start));

                    start = i + 1;
                }
            }

            parts.Add(description.Substring(start));

            return parts.ToArray();
        }

        private bool IsInteger(string part)
        {
            int i;
            return int.TryParse(part, out i);
        }

        private bool IsDecimal(string part)
        {
            decimal d;
            if (decimal.TryParse(part, out d))
                return true;
            if (part.Length >= 2 && part[0] == '$')
                return decimal.TryParse(part.Substring(1), out d);
            return false;
        }

        private bool IsString(string part)
        {
            return part.Length > 2 && part[0] == '\"' && part[part.Length - 1] == '\"';
        }
    }
}
