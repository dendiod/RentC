using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RentC.DataAccess.Attributes.Search
{
    public class SearchDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            DateTime date = (DateTime)value;
            if (date == DateTime.MinValue)
            {
                return false;
            }

            return true;
        }
    }
}