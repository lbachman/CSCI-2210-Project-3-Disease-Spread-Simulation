using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationLib
{
    public class Simulation
    {
        private List<Location> locations;

        private Random randy = new Random();

        private Configuration config;




        public Simulation(Configuration config)
        {
            this.config = config;
            locations = new List<Location>();
        }



        public void GeneratePopulation()
        {
            // create locations
            Location johnsonCity = new Location(config);
            Location kingsport = new Location(config);
            Location elizabethton = new Location(config);

            // add locations to list
            locations.Add(johnsonCity);
            locations.Add(kingsport);
            locations.Add(elizabethton);

            foreach (Location location in locations) 
            {
                // Use normal distribution to generate a population size for each location
                int populationSize = (int)Math.Round(RandomNormal(config.MeanPopSize, config.PopStdDev));

                for (int i = 0; i < populationSize; i++)
                {
                    var person = new Person();  // here we could set properties
                    location.People.Add(person);
                }

            }

        }



        /// <summary>
        /// method for testing
        /// </summary>
        public void DisplayLocationInfo()
        {
            foreach (var location in locations)
            {
                Console.WriteLine(location.Id);
            }
        }




        /// <summary>
        /// helper method to generate a random number based on a normal distribution
        /// </summary>
        /// <param name="mean"></param>
        /// <param name="stddev"></param>
        /// <returns></returns>
        private double RandomNormal(double mean, double stddev)
        {
            // Box-Muller transform to generate normal distribution
            double u1 = randy.NextDouble();
            double u2 = randy.NextDouble();
            double z0 = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Cos(2.0 * Math.PI * u2);
            return mean + z0 * stddev;
        }




    }
}
