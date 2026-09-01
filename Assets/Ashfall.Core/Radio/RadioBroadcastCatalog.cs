// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Text.Json;
using Ashfall.Core.IO;

namespace Ashfall.Core.Radio
{
    /// <summary>
    /// Unified catalog and loader for all authored radio broadcasts across ASHFALL.
    /// Invariant: Pure C#, zero engine references, loads snake_case and camelCase JSONs seamlessly.
    /// </summary>
    public sealed class RadioBroadcastCatalog
    {
        private readonly Dictionary<string, UnifiedRadioBroadcast> _broadcastsById =
            new Dictionary<string, UnifiedRadioBroadcast>(StringComparer.OrdinalIgnoreCase);

        private readonly List<UnifiedRadioBroadcast> _allBroadcasts = new List<UnifiedRadioBroadcast>();

        public IReadOnlyList<UnifiedRadioBroadcast> AllBroadcasts => _allBroadcasts;
        public int TotalCount => _allBroadcasts.Count;

        public void Register(UnifiedRadioBroadcast b)
        {
            if (b == null || string.IsNullOrEmpty(b.BroadcastId)) return;
            if (_broadcastsById.ContainsKey(b.BroadcastId))
            {
                // Update existing
                int idx = _allBroadcasts.FindIndex(x => string.Equals(x.BroadcastId, b.BroadcastId, StringComparison.OrdinalIgnoreCase));
                if (idx >= 0) _allBroadcasts[idx] = b;
                _broadcastsById[b.BroadcastId] = b;
                return;
            }
            _broadcastsById[b.BroadcastId] = b;
            _allBroadcasts.Add(b);
        }

        public UnifiedRadioBroadcast? GetById(string broadcastId)
        {
            if (string.IsNullOrEmpty(broadcastId)) return null;
            return _broadcastsById.TryGetValue(broadcastId, out var b) ? b : null;
        }

        public List<UnifiedRadioBroadcast> GetByFrequency(float freqMhz, float toleranceMhz = 0.5f)
        {
            var list = new List<UnifiedRadioBroadcast>();
            for (int i = 0; i < _allBroadcasts.Count; i++)
            {
                var b = _allBroadcasts[i];
                if (Math.Abs(b.FrequencyMhz - freqMhz) <= toleranceMhz)
                {
                    list.Add(b);
                }
            }
            return list;
        }

        public List<UnifiedRadioBroadcast> GetEligibleBroadcasts(float freqMhz, int day, float toleranceMhz = 0.5f)
        {
            var list = new List<UnifiedRadioBroadcast>();
            for (int i = 0; i < _allBroadcasts.Count; i++)
            {
                var b = _allBroadcasts[i];
                if (Math.Abs(b.FrequencyMhz - freqMhz) <= toleranceMhz)
                {
                    if (day >= b.DayMin && day <= b.DayMax && day >= b.DayTrigger)
                    {
                        list.Add(b);
                    }
                }
            }
            return list;
        }

        // ── Loaders ─────────────────────────────────────────────────────────────

        public int LoadFromDataDirectory(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            if (string.IsNullOrEmpty(dataDir) || fileIO == null || json == null) return 0;
            int countBefore = _allBroadcasts.Count;

            // 1. Base radio.json
            string radioPath = fileIO.Combine(dataDir, "radio.json");
            if (fileIO.FileExists(radioPath))
            {
                LoadBaseRadioJson(fileIO.ReadAllText(radioPath));
            }

            // 2. year_of_ash_radio.json
            string yoaPath = fileIO.Combine(dataDir, "year_of_ash_radio.json");
            if (fileIO.FileExists(yoaPath))
            {
                LoadYearOfAshRadioJson(fileIO.ReadAllText(yoaPath));
            }

            // 3. verdict_radio.json
            string verdictPath = fileIO.Combine(dataDir, "verdict_radio.json");
            if (fileIO.FileExists(verdictPath))
            {
                LoadVerdictRadioJson(fileIO.ReadAllText(verdictPath));
            }

            // 4. faction_war_radio.json
            string warPath = fileIO.Combine(dataDir, "faction_war_radio.json");
            if (fileIO.FileExists(warPath))
            {
                LoadFactionWarRadioJson(fileIO.ReadAllText(warPath));
            }

            // 5. Authored gap broadcasts (Task 24E)
            RegisterAuthoredGapBroadcasts();

            return _allBroadcasts.Count - countBefore;
        }

