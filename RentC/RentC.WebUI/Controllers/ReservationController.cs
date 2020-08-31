using RentC.DataAccess;
using RentC.DataAccess.Contracts;
using RentC.DataAccess.Models;
using RentC.DataAccess.Models.QueryModels;
using RentC.DataAccess.Models.Search;
using RentC.WebUI.Models;
using RentC.WebUI.CustomLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RentC.WebUI.Controllers
{
    public class ReservationController : Controller
    {
        private readonly ContextManager contextManager;

        private IRepo<Car> carRepo;
        private IRepo<Customer> customerRepo;
        private IRepo<Reservation> reservationRepo;
        private IRepo<Location> locationRepo;
        private IRepo<Model> modelRepo;
        private IRepo<Manufacturer> manufacturerRepo;

        public ReservationController(IRepo<Car> carRepo, IRepo<Customer> customerRepo,
            IRepo<Reservation> reservationRepo, IRepo<Location> locationRepo,
            IRepo<Model> modelRepo, IRepo<Manufacturer> manufacturerRepo)
        {
            this.carRepo = carRepo;
            this.customerRepo = customerRepo;
            this.reservationRepo = reservationRepo;
            this.locationRepo = locationRepo;
            this.modelRepo = modelRepo;
            this.manufacturerRepo = manufacturerRepo;

            contextManager = new ContextManager(customerRepo, reservationRepo, locationRepo);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(QueryReservation reservation)
        {
            if (!ModelState.IsValid)
            {
                OverrideErrorMessage("Date should be in format dd-MM-yyyy");
                return View(reservation);
            }

            contextManager.ManageReservations(true, reservation);

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Update()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Update(QueryReservation reservation)
        {
            if (!ModelState.IsValid)
            {
                OverrideErrorMessage("Date should be in format dd-MM-yyyy");
                return View(reservation);
            }

            contextManager.ManageReservations(false, reservation);

            return RedirectToAction("Index", "Home");
        }

        public ActionResult ReservationList(ReservationViewModel viewModel, string orderBy = "Id")
        {
            if (!ModelState.IsValid)
            {
                return View(new ReservationViewModel() { Reservations = new List<QueryReservation>() });
            }
            SearchReservation r = viewModel.SearchReservation ?? new SearchReservation();
            QueryManager queryManager = new QueryManager(carRepo, customerRepo, reservationRepo, locationRepo,
                modelRepo, manufacturerRepo);
            QueryReservation[] reservations = queryManager.GetReservations(orderBy, r);

            viewModel.Reservations = reservations;
            viewModel.SearchReservation = new SearchReservation();

            return View(viewModel);
        }

        private void OverrideErrorMessage(string message)
        {
            Overrider overrider = new Overrider();
            overrider.OverrideError(this, "StartDate", message);
            overrider.OverrideError(this, "EndDate", message);
        }
    }
}