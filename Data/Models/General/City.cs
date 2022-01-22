using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Resources;

namespace Data.Models.General
{
    public class City : Entity
    {
        public City()
        {
            CityTranslations=new List<CityTranslation>();
        }
        public Guid CountryId { get; set; }
        public Country Country { get; set; }
        [Display(Name = "CityName", ResourceType = typeof(Resource))]
        public string CityName { get; set; }
        public Guid? Image { get; set; }
        public ICollection<CityTranslation> CityTranslations { get; set; }
        [NotMapped]
        public string ImageUrl { get; set; }
    }
}