        public void LoadBaseRadioJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return;
            try
            {
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;
                if (root.TryGetProperty("radio_broadcasts", out var listProp) && listProp.ValueKind == JsonValueKind.Array)
                {
                    foreach (var elem in listProp.EnumerateArray())
                    {
                        string id = elem.TryGetProperty("id", out var idProp) ? idProp.GetString() ?? "" : "";
                        if (string.IsNullOrEmpty(id)) continue;

                        float freq = elem.TryGetProperty("frequency", out var fProp) ? (float)fProp.GetDouble() : 88.5f;
                        int minD = elem.TryGetProperty("minDay", out var minProp) ? minProp.GetInt32() : 1;
                        int maxD = elem.TryGetProperty("maxDay", out var maxProp) ? maxProp.GetInt32() : 9999;
                        string msg = elem.TryGetProperty("message", out var mProp) ? mProp.GetString() ?? "" : "";
                        string intel = elem.TryGetProperty("intelType", out var itProp) ? itProp.GetString() ?? "" : "Civilian";

                        var genre = BroadcastGenre.CivilianNews;
                        if (intel.Equals("Military", StringComparison.OrdinalIgnoreCase)) genre = BroadcastGenre.MilitaryEdict;
                        else if (intel.Equals("Emergency", StringComparison.OrdinalIgnoreCase)) genre = BroadcastGenre.EmergencyAlert;
                        else if (intel.Equals("NumbersStation", StringComparison.OrdinalIgnoreCase)) genre = BroadcastGenre.NumbersStation;
                        else if (intel.Equals("Survivor", StringComparison.OrdinalIgnoreCase)) genre = BroadcastGenre.SurvivorTestimony;

                        string stationId = RadioStationCatalog.StationCivilDefense;
                        if (Math.Abs(freq - 88.4f) < 0.1f) stationId = RadioStationCatalog.StationGarrisonOverlord;
                        else if (Math.Abs(freq - 104.2f) < 0.1f) stationId = RadioStationCatalog.StationVitrifiedCrater;
                        else if (Math.Abs(freq - 91.3f) < 0.1f) stationId = RadioStationCatalog.StationOpenClassroom;

                        Register(new UnifiedRadioBroadcast
                        {
                            BroadcastId = id,
                            FrequencyMhz = freq,
                            DayMin = minD,
                            DayMax = maxD,
                            DayTrigger = minD,
                            StationId = stationId,
                            SourceName = "Radio Service " + freq.ToString("0.0"),
                            Message = msg,
                            Genre = genre,
                            Reliability = genre == BroadcastGenre.MilitaryEdict ? SourceReliability.Partisan : SourceReliability.Official,
                            Priority = genre == BroadcastGenre.EmergencyAlert ? BroadcastPriority.Urgent : BroadcastPriority.Routine,
                            SignalStrength = 6
                        });
                    }
                }
            }
            catch (Exception ex_CATDIAG)
            {
                CatalogDiagnostics.Warn("radio.json", "BaseRadioLoader", ex_CATDIAG);
            }
        }

        public void LoadYearOfAshRadioJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return;
            try
            {
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;
                if (root.TryGetProperty("broadcasts", out var listProp) && listProp.ValueKind == JsonValueKind.Array)
                {
                    foreach (var elem in listProp.EnumerateArray())
                    {
                        string id = elem.TryGetProperty("id", out var idProp) ? idProp.GetString() ?? "" : "";
                        if (string.IsNullOrEmpty(id)) continue;

                        float freq = ParseFrequencyString(elem.TryGetProperty("frequency", out var fProp) ? fProp.GetString() ?? "" : "88.5");
                        int dayTrigger = elem.TryGetProperty("dayTrigger", out var dProp) ? dProp.GetInt32() : 180;
                        string source = elem.TryGetProperty("source", out var sProp) ? sProp.GetString() ?? "" : "Unknown Source";
                        string msg = elem.TryGetProperty("message", out var mProp) ? mProp.GetString() ?? "" : "";
                        bool isEmerg = elem.TryGetProperty("isEmergency", out var eProp) && eProp.GetBoolean();
                        string audioCue = elem.TryGetProperty("audio_cue", out var aProp) ? aProp.GetString() ?? "" : "";

                        string stationId = RadioStationCatalog.StationAutomatedRelay;
                        if (source.Contains("Garrison", StringComparison.OrdinalIgnoreCase)) stationId = RadioStationCatalog.StationGarrisonOverlord;
                        else if (source.Contains("Crater", StringComparison.OrdinalIgnoreCase) || source.Contains("Liturgy", StringComparison.OrdinalIgnoreCase)) stationId = RadioStationCatalog.StationVitrifiedCrater;
                        else if (source.Contains("Works", StringComparison.OrdinalIgnoreCase) || source.Contains("Allotment", StringComparison.OrdinalIgnoreCase)) stationId = RadioStationCatalog.StationOpenClassroom;

                        Register(new UnifiedRadioBroadcast
                        {
                            BroadcastId = id,
                            FrequencyMhz = freq,
                            DayMin = dayTrigger,
                            DayMax = 9999,
                            DayTrigger = dayTrigger,
                            StationId = stationId,
                            SourceName = source,
                            Message = msg,
                            Genre = isEmerg ? BroadcastGenre.EmergencyAlert : BroadcastGenre.FactionWar,
                            Reliability = SourceReliability.Partisan,
                            Priority = isEmerg ? BroadcastPriority.Urgent : BroadcastPriority.Important,
                            IsEmergency = isEmerg,
                            AudioCue = audioCue,
                            SignalStrength = 7
                        });
                    }
                }
            }
            catch (Exception ex_CATDIAG)
            {
                CatalogDiagnostics.Warn("year_of_ash_radio.json", "YearOfAshRadioLoader", ex_CATDIAG);
            }
        }

        public void LoadVerdictRadioJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return;
            try
            {
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;
                if (root.TryGetProperty("broadcasts", out var listProp) && listProp.ValueKind == JsonValueKind.Array)
                {
                    foreach (var elem in listProp.EnumerateArray())
                    {
                        string id = elem.TryGetProperty("id", out var idProp) ? idProp.GetString() ?? "" : "";
                        if (string.IsNullOrEmpty(id)) continue;

                        float freq = ParseFrequencyString(elem.TryGetProperty("frequency", out var fProp) ? fProp.GetString() ?? "" : "99.0");
                        int dayTrigger = elem.TryGetProperty("dayTrigger", out var dProp) ? dProp.GetInt32() : 210;
                        string source = elem.TryGetProperty("source", out var sProp) ? sProp.GetString() ?? "" : "Census Carrier";
                        string msg = elem.TryGetProperty("message", out var mProp) ? mProp.GetString() ?? "" : "";
                        string audioCue = elem.TryGetProperty("audio_cue", out var aProp) ? aProp.GetString() ?? "" : "";

                        Register(new UnifiedRadioBroadcast
                        {
                            BroadcastId = id,
                            FrequencyMhz = freq,
                            DayMin = dayTrigger,
                            DayMax = 9999,
                            DayTrigger = dayTrigger,
                            StationId = RadioStationCatalog.StationAutomatedRelay,
                            SourceName = source,
                            Message = msg,
                            Genre = BroadcastGenre.VerdictCensus,
                            Reliability = SourceReliability.Automated,
                            Priority = BroadcastPriority.Important,
                            AudioCue = audioCue,
                            SignalStrength = 5
                        });
                    }
                }
            }
            catch (Exception ex_CATDIAG)
            {
                CatalogDiagnostics.Warn("verdict_radio.json", "VerdictRadioLoader", ex_CATDIAG);
            }
        }

        public void LoadFactionWarRadioJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return;
            try
            {
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;
                if (root.TryGetProperty("broadcasts", out var listProp) && listProp.ValueKind == JsonValueKind.Array)
                {
                    foreach (var elem in listProp.EnumerateArray())
                    {
                        string id = elem.TryGetProperty("id", out var idProp) ? idProp.GetString() ?? "" : "";
                        if (string.IsNullOrEmpty(id)) continue;

                        float freq = ParseFrequencyString(elem.TryGetProperty("frequency", out var fProp) ? fProp.GetString() ?? "" : "88.4");
                        int dayTrigger = elem.TryGetProperty("dayTrigger", out var dProp) ? dProp.GetInt32() : 480;
                        string source = elem.TryGetProperty("source", out var sProp) ? sProp.GetString() ?? "" : "Faction Net";
                        string msg = elem.TryGetProperty("message", out var mProp) ? mProp.GetString() ?? "" : "";

                        Register(new UnifiedRadioBroadcast
                        {
                            BroadcastId = id,
                            FrequencyMhz = freq,
                            DayMin = dayTrigger,
                            DayMax = 9999,
                            DayTrigger = dayTrigger,
                            StationId = RadioStationCatalog.StationGarrisonOverlord,
                            SourceName = source,
                            Message = msg,
                            Genre = BroadcastGenre.FactionWar,
                            Reliability = SourceReliability.Partisan,
                            Priority = BroadcastPriority.Important,
                            SignalStrength = 6
                        });
                    }
                }
            }
            catch (Exception ex_CATDIAG)
            {
                CatalogDiagnostics.Warn("faction_war_radio.json", "FactionWarRadioLoader", ex_CATDIAG);
            }
        }

        /// <summary>
        /// Task 24E: 12 authored broadcasts explicitly authored to fill identified schedule gaps.
        /// </summary>
        public void RegisterAuthoredGapBroadcasts()
        {
            var gapBroadcasts = new[]
            {
                // 1-2: Public Service / News
                new UnifiedRadioBroadcast
                {
                    BroadcastId = "rad_gap_civil_chlorine_reserve",
                    FrequencyMhz = 88.50f,
                    DayMin = 35,
                    DayMax = 160,
                    DayTrigger = 35,
                    StationId = RadioStationCatalog.StationCivilDefense,
                    SourceName = "Central Civil Defense Directorate",
                    Title = "Municipal Chlorine Reserve Titration",
                    Message = "Public Health Bulletin 35: Central reservoir chlorination units are operating at half-titration. All domestic intake must be boiled for ten minutes or treated with two drops of chlorine reagent per liter. Do not drink raw runoff.",
                    Genre = BroadcastGenre.CivilianNews,
                    Reliability = SourceReliability.Official,
                    Priority = BroadcastPriority.Important,
                    SignalStrength = 7
                },
                new UnifiedRadioBroadcast
                {
                    BroadcastId = "rad_gap_civil_strontium_soil_assay",
                    FrequencyMhz = 88.50f,
                    DayMin = 65,
                    DayMax = 200,
                    DayTrigger = 65,
                    StationId = RadioStationCatalog.StationCivilDefense,
                    SourceName = "Bureau of Agricultural Reclamation",
                    Title = "Strontium-90 Soil Depth Advisory",
                    Message = "Agricultural Advisory: Topsoil core samples taken in Sector 4 indicate Strontium-90 penetration down to twelve centimeters. Surface crops must be discarded. Only deep-root root crops and sealed glasshouse yields are approved for consumption.",
                    Genre = BroadcastGenre.CivilianNews,
                    Reliability = SourceReliability.Official,
                    Priority = BroadcastPriority.Important,
                    SignalStrength = 6
                },

                // 3-4: Faction / Political
                new UnifiedRadioBroadcast
                {
                    BroadcastId = "rad_gap_garrison_fuel_allocation",
                    FrequencyMhz = 88.40f,
                    DayMin = 50,
                    DayMax = 180,
                    DayTrigger = 50,
                    StationId = RadioStationCatalog.StationGarrisonOverlord,
                    SourceName = "Iron Garrison Ordnance Quartermaster",
                    Title = "Diesel Requisition Order 50",
                    Message = "Quartermaster Decree: Any private generator operating within Sector 4 without a Garrison compliance stamp is subject to immediate fuel confiscation. Diesel is reserved for perimeter perimeter searchlights and water pump batteries.",
                    Genre = BroadcastGenre.MilitaryEdict,
                    Reliability = SourceReliability.Partisan,
                    Priority = BroadcastPriority.Important,
                    SignalStrength = 8
                },
                new UnifiedRadioBroadcast
                {
                    BroadcastId = "rad_gap_cult_black_rain_sermon",
                    FrequencyMhz = 104.20f,
                    DayMin = 75,
                    DayMax = 220,
                    DayTrigger = 75,
                    StationId = RadioStationCatalog.StationVitrifiedCrater,
                    SourceName = "Voice of the Vitrified Crater",
                    Title = "The Sermon of the Black Rain",
                    Message = "The black rain falls because the sky cannot hold our sins. Wash your hands in the ash basin, children. The lead and the sulfur are not poisons; they are the weight of the old world returning to dust. Do not fear the dark clouds.",
                    Genre = BroadcastGenre.ReligiousLiturgy,
                    Reliability = SourceReliability.Partisan,
                    Priority = BroadcastPriority.Routine,
                    SignalStrength = 5
                },

                // 5-6: Pirate / Cultural / Free Radio
                new UnifiedRadioBroadcast
                {
                    BroadcastId = "rad_gap_open_classroom_seed_preservation",
                    FrequencyMhz = 91.30f,
                    DayMin = 40,
                    DayMax = 150,
                    DayTrigger = 40,
                    StationId = RadioStationCatalog.StationOpenClassroom,
                    SourceName = "The Open Classroom (Ottilie)",
                    Title = "Lesson 9 — Saving Heirloom Seeds in Jars",
                    Message = "Good morning students. Today we talk about seeds. If you have glass jars, wash them in boiling water and dry them in the sun. Place two dried bay leaves in the bottom to keep the grain mites away. A jar of rye seed sealed today will feed your grandchildren.",
                    Genre = BroadcastGenre.Educational,
                    Reliability = SourceReliability.Anonymous,
                    Priority = BroadcastPriority.Important,
                    SignalStrength = 6
                },
                new UnifiedRadioBroadcast
                {
                    BroadcastId = "rad_gap_lineman_substation_junction_fix",
                    FrequencyMhz = 142.50f,
                    DayMin = 85,
                    DayMax = 250,
                    DayTrigger = 85,
                    StationId = RadioStationCatalog.StationOpenClassroom,
                    SourceName = "The Lineman's Loop",
                    Title = "Junction 7 Porcelain Insulator Repair",
                    Message = "Lineman Pavel on 142.5. I repaired the porcelain bell insulator on Tower 18 near the limestone quarry. If anyone is running steam power through that line, the insulation is solid. Please don't throw stones at the glass bells.",
                    Genre = BroadcastGenre.InfrastructureLogistics,
                    Reliability = SourceReliability.Anonymous,
                    Priority = BroadcastPriority.Routine,
                    SignalStrength = 5
                },

                // 7-8: Emergency / Weather Integration
                new UnifiedRadioBroadcast
                {
                    BroadcastId = "rad_gap_weather_permafrost_ice_surge",
                    FrequencyMhz = 88.50f,
                    DayMin = 110,
                    DayMax = 300,
                    DayTrigger = 110,
                    StationId = RadioStationCatalog.StationCivilDefense,
                    SourceName = "Civil Meteorological Bureau",
                    Title = "Glacial Runoff Flood Warning",
                    Message = "FLASH ADVISORY: Rapid melting along the western moraine has created an ice-dammed meltwater surge. Low-lying basements along the old canal will flood within eighteen hours. Move stored food to upper tiers.",
                    Genre = BroadcastGenre.EmergencyAlert,
                    Reliability = SourceReliability.Official,
                    Priority = BroadcastPriority.Emergency,
                    IsEmergency = true,
                    SignalStrength = 8
                },
                new UnifiedRadioBroadcast
                {
                    BroadcastId = "rad_gap_orbital_debris_tracking_watch",
                    FrequencyMhz = 104.70f,
                    DayMin = 160,
                    DayMax = 340,
                    DayTrigger = 160,
                    StationId = RadioStationCatalog.StationAutomatedRelay,
                    SourceName = "Station 0 (The Deep Vault)",
                    Title = "Orbital Decay Watch — Debris Field Zeta",
                    Message = "104.7 MHz telemetry log. Uncontrolled kinetic debris field Zeta has decayed below orbit threshold. Fragment reentry expected over southern quadrants between 02:00 and 05:00. Secure exterior antenna ties and stay in bunker bunkrooms.",
                    Genre = BroadcastGenre.EmergencyAlert,
                    Reliability = SourceReliability.Automated,
                    Priority = BroadcastPriority.Urgent,
                    IsEmergency = true,
                    SignalStrength = 6
                },

                // 9-10: Market & Waystation Infrastructure
                new UnifiedRadioBroadcast
                {
                    BroadcastId = "rad_gap_market_antifreeze_scarcity",
                    FrequencyMhz = 101.50f,
                    DayMin = 95,
                    DayMax = 280,
                    DayTrigger = 95,
                    StationId = RadioStationCatalog.StationOpenClassroom,
                    SourceName = "The Works Public Council",
                    Title = "Caravan Market Trade Bulletin — Antifreeze",
                    Message = "Exchange Notice: Glycol antifreeze is in critical demand across Sector 4. The Works trade post is offering thirty kilograms of dried beans or two crates of 12-gauge shells for every five liters of pure coolant.",
                    Genre = BroadcastGenre.TradeMarket,
                    Reliability = SourceReliability.Anonymous,
                    Priority = BroadcastPriority.Important,
                    SignalStrength = 7
                },
                new UnifiedRadioBroadcast
                {
                    BroadcastId = "rad_gap_route_south_pass_rockslide",
                    FrequencyMhz = 142.50f,
                    DayMin = 130,
                    DayMax = 320,
                    DayTrigger = 130,
                    StationId = RadioStationCatalog.StationOpenClassroom,
                    SourceName = "Lineman's Loop / Waystation Scout",
                    Title = "South Pass Rockfall Route Hazard",
                    Message = "Warning to all couriers and caravans: The south highway cut near Mile Marker 42 is blocked by a forty-ton limestone rockfall. Heavy wagons must detour through the old marsh causeway. Foot travelers can scramble through with caution.",
                    Genre = BroadcastGenre.InfrastructureLogistics,
                    Reliability = SourceReliability.Anonymous,
                    Priority = BroadcastPriority.Important,
                    SignalStrength = 6
                },

                // 11-12: Flexible World Atmosphere & Public Health
                new UnifiedRadioBroadcast
                {
                    BroadcastId = "rad_gap_public_health_water_cramps_alert",
                    FrequencyMhz = 88.50f,
                    DayMin = 25,
                    DayMax = 140,
                    DayTrigger = 25,
                    StationId = RadioStationCatalog.StationCivilDefense,
                    SourceName = "Bureau of Public Health",
                    Title = "Waterborne Bacterial Cramps Advisory",
                    Message = "Medical Alert: Increased incidence of acute gastrointestinal cramps reported in eastern shelters. Symptoms: rice-water stool and nausea. Verify cistern filters and boil all drinking supplies immediately.",
                    Genre = BroadcastGenre.EmergencyAlert,
                    Reliability = SourceReliability.Official,
                    Priority = BroadcastPriority.Urgent,
                    SignalStrength = 7
                },
                new UnifiedRadioBroadcast
                {
                    BroadcastId = "rad_gap_foundry_strike_resolution_notice",
                    FrequencyMhz = 88.40f,
                    DayMin = 145,
                    DayMax = 310,
                    DayTrigger = 145,
                    StationId = RadioStationCatalog.StationGarrisonOverlord,
                    SourceName = "Iron Garrison Industrial Commission",
                    Title = "Foundry Shift Allocation Agreement",
                    Message = "Industrial Order 145: The casting shift dispute at Crucible 3 has been resolved under military mediation. Furnace shifts will resume on an eight-hour rotation. Metal ingot distribution resumes Monday morning.",
                    Genre = BroadcastGenre.MilitaryEdict,
                    Reliability = SourceReliability.Partisan,
                    Priority = BroadcastPriority.Important,
                    SignalStrength = 8
                }
            };

            foreach (var b in gapBroadcasts)
            {
                Register(b);
            }
        }

        private static float ParseFrequencyString(string freqStr)
        {
            if (string.IsNullOrWhiteSpace(freqStr)) return 88.5f;
            string cleaned = freqStr.Replace("MHz", "", StringComparison.OrdinalIgnoreCase).Trim();
            if (float.TryParse(cleaned, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float result))
            {
                return result;
            }
            return 88.5f;
        }
    }
}
