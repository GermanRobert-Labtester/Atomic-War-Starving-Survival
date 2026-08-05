using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class GlassCraterState
    {
        public string anomalyId = "map_anomaly_glass_crater";
        public string displayName = "The Glass Crater";
        public int lootCount = 0; // Zero loot
        public float radiationMillisieverts = 4000f;
        public float slipLacerationChance = 0.50f;
        public string lacerationAffliction = "razor_glass_laceration";
    }

    /// <summary>
    /// Prompt #449: Anomaly: The Glass Crater.
    /// Ground zero of a tactical nuke with zero loot and extreme radiation.
    /// Survivors navigating this node have a 50% chance to slip and suffer Lacerations on razor-sharp glass.
    /// </summary>
    public class MapAnomaly_GlassCrater
    {
        private GlassCraterState _state = new GlassCraterState();

        public event Action<GlassCraterState, string, string> OnGlassSlipLacerationContracted;

        public GlassCraterState State => _state;

        public bool NavigateCrater(string survivorId, System.Random rng, out string lacerationAffliction)
        {
            lacerationAffliction = null;
            if (rng.NextDouble() < _state.slipLacerationChance)
            {
                lacerationAffliction = _state.lacerationAffliction;
                OnGlassSlipLacerationContracted?.Invoke(_state, survivorId, lacerationAffliction);
                return true;
            }
            return false;
        }
    }
}
