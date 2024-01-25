using MTSim.Objects.Abstraction;
using System.Collections.Generic;

namespace MTSim.Map
{
    public sealed class Location
    {
        private readonly Dictionary<long, GameObject> _objects = new();
    }
}
