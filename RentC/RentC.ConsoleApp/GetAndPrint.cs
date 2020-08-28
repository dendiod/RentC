using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using RentC.DataAccess.Models.QueryModels;
using RentC.DataAccess;
using RentC.DataAccess.Models.Search;
using System.Reflection.Emit;

namespace RentC.ConsoleApp
{    
    public class GetAndPrint<T>
    {
        private readonly QueryManager queryManager;
        private readonly InAppBehavior inAppBehavior;
        private readonly Searcher<T> searcher;

        private int propIndex;
        private PropertyInfo[] properties;
        private Action<string, int> action;
        private int optionsLength;
        private readonly T searchItem;
        string orderBy = "";

        public GetAndPrint(ModelContext modelContext, InAppBehavior inAppBehavior, T searchItem)
        {
            queryManager = new QueryManager(modelContext);
            this.inAppBehavior = inAppBehavior;
            this.searchItem = searchItem;
            searcher = new Searcher<T>(inAppBehavior, searchItem);
        }

        private void Set(int propIndex, PropertyInfo[] properties, Action<string, int> action, int optionsLength, string orderBy)
        {
            this.propIndex = propIndex;
            this.properties = properties;
            this.action = action;
            this.optionsLength = optionsLength;
            this.orderBy = orderBy;
        }

        private void PrepareOrderBy(string orderBy)
        {
            bool asc = !orderBy.EndsWith(" desc");
            OrderByOrQuit(asc);
        }

        internal void GetAvailableCars(string orderBy = "Id", int propIndex = 0)
        {
            inAppBehavior.MenuItemEntry("List Available Cars");

            localhost.AvailableCarsService webService = new localhost.AvailableCarsService();
            localhost.QueryCar[] cars = webService.GetAvailableCars(orderBy, searchItem as localhost.QueryCar);

            PropertyInfo[] properties = typeof(localhost.QueryCar).GetProperties();
            string[] labels = { "Car Plate", "Car Manufacturer", "Car Model", "Start Date", "EndDate", "City" };

            PrintList(labels, properties, cars);

            Set(propIndex, properties, GetAvailableCars, labels.Length, orderBy);
            PrepareOrderBy(orderBy);
        }
        internal void GetCustomers(string orderBy = "CustomId", int propIndex = 1)
        {
            inAppBehavior.MenuItemEntry("List Customers");

            var customers = queryManager.GetCustomers(orderBy, searchItem as SearchCustomer);

            PropertyInfo[] properties = typeof(QueryCustomer).GetProperties();
            string[] labels = { "Client Id", "Client Name", "Birth Date", "Location" };

            PrintList(labels, properties, customers);

            Set(propIndex, properties, GetCustomers, labels.Length, orderBy);
            PrepareOrderBy(orderBy);
        }

        internal void GetVipCustomers(string orderBy = "CustomId", int propIndex = 1)
        {
            inAppBehavior.MenuItemEntry("Vip Customers");

            var customers = queryManager.GetCustomers(orderBy, searchItem as SearchCustomer);

            PropertyInfo[] properties = typeof(QueryCustomer).GetProperties();
            string[] labels = { "Client Id", "Client Name", "Birth Date", "Location" };

            PrintList(labels, properties, customers);

            Set(propIndex, properties, GetCustomers, labels.Length, orderBy);
            PrepareOrderBy(orderBy);
        }

        internal void GetReservations(string orderBy = "Id", int propIndex = 0)
        {
            inAppBehavior.MenuItemEntry("List Rents");

            var reservations = queryManager.GetReservations(orderBy, searchItem as SearchReservation);

            PropertyInfo[] properties = typeof(QueryReservation).GetProperties();
            string[] labels = { "Car plate", "Client Id", "Start Date", "End Date", "Location" };

            PrintList(labels, properties, reservations);

            Set(propIndex, properties, GetReservations, labels.Length, orderBy);
            PrepareOrderBy(orderBy);
        }

        private void PrintList<U>(string[] labels, PropertyInfo[] properties, U[] items)
        {
            int labelsLength = labels.Length;

            StringBuilder sb = new StringBuilder();
            foreach (U item in items)
            {
                sb.Append("------------------------------------------------\n");
                for (int i = 0; i < labelsLength; ++i)
                {
                    var value = properties[i].GetValue(item, null);
                    DateTime temp;
                    if(DateTime.TryParse(value.ToString(), out temp))
                    {
                        value = ((DateTime)value).ToString("dd-MM-yyyy");                        
                    }

                    sb.Append(labels[i] + ": " + value + "\n");
                }
            }
            sb.Append("\n");
            for (int i = 1; i <= labelsLength; ++i)
            {
                sb.Append(i.ToString() + " - Sort by " + labels[i - 1] + "\n");
            }
            sb.Append("\n" + (labelsLength + 1).ToString() + " - Search\n");
            sb.Append((labelsLength + 2).ToString() + " - Quit\n");
            Console.WriteLine(sb);
        }

        private void OrderByOrQuit(bool asc)
        {
            Console.Write("Choose option: ");
            int lastPropIndex = propIndex;
            int.TryParse(Console.ReadLine().Trim(), out propIndex);

            if(propIndex < 1 || propIndex > optionsLength + 2)
            {                
                Console.WriteLine("You entered wrong value");
                inAppBehavior.ContinueOrQuit(OrderByOrQuit, inAppBehavior.Menu);
                return;
            }

            if (propIndex == optionsLength + 2)
            {
                inAppBehavior.Menu();
                return;
            }

            if (propIndex == optionsLength + 1)
            {
                searcher.Search();
                action(orderBy, lastPropIndex);
                return;
            }

            asc = lastPropIndex == propIndex ? !asc : true;
            orderBy = properties[propIndex - 1].Name;
            if (!asc)
            {
                orderBy += " desc";
            }

            action(orderBy, propIndex);
        }  
    }
}
