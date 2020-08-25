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
        public ActionResult AvailableCars(string plate, string manufacturer, string modelName, 
            DateTime? startDate, DateTime? endDate, string location, string orderBy = "Id")
        {
            localhost.AvailableCarsService webService = new localhost.AvailableCarsService();
            localhost.QueryCar[] cars = webService.GetAvailableCars(orderBy, plate, manufacturer, modelName,
                startDate, endDate, location);

            return View(cars);
        }
    }
}