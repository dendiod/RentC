﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using RentC.DataAccess.Models.QueryModels;
using RentC.DataAccess;

namespace RentC.ConsoleApp
{
    public class GetAndPrint
    {
        internal void GetAvailableCars(bool asc = true, string orderBy = "Id", int propIndex = 0)
        {
            localhost.AvailableCarsService webService = new localhost.AvailableCarsService();
            localhost.QueryCar[] cars = webService.GetAvailableCars(asc, orderBy);

            PropertyInfo[] properties = GetProps<localhost.QueryCar>();
            string[] labels = { "Car Plate", "Car Manufacturer", "Car Model", "Start Date", "EndDate", "City" };

            PrintList(labels, properties, cars);

            Action<bool, string, int> action = GetAvailableCars;
            OrderByOrQuit(propIndex, asc, properties, action);
        }
        internal void GetCustomers(bool asc = true, string orderBy = "Id", int propIndex = 1)
        {
            SQLRepo<QueryCustomer> data = new SQLRepo<QueryCustomer>(new ModelContext());
            var customers = data.GetCustomers(asc, orderBy);

            PropertyInfo[] properties = GetProps<QueryCustomer>();
            string[] labels = { "Client Id", "Client Name", "Birth Date", "Location"};

            PrintList(labels, properties, customers);

            Action<bool, string, int> action = GetCustomers;
            OrderByOrQuit(propIndex, asc, properties, action);
        }

        internal void GetReservations(bool asc = true, string orderBy = "Id", int propIndex = 0)
        {
            SQLRepo<QueryReservation> data = new SQLRepo<QueryReservation>(new ModelContext());
            var reservations = data.GetReservations(asc, orderBy);

            PropertyInfo[] properties = GetProps<QueryReservation>();
            string[] labels = {"Car plate", "Client Id", "Start Date", "End Date", "Location" };

            PrintList(labels, properties, reservations);

            Action<bool, string, int> action = GetReservations;
            OrderByOrQuit(propIndex, asc, properties, action);
        }

        internal void NewCustomer()
        {
            Console.Write("Client ID: ");
            int id = int.Parse(Console.ReadLine().Trim());
            Console.Write("Client Name: ");
            string name = Console.ReadLine();
            Console.Write("Birth Date: ");
            DateTime birhDate = Convert.ToDateTime(Console.ReadLine().Trim());
            Console.Write("Location: ");
            string location = Console.ReadLine();

            QueryCustomer user = new QueryCustomer { Id = id, Name = name, BirthDate = birhDate, Location = location };

            var results = new List<ValidationResult>();
            var context = new ValidationContext(user);
            if (!Validator.TryValidateObject(user, context, results, true))
            {
                foreach (var error in results)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }

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
                    sb.Append(labels[i] + ": " + properties[i].GetValue(t, null) + "\n");
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

        private void OrderByOrQuit(int propIndex, bool asc, PropertyInfo[]properties, Action<bool, string, int> action)
        {
            int lastPropIndex = propIndex;
            propIndex = int.Parse(Console.ReadLine().Trim());
            asc = lastPropIndex == propIndex ? !asc : true;
            action(asc, properties[propIndex - 1].Name, propIndex);
        }        
    }
}
