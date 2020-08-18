using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using RentC.Core.Models.QueryModels;
using RentC.DataAccess;

namespace RentC.ConsoleApp
{
    public class GetAndPrint
    {
        //internal static void GetCustomers(bool asc, Func<QueryCustomer, T> orderByFunc)
        //{
        //    var customers = Data<T>.GetCustomers(asc, orderByFunc);

        //    StringBuilder sb = new StringBuilder();
        //    string[] strs = { "Customer Id", "Client Name", "Birth Date", "Location" };
        //    foreach (QueryCustomer c in customers)
        //    {
        //        sb.Append("------------------------------------------------\n");
        //        sb.Append(strs[0] + ": " + c.Id + "\n");
        //        sb.Append("Car manufacturer: " + c.Name + "\n");
        //        sb.Append("Car model: " + c.BirthDate + "\n");
        //        sb.Append("Start Date: " + c.Location + "\n");
        //    }
        //    sb.Append("\nSort by\n");
        //    sb.Append("1-Car plate\n");
        //    sb.Append("2-Car model\n");
        //    sb.Append("3-Car manufacturer\n");
        //    sb.Append("4-Start Date\n");
        //    sb.Append("5-End Date\n");
        //    sb.Append("6-City\n");
        //    sb.Append("7-Quit\n");
        //    Console.WriteLine(sb);
        //}

        //internal static void GetReservations(bool asc, Func<QueryReservation, int> func)
        //{
        //    var queryReservations = Data.GetReservations(true, func);
        //    Console.WriteLine(queryReservations[0].StartDate);
        //}

        //internal static void NewCustomer()
        //{
        //    Console.Write("Client ID: ");
        //    int id;
        //    bool idIsInt = int.TryParse(Console.ReadLine().Trim(), out id);
        //    if (!idIsInt)
        //    {
        //        Console.WriteLine("Id must be integer");
        //        ContinueOrQuit(NewCustomer, Menu);
        //    }
        //    if (Data.IsCustomerExists(id) != null)
        //    {
        //        Console.WriteLine("Customer with this id already exists");
        //        ContinueOrQuit(NewCustomer, Menu);
        //    }
        //    Console.Write("Client Name: ");
        //    string name = Console.ReadLine();
        //    Console.Write("Birth Date: ");

        //}

        private PropertyInfo[] GetProps<T>()
        {
            return typeof(T).GetProperties();
        }

        private void PrintList<T>(string[] options, PropertyInfo[] properties, T[] items)
        {
            int optionsLength = options.Length;

            StringBuilder sb = new StringBuilder();
            foreach (T t in items)
            {
                sb.Append("------------------------------------------------\n");
                for (int i = 0; i < optionsLength; ++i)
                {
                    sb.Append(options[i] + ": " + properties[i].GetValue(t, null) + "\n");
                }
            }
            sb.Append("\nSort by\n");
            for (int i = 1; i <= optionsLength; ++i)
            {
                sb.Append(i.ToString() + " - " + options[i - 1] + "\n");
            }
            sb.Append((optionsLength + 1).ToString() + " - Quit\n");
            Console.WriteLine(sb);
        }

        internal void GetAvailableCars(bool asc, string orderBy, int propIndex = 0)
        {
            localhost.AvailableCarsService webService = new localhost.AvailableCarsService();
            localhost.QueryCar[] cars = webService.GetAvailableCars(asc, orderBy);

            PropertyInfo[] properties = GetProps<localhost.QueryCar>();
            string[] options = {"Car Plate", "Car Manufacturer", "Car Model", "Start Date", "EndDate", "City" };

            PrintList<localhost.QueryCar>(options, properties, cars);

            int lastPropIndex = propIndex;
            propIndex = int.Parse(Console.ReadLine().Trim());
            asc = lastPropIndex == propIndex ? !asc : true;
            GetAvailableCars(asc, properties[propIndex-1].Name, propIndex);
        }
    }
}
