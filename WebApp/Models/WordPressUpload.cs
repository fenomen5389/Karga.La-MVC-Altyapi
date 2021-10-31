using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class WordPressUpload
    {
        public IFormFile File { get; set; }
    }
}
