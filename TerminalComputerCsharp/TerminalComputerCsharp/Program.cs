using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;

namespace TerminalComputerCsharp
{
    class Program
    {
        static bool exit = false;
        static bool connected = false;
        static SerialPort serial_port = new SerialPort();
        static int baudrate = 38400;

        static int command_length = 0;

        static Dictionary<string,string> commands = new Dictionary<string, string>()
        {
            {"ports", "Shows all available ports" },
            {"clear","Clears console and all its text" },
            {"connect", "Connects to a specified port. connect *PORT_NAME* " },
            {"disconnect", "Disconnects with the current port" }
        };
        static void Main(string[] args)
        {
            StartConsole();
            do
            {
                Console.Write(">");
                string command = Console.ReadLine();
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
                CommandControls.AvailableCommands(false, command, commands);
                return;
            }
            else if (command.Contains("!") && !command.Contains("commands"))
            {
                CommandControls.AvailableCommands(true, command, commands);
                return;
            }
            else if (command.Contains("connect") && !command.Contains("disconnect") && command.Contains("com"))
            {
                string[] splits = command.Split(' ');
                if (splits[1].Contains("com") && splits[1].Any(c => char.IsDigit(c)))
                {
                    if (!serial_port.IsOpen)
                    {
                        serial_port.PortName = splits[1];
                        CommandControls.EstablishConnection(splits[1], serial_port, baudrate);
                        connected = true;
                        return;
                    }
                    else { Console.WriteLine($"Already connected to {serial_port.PortName.ToUpper()} port"); return; };
                    
                }  
            }

            if (!connected)
            {
                switch (command)
                {
                    case "ports":
                        CommandControls.ScanPorts();
                        break;
                    case "clear":
                        Console.Clear();
                        break;
                    case "disconnect":
                        CommandControls.CloseConnection(serial_port);
                        break;
                    default:
                        Console.WriteLine("Command not found. !commands for available commands");
                        break;
                }
            } else
            {
                string[] command_words = command.Split(' ');
                string first_command_word = command_words[0];
                switch (first_command_word)
                {
                    case "sum":
                        CommandControls.SumOfNumbers(command); // SendCommand()
                        break;
                    default:
                        Console.WriteLine("Command not found. !commands for available commands");
                        break;
                }
            }
        }

    }
}
