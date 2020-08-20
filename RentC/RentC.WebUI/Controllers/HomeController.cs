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
            OverrideError("Id", "Id should be between 1 and 2147483647");
            OverrideError("BirthDate", "Date should be in format dd-MM-yyyy");
            if (!ModelState.IsValid)
            {
                return View(customer);
            }
            return RedirectToAction("Index");
        }

        private void OverrideError(string key, string message)
        {
            var modelState = ModelState[key];
            for (int i = 0; i < modelState.Errors.Count; ++i)
            {
                if (modelState.Errors[i].ErrorMessage.Contains("is not valid for"))
                {
                    modelState.Errors.RemoveAt(i);
                    modelState.Errors.Add(message);
                    break;
                }
            }
        }
    }
}