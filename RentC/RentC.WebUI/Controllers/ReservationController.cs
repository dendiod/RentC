using RentC.DataAccess;
using RentC.DataAccess.Contracts;
using RentC.DataAccess.Models;
using RentC.DataAccess.Models.QueryModels;
using RentC.DataAccess.Models.Search;
using RentC.DataAccess.Models.ViewModels;
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

        public ActionResult ReservationList(ReservationViewModel viewModel, string orderBy = "Id")
        {
            if (!ModelState.IsValid)
            {
                return View(new ReservationViewModel() { Reservations = new List<QueryReservation>() });
            }
            SearchReservation r = viewModel.SearchReservation ?? new SearchReservation();
            QueryManager queryManager = new QueryManager(repo.GetContext());
            QueryReservation[] reservations = queryManager.GetReservations(orderBy, r);

            viewModel.Reservations = reservations;
            viewModel.SearchReservation = new SearchReservation();

            return View(viewModel);
        }
    }
}