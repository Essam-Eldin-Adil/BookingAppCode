using System;
using iQuarc.DataLocalization;

namespace Data.Models.General
{
    [TranslationFor(typeof(Neighborhood1))]
    public class NeighborhoodTranslation : Entity
    {
        public Guid NeighborhoodId { get; set; }
        public Guid LanguageId { get; set; }
        public string Name { get; set; }
        public virtual Neighborhood1 Neighborhood { get; set; }
        public virtual Language Language { get; set; }
    }
}