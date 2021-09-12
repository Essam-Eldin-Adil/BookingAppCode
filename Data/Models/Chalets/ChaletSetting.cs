using System;
using System.ComponentModel.DataAnnotations;
using Resources;

namespace Data.Models.Chalets
{
    public class ChaletSetting:Entity
    {
        [DataType(DataType.Time)]
        [Display(Name= "ENTERTIME", ResourceType = typeof(Resource))]
        public DateTime EnterTime { get; set; }
        [DataType(DataType.Time)]
        [Display(Name= "EXITTIME", ResourceType = typeof(Resource))]
        public DateTime ExitTime { get; set; }
        [Display(Name= "CLEANCONDITION", ResourceType = typeof(Resource))]
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
        public Guid ChaletId { get; set; }
        public Chalet Chalet { get; set; }
    }
}