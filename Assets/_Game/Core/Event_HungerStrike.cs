using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class HungerStrikeState
    {
        public string eventId = "event_hunger_strike";
        public int evilChoiceThreshold = 3;
        public int strikeDurationDays;
        public int evilChoiceCount;
        public List<string> strikerIds = new List<string>();
        public List<int> strikerDaysWithoutFood = new List<int>();
    }

    /// <summary>
    /// Hunger strike event: when the player repeatedly makes cruel decisions,
    /// high-morale or empathic survivors refuse to eat in protest. If the
    /// player doesn't change behaviour, strikers starve — and after 7 days
    /// without food, they die.
    /// Plain C# class, not a MonoBehaviour.
    /// </summary>
    public class Event_HungerStrike
    {
        // ── Events ──────────────────────────────────────────────────────
        public event Action<string> OnHungerStrikeStarted;       // survivorId
        public event Action<string> OnHungerStrikeEnded;         // survivorId
        public event Action<string> OnSurvivorStarvedToDeath;    // survivorId

        // ── Config ──────────────────────────────────────────────────────
        private const int EvilChoiceThreshold = 3;
        private const int DeathAfterDays = 7;

        // Minimum morale to qualify as a striker (high-morale survivors protest)
        private const float MoraleThreshold = 0.6f;

        // ── State ───────────────────────────────────────────────────────
        private int _evilChoiceCount;
        private List<string> _strikerIds = new List<string>();
        private Dictionary<string, int> _daysWithoutFood = new Dictionary<string, int>();

        // ── Public API ──────────────────────────────────────────────────

        /// <summary>
        /// Called whenever the player makes a morally questionable ("evil")
        /// choice. Once the counter reaches the threshold, empathic and
        /// high-morale survivors may begin a hunger strike.
        /// </summary>
        public void TrackEvilChoice()
        {
            _evilChoiceCount++;
        }

        /// <summary>
        /// Evaluates whether any survivors qualify for a hunger strike.
        /// A survivor qualifies if:
        ///   - The evil-choice count has reached the threshold, AND
        ///   - They have high morale (>= 0.6) OR are flagged as empath.
        /// Survivors already on strike are not added again.
        /// </summary>
        public void CheckForStrike(List<(string id, float morale, bool empath)> survivors)
        {
            if (_evilChoiceCount < EvilChoiceThreshold) return;
            if (survivors == null) return;

            for (int i = 0; i < survivors.Count; i++)
            {
                var s = survivors[i];
                if (string.IsNullOrEmpty(s.id)) continue;
                if (_strikerIds.Contains(s.id)) continue;

                bool qualifies = s.empath || s.morale >= MoraleThreshold;
                if (!qualifies) continue;

                _strikerIds.Add(s.id);
                _daysWithoutFood[s.id] = 0;
                OnHungerStrikeStarted?.Invoke(s.id);
            }
        }

        /// <summary>
        /// Called once per in-game day for each striker. Increments their
        /// starvation counter. If the player doesn't change behaviour the
        /// striker will starve to death after 7 days.
        /// </summary>
        public void TickDay(List<string> strikerIds)
        {
            if (strikerIds == null) return;

            for (int i = 0; i < strikerIds.Count; i++)
            {
                string id = strikerIds[i];
                if (string.IsNullOrEmpty(id)) continue;
                if (!_strikerIds.Contains(id)) continue;

                if (!_daysWithoutFood.ContainsKey(id))
                    _daysWithoutFood[id] = 0;

                _daysWithoutFood[id]++;

                if (_daysWithoutFood[id] >= DeathAfterDays)
                {
                    _strikerIds.Remove(id);
                    _daysWithoutFood.Remove(id);
                    OnSurvivorStarvedToDeath?.Invoke(id);
                }
            }
        }

        /// <summary>
        /// Ends a specific survivor's hunger strike — they resume eating.
        /// </summary>
        public void EndStrike(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return;
            if (!_strikerIds.Contains(survivorId)) return;

            _strikerIds.Remove(survivorId);
            _daysWithoutFood.Remove(survivorId);
            OnHungerStrikeEnded?.Invoke(survivorId);
        }

        // ── Save / Load ─────────────────────────────────────────────────

        public HungerStrikeState CaptureState()
        {
            var state = new HungerStrikeState
            {
                eventId = "event_hunger_strike",
                evilChoiceThreshold = EvilChoiceThreshold,
                evilChoiceCount = _evilChoiceCount,
                strikeDurationDays = 0,
                strikerIds = new List<string>(_strikerIds),
                strikerDaysWithoutFood = new List<int>()
            };

            for (int i = 0; i < _strikerIds.Count; i++)
            {
                string id = _strikerIds[i];
                _daysWithoutFood.TryGetValue(id, out int days);
                state.strikerDaysWithoutFood.Add(days);
            }

            return state;
        }

        public void RestoreState(HungerStrikeState saved)
        {
            _strikerIds.Clear();
            _daysWithoutFood.Clear();
            _evilChoiceCount = 0;

            if (saved == null) return;

            _evilChoiceCount = saved.evilChoiceCount;
            if (saved.strikerIds == null) return;

            for (int i = 0; i < saved.strikerIds.Count; i++)
            {
                string id = saved.strikerIds[i];
                _strikerIds.Add(id);
                int days = (saved.strikerDaysWithoutFood != null && i < saved.strikerDaysWithoutFood.Count)
                    ? saved.strikerDaysWithoutFood[i] : 0;
                _daysWithoutFood[id] = days;
            }
        }
    }
}
