using Resources;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Data.Models.General;

namespace Data.Models
{
    public class User:Entity
    {
        [Display(Name = "FirstName", ResourceType = typeof(Resource))]
        public string FirstName { get; set; }
        [Display(Name = "LastName", ResourceType = typeof(Resource))]
        public string LastName { get; set; }
        public int UserType { get; set; }
        [Display(Name = "Image", ResourceType = typeof(Resource))]
        public Guid? Image { get; set; }
        //[Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.ValidationsRequired), ErrorMessage = null)]
        [Display(Name = "PhoneNumber", ResourceType = typeof(Resource))]
        public string PhoneNumber { get; set; }
        [Display(Name = "WhatsAppNumber", ResourceType = typeof(Resource))]
        public string WhatsAppNumber { get; set; }
        [Display(Name = "UserName", ResourceType = typeof(Resource))]
        public string UserName { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.ValidationsRequired), ErrorMessage = null)] 
        [Display(Name = "Email", ResourceType = typeof(Resource))]
        public string Email { get; set; }
        [Display(Name = "ConfirmCode", ResourceType = typeof(Resource))]
        public long ConfirmCode { get; set; }
        public bool IsConfirmed { get; set; }
        public bool Status { get; set; }
        [Display(Name = "Password", ResourceType = typeof(Resource))]
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "BirthDate", ResourceType = typeof(Resource))]
        public DateTime BirthDate { get; set; }
        public DateTime? LastActivity { get; set; }
        public bool TemporaryPassword { get; set; } = false;
        [Display(Name = "JobId", ResourceType = typeof(Resource))]
        public Guid? JobId { get; set; } = Guid.Empty;

        [NotMapped] public bool WhatsAppNotifications { get; set; }
    }
}
