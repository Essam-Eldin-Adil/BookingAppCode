using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Data.Models;
using Data.Models.Chalets;
using Data.Models.Chalets.ChaletDetails;
using Data.Models.Chalets.RatingAndReview;
using Resources;

namespace Data.ViewModels
{
    public class UnitsViewModel
    {
        public UnitsViewModel()
        {
            Units=new List<Unit>();
            ChaletImages=new List<ChaletImage>();
            Chalet=new Chalet();
        }
        public List<Unit> Units { get; set; }
        public Chalet Chalet { get; set; }
        public List<ChaletImage> ChaletImages { get; set; }

    }
    public class ChaletViewModel
    {
        public Chalet Chalet { get; set; }
        public Unit Unit { get; set; }
        public PricePerDay PricePerDay { get; set; }
        public Offer Offer { get; set; }
        public ChaletViewModel()
        {
            Offer = new Offer();
            PricePerDay = new PricePerDay();
            Unit = new Unit();
            Chalet = new Chalet();
            ParameterGroups = new List<ParameterGroup>();
            ResortParameterValues = new List<ResortParameterValue>();
            ChaletParameterValues = new List<ChaletParameterValue>();
            ChaletUsers = new List<ChaletUser>();
            User = new User();
            Offers = new List<Offer>();
        }
        public List<Offer> Offers { get; set; }
        public List<ParameterGroup> ParameterGroups { get; set; }
        public List<ResortParameterValue> ResortParameterValues { get; set; }
        public List<ChaletParameterValue> ChaletParameterValues { get; set; }
        public User User { get; set; }
        public List<ChaletUser> ChaletUsers { get; set; }
    }
    public class UnitViewModel
    {
        public UnitViewModel()
        {
            PricePerDay = new PricePerDay();
            ParameterGroups = new List<ParameterGroup>();
            Offers =new List<Offer>();
            Unit=new Unit();
            Offer=new Offer();
            ChaletParameterValues=new List<ChaletParameterValue>();
            UnitImage=new List<UnitImage>();
            SimilarUnits = new List<Unit>();
        }
        public Unit Unit { get; set; }
        public PricePerDay PricePerDay { get; set; }
        public List<Offer> Offers { get; set; }
        public List<UnitImage> UnitImage { get; set; }
        public List<Unit> SimilarUnits { get; set; }
        public List<ParameterGroup> ParameterGroups { get; set; }
        public List<ChaletParameterValue> ChaletParameterValues { get; set; }
        public Offer Offer { get; set; }
        public Guid ChaletId { get; set; }
    }

    public class SingleUnitViewModel
    {
        public SingleUnitViewModel()
        {
            ParameterGroups = new List<ParameterGroup>();
            ResortParameterGroups = new List<ParameterGroup>();
            Rate = new Rate();
            Rates = new List<Rate>();
        }
        public Unit Unit { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public List<ParameterGroup> ParameterGroups { get; set; }
        public List<ParameterGroup> ResortParameterGroups { get; set; }
        public Guid Id { get; set; }
        [Display(Name = "Email", ResourceType = typeof(Resource))]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Guid Country { get; set; }
        [Display(Name = "Password", ResourceType = typeof(Resource))]
        public string Password { get; set; }
        public int Code { get; set; }
        public bool isPassword { get; set; }
        public bool isRegistration { get; set; }
        public bool isPostback { get; set; }
        public int UserType { get; set; }
        public Rate Rate { get; set; }
        public List<Rate> Rates { get; set; }
    }
}
