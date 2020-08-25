﻿using RentC.DataAccess;
using RentC.DataAccess.Contracts;
using RentC.DataAccess.Models;
using RentC.DataAccess.Models.QueryModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RentC.Core
{
    public class IdIsValidAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return false;
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
