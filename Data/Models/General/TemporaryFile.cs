using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Models
{
    public class TemporaryFile : Entity
    {

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.ValidationsRequired), ErrorMessage = null)]
        public string Name { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.ValidationsRequired), ErrorMessage = null)]
        public int Size { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.ValidationsRequired), ErrorMessage = null)]
        public string Type { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.ValidationsRequired), ErrorMessage = null)]
        public byte BLOB { get; set; }
    }
}
