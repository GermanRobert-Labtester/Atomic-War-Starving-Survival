// SPDX-License-Identifier: MIT
using System.Collections.Generic;
using Ashfall.Core.Radio;

namespace Ashfall.Core.Tests.Radio
{
    /// <summary>
    /// Test-only fixture containing the 6 legacy station definitions originally
    /// hardcoded in <c>RadioStationCatalog.RegisterDefaults()</c>. Used to pin field-for-field
    /// parity against the authoritative <c>radio_stations.json</c>.
    /// </summary>
    public static class RadioLegacyCatalogFixture
    {
        public static IReadOnlyList<RadioStationDefinition> CreateDefaults()
        {
            return new List<RadioStationDefinition>
            {
                // 1. Civil Defense & Public Service
                new RadioStationDefinition
                {
                    StationId = RadioStationCatalog.StationCivilDefense,
                    DisplayName = "Central Civil Defense Radio",
                    FrequencyMhz = 88.50f,
                    OwnerFactionId = "faction_civil_defense",
                    PersonaVoice = "Formal, measured mid-century emergency announcer with periodic half-hour chimes.",
                    Reliability = SourceReliability.Official,
                    DefaultState = RadioStationState.Normal,
                    SilenceText = "STATIC... [ Civil Defense carrier lost. Emergency tone unmonitored. ] ...STATIC",
                    JammedText = "STATIC... [ Heavy ionospheric flutter overriding Civil Defense frequency. ]"
                },

                // 2. Iron Garrison Command / Overlord Actual
                new RadioStationDefinition
                {
                    StationId = RadioStationCatalog.StationGarrisonOverlord,
                    DisplayName = "Iron Garrison / Overlord Actual",
                    FrequencyMhz = 88.40f,
                    OwnerFactionId = "military_remnants",
                    PersonaVoice = "Barked tactical military orders, diesel generator hum, squelch tail clicks.",
                    Reliability = SourceReliability.Partisan,
                    DefaultState = RadioStationState.Normal,
                    SilenceText = "STATIC... [ Garrison tactical frequency silent. No carrier active. ] ...STATIC",
                    JammedText = "STATIC... [ Tactical frequency jammed with pulsed tactical noise. ]"
                },

                // 3. Voice of the Vitrified Crater / Ash Pulpit
                new RadioStationDefinition
                {
                    StationId = RadioStationCatalog.StationVitrifiedCrater,
                    DisplayName = "Voice of the Vitrified Crater",
                    FrequencyMhz = 104.20f,
                    OwnerFactionId = "children_of_the_crater",
                    PersonaVoice = "Deep resonant liturgical chanting recorded in sealed subterranean chambers.",
                    Reliability = SourceReliability.Partisan,
                    DefaultState = RadioStationState.Normal,
                    SilenceText = "STATIC... [ The sermon has ended. Pure analog vacuum hiss fills the band. ]",
                    JammedText = "STATIC... [ Static screams with electrical discharge from the fallout cloud. ]"
                },

                // 4. The Open Airwaves / Civilian Free Network
                new RadioStationDefinition
                {
                    StationId = RadioStationCatalog.StationOpenClassroom,
                    DisplayName = "The Open Airwaves (Classroom & Lineman)",
                    FrequencyMhz = 91.30f,
                    OwnerFactionId = "faction_independent_survivors",
                    PersonaVoice = "Warm, patient human voices; chalk taps, tool rattles, wind in antenna masts.",
                    Reliability = SourceReliability.Anonymous,
                    DefaultState = RadioStationState.Normal,
                    SilenceText = "STATIC... [ The classroom is empty. Antenna line swaying in the ash wind. ]",
                    JammedText = "STATIC... [ Weak pirate signal buried in thermal receiver noise. ]"
                },

                // 5. Unidentified Numbers Stations & Sigint
                new RadioStationDefinition
                {
                    StationId = RadioStationCatalog.StationNumbersSigint,
                    DisplayName = "Clandestine Numbers Array",
                    FrequencyMhz = 14.487f,
                    OwnerFactionId = "faction_unknown_intelligence",
                    PersonaVoice = "Synthetic female voice reading 5-figure phonetic number groups, music box chimes.",
                    Reliability = SourceReliability.Automated,
                    DefaultState = RadioStationState.Normal,
                    SilenceText = "STATIC... [ Clandestine carrier dead. Transmitter room RTG depleted. ] ...STATIC",
                    JammedText = "STATIC... [ Heterodyne whistle shifting across the shortwave band. ]"
                },

                // 6. Automated Meteorological & Emergency Relay
                new RadioStationDefinition
                {
                    StationId = RadioStationCatalog.StationAutomatedRelay,
                    DisplayName = "Automated Emergency Beacon Array",
                    FrequencyMhz = 142.85f,
                    OwnerFactionId = "faction_automated_infrastructure",
                    PersonaVoice = "Synthetic robotic teletype synthesizer, continuous navigational carrier pings.",
                    Reliability = SourceReliability.Automated,
                    DefaultState = RadioStationState.Normal,
                    SilenceText = "STATIC... [ Automated repeater shut down. Battery bank drained to zero. ]",
                    JammedText = "STATIC... [ Telemetry carrier mutilated by EMP surge. ]"
                }
            };
        }

        public static void Populate(RadioStationCatalog catalog)
        {
            if (catalog == null) return;
            foreach (var def in CreateDefaults())
            {
                catalog.Register(def);
            }
        }
    }
}
