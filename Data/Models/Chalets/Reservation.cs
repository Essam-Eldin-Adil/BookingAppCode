using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Data.Models.Chalets.ChaletDetails;

namespace Data.Models.Chalets
{
    public class Reservation:Entity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReservationNumber { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid UnitId { get; set; }
        public Unit Unit { get; set; }
        public int Status { get; set; }
    }
}
