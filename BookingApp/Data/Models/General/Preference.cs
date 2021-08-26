using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models.General
{
    public class Preference : Entity
    {
        public int SettingId { get; set; }
        public int? SettingOptionId { get; set; }
        public string Value { get; set; }
      

    }
}
