namespace EnKdev.Eidolon.DungeonSpawner;

public static class RandomExtensions
{
    public static double NextDouble(this Random random, double minVal, double maxVal)
    {
        return random.NextDouble() * (maxVal - minVal) + minVal;
    }
}