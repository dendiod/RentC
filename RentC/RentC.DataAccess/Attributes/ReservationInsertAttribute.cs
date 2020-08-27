using RentC.DataAccess.Models.QueryModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RentC.DataAccess.Attributes
{
    public class ReservationInsertAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var reservation = value as QueryReservation;
            if ((bool)!reservation.IsCreating)
            {
                return true;
            }
            if (string.IsNullOrEmpty(reservation.Plate))
            {
                this.ErrorMessage = "The Plate field is required";
                return false;
            }

            return true;
        }
    }
}