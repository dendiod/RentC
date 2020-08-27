using RentC.DataAccess.Contracts;
using RentC.DataAccess.Models.QueryModels;
using RentC.DataAccess.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebSockets;

namespace RentC.WebUI.Controllers
{
    public class CarController : Controller
    {
        public ActionResult AvailableCars(CarsViewModel<localhost.QueryCar> viewModel, string orderBy = "Id")
        {
            if (!ModelState.IsValid)
            {
                return View(new CarsViewModel<localhost.QueryCar>() { Cars = new List<localhost.QueryCar>() });
            }
            localhost.QueryCar car = viewModel.SearchCar ?? new localhost.QueryCar();
            localhost.AvailableCarsService webService = new localhost.AvailableCarsService();
            localhost.QueryCar[] cars = webService.GetAvailableCars(orderBy, car);

            viewModel.Cars = cars;

            return View(viewModel);
        }
    }
}