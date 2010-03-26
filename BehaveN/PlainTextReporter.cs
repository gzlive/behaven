// <copyright file="PlainTextReporter.cs" company="Jason Diamond">
//
// Copyright (c) 2009-2010 Jason Diamond
//
// This source code is released under the MIT License.
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use, copy,
// modify, merge, publish, distribute, sublicense, and/or sell copies
// of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.
//
// </copyright>

namespace BehaveN
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.RegularExpressions;

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

        private readonly TextWriter writer;
        private StepType lastStepType;

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
            this.writer = writer;
        }

        /// <summary>
        /// Reports the feature.
        /// </summary>
        /// <param name="feature">The feature.</param>
        /// <remarks>This reports all scenarios in the file and their
        /// undefined steps.</remarks>
        public override void ReportFeature(Feature feature)
        {
            foreach (Scenario scenario in feature.Scenarios)
            {
                this.ReportScenario(scenario);

                this.WriteDivider();
            }

            this.ReportUndefinedSteps(feature.GetUndefinedSteps());
        }

        /// <summary>
        /// Reports the scenario.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        public override void ReportScenario(Scenario scenario)
        {
            this.writer.WriteLine("Scenario: {0}", scenario.Name);
            this.writer.WriteLine();

            foreach (Step step in scenario.Steps)
            {
                switch (step.Result)
                {
                    case StepResult.Passed:
                        this.ReportPassed(step);
                        break;
                    case StepResult.Failed:
                        this.ReportFailed(step);
                        break;
                    case StepResult.Undefined:
                        this.ReportUndefined(step);
                        break;
                    case StepResult.Pending:
                        this.ReportPending(step);
                        break;
                    case StepResult.Skipped:
                        this.ReportSkipped(step);
                        break;
                }

                if (step.Block != null)
                {
                    this.ReportBlock(step.Block);
                }
            }

            this.writer.WriteLine();

            this.ReportException(scenario);
        }

        /// <summary>
        /// Reports the undefined steps.
        /// </summary>
        /// <param name="undefinedSteps">The undefined steps.</param>
        public override void ReportUndefinedSteps(ICollection<Step> undefinedSteps)
        {
            if (undefinedSteps.Count > 0)
            {
                this.writer.WriteLine("Your undefined steps can be defined with the following code:");
                this.writer.WriteLine();

                foreach (Step undefinedStep in undefinedSteps)
                {
                    this.ReportUndefinedStep(undefinedStep);
                }
            }
        }

        private void WriteDivider()
        {
            this.writer.WriteLine("---");
            this.writer.WriteLine();
        }

        private void ReportException(Scenario scenario)
        {
            if (scenario.Exception != null)
            {
                this.writer.WriteLine(scenario.Exception.Message);

                if (!string.IsNullOrEmpty(scenario.Exception.StackTrace))
                {
                    this.writer.WriteLine();
                    this.writer.WriteLine(this.GetStackTraceThatIsClickableInOutputWindow(scenario.Exception));
                }

                this.writer.WriteLine();
            }
        }

        private string GetStackTraceThatIsClickableInOutputWindow(Exception e)
        {
            return Regex.Replace(e.StackTrace, @"  at (.+) in (.+):line (\d+)", "$2($3): $1");
        }

        private void ReportUndefined(Step step)
        {
            this.ReportStatus(step, Undefined);
        }

        private void ReportPending(Step step)
        {
            this.ReportStatus(step, Pending);
        }

        private void ReportPassed(Step step)
        {
            this.ReportStatus(step, Passed);
        }

        private void ReportFailed(Step step)
        {
            this.ReportStatus(step, Failed);
        }

        private void ReportSkipped(Step step)
        {
            this.ReportStatus(step, Skipped);
        }

        private void ReportStatus(Step step, string status)
        {
            if (this.lastStepType != StepType.Unknown && step.Type != this.lastStepType)
            {
                this.writer.WriteLine();
            }

            this.writer.WriteLine(status + " " + step.Text);

            this.lastStepType = step.Type;
        }

        private void ReportBlock(IBlock block)
        {
            this.writer.Write(block.Format());
        }

        private void ReportUndefinedStep(Step undefinedStep)
        {
            string code = UndefinedStepDefinitionHelper.GetUndefinedStepCode(undefinedStep);
            this.writer.WriteLine(code);
            this.writer.WriteLine();
        }
    }
}
