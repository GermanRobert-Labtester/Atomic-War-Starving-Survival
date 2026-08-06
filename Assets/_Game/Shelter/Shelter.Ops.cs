using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Shelter.Modules;

namespace AtomicWar._Game.Shelter
{
    public partial class Shelter
    {
        /// <summary>Indoor radiation level for a given exterior radiation dose rate.</summary>
        public float GetInteriorRadsPerHour(float exteriorRads)
        {
            // Overworld structure shielding (rubble, deep underground, etc.) and
            // bunker radiation-shielding module stack multiplicatively.
            float overworldFactor = 1f - Mathf.Clamp01(OverworldShieldingBonus);
            float rads = exteriorRads * overworldFactor;

            var shieldingModule = GetModule("radiation_shielding");
            if (shieldingModule != null && shieldingModule.IsOperational)
            {
                float attenuation = 0f;
                if (shieldingModule.Definition is RadiationShieldingModuleSO shieldSO)
                {
                    attenuation = shieldSO.GetAttenuationFraction(shieldingModule.Level);
                }
                else
                {
                    attenuation = Mathf.Clamp01(shieldingModule.Level * 0.15f);
                }
                // Module shielding stacks multiplicatively with overworld bonus (audit bugfix #2).
                rads = exteriorRads * overworldFactor * (1f - attenuation);
            }

            var airModule = GetModule("air_filtration");
            if (airModule != null && airModule.IsOperational)
            {
                float lowThreshold = 25f;
                float leakRate = 5f;
                if (airModule.Definition is AirFiltrationModuleSO airSO)
                {
                    lowThreshold = airSO.LowHealthThreshold;
                    leakRate = airSO.RadLeakPerTickWhenDepleted;
                }

                if (airModule.FilterHealth <= lowThreshold)
                {
                    float depletionFactor = lowThreshold > 0f ? (lowThreshold - airModule.FilterHealth) / lowThreshold : 1f;
                    rads += leakRate * Mathf.Clamp01(depletionFactor);
                }
            }
            else
            {
                // Unfiltered air leak
                rads += 5f;
            }

            return Mathf.Max(0f, rads);
        }

        public void Tick(float gameHours)
        {
            if (gameHours <= 0f || _modules == null) return;

            for (int i = 0; i < _modules.Count; i++)
            {
                if (_modules[i] != null)
                {
                    _modules[i].Tick(gameHours, this);
                }
            }

            // Decay accumulated ambient contamination. Half-life is on
            // Shelter; mirrors the per-item Contamination decay model.
            TickContaminationDecay(gameHours);
        }

        // -----------------------------------------------------------------
        // Room registry + ids
        // -----------------------------------------------------------------

        /// <summary>Register or replace a room by RoomId.</summary>
        public void RegisterRoom(ShelterRoom room)
        {
            if (room == null || string.IsNullOrEmpty(room.RoomId)) return;
            if (_rooms == null) _rooms = new List<ShelterRoom>();
            for (int i = 0; i < _rooms.Count; i++)
            {
                if (_rooms[i] != null && _rooms[i].RoomId == room.RoomId)
                {
                    _rooms[i] = room;
                    return;
                }
            }
            _rooms.Add(room);
        }

        // -----------------------------------------------------------------
        // Room adjacency (sleep noise, contamination spread hooks)
        // -----------------------------------------------------------------

        /// <summary>Mark two rooms as adjacent (undirected).</summary>
        public void SetRoomsAdjacent(string roomA, string roomB)
        {
            if (string.IsNullOrEmpty(roomA) || string.IsNullOrEmpty(roomB)) return;
            if (string.Equals(roomA, roomB, StringComparison.Ordinal)) return;
            string key = RoomAdjacencyKey(roomA, roomB);
            if (_roomAdjacencyKeys == null) _roomAdjacencyKeys = new List<string>();
            if (!_roomAdjacencyKeys.Contains(key))
            {
                _roomAdjacencyKeys.Add(key);
            }
        }

        /// <summary>True when rooms share a wall / hatch (same room is not adjacent).</summary>
        public bool AreRoomsAdjacent(string roomA, string roomB)
        {
            if (string.IsNullOrEmpty(roomA) || string.IsNullOrEmpty(roomB)) return false;
            if (string.Equals(roomA, roomB, StringComparison.Ordinal)) return false;
            if (_roomAdjacencyKeys == null || _roomAdjacencyKeys.Count == 0) return false;
            return _roomAdjacencyKeys.Contains(RoomAdjacencyKey(roomA, roomB));
        }


