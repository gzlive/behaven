using System;

namespace BehaveN
{
    /// <summary>
    /// Represents a step.
    /// </summary>
    public abstract class Step
    {
        /// <summary>
        /// Executes the step.
        /// </summary>
        /// <param name="specifications">The specifications.</param>
        public abstract void Execute(Specifications specifications);

        /// <summary>
        /// Skips the step.
        /// </summary>
        /// <param name="specifications">The specifications.</param>
        public abstract void Skip(Specifications specifications);
    }

    internal class DescriptionStep : Step
    {
        private readonly StepType _stepType;
        private readonly string _description;

        public DescriptionStep(StepType stepType, string description)
        {
            _stepType = stepType;
            _description = description;
        }

        public override void Execute(Specifications specifications)
        {
            specifications.ExecuteStep(_stepType, _description);
        }

        public override void Skip(Specifications specifications)
        {
            specifications.ReportSkipped(_stepType, _description);
        }
    }

    internal class TextStep : Step
    {
        private readonly StepType _stepType;
        private readonly string _description;
        private readonly IConvertibleObject _convertibleObject;

        public TextStep(StepType stepType, string description, IConvertibleObject convertibleObject)
        {
            _stepType = stepType;
            _description = description;
            _convertibleObject = convertibleObject;
        }

        public override void Execute(Specifications specifications)
        {
            specifications.ExecuteStep(_stepType, _description, _convertibleObject);
        }

        public override void Skip(Specifications specifications)
        {
            if (specifications.IsStepDefined(_stepType, _description, _convertibleObject))
            {
                specifications.ReportSkipped(_stepType, _description);
            }
            else
            {
                specifications.ReportUndefined(_stepType, _description, _convertibleObject);
            }

            if (_convertibleObject != null)
            {
                specifications.ReportConvertibleObject(_convertibleObject);
            }
        }
    }
}
