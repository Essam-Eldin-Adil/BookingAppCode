using GlobalResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.Models
{
    public class Setting : Entity
    {
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = nameof(Resources.ValidationsRequired), ErrorMessage = null)]
        public string Key { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = nameof(Resources.ValidationsRequired), ErrorMessage = null)]
        public string Value { get; set; }
    }
}
