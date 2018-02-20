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

        void Start()
        {
            _myModules = new ModuleContainer(); // will be used in the future
            _myAgentFactory = _myModules._agentFactory;
            _myWorldGrid = _myModules._worldGrid;
            for (int i = 0; i < 100; i++)
            {
                _myAgentFactory.CreateAgent(_myWorldGrid);
            }
            _myWorldGrid.isStarted = true;
        }

        void Update()
        {
            
        }
    }
}
