using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Ports;
using System.Text;

namespace TerminalComputerCsharp
{
     static class CommandControls
    {
        /// <summary>
        /// This function determines what type to use, to manipulate the given variables
        /// </summary>
        /// <returns></returns>
        static string DetermineTheVariableTypeOfNumber(string[] numbers)
        {
            List<int> integer_numbers = new List<int>();
            List<float> floating_numbers = new List<float>();
            List<double> double_numbers = new List<double>();

            var numberStyle = NumberStyles.AllowLeadingWhite |
                 NumberStyles.AllowTrailingWhite |
                 NumberStyles.AllowLeadingSign |
                 NumberStyles.AllowDecimalPoint |
                 NumberStyles.AllowThousands |
                 NumberStyles.AllowExponent;//Choose what you need

            int integer = 0;
            float floating = 0;
            double double_number = 0;
            foreach (var potential_number in numbers)
            {
                if (Int32.TryParse(potential_number, out integer))
                {
                    integer_numbers.Add(integer);
                }
                else if (float.TryParse(potential_number, numberStyle, CultureInfo.InvariantCulture, out floating))
                {
                    floating_numbers.Add(floating);
                }
                else if (double.TryParse(potential_number, numberStyle, CultureInfo.InvariantCulture, out double_number))
                {
                    double_numbers.Add(double_number);
                }
            }

            if (integer_numbers.Count == 0 && floating_numbers.Count == 0 && double_numbers.Count == 0)
            {
                return "error";
            }
            else if (floating_numbers.Count >= integer_numbers.Count)
            {
                return "float";
            }
            else if (integer_numbers.Count > 0)
            {
                return "int";
            }
            else return "error";
        }

        public return void ConvertToIntNumbers()
        {

        }

        #region Commands for connection

        /// <summary>
        /// Shows available ports
        /// </summary>
        public static void ScanPorts()
        {
            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
            {
                Console.WriteLine(port);
            }
        }

        /// <summary>
        /// Displays all the available commands
        /// </summary>
        public static void AvailableCommands(bool display_information, string command_to_show, Dictionary<string,string> commands)
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
        public static void EstablishConnection(string port, SerialPort serial_port, int baudrate)
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

                Console.WriteLine($"Connection established with {port.ToUpper()} port");
            }
            catch (SystemException ex)
            {
                Console.WriteLine($"Could not establish a connection with {port.ToUpper()} port");
            }
        }

        public static void CloseConnection(SerialPort serial_port)
        {
            if (serial_port.IsOpen)
            {
                Console.WriteLine($"Closed connection with {serial_port.PortName.ToUpper()} port");
                serial_port.Close();
            }
            else Console.WriteLine("No connection established");

        }

        #endregion

        #region Basic Mathematical Operations

        /// <summary>
        /// Returns the sum of the given numbers
        /// </summary>
        public static string SumOfNumbers(string command)
        {
            int number_count = 0;

            string[] potential_numbers = command.Split(' ');

            // has to be at least one number:
            if (potential_numbers.Length == 1 || potential_numbers[1] == string.Empty)
            {
                return "No numbers too add";
            }
            string type = DetermineTheVariableTypeOfNumber(potential_numbers);

            if (type == "error")
            {
                return "Enter valid numbers";
            }
            else if(type == "int") ConvertToIntNumbers(potential_numbers);


            return "error";
        }

        #endregion
    }
}
