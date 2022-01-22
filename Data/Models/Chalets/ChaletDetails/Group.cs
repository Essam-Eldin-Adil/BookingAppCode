using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Resources;

namespace Data.Models.Chalets.ChaletDetails
{
    public class ParameterGroup:Entity
    {
        public ParameterGroup()
        {
            Parameters = new List<Parameter>();
        }
        [Required(ErrorMessageResourceName  = "ValidationsRequired", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name="GroupName",ResourceType = typeof(Resource))]
        public string Name { get; set; }
        public bool IsChild { get; set; }
        public Guid ParentId { get; set; }
        public Guid? Image { get; set; }
        public bool Filterable { get; set; }
        [Display(Name = "PropertyType", ResourceType = typeof(Resource))]
        public int PropertyType { get; set; }
        public ICollection<Parameter> Parameters { get; set; }
    }
}
