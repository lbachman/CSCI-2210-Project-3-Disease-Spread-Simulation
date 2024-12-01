using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationLib
{
    public class Person
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Location CurrentLocation { get; set; }  // Track the current location of the person

        public int TravelStartTime { get; set; }

        public int TravelEndTime { get; set;}

        public bool IsInfected { get; set; }

        public int InfectionCount { get; set; }

        public int InfectionSpreadCount { get; set; }

        public bool IsDead { get; set; }

        public bool IsQuarantined { get; set; }

        public double QuarantineChance = 0.5;

        private static Random rand =  new Random();


        public Person(Configuration config)
        {
            CurrentLocation = new Location(config);
        }


        public void Travel(double travelChance, List<(Person person, Location destination)> travelQueue)
        {
            if (rand.NextDouble() < travelChance)
            {
                //  if CurrentLocation or Neighbors is null or empty
                if (CurrentLocation == null || CurrentLocation.Neighbors == null || !CurrentLocation.Neighbors.Any())
                {
                    Console.WriteLine($"Person {Id} cannot travel, no valid neighbors.");
                    return;
                }

                Location destination = CurrentLocation.Neighbors.ToList()[rand.Next(CurrentLocation.Neighbors.Count)];

                // Log the travel action
                Console.WriteLine($"Person {Id} traveling from {CurrentLocation.Id} to {destination.Id}");

                travelQueue.Add((this, destination));
            }
        }




        public void Infect()
        {
            if (!IsDead && !IsQuarantined)
            {
                IsInfected = true;
                InfectionSpreadCount++;
                
                Console.WriteLine($"Person {Id} is now infected.");  // log for debugging
            }
        }

        public void Die()
        {
            IsDead = true;
            IsInfected = false;
            IsQuarantined = false;
        }

        public void SpreadInfection()
        {
            if (IsInfected && !IsDead && !IsQuarantined)
            {
                InfectionSpreadCount++;
                Console.WriteLine($"Person {Id} is spreading infection. Spread count: {InfectionSpreadCount}"); // Log the infection spread
            }
        }

        public void PossibleQuarantine()
        {
            if (IsInfected && !IsQuarantined)
            {
                IsQuarantined = rand.NextDouble() < QuarantineChance;
            }
        }
    }
}
