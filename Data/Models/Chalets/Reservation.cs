using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Data.Models.Chalets.ChaletDetails;
using Data.Models.Chalets.RatingAndReview;

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
        public double DayPrice { get; set; }
        public double TotalPrice { get; set; }
        public int TotalDays { get; set; }
        public int Status { get; set; }
        public int ReservedBy { get; set; }
        public string ReservedByUser { get; set; }
        public string CancelResones { get; set; }
        public ICollection<PaymentTransaction> Invoices { get; set; }
    }

    public class PaymentTransaction:Entity
    {
        public int PaymentType { get; set; }
        public DateTime PaymentDateTime { get; set; }
        public double Amount { get; set; }
        public Guid UserId { get; set; }
        public Guid ReservationId { get; set; }
        public Reservation Reservation { get; set; }
        public User User { get; set; }
        public string RefNo { get; set; }
    }
}
