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
            Console.Write("Birth Date: ");
            DateTime.TryParseExact(Console.ReadLine().Trim(),
                "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
            return date;
        } 

        internal void NewCustomer()
        {
            Console.Clear();
            int id = ReadInt("Client ID: ");
            string name = ReadString("Client Name: ");
            DateTime birthDate = ReadDate("Birth Date: ");
            string location = ReadString("Location: ");

            QueryCustomer queryCustomer = new QueryCustomer { CustomId = id, Name = name, BirthDate = birthDate, Location = location};

            var results = new List<ValidationResult>();
            var context = new ValidationContext(queryCustomer);
            if (!Validator.TryValidateObject(queryCustomer, context, results, true))
            {
                foreach (var error in results)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                inAppBehavior.ContinueOrQuit(NewCustomer, inAppBehavior.Menu);
            }
            else
            {
                IRepo<Location> locationRepo = new SQLRepo<Location>(modelContext);

                Customer customer = new Customer { CustomId = id, Name = name, BirthDate = birthDate};
                if(location != "")
                {
                    Location loc = locationRepo.FirstOrDefault(x => x.Name == location);
                    if(loc != null)
                    {
                        customer.LocationId = loc.Id;
                    }
                    else
                    {
                        int locatId = locationRepo.Collection().Count() + 1;
                        locationRepo.Insert(new Location {Id = locatId, Name = location });
                        locationRepo.Commit();
                        customer.LocationId = locatId;
                    }
                }

                IRepo<Customer> customerRepo = new SQLRepo<Customer>(modelContext);
                customerRepo.Insert(customer);
                customerRepo.Commit();
            }
        }

        internal void NewReservation()
        {
            string plate = ReadString("Car Plate: ");
            int customerId = ReadInt("Client ID: ");
            DateTime startDate = ReadDate("Start Date: ");
            DateTime endDate = ReadDate("End Date: ");            
            string location = ReadString("City: ");

            QueryReservation queryReservation = new QueryReservation
            {
                Plate = plate,
                CustomerId = customerId,
                StartDate = startDate,
                EndDate = endDate,
                Location = location
            };

            var results = new List<ValidationResult>();
            var context = new ValidationContext(queryReservation);
            if (!Validator.TryValidateObject(queryReservation, context, results, true))
            {
                foreach (var error in results)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                inAppBehavior.ContinueOrQuit(NewReservation, inAppBehavior.Menu);
            }
            else
            {
                IRepo<Car> carRepo = new SQLRepo<Car>(modelContext);
                int carId = carRepo.FirstOrDefault(x => x.Plate == plate).Id;

                IRepo<Location> locationRepo = new SQLRepo<Location>(modelContext);
                int locationId = locationRepo.FirstOrDefault(x => x.Name == location).Id;

                Reservation reservation = new Reservation
                {
                    CarId = carId,
                    CustomerId = customerId,
                    StartDate = startDate,
                    EndDate = endDate,
                    LocationId = locationId                    
                };

                IRepo<Reservation> reservationRepo = new SQLRepo<Reservation>(modelContext);
                reservationRepo.Insert(reservation);
                reservationRepo.Commit();
            }
        }
    }
}
