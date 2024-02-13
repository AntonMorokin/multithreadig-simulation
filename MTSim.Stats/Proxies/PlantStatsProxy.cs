using MTSim.Map;
using MTSim.Objects.Plants;

namespace MTSim.Stats.Proxies
{
    public sealed class PlantStatsProxy : Plant
    {
        private readonly StatsCollector _collector;
        private readonly Plant _target;

        #region Property overrides

        public override string TypeName => _target.TypeName;

        public override double Weight => _target.Weight;

        public override double GrowSpeed => _target.GrowSpeed;

        public override Island Island => _target.Island;

        public override Point Coords => _target.Coords;

        public override bool IsDead => _target.IsDead;

        #endregion

        public PlantStatsProxy(StatsCollector collector, Plant target)
            : base(target.Id, target.Island, target.Coords, target.GrowSpeed, target.Weight)
        {
            _collector = collector;
            _target = target;
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

        public override void Act()
        {
            _collector.AddTotalInCycle(_target);

            _target.Act();
        }

        #endregion

        public override void Grow()
        {
            _target.Grow();

            _collector.AddGrewInCycle(_target);
        }

        public override double BeEaten()
        {
            var result = _target.BeEaten();

            _collector.AddEatenInCycle(_target);

            return result;
        }
    }
}
