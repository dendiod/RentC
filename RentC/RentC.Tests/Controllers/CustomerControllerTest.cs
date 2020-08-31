using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RentC.DataAccess.Contracts;
using RentC.DataAccess.Models;
using RentC.DataAccess.Models.QueryModels;
using RentC.Tests.Factories;
using RentC.WebUI.Controllers;
using RentC.WebUI.Models;

namespace RentC.Tests.Controllers
{
    [TestClass]
    public class CustomerControllerTest
    {
        [TestMethod]
        public void CanAddCustomer()
        {
            // arrange
            IRepo<Customer> customerRepo = new MockContext<Customer>();
            IRepo<Car> carRepo = new MockContext<Car>();
            IRepo<Reservation> reservationRepo = new MockContext<Reservation>();
            IRepo<Location> locationRepo = new MockContext<Location>();
            IRepo<Model> modelRepo = new MockContext<Model>();
            IRepo<Manufacturer> manufacturerRepo = new MockContext<Manufacturer>();

            var controller = new CustomerController(carRepo, customerRepo, reservationRepo,
                locationRepo, modelRepo, manufacturerRepo);

            var customerFactory = new CustomerFactory();
            var customer = customerFactory.GetQueryCustomer(1, 1, "c");

            // act
            controller.Create(customer);
            Customer c = customerRepo.Collection().FirstOrDefault();

            // assert
            Assert.IsNotNull(c);
            Assert.AreEqual(1, customerRepo.Collection().Count());
            Assert.AreEqual("c", c.Name);
        }

        [TestMethod]
        public void CanUpdateCustomer()
        {
            // arrange
            IRepo<Customer> customerRepo = new MockContext<Customer>();
            IRepo<Car> carRepo = new MockContext<Car>();
            IRepo<Reservation> reservationRepo = new MockContext<Reservation>();
            IRepo<Location> locationRepo = new MockContext<Location>();
            IRepo<Model> modelRepo = new MockContext<Model>();
            IRepo<Manufacturer> manufacturerRepo = new MockContext<Manufacturer>();

            var customerFactory = new CustomerFactory();
            customerRepo.Insert(customerFactory.GetCustomer(1, 1, "1"));

            var controller = new CustomerController(carRepo, customerRepo, reservationRepo,
                locationRepo, modelRepo, manufacturerRepo);

            
            var customer = customerFactory.GetQueryCustomer(1, 1, "c");
            customer.Customer = customerRepo.Collection().FirstOrDefault();

            // act
            controller.Update(customer);
            Customer c = customerRepo.Collection().FirstOrDefault();

            // assert
            Assert.IsNotNull(c);
            Assert.AreEqual(1, customerRepo.Collection().Count());
            Assert.AreEqual("c", c.Name);
        }

        [TestMethod]
        public void CanViewCustomers()
        {
            // arrange
            IRepo<Customer> customerRepo = new MockContext<Customer>();
            IRepo<Car> carRepo = new MockContext<Car>();
            IRepo<Reservation> reservationRepo = new MockContext<Reservation>();
            IRepo<Location> locationRepo = new MockContext<Location>();
            IRepo<Model> modelRepo = new MockContext<Model>();
            IRepo<Manufacturer> manufacturerRepo = new MockContext<Manufacturer>();

            var customerFactory = new CustomerFactory();

            customerRepo.Insert(customerFactory.GetCustomer(1, 1, "1"));
            customerRepo.Insert(customerFactory.GetCustomer(2, 2, "2"));

            var viewModel = new CustomerViewModel();

            var controller = new CustomerController(carRepo, customerRepo, reservationRepo,
                locationRepo, modelRepo, manufacturerRepo);

            // act
            controller.CustomersList(viewModel, "Name desc");
            IEnumerable<QueryCustomer> customers = viewModel.Customers;

            // assert
            Assert.IsNotNull(customers);
            Assert.AreEqual(2, customers.Count());
            Assert.AreEqual("2", customers.ElementAt(0).Name);
            Assert.AreEqual("1", customers.ElementAt(1).Name);
        }

        [TestMethod]
        public void CanViewVipCustomers()
        {
            // arrange
            IRepo<Customer> customerRepo = new MockContext<Customer>();
            IRepo<Car> carRepo = new MockContext<Car>();
            IRepo<Reservation> reservationRepo = new MockContext<Reservation>();
            IRepo<Location> locationRepo = new MockContext<Location>();
            IRepo<Model> modelRepo = new MockContext<Model>();
            IRepo<Manufacturer> manufacturerRepo = new MockContext<Manufacturer>();

            var customerFactory = new CustomerFactory();
            var reservationFactory = new ReservationFactory();

            customerRepo.Insert(customerFactory.GetCustomer(1, 1, "1"));
            customerRepo.Insert(customerFactory.GetCustomer(2, 2, "2"));
            customerRepo.Insert(customerFactory.GetCustomer(3, 3, "3"));

            reservationRepo.Insert(reservationFactory.GetReservation(1,1, 3, new DateTime(2019, 8, 29), new DateTime(2020, 8, 31)));
            reservationRepo.Insert(reservationFactory.GetReservation(2, 22, 3, new DateTime(2020, 8, 29), new DateTime(2020, 8, 31)));
            reservationRepo.Insert(reservationFactory.GetReservation(3, 2, 3, new DateTime(2020, 8, 29), new DateTime(2020, 8, 31)));
            reservationRepo.Insert(reservationFactory.GetReservation(4, 3, 3, new DateTime(2020, 8, 29), new DateTime(2020, 8, 31)));
            reservationRepo.Insert(reservationFactory.GetReservation(5, 4, 3, new DateTime(2020, 8, 29), new DateTime(2020, 8, 31)));
            reservationRepo.Insert(reservationFactory.GetReservation(6, 4, 2, new DateTime(2020, 8, 29), new DateTime(2020, 8, 31)));
            reservationRepo.Insert(reservationFactory.GetReservation(7, 5, 2, new DateTime(2020, 8, 29), new DateTime(2020, 8, 31)));
            reservationRepo.Insert(reservationFactory.GetReservation(8, 4, 1, new DateTime(2020, 8, 29), new DateTime(2020, 8, 31)));

            var viewModel = new CustomerViewModel();

            var controller = new CustomerController(carRepo, customerRepo, reservationRepo,
                locationRepo, modelRepo, manufacturerRepo);

            // act
            controller.VipCustomersList(viewModel, "Status desc");
            IEnumerable<QueryCustomer> customers = viewModel.Customers;

            // assert
            Assert.IsNotNull(customers);
            Assert.AreEqual(2, customers.Count());
            Assert.AreEqual("2", customers.ElementAt(0).Name);
            Assert.AreEqual("3", customers.ElementAt(1).Name);
            Assert.AreEqual(4, customers.ElementAt(1).ReservationsCount);
        }        
    }
}
