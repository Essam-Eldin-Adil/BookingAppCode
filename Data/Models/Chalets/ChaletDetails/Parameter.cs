using System;
using System.ComponentModel.DataAnnotations;
using Resources;

namespace Data.Models.Chalets.ChaletDetails
{
    public class Parameter : Entity
    {
        [Display(Name="Name",ResourceType = typeof(Resource))]
        public string Name { get; set; }
        [Display(Name="ParameterType",ResourceType = typeof(Resource))]
        public int Type { get; set; }
        [Display(Name = "Index", ResourceType = typeof(Resource))]
        public int Index { get; set; }

        public ParameterGroup ParameterGroup { get; set; }
        public Guid ParameterGroupId { get; set; }
    }
}