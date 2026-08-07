using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class ScorchedEarthState
    {
        public string actionId = "action_scorched_earth";
        public bool isAvailable = false;
        public bool requiresRaidInProgress = true;
        public bool requiresNoAmmo = true;
        public bool killsAllInside = true;
        public bool destroysAllLoot = true;
    }

    /// <summary>
    /// Prompt #564: Action: Scorched Earth (Self Destruct).
    /// If a Raid is about to breach the Vault Door and the player has no ammo,
    /// trigger GeneratorOverload. Kills everyone inside but destroys all loot
    /// so Factions get nothing. A spiteful end.
    /// </summary>
    public class Action_ScorchedEarth
    {
        private ScorchedEarthState _state = new ScorchedEarthState();

        public event Action<ScorchedEarthState, int> OnScorchedEarthTriggered;
        public event Action<ScorchedEarthState> OnGeneratorOverloaded;

        public ScorchedEarthState State => _state;

        public bool CanTrigger(bool isRaidBreaching, int ammoCount)
        {
            return isRaidBreaching && ammoCount <= 0;
        }

        public void Execute(int survivorCount, Action<string> killSurvivor, Action clearLoot)
        {
            if (!CanTrigger(true, 0)) return;

            _state.isAvailable = false;

            // Kill all survivors inside
            for (int i = 0; i < survivorCount; i++)
            {
                killSurvivor?.Invoke("scorched_earth_generator_overload");
            }

            // Destroy all loot so factions get nothing
            clearLoot?.Invoke();

            OnScorchedEarthTriggered?.Invoke(_state, survivorCount);
            OnGeneratorOverloaded?.Invoke(_state);
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public ScorchedEarthState CaptureState() => _state;

        public void RestoreState(ScorchedEarthState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
