using RentC.DataAccess.Models.QueryModels;
using RentC.DataAccess.Models.Search;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RentC.DataAccess.Models.ViewModels
{
    public class ReservationViewModel
    {
        public SearchReservation SearchReservation { get; set; }
        public IEnumerable<QueryReservation> Reservations { get; set; }
    }
}