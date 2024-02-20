using MTSim.Objects.Abstraction;
using MTSim.Objects.Animals;
using MTSim.Objects.Behaviors;
using MTSim.Objects.Plants;

namespace MTSim.Stats.Behaviors;

public class StatsCollectingObjectBehavior : ObjectsStandardBehavior
{
    private readonly StatsCollector _collector;

    public StatsCollectingObjectBehavior(StatsCollector collector)
    {
        _collector = collector;
    }

    protected override void ActInternal(Animal animal)
    {
        _collector.AddTotalInCycle(animal);

        base.ActInternal(animal);

        if (animal.IsDead)
        {
            _collector.AddDeadInCycle(animal);
        }
    }

    protected override GameObject? Eat(Animal animal)
    {
        var eaten = base.Eat(animal);

        if (eaten is not null)
        {
            _collector.AddEatenInCycle(eaten);
        }

        return eaten;
    }

    protected override IReadOnlyCollection<Animal> Reproduce(Animal animal)
    {
        var born = base.Reproduce(animal);

        _collector.AddBornInCycle(born);

        return born;
    }

    protected override bool Move(Animal animal)
    {
        var moved = base.Move(animal);

        if (moved)
        {
            _collector.AddMovedInCycle(animal);
        }

        return moved;
    }

    protected override void ActInternal(Plant plant)
    {
        _collector.AddTotalInCycle(plant);

        base.ActInternal(plant);
    }

    protected override void Grow(Plant plant)
    {
        base.Grow(plant);

        _collector.AddGrewInCycle(plant);
    }
}