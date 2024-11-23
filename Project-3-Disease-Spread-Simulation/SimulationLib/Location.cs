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


    }
}
