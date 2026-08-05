using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class LightningStrikeState
    {
        public bool hasLightningRod = false;
        public float strikeChancePerStormHour = 0.10f;
        public List<string> destroyedModules = new List<string>();
    }

    /// <summary>
    /// Prompt #375: System: Static Lightning Strikes.
    /// Lightning during FalloutStorms can strike external modules (RadioAntenna, WindTurbine),
    /// destroying them and zeroing power load until repaired. Prevented by crafting a LightningRod.
    /// </summary>
    public class LightningStrikesSystem
    {
        private LightningStrikeState _state = new LightningStrikeState();

        public event Action<string> OnModuleDestroyedByLightning;
        public event Action OnLightningBlockedByRod;

        public LightningStrikeState State => _state;

        public bool CheckLightningStrike(bool isFalloutStormActive, bool hasLightningRod, System.Random rng, out string destroyedModule)
        {
            destroyedModule = null;
            if (!isFalloutStormActive) return false;

            if (hasLightningRod)
            {
                OnLightningBlockedByRod?.Invoke();
                return false;
            }

            if (rng.NextDouble() < _state.strikeChancePerStormHour)
            {
                destroyedModule = rng.NextDouble() < 0.5 ? "RadioAntenna" : "WindTurbine";
                _state.destroyedModules.Add(destroyedModule);
                OnModuleDestroyedByLightning?.Invoke(destroyedModule);
                return true;
            }

            return false;
        }
    }
}
