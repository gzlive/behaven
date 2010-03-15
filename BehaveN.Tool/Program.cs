// <copyright file="Program.cs" company="Jason Diamond">
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

namespace BehaveN.Tool
{
    public class Program
    {
        public static int Main(string[] args)
        {
            try
            {
                args = ExpandArgs(args);

                if (args.Length == 0)
                {
                    ShowHelp();

                    return -1;
                }

                string commandName = args[0];

                Type commandType = CommandHelpers.GetCommandType(commandName);

                if (commandType == null)
                {
                    ShowHelp();

                    return -1;
                }

                var command = (ICommand)Activator.CreateInstance(commandType);

                return command.Run(new List<string>(args).GetRange(1, args.Length - 1).ToArray());
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected error: {0}", e.Message);
            }

            return -1;
        }

        private static string[] ExpandArgs(string[] args)
        {
            var newArgs = new List<string>();

            foreach (string arg in args)
            {
                if (arg.Length > 1 && arg[0] == '@')
                {
                    AddArgs(newArgs, arg.Substring(1));
                }
                else
                {
                    newArgs.Add(arg);
                }
            }

            return newArgs.ToArray();
        }

        private static void AddArgs(List<string> args, string path)
        {
            string actualPath = null;

            if (File.Exists(path))
            {
                actualPath = Path.GetFullPath(path);
            }
            else if (!(path.Contains("/") || path.Contains("\\")))
            {
                actualPath = FindPath(path);
            }

            if (actualPath == null)
            {
                Console.WriteLine("Could not find {0}", path);
            }
            else
            {
                Console.WriteLine("Reading files from {0}", actualPath);

                string dir = Path.GetDirectoryName(actualPath);

                foreach (var line in File.ReadAllLines(actualPath))
                {
                    string newPath = Path.Combine(dir, line);
                    args.Add(newPath);
                }
            }
        }

        private static string FindPath(string file)
        {
            var dir = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent;

            while (dir != null)
            {
                string path = Path.Combine(dir.FullName, file);

                if (File.Exists(path))
                    return path;

                dir = dir.Parent;
            }

            return null;
        }

        internal static void ShowHelp()
        {
            Console.WriteLine("usage: BehaveN.Tool COMMAND [ARGS]");
            Console.WriteLine();
            Console.WriteLine("The available commands are:");

            var commandTypes = CommandHelpers.GetCommandTypes();

            foreach (var commandType in commandTypes)
            {
                string name = commandType.Name;
                name = name.Substring(0, commandType.Name.Length - "Command".Length);

                string summary = CommandHelpers.GetSummary(commandType);

                Console.WriteLine("   {0} {1}", name.PadRight(10), summary);
            }
        }
    }
}
