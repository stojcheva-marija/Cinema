using System;
using System.Collections.Generic;
using System.Text;

namespace Cinema.Domain.DTO
{
    public class RegistrationDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
