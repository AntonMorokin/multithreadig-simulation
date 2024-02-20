using System;
using System.Collections.Generic;
using MTSim.Objects.Abstraction;
using MTSim.Objects.Abstraction.Utils;
using MTSim.Objects.Animals;
using MTSim.Objects.Plants;

namespace MTSim.Objects.Behaviors;

public class ObjectsStandardBehavior : IObjectsBehavior
{
    public void Act(Animal animal)
    {
        SafeExecutor exec;

        while (!SafeExecutor.TryToCapture(animal, out exec))
        {
        }

        using (exec)
        {
            ActInternal(animal);
        }
    }

    protected virtual void ActInternal(Animal animal)
    {
        Live(animal);
        animal.DecreaseSatiety();
    }

    protected virtual void Live(Animal animal)
    {
        const double ReproductionPossibility = 0.5d;

        var island = animal.Island;

        if (animal.IsHungry)
        {
            if (island.AnyOfExcept(animal.Coords, animal.CanBeEaten, animal))
            {
                // It's hungry and here is something to eat
                Move(animal);
                return;
            }
        }
        else if (Random.Shared.NextDouble() < ReproductionPossibility)
        {
            if (island.AnyOfExcept(animal.Coords, animal))
            {
                // It wants to reproduce and here is someone to do so with
                Reproduce(animal);
                return;
            }
        }

        // Nothing to do. Leave location
        Move(animal);
    }

    protected virtual GameObject? Eat(Animal animal)
    {
        return animal.Eat();
    }

    protected virtual IReadOnlyCollection<Animal> Reproduce(Animal animal)
    {
        return animal.Reproduce();
    }

    protected virtual bool Move(Animal animal)
    {
        return animal.Move();
    }

    public void Act(Plant plant)
    {
        SafeExecutor exec;

        while (!SafeExecutor.TryToCapture(plant, out exec))
        {
        }

        using (exec)
        {
            ActInternal(plant);
        }
    }

    protected virtual void ActInternal(Plant plant)
    {
        Live(plant);
    }

    protected virtual void Live(Plant plant)
    {
        Grow(plant);
    }

    protected virtual void Grow(Plant plant)
    {
        plant.Grow();
    }
}