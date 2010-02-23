using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace BehaveN.Tool
{
    [Summary("Verifies the scenarios contained in text files")]
    public class VerifyCommand : ICommand
    {
        public static void WriteHelp()
        {
            Console.WriteLine("usage: BehaveN.Tool Verify <filepattern>...");
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
            var assemblyFiles = args.Where(arg => arg.EndsWith(".dll", StringComparison.OrdinalIgnoreCase));
            var scenarioFiles = args.Where(arg => !arg.EndsWith(".dll", StringComparison.OrdinalIgnoreCase));

            LoadAssemblies(assemblyFiles);
            VerifyScenarios(scenarioFiles);

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

        private void VerifyScenarios(IEnumerable<string> scenarioFiles)
        {
            foreach (var scenarioFile in scenarioFiles)
            {
                var scenario = new Scenario();

                foreach (var assembly in _assemblies)
                {
                    scenario.UseStepDefinitionsFromAssembly(assembly);
                }

                try
                {
                    scenario.VerifyFile(scenarioFile);

                    WriteLineWithColor(ConsoleColor.Green, Passed);
                }
                catch (VerificationException e)
                {
                    string message = GetMessageThatIsClickableInOutputWindow(e);

                    Console.WriteLine(message);

                    WriteLineWithColor(ConsoleColor.Red, Failed);
                }
            }
        }

        private string GetMessageThatIsClickableInOutputWindow(VerificationException e)
        {
            return Regex.Replace(e.ToString(), @"  at (.+) in (.+):line (\d+)", "$2($3): $1");
        }

        private void WriteLineWithColor(ConsoleColor color, string format, params object[] args)
        {
            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(format, args);
            Console.ForegroundColor = oldColor;
        }

        private const string Passed = @"  _____                        _ _ 
 |  __ \                      | | |
 | |__) |__ _ ___ ___  ___  __| | |
 |  ___// _` / __/ __|/ _ \/ _` | |
 | |   | (_| \__ \__ \  __/ (_| |_|
 |_|    \__,_|___/___/\___|\__,_(_)
";

        private const string Failed = @"  ______    _ _          _ _ 
 |  ____|  (_) |        | | |
 | |__ __ _ _| | ___  __| | |
 |  __/ _` | | |/ _ \/ _` | |
 | | | (_| | | |  __/ (_| |_|
 |_|  \__,_|_|_|\___|\__,_(_)
";
    }
}
