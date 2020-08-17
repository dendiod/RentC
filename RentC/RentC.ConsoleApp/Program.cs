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
            Func<QueryReservation, int> func = x => x.Id;
            switch (input)
            {
                case "3":
                    GetReservations(true, func);
                    ContinueOrQuit(Menu, () => { });
                    break;
                case "4":
                    GetAvailableCars();
                    ContinueOrQuit(Menu, () => { });
                    break;
                case "5":
                    NewCustomer();
                    Menu();
                    break;
                case "8":
                    break;
                default:
                    Console.WriteLine("You entered wrong value");
                    ContinueOrQuit(Menu, () => { });
                    break;
            }
        }

        private static void GetReservations(bool asc, Func<QueryReservation, int>func)
        {
            var queryReservations = Data.GetReservations(true, func);
            Console.WriteLine(queryReservations[0].StartDate);
        }

        private static void NewCustomer()
        {
            Console.Write("Client ID: ");
            int id;
            bool idIsInt = int.TryParse(Console.ReadLine().Trim(), out id);
            if (!idIsInt)
            {
                Console.WriteLine("Id must be integer");
                ContinueOrQuit(NewCustomer, Menu);
            }
            if (Data.IsCustomerExists(id) != null)
            {
                Console.WriteLine("Customer with this id already exists");
                ContinueOrQuit(NewCustomer, Menu);
            }
            Console.Write("Client Name: ");
            string name = Console.ReadLine();
            Console.Write("Birth Date: ");

        }

        private static void GetAvailableCars()
        {
            localhost.AvailableCarsService webService = new localhost.AvailableCarsService();
            localhost.QueryCar[] cars = webService.GetAvailableCars(true);

            StringBuilder sb = new StringBuilder();
            foreach (localhost.QueryCar c in cars)
            {
                sb.Append("------------------------------------------------\n");
                sb.Append("Car plate: " + c.Plate + "\n");
                sb.Append("Car manufacturer: " + c.Manufacturer + "\n");
                sb.Append("Car model: " + c.Model + "\n");
                sb.Append("Start Date: " + c.StartDate + "\n");
                sb.Append("End Date: " + c.EndDate + "\n");
                sb.Append("City: " + c.Location + "\n");
            }
            sb.Append("Sort by\n");
            sb.Append("1-Car plate\n");
            sb.Append("2-Car manufacturer\n");
            sb.Append("3-Start Date\n");
            sb.Append("4-End Date\n");
            sb.Append("5-City\n");
            sb.Append("6-Quit\n");
            Console.WriteLine(sb);
            int input = int.Parse(Console.ReadLine().Trim());
        }
        static void Main(string[] args)
        {
            WelcommmingScreen();
        }
    }
}
