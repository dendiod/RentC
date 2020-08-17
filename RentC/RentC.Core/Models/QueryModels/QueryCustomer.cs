using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentC.Core.Models.QueryModels
{
    public class QueryCustomer : BaseEntity
    {
        public string Name  { get; set; }
        public DateTime BirthDate { get; set; }
        public string Location { get; set; }
    }
}
