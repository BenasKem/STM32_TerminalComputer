using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;

namespace TerminalComputerCsharp
{
    class Program
    {
        static bool exit = false;
        static SerialPort serial_port = new SerialPort();
        static int baudrate = 38400;

        static Dictionary<string,string> commands = new Dictionary<string, string>()
        {
            { "ports", "Shows all available ports" },
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
            else if (command.Contains("connect") && !command.Contains("disconnect"))
            {
                string[] splits = command.Split(' ');
                if (splits[1].Contains("com") && splits[1].Any(c => char.IsDigit(c)))
                {
                    if (!serial_port.IsOpen)
                    {
                        serial_port.PortName = splits[1];
                        EstablishConnection(splits[1]);
                        return;
                    }
                    else { Console.WriteLine($"Already connected to {serial_port.PortName.ToUpper()} port"); return; };
                    
                }  
            }

            switch (command)
            {
                case "ports":
                    ScanPorts();
                    break;
                case "clear":
                    Console.Clear();
                    break;
                case "disconnect":
                    CloseConnection();
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
        /// <summary>
        /// Tries to establish a connection with the given port
        /// </summary>
        /// <param name="port"></param>
        static void EstablishConnection(string port)
        {
            serial_port.PortName = port;
            try
            {
                serial_port.Open();
                serial_port.BaudRate = baudrate;
                serial_port.Parity = Parity.None;
                serial_port.StopBits = StopBits.One;
                serial_port.NewLine = "\n";
                byte[] buffer = new byte[2];
               
                Console.WriteLine(serial_port.ReadLine());
                Console.WriteLine($"Connection established with {port.ToUpper()} port");
            }
            catch(SystemException ex)
            {
                Console.WriteLine($"Could not establish a connection with {port.ToUpper()} port");
            }
        }

        /// <summary>
        /// Close the current connection, if there is any
        /// </summary>
        static void CloseConnection()
        {
            if (serial_port.IsOpen)
            {
                Console.WriteLine($"Closed connection with {serial_port.PortName.ToUpper()} port");
                serial_port.Close();
            }
            else Console.WriteLine("No connection established");
          
        }
    }
}
