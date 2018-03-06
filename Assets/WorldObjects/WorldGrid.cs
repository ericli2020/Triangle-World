using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace Assets.WorldObjects
{
    // DIFFERENT WORLD GRID AND WORLD AGENT FOR EACH TYPE
    // is reported to by every object which moves
    // allows gameobjects to either calculate or precalculate their nearest neighbors
    // all logic for nearest neighbor searches is stored here, searches are performed using parameters
    public class WorldGrid
        // eventually make this an object in the world?
    {
        public string Type { get; set; }
        public Random RandomGen { get; set; }
        public int XDim { get; set; }
        public int YDim { get; set; }
        private int GridX { get; set; }
        private int GridY { get; set; }
        public bool Started { get; set; }
        public double MoveSpeed { get; set; }
        public float MaxRotate { get; set; }
        public double SearchRadius { get; set; }
        private List<WorldAgent>[,] _agentGrid;
        private Dictionary<string, Pair<int, int>> _coordsOfAgent;

        public WorldGrid(int xDim = 18, int yDim = 10, double moveSpeed = 0.05, float maxRotate = 2, double searchRadius = 1) // wraps at yMax
        {
            SearchRadius = searchRadius;
            XDim = xDim;
            YDim = yDim;
            MoveSpeed = moveSpeed;
            MaxRotate = maxRotate;
            GridX = (int) Math.Ceiling(XDim / SearchRadius);
            GridY = (int) Math.Ceiling(YDim / SearchRadius);

            RandomGen = new Random();
            _agentGrid = new List<WorldAgent>[GridX, GridY];
            for (int i = 0; i < _agentGrid.GetLength(0); i++)
            {
                for (int j = 0; j < _agentGrid.GetLength(1); j++)
                {
                    _agentGrid[i, j] = new List<WorldAgent>();
                }
            }
            _coordsOfAgent = new Dictionary<string, Pair<int, int>>();
        }

        public void AddAgent(WorldAgent newAgent)
        {
            newAgent.transform.position = GetNewLocation();
            // newAgent.transform.position = GetCenterLocation();
            newAgent.transform.eulerAngles = GetNewAngle();
            newAgent.GetComponent<SpriteRenderer>().color = GetNewColor();
            
            int newX = (int)Math.Floor(newAgent.transform.position.x / SearchRadius);
            int newY = (int)Math.Floor(newAgent.transform.position.y / SearchRadius);
            _coordsOfAgent[newAgent.Id] = new Pair<int, int>(newX, newY);
            _agentGrid[newX, newY].Add(newAgent);
        }

        private Vector3 GetNewLocation()
        {
            double xScale = RandomGen.NextDouble();
            double yScale = RandomGen.NextDouble();
            double xCoord = xScale * XDim;
            double yCoord = yScale * YDim;
            return new Vector3((float)xCoord, (float)yCoord);
        }

        private Vector3 GetCenterLocation()
        {
            return new Vector3((float) XDim / 2, (float) YDim / 2);
        }

        private Vector3 GetNewAngle()
        {
            double angleScale = RandomGen.NextDouble();
            double rotateAngle = 360 * angleScale;
            return new Vector3(0, 0, (float)rotateAngle);
        }

        private Color GetNewColor()
        {
            double rVal = RandomGen.NextDouble();
            double gVal = RandomGen.NextDouble();
            double bVal = RandomGen.NextDouble();
            return new Color((float) rVal, (float) gVal, (float) bVal);
        }

        public void reportNewLocation(WorldAgent newAgent)
        {
            int newX = (int) Math.Floor(newAgent.transform.position.x / SearchRadius);
            int newY = (int) Math.Floor(newAgent.transform.position.y / SearchRadius);
            Pair<int, int> currPos = _coordsOfAgent[newAgent.Id];
            _coordsOfAgent[newAgent.Id] = new Pair<int, int>(newX, newY);
            _agentGrid[currPos.First, currPos.Second].Remove(newAgent);
            _agentGrid[newX, newY].Add(newAgent);
        }

        public Vector3 GetNextHeading(WorldAgent newAgent)
        {
            Vector3 heading = newAgent.transform.eulerAngles;
            float actualX = newAgent.transform.position.x;
            float actualY = newAgent.transform.position.y;
            int newX = (int)Math.Floor(actualX / SearchRadius);
            int newY = (int)Math.Floor(actualY / SearchRadius);
            double currClosestDistance = double.PositiveInfinity;
            Vector3 targetDirection = Vector3.negativeInfinity;

            Pair<double, Vector3> currPair = SearchGrid(newAgent, newX, newY, currClosestDistance, targetDirection);
            currClosestDistance = currPair.First;
            targetDirection = currPair.Second;

            if (newX != 0)
            {
                if (newY != 0 && (Math.Sqrt((actualX - newX * SearchRadius) * (actualX - newX * SearchRadius) + (actualY - newY * SearchRadius) * (actualY - newY * SearchRadius)) < currClosestDistance))
                {
                    currPair = SearchGrid(newAgent, newX - 1, newY - 1, currClosestDistance, targetDirection);
                    currClosestDistance = currPair.First;
                    targetDirection = currPair.Second;
                }
                if (newY != (YDim - 1) && (Math.Sqrt((actualX - newX * SearchRadius) * (actualX - newX * SearchRadius) + ((newY + 1) * SearchRadius - actualY) * ((newY + 1) * SearchRadius - actualY)) <
                    currClosestDistance))
                {
                    currPair = SearchGrid(newAgent, newX - 1, newY + 1, currClosestDistance, targetDirection);
                    currClosestDistance = currPair.First;
                    targetDirection = currPair.Second;
                }
                if (actualX - newX * SearchRadius < currClosestDistance)
                {
                    currPair = SearchGrid(newAgent, newX - 1, newY, currClosestDistance, targetDirection);
                    currClosestDistance = currPair.First;
                    targetDirection = currPair.Second;
                }
            }
                if (newY != 0 && (actualY - newY * SearchRadius < currClosestDistance))
                {
                    currPair = SearchGrid(newAgent, newX, newY - 1, currClosestDistance, targetDirection);
                    currClosestDistance = currPair.First;
                    targetDirection = currPair.Second;
                }
                if (newY != (YDim - 1) && ((newY + 1) * SearchRadius - actualY < currClosestDistance))
                {
                    currPair = SearchGrid(newAgent, newX, newY + 1, currClosestDistance, targetDirection);
                    currClosestDistance = currPair.First;
                    targetDirection = currPair.Second;
                }
            if (newX != (XDim - 1))
            {
                if (newY != 0 && (Math.Sqrt(((newX + 1) * SearchRadius - actualX) * ((newX + 1) * SearchRadius - actualX) + (actualY - newY * SearchRadius) * (actualY - newY * SearchRadius)) < currClosestDistance))
                {
                    currPair = SearchGrid(newAgent, newX + 1, newY - 1, currClosestDistance, targetDirection);
                    currClosestDistance = currPair.First;
                    targetDirection = currPair.Second;
                }
                if (newY != (YDim - 1) && (Math.Sqrt(((newX + 1) * SearchRadius - actualX) * ((newX + 1) * SearchRadius - actualX) + ((newY + 1) * SearchRadius - actualY) * ((newY + 1) * SearchRadius - actualY)) <
                                      currClosestDistance))
                {
                    currPair = SearchGrid(newAgent, newX + 1, newY + 1, currClosestDistance, targetDirection);
                    currClosestDistance = currPair.First;
                    targetDirection = currPair.Second;
                }
                if ((newX + 1) * SearchRadius - actualX < currClosestDistance)
                {
                    currPair = SearchGrid(newAgent, newX + 1, newY, currClosestDistance, targetDirection);
                    currClosestDistance = currPair.First;
                    targetDirection = currPair.Second;
                }
            }

            if (Double.IsPositiveInfinity(currClosestDistance))
            {
                double newZ = heading.z + Math.PI * MaxRotate * (RandomGen.NextDouble() - 0.5);
                heading = new Vector3(0, 0, (float) newZ);
            }
            else
            {
                Vector3 targetDir = targetDirection - newAgent.transform.position;
                Vector3 defaultPointer = new Vector3(1, 0, 0);
                // double endAngle = Math.Atan(targetDir.y / targetDir.x) / Math.PI * 180;
                float endAngle = Vector3.Angle(defaultPointer, targetDir);
                
                Vector3 crossProduct = Vector3.Cross(defaultPointer, targetDir);
                if (crossProduct.z < 0)
                {
                    endAngle = 360 - endAngle;
                    // problem is this doesn't find the next closest neighbor to follow
                }

                double angleToNew = endAngle - heading.z;

                if (angleToNew > 180)
                {
                    angleToNew = -(360 - angleToNew);
                }
                
                if (angleToNew > MaxRotate)
                {
                    angleToNew = MaxRotate;
                } else if (angleToNew < -MaxRotate)
                {
                    angleToNew = -MaxRotate;
                }
                
                return new Vector3(0, 0, heading.z + (float) angleToNew);
            }
            
            return heading;
        }
        
        public Pair<Double, Vector3> SearchGrid(WorldAgent newAgent, int newX, int newY, double currClosestDistance, Vector3 targetDirection)
        {
            foreach (WorldAgent nearestNeighbor in _agentGrid[newX, newY])
            {
                double newDistance = newAgent.FindDistance(nearestNeighbor);
                if (newDistance < currClosestDistance)
                {
                    currClosestDistance = newDistance;
                    targetDirection = nearestNeighbor.transform.position;
                }
            }

            return new Pair<Double, Vector3>(currClosestDistance, targetDirection);
        }
    }

    public class Pair<T1, T2>
    {
        public T1 First { get; set; }
        public T2 Second { get; set; }
        public Pair(T1 first, T2 second)
        {
            First = first;
            Second = second;
        }
    }
}