        /// <summary>Indoor air quality index (0..100).</summary>
        public float AirQuality
        {
            get
            {
                var airModule = GetModule("air_filtration");
                if (airModule == null || !airModule.IsOperational)
                {
                    return 0f;
                }
                return Mathf.Clamp(airModule.FilterHealth, 0f, 100f);
            }
        }

        /// <summary>Interior warmth bonus (°C) output by heater modules.</summary>
        public float IndoorTempBonus
        {
            get
            {
                var heaterModule = GetModule("heater");
                if (heaterModule == null || !heaterModule.IsOperational)
                {
                    return 0f;
                }

                if (heaterModule.Definition is HeaterModuleSO heaterSO)
                {
                    if (heaterModule.Fuel <= 0f && heaterSO.FuelConsumptionRatePerHour > 0f)
                    {
                        return 0f;
                    }
                    return heaterModule.Level * heaterSO.HeatOutputPerLevel;
                }

                return heaterModule.Fuel > 0f ? heaterModule.Level * 5f : 0f;
            }
        }

        /// <summary>
        /// True when the grow-light module is installed, enabled, and has fuel remaining.
        /// Queried by NeedsSystem each tick to grant survivors an artificial light fraction.
        /// </summary>
        public bool IsGrowLightActive
        {
            get
            {
                var growModule = GetModule("grow_light");
                return growModule != null && growModule.IsOperational && growModule.Fuel > 0f;
            }
        }

        /// <summary>
        /// Direct morale bonus per in-game hour from a running grow-light.
        /// Returns 0 when the module is absent, disabled, or out of fuel.
        /// </summary>
        public float GrowLightMoraleBoost
        {
            get
            {
                if (!IsGrowLightActive) return 0f;
                var growModule = GetModule("grow_light");
                if (growModule?.Definition is Modules.GrowLightModuleSO growSO)
                {
                    return growSO.MoraleBoostPerHour;
                }
                return 0.3f; // fallback default matching LightProfile
            }
        }

        // -------------------------------------------------------------------
        // Bunker contamination (Prompt #26 hatch-dilemma consequence)
        // -------------------------------------------------------------------

        /// <summary>
        /// Accumulated ambient contamination inside the bunker, in
        /// RadsPerHour. Spikes when a contaminated survivor is let in
        /// through the hatch (the Day-30 dilemma's "let_them_in" choice).
        /// Decays naturally over time (see <see cref="TickContaminationDecay"/>).
        /// Save/load safe: the underlying field is a plain float.
        /// </summary>
        public float BunkerContamination { get; private set; }

        /// <summary>
        /// External shielding override from overworld structure (rubble, depth).
        /// Set by HouseToBunkerSystem. 0 = no bonus, 1 = full attenuation.
        /// </summary>
        public float OverworldShieldingBonus { get; set; }

        /// <summary>
        /// Restore the bunker contamination level from a save snapshot.
        /// Only the SaveSystem should call this; normal accumulation uses
        /// <see cref="AddBunkerContamination"/>.
        /// </summary>
        public void SetBunkerContamination(float radsPerHour)
        {
            BunkerContamination = Mathf.Max(0f, radsPerHour);
        }

        /// <summary>
        /// Add a contamination spike to the bunker's ambient level. Used by
        /// the hatch-dilemma handler when the player lets a comms-severed
        /// survivor in. Clamped to non-negative; never goes down via this method.
        /// </summary>
        public void AddBunkerContamination(float radsPerHour)
        {
            if (radsPerHour <= 0f) return;
            BunkerContamination = Mathf.Max(0f, BunkerContamination + radsPerHour);
        }

        /// <summary>
        /// Decay the bunker's accumulated contamination over elapsed game hours.
        /// Mirrors the natural-decay model on <see cref="Contamination"/>:
        /// the rate halves every <see cref="BunkerContaminationHalfLifeHours"/>
        /// of in-game time. Called from <see cref="Shelter.Tick"/> so the
        /// ambient level approaches zero if no further contamination is added.
        /// </summary>
        public void TickContaminationDecay(float gameHours)
        {
            if (gameHours <= 0f || BunkerContamination <= 0f) return;
            float decay = BunkerContamination * (1f - Mathf.Pow(0.5f, gameHours / BunkerContaminationHalfLifeHours));
            BunkerContamination = Mathf.Max(0f, BunkerContamination - decay);
        }

    }
}
