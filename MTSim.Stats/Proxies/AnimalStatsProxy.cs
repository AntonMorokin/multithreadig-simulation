using MTSim.Map;
using MTSim.Objects.Animals;

namespace MTSim.Stats.Proxies
{
    public sealed class AnimalStatsProxy : Animal
    {
        private readonly StatsCollector _collector;
        private readonly Animal _target;

        #region Property overrides

        public override string TypeName => _target.TypeName;

        public override double Weight => _target.Weight;

        public override int MaxSpeed => _target.MaxSpeed;

        public override IReadOnlyDictionary<string, double> WhatCanBeEaten => _target.WhatCanBeEaten;

        public override double MaxSatiety => _target.MaxSatiety;

        public override double CurrentSatiety
        {
            get => _target.CurrentSatiety;
            set
            {
                if (_target is null)
                {
                    return;
                }

                _target.CurrentSatiety = value;
            }
        }

        public override double SatietyDecreaseSpeed => _target.SatietyDecreaseSpeed;

        public override Island Island => _target.Island;

        public override Point Coords
        {
            get => _target.Coords;
            set
            {
                if (_target is null)
                {
                    return;
                }

                _target.Coords = value;
            }
        }

        public override bool IsDead => _target.IsDead;

        public override bool IsHungry => _target.IsHungry;

        #endregion

        public AnimalStatsProxy(StatsCollector collector, Animal target)
            : base(target.Id, target.Island, target.Coords, target.MaxSpeed, target.MaxSatiety, target.Weight, target.WhatCanBeEaten)
        {
            _collector = collector;
            _target = target;

            Coords = _target.Coords;
            CurrentSatiety = _target.CurrentSatiety;
        }

        #region Method overrides

        public override bool TryToCapture()
        {
            return _target.TryToCapture();
        }

        public override void SetFree()
        {
            _target.SetFree();
        }

        #endregion

        public override void Act()
        {
            _collector.AddTotalInCycle(_target);

            _target.Act();

            if (_target.IsDead)
            {
                _collector.AddDeadInCycle(_target);
            }
        }

        public override void Move()
        {
            var previous = _target.Coords;
            _target.Move();
            var now = _target.Coords;

            if (previous != now)
            {
                _collector.AddMovedInCycle(_target);
            }
        }

        public override IReadOnlyCollection<Animal> BornNewAnimals(Animal partner)
        {
            var result = _target.BornNewAnimals(partner);

            _collector.AddBornInCycle(result);

            return result;
        }

        public override double BeEaten()
        {
            var result = _target.BeEaten();

            _collector.AddEatenInCycle(_target);

            return result;
        }
    }
}
