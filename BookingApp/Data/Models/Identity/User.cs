using GlobalResources;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Models
{
    public class User : IdentityUser<Guid>
    {
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = nameof(Resources.ValidationsRequired), ErrorMessage = null)]
        public string Name { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = nameof(Resources.ValidationsRequired), ErrorMessage = null)]
        public int Type { get; set; }
        public string Position { get; set; }
        [NotMapped]
        public string CurrentPassword { get; set; }
        [NotMapped]
        public string Password { get; set; }
        [NotMapped]
        public string ConfirmPassword { get; set; }

        public Guid? Image { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }

        public DateTime? LastActivity { get; set; }
        public Guid? Signature { get; set; }
        public virtual ICollection<NotificationUser> NotificationUsers { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }

    }
}
