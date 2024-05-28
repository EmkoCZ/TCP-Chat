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
            // Menu pro zboleni spusteni
            Console.WriteLine("Enter number form 1 to 2");
            Console.WriteLine("1. Client");
            Console.WriteLine("2. Server");
            try
            {
                int input = Int16.Parse(Console.ReadLine());
                switch (input)
                {
                    case 1:
                        // Spusteni klienta
                        new Client();
                        break;
                    case 2:
                        // Spusteni serveru
                        new Server();
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Numbers from 1 to 2 PLEASE");
                        Menu();
                        // Pokud uzivatel napise spatny vyber, spustit znovu vyber
                        break;
                }

            } catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine(ex.Message);
                Console.WriteLine("Please enter only numbers.");
                Menu();
                // Pokud uzivatel napise spatny vyber, spustit znovu vyber
            }
        }
    }
}
