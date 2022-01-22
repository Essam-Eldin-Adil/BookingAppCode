using Data.Models.Chalets;
using Data.Models.Chalets.RatingAndReview;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.ViewModels
{
    public class UserReservationViewModel:Reservation
    {
        public Rate UserRate { get; set; }
        public Rate Rate { get; set; } = new Rate();
        public int RateCount { get; set; }
        public double CleaningRate { get; set; }
        public double CrewRate { get; set; }
        public double PropertyConditionRate { get; set; }
}
}
