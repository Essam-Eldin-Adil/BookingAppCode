using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data
{
    public abstract class Entity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public int? Order { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; } = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
        public bool IsDeleted { get; set; } = false;
    }
}
