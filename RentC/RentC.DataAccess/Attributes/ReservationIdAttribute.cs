using RentC.DataAccess.Models.QueryModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RentC.DataAccess.Attributes
{
    public class ReservationIdAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            QueryReservation r = value as QueryReservation;
            int id = r.Id;

            if (!r.IsCreating && (id < 1 || id > int.MaxValue))
            {
                this.ErrorMessage = String.Format("Id should be between 1 and {0}", int.MaxValue);
                return false;
            }

            return true;
        }
    }  
}