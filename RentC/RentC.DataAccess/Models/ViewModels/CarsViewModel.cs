using RentC.DataAccess.Models.QueryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentC.DataAccess.Models.ViewModels
{
    public class CarsViewModel<T>
    {
        public IEnumerable<T> Cars { get; set; }
        public T SearchCar { get; set; }
    }
}