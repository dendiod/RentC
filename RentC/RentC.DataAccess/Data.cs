using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using RentC.Core.Models.QueryModels;
using RentC.Core.Models;
using System.Linq.Expressions;
using System.Reflection;
using System.Linq.Dynamic.Core;

namespace RentC.DataAccess
{
    public class Data<T>
    {
        public static List<QueryCar> GetAvailableCars(bool asc, string orderBy)
        {
            using (ModelContext db = new ModelContext())
            {
                DateTime today = DateTime.Today;
                var cars = from car in db.Cars
                           join model in db.Models on car.ModelId equals model.Id
                           join manufacturer in db.Manufacturers on car.ManufacturerId equals manufacturer.Id
                           join location in db.Locations on car.LocationId equals location.Id
                           join reservation in db.Reservations on car.Id equals reservation.CarId into leftJoin

                           from reservation in leftJoin.DefaultIfEmpty()
                           let earlierThanStart = reservation.StartDate > today
                           let laterThanEnd = reservation.EndDate < today
                           where reservation == null || earlierThanStart || laterThanEnd

                           select new QueryCar
                           {
                               Id = car.Id,
                               Plate = car.Plate,
                               Model = model.Name,
                               Manufacturer = manufacturer.Name,
                               StartDate = laterThanEnd ? reservation.EndDate : today,
                               EndDate = earlierThanStart ? reservation.StartDate : new DateTime(2100, 1, 1),
                               Location = location.Name
                           };

                if (!asc)
                {
                    orderBy += " desc";
                }
                cars = cars.OrderBy(orderBy);

                return cars.ToList();
            }
        }        

        public static IQueryable<QueryReservation> GetReservations(bool asc, Func<QueryReservation, T>func)
        {
            using (ModelContext db = new ModelContext())
            {
                return from reservation in db.Reservations
                       join location in db.Locations on reservation.LocationId equals location.Id
                       join car in db.Cars on reservation.CarId equals car.Id
                       orderby asc ? func : func descending

                       select new QueryReservation
                       {
                           Id = reservation.Id,
                           Plate = car.Plate,
                           CustomerId = reservation.CustomerId,
                           StartDate = reservation.StartDate,
                           EndDate = reservation.EndDate,
                           Location = location.Name
                       };
            }
        }

        public static IQueryable<QueryCustomer> GetCustomers(bool asc, Func<QueryCustomer, T> orderByFunc)
        {
            using (ModelContext db = new ModelContext())
            {
                return from customer in db.Customers
                       join location in db.Locations on customer.LocationId equals location.Id
                       orderby asc ? orderByFunc : orderByFunc descending

                       select new QueryCustomer
                       {
                           Id = customer.Id,
                           Name = customer.Name,
                           BirthDate = customer.BirthDate,
                           Location = location.Name
                       };
            }
        }

        public static IQueryable<Car> IsCarExists(string plate)
        {
            using (ModelContext db = new ModelContext())
            {
                return db.Cars.Where(c => c.Plate == plate).Take(1);
            }
        }

        public static Customer IsCustomerExists(int id)
        {
            using (ModelContext db = new ModelContext())
            {
                return db.Customers.Find(id);
            }
        }
    }
}