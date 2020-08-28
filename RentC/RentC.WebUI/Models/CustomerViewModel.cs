using RentC.DataAccess.Models.QueryModels;
using RentC.DataAccess.Models.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentC.WebUI.Models
{
    public class CustomerViewModel
    {
        public IEnumerable<QueryCustomer> Customers { get; set; }
        public SearchCustomer SearchCustomer { get; set; }
    }
}