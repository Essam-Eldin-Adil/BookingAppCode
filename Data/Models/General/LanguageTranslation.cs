using Data.Models.General;
using iQuarc.DataLocalization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models
{
    [TranslationFor(typeof(Language))]
    public class LanguageTranslation : Entity
    {
        public int LanguageId { get; set; }
        public string Name { get; set; }
        public virtual Language Language { get; set; }
    }
}
