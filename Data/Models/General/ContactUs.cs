using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.General
{
    public class ContactUs:Entity
    {
        [Display(Name = "Name", ResourceType = typeof(Resource))]
        public string Name { get; set; }
        [Display(Name = "Email", ResourceType = typeof(Resource))]
        public string Email { get; set; }
        [Display(Name = "Subject", ResourceType = typeof(Resource))]
        public string Subject { get; set; }
        [Display(Name = "Message", ResourceType = typeof(Resource))]
        public string Message { get; set; }
    }
}
