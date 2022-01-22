using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Data.Models.General;
using Resources;

namespace Data.Models.Chalets
{
    public class Region1:Entity
    {
        [Display(Name = "RegionName", ResourceType = typeof(Resource))]
        public string Name { get; set; }
        public Guid CityId { get; set; }
        public City City { get; set; }
        public ICollection<RegionTranslation> RegionTranslations { get; set; }
    }
}