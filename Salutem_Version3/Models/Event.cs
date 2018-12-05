namespace Salutem_Version3.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Event")]
    public partial class Event
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Event()
        {
            EventBookings = new HashSet<EventBooking>();
        }

        public int EventId { get; set; }

        [Required]
        public string EventName { get; set; }

        public string Description { get; set; }

        [Column(TypeName = "date")]
        public DateTime EventDate { get; set; }

        public TimeSpan EventStartTime { get; set; }

        public TimeSpan EventEndTime { get; set; }

        [Required]
        public string EventLocation { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EventBooking> EventBookings { get; set; }
    }
}
