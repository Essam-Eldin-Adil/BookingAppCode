using System;
using System.Collections.Generic;
using System.Text;

namespace Data.ViewModels.General
{
    public class vmEmailSettings
    {
        public string EmailHost { get; set; }
        public string EmailPort { get; set; }
        public string EmailUsername { get; set; }
        public string EmailPassword { get; set; }
        public bool EmailEnableSSL { get; set; }
    }
}
