using System;
using iQuarc.DataLocalization;

namespace Data.Models.General
{
    [TranslationFor(typeof(Bank))]
    public class BankTranslation : Entity
    {
        public Guid BankId { get; set; }
        public Guid LanguageId { get; set; }
        public string Name { get; set; }
        public virtual Bank Bank { get; set; }
        public virtual Language Language { get; set; }
    }
}