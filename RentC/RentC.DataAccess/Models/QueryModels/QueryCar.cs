using RentC.DataAccess.Attributes;
using RentC.DataAccess.Attributes.Search;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentC.DataAccess.Models.QueryModels
{    
    public class QueryCar : BaseEntity
    {
        public string Plate { get; set; }
        public string Manufacturer{ get; set; }
        public string Model { get; set; }
        [SearchToday(ErrorMessage = "Start Date can't be earlier than today")]
        [SearchDate(ErrorMessage = "Invalid Start Date. Valid format is dd-MM-yyyy")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? StartDate { get; set; }
        [SearchToday(ErrorMessage = "End Date can't be earlier than today")]
        [SearchDate(ErrorMessage = "Invalid End Date. Valid format is dd-MM-yyyy")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? EndDate { get; set; }
        public string Location { get; set; }

        [SearchDate(ErrorMessage = "Invalid End Date. Valid format is dd-MM-yyyy")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? MonthDate { get; set; }
        public int? ReservationsCount { get; set; }
    }
}
