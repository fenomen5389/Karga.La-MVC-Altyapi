using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DTOs
{
    public class AuthorAddDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mail { get; set; }
        public string VisibleName { get; set; }
        public string Biography { get; set; }
        public string Password { get; set; }
    }
}
