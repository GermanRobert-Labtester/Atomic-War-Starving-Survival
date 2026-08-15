using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Medical
{
    [Serializable]
    public class PhantomLimbState
    {
        public string affliction_id = "affliction_phantom_limb";
        public string survivor_id = "";
        public bool episode_active = false;
        public int episodes_per_day = 1;
        public bool morphine_suppressed = false;
        public int total_episodes = 0;
        public float morphine_hours_remaining = 0f;
        public float episode_hours_remaining = 0f;
    }

    /// <summary>
    /// Prompt #830: Phantom Limb.
    /// Post-amputation agonizing pain in the missing limb. Fatigue spikes to
    /// 100 %, screaming wakes everyone in the same room. Requires Morphine
    /// to suppress. Irreversible without a bionic limb.
    /// Plain C# class, not a MonoBehaviour.
    /// </summary>
    public class Affliction_PhantomLimb
    {
        // ── Constants ──────────────────────────────────────────────────
        private const float FATIGUE_SPIKE = 100f;
        private const float MORPHINE_SUPPRESS_HOURS = 8f;
        private const float EPISODE_DURATION_HOURS = 1f;
        private const int MIN_EPISODES_PER_DAY = 1;
        private const int MAX_EPISODES_PER_DAY = 3;

        // ── Events ─────────────────────────────────────────────────────
        public event Action<string> OnEpisodeStarted;              // survivorId
        public event Action<string, float> OnFatigueSpiked;        // survivorId, amount
        public event Action<string> OnOthersWoken;                 // roomId
        public event Action OnMorphineUsed;
        public event Action OnEpisodeEnded;

        // ── State ──────────────────────────────────────────────────────
        private string _survivorId;
        private bool _episodeActive;
        private int _episodesPerDay;
        private bool _morphineSuppressed;
        private int _totalEpisodes;
        private float _morphineHoursRemaining;
        private float _episodeHoursRemaining;
        private string _roomId;

        private readonly System.Random _rng = AtomicWar._Game.Utilities.SeededRandom.Create(
            AtomicWar._Game.Utilities.SeededRandom.WorldSeed, "affliction_phantomlimb");

        // ── Public API ─────────────────────────────────────────────────

        /// <summary>
        /// Initialise the affliction for a specific survivor.
        /// </summary>
        public void Init(string survivorId, string roomId)
        {
            if (string.IsNullOrEmpty(survivorId)) return;
            _survivorId = survivorId;
            _roomId = roomId ?? "";
            _episodesPerDay = _rng.Next(MIN_EPISODES_PER_DAY, MAX_EPISODES_PER_DAY + 1);
        }

        /// <summary>
        /// Trigger a phantom-limb episode. Sets fatigue to 100 % and
        /// screams wake everyone in the room.
        /// </summary>
        public float TriggerEpisode(float currentFatigue)
        {
            if (_morphineSuppressed) return currentFatigue;

            _episodeActive = true;
            _episodeHoursRemaining = EPISODE_DURATION_HOURS;
            _totalEpisodes++;

            OnEpisodeStarted?.Invoke(_survivorId);
            OnFatigueSpiked?.Invoke(_survivorId, FATIGUE_SPIKE);

            if (!string.IsNullOrEmpty(_roomId))
                OnOthersWoken?.Invoke(_roomId);

            return FATIGUE_SPIKE; // caller assigns to survivor
        }

        /// <summary>
        /// Suppress episodes with Morphine for 8 hours.
        /// </summary>
        public void SuppressWithMorphine()
        {
            _morphineSuppressed = true;
            _morphineHoursRemaining = MORPHINE_SUPPRESS_HOURS;
            OnMorphineUsed?.Invoke();
        }

        /// <summary>
        /// Call once per in-game hour. Ticks down episode and morphine
        /// timers.
        /// </summary>
        public void TickHour()
        {
            if (_episodeActive)
            {
                _episodeHoursRemaining -= 1f;
                if (_episodeHoursRemaining <= 0f)
                {
                    _episodeActive = false;
                    _episodeHoursRemaining = 0f;
                    OnEpisodeEnded?.Invoke();
                }
            }

            if (_morphineSuppressed)
            {
                _morphineHoursRemaining -= 1f;
                if (_morphineHoursRemaining <= 0f)
                {
                    _morphineSuppressed = false;
                    _morphineHoursRemaining = 0f;
                }
            }
        }

        /// <summary>Returns true if the survivor is currently in an episode.</summary>
        public bool IsInEpisode()
        {
            return _episodeActive;
        }

        /// <summary>Returns the fatigue spike value applied during an episode.</summary>
        public float GetFatigueSpike()
        {
            return FATIGUE_SPIKE;
        }

        // ── Save / Load ────────────────────────────────────────────────

        public PhantomLimbState CaptureState()
        {
            return new PhantomLimbState
            {
                affliction_id = "affliction_phantom_limb",
                survivor_id = _survivorId ?? "",
                episode_active = _episodeActive,
                episodes_per_day = _episodesPerDay,
                morphine_suppressed = _morphineSuppressed,
                total_episodes = _totalEpisodes,
                morphine_hours_remaining = _morphineHoursRemaining,
                episode_hours_remaining = _episodeHoursRemaining
            };
        }

        public void RestoreState(PhantomLimbState saved)
        {
            if (saved == null) return;
            _survivorId = saved.survivor_id;
            _episodeActive = saved.episode_active;
            _episodesPerDay = saved.episodes_per_day;
            _morphineSuppressed = saved.morphine_suppressed;
            _totalEpisodes = saved.total_episodes;
            _morphineHoursRemaining = saved.morphine_hours_remaining;
            _episodeHoursRemaining = saved.episode_hours_remaining;
        }
    }
}
