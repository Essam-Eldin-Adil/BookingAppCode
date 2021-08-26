//using Domain;
using GlobalResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = nameof(Resources.ValidationsRequired), ErrorMessage = null)]
        public string Username { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = nameof(Resources.ValidationsRequired), ErrorMessage = null)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = nameof(Resources.ValidationsRequired), ErrorMessage = null)]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
        //public Enums.UserType UserType { get; set; } = Enums.UserType.External;

    }
}
