using RentC.Core;
using RentC.DataAccess.Attributes;
using RentC.DataAccess.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RentC.DataAccess.Models.QueryModels
{
    public class QueryCustomer : BaseEntity, IValidatableObject
    {  
        [IdIsValid]
        public int CustomId { get; set; }
        [Required]
        [StringLength(50)]
        public string Name  { get; set; }
        [Required]
        [DateIsValid(ErrorMessage = "Invalid Birth Date. Valid format is dd-MM-yyyy")]
        [Age]
        public DateTime BirthDate { get; set; }
        public string Location { get; set; }

        public bool IsCreating { get; set; }
        public Customer Customer { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();

            IRepo<Customer> repo = new SQLRepo<Customer>(new ModelContext());

            var customer = repo.FirstOrDefault(x => x.CustomId == CustomId);

            if (customer != null)
            {
                if (IsCreating)
                {
                    errors.Add(new ValidationResult("Customer with this Id already exists"));
                    return errors;
                }
                Customer = customer;
            }
            else if (!IsCreating)
            {
                errors.Add(new ValidationResult("There is no customer with this id"));
                return errors;
            }

            return errors;
        }
    }
}
