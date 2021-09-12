using System;

namespace Data.Models.General
{
    public class City : Entity
    {
        public Guid CountryId { get; set; }
        public Country Country { get; set; }
        public string CityName { get; set; }
        public Guid? Image { get; set; }
    }
}
