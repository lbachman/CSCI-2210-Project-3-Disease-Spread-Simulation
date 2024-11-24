using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationLib
{
    internal class Configuration
    {
        public string ParseFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;

                    while ((line = reader.ReadLine()) != null) 
                    {
                        Console.WriteLine(line);
                    }
                }
            }
            else
            {
                Console.WriteLine($"File not found.");
            }   
        }
    }
}
