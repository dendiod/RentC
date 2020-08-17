using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentC.Core.Models
{
    public class Car : BaseEntity
    {
        public string Plate { get; set; }
        public int ManufacturerId { get; set; }
        public int ModelId { get; set; }
        public int LocationId { get; set; }
        public decimal PricePerDay { get; set; }
    }
}
