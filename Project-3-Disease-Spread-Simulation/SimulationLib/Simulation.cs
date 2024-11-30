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

        private string csvFilePath = "simulation_log.csv";




        public Simulation(Configuration config)
        {
            this.config = config;
            locations = new List<Location>();
        }

        // add new methods for csv logs
        public void InitCsv()
        {
            //overwrite if exists
            using (StreamWriter writer = new StreamWriter(csvFilePath, false))
            {
                writer.WriteLine("Hour,InfectedMost,SpreadMost,NotDead,Dead,Infected,Quarantined");
            }
        }

        // writes the fields to a csv file
        public void LogCsv(int hour, Person infectedMost, Person spreadMost, int notDead, int dead, int infected, int quaratined)
        {
            using (StreamWriter writer = new StreamWriter(csvFilePath, true))
            {
                writer.WriteLine($"{hour},{infectedMost?.Id},{spreadMost.Id},{notDead},{dead},{infected},{quaratined}");
            }
        }

        public bool CheckEndConditions(List<Location> locations)
        {
            int totalInfected = locations.Sum(location => location.People.Count(p => p.IsInfected));
            int totalAlive = locations.Sum(location => location.People.Count(p => !p.IsDead));

            return totalInfected == 0 || totalAlive == 0;
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

        // runs simulation, initializes CSV
        public void RunSimulation()
        {
            InitCsv();
            int hour = 0;

            while (hour < config.SimulationDuration)
            {
                foreach (var location in locations)
                {
                    location.SpreadDisease(config.SpreadChance);
                }

                var (infectedMost, spreadMost, notDead, dead, infected, quarantined) = GatherStatistics();

                LogCsv(hour, infectedMost, spreadMost, notDead, dead, infected, quarantined);

                if (CheckEndConditions(locations))
                {
                    Console.WriteLine("Simulation terminated...");
                    break;
                }

                hour++;
            }
        }

        // I threw in a method for getting the stats of it all
        public (Person InfectedMost, Person SpreadMost, int NotDead, int Dead, int Infected, int Quarantined) GatherStatistics()
        {
            Person mostInfected = null;
            Person mostSpread = null;

            int maxInfections = 0;
            int maxSpread = 0;

            int notDead = 0;
            int dead = 0;
            int infected = 0;
            int quarantined = 0;

            //again, there is probably a better way to write this. But, it's 3am
            foreach (var location in locations)
            {
                foreach (var person in location.People)
                {
                    if (person.IsDead) dead++;
                    else
                    {
                        notDead++;
                        if (person.IsInfected) infected++;
                        if (person.IsQuarantined) quarantined++;

                        if (person.InfectionCount > maxInfections)
                        {
                            maxInfections = person.InfectionCount;
                            mostInfected = person;
                        }

                        if (person.InfectionSpreadCount > maxSpread)
                        {
                            maxSpread = person.InfectionSpreadCount;
                            mostSpread = person;
                        }
                    }
                }
            }
            return (mostInfected, mostSpread, notDead, dead, infected, quarantined);
        }
    }
}
