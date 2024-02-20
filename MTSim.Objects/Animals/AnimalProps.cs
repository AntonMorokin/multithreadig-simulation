using System.Collections.Generic;

namespace MTSim.Objects.Animals;

public class AnimalProps
{
    public readonly int MaxSpeed;
    public readonly double MaxSatiety;
    public readonly double Weight;
    public readonly IReadOnlyDictionary<string, double> Food;

    public AnimalProps(int maxSpeed, double maxSatiety, double weight, IReadOnlyDictionary<string, double> food)
    {
        MaxSpeed = maxSpeed;
        MaxSatiety = maxSatiety;
        Weight = weight;
        Food = food;
    }
}