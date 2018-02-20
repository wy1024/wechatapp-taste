using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Taste_Service.Models
{
    public class Restaurant
    {
        public string Name { get; set; }
        public string CloverId { get; set; }
        public string Location { get; set; }
        public string Phone { get; set; }
        public string Owner { get; set; }
        public Stream Image { get; set; }
    }
}