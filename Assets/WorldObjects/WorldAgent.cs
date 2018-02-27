using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.WorldControllers;
using UnityEngine;

namespace Assets.WorldObjects
{
    public class WorldAgent : MonoBehaviour, IEquatable<WorldAgent>
    {
        private WorldGrid _myWorld;
        public string Id;

        public void Initialize(WorldGrid newWorld, string Id)
        {
            _myWorld = newWorld;
            this.Id = Id;
            _myWorld.AddAgent(this);
        }

        void Start()
        {
            
        }

        void Update()
        {
            if (_myWorld.isStarted)
            {
                Vector3 heading = _myWorld.GetNextHeading(this);
                this.transform.eulerAngles = heading;
                double xForward = _myWorld.MoveSpeed * Math.Cos(heading.z * Math.PI / 180);
                double yForward = _myWorld.MoveSpeed * Math.Sin(heading.z * Math.PI / 180);
                double currY = (double) this.transform.position.y;
                double currX = (double) this.transform.position.x;
                double nextX = ((currX + xForward) % _myWorld._xDim + _myWorld._xDim) % _myWorld._xDim;
                double nextY = ((currY + yForward) % _myWorld._yDim + _myWorld._yDim) % _myWorld._yDim;
                this.transform.position = new Vector3((float) nextX, (float) nextY);

                _myWorld.reportNewLocation(this);
            }
        }

        public bool Equals(WorldAgent other)
        {
            if (other == null)
            {
                return false;
            }
            return Id == other.Id;
        }

        public double findDistance(WorldAgent other)
        {
            if (other.Id == this.Id)
            {
                return double.PositiveInfinity;
            }
            else
            {
                return Math.Sqrt((other.transform.position.x - this.transform.position.x) * (other.transform.position.x - this.transform.position.x) + (other.transform.position.y - this.transform.position.y) * (other.transform.position.y - this.transform.position.y));
            }
        }
    }
}
