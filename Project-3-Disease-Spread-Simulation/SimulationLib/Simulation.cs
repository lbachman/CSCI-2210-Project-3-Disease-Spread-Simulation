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

        private string csvFilePath = @"..\..\..\..\SimulationLib\SimulationResults\simulation_log.csv";


        /// <summary>
        /// parameterized constructor that takes config obj as argument
        /// </summary>
        /// <param name="config"></param>
        public Simulation(Configuration config)
        {
            this.config = config;
            locations = new List<Location>();
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

            // Define neighbors
            johnsonCity.Neighbors.Add(kingsport);
            johnsonCity.Neighbors.Add(elizabethton);
            kingsport.Neighbors.Add(johnsonCity);
            kingsport.Neighbors.Add(elizabethton);
            elizabethton.Neighbors.Add(johnsonCity);
            elizabethton.Neighbors.Add(kingsport);



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
                    var person = new Person(config)
                    {
                        QuarantineChance = config.MeanChanceQuarantine,          
                        CurrentLocation = location,

                        
                    };  

                    location.People.Add(person);
                }

                // log location info
                Console.WriteLine($"Generated population size for {location.Id}: {populationSize}");

            }

            // Infect a small percentage of the population
            InfectInitialPopulation(0.05); 

        }

        private void InfectInitialPopulation(double initialInfectionRate)
        {
            Random randy = new Random();

            foreach (var location in locations)
            {
                int infectedCount = (int)(location.People.Count * initialInfectionRate);

                var shuffledPeople = location.People.OrderBy(p => randy.Next()).ToList();

                for (int i = 0; i < infectedCount; i++)
                {
                    shuffledPeople[i].IsInfected = true;
                    Console.WriteLine($"Person {shuffledPeople[i].Id} in location {location.Id} was initially infected.");
                }
            }
        }



        /// <summary>
        /// helper method to generate a random number based on a normal distribution
        /// </summary>
        /// <param name="mean"></param>
        /// <param name="stddev"></param>
        /// <returns>double</returns>
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
                List<(Person person, Location destination)> travelQueue = new List<(Person person, Location destination)>();

                foreach (var location in locations)
                {
                    // spread disease at location
                    location.SpreadDisease(config.SpreadChance);

                    // iteratie over each person at the location
                    foreach (var person in location.People.ToList())
                    {
                        person.Travel(config.TravelChance / 100.0, travelQueue);
                    }
                }

                // Execute all travel actions after iteration
                foreach (var (person, destination) in travelQueue)
                {
                    person.CurrentLocation.People.Remove(person); // Remove from current location
                    destination.People.Add(person); // Add to destination
                    person.CurrentLocation = destination; // Update current location
                }

                // Clear the travel queue for the next cycle
                travelQueue.Clear();


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



        /// <summary>
        /// Initializes the CSV file for logging simulation data.
        /// </summary>
        public void InitCsv()
        {
            //overwrite if exists
            using (StreamWriter writer = new StreamWriter(csvFilePath, false))
            {
                writer.WriteLine("Hour,InfectedMost,SpreadMost,NotDead,Dead,Infected,Quarantined");
            }
        }



        /// <summary>
        /// writes the fields to a csv file
        /// </summary>
        /// <param name="hour"></param>
        /// <param name="infectedMost"></param>
        /// <param name="spreadMost"></param>
        /// <param name="notDead"></param>
        /// <param name="dead"></param>
        /// <param name="infected"></param>
        /// <param name="quaratined"></param>
        public void LogCsv(int hour, Person infectedMost, Person spreadMost, int notDead, int dead, int infected, int quaratined)
        {
            using (StreamWriter writer = new StreamWriter(csvFilePath, true))
            {
                writer.WriteLine($"{hour},{infectedMost?.Id},{spreadMost?.Id},{notDead},{dead},{infected},{quaratined}");
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
