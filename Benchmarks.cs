using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using impl_search.Search;

namespace impl_search
{
    [MemoryDiagnoser]
    public class SortBenchmark
    {

        [Params(0,1,2,3,4,5,6,7)]
        public int whichGrid;

        // Setup method to create an array before each benchmark
        [GlobalSetup]
        public void Setup()
        {
            RunAStarDia.SetUpAStarBig(whichGrid);
            RunCnCDia.SetUpCnCBig(whichGrid);
            RunJPS.SetUpJPSBig(whichGrid);
            
        }

        [Benchmark]
        public void Running_Astar()
        {
            RunAStarDia.GoA_B();
        }

        [Benchmark]
        public void Running_CnC()
        {
            RunCnCDia.GoCnCOG_B();
        }

        [Benchmark]
        public void Running_JPS()
        {
            RunJPS.GoJPS_B();
        }

    }
}
