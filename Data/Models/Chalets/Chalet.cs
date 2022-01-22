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
        public string Region { get; set; }
        [Display(Name = "Neighborhood", ResourceType = typeof(Resource))]
        public Guid NeighborhoodId { get; set; }
        [Display(Name = "PropertyType", ResourceType = typeof(Resource))]
        public int PropertyType { get; set; }
        public string Neighborhood { get; set; }
        [Display(Name = "CloseToSea", ResourceType = typeof(Resource))]
        public bool CloseToSea { get; set; }
        [Display(Name = "DistanceFromSea", ResourceType = typeof(Resource))]
        public int DistanceFromSea { get; set; }
        [Display(Name = "ChaletCount", ResourceType = typeof(Resource))]
        public int ProprityCount { get; set; }

        //Settings
        [DataType(DataType.Time)]
        [Display(Name = "ENTERTIME", ResourceType = typeof(Resource))]
        public DateTime EnterTime { get; set; }
        [DataType(DataType.Time)]
        [Display(Name = "EXITTIME", ResourceType = typeof(Resource))]
        public DateTime ExitTime { get; set; }
        [Display(Name = "CLEANCONDITION", ResourceType = typeof(Resource))]
        public bool CleanCondition { get; set; }
        public bool InsuranceCondition { get; set; }
        [Display(Name = "FamilyCondition", ResourceType = typeof(Resource))]
        public bool FamilyCondition { get; set; }
        [Display(Name = "MONEYTRANSFERCONDITION", ResourceType = typeof(Resource))]
        public bool MoneyTransferCondition { get; set; }
        [Display(Name = "OTHERCONDITION", ResourceType = typeof(Resource))]
        public string OtherCondition { get; set; }
        public double InsuranceAmount { get; set; }
        [Display(Name = "RESERVATIONMANAGER", ResourceType = typeof(Resource))]
        public string ReservationManager { get; set; }
        [Display(Name = "RESERVATIONPHONENUMBER", ResourceType = typeof(Resource))]
        public string ReservationPhoneNumber { get; set; }
        //Settings

        public ICollection<ResortParameterValue> ResortParameterValue { get; set; }
        public ICollection<ChaletImage> ChaletImages { get; set; }
        public ICollection<ChaletUser> ChaletUsers { get; set; }
        public ICollection<Unit> Units { get; set; }
       // public ICollection<ChaletSetting> ChaletSettings { get; set; }
        public bool IsConfirmed { get; set; }
    }
}
