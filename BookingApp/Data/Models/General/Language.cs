using GlobalResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Models
{
    public class Language : Entity
    {
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = nameof(Resources.ValidationsRequired), ErrorMessage = null)]
        public string Name { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = nameof(Resources.ValidationsRequired), ErrorMessage = null)]
        public string Code { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = nameof(Resources.ValidationsRequired), ErrorMessage = null)]
        public string Flag { get; set; }
        public virtual ICollection<LanguageTranslation> LanguageTranslations { get; set; }
        public virtual ICollection<OrganizationTranslation> OrganizationTranslations { get; set; }
        public virtual ICollection<RoleTranslation> RoleTranslations { get; set; }


    }
}
