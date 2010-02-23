using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BehaveN
{
    /// <summary>
    /// Represents a plain text reporter for Scenario steps.
    /// </summary>
    public class PlainTextScenarioReporter : Reporter
    {
        private TextWriter _writer;
        private StepType _lastStepType;
        private int _stepNumber;
        private List<UndefinedStepInfo> _undefinedSteps = new List<UndefinedStepInfo>();

        private class UndefinedStepInfo
        {
            public StepType StepType;
            public string Description;
            public IConvertibleObject ConvertibleObject;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlainTextScenarioReporter"/> class.
        /// </summary>
        public PlainTextScenarioReporter() : this(Console.Out)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlainTextScenarioReporter"/> class.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public PlainTextScenarioReporter(TextWriter writer)
        {
            _writer = writer;
        }

        /// <summary>
        /// Reports the description of the scenario.
        /// </summary>
        /// <param name="description">The description.</param>
        public override void ReportDescription(string description)
        {
            _writer.WriteLine("Scenario: {0}", description);
            _lastStepType = StepType.Name;
        }

        /// <summary>
        /// Reports an undefined step.
        /// </summary>
        /// <param name="stepType">Type of the step.</param>
        /// <param name="description">The description.</param>
        /// <param name="convertibleObject">The convertible object.</param>
        public override void ReportUndefined(StepType stepType, string description, IConvertibleObject convertibleObject)
        {
            ReportStatus(stepType, description, PlainTextStepResultSymbols.Undefined);

            UndefinedStepInfo undefinedStepInfo = new UndefinedStepInfo();
            undefinedStepInfo.StepType = stepType;
            undefinedStepInfo.Description = description;
            undefinedStepInfo.ConvertibleObject = convertibleObject;
            _undefinedSteps.Add(undefinedStepInfo);
        }

        /// <summary>
        /// Reports a pending step.
        /// </summary>
        /// <param name="stepType">Type of the step.</param>
        /// <param name="description">The description.</param>
        public override void ReportPending(StepType stepType, string description)
        {
            ReportStatus(stepType, description, PlainTextStepResultSymbols.Pending);
        }

        /// <summary>
        /// Reports a passed step.
        /// </summary>
        /// <param name="stepType">Type of the step.</param>
        /// <param name="description">The description.</param>
        public override void ReportPassed(StepType stepType, string description)
        {
            ReportStatus(stepType, description, PlainTextStepResultSymbols.Passed);
        }

        /// <summary>
        /// Reports a failed step.
        /// </summary>
        /// <param name="stepType">Type of the step.</param>
        /// <param name="description">The description.</param>
        /// <param name="e">The exception thrown by the step.</param>
        public override void ReportFailed(StepType stepType, string description, Exception e)
        {
            ReportStatus(stepType, description, PlainTextStepResultSymbols.Failed);
        }

        /// <summary>
        /// Reports a skipped step.
        /// </summary>
        /// <param name="stepType">Type of the step.</param>
        /// <param name="description">The description.</param>
        public override void ReportSkipped(StepType stepType, string description)
        {
            ReportStatus(stepType, description, PlainTextStepResultSymbols.Skipped);
        }

        private void ReportStatus(StepType stepType, string description, string status)
        {
            if (stepType != _lastStepType)
            {
                if (_lastStepType != StepType.Unspecified)
                {
                    _writer.WriteLine();
                }
                
                _lastStepType = stepType;
                _stepNumber = 0;
            }
            else
            {
                _stepNumber++;
            }

            string text = status + " ";
            text += _stepNumber == 0 ? GetStepTypeDescription(stepType) : "And";
            text += " " + description;
            _writer.WriteLine(text);
        }

        private static string GetStepTypeDescription(StepType stepType)
        {
            return stepType.ToString();
        }

        /// <summary>
        /// Reports a convertible object that was associated with a step.
        /// </summary>
        /// <param name="convertibleObject">The convertible object.</param>
        public override void ReportConvertibleObject(IConvertibleObject convertibleObject)
        {
            _writer.Write(convertibleObject.Format());
        }

        /// <summary>
        /// Reports the end.
        /// </summary>
        public override void ReportEnd()
        {
            _writer.WriteLine();

            ReportUndefinedSteps();
        }

        private void ReportUndefinedSteps()
        {
            if (_undefinedSteps.Count > 0)
            {
                _writer.WriteLine("Your undefined steps can be defined with the following code:");
                _writer.WriteLine();

                foreach (UndefinedStepInfo undefinedStep in _undefinedSteps)
                {
                    ReportUndefinedStep(undefinedStep);
                }
            }

            _undefinedSteps.Clear();
        }

        private void ReportUndefinedStep(UndefinedStepInfo undefinedStep)
        {
            string methodName = GetMethodName(undefinedStep.Description);
            string parameters = GetParameters(undefinedStep.Description, undefinedStep.ConvertibleObject);

            _writer.WriteLine("public void {0}_{1}({2})", undefinedStep.StepType.ToString().ToLowerInvariant(), methodName, parameters);
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

            return sb.ToString();
        }

        private string GetParameters(string description, IConvertibleObject convertibleObject)
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

            if (convertibleObject != null)
            {
                parameters.Add(string.Format("{0} {1}", convertibleObject.GetType().Name, convertibleObject.GetType().Name.ToLower()));
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
