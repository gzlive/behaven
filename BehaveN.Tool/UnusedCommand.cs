using System;
using System.Collections.Generic;
using System.Reflection;

namespace BehaveN.Tool
{
    [Summary("Lists the unused step definitions in assemblies")]
    public class UnusedCommand : ICommand
    {
        public static void WriteHelp()
        {
            Console.WriteLine("usage: BehaveN.Tool Unused <filepattern>...");
            Console.WriteLine();
            Console.WriteLine("<filepattern> can be the path to any file. Wildcards don't work yet.");
            Console.WriteLine("Any file that ends in .dll is considered an assembly containing step");
            Console.WriteLine("definitions. Any other file is considered a text file that could contain");
            Console.WriteLine("one or more scenarios.");
            Console.WriteLine();
            Console.WriteLine("NOTE: You need to specify at least one assembly and one text file!");
        }

        public int Run(string[] args)
        {
            var files = new List<string>(args);

            List<string> assemblyFiles = files.FindAll(arg => arg.EndsWith(".dll", StringComparison.OrdinalIgnoreCase));
            List<string> specFiles = files.FindAll(arg => !arg.EndsWith(".dll", StringComparison.OrdinalIgnoreCase));

            LoadAssemblies(assemblyFiles);
            LoadSteps(specFiles);

            var unused = FindUnusedStepDefinitions();

            foreach (var stepDefinition in unused)
                Console.WriteLine(stepDefinition.MethodName);

            return 0;
        }

        private readonly List<Assembly> _assemblies = new List<Assembly>();

        private void LoadAssemblies(IEnumerable<string> assemblyFiles)
        {
            foreach (var assemblyFile in assemblyFiles)
            {
                _assemblies.Add(Assembly.LoadFrom(assemblyFile));
            }
        }

        private readonly List<Step> _steps = new List<Step>();

        private void LoadSteps(IEnumerable<string> specFiles)
        {
            foreach (var specFile in specFiles)
            {
                var specificationsFile = new SpecificationsFile();
                specificationsFile.LoadFile(specFile);

                foreach (var scenario in specificationsFile.Scenarios)
                {
                    foreach (var step in scenario.Steps)
                    {
                        _steps.Add(step);
                    }
                }
            }
        }

        private IEnumerable<StepDefinition> FindUnusedStepDefinitions()
        {
            var stepDefinitions = new StepDefinitionCollection();

            foreach (var assembly in _assemblies)
                stepDefinitions.UseStepDefinitionsFromAssembly(assembly);

            stepDefinitions.CreateContext();

            var unused = new List<StepDefinition>(stepDefinitions);

            unused.RemoveAll(sd => _steps.Exists(sd.Matches));

            unused.Sort((a, b) => a.MethodName.ToLowerInvariant().CompareTo(b.MethodName.ToLowerInvariant()));

            return unused;
        }
    }
}
