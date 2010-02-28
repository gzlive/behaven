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
