using System.Collections.Generic;
using Data.Models.Chalets;

namespace Data.ViewModels
{public class RegionViewModel
    {
        public RegionViewModel()
        {
            Region = new Region1();
            RegionTranslations = new List<RegionTranslation>();
            RegionTranslation = new RegionTranslation();
        }
        public Region1 Region { get; set; }
        public RegionTranslation RegionTranslation { get; set; }
        public List<RegionTranslation> RegionTranslations { get; set; }
    }
}