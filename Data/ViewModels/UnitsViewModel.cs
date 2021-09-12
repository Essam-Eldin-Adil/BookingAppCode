using System;
using System.Collections.Generic;
using System.Text;
using Data.Models.Chalets;
using Data.Models.Chalets.ChaletDetails;

namespace Data.ViewModels
{
    public class UnitsViewModel
    {
        public UnitsViewModel()
        {
            Units=new List<Unit>();
            ChaletImages=new List<ChaletImage>();
            Chalet=new Chalet();
        }
        public List<Unit> Units { get; set; }
        public Chalet Chalet { get; set; }
        public List<ChaletImage> ChaletImages { get; set; }

    }

    public class UnitViewModel
    {
        public UnitViewModel()
        {
            PricePerDay = new PricePerDay();
            ParameterGroups = new List<ParameterGroup>();
            Offers =new List<Offer>();
            Unit=new Unit();
            Offer=new Offer();
            ChaletParameterValues=new List<ChaletParameterValue>();
            UnitImage=new List<UnitImage>();
            SimilarUnits = new List<Unit>();
        }
        public Unit Unit { get; set; }
        public PricePerDay PricePerDay { get; set; }
        public List<Offer> Offers { get; set; }
        public List<UnitImage> UnitImage { get; set; }
        public List<Unit> SimilarUnits { get; set; }
        public List<ParameterGroup> ParameterGroups { get; set; }
        public List<ChaletParameterValue> ChaletParameterValues { get; set; }
        public Offer Offer { get; set; }
        public Guid ChaletId { get; set; }
    }
}
