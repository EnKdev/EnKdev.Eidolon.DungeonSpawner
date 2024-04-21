using Newtonsoft.Json;

namespace EnKdev.Eidolon.DungeonSpawner.Models;

public class RegionModel
{
    [JsonProperty("region")]
    public string Region { get; set; }
    
    [JsonProperty("grade")]
    public string Grade { get; set; }
    
    [JsonProperty("dungeons")]
    public int Dungeons { get; set; }
    
    [JsonProperty("spawnChance")]
    public double SpawnChance { get; set; }
    
    [JsonProperty("baseModifier")]
    public double BaseModifier { get; set; }
}