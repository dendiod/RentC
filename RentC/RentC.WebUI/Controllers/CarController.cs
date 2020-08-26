using RentC.DataAccess.Contracts;
using RentC.DataAccess.Models.QueryModels;
using RentC.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RentC.WebUI.Controllers
{
    public class CarController : Controller
    {
        public ActionResult AvailableCars(CarsViewModel viewModel, string orderBy = "Id")
        {            
            localhost.QueryCar car = (localhost.QueryCar)viewModel.SearchCar ?? new localhost.QueryCar();
            localhost.AvailableCarsService webService = new localhost.AvailableCarsService();
            localhost.QueryCar[] cars = webService.GetAvailableCars(orderBy, car);

            viewModel.Cars = cars;

            return View(viewModel);
        }
    }
}