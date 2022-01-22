using System;
using Data.Models.General;
using iQuarc.DataLocalization;

namespace Data.Models.Chalets
{
    [TranslationFor(typeof(Region1))]
    public class RegionTranslation : Entity
    {
        public Guid RegionId { get; set; }
        public Guid LanguageId { get; set; }
        public string Name { get; set; }
        public virtual Region1 Region { get; set; }
        public virtual Language Language { get; set; }
    }
}