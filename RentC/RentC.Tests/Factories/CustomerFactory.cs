using RentC.DataAccess.Models;
using RentC.DataAccess.Models.QueryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentC.Tests.Factories
{
    internal class CustomerFactory
    {
        internal QueryCustomer GetQueryCustomer(int id, int customId, string name)
        {
            return new QueryCustomer
            {
                Id = id,
                CustomId = customId,
                Name = name,
                BirthDate = new DateTime(2000, 2, 1),
                Location = null,
                ReservationsCount = 0,
                Status = ""
            };
        }

        internal Customer GetCustomer(int id, int customId, string name)
        {
            return new Customer
            {
                Id = id,
                CustomId = customId,
                Name = name,
                BirthDate = new DateTime(2000, 2, 1),
                LocationId = null
            };
        }
    }
}
