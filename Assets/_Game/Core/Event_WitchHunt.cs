using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class WitchHuntState
    {
        public string eventId = "event_witch_hunt";
        public string accusedId;
        public List<string> accuserIds = new List<string>();
        public int badLuckStreak;
        public bool strikeActive;
        public int daysUntilStrike;
        public bool playerBanished;
        public bool huntActive;
        public bool huntResolved;
        public List<string> badLuckLog = new List<string>();
    }

    /// <summary>
    /// Prompt #842: Witch Hunts — Bad luck streak causes crew to blame one
    /// survivor (usually Mutant/Outsider). They demand banishment or the
    /// crew goes on strike.
    /// </summary>
    public class Event_WitchHunt
    {
        private WitchHuntState _state = new WitchHuntState();

        private const int BadLuckThreshold = 5;
        private const int DecisionWindowDays = 3;
        private const int StrikeDurationDays = 5;

        public event Action<string, int> OnBadLuckTracked;         // eventType, streak
        public event Action<string, string[]> OnHuntStarted;       // accusedId, accusers
        public event Action<string> OnBanishmentDemanded;          // accusedId
        public event Action<string> OnPlayerBanished;              // accusedId
        public event Action<string[]> OnStrikeStarted;             // strikers
        public event Action OnStrikeEnded;

        public WitchHuntState CaptureState() => _state;

        public void RestoreState(WitchHuntState state)
        {
            _state = state ?? new WitchHuntState();
            if (_state.accuserIds == null)
                _state.accuserIds = new List<string>();
            if (_state.badLuckLog == null)
                _state.badLuckLog = new List<string>();
        }

        /// <summary>
        /// Records a bad luck event. Call after each negative event in the bunker.
        /// </summary>
        public void TrackBadLuck(string eventType)
        {
            _state.badLuckLog.Add(eventType);
            _state.badLuckStreak++;

            OnBadLuckTracked?.Invoke(eventType, _state.badLuckStreak);
        }

        /// <summary>
        /// Checks whether conditions are met for a witch hunt.
        /// Returns the accused ID (lowest affinity or mutant trait) or null.
        /// </summary>
        public string CheckForHunt(List<string> survivorIds, Func<string, bool> isMutant,
            Func<string, float> getAverageAffinity)
        {
            if (_state.badLuckStreak < BadLuckThreshold) return null;
            if (_state.huntActive || _state.huntResolved) return null;

            // Find accused: prefer mutant, then lowest affinity
            string accused = null;
            float lowestAffinity = float.MaxValue;

            foreach (var id in survivorIds)
            {
                if (isMutant(id))
                {
                    accused = id;
                    break;
                }

                float aff = getAverageAffinity(id);
                if (aff < lowestAffinity)
                {
                    lowestAffinity = aff;
                    accused = id;
                }
            }

            return accused;
        }

        /// <summary>
        /// Begins the witch hunt against the accused survivor.
        /// All survivors except the accused become accusers.
        /// </summary>
        public void StartHunt(string accusedId, List<string> accuserIds)
        {
            _state.accusedId = accusedId;
            _state.accuserIds = new List<string>(accuserIds);
            _state.huntActive = true;
            _state.daysUntilStrike = DecisionWindowDays;
            _state.playerBanished = false;

            OnHuntStarted?.Invoke(accusedId, accuserIds.ToArray());
            OnBanishmentDemanded?.Invoke(accusedId);
        }

        /// <summary>
        /// Player responds to the banishment demand.
        /// If banish = true, the accused is exiled. If false, accusers strike.
        /// </summary>
        public void PlayerRespond(bool banish)
        {
            if (!_state.huntActive) return;

            _state.huntResolved = true;
            _state.huntActive = false;

            if (banish)
            {
                _state.playerBanished = true;
                OnPlayerBanished?.Invoke(_state.accusedId);
            }
            else
            {
                // Refuse to banish — accusers go on strike
                _state.strikeActive = true;
                _state.daysUntilStrike = StrikeDurationDays;
                OnStrikeStarted?.Invoke(_state.accuserIds.ToArray());
            }
        }

        /// <summary>
        /// Advances the hunt/strike by one day.
        /// </summary>
        public void TickDay()
        {
            if (_state.huntActive)
            {
                _state.daysUntilStrike--;
                if (_state.daysUntilStrike <= 0)
                {
                    // Player didn't decide in time — auto-strike
                    PlayerRespond(false);
                }
            }
            else if (_state.strikeActive)
            {
                _state.daysUntilStrike--;
                if (_state.daysUntilStrike <= 0)
                {
                    _state.strikeActive = false;
                    OnStrikeEnded?.Invoke();
                }
            }
        }

        /// <summary>
        /// Returns true if the accuser strike is currently active.
        /// </summary>
        public bool IsStrikeActive() => _state.strikeActive;

        /// <summary>
        /// Resets the bad luck streak (call on streak break or new period).
        /// </summary>
        public void ResetStreak()
        {
            _state.badLuckStreak = 0;
            _state.badLuckLog.Clear();
        }
    }
}
