using RentC.DataAccess.Models;
using RentC.DataAccess.Models.QueryModels;
using RentC.DataAccess.Models.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace RentC.DataAccess
{
    class CarComparer : IEqualityComparer<QueryCar>
    {
        public bool Equals(QueryCar x, QueryCar y)
        {
            //Check whether the compared objects reference the same data.
            if (Object.ReferenceEquals(x, y)) return true;

            //Check whether any of the compared objects is null.
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            //Check whether the products' properties are equal.
            return x.Id == y.Id;
        }

        public int GetHashCode(QueryCar car)
        {
            //Check whether the object is null
            if (Object.ReferenceEquals(car, null)) return 0;

            //Get hash code for the CarId field if it is not null.
            int hasCarId = car.Id;

            //Calculate the hash code for the item.
            return hasCarId;
        }
    }
    public class QueryManager
    {
        private ModelContext context;

        public QueryManager(ModelContext modelContext)
        {
            context = modelContext;
        }

        private T[] OrderItems<T>(IQueryable<T> items, string orderBy)
        {
            items = items.OrderBy(orderBy);

            return items.ToArray();
        }

        public List<QueryCar> RemoveNotAvailable(bool isEndDateNull, List<QueryCar> cars, DateTime startDate, DateTime endDate)
        {
            Func<Reservation, bool> predicate;
            if (!isEndDateNull)
            {
                predicate = x => (x.StartDate >= startDate && x.StartDate <= endDate) ||
                (x.EndDate >= startDate && x.EndDate <= endDate);
            }
            else
            {
                predicate = x => x.StartDate <= startDate && x.EndDate >= startDate;
            }

            var reservations = context.Reservations.Where(predicate)
               .Select(x => x.CarId).Distinct().OrderByDescending(x => x);

            for (int i = cars.Count - 1; i >= 0; --i)
            {
                foreach (int num in reservations)
                {
                    if (cars[i].Id == num)
                    {
                        cars.RemoveAt(i);
                        ++i;
                        break;
                    }
                }
            }

            return cars;
        }

        public List<QueryCar> SelectNearestReservation(List<QueryCar> newCars, List<QueryCar> cars, DateTime startDate)
        {
            var orderedCars = cars.OrderBy(x => x.Id);
            cars = orderedCars.ToList();

            QueryCar carToAdd = cars[0];
            for (int i = 1; i < cars.Count; ++i)
            {
                if (cars[i].Id == cars[i - 1].Id)
                {
                    if (cars[i].StartDate > startDate)
                    {
                        if (carToAdd.StartDate <= startDate)
                        {
                            carToAdd = cars[i];
                        }
                        else if (cars[i].StartDate < carToAdd.StartDate)
                        {
                            carToAdd = cars[i];
                        }
                    }
                }
                else
                {
                    carToAdd.EndDate = carToAdd.StartDate != startDate ? carToAdd.StartDate : DateTime.MaxValue;
                    carToAdd.StartDate = startDate;

                    newCars.Add(carToAdd);
                    carToAdd = cars[i];
                }
            }
            newCars.Add(carToAdd);

            return newCars;
        }

        public QueryCar[] GetAvailableCars(string orderBy, QueryCar searchCar)
        {
            string plate = searchCar.Plate;
            string manufacturer = searchCar.Manufacturer;
            string model = searchCar.Model;
            string location = searchCar.Location;
            DateTime? startDate = searchCar.StartDate;
            DateTime? endDate = searchCar.EndDate;

            bool isPlateEmpty = string.IsNullOrWhiteSpace(plate);
            bool isManufacturerEmpty = string.IsNullOrWhiteSpace(manufacturer);
            bool isModelEmpty = string.IsNullOrWhiteSpace(model);
            bool isLocationmpty = string.IsNullOrWhiteSpace(location);
            bool isStartDateNull = startDate == null;
            bool isEndDateNull = endDate == null;
            
            startDate = !isStartDateNull ? startDate : DateTime.Today;
            endDate = !isEndDateNull ? endDate : DateTime.MaxValue;

            var cars = (from carTable in context.Cars
                       join modelTable in context.Models on carTable.ModelId equals modelTable.Id
                       join manufacturerTable in context.Manufacturers on carTable.ManufacturerId equals manufacturerTable.Id
                       join locationTable in context.Locations on carTable.LocationId equals locationTable.Id
                       join reservation in context.Reservations on carTable.Id equals reservation.CarId into leftJoin
                       from reservation in leftJoin.DefaultIfEmpty()
                       where (!isPlateEmpty && carTable.Plate == plate || isPlateEmpty) &&
                       (!isManufacturerEmpty && manufacturerTable.Name == manufacturer || isManufacturerEmpty) &&
                       (!isModelEmpty && modelTable.Name == model || isModelEmpty) &&
                       (!isLocationmpty && locationTable.Name == location || isLocationmpty)

                       select new QueryCar
                       {
                           Id = carTable.Id,
                           Plate = carTable.Plate,
                           Model = modelTable.Name,
                           Manufacturer = manufacturerTable.Name,
                           StartDate = (DateTime)(reservation != null ? reservation.StartDate : startDate),
                           EndDate = (DateTime)(reservation != null ? reservation.EndDate : endDate),
                           Location = locationTable.Name
                       }).ToList();            

            RemoveNotAvailable(isEndDateNull, cars, (DateTime)startDate, (DateTime)endDate);            

            if(cars.Count == 0)
            {
                return OrderItems(cars.AsQueryable(), orderBy);
            }

            List<QueryCar> newCars = !isEndDateNull ? cars.Distinct(new CarComparer()).ToList() : 
                SelectNearestReservation(new List<QueryCar>(), cars, (DateTime)startDate);

            if (!isEndDateNull)
            {
                foreach(QueryCar c in newCars)
                {
                    c.StartDate = (DateTime)startDate;
                    c.EndDate = (DateTime)endDate;
                }
            }

            return OrderItems(newCars.AsQueryable(), orderBy);
        }

        public QueryReservation[] GetReservations(string orderBy, SearchReservation r)
        {
            string plate = r.Plate;
            string location = r.Location;
            int? customerId = r.CustomerId;
            DateTime? startDate = r.StartDate;
            DateTime? endDate = r.EndDate;

            bool isPlateEmpty = string.IsNullOrWhiteSpace(plate);
            bool isLocationEmpty = string.IsNullOrWhiteSpace(location);
            bool isStartDateNull = startDate == null;
            bool isEndDateNull = endDate == null;

            var reservations = from reservation in context.Reservations
                               join locationTable in context.Locations on reservation.LocationId equals locationTable.Id
                               join car in context.Cars on reservation.CarId equals car.Id
                               where (customerId == null || reservation.CustomerId == customerId) &&
                               (startDate == null || reservation.StartDate >= startDate) &&
                               (endDate == null || reservation.EndDate <= endDate) &&
                               (!isPlateEmpty && car.Plate == plate || isPlateEmpty) &&
                               (!isLocationEmpty && locationTable.Name == location || isLocationEmpty)

                               select new QueryReservation
                               {
                                   Id = reservation.Id,
                                   Plate = car.Plate,
                                   CustomerId = reservation.CustomerId,
                                   StartDate = reservation.StartDate,
                                   EndDate = reservation.EndDate,
                                   Location = locationTable.Name
                               };

            return OrderItems(reservations, orderBy);
        }

        public QueryCustomer[] GetCustomers(string orderBy, SearchCustomer c)
        {
            int? customId = c.CustomId;
            string name = c.Name;
            string location = c.Location;            
            DateTime? birthDate = c.BirthDate;

            bool isNameEmpty = string.IsNullOrWhiteSpace(name);
            bool isLocationEmpty = string.IsNullOrWhiteSpace(location);

            var customers = from customer in context.Customers
                            join locationTable in context.Locations on customer.LocationId equals locationTable.Id into leftJoin
                            from locationJoined in leftJoin.DefaultIfEmpty()
                            where (customId == null || customer.CustomId == customId) &&
                            (birthDate == null || customer.BirthDate == birthDate) &&
                            (!isNameEmpty && customer.Name == name || isNameEmpty) &&
                            (!isLocationEmpty && locationJoined.Name == location || isLocationEmpty)

                            select new QueryCustomer
                            {
                                CustomId = customer.CustomId,
                                Name = customer.Name,
                                BirthDate = customer.BirthDate,
                                Location = locationJoined.Name ?? "undefined"
                            };

            return OrderItems(customers, orderBy);
        }
    }
}