using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models.General
{
    public class Bank:Entity
    {
        public string Name { get; set; }
        public Guid? Image { get; set; }
    }
}
