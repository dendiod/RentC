using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentC.ConsoleApp
{
    class Program
    {
        static void WriteEmptyLines(int num)
        {
            for (int i = 0; i < num; ++i)
            {
                Console.WriteLine();
            }
        }
        static void WelcommmingScreen()
        {
            Console.WriteLine("    Welcome to RentC, your brand new solution to");
            Console.WriteLine("    manage and control your company's data");
            Console.WriteLine("    without missing anything");
            WriteEmptyLines(5);
            Console.WriteLine("    Press ENTER to continue or ESC to quit");
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
        static void Menu()
        {
            Console.Clear();
            WriteEmptyLines(3);
            Console.WriteLine(" 1 Register new Car Rent");
            Console.WriteLine(" 2 Update Car Rent");
            Console.WriteLine(" 3 List Rents");
            Console.WriteLine(" 4 List Available Cars");
            Console.WriteLine(" 5 Register new Customer");
            Console.WriteLine(" 6 Update Customer");
            Console.WriteLine(" 7 List Customers");
            Console.WriteLine(" 8 Quit");
            WriteEmptyLines(3);
            string input = Console.ReadLine();
            switch (input)
            {
                case "4":
                    Console.WriteLine("yee");
                    break;
                default:
                    break;
            }
        }
        static void Main(string[] args)
        {
            WelcommmingScreen();
        }
    }
}
