using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class SinkholeState
    {
        public string anomalyId = "map_anomaly_sinkhole";
        public string displayName = "The Sinkhole";
        public float ropeSnapDeathChance = 0.10f;
        public float caveInChance = 0.20f;
        public int foodLootCount = 20;
    }

    /// <summary>
    /// Prompt #456: Anomaly: The Sinkhole.
    /// Entire pre-war grocery store collapsed into the earth, holding massive food loot.
    /// Requires Rope to descend. 10% chance rope snaps (instant death), 20% chance of CaveIn.
    /// </summary>
    public class MapAnomaly_Sinkhole
    {
        private SinkholeState _state = new SinkholeState();

        public event Action<SinkholeState, string> OnRopeSnappedFatal;
        public event Action<SinkholeState, string> OnCaveInTriggered;

        public SinkholeState State => _state;

        public int LootSinkholeWithRope(string survivorId, bool hasRopeItem, System.Random rng, out bool isDead, out bool isCaveIn)
        {
            isDead = false;
            isCaveIn = false;

            if (!hasRopeItem) return 0;

            if (rng.NextDouble() < _state.ropeSnapDeathChance)
            {
                isDead = true;
                OnRopeSnappedFatal?.Invoke(_state, survivorId);
                return 0;
            }

            if (rng.NextDouble() < _state.caveInChance)
            {
                isCaveIn = true;
                OnCaveInTriggered?.Invoke(_state, survivorId);
            }

            return _state.foodLootCount;
        }
    }
}
