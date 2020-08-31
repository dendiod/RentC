using RentC.DataAccess;
using RentC.DataAccess.Contracts;
using RentC.DataAccess.Models;
using RentC.DataAccess.Models.QueryModels;
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
        private InsertUpdate insertUpdate;
        private ModelContext modelContext;

        private IRepo<Car> carRepo;
        private IRepo<Customer> customerRepo;
        private IRepo<Reservation> reservationRepo;
        private IRepo<Location> locationRepo;
        private IRepo<Model> modelRepo;
        private IRepo<Manufacturer> manufacurerRepo;

        public InAppBehavior()
        {
            modelContext = new ModelContext();

            carRepo = new SQLRepo<Car>(modelContext);
            customerRepo = new SQLRepo<Customer>(modelContext);
            reservationRepo = new SQLRepo<Reservation>(modelContext);
            locationRepo = new SQLRepo<Location>(modelContext);
            modelRepo = new SQLRepo<Model>(modelContext);
            manufacurerRepo = new SQLRepo<Manufacturer>(modelContext);

            insertUpdate = new InsertUpdate(this, customerRepo, reservationRepo, locationRepo);
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
            sb.Append("\n\n\n1  Register new Car Rent\n");
            sb.Append("2  Update Car Rent\n");
            sb.Append("3  List Rents\n");
            sb.Append("4  List Available Cars\n");
            sb.Append("5  Register new Customer\n");
            sb.Append("6  Update Customer\n");
            sb.Append("7  List Customers\n");
            sb.Append("8  Vip Customers\n");
            sb.Append("9  Most recently rented Cars\n");
            sb.Append("10 Rented Cars In Given Month\n");
            sb.Append("11 Quit\n\n\n");
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
                    var getRents = new GetAndPrint<SearchReservation>(this, new SearchReservation(), carRepo, customerRepo, reservationRepo, locationRepo,
                modelRepo, manufacurerRepo);
                    getRents.GetReservations();
                    break;
                case "4":
                    var getCars = new GetAndPrint<localhost.QueryCar>(this, new localhost.QueryCar(), carRepo, customerRepo, reservationRepo, locationRepo,
                modelRepo, manufacurerRepo);
                    getCars.GetAvailableCars();
                    break;
                case "5":
                    insertUpdate.ManageCustomers(true);
                    break;
                case "6":
                    insertUpdate.ManageCustomers(false);
                    break;
                case "7":
                    var getCustomers = new GetAndPrint<SearchCustomer>(this, new SearchCustomer(), carRepo, customerRepo, reservationRepo, locationRepo,
                modelRepo, manufacurerRepo);
                    getCustomers.GetCustomers();
                    break;
                case "8":
                    var getVipCustomers = new GetAndPrint<SearchCustomer>(this, new SearchCustomer(), carRepo, customerRepo, reservationRepo, locationRepo,
                modelRepo, manufacurerRepo);
                    getVipCustomers.GetVipCustomers();
                    break;
                case "9":
                    var getRecentlyCars = new GetAndPrint<QueryCar>(this, new QueryCar(), carRepo, customerRepo, reservationRepo, locationRepo,
                modelRepo, manufacurerRepo);
                    getRecentlyCars.GetRecentCars();
                    break;
                case "10":
                    var getRentedCarsInMonth = new GetAndPrint<QueryCar>(this, new QueryCar(), carRepo, customerRepo, reservationRepo, locationRepo,
                modelRepo, manufacurerRepo);
                    getRentedCarsInMonth.GetRentedCarsInMonth();
                    break;
                case "11":
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
