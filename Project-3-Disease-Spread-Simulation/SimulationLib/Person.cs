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

        public Location? CurrentLocation { get; set; }  // Track the current location of the person

        public int TravelStartTime { get; set; }

        public int TravelEndTime { get; set;}

        public bool IsInfected { get; set; }

        public int InfectionCount { get; set; }

        public int InfectionSpreadCount { get; set; }

        public bool IsDead { get; set; }

        public bool IsQuarantined { get; set; }

        public double QuarantineChance = 0.5;

        private static Random rand =  new Random();


        public Person(Configuration config, Location? currentLocation)
        {
            CurrentLocation = currentLocation;
            QuarantineChance = Math.Max(0, Math.Min(1, RandomNormal(
            config.MeanChanceQuarantine / 100.0,
            config.StdDevChanceQuarantine / 100.0
            )));
        }


        public void Travel(double travelChance, List<(Person person, Location destination)> travelQueue)
        {
            if (rand.NextDouble() < travelChance)
            {
                if (CurrentLocation == null || CurrentLocation.Neighbors == null || !CurrentLocation.Neighbors.Any())
                {
                    Console.WriteLine($"Person {Id} cannot travel, no valid neighbors.");
                    return;
                }

                var weightedNeighbors = CurrentLocation.Neighbors
                    .Select(location => new { Location = location, Weight = location.Popularity })
                    .ToList();

                if (weightedNeighbors.All(n => n.Weight <= 0))
                {
                    Console.WriteLine($"Person {Id} cannot travel, all neighbors have zero or negative popularity.");
                    return;
                }

                double totalWeight = weightedNeighbors.Sum(n => n.Weight);
                double randomWeight = rand.NextDouble() * totalWeight;

                Location destination = null;
                foreach (var neighbor in weightedNeighbors)
                {
                    if (randomWeight < neighbor.Weight)
                    {
                        destination = neighbor.Location;
                        break;
                    }
                    randomWeight -= neighbor.Weight;
                }

                if (destination == null && weightedNeighbors.Any())
                {
                    // Fallback to random neighbor
                    destination = weightedNeighbors.OrderBy(_ => rand.Next()).FirstOrDefault()?.Location;
                    Console.WriteLine($"Fallback: Person {Id} travels to random neighbor {destination?.Name}");
                }

                if (destination != null)
                {
                    travelQueue.Add((this, destination));
                    Console.WriteLine($"Person {Id} travels from {CurrentLocation.Name} to {destination.Name}.");
                }
                else
                {
                    Console.WriteLine($"Person {Id} could not determine a valid destination.");
                }
            }
        }





        public void Infect()
        {
            if (!IsDead && !IsQuarantined && !IsInfected)
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

        private double RandomNormal(double mean, double stddev)
        {
            Random rand = new Random();
            double u1 = rand.NextDouble();
            double u2 = rand.NextDouble();
            double z0 = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Cos(2.0 * Math.PI * u2);
            return mean + z0 * stddev;
        }
    }
}
