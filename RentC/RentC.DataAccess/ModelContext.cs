namespace RentC.DataAccess
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using RentC.DataAccess.Models;

    public partial class ModelContext : DbContext
    {
        public ModelContext()
            : base("name=ModelContext")
        {
        }

        public virtual DbSet<Car> Cars { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Manufacturer> Manufacturers { get; set; }
        public virtual DbSet<Model> Models { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
    }
}
