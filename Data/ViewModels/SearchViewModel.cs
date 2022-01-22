using System;
using System.Collections.Generic;
using System.Text;
using Data.Models.Chalets.ChaletDetails;
using Data.Models.General;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Data.ViewModels
{
    public class SearchViewModel
    {
        public SearchViewModel()
        {
            ParameterGroups=new List<ParameterGroup>();
            Cities = new List<City>();
        }
        public List<City> Cities { get; set; }
        public List<Guid> City { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public List<int> ProprtyType { get; set; }
        public List<ParameterGroup> ParameterGroups { get; set; }
    }

    public class SearchItemViewModel
    {
        public List<Unit> Units { get; set; }
        public int PageNumber { get; set; }
        public int TotalRecord { get; set; }
        public int PageSize { get; set; }
    }
}
