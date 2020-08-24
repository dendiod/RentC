using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using RentC.DataAccess.Models.QueryModels;
using RentC.DataAccess;

namespace RentC.ConsoleApp
{    
    public class GetAndPrint
    {
        private ModelContext modelContext;
        private QueryManager queryManager;
        private InAppBehavior inAppBehavior;

        private int propIndex;
        private PropertyInfo[] properties;
        private Action<bool, string, int> action;
        private int optionsLength;

        private void Set(int propIndex, PropertyInfo[]properties, Action<bool, string, int> action, int optionsLength)
        {
            this.propIndex = propIndex;
            this.properties = properties;
            this.action = action;
            this.optionsLength = optionsLength;
        }

        public GetAndPrint(ModelContext modelContext, InAppBehavior inAppBehavior)
        {
            this.modelContext = modelContext;
            queryManager = new QueryManager(modelContext);
            this.inAppBehavior = inAppBehavior;
        }
        internal void GetAvailableCars(bool asc = true, string orderBy = "Id", int propIndex = 0)
        {
            inAppBehavior.MenuItemEntry("List Available Cars");

            localhost.AvailableCarsService webService = new localhost.AvailableCarsService();
            localhost.QueryCar[] cars = webService.GetAvailableCars(asc, orderBy);

            PropertyInfo[] properties = GetProps<localhost.QueryCar>();
            string[] labels = { "Car Plate", "Car Manufacturer", "Car Model", "Start Date", "EndDate", "City" };

            PrintList(labels, properties, cars);

            Set(propIndex, properties, GetAvailableCars, labels.Length);
            OrderByOrQuit(asc);
        }
        internal void GetCustomers(bool asc = true, string orderBy = "CustomId", int propIndex = 1)
        {
            inAppBehavior.MenuItemEntry("List Customers");

            var customers = queryManager.GetCustomers(asc, orderBy);

            PropertyInfo[] properties = GetProps<QueryCustomer>();
            string[] labels = { "Client Id", "Client Name", "Birth Date", "Location"};

            PrintList(labels, properties, customers);

            Set(propIndex, properties, GetCustomers, labels.Length);
            OrderByOrQuit(asc);
        }

        internal void GetReservations(bool asc = true, string orderBy = "Id", int propIndex = 0)
        {
            inAppBehavior.MenuItemEntry("List Rents");

            var reservations = queryManager.GetReservations(asc, orderBy);

            PropertyInfo[] properties = GetProps<QueryReservation>();
            string[] labels = {"Car plate", "Client Id", "Start Date", "End Date", "Location" };

            PrintList(labels, properties, reservations);

            Set(propIndex, properties, GetReservations, labels.Length);
            OrderByOrQuit(asc);
        }        

        private PropertyInfo[] GetProps<T>()
        {
            return typeof(T).GetProperties();
        }

        private void PrintList<T>(string[] labels, PropertyInfo[] properties, T[] items)
        {
            int labelsLength = labels.Length;

            StringBuilder sb = new StringBuilder();
            foreach (T t in items)
            {
                sb.Append("------------------------------------------------\n");
                for (int i = 0; i < labelsLength; ++i)
                {
                    var value = properties[i].GetValue(t, null);                    
                    if(properties[i].PropertyType.Name == "DateTime")
                    {
                        value = ((DateTime)value).ToString("dd-MM-yyyy");                        
                    }

                    sb.Append(labels[i] + ": " + value + "\n");
                }
            }
            sb.Append("\nSort by\n");
            for (int i = 1; i <= labelsLength; ++i)
            {
                sb.Append(i.ToString() + " - " + labels[i - 1] + "\n");
            }
            sb.Append((labelsLength + 1).ToString() + " - Quit\n");
            Console.WriteLine(sb);
        }

        private void OrderByOrQuit(bool asc)
        {
            Console.Write("Choose option: ");
            int lastPropIndex = propIndex;
            int.TryParse(Console.ReadLine().Trim(), out propIndex);

            if(propIndex < 1 || propIndex > optionsLength)
            {
                if (propIndex != optionsLength + 1)
                {
                    Console.WriteLine("You entered wrong value");
                    inAppBehavior.ContinueOrQuit(OrderByOrQuit, inAppBehavior.Menu);
                    return;
                }

                inAppBehavior.Menu();
                return;
            }

            asc = lastPropIndex == propIndex ? !asc : true;
            action(asc, properties[propIndex - 1].Name, propIndex);
        }        
    }
}
