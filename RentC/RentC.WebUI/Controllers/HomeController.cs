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
        public ActionResult Index()
        {
            return View();
        }
    }
}