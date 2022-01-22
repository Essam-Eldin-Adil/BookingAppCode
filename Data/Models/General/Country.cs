using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Resources;

namespace Data.Models.General
{
    public class Country:Entity
    {
        [Display(Name = "CountryName", ResourceType = typeof(Resource))]
        public string Name { get; set; }
        [Display(Name = "CountryCode", ResourceType = typeof(Resource))]
        public string Code { get; set; }
        public string ISOCode { get; set; }
    }
}
