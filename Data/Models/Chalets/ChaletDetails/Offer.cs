using System;
using System.ComponentModel.DataAnnotations;
using Resources;

namespace Data.Models.Chalets.ChaletDetails
{
    public class Offer : Entity
    {

        [Display(Name = "DateFrom", ResourceType = typeof(Resource))]
        [DataType(DataType.Date)]
        public DateTime DateFrom { get; set; } = DateTime.Now;
        [Display(Name = "DateTo", ResourceType = typeof(Resource))]
        [DataType(DataType.Date)]
        public DateTime DateTo { get; set; } = DateTime.Now;
        [Display(Name = "Amount", ResourceType = typeof(Resource))]
        public double Amount { get; set; }
        public Unit Unit { get; set; }
        public Guid UnitId { get; set; }
    }
}