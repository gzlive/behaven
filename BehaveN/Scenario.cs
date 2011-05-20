// <copyright file="Scenario.cs" company="Jason Diamond">
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
    using System.Reflection;

    /// <summary>
    /// Represents a scenario.
    /// </summary>
    public class Scenario
    {
        private string name;
        private StepDefinitionCollection stepDefinitions = new StepDefinitionCollection();
        private StepCollection steps = new StepCollection();
        private bool passed;
        private Exception exception;
        private Reporter reporter;

        /// <summary>
        /// Gets or sets the scenario name.
        /// </summary>
        /// <value>The name of the scenario.</value>
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        /// <summary>
        /// Gets or sets the step definitions.
        /// </summary>
        /// <value>The step definitions.</value>
        public StepDefinitionCollection StepDefinitions
        {
            get { return this.stepDefinitions; }
            set { this.stepDefinitions = value; }
        }

        /// <summary>
        /// Gets the steps.
        /// </summary>
        /// <value>The steps.</value>
        public StepCollection Steps
        {
            get { return this.steps; }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Scenario"/> is passed.
        /// </summary>
        /// <value><c>true</c> if passed; otherwise, <c>false</c>.</value>
        public bool Passed
        {
            get { return this.passed; }
        }

        /// <summary>
        /// Gets the exception.
        /// </summary>
        /// <value>The exception.</value>
        public Exception Exception
        {
            get { return this.exception; }
        }

        /// <summary>
        /// Gets or sets the reporter.
        /// </summary>
        /// <value>The reporter.</value>
        public Reporter Reporter
        {
            get { return this.reporter ?? (this.reporter = new DefaultReporter()); }
            set { this.reporter = value; }
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        public void Execute()
        {
            this.PrepareToExecute();

            int lastStepIndex = 0;

            try
            {
                this.ExecuteSteps(out lastStepIndex);
            }
            catch (Exception e)
            {
                this.SaveException(e);
                this.SetFailedStepResult(lastStepIndex);
            }
            finally
            {
                this.CheckForMoreUndefinedSteps(lastStepIndex + 1);
                this.CleanUpAfterExecuting();
            }
        }

        /// <summary>
        /// Reports this instance.
        /// </summary>
        public void Report()
        {
            Reporter.ReportScenario(this);
        }

        /// <summary>
        /// Executes and reports this scenario. Throws an exception if
        /// all of the steps don't pass.
        /// </summary>
        public void Verify()
        {
            this.Execute();
            this.Report();

            if (!this.Passed)
            {
                if (this.exception != null)
                {
                    throw new VerificationException(this.exception);
                }

                int undefinedStepCount = 0;

                foreach (var step in this.steps)
                {
                    if (step.Result == StepResult.Undefined)
                    {
                        undefinedStepCount++;
                    }
                }

                if (undefinedStepCount > 0)
                {
                    string message = string.Format("Scenario has {0} undefined step(s).", undefinedStepCount);
                    throw new VerificationException(new Exception(message));
                }

                throw new VerificationException(new Exception("Scenario failed."));
            }
        }

        private void PrepareToExecute()
        {
            this.StepDefinitions.CreateContext();

            this.passed = true;
            this.exception = null;
        }

        private void CleanUpAfterExecuting()
        {
            this.StepDefinitions.Dispose();
        }

        private void ExecuteSteps(out int lastStepIndex)
        {
            for (lastStepIndex = 0; lastStepIndex < this.steps.Count; lastStepIndex++)
            {
                Step step = this.steps[lastStepIndex];

                try
                {
                    if (!this.stepDefinitions.TryExecute(step))
                    {
                        this.passed = false;
                        step.Result = StepResult.Undefined;
                        break;
                    }

                    step.Result = StepResult.Passed;
                }
                catch (Exception e)
                {
                    if (!NextStepCanHandleException(lastStepIndex))
                    {
                        throw;
                    }

                    step.Result = StepResult.Passed;

                    this.steps[lastStepIndex + 1].Exception = GetRealException(e);
                }
            }
        }

        private bool NextStepCanHandleException(int lastStepIndex)
        {
            if ((lastStepIndex + 1) < this.steps.Count)
            {
                Step step = this.steps[lastStepIndex + 1];
                return this.stepDefinitions.CanHandleException(step);
            }

            return false;
        }

        private void SaveException(Exception e)
        {
            this.passed = false;
            this.exception = GetRealException(e);
        }

        private static Exception GetRealException(Exception e)
        {
            return e is TargetInvocationException ? e.InnerException : e;
        }

        private void SetFailedStepResult(int stepIndex)
        {
            if (this.exception is NotImplementedException)
            {
                this.steps[stepIndex].Result = StepResult.Pending;
            }
            else
            {
                this.steps[stepIndex].Result = StepResult.Failed;
            }
        }

        private void CheckForMoreUndefinedSteps(int stepIndex)
        {
            for (; stepIndex < this.steps.Count; stepIndex++)
            {
                Step step = this.steps[stepIndex];

                if (this.stepDefinitions.HasMatchFor(step))
                {
                    step.Result = StepResult.Skipped;
                }
                else
                {
                    step.Result = StepResult.Undefined;
                }
            }
        }
    }
}
