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
        private ModelContext modelContext;
        public CustomerExistsAttribute(ModelContext modelContext)
        {
            this.modelContext = modelContext;
        }
        public override bool IsValid(object value)
        {            
            if (value == null)
            {
                return false;
            }

            IRepo<Customer> repo = new SQLRepo<Customer>(modelContext);
            int id;
            bool idIsInt = int.TryParse(value.ToString(), out id);

            if (!idIsInt)
            {
                return false;
            }

            if (repo.Find(id) != null)
            {
                this.ErrorMessage = "Customer with this Id already exists";
                return false;
            }            

            return true;
        }
    }
}
