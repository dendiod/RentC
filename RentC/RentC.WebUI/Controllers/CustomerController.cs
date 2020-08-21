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
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(QueryCustomer customer)
        {
            if (!ModelState.IsValid)
            {
                Overrider overrider = new Overrider();
                overrider.OverrideError(this, "BirthDate", "Date should be in format dd-MM-yyyy");
                return View(customer);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}