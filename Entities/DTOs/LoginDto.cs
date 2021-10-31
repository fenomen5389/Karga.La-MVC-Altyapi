using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DTOs
{
    public class LoginDto
    {
        public string Mail { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
