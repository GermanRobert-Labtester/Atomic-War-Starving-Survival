// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Ashfall.Core.IO;

namespace Ashfall.Core.Radio
{
    public enum DistressSignalStatus
    {
        Inactive = 0,
        Intercepted = 1,
        Triangulated = 2,
        Dispatched = 3,
        ResolvedRescued = 4,
        ResolvedGrimTooLate = 5,
        ResolvedTrapDefeated = 6,
        ResolvedMysteryDecoded = 7,
        Expired = 8
    }

    public enum DistressOutcomeType
    {
        SurvivorRecruit = 0,
        SurvivorAlliedGroup = 1,
        GrimMemorialLog = 2,
        RaiderTrapCombat = 3,
        PrewarMysteryBeacon = 4,
        ResourceCache = 5
    }

    [Serializable]
    public sealed class DistressMessageFragment
    {
        [JsonPropertyName("day")]
        public int Day { get; set; }

        [JsonPropertyName("clarity")]
        public float Clarity { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class DistressSignalDefinition
    {
        [JsonPropertyName("frequency_id")]
        public string FrequencyId { get; set; } = string.Empty;

        [JsonPropertyName("frequency_mhz")]
        public string FrequencyMhzStr { get; set; } = "100.0";

        [JsonIgnore]
        public float FrequencyMhz
        {
            get
            {
                if (float.TryParse(FrequencyMhzStr.Replace("MHz", "", StringComparison.OrdinalIgnoreCase).Trim(),
                    System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float f))
                    return f;
                return 100.0f;
            }
        }

        [JsonPropertyName("source_name")]
        public string SourceName { get; set; } = string.Empty;

        [JsonPropertyName("outcome_type")]
        public string OutcomeTypeStr { get; set; } = "survivor_isolated";

        [JsonPropertyName("days_to_trace")]
        public int DaysToTrace { get; set; } = 4;

        [JsonPropertyName("revealed_location")]
        public string RevealedLocation { get; set; } = string.Empty;

        [JsonPropertyName("revealed_items")]
        public List<string> RevealedItems { get; set; } = new List<string>();

        [JsonPropertyName("message_fragments")]
        public List<DistressMessageFragment> MessageFragments { get; set; } = new List<DistressMessageFragment>();

        [JsonPropertyName("narrative_id")]
        public string NarrativeId { get; set; } = string.Empty;

        [JsonPropertyName("recruit_survivor_id")]
        public string RecruitSurvivorId { get; set; } = string.Empty;

        [JsonPropertyName("reputation_faction_id")]
        public string ReputationFactionId { get; set; } = string.Empty;

        [JsonPropertyName("reputation_delta")]
        public int ReputationDelta { get; set; } = 15;

        // Plan 52 — recurring-NPC arc integration (backward-compatible defaults)
        /// <summary>npc_* id of the recurring character this signal is about.
        /// Empty = anonymous signal. Drives stale-signal suppression once the
        /// NPC's arc is terminal.</summary>
        [JsonPropertyName("npc_id")]
        public string NpcId { get; set; } = string.Empty;

        /// <summary>Expansion quest completed when this signal resolves, which
        /// advances the NPC's authored arc. Empty = no arc link.</summary>
        [JsonPropertyName("resolve_quest_id")]
        public string ResolveQuestId { get; set; } = string.Empty;
    }

    /// <summary>
    /// Runtime state for an individual distress signal.
    /// </summary>
    [Serializable]
    public sealed class ActiveDistressSignal
    {
        public string SignalId { get; set; } = string.Empty;
        public DistressSignalStatus Status { get; set; } = DistressSignalStatus.Inactive;
        public int InterceptedDay { get; set; }
        public int DaysRemaining { get; set; }
        public float HighestClarity { get; set; }
        public bool IsTriangulated { get; set; }
        public bool IsDispatched { get; set; }
        public bool IsResolved { get; set; }
        public string ResolutionSummary { get; set; } = string.Empty;
    }

    /// <summary>
    /// Authoritative distress signal manager for ASHFALL airwaves.
    /// Invariant: Pure C#, zero engine references, deterministic lifecycle with terminal outcomes.
    /// </summary>
    public sealed class RadioDistressSystem
    {
        public const string SystemId = "radio_distress_system";

        private readonly Dictionary<string, DistressSignalDefinition> _definitions =
            new Dictionary<string, DistressSignalDefinition>(StringComparer.OrdinalIgnoreCase);

        private readonly Dictionary<string, ActiveDistressSignal> _activeSignals =
            new Dictionary<string, ActiveDistressSignal>(StringComparer.OrdinalIgnoreCase);

        public event Action<DistressSignalDefinition, ActiveDistressSignal>? OnSignalIntercepted;
        public event Action<DistressSignalDefinition, ActiveDistressSignal>? OnSignalTriangulated;
        public event Action<DistressSignalDefinition, ActiveDistressSignal>? OnSignalExpired;
        public event Action<DistressSignalDefinition, ActiveDistressSignal, string>? OnSignalResolved;

        /// <summary>
        /// Plan 52 — optional NPC-arc suppression filter. When set, a signal
        /// whose npc_id is reported suppressed (dead / recruited / terminal
        /// arc) can no longer be intercepted — the world does not re-beggar
        /// people it has already resolved. Mirrors the encounter
        /// WeatherGateFilter pattern: null means no filtering.
        /// </summary>
        public Func<string, bool>? NpcSignalSuppressionFilter { get; set; }

        public RadioDistressSystem()
        {
            RegisterBuiltinCanonicalSignals();
        }

        public IReadOnlyCollection<DistressSignalDefinition> Definitions => _definitions.Values;
        public IReadOnlyCollection<ActiveDistressSignal> ActiveSignals => _activeSignals.Values;

        public int TotalRegisteredSignals => _definitions.Count;

        public void RegisterSignal(DistressSignalDefinition def)
        {
            if (def == null || string.IsNullOrEmpty(def.FrequencyId)) return;
            _definitions[def.FrequencyId] = def;
            if (!_activeSignals.ContainsKey(def.FrequencyId))
            {
                _activeSignals[def.FrequencyId] = new ActiveDistressSignal
                {
                    SignalId = def.FrequencyId,
                    Status = DistressSignalStatus.Inactive,
                    DaysRemaining = def.DaysToTrace
                };
            }
        }

        public DistressSignalDefinition? GetDefinition(string signalId)
        {
            if (string.IsNullOrEmpty(signalId)) return null;
            return _definitions.TryGetValue(signalId, out var def) ? def : null;
        }

        public ActiveDistressSignal? GetActiveState(string signalId)
        {
            if (string.IsNullOrEmpty(signalId)) return null;
            return _activeSignals.TryGetValue(signalId, out var state) ? state : null;
        }

        public DistressSignalDefinition? FindSignalAtFrequency(float freqMhz, float toleranceMhz = 0.5f)
        {
            DistressSignalDefinition? best = null;
            float minDiff = float.MaxValue;
            foreach (var d in _definitions.Values)
            {
                float diff = Math.Abs(d.FrequencyMhz - freqMhz);
                if (diff <= toleranceMhz && diff < minDiff)
                {
                    minDiff = diff;
                    best = d;
                }
            }
            return best;
        }

        /// <summary>
        /// Intercept a distress signal when the player tunes to its frequency.
        /// </summary>
        public bool Intercept(string signalId, int day)
        {
            if (!_definitions.TryGetValue(signalId, out var def)) return false;
            if (!string.IsNullOrEmpty(def.NpcId)
                && NpcSignalSuppressionFilter != null
                && NpcSignalSuppressionFilter(def.NpcId))
                return false;
            var state = _activeSignals[signalId];
            if (state.Status == DistressSignalStatus.Inactive)
            {
                state.Status = DistressSignalStatus.Intercepted;
                state.InterceptedDay = day;
                state.DaysRemaining = def.DaysToTrace;
                state.HighestClarity = 0.35f;
                OnSignalIntercepted?.Invoke(def, state);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Mark signal as triangulated after direction-finding observations meet threshold.
        /// </summary>
        public bool MarkTriangulated(string signalId)
        {
            if (!_definitions.TryGetValue(signalId, out var def)) return false;
            var state = _activeSignals[signalId];
            if (state.Status == DistressSignalStatus.Intercepted)
            {
                state.Status = DistressSignalStatus.Triangulated;
                state.IsTriangulated = true;
                state.HighestClarity = Math.Max(state.HighestClarity, 0.85f);
                OnSignalTriangulated?.Invoke(def, state);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Dispatch an expedition party to the distress site.
        /// </summary>
        public bool DispatchExpedition(string signalId)
        {
            if (!_definitions.TryGetValue(signalId, out _)) return false;
            var state = _activeSignals[signalId];
            if (state.Status == DistressSignalStatus.Intercepted || state.Status == DistressSignalStatus.Triangulated)
            {
                state.Status = DistressSignalStatus.Dispatched;
                state.IsDispatched = true;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Resolve distress call with terminal outcome.
        /// </summary>
        public bool Resolve(string signalId, DistressSignalStatus resolutionStatus, string summary)
        {
            if (!_definitions.TryGetValue(signalId, out var def)) return false;
            var state = _activeSignals[signalId];
            if (state.IsResolved || state.Status == DistressSignalStatus.Expired) return false;

            state.Status = resolutionStatus;
            state.IsResolved = true;
            state.ResolutionSummary = summary;
            OnSignalResolved?.Invoke(def, state, summary);
            return true;
        }

        /// <summary>
        /// Daily clock tick: decrement active distress countdowns and expire overdue signals.
        /// Deterministic integer day math.
        /// </summary>
        public void TickDaily(int currentDay)
        {
            foreach (var kvp in _activeSignals)
            {
                var state = kvp.Value;
                if (state.IsResolved || state.Status == DistressSignalStatus.Expired || state.Status == DistressSignalStatus.Inactive)
                    continue;

                // Decrement if intercepted
                if (state.Status == DistressSignalStatus.Intercepted || state.Status == DistressSignalStatus.Triangulated)
                {
                    state.DaysRemaining--;
                    if (state.DaysRemaining <= 0)
                    {
                        state.Status = DistressSignalStatus.Expired;
                        state.ResolutionSummary = "Distress signal expired. Transmitter has fallen silent.";
                        if (_definitions.TryGetValue(state.SignalId, out var def))
                        {
                            OnSignalExpired?.Invoke(def, state);
                        }
                    }
                }
            }
        }

        // ── Loaders ─────────────────────────────────────────────────────────────

        public int LoadFromJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return 0;
            int added = 0;
            try
            {
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;
                if (root.TryGetProperty("radio_broadcasts", out var arr) && arr.ValueKind == JsonValueKind.Array)
                {
                    foreach (var elem in arr.EnumerateArray())
                    {
                        var def = JsonSerializer.Deserialize<DistressSignalDefinition>(elem.GetRawText(), SystemTextJsonSerializer.Options);
                        if (def != null && !string.IsNullOrEmpty(def.FrequencyId))
                        {
                            RegisterSignal(def);
                            added++;
                        }
                    }
                }
            }
            catch (Exception ex_CATDIAG)
            {
                CatalogDiagnostics.Warn("<json>", "RadioDistressSystem", ex_CATDIAG);
            }
            return added;
        }

        // ── Builtin 26 Canonical Signals (Tasks 24S–24X) ─────────────────────────

        private void RegisterBuiltinCanonicalSignals()
        {
            // 1. Checkpoint Kilo (Baseline / Grim)
            RegisterSignal(new DistressSignalDefinition
            {
                FrequencyId = "freq_distress_217_4",
                FrequencyMhzStr = "217.4",
                SourceName = "Checkpoint Kilo Automated Beacon",
                OutcomeTypeStr = "survivor_community",
                DaysToTrace = 4,
                RevealedLocation = "loc_checkpoint_kilo",
                RevealedItems = new List<string> { "item_military_mre", "item_ammo_556", "item_field_surgical_kit" },
                NarrativeId = "narrative_radio_checkpoint_kilo"
            });

            // 2. Civilian Bunker 4-East (Baseline / False Trap)
            RegisterSignal(new DistressSignalDefinition
            {
                FrequencyId = "freq_distress_148_2",
                FrequencyMhzStr = "148.2",
                SourceName = "Civilian Bunker 4-East (Raider Bait)",
                OutcomeTypeStr = "bait_trap",
                DaysToTrace = 3,
                RevealedLocation = "loc_bunker_4_east_trap",
                RevealedItems = new List<string> { "item_ammo_762", "item_scrap_metal" },
                NarrativeId = "narrative_radio_bait_trap"
            });

            // 3. Sector 9 Substation (Baseline / Grim)
            RegisterSignal(new DistressSignalDefinition
            {
                FrequencyId = "freq_distress_108_9",
                FrequencyMhzStr = "108.9",
                SourceName = "Sector 9 Electrical Substation",
                OutcomeTypeStr = "abandoned_cache",
                DaysToTrace = 5,
                RevealedLocation = "loc_sector_9_substation",
                RevealedItems = new List<string> { "item_copper_wire", "item_fuses_pack" },
                NarrativeId = "narrative_radio_sector_9"
            });

            // 4. Relay 44 Bunker SOS (Baseline / Genuine Rescue)
            RegisterSignal(new DistressSignalDefinition
            {
                FrequencyId = "freq_distress_134_5",
                FrequencyMhzStr = "134.5",
                SourceName = "Relay 44 Bunker SOS",
                OutcomeTypeStr = "survivor_isolated",
                DaysToTrace = 2,
                RevealedLocation = "loc_relay_44_bunker",
                RecruitSurvivorId = "survivor_elena_vasquez",
                ReputationFactionId = "faction_independent_survivors",
                ReputationDelta = 15,
                NarrativeId = "narrative_radio_relay_44"
            });

            // 5. Marsh Water Caravan (Baseline / Genuine Rescue)
            RegisterSignal(new DistressSignalDefinition
            {
                FrequencyId = "freq_distress_162_1",
                FrequencyMhzStr = "162.1",
                SourceName = "Marsh Water Caravan Distress",
                OutcomeTypeStr = "water_caravan_wreck",
                DaysToTrace = 4,
                RevealedLocation = "loc_marsh_caravan_wreck",
                RevealedItems = new List<string> { "item_clean_water", "item_water_filtration_mesh" },
                ReputationFactionId = "faction_scavengers_guild",
                ReputationDelta = 20,
                NarrativeId = "narrative_radio_marsh_caravan"
            });

            // 6. Meridian Cold Store (Genuine Rescue: Pavel)
            RegisterSignal(new DistressSignalDefinition
            {
                FrequencyId = "freq_distress_77_3",
                FrequencyMhzStr = "77.3",
                SourceName = "Meridian Cold Store — Sub-Level 2",
                OutcomeTypeStr = "survivor_isolated",
                DaysToTrace = 5,
                RevealedLocation = "loc_meridian_cold_store",
                RecruitSurvivorId = "survivor_pavel_lineman",
                RevealedItems = new List<string> { "item_seed_potatoes", "item_antifreeze_glycol" },
                ReputationFactionId = "faction_works_allotment",
                ReputationDelta = 25,
                NarrativeId = "narrative_radio_meridian_cold"
            });

            // 7. Barge Olenka Drift (Genuine Rescue: Boatman family)
            RegisterSignal(new DistressSignalDefinition
            {
                FrequencyId = "freq_distress_162_8",
                FrequencyMhzStr = "162.8",
                SourceName = "Barge 'Olenka' — VHF Channel 16",
                OutcomeTypeStr = "survivor_drift",
                DaysToTrace = 4,
                RevealedLocation = "loc_river_barge_olenka",
                RevealedItems = new List<string> { "item_river_navigation_charts", "item_lamp_oil" },
                ReputationFactionId = "faction_river_nomads",
                ReputationDelta = 20,
                NarrativeId = "narrative_radio_barge_olenka"
            });

            // 8. Field Medic Post Omicron (Genuine Rescue: Dr. Tomas Araujo)
            RegisterSignal(new DistressSignalDefinition
            {
                FrequencyId = "freq_distress_124_7",
                FrequencyMhzStr = "124.7",
                SourceName = "Field Medic Post Omicron",
                OutcomeTypeStr = "survivor_medic",
                DaysToTrace = 3,
                RevealedLocation = "loc_field_medic_post",
                RecruitSurvivorId = "survivor_dr_tomas_araujo",
                RevealedItems = new List<string> { "item_antibiotics", "item_field_surgical_kit" },
                ReputationFactionId = "faction_civil_defense",
                ReputationDelta = 25,
                NarrativeId = "narrative_radio_medic_post"
            });
        }

        // ── Save / Load ─────────────────────────────────────────────────────────

        public List<DistressSignalSaveEntry> CaptureState()
        {
            var list = new List<DistressSignalSaveEntry>(_activeSignals.Count);
            foreach (var kvp in _activeSignals)
            {
                var s = kvp.Value;
                list.Add(new DistressSignalSaveEntry
                {
                    signalId = s.SignalId,
                    status = (int)s.Status,
                    interceptedDay = s.InterceptedDay,
                    daysRemaining = s.DaysRemaining,
                    highestClarity = s.HighestClarity,
                    isDispatched = s.IsDispatched,
                    isResolved = s.IsResolved,
                    resolutionType = s.ResolutionSummary
                });
            }
            list.Sort((a, b) => string.Compare(a.signalId, b.signalId, StringComparison.Ordinal));
            return list;
        }

        public void RestoreState(List<DistressSignalSaveEntry>? savedEntries)
        {
            if (savedEntries == null) return;
            foreach (var entry in savedEntries)
            {
                if (entry == null || string.IsNullOrEmpty(entry.signalId)) continue;
                if (_activeSignals.TryGetValue(entry.signalId, out var state))
                {
                    state.Status = (DistressSignalStatus)entry.status;
                    state.InterceptedDay = entry.interceptedDay;
                    state.DaysRemaining = entry.daysRemaining;
                    state.HighestClarity = entry.highestClarity;
                    state.IsDispatched = entry.isDispatched;
                    state.IsResolved = entry.isResolved;
                    state.ResolutionSummary = entry.resolutionType;
                }
                else
                {
                    _activeSignals[entry.signalId] = new ActiveDistressSignal
                    {
                        SignalId = entry.signalId,
                        Status = (DistressSignalStatus)entry.status,
                        InterceptedDay = entry.interceptedDay,
                        DaysRemaining = entry.daysRemaining,
                        HighestClarity = entry.highestClarity,
                        IsDispatched = entry.isDispatched,
                        IsResolved = entry.isResolved,
                        ResolutionSummary = entry.resolutionType
                    };
                }
            }
        }
    }
}
