using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PoissonDiskLogic
{
    internal static class Vector2Extensions
    {
        internal static float DistanceTo(this Vector2 a, Vector2 b)
        {
            return (float)Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }
        internal static Vector2 WrapAround(this Vector2 a, Vector2 b)
        {
            return new Vector2(a.X % b.X, a.Y % b.Y);
        }
        internal static Vector2 WrapAround(this Vector2 a, int x, int y)
        {
            return new Vector2(a.X % x, a.Y % y);
        }
    }
}
