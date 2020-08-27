using RentC.DataAccess.Contracts;
using RentC.DataAccess.Models;
using RentC.DataAccess.Models.QueryModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RentC.DataAccess.Attributes
{
    public class CustomerExistingAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var reservation = value as QueryReservation; 
            if(value == null)
            {
                return false;
            }

            IRepo<Customer> customerRepo = new SQLRepo<Customer>(new ModelContext());
            var customer = customerRepo.FirstOrDefault(x => x.CustomId == (int)reservation.CustomerId);

            if (customer == null)
            {
                this.ErrorMessage = "There is no customer with this id";
                return false;
            }

            return true;
        }
    }
}