using GlobalResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Models
{
    public class GroupRole : Entity
    {
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = nameof(Resources.ValidationsRequired), ErrorMessage = null)]
        public Guid GroupId { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = nameof(Resources.ValidationsRequired), ErrorMessage = null)]
        public Guid RoleId { get; set; }
        public virtual Group Group { get; set; }
        public virtual Role Role { get; set; }

    }
}
