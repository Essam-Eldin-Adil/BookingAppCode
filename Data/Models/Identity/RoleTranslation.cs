using Resources;
using iQuarc.DataLocalization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Data.Models.General;

namespace Data.Models
{
    [TranslationFor(typeof(Role))]
    public class RoleTranslation : Entity
    {
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.ValidationsRequired), ErrorMessage = null)]
        public Guid RoleId { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.ValidationsRequired), ErrorMessage = null)]
        public Guid LanguageId { get; set; }
        public string NormalizedName { get; set; }
        public virtual Language Language { get; set; }
        public virtual Role Role { get; set; }

    }
}
