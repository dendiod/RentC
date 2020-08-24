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
    class CarComparer : IEqualityComparer<Reservation>
    {
        public bool Equals(Reservation x, Reservation y)
        {
            //Check whether the compared objects reference the same data.
            if (Object.ReferenceEquals(x, y)) return true;

            //Check whether any of the compared objects is null.
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            //Check whether the products' properties are equal.
            return x.CarId == y.CarId;
        }

        public int GetHashCode(Reservation car)
        {
            //Check whether the object is null
            if (Object.ReferenceEquals(car, null)) return 0;

            //Get hash code for the CarId field if it is not null.
            int hasCarId = car.CarId;

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

        private T[] OrderItems<T>(bool asc, IQueryable<T> items, string orderBy)
        {
            if (!asc)
            {
                orderBy += " desc";
            }
            items = items.OrderBy(orderBy);

            return items.ToArray();
        }
        public QueryCar[] GetAvailableCars(bool asc, string orderBy)
        {
            DateTime today = DateTime.Today;

            var cars = (from car in context.Cars
                       join model in context.Models on car.ModelId equals model.Id
                       join manufacturer in context.Manufacturers on car.ManufacturerId equals manufacturer.Id
                       join location in context.Locations on car.LocationId equals location.Id
                       join reservation in context.Reservations on car.Id equals reservation.CarId into leftJoin
                       from reservation in leftJoin.DefaultIfEmpty()

                       select new QueryCar
                       {
                           Id = car.Id,
                           Plate = car.Plate,
                           Model = model.Name,
                           Manufacturer = manufacturer.Name,
                           StartDate = reservation != null ? reservation.StartDate : today,
                           EndDate = reservation != null ? reservation.EndDate : DateTime.MaxValue,
                           Location = location.Name
                       }).ToList();

            var reservations = context.Reservations.Where(x => x.StartDate <= today && x.EndDate >= today)
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
                return OrderItems(asc, cars.AsQueryable(), orderBy);
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
                    if(cars[i].StartDate > today)
                    {
                        if(carToAdd.StartDate <= today)
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
                        carToAdd.StartDate = today;                        
                    }
                    repetition = false;

                    newCars.Add(carToAdd);
                    carToAdd = cars[i];
                }
            }
            newCars.Add(carToAdd);

            return OrderItems(asc, newCars.AsQueryable(), orderBy);
        }

        public QueryReservation[] GetReservations(bool asc, string orderBy)
        {
            var reservations = from reservation in context.Reservations
                               join location in context.Locations on reservation.LocationId equals location.Id
                               join car in context.Cars on reservation.CarId equals car.Id

                               select new QueryReservation
                               {
                                   Id = reservation.Id,
                                   Plate = car.Plate,
                                   CustomerId = reservation.CustomerId,
                                   StartDate = reservation.StartDate,
                                   EndDate = reservation.EndDate,
                                   Location = location.Name
                               };

            return OrderItems(asc, reservations, orderBy);
        }

        public QueryCustomer[] GetCustomers(bool asc, string orderBy)
        {
            var customers = from customer in context.Customers
                            join location in context.Locations on customer.LocationId equals location.Id into leftJoin

                            from location in leftJoin.DefaultIfEmpty()

                            select new QueryCustomer
                            {
                                CustomId = customer.CustomId,
                                Name = customer.Name,
                                BirthDate = customer.BirthDate,
                                Location = location.Name ?? "undefined"
                            };

            return OrderItems(asc, customers, orderBy);
        }
    }
}