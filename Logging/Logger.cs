using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using impl_search.SetUp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace impl_search.Logging
{
    public static class Logger
    {
        static string fileName = "C:\\Users\\herob\\Uni\\RealUni\\Bachelor\\code\\impl_search_algth\\impl_search\\Logging\\loggedInfo.json";
        static string grid5Name = "C:\\Users\\herob\\Uni\\RealUni\\Bachelor\\code\\impl_search_algth\\impl_search\\Logging\\grid5_maze.json";
        static string grid6Name = "C:\\Users\\herob\\Uni\\RealUni\\Bachelor\\code\\impl_search_algth\\impl_search\\Logging\\grid6_nocorners.json";
        static string grid7Name = "C:\\Users\\herob\\Uni\\RealUni\\Bachelor\\code\\impl_search_algth\\impl_search\\Logging\\grid7.json";
        static string editName = "C:\\Users\\herob\\Uni\\RealUni\\Bachelor\\code\\impl_search_algth\\impl_search\\Logging\\test.json";
        // static string fileName = "/home/mladen/bachlr_project/bchlr_comparing/Logging/loggedInfo.json";
        public static void writeResults(Information info)
        {
            var initialJson = File.ReadAllText(fileName);
            var array = JArray.Parse(initialJson);

            string jsonString = System.Text.Json.JsonSerializer.Serialize(info);

            array.Add(jsonString);

            var jsonToOutput = JsonConvert.SerializeObject(array, Formatting.Indented);


            File.WriteAllText(fileName, jsonToOutput);

        }

        public static void writeEmptyResult()
        {
            Information info = new Information
            {
                whichAlg = "---",
                focusTest = "---",
                Date = DateTime.Parse("2026-03-24"),
                nodesTouched = 0,
                pathLength = 0,
                memoryUsed = 0,
                ticksTaken = 0
            };
            writeResults(info);
        }

        public static CustomMap loadGrid()
        {
            var initialJson = File.ReadAllText(editName);

            CustomMap newMap = JsonConvert.DeserializeObject<CustomMap>(initialJson);

            return newMap;

        }

        public static CustomMap loadGrid5()
        {
            var initialJson = File.ReadAllText(grid5Name);

            CustomMap newMap = JsonConvert.DeserializeObject<CustomMap>(initialJson);

            return newMap;

        }

        public static CustomMap loadGrid6()
        {
            var initialJson = File.ReadAllText(grid6Name);

            CustomMap newMap = JsonConvert.DeserializeObject<CustomMap>(initialJson);

            return newMap;

        }

        public static CustomMap loadGrid7()
        {
            var initialJson = File.ReadAllText(grid7Name);

            CustomMap newMap = JsonConvert.DeserializeObject<CustomMap>(initialJson);

            return newMap;

        }

        public static void saveGrid(CustomMap map)
        {

            string output = JsonConvert.SerializeObject(map);

            File.WriteAllText(editName, output);
        }

        
    }
}
