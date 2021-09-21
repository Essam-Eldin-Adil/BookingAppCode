using Data.Models.General;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Data.Models.Chalets.ChaletDetails;
using Resources;

namespace Data.Models.Chalets
{
    public class Chalet:Entity
    {
        public Chalet()
        {
            ChaletImages = new List<ChaletImage>();
        }
        [Display(Name = "PropertyName", ResourceType = typeof(Resource))]
        public string ChaletName { get; set; }
        [Display(Name = "ViewStatus", ResourceType = typeof(Resource))]
        public bool ViewStatus { get; set; }
        [Display(Name = "Description", ResourceType = typeof(Resource))]
        public string Description { get; set; }
        [Display(Name = "Location", ResourceType = typeof(Resource))]
        public string Location { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        [Display(Name = "Direction", ResourceType = typeof(Resource))]
        public int Direction { get; set; }
        [Display(Name = "City", ResourceType = typeof(Resource))]
        public Guid CityId { get; set; }
        public City City { get; set; }
        [Display(Name = "Region", ResourceType = typeof(Resource))]
        public Guid RegionId { get; set; }
        public Region Region { get; set; }
        [Display(Name = "Neighborhood", ResourceType = typeof(Resource))]
        public Guid NeighborhoodId { get; set; }
        [Display(Name = "PropertyType", ResourceType = typeof(Resource))]
        public int PropertyType { get; set; }
        public Neighborhood Neighborhood { get; set; }
        public bool CloseToSea { get; set; }
        public int DistanceFromSea { get; set; }
        public ICollection<ChaletImage> ChaletImages { get; set; }
        public ICollection<Unit> Units { get; set; }
        public bool IsConfirmed { get; set; }
    }
}
