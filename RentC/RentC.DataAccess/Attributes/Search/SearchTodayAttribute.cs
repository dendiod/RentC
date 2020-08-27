using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RentC.DataAccess.Attributes.Search
{
    public class SearchTodayAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if(value == null)
            {
                return true;
            }
            if((DateTime)value < DateTime.Today)
            {
                return false;
            }

            return true;
        }
    }
}