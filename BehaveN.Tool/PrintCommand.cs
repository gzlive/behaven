// <copyright file="PrintCommand.cs" company="Jason Diamond">
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
using System.IO;
using System.Reflection;
using Mono.Options;

namespace BehaveN.Tool
{
    [Summary("Pretty prints a file.")]
    public class PrintCommand : ICommand
    {
        public static void WriteHelp()
        {
            Console.WriteLine("usage: BehaveN.Tool Print <filepattern>...");
            Console.WriteLine();
            Console.WriteLine("<filepattern> can be the path to any file. Wildcards like *.txt do work.");
            Console.WriteLine("All files are assumed to be text files containing features/scenarios.");
            Console.WriteLine();
            Console.WriteLine("NOTE: You need to specify at least one text file!");
        }

        public int Run(string[] args)
        {
            OptionSet options = GetOptions();

            var files = options.Parse(args);

            if (files.Count < 1)
                return -1;

            foreach (var f in files) Console.WriteLine(f);

            var expandedFiles = FileHelpers.ExpandWildcards(files);

            foreach (var file in expandedFiles)
            {
                var feature = new Feature();
                feature.ReadFile(file);

                string outputFile = Path.ChangeExtension(file, ".html");

                Console.WriteLine("Writing to " + outputFile);

                var reporter = new HtmlReporter(new StreamWriter(outputFile));
                reporter.ReportFeature(feature);
            }

            return 0;
        }

        private static OptionSet GetOptions()
        {
            var options = new OptionSet();
            return options;
        }
    }
}
