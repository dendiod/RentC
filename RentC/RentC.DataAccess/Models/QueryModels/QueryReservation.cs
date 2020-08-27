using RentC.Core;
using RentC.DataAccess.Attributes;
using RentC.DataAccess.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace RentC.DataAccess.Models.QueryModels
{
    [StartEndDates]
    [ReservationId]
    [CustomerExisting]
    [ReservationInsert]
    public class QueryReservation : IValidatableObject
    {        
        [StringLength(10)]
        public string Plate { get; set;}        
        public int? CustomerId { get; set; }
        [Required]
        [DateIsValid(ErrorMessage = "Invalid Start Date. Valid format is dd-MM-yyyy")]
        [Today(ErrorMessage = "Error. Start Date is earlier than today")]
        public DateTime? StartDate { get; set; }
        [Required]
        [DateIsValid(ErrorMessage = "Invalid End Date. Valid format is dd-MM-yyyy")]
        [Today(ErrorMessage = "Error. End Date is earlier than today")]
        public DateTime? EndDate { get; set; }
        public string Location { get; set; }        
        public int Id { get; set; }

        public bool IsCreating { get; set; }
        public int CarId { get; set; }
        public int LocationId { get; set; }
        public Reservation Reservation { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();

            ModelContext context = new ModelContext();

            IRepo<Reservation> reservationRepo = new SQLRepo<Reservation>(context);
            Reservation reservation = null;

            if ((bool)!IsCreating)
            {
                reservation = reservationRepo.Find(this.Id);
                if(reservation == null)
                {
                    errors.Add(new ValidationResult("There is no reservation with this id"));
                    return errors;
                }
            }

            IRepo<Car> carRepo = new SQLRepo<Car>(context);
            Func<Car, bool> predicate;
            if ((bool)IsCreating)
            {
                predicate = x => x.Plate == this.Plate;
            }
            else
            {
                predicate = x => x.Id == reservation.CarId;
            }
            var car = carRepo.FirstOrDefault(predicate);


            if ((bool)IsCreating)
            {
                if (car == null)
                {
                    errors.Add(new ValidationResult("There is no car with this plate"));
                    return errors;
                }

                IRepo<Location> locationRepo = new SQLRepo<Location>(context);
                var location = locationRepo.Find(car.LocationId);
                if (location.Name != Location)
                {
                    errors.Add(new ValidationResult("This car is not available in this city"));
                    return errors;
                }
            }            

            var r = reservationRepo.FirstOrDefault(x => x.CarId == car.Id && (
            x.StartDate >= StartDate && x.StartDate <= EndDate ||
            x.EndDate >= StartDate && x.EndDate <= EndDate));

            if (r != null && (bool)IsCreating || ((bool)!IsCreating && r!= null && r.Id != reservation.Id))
            {
                errors.Add(new ValidationResult(String.Format("Error. Car was rented from {0} to {1}",
                    r.StartDate.ToString("dd-MM-yyyy"), r.EndDate.ToString("dd-MM-yyyy"))));
                return errors;
            }

            CarId = car.Id;
            LocationId = car.LocationId;
            Reservation = reservation;

            return errors;
        }
    }
}
