using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace impl_search.Logging
{
    public class Information
    {
        public string whichAlg { get; set; }
        public string focusTest { get; set; }
        public DateTimeOffset Date { get; set; }
        public int nodesTouched {get; set; }
        public int pathLength { get; set; }
        public long memoryUsed { get; set; }
        public long ticksTaken {get; set; }

    }

}
