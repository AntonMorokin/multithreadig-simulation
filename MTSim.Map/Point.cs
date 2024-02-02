using System;
using System.Diagnostics.CodeAnalysis;

namespace MTSim.Map
{
    public readonly struct Point(int x, int y)
    {
        public readonly int X = x;
        public readonly int Y = y;

        public static bool operator==(Point left, Point right)
        {
            return left.X == right.X && left.Y == right.Y;
        }

        public static bool operator!=(Point left, Point right)
        {
            return !(left == right);
        }

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is not Point right)
            {
                return false;
            }

            return this == right;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }
}
