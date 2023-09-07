using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoissonDiskLogic
{
    public class Grid
    {
        public Vector2[] InternalPoints; //list of all points
        private bool[,] definedPoints; //checks if each cell is defined
        public int ResolutionX { get; private set; }
        public int ResolutionY { get; private set; }
        public float CellSize { get; private set; }
        public float Radius { get; private set; }
        public int CellsX { get { return definedPoints.GetLength(0); } }
        public int CellsY { get { return definedPoints.Length/ definedPoints.GetLength(0); } }
        public Vector2? this[float x, float y] //X and Y are positions, not cells
        {
            get
            {
                return GetPointByCoordinate(x, y);
            }
            set
            {
                int cellX = lengthToGrid(x, CellsX);
                int cellY = lengthToGrid(y, CellsY);
                if (!value.HasValue)
                    throw new ArgumentNullException("Position cannot be null");
                var pos = value.Value;
                pos.Wrap(ResolutionX, ResolutionY);
                InternalPoints[cellToIndex(cellX, cellY)] = pos;
                definedPoints[cellX, cellY] = true;
            }
        }
        public void Add(Vector2 pos)
        {
            this[pos.X, pos.Y] = pos;
        }
        public Grid(int width, int height, int radius)
        {
            if (radius == 0)
                throw new ArgumentException("Radius must be greater than 0");
            if (width == 0 || height == 0)
                throw new ArgumentException("Dimensions must be greater than 0");
            ResolutionX = width;
            ResolutionY = height;
            Radius = radius;
            CellSize = radius / MathF.Sqrt(2);
            int cellX = (int)MathF.Ceiling(ResolutionX/ CellSize);
            int cellY = (int)MathF.Ceiling(ResolutionY/ CellSize);
            definedPoints = new bool[cellX, cellY];
            InternalPoints = new Vector2[cellX * cellY];
        }
        private int lengthToGrid(float n, float max)
        {
            return (int)MathF.Floor((n / CellSize+ max) % max);
        }
        private int cellToIndex(int x, int y)
        {
            return y * CellsX + x;
        }
        public Vector2 GetPointByIndex(int i) => InternalPoints[i];
        public Vector2 GetPointByCoordinate(Vector2 pos)
        {
            return GetPointByCoordinate(pos.X,pos.Y);
        }
        public Vector2 GetPointByCoordinate(float x, float y)
        {
            int x_cell = lengthToGrid(x, CellsX);
            int y_cell = lengthToGrid(y, CellsY);
            int index = cellToIndex(x_cell, y_cell);
            return GetPointByIndex(index);
        }
        public Vector2 GetPointByCell(int x, int y)
        {
            return GetPointByIndex(cellToIndex(x,y));
        }
        public bool IsCellOccupied(int x, int y)
        {
            return definedPoints[x, y];
        }
        public bool IsPositionOccupied(int x, int y)
        {
            int xCell = lengthToGrid(x, CellsX);
            int yCell = lengthToGrid(y, CellsY);
            return definedPoints[xCell, yCell];
        }
        /// <summary>
        /// Returns true if there are no neighbours within the radius, false if otherwise. Note: only scans 2 cells around and wraps;
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool IsValidSample(Vector2 pos, float radius)
        {
            if (pos.X < 0 | pos.X > ResolutionX | pos.Y < 0 | pos.Y > ResolutionY)
                return false;
            int xPos = lengthToGrid(pos.X, CellsX);
            int yPos = lengthToGrid(pos.Y, CellsY);
            if (definedPoints[xPos, yPos])
                return false;
            for (int x = -2; x <= 2; x++)
            {
                for (int y = -2; y <= 2; y++)
                {
                    int gridX = (x + xPos + CellsX) % CellsX;
                    int gridY = (y + yPos + CellsY) % CellsY;

                    if (IsCellOccupied(gridX, gridY))
                    {
                        Vector2 target = GetPointByIndex(cellToIndex(gridX, gridY));
                        //tilable wrapping
                        float targetx = target.X;
                        float targety = target.Y;
                        if (xPos + x > CellsX)
                            targetx += ResolutionX;
                        if (yPos + y > CellsY)
                            targety += ResolutionY;
                        if (xPos + x < 0)
                            targetx -= ResolutionX;
                        if (yPos + y < 0)
                            targety -= ResolutionY;

                        float distance = pos.Distance(targetx, targety);
                        if (distance < radius)
                            return false;
                        if (distance > radius*CellSize)
                            Console.WriteLine("Error");
                    }
                }
            }
            return true;
        }
        public ValueTuple<float, Vector2> NearestDistance(float x, float y, int search)
        {
            int xPos = lengthToGrid(x, CellsX);
            int yPos = lengthToGrid(y, CellsY);
            float distance = float.MaxValue;
            Vector2 point = InternalPoints[0];
            for (int xScan = -search; xScan <= search; xScan++)
            {
                for (int yScan = -search; yScan <= search; yScan++)
                {
                    int gridX = (xScan + xPos + CellsX) % CellsX;
                    int gridY = (yScan + yPos + CellsY) % CellsY;

                    if (IsCellOccupied(gridX, gridY))
                    {
                        Vector2 target = GetPointByCell(gridX, gridY);
                        ////tilable wrapping
                        float distanceX = MathF.Abs(target.X-x);
                        float distanceY = MathF.Abs(target.Y-y);
                        if (distanceX > ResolutionX / 2)
                            distanceX = distanceX - ResolutionX;
                        if (distanceY > ResolutionY / 2)
                            distanceY = distanceY - ResolutionY;

                        //x==(int)targetx && y == (int)targety ?100:0;
                        float newdistance = MathF.Sqrt(distanceX* distanceX + distanceY* distanceY);
                        if (newdistance < distance)
                        {
                            distance = newdistance;
                            point = target;
                        }
                    }
                }
            }
            return new ValueTuple<float, Vector2>(distance, point);
        }
        public float GetDistanceToEdge(float x, float y, int search)
        {
            int xPos = lengthToGrid(x, CellsX);
            int yPos = lengthToGrid(y, CellsY);

            Vector2 p1 = InternalPoints[0];
            Vector2 p2 = InternalPoints[1]; 
            double distance1 = float.MaxValue;
            double distance2 = float.MaxValue;
            for (int xScan = -search; xScan <= search; xScan++)
            {
                for (int yScan = -search; yScan <= search; yScan++)
                {
                    int gridX = (xScan + xPos + CellsX) % CellsX;
                    int gridY = (yScan + yPos + CellsY) % CellsY;


                    if (IsCellOccupied(gridX, gridY))
                    {
                        Vector2 target = GetPointByCell(gridX, gridY);
                        ////tilable wrapping
                        float distanceX = MathF.Abs(target.X - x);
                        float distanceY = MathF.Abs(target.Y - y);
                        if (distanceX > ResolutionX / 2)
                            distanceX = distanceX - ResolutionX;
                        if (distanceY > ResolutionY / 2)
                            distanceY = distanceY - ResolutionY;

                        //x==(int)targetx && y == (int)targety ?100:0;
                        float newdistance = MathF.Sqrt(distanceX * distanceX + distanceY * distanceY);
                        if (newdistance < distance1)
                        {
                            distance2 = distance1;
                            distance1 = newdistance;
                            p2 = p1;
                            p1 = target;
                        }
                        if (newdistance < distance2 & target != p1)
                        {
                            distance2 = newdistance;
                            p2 = target;
                        }
                    }
                }
            }

            //distance to bisector of both points

            if (x - p1.X < -ResolutionX / 2)
                p1.X -= ResolutionX - 1;
            if (x - p2.X < -ResolutionX / 2)
                p2.X -= ResolutionX - 1;

            if (y - p1.Y < -ResolutionY / 2)
                p1.Y -= ResolutionY - 1;
            if (y - p2.Y < -ResolutionY / 2)
                p2.Y -= ResolutionY - 1;

            if (x - p1.X > ResolutionX / 2)
                p1.X += ResolutionX - 1;
            if (x - p2.X > ResolutionX / 2)
                p2.X += ResolutionX - 1;

            if (y - p1.Y > ResolutionY / 2)
                p1.Y += ResolutionY - 1;
            if (y - p2.Y > ResolutionY / 2)
                p2.Y += ResolutionY - 1;

            Vector2 midpoint = (p1 + p2) / 2;
            float a = (p2.X - p1.X) / (p2.Y - p1.Y);
            float b = 1;
            float c = -a * midpoint.X - midpoint.Y;
            return MathF.Abs(a * x + b * y + c) / MathF.Sqrt(a * a + b * b);
        }
    }
}
