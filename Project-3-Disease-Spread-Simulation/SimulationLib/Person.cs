using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationLib
{
    public class Person
    {
        public string Id { get; set; }

        public int TravelStartTime { get; set; }

        public int TravelEndTime { get; set;}

        public bool IsInfected { get; set; }

        public int InfectionCount { get; set; }

        public int InfectionSpreadCount { get; set; }

        public bool IsDead { get; set; }

        public bool IsQuarantined { get; set; }

        public double QuarantineChance { get; set; }

        public Random rand =  new Random();




        public Person()
        {
            
        }





        public void Infect()
        {
            if (!IsDead && !IsQuarantined)
            {
                InfectionSpreadCount++;
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
