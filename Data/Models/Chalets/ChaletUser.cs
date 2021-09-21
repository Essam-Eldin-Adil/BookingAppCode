using System;
using System.ComponentModel.DataAnnotations;
using Resources;

namespace Data.Models.Chalets
{
    public class ChaletUser:Entity
    {
        [Display(Name = "SendWhatsAppNotifications", ResourceType = typeof(Resource))]
        public bool SendWhatsAppNotifications { get; set; }
        public bool IsAdmin { get; set; }
        public Guid ChaletId { get; set; }
        public Guid UserId { get; set; }
        public Chalet Chalet { get; set; }
        public User User { get; set; }
    }
}
