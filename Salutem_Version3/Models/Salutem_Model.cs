namespace Salutem_Version3.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Salutem_Model : DbContext
    {
        public Salutem_Model()
            : base("name=Salutem_Model")
        {
        }

        public virtual DbSet<Applicant> Applicants { get; set; }
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<EventBooking> EventBookings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Applicant>()
                .HasMany(e => e.EventBookings)
                .WithRequired(e => e.Applicant)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Event>()
                .HasMany(e => e.EventBookings)
                .WithRequired(e => e.Event)
                .WillCascadeOnDelete(false);
        }
    }
}
