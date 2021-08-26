using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Models
{
    public class Role : IdentityRole<Guid>
    {
        public virtual ICollection<GroupRole> GroupRoles { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<RoleTranslation> RoleTranslations { get; set; }


    }
}
