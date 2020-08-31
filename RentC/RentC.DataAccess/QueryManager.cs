using RentC.DataAccess.Contracts;
using RentC.DataAccess.Models;
using RentC.DataAccess.Models.QueryModels;
using RentC.DataAccess.Models.Search;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Security.Cryptography.X509Certificates;

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
        private IRepo<Car> carRepo;
        private IRepo<Customer> customerRepo;
        private IRepo<Reservation> reservationRepo;
        private IRepo<Location> locationRepo;
        private IRepo<Model> modelRepo;
        private IRepo<Manufacturer> manufacturerRepo;

        private List<Car> carsList;
        private List<Model> modelsList;
        private List<Manufacturer> manufacturersList;
        private List<Location> locationsList;
        private List<Customer> customersList;
        private List<Reservation> reservationsList;

        public QueryManager(IRepo<Car> carRepo, IRepo<Customer> customerRepo,
            IRepo<Reservation> reservationRepo, IRepo<Location> locationRepo,
            IRepo<Model> modelRepo, IRepo<Manufacturer> manufacturerRepo)
        {
            this.carRepo = carRepo;
            this.customerRepo = customerRepo;
            this.reservationRepo = reservationRepo;
            this.locationRepo = locationRepo;
            this.modelRepo = modelRepo;
            this.manufacturerRepo = manufacturerRepo;

            carsList = carRepo.Collection().ToList();
            modelsList = modelRepo.Collection().ToList();
            manufacturersList = manufacturerRepo.Collection().ToList();
            locationsList = locationRepo.Collection().ToList();
            customersList = customerRepo.Collection().ToList();
            reservationsList = reservationRepo.Collection().ToList();
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

            var reservations = reservationsList.Where(predicate)
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

            var cars = (from carTable in carsList
                       join modelTable in modelsList on carTable.ModelId equals modelTable.Id
                       join manufacturerTable in manufacturersList on carTable.ManufacturerId equals manufacturerTable.Id
                       join locationTable in locationsList on carTable.LocationId equals locationTable.Id
                       join reservation in reservationsList on carTable.Id equals reservation.CarId into leftJoin
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

            var reservations = from reservation in reservationsList
                               join locationTable in locationsList on reservation.LocationId equals locationTable.Id
                               join car in carsList on reservation.CarId equals car.Id
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

            return OrderItems(reservations.AsQueryable(), orderBy);
        }

        public QueryCustomer[] GetCustomers(string orderBy, SearchCustomer c)
        {
            var customers = GetUnorderedCustomers(c).AsQueryable();
            return OrderItems(customers, orderBy);
        }

        private List<QueryCustomer> GetUnorderedCustomers(SearchCustomer c)
        {
            int? customId = c.CustomId;
            string name = c.Name;
            string location = c.Location;
            DateTime? birthDate = c.BirthDate;

            bool isNameEmpty = string.IsNullOrWhiteSpace(name);
            bool isLocationEmpty = string.IsNullOrWhiteSpace(location);

            var customers = customerRepo.Collection().ToList();
            var locations = locationRepo.Collection().ToList();

            return (from customer in customers
                   join locationTable in locations on customer.LocationId equals locationTable.Id into leftJoin
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
                       Location = locationJoined != null ? locationJoined.Name : "undefined",
                       ReservationsCount = 0,
                       Status = "undefined"
                   }).ToList();
        }

        public QueryCustomer[] GetVipCustomers(string orderBy, SearchCustomer c)
        {
            var reservations = from reservation in reservationsList
                               let subtraction = DateTime.Today.Subtract(reservation.StartDate)
                               where subtraction.Days <= 30 && subtraction.Days >= 0
                               group reservation by reservation.CustomerId into g

                               select new
                               {
                                   CustomerId = g.Key,
                                   ReservationsCount = g.Count()
                               };

            var tempCustomers = GetUnorderedCustomers(c);

            var customers = from customer in tempCustomers
                            join reservation in reservations on customer.CustomId equals reservation.CustomerId
                            where reservation.ReservationsCount > 1

                                  select new QueryCustomer
                                  {
                                      CustomId = customer.CustomId,
                                      Name = customer.Name,
                                      BirthDate = customer.BirthDate,
                                      Location = customer.Location,
                                      ReservationsCount = reservation.ReservationsCount,
                                      Status = reservation.ReservationsCount < 4 ? "Silver" : "Gold"
                                  };

            return OrderItems(customers.AsQueryable(), orderBy);
        }

        public QueryCar[] GetRecentCars(string orderBy, QueryCar searchCar)
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

            DateTime today = DateTime.Today;
            startDate = startDate ?? today;
            endDate = endDate ?? DateTime.MaxValue;

            var tempCars = (from carTable in carsList
                            join modelTable in modelsList on carTable.ModelId equals modelTable.Id
                            join manufacturerTable in manufacturersList on carTable.ManufacturerId equals manufacturerTable.Id
                            join locationTable in locationsList on carTable.LocationId equals locationTable.Id
                            join reservation in reservationsList on carTable.Id equals reservation.CarId
                            
                            where (!isPlateEmpty && carTable.Plate == plate || isPlateEmpty) &&
                            (!isManufacturerEmpty && manufacturerTable.Name == manufacturer || isManufacturerEmpty) &&
                            (!isModelEmpty && modelTable.Name == model || isModelEmpty) &&
                            (!isLocationmpty && locationTable.Name == location || isLocationmpty) &&
                            (reservation.StartDate <= today)
                            
                            select new QueryCar
                            {
                                Id = carTable.Id,
                                Plate = carTable.Plate,
                                Model = modelTable.Name,
                                Manufacturer = manufacturerTable.Name,
                                StartDate = (DateTime)(reservation != null ? reservation.StartDate : startDate),
                                EndDate = (DateTime)(reservation != null ? reservation.EndDate : endDate),
                                Location = locationTable.Name
                            }).OrderByDescending(x => x.StartDate).ToList();

            var cars = tempCars.Distinct(new CarComparer());

            return OrderItems(cars.AsQueryable(), orderBy);
        }

        public QueryCar[] GetCarsInMonth(string orderBy, QueryCar searchCar)
        {
            string plate = searchCar.Plate;
            string manufacturer = searchCar.Manufacturer;
            string model = searchCar.Model;
            string location = searchCar.Location;
            DateTime? startDate = searchCar.StartDate;
            DateTime? endDate = searchCar.EndDate;
            DateTime? monthDate = searchCar.MonthDate;

            bool isPlateEmpty = string.IsNullOrWhiteSpace(plate);
            bool isManufacturerEmpty = string.IsNullOrWhiteSpace(manufacturer);
            bool isModelEmpty = string.IsNullOrWhiteSpace(model);
            bool isLocationmpty = string.IsNullOrWhiteSpace(location);

            DateTime today = DateTime.Today;
            startDate = startDate ?? today;
            endDate = endDate ?? DateTime.MaxValue;
            monthDate = monthDate ?? today;

            var reservationsGroup = from reservation in reservationsList
                               where monthDate.Value.Month == reservation.StartDate.Month &&
                               monthDate.Value.Year == reservation.StartDate.Year
                               group reservation by reservation.CarId into g

                               select new
                               {
                                   CarId = g.Key,
                                   ReservationsCount = g.Count()
                               };

            var cars = from carTable in carsList
                       join modelTable in modelsList on carTable.ModelId equals modelTable.Id
                       join manufacturerTable in manufacturersList on carTable.ManufacturerId equals manufacturerTable.Id
                       join locationTable in locationsList on carTable.LocationId equals locationTable.Id
                       join reservationGroup in reservationsGroup on carTable.Id equals reservationGroup.CarId

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
                           Location = locationTable.Name,
                           ReservationsCount = reservationGroup.ReservationsCount,
                           MonthDate = monthDate,
                           StartDate = monthDate,
                           EndDate = monthDate
                       };

            return OrderItems(cars.AsQueryable(), orderBy);
        }
    }
}