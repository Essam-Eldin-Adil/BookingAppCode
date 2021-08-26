using iQuarc.DataLocalization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models
{
    [TranslationFor(typeof(Organization))]
    public class OrganizationTranslation : Entity
    {
        public int LanguageId { get; set; }
        public int OrganizationId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual Language Language { get; set; }
        public virtual Organization Organization { get; set; }

    }
}
