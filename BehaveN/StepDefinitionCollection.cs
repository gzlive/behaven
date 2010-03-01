using System;
using System.Collections.Generic;
using System.Reflection;

namespace BehaveN
{
    /// <summary>
    /// Represents a collection of StepDefinition objects.
    /// </summary>
    public class StepDefinitionCollection
    {
        private readonly List<object> _stepDefinitionObjects = new List<object>();
        private readonly List<StepDefinition> _stepDefinitions = new List<StepDefinition>();
        private readonly Dictionary<Type, object> _context = new Dictionary<Type, object>();

        /// <summary>
        /// Uses the step definitions from the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        public void UseStepDefinitionsFromAssembly(Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (TypeHasStepDefinitions(type))
                {
                    UseStepDefinitionsFromType(type);
                }
            }
        }

        private bool TypeHasStepDefinitions(Type type)
        {
            foreach (MethodInfo mi in type.GetMethods())
            {
                if (NameParser.IsStepDefinition(mi))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Uses the step definitions from the specified type.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        public void UseStepDefinitionsFromType<T>()
        {
            UseStepDefinitionsFromType(typeof(T));
        }

        /// <summary>
        /// Uses the step definitions from the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        public void UseStepDefinitionsFromType(Type type)
        {
            _stepDefinitionObjects.Add(type);
        }

        /// <summary>
        /// Uses the step definitions from the specified object.
        /// </summary>
        /// <param name="object">The @object.</param>
        public void UseStepDefinitionsFromObject(object @object)
        {
            _stepDefinitionObjects.Add(@object);
        }

        /// <summary>
        /// Creates the context including all of the step definitions.
        /// </summary>
        public void CreateContext()
        {
            _context.Clear();
            _stepDefinitions.Clear();

            foreach (object @object in _stepDefinitionObjects)
            {
                if (@object is Type)
                {
                    _stepDefinitions.AddRange(GetStepDefinitionsFrom(CreateStepDefinitionsObject((Type)@object)));
                }
                else
                {
                    _stepDefinitions.AddRange(GetStepDefinitionsFrom(@object));
                }
            }
        }

        private object CreateStepDefinitionsObject(Type type)
        {
            ConstructorInfo[] constructors = type.GetConstructors();

            if (constructors.Length < 1)
                throw new ArgumentException(string.Format("{0} has no constructors.", type.FullName));

            if (constructors.Length > 1)
                throw new ArgumentException(string.Format("{0} has more than one constructor.", type.FullName));

            ConstructorInfo constructor = constructors[0];

            if (constructor.GetParameters().Length == 0)
                return Activator.CreateInstance(type);

            object[] parameters = new object[constructor.GetParameters().Length];

            int i = 0;

            foreach (ParameterInfo pi in constructor.GetParameters())
            {
                parameters[i++] = CreateOrGetContextObject(pi.ParameterType);
            }

            return Activator.CreateInstance(type, parameters);
        }

        private IEnumerable<StepDefinition> GetStepDefinitionsFrom(object target)
        {
            foreach (MethodInfo mi in target.GetType().GetMethods())
            {
                if (mi.DeclaringType.Namespace != "BehaveN" && NameParser.IsStepDefinition(mi))
                    yield return new StepDefinition(target, mi);
            }
        }

        private object CreateOrGetContextObject(Type type)
        {
            object contextObject;

            if (_context.TryGetValue(type, out contextObject))
                return contextObject;

            contextObject = Activator.CreateInstance(type);
            _context[type] = contextObject;

            return contextObject;
        }

        /// <summary>
        /// Tries to execute a step.
        /// </summary>
        /// <param name="step">The step.</param>
        /// <returns>True if the step was executed.</returns>
        public bool TryExecute(Step step)
        {
            foreach (StepDefinition stepDefinition in _stepDefinitions)
            {
                if (stepDefinition.TryExecute(step))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether there is a match for the specified step.
        /// </summary>
        /// <param name="step">The step.</param>
        /// <returns>
        /// 	<c>true</c> if there is a match; otherwise, <c>false</c>.
        /// </returns>
        public bool HasMatchFor(Step step)
        {
            foreach (StepDefinition stepDefinition in _stepDefinitions)
            {
                if (stepDefinition.Matches(step))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Registers the context object.
        /// </summary>
        /// <param name="contextObject">The context object.</param>
        public void RegisterContextObject(object contextObject)
        {
            _context.Add(contextObject.GetType(), contextObject);
        }

        /// <summary>
        /// Registers the context object.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="contextObject">The context object.</param>
        public void RegisterContextObject(Type type, object contextObject)
        {
            _context.Add(type, contextObject);
        }

        /// <summary>
        /// Gets the context object.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public object GetContextObject(Type type)
        {
            return _context[type];
        }
    }
}