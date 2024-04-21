using Newtonsoft.Json;

namespace EnKdev.Eidolon.DungeonSpawner.Models;

public class DataModel
{
    [JsonProperty("regions")]
    public List<RegionModel> Regions { get; set; }
    
    [JsonProperty("totalDungeons")]
    public int TotalDungeons { get; set; }
    
    [JsonProperty("worldGrade")]
    public string WorldGrade { get; set; }
}