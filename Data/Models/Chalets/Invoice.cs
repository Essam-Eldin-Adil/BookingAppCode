using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.Chalets
{
    public class Invoice:Entity
    {
        public string PaymentMethod { get; set; }
        public double PaymentAmount { get; set; }
        public Guid ReservationId { get; set; }
        public Guid UserId { get; set; }
        public Reservation Reservation { get; set; }
        public User User { get; set; }
        public DateTime PaymentDateTime { get; set; }

    }
}
