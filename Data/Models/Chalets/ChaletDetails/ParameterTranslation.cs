using System;
using Data.Models.General;
using iQuarc.DataLocalization;

namespace Data.Models.Chalets.ChaletDetails
{
    [TranslationFor(typeof(Parameter))]
    public class ParameterTranslation : Entity
    {
        public Guid ParameterId { get; set; }
        public Guid LanguageId { get; set; }
        public string Name { get; set; }
        public virtual Parameter Parameter { get; set; }
        public virtual Language Language { get; set; }
    }
}