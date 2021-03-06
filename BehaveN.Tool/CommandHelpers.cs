﻿// <copyright file="CommandHelpers.cs" company="Jason Diamond">
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
    internal class CommandHelpers
    {
        public static Type GetCommandType(string commandName)
        {
            var commandType = Type.GetType(String.Format("BehaveN.Tool.{0}Command", commandName), false, true);

            if (commandType == null) return null;

            if (!typeof(ICommand).IsAssignableFrom(commandType))
                return null;

            return commandType;
        }

        public static IEnumerable<Type> GetCommandTypes()
        {
            var commandTypes = new List<Type>();

            foreach (var type in typeof(Program).Assembly.GetTypes())
            {
                if (type.Name.EndsWith("Command") && typeof(ICommand).IsAssignableFrom(type) && !type.IsAbstract)
                    commandTypes.Add(type);
            }

            commandTypes.Sort((a, b) => a.Name.CompareTo(b.Name));

            return commandTypes;
        }

        public static string GetSummary(Type commandType)
        {
            var summaryAttribute = (SummaryAttribute)Attribute.GetCustomAttribute(commandType, typeof(SummaryAttribute));
            return summaryAttribute != null ? summaryAttribute.Text : "";
        }

        public static void WriteHelp(Type commandType)
        {
            var writeHelpMethod = commandType.GetMethod("WriteHelp", BindingFlags.Public | BindingFlags.Static);

            if (writeHelpMethod == null)
            {
                Console.WriteLine("Sorry, no help for this command!");
            }
            else
            {
                writeHelpMethod.Invoke(null, null);
            }
        }
    }
}
