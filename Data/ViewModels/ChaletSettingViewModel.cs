using System;
using System.Collections.Generic;
using System.Text;
using Data.Models.Chalets;
using Data.Models.General;

namespace Data.ViewModels
{
    public class ChaletSettingViewModel
    {
        public ChaletSettingViewModel()
        {
            ChaletBank = new ChaletBank();
            Banks = new List<Bank>();
            ChaletSetting = new ChaletSetting();
            ChaletBanks = new List<ChaletBank>();
        }
        public ChaletBank ChaletBank { get; set; }
        public List<Bank> Banks { get; set; }
        public ChaletSetting ChaletSetting { get; set; }
        public List<ChaletBank> ChaletBanks { get; set; }
    }
}
