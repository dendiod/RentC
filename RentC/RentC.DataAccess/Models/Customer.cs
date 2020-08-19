using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentC.DataAccess.Models
{
    public class Customer : BaseEntity
    {
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public int LocationId { get; set; }
    }
}
