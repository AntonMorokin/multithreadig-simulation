namespace MTSim.Objects.Plants;

public class PlantProps
{
    public readonly double GrowSpeed;
    public readonly double Weight;

    public PlantProps(double growSpeed, double weight)
    {
        GrowSpeed = growSpeed;
        Weight = weight;
    }
}