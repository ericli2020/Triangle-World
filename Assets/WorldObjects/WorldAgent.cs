using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.WorldControllers;
using UnityEngine;

namespace Assets.WorldObjects
{
    public class WorldAgent : MonoBehaviour
    {
        private WorldGrid _myWorld;

        public void Initialize(WorldGrid newWorld)
        {
            _myWorld = newWorld;
            _myWorld.AddAgent(this);
        }

        void Start()
        {
            
        }

        void Update()
        {
            if (_myWorld.isStarted)
            {
                double heading = this.transform.eulerAngles.z;
                heading += Math.PI * 10 * (_myWorld._myRandom.NextDouble() - 0.5);
                this.transform.eulerAngles = new Vector3(0, 0, (float) heading);
                double xForward = _myWorld.MoveSpeed * Math.Cos(heading * Math.PI / 180);
                double yForward = _myWorld.MoveSpeed * Math.Sin(heading * Math.PI / 180);
                double currY = (double) this.transform.position.y;
                double currX = (double) this.transform.position.x;
                double nextX = ((currX + xForward) % _myWorld._xDim + _myWorld._xDim) % _myWorld._xDim;
                double nextY = ((currY + yForward) % _myWorld._yDim + _myWorld._yDim) % _myWorld._yDim;
                this.transform.position = new Vector3((float) nextX, (float) nextY);
            }
        }
    }
}
