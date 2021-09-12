using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.Models.General
{
    public class Language : Entity
    {
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.ValidationsRequired), ErrorMessage = null)]
        public string Name { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.ValidationsRequired), ErrorMessage = null)]
        public string Code { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.ValidationsRequired), ErrorMessage = null)]
        public string Flag { get; set; }

        public virtual ICollection<RoleTranslation> RoleTranslations { get; set; }


    }
}
