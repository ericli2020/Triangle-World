using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace Assets.WorldObjects
{
    // is reported to by every object which moves
    // allows gameobjects to either calculate or precalculate their nearest neighbors
    // all logic for nearest neighbor searches is stored here, searches are performed using parameters
    public class WorldGrid
        // eventually make this an object in the world?
    {
        public Random _myRandom { get; set; }
        public int _xDim { get; set; }
        public int _yDim { get; set; }
        public bool isStarted { get; set; }
        public double MoveSpeed { get; set; }
        public float _maxRotate { get; set; }
        private List<WorldAgent>[,] _agentGrid;
        public Dictionary<string, Pair<int, int>> coordsOfAgent;

        public WorldGrid(int xDim = 18, int yDim = 10, double moveSpeed = 0.05, float maxRotate = 10) // wraps at yMax
        {
            _xDim = xDim;
            _yDim = yDim;
            MoveSpeed = moveSpeed;
            _maxRotate = maxRotate;
            _myRandom = new Random();
            _agentGrid = new List<WorldAgent>[xDim, yDim];
            for (int i = 0; i < _agentGrid.GetLength(0); i++)
            {
                for (int j = 0; j < _agentGrid.GetLength(1); j++)
                {
                    _agentGrid[i, j] = new List<WorldAgent>();
                }
            }
            coordsOfAgent = new Dictionary<string, Pair<int, int>>();
        }

        public void AddAgent(WorldAgent newAgent)
        {
            newAgent.transform.position = GetNewLocation();
            // newAgent.transform.position = GetCenterLocation();
            newAgent.transform.eulerAngles = GetNewAngle();
            newAgent.GetComponent<SpriteRenderer>().color = GetNewColor();
            int newX = (int)Math.Floor(newAgent.transform.position.x);
            int newY = (int)Math.Floor(newAgent.transform.position.y);
            coordsOfAgent[newAgent.Id] = new Pair<int, int>(newX, newY);
            _agentGrid[newX, newY].Add(newAgent);
        }

        private Vector3 GetNewLocation()
        {
            double xScale = _myRandom.NextDouble();
            double yScale = _myRandom.NextDouble();
            double xCoord = xScale * _xDim;
            double yCoord = yScale * _yDim;
            return new Vector3((float)xCoord, (float)yCoord);
        }

        private Vector3 GetCenterLocation()
        {
            return new Vector3(9, 5);
        }

        private Vector3 GetNewAngle()
        {
            double angleScale = _myRandom.NextDouble();
            double rotateAngle = 360 * angleScale;
            return new Vector3(0, 0, (float)rotateAngle);
        }

        private Color GetNewColor()
        {
            double rVal = _myRandom.NextDouble();
            double gVal = _myRandom.NextDouble();
            double bVal = _myRandom.NextDouble();
            return new Color((float) rVal, (float) gVal, (float) bVal);
        }

        public void reportNewLocation(WorldAgent newAgent)
        {
            int newX = (int) Math.Floor(newAgent.transform.position.x);
            int newY = (int) Math.Floor(newAgent.transform.position.y);
            Pair<int, int> currPos = coordsOfAgent[newAgent.Id];
            coordsOfAgent[newAgent.Id] = new Pair<int, int>(newX, newY);
            _agentGrid[currPos.First, currPos.Second].Remove(newAgent);
            _agentGrid[newX, newY].Add(newAgent);
        }

        public Vector3 GetNextHeading(WorldAgent newAgent)
        {
            // eventually combine move and update location
            Vector3 heading = newAgent.transform.eulerAngles;
            float actualX = newAgent.transform.position.x;
            float actualY = newAgent.transform.position.y;
            int newX = (int)Math.Floor(actualX);
            int newY = (int)Math.Floor(actualY);
            double currClosestDistance = double.PositiveInfinity;
            Vector3 targetDirection = Vector3.negativeInfinity;

            // move this to a function
            Pair<double, Vector3> currPair = SearchGrid(newAgent, newX, newY, currClosestDistance, targetDirection);
            currClosestDistance = currPair.First;
            targetDirection = currPair.Second;

            if (newX != 0)
            {
                if (newY != 0 && (Math.Sqrt((actualX - newX) * (actualX - newX) + (actualY - newY) * (actualY - newY)) < currClosestDistance))
                {
                    currPair = SearchGrid(newAgent, newX - 1, newY - 1, currClosestDistance, targetDirection);
                    currClosestDistance = currPair.First;
                    targetDirection = currPair.Second;
                }
                if (newY != (_yDim - 1) && (Math.Sqrt((actualX - newX) * (actualX - newX) + (newY + 1 - actualY) * (newY + 1 - actualY)) <
                    currClosestDistance))
                {
                    currPair = SearchGrid(newAgent, newX - 1, newY + 1, currClosestDistance, targetDirection);
                    currClosestDistance = currPair.First;
                    targetDirection = currPair.Second;
                }
                if (actualX - newX < currClosestDistance)
                {
                    currPair = SearchGrid(newAgent, newX - 1, newY, currClosestDistance, targetDirection);
                    currClosestDistance = currPair.First;
                    targetDirection = currPair.Second;
                }
            }
                if (newY != 0 && ((actualY - newY) < currClosestDistance))
                {
                    currPair = SearchGrid(newAgent, newX, newY - 1, currClosestDistance, targetDirection);
                    currClosestDistance = currPair.First;
                    targetDirection = currPair.Second;
                }
                if (newY != (_yDim - 1) && ((newY + 1 - actualY) < currClosestDistance))
                {
                    currPair = SearchGrid(newAgent, newX, newY + 1, currClosestDistance, targetDirection);
                    currClosestDistance = currPair.First;
                    targetDirection = currPair.Second;
                }
            if (newX != (_xDim - 1))
            {
                if (newY != 0 && (Math.Sqrt((newX + 1 - actualX) * (newX + 1 - actualX) + (actualY - newY) * (actualY - newY)) < currClosestDistance))
                {
                    currPair = SearchGrid(newAgent, newX + 1, newY - 1, currClosestDistance, targetDirection);
                    currClosestDistance = currPair.First;
                    targetDirection = currPair.Second;
                }
                if (newY != (_yDim - 1) && (Math.Sqrt((newX + 1 - actualX) * (newX + 1 - actualX) + (newY + 1 - actualY) * (newY + 1 - actualY)) <
                                      currClosestDistance))
                {
                    currPair = SearchGrid(newAgent, newX + 1, newY + 1, currClosestDistance, targetDirection);
                    currClosestDistance = currPair.First;
                    targetDirection = currPair.Second;
                }
                if (newX + 1 - actualX < currClosestDistance)
                {
                    currPair = SearchGrid(newAgent, newX + 1, newY, currClosestDistance, targetDirection);
                    currClosestDistance = currPair.First;
                    targetDirection = currPair.Second;
                }
            }

            if (currClosestDistance == double.PositiveInfinity)
            {
                double newZ = heading.z + Math.PI * _maxRotate * (_myRandom.NextDouble() - 0.5);
                heading = new Vector3(0, 0, (float) newZ);
            }
            else
            {
                Vector3 targetDir = targetDirection - newAgent.transform.position;
                // calculate target euler angles
                // replace with arctan?
                Vector3 defaultPointer = new Vector3(1, 0, 0);
                float endAngle = Vector3.Angle(defaultPointer, targetDir);
                Vector3 crossProduct = Vector3.Cross(defaultPointer, targetDir);
                if (crossProduct.z < 0)
                {
                    endAngle = 360 - endAngle;
                }
                /*
                isStarted = false;
                Debug.Log(crossProduct);
                Debug.Log(targetDir);
                Debug.Log(endAngle);
                Debug.Log(heading.z);
                */
                float angleToNew = endAngle - heading.z;
                if (angleToNew > _maxRotate)
                {
                    angleToNew = _maxRotate;
                } else if (angleToNew < -_maxRotate)
                {
                    angleToNew = -_maxRotate;
                }
                
                return new Vector3(0, 0, heading.z + angleToNew);
            }
            
            return heading;

            // return random
        }
        
        public Pair<Double, Vector3> SearchGrid(WorldAgent newAgent, int newX, int newY, double currClosestDistance, Vector3 targetDirection)
        {
            foreach (WorldAgent nearestNeighbor in _agentGrid[newX, newY])
            {
                double newDistance = newAgent.findDistance(nearestNeighbor);
                if (newDistance < currClosestDistance)
                {
                    currClosestDistance = newDistance;
                    targetDirection = nearestNeighbor.transform.position;
                }
            }

            return new Pair<Double, Vector3>(currClosestDistance, targetDirection);
        }

        /*
        public double GetClosestDistance(float x, float y, int xArea, int yArea)
        {
            // change this to an overload
            int newX = (int)Math.Floor(x);
            int newY = (int)Math.Floor(y);
            if (newX == xArea)
            {
                if (y > yArea)
                {
                    return y - (yArea + 1);
                }
                else
                {
                    return yArea - y;
                }
            }
            else if (newY == yArea)
            {
                if (x > xArea)
                {
                    return x - (xArea + 1);
                }
                else
                {
                    return xArea - x;
                }
            }
            else
            {
                if (x > xArea)
                {
                    if (y > yArea)
                    {
                        return Math.Sqrt((x - (xArea + 1)) * (x - (xArea + 1)) + (y - (yArea + 1)) * (y - (yArea + 1)));
                    }
                    else
                    {
                        return Math.Sqrt((x - (xArea + 1)) * (x - (xArea + 1)) + (yArea - y) * (yArea - y));
                    }
                }
                else
                {
                    if (y > yArea)
                    {
                        return Math.Sqrt((xArea - x) * (xArea - x) + (y - (yArea + 1)) * (y - (yArea + 1)));
                    }
                    else
                    {
                        return Math.Sqrt((xArea - x) * (xArea - x) + (yArea - y) * (yArea - y));
                    }
                }
            }
        }
        */
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
