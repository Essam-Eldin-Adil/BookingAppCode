using System;
using iQuarc.DataLocalization;

namespace Data.Models.General
{
    [TranslationFor(typeof(City))]
    public class CityTranslation : Entity
    {
        public Guid CityId { get; set; }
        public Guid LanguageId { get; set; }
        public string CityName { get; set; }
        public virtual City City { get; set; }
        public virtual Language Language { get; set; }
    }
}