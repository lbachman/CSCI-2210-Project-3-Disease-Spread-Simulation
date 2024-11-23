using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationLib
{
    internal class Person
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

    }
}
