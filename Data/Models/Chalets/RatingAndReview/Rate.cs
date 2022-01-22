using Data.Models.Chalets.ChaletDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.Chalets.RatingAndReview
{
    public class Rate : Entity
    {
        public string Comment { get; set; }
        public double Cleaning { get; set; }
        public double Crew { get; set; }
        public double PropertyCondition { get; set; }
        public double RateCount { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid UnitId { get; set; }
        public Unit Unit { get; set; }
    }
}
