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
        private ReadFromConsole reader;
        private CustomValidator validator;

        public InsertUpdate(ModelContext modelContext, InAppBehavior inAppBehavior)
        {
            this.modelContext = modelContext;
            this.inAppBehavior = inAppBehavior;
            this.contextManager = new ContextManager(modelContext);
            reader = new ReadFromConsole();
            validator = new CustomValidator(inAppBehavior);
        }         

        private void PushModel<T>(T model, bool creating, Action<bool, T> nextAction, Action<bool>curAction)
        {
            if(!validator.IsValid(model, curAction, creating))
            {
                return;
            }
            
            nextAction(creating, model);
            inAppBehavior.Menu();
        }

        internal void ManageCustomers(bool isCreating)
        {
            inAppBehavior.MenuItemEntry("Register new Customer", "Update Customer", isCreating);

            int id = reader.ReadInt("Client ID: ");
            string name = reader.ReadString("Client Name: ");
            DateTime birthDate = reader.ReadDate("Birth Date: ");
            string location = reader.ReadString("Location: ");

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
                id = reader.ReadInt("Reservation ID: ");
            }            
            DateTime startDate = reader.ReadDate("Start Date: ");
            DateTime endDate = reader.ReadDate("End Date: ");         

            QueryReservation queryReservation = new QueryReservation
            {
                Id = id,
                StartDate = startDate,
                EndDate = endDate,
                IsCreating = isCreating
            };

            if (isCreating)
            {
                string plate = reader.ReadString("Car Plate: ");
                int customerId = reader.ReadInt("Client ID: ");
                string location = reader.ReadString("City: ");
                queryReservation.Plate = plate;
                queryReservation.CustomerId = customerId;
                queryReservation.Location = location;
            }

            PushModel(queryReservation, isCreating, contextManager.ManageReservations, ManageReservations);
        }        
    }
}
