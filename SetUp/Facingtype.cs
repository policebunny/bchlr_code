using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace impl_search.SetUp
{
    internal enum Facingtype
    {
        FACING_N,           // North
        FACING_NE,          // North-East
        FACING_E,           // East
        FACING_SE,          // South-East
        FACING_S,           // South
        FACING_SW,          // South-West
        FACING_W,           // West
        FACING_NW,        // North-West

        FACING_COUNT			// Total of 8 directions (0..7).
    }
}
