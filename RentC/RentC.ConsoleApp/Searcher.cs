using RentC.DataAccess.Models.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RentC.ConsoleApp
{
    internal class Searcher<T>
    {
        private readonly ReadFromConsole reader;
        private readonly CustomValidator validator;
        private readonly T searchItem;

        internal Searcher(InAppBehavior inAppBehavior, T searchItem)
        {
            reader = new ReadFromConsole();
            validator = new CustomValidator(inAppBehavior);
            this.searchItem = searchItem;
        } 

        internal void Search(bool temp = true)
        {
            Type type = searchItem.GetType();

            if (type == typeof(SearchCustomer))
            {
                ReadCustomer();
            }
            else if(type == typeof(SearchReservation))
            {
                ReadReservation();
            }
            else
            {
                ReadCar();
            }
        }

        private void SetProps<U>(U item)
        {
            if (!validator.IsValid(item, Search, true))
            {
                return;
            }

            PropertyInfo[] properties = typeof(T).GetProperties();
            for (int i = 0; i < properties.Length; ++i)
            {
                var value = properties[i].GetValue(item, null);
                properties[i].SetValue(searchItem, value);
            }
        }

        private void ReadCustomer()
        {
            int? id = reader.ReadIntOptional("Client Id: ");
            string name = reader.ReadStringOptional("Client Name: ");
            DateTime? birthDate = reader.ReadDateOptional("Birth Date: ");
            string location = reader.ReadStringOptional("Location: ");

            var customer = new SearchCustomer();
            customer.CustomId = id;
            customer.Name = name;
            customer.BirthDate = birthDate;
            customer.Location = location;

            SetProps(customer);
        }

        private void ReadReservation()
        {
            string plate = reader.ReadStringOptional("Car Plate: ");
            int? id = reader.ReadIntOptional("Client Id: ");
            DateTime? startDate = reader.ReadDateOptional("Start Date: ");
            DateTime? endDate = reader.ReadDateOptional("End Date: ");
            string location = reader.ReadStringOptional("Location: ");

            var reservation = new SearchReservation();
            reservation.Plate = plate;
            reservation.CustomerId = id;
            reservation.StartDate = startDate;
            reservation.EndDate = endDate;
            reservation.Location = location;

            SetProps(reservation);
        }

        private void ReadCar()
        {
            string plate = reader.ReadStringOptional("Car Plate: ");
            string manufacturer = reader.ReadStringOptional("Car Manufacturer: ");
            string model = reader.ReadStringOptional("Car Model: ");
            DateTime? startDate = reader.ReadDateOptional("Start Date: ");
            DateTime? endDate = reader.ReadDateOptional("End Date: ");
            string location = reader.ReadStringOptional("Location: ");
            DateTime? monthDate = reader.ReadDateOptional("Any date in month: ");
            int? reservCount = reader.ReadIntOptional("Reservations Count : ");

            var car = new DataAccess.Models.QueryModels.QueryCar();
            car.Plate = plate;
            car.Manufacturer = manufacturer;
            car.Model = model;
            car.StartDate = startDate;
            car.EndDate = endDate;
            car.Location = location;
            car.MonthDate = monthDate;
            car.ReservationsCount = reservCount;

            SetProps(car);
        }
    }
}
