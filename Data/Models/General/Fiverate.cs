using Data.Models.Chalets.ChaletDetails;
using System;

namespace Data.Models
{
    public class Fiverate : Entity
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid UnitId { get; set; }
        public Unit Unit { get; set; }
    }
}
