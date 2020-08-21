using RentC.DataAccess.Models.QueryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
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
            var cars = from car in context.Cars
                       join model in context.Models on car.ModelId equals model.Id
                       join manufacturer in context.Manufacturers on car.ManufacturerId equals manufacturer.Id
                       join location in context.Locations on car.LocationId equals location.Id
                       join reservation in context.Reservations on car.Id equals reservation.CarId into leftJoin

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

            return OrderItems(asc, cars, orderBy);
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
                            join location in context.Locations on customer.LocationId equals location.Id

                            select new QueryCustomer
                            {
                                Id = customer.Id,
                                CustomId = customer.CustomId,
                                Name = customer.Name,
                                BirthDate = customer.BirthDate,
                                Location = location.Name
                            };

            return OrderItems(asc, customers, orderBy);
        }
    }
}