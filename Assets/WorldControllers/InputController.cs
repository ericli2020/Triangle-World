using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.WorldFactories;
using Assets.WorldObjects;
using UnityEngine;

namespace Assets.WorldControllers
{
    // holds all the universal variables which are used by every single object created in the world
    // this takes the place of what will eventually be the user interface
    public class InputController : MonoBehaviour
    {
        // triggers the factory and passes the new agents along
        // contains the global settings
        // knows the world controller
        
        // this should be referenced by the gridworld as well which will get its dimensions from here

        private ModuleContainer _myModules;
        private TestAgentFactory _myAgentFactory;
        private WorldGrid _myWorldGrid;
        private int ticksGone = 0;

        void Start()
        {
            _myModules = new ModuleContainer(); // will be used in the future
            _myAgentFactory = _myModules._agentFactory;
            _myWorldGrid = _myModules._worldGrid;
            for (int i = 0; i < 5; i++)
            {
                _myAgentFactory.CreateAgent(_myWorldGrid);
            }
            _myWorldGrid.isStarted = true;

            /*
            float actualTestX = (float)3.3;
            float actualTestY = (float)6.7;

            Debug.Log(_myWorldGrid.GetClosestDistance(3, 6, actualTestX, actualTestY, 2, 5));
            Debug.Log(_myWorldGrid.GetClosestDistance(3, 6, actualTestX, actualTestY, 2, 6));
            Debug.Log(_myWorldGrid.GetClosestDistance(3, 6, actualTestX, actualTestY, 2, 7));
            Debug.Log(_myWorldGrid.GetClosestDistance(3, 6, actualTestX, actualTestY, 3, 5));
            Debug.Log(_myWorldGrid.GetClosestDistance(3, 6, actualTestX, actualTestY, 3, 7));
            Debug.Log(_myWorldGrid.GetClosestDistance(3, 6, actualTestX, actualTestY, 4, 5));
            Debug.Log(_myWorldGrid.GetClosestDistance(3, 6, actualTestX, actualTestY, 4, 6));
            Debug.Log(_myWorldGrid.GetClosestDistance(3, 6, actualTestX, actualTestY, 4, 7));
            */
        }

        void Update()
        {
            /*
            if (ticksGone++ == 500)
            {
                _myWorldGrid.isStarted = false;
                Debug.Log("stopped");
                Dictionary<string, Pair<int, int>> coords = _myWorldGrid.coordsOfAgent;

                foreach (string name in coords.Keys)
                {
                    Pair<int, int> coord = coords[name];
                    Debug.Log(name + ": (" + coord.First + ", " + coord.Second + ")");
                }
            }
            */
            
        }
    }
}
