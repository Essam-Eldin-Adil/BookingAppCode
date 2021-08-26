using GlobalResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.ViewModels
{
    public class ResetViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = nameof(Resources.ValidationsRequired), ErrorMessage = null)]
        public string Token { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = nameof(Resources.ValidationsRequired), ErrorMessage = null)]
        public string UserName { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = nameof(Resources.ValidationsRequired), ErrorMessage = null)]
        public string NewPassword { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = nameof(Resources.ValidationsRequired), ErrorMessage = null)]
        public string ConfirmPassword { get; set; }
    }
}
