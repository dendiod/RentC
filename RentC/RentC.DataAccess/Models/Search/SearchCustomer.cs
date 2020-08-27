using RentC.DataAccess.Attributes.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentC.DataAccess.Models.Search
{
    public class SearchCustomer
    {
        public int? CustomId { get; set; }
        public string Name { get; set; }
        [SearchAge]
        public DateTime? BirthDate { get; set; }
        public string Location { get; set; }
    }
}