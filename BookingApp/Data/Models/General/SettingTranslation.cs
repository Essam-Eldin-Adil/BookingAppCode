using iQuarc.DataLocalization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models.General
{
    [TranslationFor(typeof(Setting))]
    public class SettingTranslation
    {
        public int Id { get; set; }
        public int SettingId { get; set; }
        public string Name { get; set; }
        public Setting Setting { get; set; }

    }
}
