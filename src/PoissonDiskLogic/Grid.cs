using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoissonDiskLogic
{
    public class Grid
    {
        private Vector2[] internalPoints;
        private bool[,] definedPoints;
        public int ResolutionX { get; }
        public int ResolutionY { get; }
        public float CellSize { get; }
        public int CellsX { get { return definedPoints.GetLength(0); } }
        public int CellsY { get { return definedPoints.Length/ definedPoints.GetLength(0); } }
        public Vector2? this[int x, int y]
        {
            get
            {
                return GetPointByCell(x, y);
            }
            set
            {
                int cellX = lengthToGrid((x+ResolutionX)% ResolutionX);
                int cellY = lengthToGrid((y+ResolutionY)%ResolutionY);
                if (!value.HasValue)
                    throw new ArgumentNullException("Position cannot be null");
                internalPoints[cellX + cellY + CellsY] = value.Value;
                definedPoints[x, y] = true;
            }
        }
        public Grid(int width, int height, int radius)
        {
            ResolutionX = width;
            ResolutionY = height;
            CellSize = radius/MathF.Sqrt(2);
            int cellX = (int)MathF.Ceiling(ResolutionX);
            int cellY = (int)MathF.Ceiling(ResolutionY);
            definedPoints = new bool[cellX, cellY];
            internalPoints = new Vector2[cellX * cellY];
        }
        private int lengthToGrid(float x)
        {
            return (int)MathF.Floor(x / CellSize);
        }
        private int cellPosToIndex(int x, int y)
        {
            return y * CellsX + x;
        }
        public Vector2 GetPointByIndex(int i) => internalPoints[i];
        public Vector2? GetPointByCoordinate(Vector2 pos)
        {
            int x = lengthToGrid(pos.X);
            int y = lengthToGrid(pos.Y);
            int index = cellPosToIndex(x, y);
            return GetPointByIndex(index);
        }
        public Vector2? GetPointByCell(int x, int y)
        {
            if (!definedPoints[x, y])
                return null;
            return GetPointByIndex(cellPosToIndex(x,y));
        }
        public bool IsCellOccupied(int x, int y)
        {
            return definedPoints[x, y];
        }
        public bool IsPositionOccupied(int x, int y)
        {
            int xCell = lengthToGrid(x);
            int yCell = lengthToGrid(y);
            return definedPoints[xCell, yCell];
        }
        /// <summary>
        /// Returns true if there are no neighbours within the radius, false if otherwise. Note: only scans 2 cells around and wraps;
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool IsValidSample(Vector2 pos, float radius)
        {
            int xPos = lengthToGrid(pos.X);
            int yPos = lengthToGrid(pos.Y);
            if (definedPoints[xPos, yPos])
                return false;
            for (int x = -2; x <= 2; x++)
            {
                for (int y = 0; y <= 2; y++)
                {
                    int gridX = (x + xPos + CellsX) % CellsX;
                    int gridY = (x + yPos + CellsY) % CellsY;

                    if (definedPoints[gridX, gridY])
                    {
                        Vector2 target = GetPointByCell(x, y).Value;
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
                    }
                }
            }
            return true;
        }
    }
}
