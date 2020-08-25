using RentC.DataAccess;
using RentC.DataAccess.Contracts;
using RentC.DataAccess.Models;
using RentC.DataAccess.Models.QueryModels;
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
        private readonly IRepo<Reservation> repo;

        public ReservationController(IRepo<Reservation> repo)
        {
            this.repo = repo;
            contextManager = new ContextManager(repo.GetContext());
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
                return View(reservation);
            }

            contextManager.ManageReservations(false, reservation);

            return RedirectToAction("Index", "Home");
        }

        public ActionResult ReservationList(string plate, int? customerId, DateTime? startDate,
            DateTime? endDate, string location, string orderBy = "Id")
        {
            QueryManager queryManager = new QueryManager(repo.GetContext());
            var reservations = queryManager.GetReservations(orderBy, plate, customerId, startDate, endDate, location);

            return View(reservations);
        }
    }
}