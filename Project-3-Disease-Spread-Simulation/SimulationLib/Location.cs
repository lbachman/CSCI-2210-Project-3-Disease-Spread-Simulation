using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationLib
{
    internal class Location
    {
        public string Id { get; set; }

        public ICollection<Person> People { get; set; }

        public ICollection<Location> Neighbors { get; set; }

        public Random rand = new Random();

        public Location(string id)
        {
            Id = id;
            People = new List<Person>();
            Neighbors = new List<Location>();
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
                        if (infected.IsInfected && rand.NextDouble() < spread)
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
