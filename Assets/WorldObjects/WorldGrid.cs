using UnityEngine;
using Random = System.Random;

namespace Assets.WorldObjects
{
    // is reported to by every object which moves
    // allows gameobjects to either calculate or precalculate their nearest neighbors
    // all logic for nearest neighbor searches is stored here, searches are performed using parameters
    public class WorldGrid
    {
        public Random _myRandom { get; set; }
        public double _xDim { get; set; }
        public double _yDim { get; set; }
        public bool isStarted { get; set; }
        public double MoveSpeed { get; set; }

        public WorldGrid(double xDim = 18, double yDim = 10) // wraps at yMax
        {
            _xDim = xDim;
            _yDim = yDim;
            MoveSpeed = 0.05;
            _myRandom = new Random();
        }

        public void AddAgent(WorldAgent newAgent)
        {
            // newAgent.transform.position = GetNewLocation();
            newAgent.transform.position = GetCenterLocation();
            newAgent.transform.eulerAngles = GetNewAngle();
            newAgent.GetComponent<SpriteRenderer>().color = GetNewColor();
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
    }
}
