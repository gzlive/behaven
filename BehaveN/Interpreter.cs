using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace BehaveN
{
    /// <summary>
    /// Represents an object with step methods.
    /// </summary>
    public class Interpreter
    {
        private object _target;
        private List<StepMethod> _stepsMethods = new List<StepMethod>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Interpreter"/> class.
        /// </summary>
        /// <param name="target">The target.</param>
        public Interpreter(object target)
        {
            _target = target;

            FindStepMethods();
        }

        private void FindStepMethods()
        {
            foreach (MethodInfo mi in _target.GetType().GetMethods())
            {
                if (mi.DeclaringType.Namespace != "BehaveN")
                    AddStepMethods(mi);
            }
        }

        private void AddStepMethods(MethodInfo method)
        {
            StepType implicitStepType = GetImplicitStepType(method);

            if (implicitStepType != StepType.Unspecified)
            {
                _stepsMethods.Add(GetStepMethod(method, implicitStepType));
            }
        }

        private static StepType GetImplicitStepType(MethodInfo method)
        {
            string parsedName = NameParser.Parse(method, false);
            if (parsedName.StartsWith("given ", StringComparison.OrdinalIgnoreCase)) return StepType.Given;
            if (parsedName.StartsWith("when ", StringComparison.OrdinalIgnoreCase)) return StepType.When;
            if (parsedName.StartsWith("then ", StringComparison.OrdinalIgnoreCase)) return StepType.Then;
            return StepType.Unspecified;
        }

        private StepMethod GetStepMethod(MethodInfo method, StepType stepType)
        {
            return new StepMethod(_target, method, stepType, GetRegexForMethod(null, method));
        }

        private Regex GetRegexForMethod(string pattern, MethodInfo method)
        {
            string description;

            if (!string.IsNullOrEmpty(pattern))
            {
                description = pattern;
            }
            else
            {
                description = PatternMaker.GetPattern(method);
            }

            return new Regex("^" + description + "$", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Invokes the matching step method.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="stepType">Type of the step.</param>
        /// <param name="convertibleObject">The convertible object.</param>
        /// <returns>Whether the step method was invoked or not.</returns>
        public bool InvokeMatchingStepMethod(ref string description, StepType stepType, IConvertibleObject convertibleObject)
        {
            foreach (StepMethod stepMethod in _stepsMethods)
            {
                if (stepMethod.InvokeIfMatching(ref description, stepType, convertibleObject))
                {
                    return true;
                }
            }

            return false;
        }

        internal bool HasMatchingStepMethod(StepType stepType, string description, IConvertibleObject convertibleObject)
        {
            foreach (StepMethod stepMethod in _stepsMethods)
            {
                if (stepMethod.Matches(stepType, description, convertibleObject))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
