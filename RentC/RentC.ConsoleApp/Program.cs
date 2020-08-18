using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using RentC.Core.Models.QueryModels;
using RentC.DataAccess;

namespace RentC.ConsoleApp
{
    class Program
    {
        private static void WelcommmingScreen()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Welcome to RentC, your brand new solution to\n");
            sb.Append("manage and control your company's data\n");
            sb.Append("without missing anything\n\n\n\n\n");
            Console.WriteLine(sb);

            ContinueOrQuit(Menu, () => { });
        }

        private static void ContinueOrQuit(Action continueAction, Action quitAction)
        {
            Console.WriteLine("Press ENTER to continue or ESC to quit");
            while (true)
            {
                ConsoleKey consoleKey = Console.ReadKey().Key;
                if (consoleKey == ConsoleKey.Enter)
                {
                    continueAction.Invoke();
                    break;
                }
                if (consoleKey == ConsoleKey.Escape)
                {
                    quitAction.Invoke();
                    break;
                }
            }
        }
        private static void Menu()
        {
            GetAndPrint getAndPrint = new GetAndPrint();
            Console.Clear();
            StringBuilder sb = new StringBuilder();
            sb.Append("\n\n\n1 Register new Car Rent\n");
            sb.Append("2 Update Car Rent\n");
            sb.Append("3 List Rents\n");
            sb.Append("4 List Available Cars\n");
            sb.Append("5 Register new Customer\n");
            sb.Append("6 Update Customer\n");
            sb.Append("7 List Customers\n");
            sb.Append("8 Quit\n\n\n");
            Console.WriteLine(sb);

            string input = Console.ReadLine().Trim();
            switch (input)
            {
                //case "3":
                //    GetReservations(true, func);
                //    ContinueOrQuit(Menu, () => { });
                //    break;
                case "4":
                    getAndPrint.GetAvailableCars(true, "Id");
                    ContinueOrQuit(Menu, () => { });
                    break;
                //case "5":
                //    NewCustomer();
                //    Menu();
                //    break;
                //case "7":
                //    GetAndPrint<int>.GetCustomers(true, x => x.Id);
                //    ContinueOrQuit(Menu, () => { });
                //    break;
                case "8":
                    break;
                default:
                    Console.WriteLine("You entered wrong value");
                    ContinueOrQuit(Menu, () => { });
                    break;
            }
        }        
        static void Main(string[] args)
        {
            WelcommmingScreen();
        }
    }
}
