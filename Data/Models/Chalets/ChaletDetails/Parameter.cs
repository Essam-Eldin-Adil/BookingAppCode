using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public ICollection<ParameterTranslation> ParameterTranslations { get; set; }

        [NotMapped]
        public string Value { get; set; }
    }
}