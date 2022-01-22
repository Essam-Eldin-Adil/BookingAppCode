using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Models.General
{
    public class Country:Entity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string ISOCode { get; set; }
    }
}
