﻿// See https://aka.ms/new-console-template for more information

using EnKdev.Eidolon.DungeonSpawner.Models;
using Newtonsoft.Json;

namespace EnKdev.Eidolon.DungeonSpawner;

public class Program
{
    private const string FilePathOldData = "./data/dataOld.json";
    private const string FilePathNewData = "./data/data.json";
    
    static Random rand = new Random(); 
    static int totalDungeons = 0;
    
    private static void Main(string[] args)
    {
        var oldData = ReadOldDataFile(FilePathOldData);

        var regionList = oldData.Item1.ToList();
        var worldGrade = oldData.Item2;
        totalDungeons = oldData.Item3;

        if (regionList[0].Region == "No regions found.")
        {
            Console.WriteLine("No regions found.");
            Console.ReadKey();
            Environment.Exit(-1);
        }
        
        foreach (var region in regionList)
        {
            Console.WriteLine($"Region: {region.Region}");
            Console.WriteLine($"Dungeons: {region.Dungeons}");
            Console.WriteLine($"Grade: {region.Grade}");
            Console.WriteLine($"Spawn Chance: {region.SpawnChance}");
            Console.WriteLine();

            if (rand.Next(0, 101) <= 50)
            {
                RemoveDungeonsFromRegion(region);
            }
            else
            {
                AddDungeonsToRegion(region);
            }
            
            region.Grade = DetermineGrade(region.Dungeons);

            Console.WriteLine("New data:");
            Console.WriteLine($"Region: {region.Region}");
            Console.WriteLine($"Dungeons: {region.Dungeons}");
            Console.WriteLine($"Grade: {region.Grade}");
            Console.WriteLine($"Spawn Chance: {region.SpawnChance}");
            Console.WriteLine();
        }

        worldGrade = DetermineWorldGrade(totalDungeons);

        var newData = new DataModel
        {
            Regions = regionList,
            TotalDungeons = totalDungeons,
            WorldGrade = worldGrade
        };

        var jsonData = JsonConvert.SerializeObject(newData, Formatting.Indented);
        File.WriteAllText(FilePathNewData, jsonData);
        File.Delete(FilePathOldData);
        File.Copy(FilePathNewData, FilePathOldData);

        Console.WriteLine("New data written!");
        Console.ReadKey();
        Environment.Exit(1);
    }

    private static (IEnumerable<RegionModel>, string, int) ReadOldDataFile(string filePath)
    {
        var fileContent = File.ReadAllText(filePath);
        var json = JsonConvert.DeserializeObject<DataModel>(fileContent);

        if (json is null)
        {
            return new ValueTuple<IEnumerable<RegionModel>, string, int>
            {
                Item1 = new List<RegionModel>
                {
                    new()
                    {
                        Region = "No regions found."
                    }
                },
                Item2 = "-",
                Item3 = 0
            };
        }

        var list = json.Regions.ToList();
        var grade = json.WorldGrade;
        var t = json.TotalDungeons;
        
        return (list, grade, t);
    }

    private static string DetermineGrade(int dungeons)
    {
        return dungeons switch
        {
            >= 450 => "SS",
            >= 371 and < 450 => "S+++",
            >= 266 and <= 370 => "S++",
            >= 226 and <= 265 => "S+",
            >= 176 and <= 225 => "S",
            >= 151 and <= 175 => "A+",
            >= 126 and <= 150 => "A",
            >= 76 and <= 125 => "B",
            >= 51 and <= 75 => "C",
            _ => "D"
        };
    }

    private static string DetermineWorldGrade(int totalDungeons)
    {
        return totalDungeons switch
        {
            >= 2500 => "SS",
            >= 2250 and < 2500 => "S+++",
            >= 2000 and <= 2499 => "S++",
            >= 1750 and <= 1999 => "S+",
            >= 1500 and <= 1749 => "S",
            >= 1250 and <= 1499 => "A+",
            >= 1000 and <= 1249 => "A",
            >= 750 and <= 999 => "B",
            >= 500 and <= 749 => "C",
            _ => "D"
        };
    }

    private static void AddDungeonsToRegion(RegionModel region)
    {
        if (!(rand.NextDouble(1.00, 100.00) < region.SpawnChance))
        {
            Console.WriteLine("Spawn check failed!");
            Console.WriteLine();
            return;
        }

        Console.WriteLine("Spawn check succeeded.");
            
        var newDungeons = Convert.ToInt32(region.Dungeons / 100.00 * region.BaseModifier);
        region.Dungeons += newDungeons;
        totalDungeons += newDungeons;
    }

    private static void RemoveDungeonsFromRegion(RegionModel region)
    {
        if (region.Dungeons <= 15)
        {
            Console.WriteLine("Threshold hit, skipping this region...");
            return;
        }
        
        var lostDungeons = Convert.ToInt32(region.Dungeons / 100.00 * region.BaseModifier);
        region.Dungeons -= lostDungeons;
        totalDungeons -= lostDungeons;

        Console.WriteLine("Removed dungeons!");
    }
}