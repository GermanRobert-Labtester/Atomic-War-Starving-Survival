#pragma warning disable CS0067 // Public API event surface; subscribers arrive with feature wiring
using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Environment;
using Ashfall.Core;

namespace AtomicWar._Game.Factions
{
    [Serializable]
    public class NPC_SunSeekersState
    {
        public string id = "faction_sun_seekers";
        public string displayName = "The Sun-Seekers";
        public bool isActive = true;
        public bool isHostile = false;
        public float trustLevel = 50f;
        public List<string> requestedItems = new List<string> { "item_welders_glass", "item_lead_visor" };
        /// <summary>Lore bible interlocks — lit routes at night offend them; the count is
        /// read by future raid-check wiring (they are the only group that does not
        /// want the routes lit).</summary>
        public int nightLampGrievances;
        /// <summary>Grievance bookkeeping: one entry per game day.</summary>
        public int lastGrievanceDay;
    }

    /// <summary>
    /// Expansion IV — Chapter 40: Factions of the Long Dark.
    /// The Sun-Seekers (UV Cultists): Surface-dwellers who worship the Ozone Scourge as a "cleansing light".
    /// Trade only during Weather_FalseSpring, demanding UV visors and welder's glass in exchange for solar tech.
    /// Violently raid if they detect shelter UV hoarding.
    /// </summary>
    public class NPC_SunSeekers
    {
        private NPC_SunSeekersState _state = new NPC_SunSeekersState();

        public event Action<NPC_SunSeekersState> OnEncounterStarted;
        public event Action<NPC_SunSeekersState> OnTradeResolved;
        public event Action<NPC_SunSeekersState> OnRaidTriggered;

        public NPC_SunSeekersState State => _state;

        /// <summary>GameBootstrap bridge: applies the currents.json entry at construction.</summary>
        public void Initialise(string displayName)
        {
            if (!string.IsNullOrEmpty(displayName)) _state.displayName = displayName;
            _state.isActive = true;
        }

        /// <summary>
        /// Lore bible 05_FACTIONS interlocks — Sun-Seekers and Lamplighters.
        /// The Seekers are the only group that does not want the routes lit at
        /// night. Called each substep; records one grievance per night while
        /// the Lamplighters keep the beacons burning and access is granted.
        /// Returns true when a new grievance was recorded.
        /// </summary>
        public bool AssessNightLamps(NPC_Lamplighters lamplighters, float hour, int day)
        {
            if (lamplighters == null || !lamplighters.State.accessGranted) return false;
            bool isNight = hour >= 20f || hour < 6f;
            if (!isNight) return false;
            if (day <= 0 || day == _state.lastGrievanceDay) return false;

            _state.lastGrievanceDay = day;
            _state.nightLampGrievances++;
            return true;
        }

        public bool CanTradeDuringWeather(WeatherKind weather)
        {
            return weather == WeatherKind.FalseSpring || weather == WeatherKind.SilentSpring;
        }

        public void ModifyTrust(float delta)
        {
            _state.trustLevel = Mathf.Clamp(_state.trustLevel + delta, 0f, 100f);
            if (_state.trustLevel < 20f)
            {
                _state.isHostile = true;
                OnRaidTriggered?.Invoke(_state);
            }
        }

        public NPC_SunSeekersState CaptureState() => _state;
        public void RestoreState(NPC_SunSeekersState saved) { _state = saved ?? new NPC_SunSeekersState(); }
    }
}
