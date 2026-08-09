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
    ///
    /// Each ChoiceId below names the method on the matching Encounter_* class
    /// that <see cref="ExpeditionSystem"/> dispatches it to. The classes own the
    /// magnitudes (morale, yields, trust), so MoraleDelta stays 0 here rather
    /// than duplicating those constants where they could drift apart.
    /// </summary>
    public static class NarrativeEncounters
    {
        // ── Choice ids (shared with ExpeditionSystem's dispatcher) ──────

        public const string ChoiceReadLetters = "read_letters";
        public const string ChoiceDeliverLetter = "deliver_letter";
        public const string ChoiceTakeSupplies = "take_supplies";
        public const string ChoiceBurnVan = "burn_van";

        public const string ChoiceExtractData = "extract_data";
        public const string ChoiceTakeSolarPanel = "take_solar_panel";
        public const string ChoiceScavengeElectronics = "scavenge_electronics";
        public const string ChoiceLeaveRunning = "leave_running";

        public const string ChoiceListen = "listen";
        public const string ChoiceShareFood = "share_food";
        public const string ChoiceTellAboutBunker = "tell_about_bunker";
        public const string ChoiceDestroyPiano = "destroy_piano";

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
            enc.choices = new List<EventChoice>
            {
                new EventChoice { ChoiceId = ChoiceReadLetters, Text = "Read the undelivered letters" },
                new EventChoice { ChoiceId = ChoiceDeliverLetter, Text = "Carry the Scavenger Camp envelope to its addressee" },
                new EventChoice { ChoiceId = ChoiceTakeSupplies, Text = "Take the supply packs and go" },
                new EventChoice { ChoiceId = ChoiceBurnVan, Text = "Burn the van behind you" }
            };
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
            enc.choices = new List<EventChoice>
            {
                new EventChoice { ChoiceId = ChoiceExtractData, Text = "Copy the logger's five days of readings" },
                new EventChoice { ChoiceId = ChoiceTakeSolarPanel, Text = "Pry off the solar panel" },
                new EventChoice { ChoiceId = ChoiceScavengeElectronics, Text = "Strip the station for scrap" },
                new EventChoice { ChoiceId = ChoiceLeaveRunning, Text = "Leave it running" }
            };
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
            enc.choices = new List<EventChoice>
            {
                new EventChoice { ChoiceId = ChoiceListen, Text = "Sit on the rubble and listen" },
                new EventChoice { ChoiceId = ChoiceShareFood, Text = "Share what you are carrying" },
                new EventChoice { ChoiceId = ChoiceTellAboutBunker, Text = "Tell him about the bunker" },
                new EventChoice { ChoiceId = ChoiceDestroyPiano, Text = "Cut the piano wire and go" }
            };
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
