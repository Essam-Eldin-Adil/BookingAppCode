using iQuarc.DataLocalization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models.General
{
    [TranslationFor(typeof(Setting))]
    public class SettingTranslation:Entity
    {
        public Guid SettingId { get; set; }
        public string Name { get; set; }
        public Setting Setting { get; set; }
        public Guid LanguageId { get; set; }
        public virtual Language Language { get; set; }


    }
}
