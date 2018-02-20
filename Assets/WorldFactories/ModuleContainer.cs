using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.WorldControllers;
using Assets.WorldObjects;

namespace Assets.WorldFactories
{
    public class ModuleContainer
    {
        public TestAgentFactory _agentFactory { get; set; }
        public WorldGrid _worldGrid { get; set; }

        public ModuleContainer()
        {
            _agentFactory = new TestAgentFactory();
            _worldGrid = new WorldGrid();

        }
    }
}
