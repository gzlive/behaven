using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BehaveN.Tool
{
    [Summary("Lists the step definitions in assemblies")]
    public class ListCommand : ICommand
    {
        public static void WriteHelp()
        {
            Console.WriteLine("usage: BehaveN.Tool List <filepattern>...");
            Console.WriteLine();
            Console.WriteLine("<filepattern> can be the path to any file. Wildcards don't work yet.");
            Console.WriteLine("All files are assumed to be assemblies and loaded. The types in each of");
            Console.WriteLine("these assemblies are examined to see if they contain any step definitions");
            Console.WriteLine("and, if so, those are output to the console.");
            Console.WriteLine();
            Console.WriteLine("NOTE: You need to specify at least one assembly!");
        }

        public int Run(string[] args)
        {
            var assemblyFiles = args;

            if (assemblyFiles.Length == 0)
            {
                WriteHelp();

                return -1;
            }

            LoadAssemblies(assemblyFiles);
            WriteStepDefinitions();

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

        private void WriteStepDefinitions()
        {
            var stepDefinitions = from a in _assemblies
                                  from t in a.GetTypes()
                                  from m in t.GetMethods()
                                  where NameParser.IsStepDefinition(m)
                                  let n = NameParser.Parse(m, false)
                                  group n by n.Split()[0].ToLowerInvariant();

            var givens = stepDefinitions.Where(g => g.Key == "given").SelectMany(g => g).OrderBy(s => s);
            var whens = stepDefinitions.Where(g => g.Key == "when").SelectMany(g => g).OrderBy(s => s);
            var thens = stepDefinitions.Where(g => g.Key == "then").SelectMany(g => g).OrderBy(s => s);

            Console.WriteLine("Givens:");
            foreach (var given in givens) Console.WriteLine("   " + given);

            Console.WriteLine("Whens:");
            foreach (var when in whens) Console.WriteLine("   " + when);

            Console.WriteLine("Thens:");
            foreach (var then in thens) Console.WriteLine("   " + then);
        }
    }
}
