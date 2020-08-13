using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentC.ConsoleApp
{
    class Program
    {
        private static void WelcommmingScreen()
        {
            Console.WriteLine("\tWelcome to RentC, your brand new solution to");
            Console.WriteLine("\tmanage and control your company's data");
            Console.WriteLine("\twithout missing anything\n\n\n\n\n");
            Console.WriteLine("\tPress ENTER to continue or ESC to quit");
            while (true)
            {
                ConsoleKey consoleKey = Console.ReadKey().Key;
                if (consoleKey == ConsoleKey.Enter)
                {
                    Menu();
                    break;
                }
                if (consoleKey == ConsoleKey.Escape)
                {
                    break;
                }
            }
        }
        private static void Menu()
        {
            Console.Clear();
            Console.WriteLine("\n\n\n\t1 Register new Car Rent");
            Console.WriteLine("\t2 Update Car Rent");
            Console.WriteLine("\t3 List Rents");
            Console.WriteLine("\t4 List Available Cars");
            Console.WriteLine("\t5 Register new Customer");
            Console.WriteLine("\t6 Update Customer");
            Console.WriteLine("\t7 List Customers");
            Console.WriteLine("\t8 Quit\n\n\n");
            string input = Console.ReadLine();
            switch (input)
            {
                case "4":
                    GetAvailableCars();
                    break;
                default:
                    break;
            }
        }

        private static void GetAvailableCars()
        {
            Localhost.WebService1 webService = new Localhost.WebService1();
            Console.WriteLine(webService.MyMethod());
            Console.ReadKey();

        }

        static void Main(string[] args)
        {
            WelcommmingScreen();
        }
    }
}
