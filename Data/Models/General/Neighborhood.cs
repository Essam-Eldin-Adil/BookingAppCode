using System;
using Data.Models.Chalets;

namespace Data.Models.General
{
    public class Neighborhood : Entity
    {
        public Guid RegionId { get; set; }
        public Region Region { get; set; }
        public string Name { get; set; }
    }
}
