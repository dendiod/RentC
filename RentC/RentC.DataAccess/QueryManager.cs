using RentC.DataAccess.Models;
using RentC.DataAccess.Models.QueryModels;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Runtime.CompilerServices;
using System.Web;

namespace RentC.DataAccess
{    
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
        public QueryCar[] GetAvailableCars(string orderBy, string plate, string manufacturer,
            string model, DateTime? startDate, DateTime? endDate, string location)
        {
            bool isPlateEmpty = string.IsNullOrWhiteSpace(plate);
            bool isManufacturerEmpty = string.IsNullOrWhiteSpace(manufacturer);
            bool isModelEmpty = string.IsNullOrWhiteSpace(model);
            bool isLocationmpty = string.IsNullOrWhiteSpace(location);
            if (startDate == null)
            {
                startDate = DateTime.Today;
            }

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
                           EndDate = reservation != null ? reservation.EndDate : DateTime.MaxValue,
                           Location = locationTable.Name
                       }).ToList();

            var reservations = context.Reservations.Where(x => x.StartDate <= startDate && x.EndDate >= startDate)
                .Select(x => x.CarId).Distinct().OrderByDescending(x => x);

            for(int i = cars.Count - 1; i >= 0; --i)
            {
                foreach(int num in reservations)
                {
                    if(cars[i].Id == num)
                    {
                        cars.RemoveAt(i);
                        ++i;
                        break;
                    }                    
                }
            }

            if(cars.Count == 0)
            {
                return OrderItems(cars.AsQueryable(), orderBy);
            }

            List<QueryCar> newCars = new List<QueryCar>();

            var orderedCars = cars.OrderBy(x => x.Id);

            cars = orderedCars.ToList();

            QueryCar carToAdd = cars[0];
            bool repetition = false;
            for(int i = 1; i < cars.Count; ++i)
            {
                if(cars[i].Id == cars[i - 1].Id)
                {
                    repetition = true;
                    if(cars[i].StartDate > startDate)
                    {
                        if(carToAdd.StartDate <= startDate)
                        {
                            carToAdd = cars[i];
                        }
                        else if(cars[i].StartDate < carToAdd.StartDate)
                        {
                            carToAdd = cars[i];
                        }
                    }
                }
                else
                {
                    if (repetition)
                    {
                        carToAdd.EndDate = carToAdd.StartDate;
                        carToAdd.StartDate = (DateTime)startDate;                        
                    }
                    repetition = false;

                    newCars.Add(carToAdd);
                    carToAdd = cars[i];
                }
            }
            newCars.Add(carToAdd);

            return OrderItems(newCars.AsQueryable(), orderBy);
        }

        public QueryReservation[] GetReservations(string orderBy, string plate, int? customerId, DateTime? startDate,
            DateTime? endDate, string location)
        {
            bool isPlateEmpty = string.IsNullOrWhiteSpace(plate);
            bool isLocationEmpty = string.IsNullOrWhiteSpace(location);

            var reservations = from reservation in context.Reservations
                               join locationTable in context.Locations on reservation.LocationId equals locationTable.Id
                               join car in context.Cars on reservation.CarId equals car.Id
                               where (customerId == null || reservation.CustomerId == customerId) &&
                               (startDate == null || reservation.StartDate == startDate) &&
                               (endDate == null || reservation.EndDate == endDate) &&
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

        public QueryCustomer[] GetCustomers(string orderBy, int? customId, string name, DateTime? birthDate, string location)
        {
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