using System;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class SiegeBiowarfareState
    {
        public string siegeId = "siege_biowarfare";
        public bool corpseOnVent;
        public float hoursSinceLaunch;
        public bool clearingSent;
        public bool cleared;
        public bool bunkerInfected;
        public string clearerSurvivorId = string.Empty;
    }

    /// <summary>
    /// Prompt #821: Biological Warfare. Raiders catapult plague-infected
    /// corpses into AirVents. If the player doesn't send someone in a
    /// HazmatSuit to the surface to clear it within 12 hours, the entire
    /// bunker contracts Phase 2 Affliction.
    /// Plain C#. Save/load safe.
    /// </summary>
    public class Siege_Biowarfare
    {
        private SiegeBiowarfareState _state = new SiegeBiowarfareState();

        private const float HoursUntilInfection = 12f;
        private const float HoursPerTick = 1f;

        // -- Events --
        public event Action OnCorpseLaunched;
        public event Action<float> OnHoursPassed;           // total hours elapsed
        public event Action<string> OnClearingSent;         // survivorId
        public event Action OnCorpseCleared;
        public event Action OnBunkerInfected;

        public SiegeBiowarfareState State => _state;

        /// <summary>
        /// Raiders launch a plague-infected corpse onto the air vents.
        /// The 12-hour countdown begins.
        /// </summary>
        public void LaunchCorpse()
        {
            _state.corpseOnVent = true;
            _state.hoursSinceLaunch = 0f;
            _state.clearingSent = false;
            _state.cleared = false;
            _state.bunkerInfected = false;
            _state.clearerSurvivorId = string.Empty;

            OnCorpseLaunched?.Invoke();
        }

        /// <summary>
        /// Advance one hour. If the 12-hour timer expires without clearing,
        /// the bunker becomes infected.
        /// </summary>
        public void TickHour()
        {
            if (!_state.corpseOnVent || _state.cleared || _state.bunkerInfected)
                return;

            _state.hoursSinceLaunch += HoursPerTick;
            OnHoursPassed?.Invoke(_state.hoursSinceLaunch);

            if (_state.hoursSinceLaunch >= HoursUntilInfection && !_state.cleared)
            {
                _state.bunkerInfected = true;
                OnBunkerInfected?.Invoke();
            }
        }

        /// <summary>
        /// Send a survivor to the surface to clear the corpse.
        /// If the survivor lacks a HazmatSuit, they become infected too.
        /// </summary>
        /// <param name="survivorId">The survivor sent to clear.</param>
        /// <param name="hasHazmat">Whether the survivor has a HazmatSuit.</param>
        /// <returns>
        /// True if the clearing survivor is safe. False if they lack HazmatSuit
        /// and will also become infected.
        /// </returns>
        public bool SendClearingTeam(string survivorId, bool hasHazmat)
        {
            if (!_state.corpseOnVent || _state.clearingSent || _state.cleared)
                return false;

            if (string.IsNullOrEmpty(survivorId))
                return false;

            _state.clearingSent = true;
            _state.clearerSurvivorId = survivorId;
            OnClearingSent?.Invoke(survivorId);

            if (!hasHazmat)
            {
                // Clearer is exposed — they will be infected alongside the bunker
                return false;
            }

            return true;
        }

        /// <summary>
        /// Resolve the clearing attempt. Must be called after SendClearingTeam.
        /// If the bunker has not yet been infected, the corpse is removed.
        /// </summary>
        public void ResolveClear()
        {
            if (!_state.clearingSent || _state.cleared) return;

            if (!_state.bunkerInfected)
            {
                _state.cleared = true;
                _state.corpseOnVent = false;
                OnCorpseCleared?.Invoke();
            }
        }

        /// <summary>True when the bunker has contracted the plague.</summary>
        public bool IsInfected()
        {
            return _state.bunkerInfected;
        }

        // -----------------------------------------------------------------
        // Save / Load
        // -----------------------------------------------------------------

        public SiegeBiowarfareState CaptureState()
        {
            return new SiegeBiowarfareState
            {
                siegeId = _state.siegeId,
                corpseOnVent = _state.corpseOnVent,
                hoursSinceLaunch = _state.hoursSinceLaunch,
                clearingSent = _state.clearingSent,
                cleared = _state.cleared,
                bunkerInfected = _state.bunkerInfected,
                clearerSurvivorId = _state.clearerSurvivorId
            };
        }

        public void RestoreState(SiegeBiowarfareState saved)
        {
            _state = saved ?? new SiegeBiowarfareState();
        }
    }
}
