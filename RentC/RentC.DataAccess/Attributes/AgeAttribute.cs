using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RentC.DataAccess.Attributes
{
    public class AgeAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return false;
            }

            const int minAge = 18, maxAge = 125;
            var today = DateTime.Today;
            var birthDate = (DateTime)value;

            // Calculate the age.
            var age = today.Year - birthDate.Year;            

            // Go back to the year the person was born in case of a leap year
            if (birthDate.Date > today.AddYears(-age))
            {
                --age;
            }
            if (age < minAge || age > maxAge)
            {
                this.ErrorMessage = String.Format("Client's age should be between {0} and {1}", minAge, maxAge);
                return false;
            }

            return true;
        }
    }
}