using RentC.DataAccess.Models.Search;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RentC.DataAccess.Attributes.Search
{
    public class SearchDatesAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var reservation = value as SearchReservation;
            if(reservation == null)
            {
                return true;
            }

            if(reservation.EndDate < reservation.StartDate)
            {
                ErrorMessage = "End Date can't be earlier than today";
                return false;
            }

            return true;
        }
    }
}