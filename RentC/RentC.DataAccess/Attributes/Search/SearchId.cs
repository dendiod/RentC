using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RentC.DataAccess.Attributes.Search
{
    public class SearchId : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            int id = (int)value;
            if (id < 1 || id > int.MaxValue)
            {
                this.ErrorMessage = String.Format("Id should be between 1 and {0}", int.MaxValue);
                return false;
            }

            return true;
        }
    }
}