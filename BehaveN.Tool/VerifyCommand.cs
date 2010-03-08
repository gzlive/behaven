using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Mono.Options;

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
            Console.WriteLine();
            Console.WriteLine("options:");
            Console.WriteLine();
            GetOptions().WriteOptionDescriptions(Console.Out);
        }

        public int Run(string[] args)
        {
            OptionSet options = GetOptions();

            List<string> files = new List<string>(options.Parse(args));

            List<string> assemblyFiles = files.FindAll(arg => arg.EndsWith(".dll", StringComparison.OrdinalIgnoreCase));
            List<string> scenarioFiles = files.FindAll(arg => !arg.EndsWith(".dll", StringComparison.OrdinalIgnoreCase));

            LoadAssemblies(assemblyFiles);
            VerifyScenarios(scenarioFiles);

            return 0;
        }

        private static string outputFile;
        private static bool sta;

        private static OptionSet GetOptions()
        {
            var options = new OptionSet();
            options.Add("output=", "the path to the file to report to", s => outputFile = s);
            options.Add("sta", "use a STA thread to execute the scenarios", s => sta = true);
            return options;
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
            ThreadStart ts = () =>
                                 {
                                     foreach (var scenarioFile in scenarioFiles)
                                     {
                                         var specificationsFile = new SpecificationsFile();

                                         if (!string.IsNullOrEmpty(outputFile))
                                         {
                                             specificationsFile.Reporter.Destination = outputFile;
                                         }

                                         foreach (var assembly in _assemblies)
                                         {
                                             specificationsFile.StepDefinitions.UseStepDefinitionsFromAssembly(assembly);
                                         }

                                         specificationsFile.LoadFile(scenarioFile);
                                         specificationsFile.Execute();
                                         specificationsFile.Report();

                                         if (specificationsFile.Passed)
                                             WriteLineWithColor(ConsoleColor.Green, Passed);
                                         else
                                             WriteLineWithColor(ConsoleColor.Red, Failed);
                                     }
                                 };

            Thread t = new Thread(ts);

            if (sta)
                t.SetApartmentState(ApartmentState.STA);

            t.Start();
            t.Join();
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
