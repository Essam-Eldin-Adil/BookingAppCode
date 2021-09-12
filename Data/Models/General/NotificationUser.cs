using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.Models
{
    public class NotificationUser : Entity
    {
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.ValidationsRequired), ErrorMessage = null)]
        public Guid NotificationId { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.ValidationsRequired), ErrorMessage = null)]
        public Guid UserId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.ValidationsRequired), ErrorMessage = null)]
        public bool IsRead { get; set; }

        public virtual Notification Notification { get; set; }
        public virtual User User { get; set; }

    }
}
