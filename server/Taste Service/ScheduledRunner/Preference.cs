using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduledRunner
{
    public class Preference
    {
        public List<string> Dishes { get; set; }
        public List<string> Ingredients { get; set; }
        public List<string> Flavors { get; set; }
        public List<int> Cuisines { get; set; }

    }
}
