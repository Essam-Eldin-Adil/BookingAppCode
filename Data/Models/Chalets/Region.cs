using System;
using Data.Models.General;

namespace Data.Models.Chalets
{
    public class Region:Entity
    {
        public string Name { get; set; }
        public Guid CityId { get; set; }
        public City City { get; set; }
    }
}