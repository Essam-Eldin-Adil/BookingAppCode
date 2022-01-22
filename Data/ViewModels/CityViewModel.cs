using System;
using System.Collections.Generic;
using System.Text;
using Data.Models.General;

namespace Data.ViewModels
{
    public class CityViewModel
    {
        public CityViewModel()
        {
            City = new City();
            CityTranslations = new List<CityTranslation>();
            CityTranslation = new CityTranslation();
        }
        public City City { get; set; }
        public CityTranslation CityTranslation { get; set; }
        public List<CityTranslation> CityTranslations { get; set; }
    }
}
