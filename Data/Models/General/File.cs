using Resources;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Models
{
    public class File : Entity
    {
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.ValidationsRequired), ErrorMessage = null)]
        public string Name { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.ValidationsRequired), ErrorMessage = null)]
        public long Size { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.ValidationsRequired), ErrorMessage = null)]
        public string Type { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.ValidationsRequired), ErrorMessage = null)]
        public string FileContent { get; set; }
        public string FileContentMin { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.ValidationsRequired), ErrorMessage = null)]
        public bool IsSecure { get; set; }
    }
}
