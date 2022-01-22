using System;
using System.Collections.Generic;
using System.Text;
using Data.Models.Chalets;
using Data.Models.General;

namespace Data.ViewModels
{
    public class NeighborhoodViewModel
    {
        public NeighborhoodViewModel()
        {
            Neighborhood = new Neighborhood1();
            NeighborhoodTranslations = new List<NeighborhoodTranslation>();
            NeighborhoodTranslation = new NeighborhoodTranslation();
        }
        public Neighborhood1 Neighborhood { get; set; }
        public NeighborhoodTranslation NeighborhoodTranslation { get; set; }
        public List<NeighborhoodTranslation> NeighborhoodTranslations { get; set; }
    }

}
