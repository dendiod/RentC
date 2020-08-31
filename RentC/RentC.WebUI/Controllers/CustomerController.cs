using RentC.DataAccess;
using RentC.DataAccess.Contracts;
using RentC.DataAccess.Models;
using RentC.DataAccess.Models.QueryModels;
using RentC.DataAccess.Models.Search;
using RentC.WebUI.Models;
using RentC.WebUI.CustomLogic;
using System.Collections.Generic;
using System.Web.Mvc;
using System;

namespace RentC.WebUI.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ContextManager contextManager;

        private IRepo<Car> carRepo;
        private IRepo<Customer> customerRepo;
        private IRepo<Reservation> reservationRepo;
        private IRepo<Location> locationRepo;
        private IRepo<Model> modelRepo;
        private IRepo<Manufacturer> manufacurerRepo;

        public CustomerController(IRepo<Car> carRepo, IRepo<Customer> customerRepo,
            IRepo<Reservation> reservationRepo, IRepo<Location> locationRepo,
            IRepo<Model> modelRepo, IRepo<Manufacturer> manufacurerRepo)
        {
            this.carRepo = carRepo;
            this.customerRepo = customerRepo;
            this.reservationRepo = reservationRepo;
            this.locationRepo = locationRepo;
            this.modelRepo = modelRepo;
            this.manufacurerRepo = manufacurerRepo;

            contextManager = new ContextManager(customerRepo, reservationRepo, locationRepo);
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
                OverrideErrorMessage();
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
                OverrideErrorMessage();
                return View(customer);
            }

            contextManager.ManageCustomers(false, customer);

            return RedirectToAction("Index", "Home");
        }

        public ActionResult CustomersList(CustomerViewModel viewModel, string orderBy = "CustomId")
        {
            if (!ModelState.IsValid)
            {
                return View(new CustomerViewModel() { Customers = new List<QueryCustomer>() });
            }
            SearchCustomer c = viewModel.SearchCustomer ?? new SearchCustomer();
            QueryManager queryManager = new QueryManager(carRepo, customerRepo, reservationRepo, locationRepo, 
                modelRepo, manufacurerRepo);
            QueryCustomer[] customers = queryManager.GetCustomers(orderBy, c);

            viewModel.Customers = customers;
            viewModel.SearchCustomer = new SearchCustomer();

            return View(viewModel);
        }

        public ActionResult VipCustomersList(CustomerViewModel viewModel, string orderBy = "CustomId")
        {
            if (!ModelState.IsValid)
            {
                return View(new CustomerViewModel() { Customers = new List<QueryCustomer>() });
            }
            SearchCustomer c = viewModel.SearchCustomer ?? new SearchCustomer();
            QueryManager queryManager = new QueryManager(carRepo, customerRepo, reservationRepo, locationRepo,
                modelRepo, manufacurerRepo);
            QueryCustomer[] customers = queryManager.GetVipCustomers(orderBy, c);

            viewModel.Customers = customers;
            viewModel.SearchCustomer = new SearchCustomer();

            return View(viewModel);
        }

        private void OverrideErrorMessage()
        {
            Overrider overrider = new Overrider();
            overrider.OverrideError(this, "BirthDate", "Date should be in format dd-MM-yyyy");
        }
    }
}