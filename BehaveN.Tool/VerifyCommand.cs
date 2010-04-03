// <copyright file="VerifyCommand.cs" company="Jason Diamond">
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
                                         var feature = new Feature();

                                         if (!string.IsNullOrEmpty(outputFile))
                                         {
                                             feature.Reporter.Destination = outputFile;
                                         }

                                         foreach (var assembly in _assemblies)
                                         {
                                             feature.StepDefinitions.UseStepDefinitionsFromAssembly(assembly);
                                         }

                                         new PlainTextReader().ReadTo(Read.File(scenarioFile), feature);
                                         feature.Execute();
                                         feature.Report();

                                         if (feature.Passed)
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
