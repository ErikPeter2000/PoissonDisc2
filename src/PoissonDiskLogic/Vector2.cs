namespace PoissonDiskLogic
{
    public struct Vector2
    {
        public float X { get; set; }
        public float Y { get; set; }
        public int ID { get; set; }
        public Vector2(float x, float y, int id)
        {
            X = x;
            Y = y;
            ID = id;
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
    }
}