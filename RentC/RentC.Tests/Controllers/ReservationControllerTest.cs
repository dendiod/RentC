using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RentC.DataAccess.Contracts;
using RentC.DataAccess.Models;
using RentC.DataAccess.Models.QueryModels;
using RentC.WebUI.Controllers;

namespace RentC.Tests.Controllers
{
    [TestClass]
    public class ReservationControllerTest
    {
        [TestMethod]
        public void CanAddReservation()
        {
            // arrange
            IRepo<Customer> customerRepo = new MockContext<Customer>();
            IRepo<Car> carRepo = new MockContext<Car>();
            IRepo<Reservation> reservationRepo = new MockContext<Reservation>();
            IRepo<Location> locationRepo = new MockContext<Location>();
            IRepo<Model> modelRepo = new MockContext<Model>();
            IRepo<Manufacturer> manufacturerRepo = new MockContext<Manufacturer>();

            var controller = new ReservationController(carRepo, customerRepo, reservationRepo,
                locationRepo, modelRepo, manufacturerRepo);

            var reservation = new QueryReservation
            {
                CarId = 1,
                CustomerId = 1,
                StartDate = new DateTime(2020, 9, 1),
                EndDate = new DateTime(2020, 9, 2),
                Location = "Brasov"
            };

            // act
            controller.Create(reservation);
            Reservation r = reservationRepo.Collection().FirstOrDefault();

            // assert
            Assert.IsNotNull(r);
            Assert.AreEqual(1, reservationRepo.Collection().Count());
            Assert.AreEqual(new DateTime(2020, 9, 1), r.StartDate);
        }
    }
}
