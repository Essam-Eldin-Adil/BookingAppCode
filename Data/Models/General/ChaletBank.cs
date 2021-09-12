using Data.Models.Chalets;
using System;
using System.ComponentModel.DataAnnotations;
using Resources;

namespace Data.Models.General
{
    public class ChaletBank : Entity
    {
        [Display(Name = "ACCOUNTNAME", ResourceType = typeof(Resource))]
        public string AccountName { get; set; }
        [Display(Name = "ACCOUNTNUMBER", ResourceType = typeof(Resource))]
        public string AccountNumber { get; set; }
        [Display(Name = "IBAN", ResourceType = typeof(Resource))]
        public string IBan { get; set; }
        [Display(Name = "BANKID", ResourceType = typeof(Resource))]
        public Guid BankId { get; set; }
        public Bank Bank { get; set; }
        public Guid ChaletId { get; set; }
        public Chalet Chalet { get; set; }
    }
}
