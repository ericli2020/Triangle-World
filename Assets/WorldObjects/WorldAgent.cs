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
        public string Type { get; set; }
        public string Id { get; set; }

        public void Initialize(WorldGrid newWorld, string id)
        {
            _myWorld = newWorld;
            Id = id;
            _myWorld.AddAgent(this);
        }

        void Start()
        {
            
        }

        void Update()
        {
            if (_myWorld.Started)
            {
                Vector3 heading = _myWorld.GetNextHeading(this);
                this.transform.eulerAngles = heading;
                double xForward = _myWorld.MoveSpeed * Math.Cos(heading.z * Math.PI / 180);
                double yForward = _myWorld.MoveSpeed * Math.Sin(heading.z * Math.PI / 180);
                double currY = (double) this.transform.position.y;
                double currX = (double) this.transform.position.x;
                double nextX = ((currX + xForward) % _myWorld.XDim + _myWorld.XDim) % _myWorld.XDim;
                double nextY = ((currY + yForward) % _myWorld.YDim + _myWorld.YDim) % _myWorld.YDim;
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

        public double FindDistance(WorldAgent other)
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
