using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure.Design;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RentC.DataAccess;
using RentC.DataAccess.Models.QueryModels;
using RentC.DataAccess.Contracts;

namespace RentC.WebUI.Controllers
{
    public class HomeController : Controller
    {
        IRepo<QueryCustomer> repo;
        public HomeController(IRepo<QueryCustomer> repo)
        {
            this.repo = repo;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Customers()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Customers( QueryCustomer customer)
        {
            if (!ModelState.IsValid)
            {
                return View(customer);
            }
            return RedirectToAction("Index");
        }
    }
}