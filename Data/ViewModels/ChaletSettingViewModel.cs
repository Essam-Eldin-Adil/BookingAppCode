using System;
using System.Collections.Generic;
using System.Text;
using Data.Models;
using Data.Models.Chalets;
using Data.Models.Chalets.ChaletDetails;
using Data.Models.General;

namespace Data.ViewModels
{
    public class ChaletSettingViewModel
    {
        public ChaletSettingViewModel()
        {
            ChaletBank = new ChaletBank();
            Banks = new List<Bank>();
            Chalet = new Chalet();
            ChaletBanks = new List<ChaletBank>();
            Offers = new List<Offer>();
            Offer = new Offer();
            Unit = new Unit();
        }
        public Unit Unit { get; set; }
        public Offer Offer { get; set; }
        public List<Offer> Offers { get; set; }
        public ChaletBank ChaletBank { get; set; }
        public List<Bank> Banks { get; set; }
        public Chalet Chalet { get; set; }
        public List<ChaletBank> ChaletBanks { get; set; }
    }
}
