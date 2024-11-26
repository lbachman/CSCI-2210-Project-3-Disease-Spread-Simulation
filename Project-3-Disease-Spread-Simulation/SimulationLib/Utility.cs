using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationLib
{
    public class Utility
    {
        public static Configuration ParseConfigData(string filePath)
        {
            if (File.Exists(filePath))
            {
                var config = new Configuration();

                // convert .txt file into string and slice where there is a line break
                string[] lines = File.ReadAllText(filePath).Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var line in lines)
                {
                    // skip comments and empty lines
                    if (line.StartsWith("#") || string.IsNullOrWhiteSpace(line)) continue;

                    var parts = line.Split(':');

                    // if line of text is not formatted a key: value format 
                    if (parts.Length != 2) throw new FormatException($"Invalid line in config file: {line}");

                    string key = parts[0].Trim();
                    string value = parts[1].Trim();
                    switch (key)
                    {
                        case "Mean Population Size":
                            config.MeanPopSize = int.Parse(value); 
                            break;

                        case "Standard Deviation of Population Size":
                            config.PopStdDev = Convert.ToInt32(value);
                            break;

                        case "Percent Chance of Disease Spread":
                            config.SpreadChance = double.Parse(value);
                            break;

                        case "Percent Chance of Death":
                            config.KillChance = double.Parse(value);
                            break;

                        case "Disease Duration (hours)":
                            config.DiseaseDuration = int.Parse(value);
                            break;

                        case "Quarantine Duration (hours)":
                            config.QuarantineDuration = int.Parse(value);
                            break;

                        case "Mean Quarantine Chance (%)":
                            config.MeanChanceQuarantine = double.Parse(value);
                            break;

                        case "Standard Deviation of Quarantine Chance":
                            config.StdDevChanceQuarantine = double.Parse(value);
                            break;

                        case "Simulation Duration (hours)":
                            config.SimulationDuration = int.Parse(value);
                            break;

                        case "Percent Chance of Travel Per Hour":
                            config.TravelChance = double.Parse(value);
                            break;

                        default:
                            throw new FormatException($"Unknown configuration key: {key}");
                    }


                }
                return config;
            }
            else
            {
                throw new Exception("File does not exist");
            }
        }


    }
}
