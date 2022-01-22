using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Data.Models.Chalets;
using Resources;

namespace Data.Models.General
{
    public class Neighborhood1 : Entity
    {
        public Guid RegionId { get; set; }
        //public Region Region { get; set; }
        [Display(Name = "NeighborhoodName", ResourceType = typeof(Resource))]
        public string Name { get; set; }
        public ICollection<NeighborhoodTranslation> NeighborhoodTranslations { get; set; }
    }
}
