using System;

namespace BehaveN.Tool
{
    [Summary("Provides help for a command")]
    public class HelpCommand : ICommand
    {
        public static void WriteHelp()
        {
            Console.WriteLine("usage: BehaveN.Tool Help COMMAND");
        }

        public int Run(string[] args)
        {
            if (args.Length == 0)
            {
                Program.ShowHelp();

                return 0;
            }

            string commandName = args[0];

            Type commandType = CommandHelpers.GetCommandType(commandName);

            if (commandType == null)
            {
                Program.ShowHelp();

                return -1;
            }

            CommandHelpers.WriteHelp(commandType);

            return 0;
        }
    }
}
