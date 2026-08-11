using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Encounters
{
    [Serializable]
    public class SiegeArtilleryState
    {
        public string siegeId = "siege_artillery";
        public int turnsActive;
        public float ceilingLoadDamage;
        public List<string> modulesDamaged = new List<string>();
        public bool expeditionSent;
        public bool crewKilled;
        public int expeditionTurnsRemaining;
    }

    /// <summary>
    /// Prompt #819: Siege Artillery. Raiders shell the bunker from 3 nodes
    /// away. CeilingLoad drops, modules take random damage. Player MUST send
    /// an expedition to kill the artillery crew or the bunker collapses.
    /// Plain C#. Save/load safe.
    /// </summary>
    public class Siege_Artillery
    {
        private SiegeArtilleryState _state = new SiegeArtilleryState();

        private const float MinShellDamage = 5f;
        private const float MaxShellDamage = 15f;
        private const int ExpeditionTravelTurns = 3;

        // -- Events --
        public event Action OnShellingStarted;
        public event Action<string, float> OnModuleDamaged;       // (moduleId, damage)
        public event Action OnExpeditionSent;
        public event Action OnCrewKilled;
        public event Action OnBunkerCollapsing;

        public SiegeArtilleryState State => _state;

        /// <summary>Begins the artillery siege. Shells start falling.</summary>
        public void StartSiege()
        {
            _state.turnsActive = 0;
            _state.ceilingLoadDamage = 0f;
            _state.modulesDamaged.Clear();
            _state.expeditionSent = false;
            _state.crewKilled = false;
            _state.expeditionTurnsRemaining = 0;

            OnShellingStarted?.Invoke();
        }

        /// <summary>
        /// Advance one turn of shelling. Deals 5–15 ceiling damage and
        /// randomly damages a module. Returns the ceiling damage dealt
        /// this turn.
        /// </summary>
        /// <param name="ceilingLoad">Current ceiling load before this tick.</param>
        /// <param name="availableModuleIds">Module IDs that can be hit.</param>
        /// <param name="rng">Random source for damage rolls.</param>
        public float TickTurn(float ceilingLoad, IReadOnlyList<string> availableModuleIds, Random rng)
        {
            if (_state.crewKilled) return 0f;

            _state.turnsActive++;

            // Ceiling damage
            float shellDmg = (float)(rng.NextDouble() * (MaxShellDamage - MinShellDamage) + MinShellDamage);
            _state.ceilingLoadDamage += shellDmg;

            // Random module damage
            if (availableModuleIds != null && availableModuleIds.Count > 0)
            {
                int idx = rng.Next(availableModuleIds.Count);
                string hitModule = availableModuleIds[idx];
                float moduleDmg = (float)(rng.NextDouble() * (MaxShellDamage - MinShellDamage) + MinShellDamage);

                if (!_state.modulesDamaged.Contains(hitModule))
                    _state.modulesDamaged.Add(hitModule);

                OnModuleDamaged?.Invoke(hitModule, moduleDmg);
            }

            // Expedition travel
            if (_state.expeditionSent && !_state.crewKilled)
            {
                _state.expeditionTurnsRemaining--;
            }

            // Bunker collapse check
            float remainingLoad = ceilingLoad - _state.ceilingLoadDamage;
            if (remainingLoad <= 0f)
            {
                OnBunkerCollapsing?.Invoke();
            }

            return shellDmg;
        }

        /// <summary>
        /// Send an expedition to assault the artillery crew. Takes 3 turns
        /// to reach the position.
        /// </summary>
        public void SendExpedition()
        {
            if (_state.expeditionSent) return;

            _state.expeditionSent = true;
            _state.expeditionTurnsRemaining = ExpeditionTravelTurns;
            OnExpeditionSent?.Invoke();
        }

        /// <summary>
        /// Resolve the expedition's combat encounter once they arrive.
        /// </summary>
        /// <param name="combatSuccess">Whether the assault succeeded.</param>
        public void ResolveExpedition(bool combatSuccess)
        {
            if (!_state.expeditionSent || _state.crewKilled) return;
            if (_state.expeditionTurnsRemaining > 0) return;

            if (combatSuccess)
            {
                _state.crewKilled = true;
                OnCrewKilled?.Invoke();
            }
        }

        /// <summary>
        /// Returns the total damage dealt to a specific module during the siege.
        /// Caller tracks actual module HP externally.
        /// </summary>
        public float GetModuleDamage()
        {
            return _state.ceilingLoadDamage;
        }

        /// <summary>
        /// True when ceiling load has been reduced to zero by shelling.
        /// </summary>
        public bool IsBunkerCollapsing(float ceilingLoad)
        {
            return (ceilingLoad - _state.ceilingLoadDamage) <= 0f;
        }

        // -----------------------------------------------------------------
        // Save / Load
        // -----------------------------------------------------------------

        public SiegeArtilleryState CaptureState()
        {
            return new SiegeArtilleryState
            {
                siegeId = _state.siegeId,
                turnsActive = _state.turnsActive,
                ceilingLoadDamage = _state.ceilingLoadDamage,
                modulesDamaged = new List<string>(_state.modulesDamaged),
                expeditionSent = _state.expeditionSent,
                crewKilled = _state.crewKilled,
                expeditionTurnsRemaining = _state.expeditionTurnsRemaining
            };
        }

        public void RestoreState(SiegeArtilleryState saved)
        {
            _state = saved ?? new SiegeArtilleryState();
        }
    }
}
