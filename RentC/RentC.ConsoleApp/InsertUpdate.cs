using RentC.DataAccess;
using RentC.DataAccess.Contracts;
using RentC.DataAccess.Models;
using RentC.DataAccess.Models.QueryModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentC.ConsoleApp
{
    public class InsertUpdate
    {
        private ModelContext modelContext;
        private InAppBehavior inAppBehavior;
        public InsertUpdate(ModelContext modelContext, InAppBehavior inAppBehavior)
        {
            this.modelContext = modelContext;
            this.inAppBehavior = inAppBehavior;
        }
        internal void NewCustomer()
        {
            Console.Write("Client ID: ");
            int id;
            int.TryParse(Console.ReadLine().Trim(), out id);

            Console.Write("Client Name: ");
            string name = Console.ReadLine();

            Console.Write("Birth Date: ");
            DateTime birhDate;
            DateTime.TryParseExact(Console.ReadLine().Trim(),
                "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out birhDate);

            Console.Write("Location: ");
            string location = Console.ReadLine();

            QueryCustomer user = new QueryCustomer { Id = id, Name = name, BirthDate = birhDate, Location = location};

            var results = new List<ValidationResult>();
            var context = new ValidationContext(user);
            if (!Validator.TryValidateObject(user, context, results, true))
            {
                foreach (var error in results)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                inAppBehavior.ContinueOrQuit(inAppBehavior.Menu, () => { });
            }
            else
            {
                IRepo<Location> locationRepo = new SQLRepo<Location>(modelContext);

                Customer customer = new Customer { Id = id, Name = name, BirthDate = birhDate};
                if(location != "")
                {
                    customer.LocationId = locationRepo.FirstOrDefault(x => x.Name == location).Id;
                }

                IRepo<Customer> customerRepo = new SQLRepo<Customer>(modelContext);
                customerRepo.Insert(customer);
                customerRepo.Commit();
            }
        }
    }
}
