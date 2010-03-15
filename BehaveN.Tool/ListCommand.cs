// <copyright file="ListCommand.cs" company="Jason Diamond">
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
            Console.WriteLine("Givens:");
            foreach (var given in GetStepDefinitions("given")) Console.WriteLine("   " + given);

            Console.WriteLine("Whens:");
            foreach (var when in GetStepDefinitions("when")) Console.WriteLine("   " + when);

            Console.WriteLine("Thens:");
            foreach (var then in GetStepDefinitions("then")) Console.WriteLine("   " + then);
        }

        private IEnumerable<string> GetStepDefinitions(string keyword)
        {
            var names = new List<string>();

            foreach (var a in _assemblies)
                foreach (var t in a.GetTypes())
                    foreach (var m in t.GetMethods())
                        if (NameParser.IsStepDefinition(m))
                        {
                            string name = NameParser.Parse(m);

                            if (name.StartsWith(keyword, StringComparison.OrdinalIgnoreCase))
                                names.Add(name);
                        }

            names.Sort();

            return names;
        }
    }
}
