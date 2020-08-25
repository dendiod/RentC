﻿using RentC.DataAccess;
using RentC.DataAccess.Contracts;
using RentC.DataAccess.Models;
using RentC.DataAccess.Models.QueryModels;
using RentC.WebUI.CustomLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RentC.WebUI.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ContextManager contextManager;
        private readonly IRepo<Customer> repo;

        public CustomerController(IRepo<Customer> repo)
        {
            this.repo = repo;
            contextManager = new ContextManager(repo.GetContext());
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(QueryCustomer customer)
        {
            if (!ModelState.IsValid)
            {
                OverrideError();
                return View(customer);
            }

            contextManager.ManageCustomers(true, customer);

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Update()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Update(QueryCustomer customer)
        {
            if (!ModelState.IsValid)
            {
                OverrideError();
                return View(customer);
            }

            contextManager.ManageCustomers(false, customer);

            return RedirectToAction("Index", "Home");
        }

        public ActionResult CustomersList(int? customId, string name, DateTime? birthDate,
            string location, string orderBy = "CustomId")
        {
            QueryManager queryManager = new QueryManager(repo.GetContext());
            var customers = queryManager.GetCustomers(orderBy, customId, name, birthDate, location);

            return View(customers);
        }

        private void OverrideError()
        {
            Overrider overrider = new Overrider();
            overrider.OverrideError(this, "BirthDate", "Date should be in format dd-MM-yyyy");
        }
    }
}