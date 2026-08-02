using System;
using UnityEngine;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Inventory;
using Random = System.Random;

namespace AtomicWar._Game.AI
{
    /// <summary>
    /// Evaluation context passed to Utility AI actions containing survivor needs,
    /// shelter state, inventory contents, and environmental world state.
    /// </summary>
    public class AIContext
    {
        public Survivor Survivor;
        public Shelter.Shelter Shelter;
        public Inventory.Inventory Inventory;
        public bool IsFalloutStorm;
        public float AmbientRadRate;
        public bool IsRadiationRising;
        /// <summary>True when the survivor currently has the Listless status (light deprivation).</summary>
        public bool IsListless;
        /// <summary>True when the shelter's grow-light module is running; relevant to morale-seeking actions.</summary>
        public bool GrowLightActive;
        public Random Random;

        public AIContext() { }

        public AIContext(Survivor survivor, Shelter.Shelter shelter = null, Inventory.Inventory inventory = null, Random random = null)
        {
            Survivor = survivor;
            Shelter = shelter;
            Inventory = inventory;
            Random = random;
        }
    }
}
