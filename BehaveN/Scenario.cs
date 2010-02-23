using System;

namespace BehaveN
{
    /// <summary>
    /// Represents the verification engine for Given/When/Then-style specifications.
    /// </summary>
    public class Scenario : Specifications
    {
        private StepType _lastStepType;

        /// <summary>
        /// Initializes a new instance of the <see cref="Scenario"/> class.
        /// </summary>
        public Scenario()
        {
        }

        #region Methods for adding steps

        /// <summary>
        /// Names the scenario.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The current <see cref="Scenario"/> object.</returns>
        public Scenario Name(string name)
        {
            OnAddingStep(StepType.Name);
            Steps.Add(StepType.Name, name);
            return this;
        }

        /// <summary>
        /// Executes a "given" step using the specified description.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <returns>The current <see cref="Scenario"/> object.</returns>
        public Scenario Given(string description)
        {
            OnAddingStep(StepType.Given);
            Steps.Add(StepType.Given, description, null);
            return this;
        }

        /// <summary>
        /// Executes a "given" step using the specified description and convertible object.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="convertibleObject">The convertible object.</param>
        /// <returns>The current <see cref="Scenario"/> object.</returns>
        public Scenario Given(string description, IConvertibleObject convertibleObject)
        {
            OnAddingStep(StepType.Given);
            Steps.Add(StepType.Given, description, convertibleObject);
            return this;
        }

        /// <summary>
        /// Executes a "when" step using the specified description.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <returns>The current <see cref="Scenario"/> object.</returns>
        public Scenario When(string description)
        {
            OnAddingStep(StepType.When);
            Steps.Add(StepType.When, description, null);
            return this;
        }

        /// <summary>
        /// Executes a "when" step using the specified description and convertible object.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="convertibleObject">The convertible object.</param>
        /// <returns>
        /// The current <see cref="Scenario"/> object.
        /// </returns>
        public Scenario When(string description, IConvertibleObject convertibleObject)
        {
            OnAddingStep(StepType.When);
            Steps.Add(StepType.When, description, convertibleObject);
            return this;
        }

        /// <summary>
        /// Executes a "then" step using the specified description.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <returns>The current <see cref="Scenario"/> object.</returns>
        public Scenario Then(string description)
        {
            OnAddingStep(StepType.Then);
            Steps.Add(StepType.Then, description, null);
            return this;
        }

        /// <summary>
        /// Executes a "then" step using the specified description and convertible object.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="convertibleObject">The convertible object.</param>
        /// <returns>
        /// The current <see cref="Scenario"/> object.
        /// </returns>
        public Scenario Then(string description, IConvertibleObject convertibleObject)
        {
            OnAddingStep(StepType.Then);
            Steps.Add(StepType.Then, description, convertibleObject);
            return this;
        }

        /// <summary>
        /// Executes a "given", "when", or "then" step using the specified description.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <returns>The current <see cref="Scenario"/> object.</returns>
        public Scenario And(string description)
        {
            return And(description, null);
        }

        /// <summary>
        /// Executes a "given", "when", or "then" step using the specified description and convertible object.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="convertibleObject">The convertible object.</param>
        /// <returns>
        /// The current <see cref="Scenario"/> object.
        /// </returns>
        public Scenario And(string description, IConvertibleObject convertibleObject)
        {
            ValidateCurrentStepType();

            if (_lastStepType == StepType.Given)
                Given(description, convertibleObject);
            else if (_lastStepType == StepType.When)
                When(description, convertibleObject);
            else
                Then(description, convertibleObject);

            return this;
        }

        private void ValidateCurrentStepType()
        {
            if (_lastStepType != StepType.Given && _lastStepType != StepType.When && _lastStepType != StepType.Then)
                throw new InvalidOperationException("Use Given, When, or Then before And!");
        }

        #endregion

        /// <summary>
        /// Gets the default reporter.
        /// </summary>
        /// <returns>A <see cref="Reporter"/> object.</returns>
        protected override Reporter  GetDefaultReporter()
        {
            return new PlainTextScenarioReporter();
        }

        private void OnAddingStep(StepType stepType)
        {
            if (_lastStepType != stepType)
            {
                _lastStepType = stepType;
            }
        }

        /// <summary>
        /// Reads the specifications in the text.
        /// </summary>
        /// <param name="text">The text.</param>
        protected override void ReadSpecifications(string text)
        {
            new PlainTextScenarioReader(text).ReadTo(this);
        }

        /// <summary>
        /// Called before resetting this instance.
        /// </summary>
        protected override void OnResetting()
        {
            _lastStepType = StepType.Unspecified;
        }
    }
}
