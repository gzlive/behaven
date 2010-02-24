using System;
using System.Collections.Generic;
using System.Reflection;

namespace BehaveN
{
    public class StepDefinitionCollection
    {
        private readonly List<StepDefinition> _stepDefinitions = new List<StepDefinition>();
        private readonly Dictionary<Type, object> _context = new Dictionary<Type, object>();

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
            if (type.GetConstructor(Type.EmptyTypes) == null)
            {
                return false;
            }

            foreach (MethodInfo mi in type.GetMethods())
            {
                if (NameParser.IsStepDefinition(mi))
                {
                    return true;
                }
            }

            return false;
        }

        public void UseStepDefinitionsFromType<T>()
        {
            UseStepDefinitionsFromType(typeof(T));
        }

        public void UseStepDefinitionsFromType(Type type)
        {
            UseStepDefinitionsFromObject(CreateStepDefinitionsObject(type));
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
                parameters[i++] = GetContextObject(pi.ParameterType);
            }

            return Activator.CreateInstance(type, parameters);
        }

        public void UseStepDefinitionsFromObject(object target)
        {
            _stepDefinitions.AddRange(GetStepDefinitionsFrom(target));
        }

        private IEnumerable<StepDefinition> GetStepDefinitionsFrom(object target)
        {
            foreach (MethodInfo mi in target.GetType().GetMethods())
            {
                if (mi.DeclaringType.Namespace != "BehaveN")
                    yield return new StepDefinition(target, mi);
            }
        }

        private object GetContextObject(Type type)
        {
            object contextObject;

            if (_context.TryGetValue(type, out contextObject))
                return contextObject;

            contextObject = Activator.CreateInstance(type);
            _context[type] = contextObject;

            return contextObject;
        }

        public bool TryExecute(Step step)
        {
            foreach (StepDefinition stepDefinition in _stepDefinitions)
            {
                if (stepDefinition.TryExecute(step))
                    return true;
            }

            return false;
        }

        public bool HasMatchFor(Step step)
        {
            foreach (StepDefinition stepDefinition in _stepDefinitions)
            {
                if (stepDefinition.Matches(step))
                    return true;
            }

            return false;
        }
    }
}