using RentC.DataAccess.Models;
using RentC.DataAccess.Models.QueryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentC.Tests.Factories
{
    internal class ReservationFactory
    {
        internal Reservation GetReservation(int id, int carId, int customerId, DateTime startDate, DateTime endDate)
        {
            return new Reservation
            {
                Id = id,
                CarId = carId,
                CustomerId = customerId,
                StartDate = startDate,
                EndDate = endDate,
                LocationId = 1
            };
        }

        internal QueryReservation GetQueryReservation(int id, int carId, int customerId, DateTime startDate, DateTime endDate)
        {
            return new QueryReservation
            {
                Id = id,
                Plate = "AAAAAA",
                CustomerId = customerId,
                StartDate = startDate,
                EndDate = endDate,
                LocationId = 1
            };
        }
    }
}
