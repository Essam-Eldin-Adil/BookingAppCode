using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Data.Models.Chalets;
using Resources;

namespace Data.Models.General
{
    public class Bank:Entity
    {
        [Display(Name = "BankName", ResourceType = typeof(Resource))]
        public string Name { get; set; }
        public Guid? Image { get; set; }
        public ICollection<BankTranslation> BankTranslations { get; set; }

        [NotMapped]
        public string ImageUrl { get; set; }
    }
}
