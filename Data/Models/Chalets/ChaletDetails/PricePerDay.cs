using System;
using System.ComponentModel.DataAnnotations;
using Resources;

namespace Data.Models.Chalets.ChaletDetails
{
    public class PricePerDay:Entity
    {
        [Display(Name= "Saturday",ResourceType = typeof(Resource))]
        public double Saturday { get; set; }
        [Display(Name= "Sunday", ResourceType = typeof(Resource))]
        public double Sunday { get; set; }
        [Display(Name= "Monday", ResourceType = typeof(Resource))]
        public double Monday { get; set; }
        [Display(Name= "Tuesday", ResourceType = typeof(Resource))]
        public double Tuesday { get; set; }
        [Display(Name= "Wednesday", ResourceType = typeof(Resource))]
        public double Wednesday { get; set; }
        [Display(Name= "Thursday", ResourceType = typeof(Resource))]
        public double Thursday { get; set; }
        [Display(Name= "Friday", ResourceType = typeof(Resource))]
        public double Friday { get; set; }
        public Guid UnitId { get; set; }
        public Unit Unit { get; set; }
    }
}