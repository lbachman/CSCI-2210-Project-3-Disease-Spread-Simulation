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

        public string Name { get; set; }

        public ICollection<Person> People { get; set; } = new List<Person>();

        public ICollection<Location> Neighbors { get; set; } = new List<Location>();

        public Random Rand { get;  } = new Random();

        public double Popularity { get; set; } = 1.0;


        // properties that are passed in from config object

        public double SpreadChance { get; set; }

        public double KillChance { get; set; }


        // constructor takes config object
        public Location(Configuration config, string name)
        {
            Name = name;
            SpreadChance = config.SpreadChance; 
            KillChance = config.KillChance;
        }

        public void MutateDisease(Configuration config)
        {
            if (Rand.NextDouble() < config.MutationChance) // Mutation chance
            {
                SpreadChance += Rand.NextDouble() * config.SpreadMutationRate - (config.SpreadMutationRate / 2);
                KillChance += Rand.NextDouble() * config.KillMutationRate - (config.KillMutationRate / 2);

                // Clamp values between 0 and 100
                SpreadChance = Math.Max(0, Math.Min(SpreadChance, 100));
                KillChance = Math.Max(0, Math.Min(KillChance, 100));

                Console.WriteLine($"Location {Id} mutated: SpreadChance = {SpreadChance}, KillChance = {KillChance}");
            }
        }


        
        public void SpreadDisease(double spread)
        {
            // added a max number of contacts for people, because realistically not every infected person interacts with every not infected person :)

            var infectedPeople = People.Where(p => p.IsInfected && !p.IsDead && !p.IsQuarantined).ToList();
            var susceptiblePeople = People.Where(p => !p.IsInfected && !p.IsDead && !p.IsQuarantined).ToList();

            foreach (var person in People)
            {
                int contactsPerHour = 3;
                var randomContacts = susceptiblePeople.OrderBy(_ => Rand.Next()).Take(contactsPerHour).ToList();

                //added newly infected people, so they dont spread the disease in the current hour simulated
                var newlyInfected = new List<Person>();

                //log the state of each person below
                //Console.WriteLine($"Checking person {person.Id} - Infected: {person.IsInfected}, Dead: {person.IsDead}, Quarantined: {person.IsQuarantined}");


                if (!person.IsInfected && !person.IsDead && !person.IsQuarantined)
                {
                    foreach (var infected in infectedPeople)
                    {
                        if (Rand.NextDouble() < spread / 100.0)
                        {
                            person.IsInfected = true;
                            person.InfectionCount++; //increment the infection count for the newly infected person
                            infected.InfectionSpreadCount++; //track how many others the infected person has spread to
                            newlyInfected.Add(person);
                            susceptiblePeople.Remove(person);
                            break;
                        }
                    }
                }
            }

            foreach (var person in infectedPeople)
            {
                if (Rand.NextDouble() < person.QuarantineChance) // Check quarantine probability
                {
                    person.IsQuarantined = true;
                    Console.WriteLine($"Person {person.Id} has been quarantined.");
                }
            }


            foreach (var person in People.Where(p => p.IsInfected && !p.IsQuarantined && !p.IsDead))
            {
                if (Rand.NextDouble() < person.QuarantineChance)
                {
                    person.IsQuarantined = true;
                }
            }

            //handle deaths for infected people
            foreach (var person in People.Where(p => p.IsInfected && !p.IsDead))
            {
                if (Rand.NextDouble() < KillChance / 100.0)
                {
                    person.Die();
                    Console.WriteLine($"Person {person.Id} has died.");
                }
            }
        }
    }
}
