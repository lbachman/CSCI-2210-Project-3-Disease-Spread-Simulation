
using SimulationLib;

namespace SimulationConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {

            string filePath = @"..\..\..\..\SimulationLib\TestData\sampleData.txt";

            Configuration config = Utility.ParseConfigData(filePath);

            // Console.WriteLine(config.ToString());

            
        }
    }
}
