using System.Collections.Generic;
using Data.Models.General;

namespace Data.ViewModels
{
    public class BankViewModel
    {
        public BankViewModel()
        {
            Bank = new Bank();
            BankTranslations = new List<BankTranslation>();
            BankTranslation = new BankTranslation();
        }
        public Bank Bank { get; set; }
        public BankTranslation BankTranslation { get; set; }
        public List<BankTranslation> BankTranslations { get; set; }
    }
}