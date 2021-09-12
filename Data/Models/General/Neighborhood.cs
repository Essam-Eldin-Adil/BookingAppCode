using System;

namespace Data.Models.General
{
    public class Neighborhood : Entity
    {
        public Guid CityId { get; set; }
        public City City { get; set; }
        public string Name { get; set; }
    }
}
