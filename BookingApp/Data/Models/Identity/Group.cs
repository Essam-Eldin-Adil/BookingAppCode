using GlobalResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Models
{
    public class Group : Entity
    {

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = nameof(Resources.ValidationsRequired), ErrorMessage = null)]
        public string Name { get; set; }
        
        public virtual ICollection<UserRole> GroupRoles { get; set; }
        public virtual ICollection<RoleTranslation> GroupTranslations { get; set; }
    }
}
