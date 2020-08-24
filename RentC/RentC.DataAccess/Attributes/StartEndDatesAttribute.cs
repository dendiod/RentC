using RentC.DataAccess.Models.QueryModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RentC.DataAccess.Attributes
{
    public class StartEndDatesAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            QueryReservation r = value as QueryReservation;

            if(r.EndDate < r.StartDate)
            {
                this.ErrorMessage = "End Date should be equal or bigger than Start Date";
                return false;
            }

            return true;
        }
    }
}