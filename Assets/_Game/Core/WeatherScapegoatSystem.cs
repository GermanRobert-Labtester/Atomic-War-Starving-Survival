using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Weather Scapegoating / Witch Hunts (Prompt #76). During extended
    /// NuclearWinter blizzards, primitive factions go mad. They blame the
    /// player's bunker (radio signals/generator smoke) for angering the sky
    /// and demand the player turn off all generators for 3 days as "tribute
    /// to the cold." Non-compliance triggers a massive raid.
    /// Save/load safe. Plain C#.
    /// </summary>
    public class WeatherScapegoatSystem
    {
        /// <summary>Consecutive Blizzard hours before factions start scapegoating.</summary>
        public const float BlizzardHoursToTrigger = 72f;

        /// <summary>Generator-off duration demanded (game hours).</summary>
        public const float TributeDurationHours = 72f;

        /// <summary>Trust penalty if the player refuses the tribute demand.</summary>
        public const float RefusalTrustPenalty = -40f;

        /// <summary>Raid strength multiplier if tribute is refused.</summary>
        public const float RefusalRaidMultiplier = 2.5f;

        /// <summary>Event id for the tribute demand.</summary>
        public const string TributeEventId = "weather_tribute_demand";

        /// <summary>Factions that participate in scapegoating.</summary>
        public static readonly string[] ScapegoatFactionIds =
        {
            "doomsday_preppers",
            "cult_of_the_glow"
        };

        /// <summary>Accumulated consecutive blizzard hours.</summary>
        private float _consecutiveBlizzardHours;

        /// <summary>Whether a tribute demand has been issued this blizzard cycle.</summary>
        private bool _tributeDemanded;

        /// <summary>Hours remaining on the tribute if accepted.</summary>
        private float _tributeHoursRemaining;

        /// <summary>Whether the player is currently under tribute.</summary>
        private bool _tributeActive;

        /// <summary>Whether the tribute was refused (raid incoming).</summary>
        private bool _tributeRefused;

        private readonly System.Random _rng;

        // -- Public state --
        public bool TributeActive => _tributeActive;
        public bool TributeRefused => _tributeRefused;
        public float TributeHoursRemaining => _tributeHoursRemaining;
        public float ConsecutiveBlizzardHours => _consecutiveBlizzardHours;

        // -- Events --
        public event Action OnTributeDemanded;
        public event Action OnTributeAccepted;
        public event Action OnTributeRefused;
        public event Action OnTributeCompleted;

        public WeatherScapegoatSystem(System.Random rng = null)
        {
            _rng = rng ?? new System.Random(76);
        }

        /// <summary>
        /// Tick: track blizzard hours and trigger scapegoating.
        /// </summary>
        public void Tick(float gameHours, Environment.WeatherKind currentWeather,
            Action<string, int, string> scheduleEvent = null, int currentDay = 1)
        {
            if (gameHours <= 0f) return;

            // Track consecutive blizzard hours.
            if (currentWeather == Environment.WeatherKind.Blizzard)
            {
                _consecutiveBlizzardHours += gameHours;
            }
            else
            {
                _consecutiveBlizzardHours = 0f;
                _tributeDemanded = false;
                _tributeRefused = false;
            }

            // Trigger tribute demand.
            if (!_tributeDemanded && !_tributeActive && !_tributeRefused
                && _consecutiveBlizzardHours >= BlizzardHoursToTrigger)
            {
                _tributeDemanded = true;
                OnTributeDemanded?.Invoke();
                scheduleEvent?.Invoke(TributeEventId, currentDay, "weather_scapegoat");
            }

            // Tribute countdown.
            if (_tributeActive && _tributeHoursRemaining > 0f)
            {
                _tributeHoursRemaining -= gameHours;
                if (_tributeHoursRemaining <= 0f)
                {
                    _tributeActive = false;
                    _tributeDemanded = false;
                    OnTributeCompleted?.Invoke();
                }
            }
        }

        /// <summary>
        /// Accept the tribute demand. All generators must be off for 72h.
        /// </summary>
        public bool AcceptTribute()
        {
            if (!_tributeDemanded || _tributeActive) return false;
            _tributeActive = true;
            _tributeHoursRemaining = TributeDurationHours;
            _tributeRefused = false;
            OnTributeAccepted?.Invoke();
            return true;
        }

        /// <summary>
        /// Refuse the tribute. Triggers a massive raid.
        /// </summary>
        public bool RefuseTribute()
        {
            if (!_tributeDemanded || _tributeRefused) return false;
            _tributeRefused = true;
            _tributeDemanded = false;
            OnTributeRefused?.Invoke();
            return true;
        }

        /// <summary>
        /// Whether generators should be forced off (tribute active).
        /// </summary>
        public bool ShouldForceGeneratorsOff => _tributeActive;

        // -----------------------------------------------------------------
        // Save / Load
        // -----------------------------------------------------------------

        public ScapegoatSave CaptureState()
        {
            return new ScapegoatSave
            {
                ConsecutiveBlizzardHours = _consecutiveBlizzardHours,
                TributeDemanded = _tributeDemanded,
                TributeActive = _tributeActive,
                TributeHoursRemaining = _tributeHoursRemaining,
                TributeRefused = _tributeRefused
            };
        }

        public void RestoreState(ScapegoatSave save)
        {
            if (save == null)
            {
                _consecutiveBlizzardHours = 0f;
                _tributeDemanded = false;
                _tributeActive = false;
                _tributeHoursRemaining = 0f;
                _tributeRefused = false;
                return;
            }
            _consecutiveBlizzardHours = save.ConsecutiveBlizzardHours;
            _tributeDemanded = save.TributeDemanded;
            _tributeActive = save.TributeActive;
            _tributeHoursRemaining = save.TributeHoursRemaining;
            _tributeRefused = save.TributeRefused;
        }
    }

    [Serializable]
    public class ScapegoatSave
    {
        public float ConsecutiveBlizzardHours;
        public bool TributeDemanded;
        public bool TributeActive;
        public float TributeHoursRemaining;
        public bool TributeRefused;
    }
}
