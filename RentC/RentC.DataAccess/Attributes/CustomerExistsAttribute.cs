using RentC.DataAccess;
using RentC.DataAccess.Contracts;
using RentC.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentC.Core
{
    class CustomerExistsAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return false;
            }

            IRepo<Customer> repo = new SQLRepo<Customer>(new ModelContext());
            int id;
            bool idIsInt = int.TryParse(value.ToString(), out id);

            if (!idIsInt || id < 1 || id > int.MaxValue)
            {
                this.ErrorMessage = String.Format("Id should be between 1 and {0}", int.MaxValue);
                return false;
            }

            if (repo.FirstOrDefault(x => x.CustomId == id) != null)
            {
                this.ErrorMessage = "Customer with this Id already exists";
                return false;
            }

            return true;
        }
    }
}
