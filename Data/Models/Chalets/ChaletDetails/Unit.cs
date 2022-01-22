using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Resources;

namespace Data.Models.Chalets.ChaletDetails
{
    public class Unit : Entity
    {
        public Unit()
        {
            Offers = new List<Offer>();
        }
        [Display(Name = "UnitName", ResourceType = typeof(Resource))]
        public string Name { get; set; }
        public int Number { get; set; }
        [Display(Name = "UnitCode", ResourceType = typeof(Resource))]
        public string Code { get; set; }
        [Display(Name = "UnitCount", ResourceType = typeof(Resource))]
        public int Count { get; set; }
        [Display(Name = "ViewStatus", ResourceType = typeof(Resource))]
        public bool ViewStatus { get; set; }
        [Display(Name = "UnitSpace", ResourceType = typeof(Resource))]
        public int Space { get; set; }
        [Display(Name = "Description", ResourceType = typeof(Resource))]
        public string Description { get; set; }
        [Display(Name = "DepositAmount", ResourceType = typeof(Resource))]
        public double DepositAmount { get; set; }
        [Display(Name = "DayPrice", ResourceType = typeof(Resource))]
        public double DayPrice { get; set; }

        public bool IsDayPrice { get; set; } = true;
        public ICollection<Offer> Offers { get; set; }
                public ICollection<UnitImage> UnitImages { get; set; }
public ICollection<ChaletParameterValue> ChaletParameterValues { get; set; }
       public ICollection<PricePerDay> PricePerDays { get; set; }
 public Guid ChaletId { get; set; }
        public Chalet Chalet { get; set; }
        public bool IsSimilar { get; set; }
        public Guid OriginId { get; set; }
        [Display(Name = "HaveSimilarChalets", ResourceType = typeof(Resource))]
        public bool HaveSimilar { get; set; }
        [Display(Name = "PromiseChangePrices", ResourceType = typeof(Resource))]
        public bool PromiseChangePrices { get; set; }
        public int AllowedPersons { get; set; }
        public int MaximumAllowed { get; set; }
        public bool MoreThanAllowed { get; set; }
        public double MoreThanAllowedPrice { get; set; }
        public int Views { get; set; }

        [NotMapped]
        public bool IsReserved { get; set; }

    }
}