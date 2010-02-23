using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BehaveN.Tool
{
    internal class CommandHelpers
    {
        public static Type GetCommandType(string commandName)
        {
            var commandType = Type.GetType(String.Format("BehaveN.Tool.{0}Command", commandName), false, true);

            if (commandType == null) return null;

            if (!commandType.GetInterfaces().Contains(typeof(ICommand))) return null;

            return commandType;
        }

        public static IEnumerable<Type> GetCommandTypes()
        {
            return from t in typeof(Program).Assembly.GetTypes()
                   where t.Name.EndsWith("Command") && t.GetInterfaces().Contains(typeof(ICommand))
                   orderby t.Name
                   select t;
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
