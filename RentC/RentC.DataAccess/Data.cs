using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RentC.Core.Models.QueryModels;
using RentC.Core.Models;
using System.Linq.Expressions;

namespace RentC.DataAccess
{
    public class Data
    {
        public static List<QueryCar> GetAvailableCars(bool asc, Expression<Func<QueryCar, int>> func)
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
                           orderby asc ? car.Id : car.Id descending

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
                return cars.OrderBy(func).ToList();
            }
        }        

        public static List<QueryReservation> GetReservations(bool asc, Func<QueryReservation, int>func)
        {
            using (ModelContext db = new ModelContext())
            {
                var reservations = from reservation in db.Reservations
                                   join location in db.Locations on reservation.LocationId equals location.Id
                                   join car in db.Cars on reservation.CarId equals car.Id
                                   orderby asc ? reservation.Id : reservation.Id descending

                                   select new QueryReservation
                                   {
                                       Id = reservation.Id,
                                       Plate = car.Plate,
                                       CustomerId = reservation.CustomerId,
                                       StartDate = reservation.StartDate,
                                       EndDate = reservation.EndDate,
                                       Location = location.Name
                                   };
                return reservations.OrderBy(func).ToList();
            }
        }

        public static IQueryable<QueryCustomer> GetCustomers(bool asc)
        {
            using (ModelContext db = new ModelContext())
            {
                return from customer in db.Customers
                       join location in db.Locations on customer.LocationId equals location.Id
                       orderby asc ? customer.Id : customer.Id descending

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