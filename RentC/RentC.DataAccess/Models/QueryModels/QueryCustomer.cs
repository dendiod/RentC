using RentC.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RentC.DataAccess.Models.QueryModels
{
    public class QueryCustomer : BaseEntity
    {
        [Required]
        [CustomerExists(ErrorMessage ="Customer with this Id already exists")]
        public override int Id { get; set; }
        [Required]
        [StringLength(50, ErrorMessage ="Name is too long")]
        public string Name  { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        public string Location { get; set; }
    }
}
