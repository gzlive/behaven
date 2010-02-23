using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace BehaveN
{
    /// <summary>
    /// Represents the base verification engine for specifications.
    /// </summary>
    public abstract class Specifications
    {
        private readonly List<Interpreter> _interpreters = new List<Interpreter>();
        private readonly Dictionary<Type, object> _context = new Dictionary<Type, object>();
        private readonly StepCollection _steps = new StepCollection();
        private readonly List<Exception> _exceptions = new List<Exception>();
        private Reporter _reporter;
        private bool _continueAfterFailedSteps;

        /// <summary>
        /// Initializes a new instance of the <see cref="Specifications"/> class.
        /// </summary>
        protected Specifications()
        {
            _interpreters.Add(new Interpreter(this));
        }

        /// <summary>
        /// Adds the step definitions from the specified object.
        /// </summary>
        /// <param name="someObject">The object containing the step definitions.</param>
        public void UseStepDefinitionsFrom(object someObject)
        {
            _interpreters.Add(new Interpreter(someObject));
        }

        /// <summary>
        /// Adds the step definitions from the specified type.
        /// </summary>
        /// <typeparam name="T">The type containing the step definitions.</typeparam>
        public void UseStepDefinitionsFromType<T>()
        {
            UseStepDefinitionsFrom(CreateStepDefinitions(typeof(T)));
        }

        /// <summary>
        /// Adds the step definitions from the specified type.
        /// </summary>
        /// <param name="type">The type containing the step definitions.</param>
        public void UseStepDefinitionsFromType(Type type)
        {
            UseStepDefinitionsFrom(CreateStepDefinitions(type));
        }

        private object CreateStepDefinitions(Type type)
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

        private object GetContextObject(Type type)
        {
            object contextObject;

            if (_context.TryGetValue(type, out contextObject))
                return contextObject;

            contextObject = Activator.CreateInstance(type);
            _context[type] = contextObject;

            return contextObject;
        }

        /// <summary>
        /// Adds the step definitions from the assembly containing the specified type.
        /// </summary>
        /// <typeparam name="T">The type in the assembly to look for other types containing step definitions.</typeparam>
        public void UseStepDefinitionsFromAssemblyOfType<T>()
        {
            UseStepDefinitionsFromAssembly(typeof(T).Assembly);
        }

        /// <summary>
        /// Adds the step definitions from the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly containing the step definitions.</param>
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

        /// <summary>
        /// Gets the steps.
        /// </summary>
        /// <value>The steps.</value>
        protected StepCollection Steps
        {
            get { return _steps; }
        }

        internal void ExecuteStep(StepType stepType, string description)
        {
            ReportDescription(description);
        }

        internal void ExecuteStep(StepType stepType, string description, IConvertibleObject convertibleObject)
        {
            try
            {
                try
                {
                    foreach (Interpreter interpreter in _interpreters)
                    {
                        if (interpreter.InvokeMatchingStepMethod(ref description, stepType, convertibleObject))
                        {
                            ReportPassed(stepType, description);
                            return;
                        }
                    }

                    throw new UndefinedException("Not all steps are defined.");
                }
                catch (Exception e)
                {
                    Exception actualException = e;

                    if (e is TargetInvocationException && e.InnerException != null)
                    {
                        actualException = e.InnerException;
                    }

                    if (actualException is UndefinedException)
                    {
                        ReportUndefined(stepType, description, convertibleObject);
                    }
                    else if (actualException is NotImplementedException)
                    {
                        ReportPending(stepType, description);
                    }
                    else
                    {
                        ReportFailed(stepType, description, actualException);
                    }

                    if (ContinueAfterFailedSteps)
                    {
                        _exceptions.Add(actualException);
                    }
                    else
                    {
                        throw new VerificationException(actualException);
                    }
                }
            }
            finally
            {
                if (convertibleObject != null)
                {
                    ReportConvertibleObject(convertibleObject);
                }
            }
        }

        internal bool IsStepDefined(StepType stepType, string description, IConvertibleObject convertibleObject)
        {
            foreach (Interpreter interpreter in _interpreters)
            {
                if (interpreter.HasMatchingStepMethod(stepType, description, convertibleObject))
                {
                    return true;
                }
            }

            return false;
        }

        private void ReportDescription(string description)
        {
            GetReporter().ReportDescription(description);
            OnDescription(description);
        }

        internal void ReportUndefined(StepType stepType, string description, IConvertibleObject convertibleObject)
        {
            GetReporter().ReportUndefined(stepType, description, convertibleObject);
            OnUndefined(stepType, description);
        }

        private void ReportPending(StepType stepType, string description)
        {
            GetReporter().ReportPending(stepType, description);
            OnPending(stepType, description);
        }

        private void ReportPassed(StepType stepType, string description)
        {
            GetReporter().ReportPassed(stepType, description);
            OnPassed(stepType, description);
        }

        private void ReportFailed(StepType stepType, string description, Exception e)
        {
            GetReporter().ReportFailed(stepType, description, e);
            OnFailed(stepType, description, e);
        }

        internal void ReportSkipped(StepType stepType, string description)
        {
            GetReporter().ReportSkipped(stepType, description);
            OnSkipped(stepType, description);
        }

        internal void ReportConvertibleObject(IConvertibleObject convertibleObject)
        {
            GetReporter().ReportConvertibleObject(convertibleObject);
            OnConvertibleObject(convertibleObject);
        }

        /// <summary>
        /// Called after a description is reported.
        /// </summary>
        /// <param name="description">The description.</param>
        protected virtual void OnDescription(string description)
        {
        }

        /// <summary>
        /// Called after an undefined step is reported.
        /// </summary>
        /// <param name="stepType">Type of the step.</param>
        /// <param name="description">The description.</param>
        protected virtual void OnUndefined(StepType stepType, string description)
        {
        }

        /// <summary>
        /// Called after a pending step is reported.
        /// </summary>
        /// <param name="stepType">Type of the step.</param>
        /// <param name="description">The description.</param>
        protected virtual void OnPending(StepType stepType, string description)
        {
        }

        /// <summary>
        /// Called after a passed step is reported.
        /// </summary>
        /// <param name="stepType">Type of the step.</param>
        /// <param name="description">The description.</param>
        protected virtual void OnPassed(StepType stepType, string description)
        {
        }

        /// <summary>
        /// Called after a failed step is reported.
        /// </summary>
        /// <param name="stepType">Type of the step.</param>
        /// <param name="description">The description.</param>
        /// <param name="e">The e.</param>
        protected virtual void OnFailed(StepType stepType, string description, Exception e)
        {
        }

        /// <summary>
        /// Called after a skipped step is reported.
        /// </summary>
        /// <param name="stepType">Type of the step.</param>
        /// <param name="description">The description.</param>
        protected virtual void OnSkipped(StepType stepType, string description)
        {
        }

        /// <summary>
        /// Called after a convertible object is reported.
        /// </summary>
        /// <param name="convertibleObject">The convertible object.</param>
        protected virtual void OnConvertibleObject(IConvertibleObject convertibleObject)
        {
        }

        /// <summary>
        /// Verifies the specifications in a file.
        /// </summary>
        /// <param name="path">The path.</param>
        public void VerifyFile(string path)
        {
            VerifyText(File.ReadAllText(path));
        }

        /// <summary>
        /// Verifies the specifications in the embedded resources that match a pattern.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="pattern">The pattern or null.</param>
        public void VerifyEmbeddedResources(Assembly assembly, string pattern)
        {
            string[] names = assembly.GetManifestResourceNames();

            Regex filter = null;

            if (!string.IsNullOrEmpty(pattern))
            {
                filter = new Regex(pattern, RegexOptions.IgnoreCase);
            }

            foreach (string name in names)
            {
                if (filter == null || filter.IsMatch(name))
                {
                    VerifyEmbeddedResource(assembly, name);
                }
            }
        }

        /// <summary>
        /// Verifies the specifications in an embedded resource.
        /// </summary>
        /// <param name="assembly">The assembly containing the embedded resource.</param>
        /// <param name="name">The name of the embedded resource.</param>
        public void VerifyEmbeddedResource(Assembly assembly, string name)
        {
            Stream stream = assembly.GetManifestResourceStream(name);

            if (stream == null)
                throw new ArgumentException("name");

            using (stream)
            using (StreamReader reader = new StreamReader(stream))
            {
                VerifyText(reader.ReadToEnd());
            }
        }

        /// <summary>
        /// Verifies the specifications in a string.
        /// </summary>
        /// <param name="text">The text.</param>
        public void VerifyText(string text)
        {
            ReadSpecifications(text);

            Verify();
        }

        /// <summary>
        /// Reads the specifications in the text.
        /// </summary>
        /// <param name="text">The text.</param>
        protected abstract void ReadSpecifications(string text);

        /// <summary>
        /// Verifies this instance.
        /// </summary>
        public void Verify()
        {
            List<Exception> exceptions;

            int i = 0;

            try
            {
                for (; i < _steps.Count; i++)
                {
                    Step step = _steps[i];
                    step.Execute(this);
                }
            }
            finally
            {
                for (++i; i < _steps.Count; i++)
                {
                    Step step = _steps[i];
                    step.Skip(this);
                }

                GetReporter().ReportEnd();

                exceptions = new List<Exception>(_exceptions);

                Reset();
            }

            if (exceptions.Count > 0)
            {
                Exception e = exceptions[0];
                throw new VerificationException(e);
            }
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void Reset()
        {
            OnResetting();

            _context.Clear();
            _steps.Clear();
            _exceptions.Clear();
        }

        /// <summary>
        /// Called before resetting this instance.
        /// </summary>
        protected virtual void OnResetting()
        {
        }

        /// <summary>
        /// Gets or sets the reporter.
        /// </summary>
        /// <value>The reporter.</value>
        public Reporter Reporter
        {
            get { return _reporter; }
            set { _reporter = value; }
        }

        private Reporter GetReporter()
        {
            if (Reporter == null)
            {
                Reporter = GetDefaultReporter();
            }

            return Reporter;
        }

        /// <summary>
        /// Gets the default reporter.
        /// </summary>
        /// <returns>A <see cref="Reporter" /> object.</returns>
        protected abstract Reporter GetDefaultReporter();

        /// <summary>
        /// Gets or sets a value indicating whether verification should continue after executing a failed step.
        /// </summary>
        /// <value>
        ///     <c>true</c> if verification should continue; otherwise, <c>false</c>.
        /// </value>
        public bool ContinueAfterFailedSteps
        {
            get { return _continueAfterFailedSteps; }
            set { _continueAfterFailedSteps = value; }
        }
    }
}
