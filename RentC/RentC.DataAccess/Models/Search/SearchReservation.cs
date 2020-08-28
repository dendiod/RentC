using RentC.DataAccess.Attributes.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentC.DataAccess.Models.Search
{
    [SearchDates]
    public class SearchReservation
    {
        public string Plate { get; set; }
        [SearchId]
        public int? CustomerId { get; set; }
        [SearchToday(ErrorMessage = "Start Date can't be earlier than today")]
        [SearchDate(ErrorMessage = "Invalid Start Date. Valid format is dd-MM-yyyy")]
        public DateTime? StartDate { get; set; }
        [SearchToday(ErrorMessage = "End Date can't be earlier than today")]
        [SearchDate(ErrorMessage = "Invalid End Date. Valid format is dd-MM-yyyy")]
        public DateTime? EndDate { get; set; }
        public string Location { get; set; }
    }
}