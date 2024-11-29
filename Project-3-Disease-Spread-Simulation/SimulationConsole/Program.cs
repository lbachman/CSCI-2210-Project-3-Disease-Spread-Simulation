
using SimulationLib;
using System.Data.Common;

namespace SimulationConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // file path to test data file
            string filePath = @"..\..\..\..\SimulationLib\TestData\sampleData.txt";

            // config object holding all of the config parameters
            Configuration config = Utility.ParseConfigData(filePath);
            
            Simulation simulation = new Simulation(config);

            simulation.GeneratePopulation();

            // for now just prints id's of locations
            simulation.DisplayLocationInfo();


            
        }
    }
}
