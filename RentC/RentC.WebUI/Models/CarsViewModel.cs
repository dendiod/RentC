using RentC.DataAccess.Models.QueryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentC.WebUI.Models
{
    public class CarsViewModel
    {
        public IEnumerable<localhost.QueryCar> Cars { get; set; }
        public localhost.QueryCar SearchCar { get; set; }
    }
}