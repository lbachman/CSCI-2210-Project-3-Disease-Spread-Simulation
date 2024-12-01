using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationLib
{

    /// <summary>
    /// class implements the graph data structure, 
    /// the Locations are the nodes and the Neighbors is an adjacency list.
    /// People are the data contained in the nodes. 
    /// </summary>
    public class Location
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public ICollection<Person> People { get; set; } = new List<Person>();

        public ICollection<Location> Neighbors { get; set; } = new List<Location>();

        public Random Rand { get;  } = new Random();


        // properties that are passed in from config object

        public double SpreadChance { get; set; }

        public double KillChance { get; set; }


        // constructor takes config object
        public Location(Configuration config)
        {
            SpreadChance = config.SpreadChance; 
            KillChance = config.KillChance;
            
        }


        // okay yeah this is probably bad code, but it might work!
        public void SpreadDisease(double spread)
        {
            // log number of people in location and how many are infected. 
            Console.WriteLine($"Starting disease spread for location with {People.Count} people. Infected: {People.Count(p => p.IsInfected)}");

            foreach (var person in People)
            {

                // Log the state of each person
                //Console.WriteLine($"Checking person {person.Id} - Infected: {person.IsInfected}, Dead: {person.IsDead}, Quarantined: {person.IsQuarantined}");

                          
                if (!person.IsInfected && !person.IsDead && !person.IsQuarantined)
                {
                    foreach (var infected in People)
                    {
                        if (infected.IsInfected)
                        {
                            // Log the chance of infection and whether it was successful
                            double infectionChance = Rand.NextDouble();
                            Console.WriteLine($"Person {person.Id} checking infection chance against infected person {infected.Id}: {infectionChance} < {spread} = {(infectionChance < spread ? "Infected!" : "Not infected.")}");

                            if (infectionChance < spread)
                            {
                                person.Infect();
                                infected.SpreadInfection();
                                Console.WriteLine($"Person {person.Id} got infected by person {infected.Id}");
                                break; // Exit the inner loop once a person has been infected
                            }

                            
                        }
                    }
                }
            }
        }





    }
}
