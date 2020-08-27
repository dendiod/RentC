using RentC.DataAccess;
using RentC.DataAccess.Contracts;
using RentC.DataAccess.Models;
using RentC.DataAccess.Models.QueryModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace RentC.ConsoleApp
{
    public class InsertUpdate
    {
        private ModelContext modelContext;
        private InAppBehavior inAppBehavior;
        private ContextManager contextManager;
        public InsertUpdate(ModelContext modelContext, InAppBehavior inAppBehavior)
        {
            this.modelContext = modelContext;
            this.inAppBehavior = inAppBehavior;
            this.contextManager = new ContextManager(modelContext);
        }

        private int ReadInt(string message)
        {
            Console.Write(message);
            int num;
            int.TryParse(Console.ReadLine().Trim(), out num);
            return num;
        }

        private string ReadString(string message)
        {
            Console.Write(message);
            return Console.ReadLine();
        }

        private DateTime ReadDate(string message)
        {
            DateTime date;
            Console.Write(message);
            DateTime.TryParseExact(Console.ReadLine().Trim(),
                "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
            return date;
        } 

        private void PushModel<T>(T model, bool creating, Action<bool, T> nextAction, Action<bool>curAction)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(model);
            if (!Validator.TryValidateObject(model, context, results, true))
            {
                Console.WriteLine();
                foreach (var error in results)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                inAppBehavior.ContinueOrQuit(curAction, inAppBehavior.Menu, creating);
                return;
            }
            
            nextAction(creating, model);
            inAppBehavior.Menu();
        }

        internal void ManageCustomers(bool isCreating)
        {
            inAppBehavior.MenuItemEntry("Register new Customer", "Update Customer", isCreating);

            int id = ReadInt("Client ID: ");
            string name = ReadString("Client Name: ");
            DateTime birthDate = ReadDate("Birth Date: ");
            string location = ReadString("Location: ");

            QueryCustomer queryCustomer = new QueryCustomer 
            { 
                CustomId = id, 
                Name = name,
                BirthDate = birthDate, 
                Location = location,
                IsCreating = isCreating
            };

            PushModel(queryCustomer, isCreating, contextManager.ManageCustomers, ManageCustomers);
        }

        internal void ManageReservations(bool isCreating)
        {
            inAppBehavior.MenuItemEntry("Register new Car Rent", "Update Car Rent", isCreating);

            int id = 1;
            if (!isCreating)
            {
                id = ReadInt("Reservation ID: ");
            }            
            DateTime startDate = ReadDate("Start Date: ");
            DateTime endDate = ReadDate("End Date: ");         

            QueryReservation queryReservation = new QueryReservation
            {
                Id = id,
                StartDate = startDate,
                EndDate = endDate,
                IsCreating = isCreating
            };

            if (isCreating)
            {
                string plate = ReadString("Car Plate: ");
                int customerId = ReadInt("Client ID: ");
                string location = ReadString("City: ");
                queryReservation.Plate = plate;
                queryReservation.CustomerId = customerId;
                queryReservation.Location = location;
            }

            PushModel(queryReservation, isCreating, contextManager.ManageReservations, ManageReservations);
        }        
    }
}
