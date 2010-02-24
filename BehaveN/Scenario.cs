using System;
using System.Collections.Generic;
using System.Reflection;

namespace BehaveN
{
    /// <summary>
    /// Represents a scenario.
    /// </summary>
    public class Scenario
    {
        private string _name;
        private StepDefinitionCollection _stepDefinitions = new StepDefinitionCollection();
        private readonly List<Step> _steps = new List<Step>();
        private bool _passed;
        private Exception _exception;

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Gets or sets the step definitions.
        /// </summary>
        /// <value>The step definitions.</value>
        public StepDefinitionCollection StepDefinitions
        {
            get { return _stepDefinitions; }
            set { _stepDefinitions = value; }
        }

        /// <summary>
        /// Gets the steps.
        /// </summary>
        /// <value>The steps.</value>
        public IList<Step> Steps
        {
            get { return _steps; }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Scenario"/> is passed.
        /// </summary>
        /// <value><c>true</c> if passed; otherwise, <c>false</c>.</value>
        public bool Passed
        {
            get { return _passed; }
        }

        /// <summary>
        /// Gets the exception.
        /// </summary>
        /// <value>The exception.</value>
        public Exception Exception
        {
            get { return _exception; }
        }

        internal void Add(string keyword, string step, IBlock block)
        {
            Step newStep = new Step();
            newStep.Keyword = keyword;
            newStep.Text = step;
            newStep.Block = block;
            _steps.Add(newStep);
        }

        /// <summary>
        /// Verifies the scenario.
        /// </summary>
        public void Verify()
        {
            _passed = true;
            _exception = null;

            int i = 0;

            try
            {
                for (i = 0; i < _steps.Count; i++)
                {
                    Step step = _steps[i];

                    if (!_stepDefinitions.TryExecute(step))
                    {
                        _passed = false;
                        step.Result = StepResult.Undefined;
                        break;
                    }

                    step.Result = StepResult.Passed;
                }
            }
            catch (Exception e)
            {
                _passed = false;
                _exception = e;

                if (e is TargetInvocationException)
                    _exception = e.InnerException;

                if (_exception is NotImplementedException)
                    _steps[i].Result = StepResult.Pending;
                else
                    _steps[i].Result = StepResult.Failed;
            }
            finally
            {
                for (i += 1; i < _steps.Count; i++)
                {
                    Step step = _steps[i];

                    if (_stepDefinitions.HasMatchFor(step))
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
}
