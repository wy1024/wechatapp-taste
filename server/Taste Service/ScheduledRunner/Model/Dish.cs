using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Taste_Service.Models
{
    public class Dish
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Flavors { get; set; }
        public string Ingredients { get; set; }
        public string Category { get; set; }
        public double Price { get; set; }
        public int Cuisine { get; set; }
        public Stream Image { get; set; }
    }
}