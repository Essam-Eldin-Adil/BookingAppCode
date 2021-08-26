using iQuarc.DataLocalization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models
{
    [TranslationFor(typeof(Application))]
    public class ApplicationTranslation : Entity
    {
        public int ApplicationId { get; set; }
        public int LanguageId { get; set; }

        public string Name { get; set; }
        public Application Application { get; set; }
        public Language Language { get; set; }

    }
}
