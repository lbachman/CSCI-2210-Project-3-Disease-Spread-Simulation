
using SimulationLib;
using System.Data.Common;

namespace SimulationConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // file path to test data file
                string filePath = @"..\..\..\..\SimulationLib\TestData\sampleData.txt";

                // config object holding all of the config parameters
                Configuration config = Utility.ParseConfigData(filePath);

                Simulation simulation = new Simulation(config);

                // display config data read from .txt file
                Console.WriteLine("Configuration Info:");
                Console.WriteLine(config.ToString());
                Console.WriteLine();
                Console.WriteLine();


                // initializes the populations and shows log data for each
                Console.WriteLine("Population Info:");
                simulation.GeneratePopulation();
                Console.WriteLine();
                Console.WriteLine();


                // run the simulation
                Console.WriteLine("Starting simulation...");
                simulation.RunSimulation();
                Console.WriteLine("Simulation completed.");


                simulation.GatherStatistics();


               

            }
            catch (Exception ex) 
            {
                Console.WriteLine($"An error occured: {ex.Message}");
            }

            
        }
    }
}
