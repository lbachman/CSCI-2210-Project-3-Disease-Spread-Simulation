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
            var peopleInfected =  new List<Person>(People);

            foreach (var person in People)
            {
                if (!person.IsInfected && !person.IsDead && !person.IsQuarantined)
                {
                    foreach (var infected in peopleInfected)
                    {
                        if (infected.IsInfected && Rand.NextDouble() < spread)
                        {
                            person.Infect();
                            infected.SpreadInfection();
                            break;
                        }
                    }
                }
            }
        }





    }
}
