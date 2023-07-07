using System;
using System.Collections.Generic;
using System.Text;

namespace Cinema.Domain.Domain_models
{
    public class AddToRoleModel
    {
        public List<string> Emails { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public string SelectedRole { get; set; }
    }
}
