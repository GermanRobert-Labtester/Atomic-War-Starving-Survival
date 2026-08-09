// Victory_TrueEnding.cs — True Ending Victory Condition (Prompt #868)
// Only on highest difficulty. Day 30 Flashpoint wasn't war — it was terraforming
// by off-world entity. Hack terraformer, clear ash from sky.
// First blue sky in 100 days.
using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Serializable state for the True Ending victory (Prompt #868).
    /// Tracks prerequisites, hack progress, and completion.
    /// </summary>
    [Serializable]
    public class TrueEndingState
    {
        public string victory_id = "victory_true_ending";
        public bool is_unlocked;
        public string difficulty_required = "highest";
        public bool terraformer_hacked;
        public bool ash_cleared;
        public int blue_sky_day;
        public bool prerequisites_met;
    }

    /// <summary>
    /// True Ending victory (Prompt #868).
    /// Requires: highest difficulty, mainframe cracked (#798), nuclear silo found,
    /// 1000W power for 48 hours to hack terraformer.
    /// On completion: ash clears, blue sky, game ends with hope.
    /// </summary>
    public class Victory_TrueEnding
    {
        // ── Events ─────────────────────────────────────────────────────
        public event Action OnPrerequisitesMet;
        public event Action OnTerraformerDiscovered;
        public event Action OnHackStarted;
        public event Action<float> OnHackProgress;
        public event Action OnTerraformerHacked;
        public event Action<int> OnAshCleared;
        public event Action<int> OnBlueSky;

        // ── Constants ──────────────────────────────────────────────────
        private const int RequiredPowerWatts = 1000;
        private const int HackDurationHours = 48;

        // ── State ──────────────────────────────────────────────────────
        private TrueEndingState _state = new TrueEndingState();

        // Runtime tracking (not serialized — rebuilt on restore)
        private float _hackProgressPercent;
        private int _hackHoursRemaining;
        private int _powerAvailable;
        private bool _hackInProgress;
        private bool _terraformerDiscovered;

        // ── Public API ─────────────────────────────────────────────────

        /// <summary>
        /// Check all prerequisites for the true ending.
        /// Requires: highest difficulty, mainframe cracked, nuclear silo found,
        /// and terraformer discovered.
        /// </summary>
        public void CheckPrerequisites(string difficulty, bool mainframeHacked,
            bool nuclearSiloFound, bool terraformerDiscovered)
        {
            bool alreadyDiscovered = _terraformerDiscovered;
            _terraformerDiscovered = terraformerDiscovered;

            bool meetsDifficulty = (difficulty == "highest");
            bool allMet = meetsDifficulty && mainframeHacked &&
                          nuclearSiloFound && terraformerDiscovered;

            if (allMet && !_state.prerequisites_met)
            {
                _state.prerequisites_met = true;
                OnPrerequisitesMet?.Invoke();
            }

            // Latch the discovery like prerequisites_met above: CheckPrerequisites is
            // polled, so an unlatched invoke re-announces the find on every call.
            if (terraformerDiscovered && !alreadyDiscovered)
            {
                OnTerraformerDiscovered?.Invoke();
            }
        }

        /// <summary>
        /// Start the terraformer hack. Requires 1000W power for 48 hours.
        /// </summary>
        public void StartHack(int powerAvailable)
        {
            if (!_state.prerequisites_met || _hackInProgress)
                return;

            if (powerAvailable < RequiredPowerWatts)
                return;

            _powerAvailable = powerAvailable;
            _hackHoursRemaining = HackDurationHours;
            _hackProgressPercent = 0f;
            _hackInProgress = true;

            OnHackStarted?.Invoke();
        }

        /// <summary>
        /// Called every in-game hour during the hack.
        /// Advances hack progress. If power drops below 1000W, hack stalls.
        /// </summary>
        public void TickHour()
        {
            if (!_hackInProgress || _state.terraformer_hacked)
                return;

            if (_powerAvailable < RequiredPowerWatts)
                return; // Power insufficient — hack stalls

            _hackHoursRemaining--;
            _hackProgressPercent = 1f - ((float)_hackHoursRemaining / HackDurationHours);
            OnHackProgress?.Invoke(_hackProgressPercent);

            if (_hackHoursRemaining <= 0)
            {
                _hackInProgress = false;
                _state.terraformer_hacked = true;
                _state.is_unlocked = true;
                OnTerraformerHacked?.Invoke();

                // Begin ash clearing sequence
                ClearAsh();
            }
        }

        /// <summary>
        /// Returns true when the true ending has been fully completed.
        /// </summary>
        public bool IsComplete()
        {
            return _state.terraformer_hacked && _state.ash_cleared;
        }

        /// <summary>
        /// Returns the in-game day when blue sky was achieved, or -1.
        /// </summary>
        public int GetEndingDay()
        {
            return _state.ash_cleared ? _state.blue_sky_day : -1;
        }

        /// <summary>
        /// Set the current in-game day (called by the day tick system
        /// so the ending day can be recorded).
        /// </summary>
        public void SetCurrentDay(int day)
        {
            // Stored for use when ash clears
            _currentDay = day;
        }

        /// <summary>
        /// Update the available power supply (called each tick by power system).
        /// </summary>
        public void UpdatePower(int watts)
        {
            _powerAvailable = watts;
        }

        // ── Internals ──────────────────────────────────────────────────

        private int _currentDay;

        private void ClearAsh()
        {
            _state.ash_cleared = true;
            _state.blue_sky_day = _currentDay;
            OnAshCleared?.Invoke(_currentDay);
            OnBlueSky?.Invoke(_currentDay);
        }

        // ── Save / Load ────────────────────────────────────────────────

        public TrueEndingState CaptureState()
        {
            return new TrueEndingState
            {
                victory_id = string.IsNullOrEmpty(_state.victory_id) ? "victory_true_ending" : _state.victory_id,
                is_unlocked = _state.is_unlocked,
                difficulty_required = string.IsNullOrEmpty(_state.difficulty_required) ? "highest" : _state.difficulty_required,
                terraformer_hacked = _state.terraformer_hacked,
                ash_cleared = _state.ash_cleared,
                blue_sky_day = _state.blue_sky_day,
                prerequisites_met = _state.prerequisites_met
            };
        }

        public void RestoreState(TrueEndingState state)
        {
            if (state == null)
            {
                _state = new TrueEndingState();
                return;
            }
            _state = new TrueEndingState
            {
                victory_id = string.IsNullOrEmpty(state.victory_id) ? "victory_true_ending" : state.victory_id,
                is_unlocked = state.is_unlocked,
                difficulty_required = string.IsNullOrEmpty(state.difficulty_required) ? "highest" : state.difficulty_required,
                terraformer_hacked = state.terraformer_hacked,
                ash_cleared = state.ash_cleared,
                blue_sky_day = state.blue_sky_day,
                prerequisites_met = state.prerequisites_met
            };
            _hackInProgress = false;
            _hackProgressPercent = 0f;
        }
    }
}
