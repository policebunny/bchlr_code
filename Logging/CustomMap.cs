using impl_search.SetUp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace impl_search.Logging
{
    public class CustomMap
    {
        public SquareGridDiagonal editedGrid {get; set; }
        public Location start {get; set; }
        public Location end {get; set; }


        public static CustomMap makeToMap(SquareGridDiagonal grid_, Location start_, Location end_)
        {
            // Console.WriteLine(start_.x);
            CustomMap map = new CustomMap
            {
                editedGrid = grid_,
                start = start_,
                end = end_,
            };
            return map;
        }

    }
}
