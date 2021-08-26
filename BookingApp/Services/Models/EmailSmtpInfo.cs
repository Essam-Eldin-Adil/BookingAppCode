using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Models
{
    public class EmailSmtpInfo
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
