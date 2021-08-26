using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.ViewModels
{
    public class GroupViewModel
    {
        public Group Group { get; set; }

        public List<RoleViewModel> Roles { get; set; }

    }

    public class RoleViewModel
    {

        public Role Role { get; set; }
        public bool IsSelected { get; set; }

      

    }
}
