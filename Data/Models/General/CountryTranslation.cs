using System;
using iQuarc.DataLocalization;

namespace Data.Models.General
{
    [TranslationFor(typeof(Country))]
    public class CountryTranslation : Entity
    {
        public Guid CountryId { get; set; }
        public Guid LanguageId { get; set; }
        public string Name { get; set; }
        public virtual Country Country { get; set; }
        public virtual Language Language { get; set; }
    }
}