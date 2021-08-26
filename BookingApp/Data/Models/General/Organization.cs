using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.Models
{
    public class Organization : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }

    }
}
