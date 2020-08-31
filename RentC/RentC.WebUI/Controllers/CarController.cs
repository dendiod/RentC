using RentC.DataAccess;
using RentC.DataAccess.Contracts;
using RentC.DataAccess.Models;
using RentC.DataAccess.Models.QueryModels;
using RentC.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebSockets;

namespace RentC.WebUI.Controllers
{
    public class CarController : Controller
    {
        private IRepo<Car> carRepo;
        private IRepo<Customer> customerRepo;
        private IRepo<Reservation> reservationRepo;
        private IRepo<Location> locationRepo;
        private IRepo<Model> modelRepo;
        private IRepo<Manufacturer> manufacturerRepo;

        public CarController(IRepo<Car> carRepo, IRepo<Customer> customerRepo,
            IRepo<Reservation> reservationRepo, IRepo<Location> locationRepo,
            IRepo<Model> modelRepo, IRepo<Manufacturer> manufacturerRepo)
        {
            this.carRepo = carRepo;
            this.customerRepo = customerRepo;
            this.reservationRepo = reservationRepo;
            this.locationRepo = locationRepo;
            this.modelRepo = modelRepo;
            this.manufacturerRepo = manufacturerRepo;
        }

        public ActionResult AvailableCars(CarsViewModel<localhost.QueryCar> viewModel, string orderBy = "Id")
        {
            if (!ModelState.IsValid)
            {
                return View(new CarsViewModel<localhost.QueryCar>() { Cars = new List<localhost.QueryCar>() });
            }
            localhost.QueryCar car = viewModel.SearchCar ?? new localhost.QueryCar();
            localhost.AvailableCarsService webService = new localhost.AvailableCarsService();
            localhost.QueryCar[] cars = webService.GetAvailableCars(orderBy, car);

            viewModel.Cars = cars;

            return View(viewModel);
        }

        public ActionResult RecentCars(CarsViewModel<QueryCar> viewModel, string orderBy = "Id")
        {
            if (!ModelState.IsValid)
            {
                return View(new CarsViewModel<QueryCar>() { Cars = new List<QueryCar>() });
            }
            QueryCar car = viewModel.SearchCar ?? new QueryCar();
            QueryManager queryManager = new QueryManager(carRepo, customerRepo, reservationRepo, locationRepo,
                modelRepo, manufacturerRepo);
            QueryCar[] cars = queryManager.GetRecentCars(orderBy, car);

            viewModel.Cars = cars;

            return View(viewModel);
        }

        public ActionResult RentedCarsInMonth(CarsViewModel<QueryCar> viewModel, string orderBy = "ReservationsCount")
        {
            if (!ModelState.IsValid)
            {
                return View(new CarsViewModel<QueryCar>() { Cars = new List<QueryCar>() });
            }
            QueryCar car = viewModel.SearchCar ?? new QueryCar();
            QueryManager queryManager = new QueryManager(carRepo, customerRepo, reservationRepo, locationRepo,
                modelRepo, manufacturerRepo);
            QueryCar[] cars = queryManager.GetCarsInMonth(orderBy, car);

            viewModel.Cars = cars;

            return View(viewModel);
        }
    }
}