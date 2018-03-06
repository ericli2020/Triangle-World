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
        public TestAgentFactory AgentFactory { get; set; }
        public WorldGrid WorldGrid { get; set; }

        public ModuleContainer()
        {
            AgentFactory = new TestAgentFactory();
            WorldGrid = new WorldGrid();

        }
    }
}
