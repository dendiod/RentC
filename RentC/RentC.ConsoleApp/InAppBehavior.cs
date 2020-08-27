using RentC.DataAccess;
using RentC.DataAccess.Models.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentC.ConsoleApp
{
    public class InAppBehavior
    {
        private ModelContext modelContext;
        private InsertUpdate insertUpdate;

        private GetAndPrint<SearchReservation> getRents;
        private GetAndPrint<localhost.QueryCar> getCars;
        private GetAndPrint<SearchCustomer> getCustomers;

        public InAppBehavior()
        {
            modelContext = new ModelContext();
            insertUpdate = new InsertUpdate(modelContext, this);
            getRents = new GetAndPrint<SearchReservation>(modelContext, this, new SearchReservation());
            getCars = new GetAndPrint<localhost.QueryCar>(modelContext, this, new localhost.QueryCar());
            getCustomers = new GetAndPrint<SearchCustomer>(modelContext, this, new SearchCustomer());
        }
        internal void WelcommmingScreen()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Welcome to RentC, your brand new solution to\n");
            sb.Append("manage and control your company's data\n");
            sb.Append("without missing anything\n\n\n\n");
            Console.WriteLine(sb);

            ContinueOrQuit(Menu, (_) => { });
        }

        internal void ContinueOrQuit(Action<bool> continueAction, Action<bool> quitAction, bool creating = true)
        {
            Console.WriteLine("\nPress ENTER to continue or ESC to quit");
            while (true)
            {
                ConsoleKey consoleKey = Console.ReadKey().Key;
                if (consoleKey == ConsoleKey.Enter)
                {
                    continueAction(creating);
                    break;
                }
                if (consoleKey == ConsoleKey.Escape)
                {
                    quitAction(creating);
                    break;
                }
            }
        }
        internal void Menu(bool temp = true)
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
            switch (input)
            {
                case "1":
                    insertUpdate.ManageReservations(true);
                    break;
                case "2":
                    insertUpdate.ManageReservations(false);
                    break;
                case "3":
                    getRents.GetReservations();
                    break;
                case "4":
                    getCars.GetAvailableCars();
                    break;
                case "5":
                    insertUpdate.ManageCustomers(true);
                    break;
                case "6":
                    insertUpdate.ManageCustomers(false);
                    break;
                case "7":
                    getCustomers.GetCustomers();
                    break;
                case "8":
                    break;
                default:
                    Console.WriteLine("You entered wrong value");
                    ContinueOrQuit(Menu, (_) => { });
                    break;
            }
        }

        internal void MenuItemEntry(string name, string secondName = "", bool creating = true)
        {
            Console.Clear();
            string screenLabel = creating ? name : secondName;
            Console.WriteLine(screenLabel + "\n");
        }
    }
}
