using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationLib
{
    public class Configuration
    {
        public int MeanPopSize { get; set; }

        public double PopStdDev { get; set; }

        public double SpreadChance { get; set; }

        public double KillChance { get; set; }

        public int DiseaseDuration { get; set; }

        public int QuarantineDuration { get; set; }

        public double MeanChanceQuarantine { get; set; }

        public double StdDevChanceQuarantine { get; set; }

        public int SimulationDuration { get; set; }

        public double TravelChance { get; set; }

        public override string ToString()
        {
            return 
                   $"Mean Population Size: {MeanPopSize}\n" +
                   $"Population Standard Deviation: {PopStdDev}\n" +
                   $"Spread Chance: {SpreadChance}%\n" +
                   $"Kill Chance: {KillChance}%\n" +
                   $"Disease Duration: {DiseaseDuration} hours\n" +
                   $"Quarantine Duration: {QuarantineDuration} hours\n" +
                   $"Mean Quarantine Chance: {MeanChanceQuarantine}%\n" +
                   $"Quarantine Chance Std Dev: {StdDevChanceQuarantine}%\n" +
                   $"Simulation Duration: {SimulationDuration} hours\n" +
                   $"Travel Chance Per Hour: {TravelChance}%";
        }

    }
}
