using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Events;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Factory methods for Prompt #901-#904 encounters.
    /// Creates EncounterSO instances at runtime and registers them with
    /// the ExpeditionSystem. These are narrative/utility encounters —
    /// no combat, no skill checks unless specified.
    /// </summary>
    public static class NarrativeEncounters
    {
        // ── Dead Letter Office ──────────────────────────────────────────

        public const string DeadLetterOfficeId = "enc_dead_letter_office";

        public static EncounterSO CreateDeadLetterOffice()
        {
            var enc = ScriptableObject.CreateInstance<EncounterSO>();
            enc.id = DeadLetterOfficeId;
            enc.title = "The Dead Letter Office";
            enc.description =
                "An overturned postal van on the ring road. The driver died at the wheel, " +
                "hands still gripping it. In the back: undelivered letters, supply packs, " +
                "and one envelope addressed to someone at the Scavenger Camp.";
            enc.category = EncounterCategory.Discovery;
            enc.baseWeight = 2f;
            enc.minDangerLevel = 0f;
            enc.requiredLocationId = string.Empty; // can appear on any road node
            enc.forceOnArrival = true;
            enc.enableAutoResolution = false;
            return enc;
        }

        // ── Weather Station ─────────────────────────────────────────────

        public const string WeatherStationId = "enc_weather_station";

        public static EncounterSO CreateWeatherStation()
        {
            var enc = ScriptableObject.CreateInstance<EncounterSO>();
            enc.id = WeatherStationId;
            enc.title = "Automated Weather Station";
            enc.description =
                "A pre-war weather station on a low hill, still running on a cracked solar panel. " +
                "Its LED blinks green every twelve seconds. The data logger contains five days of " +
                "barometric pressure, wind speed, and plume-drift readings.";
            enc.category = EncounterCategory.Discovery;
            enc.baseWeight = 3f;
            enc.minDangerLevel = 0f;
            enc.requiredLocationId = string.Empty; // appears on hill/ridge nodes
            enc.forceOnArrival = true;
            enc.enableAutoResolution = false;
            return enc;
        }

        // ── The Pianist ─────────────────────────────────────────────────

        public const string PianistId = "enc_pianist";

        public static EncounterSO CreatePianist()
        {
            var enc = ScriptableObject.CreateInstance<EncounterSO>();
            enc.id = PianistId;
            enc.title = "The Pianist";
            enc.description =
                "Someone is playing a piano in a ruined conservatory on the east edge of town. " +
                "The building has no roof. The piano is out of tune. The pianist is an old man " +
                "named Matej who has been blind since the flash. He plays from muscle memory.";
            enc.category = EncounterCategory.Discovery;
            enc.baseWeight = 1.5f;
            enc.minDangerLevel = 0f;
            enc.requiredLocationId = string.Empty; // appears on town/city nodes
            enc.forceOnArrival = false; // you can walk past without engaging
            enc.enableAutoResolution = false;
            return enc;
        }

        // ── Register all with the expedition system ─────────────────────

        /// <summary>
        /// Call once from GameBootstrap after ExpeditionSystem is constructed
        /// to register all narrative encounters.
        /// </summary>
        public static void RegisterAll(ExpeditionSystem expedition)
        {
            if (expedition == null) return;
            expedition.AddEncounter(CreateDeadLetterOffice());
            expedition.AddEncounter(CreateWeatherStation());
            expedition.AddEncounter(CreatePianist());
        }
    }
}
