using RentC.DataAccess.Attributes.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentC.DataAccess.Models.Search
{
    public class SearchCustomer
    {
        [SearchId]
        public int? CustomId { get; set; }
        public string Name { get; set; }
        [SearchAge]
        [SearchDate(ErrorMessage = "Invalid Birth Date. Valid format is dd-MM-yyyy")]
        public DateTime? BirthDate { get; set; }
        public string Location { get; set; }
    }
}