using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;

namespace RentC.DataAccess.Attributes
{
    public class DateIsValidAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return false;
            }

            DateTime date = (DateTime)value;  
            if(date == DateTime.MinValue)
            {
                return false;
            }

            return true;
        }
    }
}