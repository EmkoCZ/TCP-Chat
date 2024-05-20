using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCP_Chat
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Menu();
        }

        private static void Menu()
        {
            Console.WriteLine("Enter number form 1 to 2");
            Console.WriteLine("1. Client");
            Console.WriteLine("2. Server");
            try
            {
                int input = Int16.Parse(Console.ReadLine());
                switch (input)
                {
                    case 1:
                        new Client();
                        break;
                    case 2:
                        new Server();
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Numbers from 1 to 2 PLEASE");
                        Menu();
                        break;
                }

            } catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine(ex.Message);
                Console.WriteLine("Please enter only numbers.");
                Menu();
            }
        }
    }
}
