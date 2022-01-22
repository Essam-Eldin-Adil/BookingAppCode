using System;
using Data.Models.General;
using iQuarc.DataLocalization;

namespace Data.Models.Chalets.ChaletDetails
{
    [TranslationFor(typeof(ParameterGroup))]
    public class ParameterGroupTranslation : Entity
    {
        public Guid ParameterGroupId { get; set; }
        public Guid LanguageId { get; set; }
        public string Name { get; set; }
        public virtual ParameterGroup ParameterGroup { get; set; }
        public virtual Language Language { get; set; }
    }
}