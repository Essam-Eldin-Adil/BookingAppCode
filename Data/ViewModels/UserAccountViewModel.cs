using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.ViewModels
{
    public class UserAccountViewModel
    {
        public User User { get; set; }
        public ResetPasswordViewModel ResetPasswordViewModel { get; set; }
    }
}
