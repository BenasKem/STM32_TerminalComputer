using System;
using System.Collections.Generic;
using System.IO.Ports;

namespace TerminalComputerCsharp
{
    class Program
    {
        static bool exit = false;

        static Dictionary<string,string> commands = new Dictionary<string, string>()
        {
            { "ports", "Shows all available ports" },
            {"clear","Clears console and all its text" }
        };
        static void Main(string[] args)
        {
            StartConsole();

            do
            {
                Console.Write(">");
                var command = Console.ReadLine();
                ParseCommand(command);
            } while (!exit);
            
        }

        /// <summary>
        /// Innits the console
        /// </summary>
        static void StartConsole()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Title = "Terminal Computer";
            Console.Beep();
        }

        /// <summary>
        /// Tries to parse the users input command. If parsed, does the input action
        /// </summary>
        /// <param name="command"></param>
        static void ParseCommand(string command)
        {
            command = command.ToLower();

            if (command.Contains("commands"))
            {
                AvailableCommands(false, command);
                return;
            }
            else if (command.Contains("!") && !command.Contains("commands"))
            {
                AvailableCommands(true, command);
                return;
            }

            switch (command)
            {
                case "ports":
                    ScanPorts();
                    break;
                case "clear":
                    Console.Clear();
                    break;
                default:
                    Console.WriteLine("Command not found. !commands for available commands");
                    break;

            }
        }

        /// <summary>
        /// Shows available ports
        /// </summary>
        static void ScanPorts()
        {
            string[] ports = SerialPort.GetPortNames();
            foreach (var item in ports)
            {
                Console.WriteLine(item);
            }
        }

        /// <summary>
        /// Displays all the available commands
        /// </summary>
        static void AvailableCommands(bool display_information, string command_to_show)
        {

            if (!display_information)
            {
                Console.WriteLine("Type \'!*command_name*\' for more information about each command");
                foreach (var command in commands)
                {
                    Console.WriteLine(command.Key);
                }
            }
            else
            {
                command_to_show = command_to_show.Trim('!');
                Console.WriteLine(commands[command_to_show]);
            }
        }
    }
}
