using RentC.DataAccess.Contracts;
using RentC.DataAccess.Models;
using RentC.DataAccess.Models.QueryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentC.DataAccess
{
    public class ContextManager
    {
        private ModelContext modelContext;

        public ContextManager(ModelContext modelContext)
        {
            this.modelContext = modelContext;
        }
        public void ManageCustomers(bool creating, QueryCustomer queryCustomer)
        {
            IRepo<Location> locationRepo = new SQLRepo<Location>(modelContext);

            int id = queryCustomer.CustomId;
            string location = queryCustomer.Location;

            Customer c = creating ? new Customer() : queryCustomer.Customer;
            c.CustomId = id;
            c.Name = queryCustomer.Name;
            c.BirthDate = queryCustomer.BirthDate;
            c.LocationId = null;

            if (!string.IsNullOrWhiteSpace(location))
            {
                Location loc = locationRepo.FirstOrDefault(x => x.Name == location);
                if (loc != null)
                {
                    c.LocationId = loc.Id;
                }
                else
                {
                    int locatId = locationRepo.Collection().Count() + 1;
                    locationRepo.Insert(new Location { Id = locatId, Name = location });
                    locationRepo.Commit();
                    c.LocationId = locatId;
                }
            }

            InsertUpdateCommit(creating, c);
        }

        public void ManageReservations(bool creating, QueryReservation queryReservation)
        {
            Reservation r = creating ? new Reservation() : queryReservation.Reservation;
            if ((bool)queryReservation.IsCreating)
            {
                r.CarId = queryReservation.CarId;
                r.CustomerId = (int)queryReservation.CustomerId;
                r.LocationId = queryReservation.LocationId;
            }            
            r.StartDate = (DateTime)queryReservation.StartDate;
            r.EndDate = (DateTime)queryReservation.EndDate;
            

            InsertUpdateCommit(creating, r);
        }

        private void InsertUpdateCommit<T>(bool creating, T item) where T : BaseEntity
        {
            IRepo<T> repo = new SQLRepo<T>(modelContext);
            if (creating)
            {
                repo.Insert(item);
            }
            else
            {                
                repo.Update(item);
            }
            repo.Commit();
        }
    }
}