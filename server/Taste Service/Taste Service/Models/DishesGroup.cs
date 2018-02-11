using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Taste_Service.Models
{
    public class DishesGroup
    {
        public string Category { get; set; }
        public List<Dish> Dishes { get; set; }
    }
}