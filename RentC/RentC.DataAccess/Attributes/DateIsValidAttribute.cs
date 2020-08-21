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

            DateTime birthDate = (DateTime)value;  
            if(birthDate == DateTime.MinValue)
            {
                this.ErrorMessage = "Invalid date. Valid format is dd-MM-yyyy";
                return false;
            }

            // Save today's date.
            var today = DateTime.Today;

            // Calculate the age.
            var age = today.Year - birthDate.Year;

            const int minAge = 18, maxAge = 125;

            // Go back to the year the person was born in case of a leap year
            if (birthDate.Date > today.AddYears(-age))
            {
                --age;
            }
            if(age < minAge || age > maxAge)
            {
                this.ErrorMessage = String.Format("Client's age should be between {0} and {1}", minAge, maxAge);
                return false;
            }

            return true;
        }
    }
}