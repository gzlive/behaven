// <copyright file="StepDefinitionCollection.cs" company="Jason Diamond">
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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace BehaveN
{
    /// <summary>
    /// Represents a collection of StepDefinition objects.
    /// </summary>
    public class StepDefinitionCollection : IEnumerable<StepDefinition>
    {
        private readonly List<object> _stepDefinitionObjects = new List<object>();
        private readonly List<StepDefinition> _stepDefinitions = new List<StepDefinition>();
        private readonly Dictionary<Type, object> _context = new Dictionary<Type, object>();
        private readonly List<IDisposable> _disposables = new List<IDisposable>();

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
        /// <param name="object">The object.</param>
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
            _disposables.Clear();

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
            {
                throw new ArgumentException(string.Format("{0} has no public constructors.", type.FullName));
            }

            if (constructors.Length > 1)
            {
                throw new ArgumentException(string.Format("{0} has more than one public constructor.", type.FullName));
            }

            ConstructorInfo constructor = constructors[0];

            object[] parameters = GetConstructorParameters(constructor);

            object result = Activator.CreateInstance(type, parameters);

            if (result is IDisposable)
            {
                _disposables.Add((IDisposable)result);
            }

            return result;
        }

        private object[] GetConstructorParameters(ConstructorInfo constructor)
        {
            object[] parameters = new object[constructor.GetParameters().Length];

            int i = 0;

            foreach (ParameterInfo pi in constructor.GetParameters())
            {
                parameters[i++] = CreateOrGetContextObject(pi.ParameterType);
            }

            return parameters;
        }

        private IEnumerable<StepDefinition> GetStepDefinitionsFrom(object target)
        {
            var methods = new List<MethodInfo>(target.GetType().GetMethods());

            // Sort the methods in reverse order so that longer names get
            // matched before shorter ones.
            methods.Sort((a, b) => -string.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase));

            foreach (MethodInfo mi in methods)
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

            if (contextObject is IDisposable)
                _disposables.Add((IDisposable)contextObject);

            return contextObject;
        }

        /// <summary>
        /// Tries to execute a step. Returns true if the step was executed, false if it wasn't found.
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
        /// Determines whether the specified step can handle an exception.
        /// </summary>
        /// <param name="step">The step.</param>
        /// <returns>
        /// 	<c>true</c> if the step can handler an exception; otherwise, <c>false</c>.
        /// </returns>
        public bool CanHandleException(Step step)
        {
            foreach (StepDefinition stepDefinition in _stepDefinitions)
            {
                if (stepDefinition.Matches(step))
                {
                    return stepDefinition.CanHandleException();
                }
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

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<StepDefinition> GetEnumerator()
        {
            return _stepDefinitions.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Releases all of the disposable step definition objects.
        /// </summary>
        public void Dispose()
        {
            foreach (var disposable in _disposables)
                try { disposable.Dispose(); } catch { }
        }
    }
}