using RentC.DataAccess;
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

        public InAppBehavior()
        {
            modelContext = new ModelContext();
            insertUpdate = new InsertUpdate(modelContext, this);
        }
        internal void WelcommmingScreen()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Welcome to RentC, your brand new solution to\n");
            sb.Append("manage and control your company's data\n");
            sb.Append("without missing anything\n\n\n\n\n");
            Console.WriteLine(sb);

            ContinueOrQuit(Menu, () => { });
        }

        internal void ContinueOrQuit(Action continueAction, Action quitAction)
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
        internal void Menu()
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
                case "3":
                    getAndPrint.GetReservations();
                    ContinueOrQuit(Menu, () => { });
                    break;
                case "4":
                    getAndPrint.GetAvailableCars();
                    ContinueOrQuit(Menu, () => { });
                    break;
                case "5":
                    insertUpdate.NewCustomer();
                    Menu();
                    break;
                case "7":
                    getAndPrint.GetCustomers();
                    ContinueOrQuit(Menu, () => { });
                    break;
                case "8":
                    break;
                default:
                    Console.WriteLine("You entered wrong value");
                    ContinueOrQuit(Menu, () => { });
                    break;
            }
        }
    }
}
