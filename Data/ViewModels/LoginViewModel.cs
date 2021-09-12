using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Resources;

namespace Data.ViewModels
{
    public class LoginViewModel
    {
        public Guid Id { get; set; }
        [Display(Name="Email",ResourceType = typeof(Resource))]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Guid Country { get; set; }
        [Display(Name = "Password", ResourceType = typeof(Resource))]
        public string Password { get; set; }
        public int Code { get; set; }
        public bool isPassword { get; set; }
        public bool isRegistration { get; set; }
        public bool isPostback { get; set; }
        public int UserType { get; set; }
    }

    public class ResetPasswordViewModel
    {
        public Guid UserId { get; set; }
        [Display(Name = "Password", ResourceType = typeof(Resource))]
        public string Password { get; set; }
        [Display(Name = "ConfirmPassword", ResourceType = typeof(Resource))]
        public string ConfirmPassword { get; set; }
    }
    }
