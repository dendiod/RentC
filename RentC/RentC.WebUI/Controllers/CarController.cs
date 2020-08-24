using RentC.DataAccess.Contracts;
using RentC.DataAccess.Models.QueryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RentC.WebUI.Controllers
{
    public class CarController : Controller
    {
        public ActionResult AvailableCars()
        {
            localhost.AvailableCarsService webService = new localhost.AvailableCarsService();
            localhost.QueryCar[] cars = webService.GetAvailableCars(true, "Id");

            return View(cars);
        }
    }
}