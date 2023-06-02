namespace PoissonDiskLogic
{
    public struct Vector2
    {
#if DEBUG
        public float DebugValue = 0;
#endif
        public float X { get; set; }
        public float Y { get; set; }
        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }
        public float Distance(Vector2 b)
        {
            return MathF.Sqrt(Distance2(b));
        }
        public float Distance(float x, float y)
        {
            return MathF.Sqrt(Distance2(x, y));
        }
        public float Distance2(Vector2 b)
        {
            var x = b.X - X;
            var y = b.Y - Y;
            return x * x + y * y;
        }
        public float Distance2(float x, float y)
        {
            var xOff = x - X;
            var yOff = y - Y;
            return xOff * xOff + yOff * yOff;
        }
        private const float epsilon = 0.0001f;
        public static Vector2 operator +(Vector2 a, Vector2 b) => new Vector2(a.X + b.X, a.Y + b.Y);
        public static Vector2 operator *(Vector2 a, float scalar) => new Vector2(a.X * scalar, a.Y * scalar);
        public static Vector2 operator /(Vector2 a, float scalar) => new Vector2(a.X / scalar, a.Y / scalar);
        public static bool operator ==(Vector2 a, Vector2 b)
        {
            return a.X > b.X - epsilon & a.X < b.X + epsilon & a.Y > b.Y - epsilon & a.Y < b.Y + epsilon;
        }
        public static bool operator !=(Vector2 a, Vector2 b)
        {
            return !(a == b);
        }
        public void Wrap(float width, float height)
        {
            X = (X + width) % width;
            Y = (Y + height) % height;
        }
        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }
}