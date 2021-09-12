using System;

namespace Data.Models.Chalets
{
    public class ChaletUser:Entity
    {
        public bool IsAdmin { get; set; }
        public Guid ChaletId { get; set; }
        public Guid UserId { get; set; }
        public Chalet Chalet { get; set; }
        public User User { get; set; }
    }
}
