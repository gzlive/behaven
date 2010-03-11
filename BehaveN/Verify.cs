// <copyright file="Verify.cs" company="Jason Diamond">
//
// Copyright (c) 2009-2010 Jason Diamond
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
using System.Reflection;

namespace BehaveN
{
    /// <summary>
    /// Utility class to easily verify scenarios.
    /// </summary>
    public static class Verify
    {
        /// <summary>
        /// Verifies the specifications in the specified embedded resource.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="stepDefinitions">The assemblies, types, or objects containing step definitions.</param>
        public static void EmbeddedResource(Assembly assembly, string resourceName, params object[] stepDefinitions)
        {
            var specs = new SpecificationsFile();
            specs.LoadEmbeddedResource(assembly, resourceName);
            AddStepDefinitions(specs, stepDefinitions);
            specs.Verify();
        }

        /// <summary>
        /// Verifies the specifications in the specified file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="stepDefinitions">The assemblies, types, or objects containing step definitions.</param>
        public static void File(string path, params object[] stepDefinitions)
        {
            var specs = new SpecificationsFile();
            specs.LoadFile(path);
            AddStepDefinitions(specs, stepDefinitions);
            specs.Verify();
        }

        /// <summary>
        /// Verifies the specifications in the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="stepDefinitions">The assemblies, types, or objects containing step definitions.</param>
        public static void Text(string text, params object[] stepDefinitions)
        {
            var specs = new SpecificationsFile();
            specs.LoadText(text);
            AddStepDefinitions(specs, stepDefinitions);
            specs.Verify();
        }

        /// <summary>
        /// Verifies the named scenario in the specified embedded resource.
        /// </summary>
        /// <param name="scenarioName">Name of the scenario.</param>
        /// <param name="assembly">The assembly.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="stepDefinitions">The assemblies, types, or objects containing step definitions.</param>
        public static void ScenarioInEmbeddedResource(string scenarioName, Assembly assembly, string resourceName, params object[] stepDefinitions)
        {
            var specs = new SpecificationsFile();
            specs.LoadEmbeddedResource(assembly, resourceName);
            AddStepDefinitions(specs, stepDefinitions);
            specs.Scenarios[scenarioName].Verify();
        }

        /// <summary>
        /// Verifies the named scenario in the specified file.
        /// </summary>
        /// <param name="scenarioName">Name of the scenario.</param>
        /// <param name="path">The path.</param>
        /// <param name="stepDefinitions">The assemblies, types, or objects containing step definitions.</param>
        public static void ScenarioInFile(string scenarioName, string path, params object[] stepDefinitions)
        {
            var specs = new SpecificationsFile();
            specs.LoadFile(path);
            AddStepDefinitions(specs, stepDefinitions);
            specs.Scenarios[scenarioName].Verify();
        }

        /// <summary>
        /// Verifies the named scenario in the specified text.
        /// </summary>
        /// <param name="scenarioName">Name of the scenario.</param>
        /// <param name="text">The text.</param>
        /// <param name="stepDefinitions">The assemblies, types, or objects containing step definitions.</param>
        public static void ScenarioInText(string scenarioName, string text, params object[] stepDefinitions)
        {
            var specs = new SpecificationsFile();
            specs.LoadText(text);
            AddStepDefinitions(specs, stepDefinitions);
            specs.Scenarios[scenarioName].Verify();
        }

        private static void AddStepDefinitions(SpecificationsFile specs, object[] stepDefinitions)
        {
            foreach (var stepDefinition in stepDefinitions)
            {
                if (stepDefinition is Assembly)
                {
                    specs.StepDefinitions.UseStepDefinitionsFromAssembly((Assembly)stepDefinition);
                }
                else if (stepDefinition is Type)
                {
                    specs.StepDefinitions.UseStepDefinitionsFromType((Type)stepDefinition);
                }
                else
                {
                    specs.StepDefinitions.UseStepDefinitionsFromObject(stepDefinition);
                }
            }
        }
    }
}
