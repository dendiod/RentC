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

            // Save today's date.
            var today = DateTime.Today;

            // Calculate the age.
            var age = today.Year - birthDate.Year;

            const int minAge = 18, maxAge = 125;
            if (age < minAge || age > maxAge)
            {
                this.ErrorMessage = String.Format("Year should be between {0} and {1}", today.Year - maxAge, today.Year - minAge);
                return false;
            }

            // Go back to the year the person was born in case of a leap year
            if (birthDate.Date > today.AddYears(-age))
            {
                --age;
            }
            if(age < minAge || age > maxAge)
            {
                this.ErrorMessage = "Client's age should be between 18 and 125";
                return false;
            }

            return true;
        }
    }
}