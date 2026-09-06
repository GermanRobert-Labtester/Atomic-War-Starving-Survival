// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Ashfall.Core.IO;
using Ashfall.Core.MoralChoice;
using Ashfall.Core.YearOfAsh;

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
        Expired = 8,
        ResolvedTrapAvoided = 9,
        ResolvedIgnored = 10
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

        [JsonIgnore]
        public string OutcomeType => OutcomeTypeStr;

        [JsonPropertyName("authenticity")]
        public string Authenticity { get; set; } = string.Empty;

        [JsonPropertyName("tone_register")]
        public string ToneRegister { get; set; } = string.Empty;

        [JsonPropertyName("days_to_trace")]
        public int DaysToTrace { get; set; } = 4;

        [JsonPropertyName("deadline_days")]
        public int? DeadlineDaysSnake { get; set; }

        [JsonPropertyName("deadlineDays")]
        public int? DeadlineDaysCamel { get; set; }

        [JsonIgnore]
        public int DeadlineDays
        {
            get => DeadlineDaysSnake ?? DeadlineDaysCamel ?? DaysToTrace;
            set => DeadlineDaysSnake = value;
        }

        [JsonPropertyName("sender_survival_days")]
        public int? SenderSurvivalDaysSnake { get; set; }

        [JsonPropertyName("senderSurvivalDays")]
        public int? SenderSurvivalDaysCamel { get; set; }

        [JsonIgnore]
        public int SenderSurvivalDays
        {
            get => SenderSurvivalDaysSnake ?? SenderSurvivalDaysCamel ?? 0;
            set => SenderSurvivalDaysSnake = value;
        }

        [JsonPropertyName("ignore_consequence")]
        public string? IgnoreConsequenceSnake { get; set; }

        [JsonPropertyName("ignoreConsequence")]
        public string? IgnoreConsequenceCamel { get; set; }

        [JsonIgnore]
        public string IgnoreConsequence
        {
            get => IgnoreConsequenceSnake ?? IgnoreConsequenceCamel ?? string.Empty;
            set => IgnoreConsequenceSnake = value;
        }

        [JsonPropertyName("warning_text")]
        public string WarningText { get; set; } = string.Empty;

        [JsonPropertyName("revealed_location")]
        public string RevealedLocation { get; set; } = string.Empty;

        [JsonPropertyName("location_reference")]
        public string? LocationReferenceSnake { get; set; }

        [JsonPropertyName("locationReference")]
        public string? LocationReferenceCamel { get; set; }

        [JsonIgnore]
        public string LocationReference
        {
            get => LocationReferenceSnake ?? LocationReferenceCamel ?? RevealedLocation;
            set => LocationReferenceSnake = value;
        }

        [JsonPropertyName("revealed_knowledge")]
        public string RevealedKnowledge { get; set; } = string.Empty;

        [JsonPropertyName("knowledge_points")]
        public int KnowledgePoints { get; set; }

        [JsonPropertyName("revealed_items")]
        public List<string> RevealedItems { get; set; } = new List<string>();

        [JsonPropertyName("message_fragments")]
        public List<DistressMessageFragment> MessageFragments { get; set; } = new List<DistressMessageFragment>();

        [JsonPropertyName("narrative_id")]
        public string NarrativeId { get; set; } = string.Empty;

        [JsonPropertyName("recruit_survivor_id")]
        public string RecruitSurvivorId { get; set; } = string.Empty;

        [JsonPropertyName("sender_faction_id")]
        public string SenderFactionId { get; set; } = string.Empty;

        [JsonPropertyName("deceptive_faction_id")]
        public string DeceptiveFactionId { get; set; } = string.Empty;

        [JsonPropertyName("moral_choice_id")]
        public string MoralChoiceId { get; set; } = string.Empty;

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

        [JsonIgnore]
        public bool IsTrapOrDeception =>
            string.Equals(Authenticity, "trap", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(Authenticity, "false_flag", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(Authenticity, "bait_trap", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(OutcomeTypeStr, "bait_trap", StringComparison.OrdinalIgnoreCase) ||
            !string.IsNullOrEmpty(DeceptiveFactionId);

        [JsonIgnore]
        public bool IsAutomated =>
            string.Equals(Authenticity, "automated", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(OutcomeTypeStr, "knowledge", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(OutcomeTypeStr, "encrypted", StringComparison.OrdinalIgnoreCase) ||
            FrequencyId == "freq_distress_392_7" ||
            FrequencyId == "freq_distress_512_4" ||
            FrequencyId == "freq_distress_623_8" ||
            FrequencyId == "freq_distress_701_3";

        [JsonIgnore]
        public bool IsGenuineRescue =>
            !IsTrapOrDeception &&
            !IsAutomated &&
            (string.Equals(Authenticity, "genuine", StringComparison.OrdinalIgnoreCase) ||
             !string.IsNullOrEmpty(MoralChoiceId) ||
             !string.IsNullOrEmpty(SenderFactionId) ||
             !string.IsNullOrEmpty(RecruitSurvivorId) ||
             OutcomeTypeStr.StartsWith("survivor", StringComparison.OrdinalIgnoreCase));

        [JsonIgnore]
        public bool IsGrimOrMemorial =>
            !IsTrapOrDeception &&
            !IsGenuineRescue &&
            (string.Equals(Authenticity, "stale", StringComparison.OrdinalIgnoreCase) ||
             string.Equals(OutcomeTypeStr, "narrative", StringComparison.OrdinalIgnoreCase) ||
             string.Equals(OutcomeTypeStr, "supply_cache", StringComparison.OrdinalIgnoreCase) ||
             string.Equals(OutcomeTypeStr, "military", StringComparison.OrdinalIgnoreCase));
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
        public bool IsMoralChoiceAvailable { get; set; }
        public int MoralChoiceResolutionIndex { get; set; } = -1;
        public bool IsIgnored { get; set; }
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

        private readonly Dictionary<int, DistressSignalDefinition> _exactFrequencyIndex =
            new Dictionary<int, DistressSignalDefinition>();

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
            int freqKey = (int)Math.Round(def.FrequencyMhz * 10f);
            _exactFrequencyIndex[freqKey] = def;
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

        public DistressSignalDefinition? GetByExactFrequency(float freqMhz)
        {
            int key = (int)Math.Round(freqMhz * 10f);
            return _exactFrequencyIndex.TryGetValue(key, out var def) ? def : null;
        }

        public ActiveDistressSignal? GetActiveState(string signalId)
        {
            if (string.IsNullOrEmpty(signalId)) return null;
            return _activeSignals.TryGetValue(signalId, out var state) ? state : null;
        }

        public DistressSignalDefinition? FindSignalAtFrequency(float freqMhz, float toleranceMhz = 0.5f)
        {
            int exactKey = (int)Math.Round(freqMhz * 10f);
            if (_exactFrequencyIndex.TryGetValue(exactKey, out var exactDef))
            {
                float exactDiff = Math.Abs(exactDef.FrequencyMhz - freqMhz);
                if (exactDiff <= toleranceMhz) return exactDef;
            }

            int minKey = (int)Math.Floor((freqMhz - toleranceMhz) * 10f);
            int maxKey = (int)Math.Ceiling((freqMhz + toleranceMhz) * 10f);
            DistressSignalDefinition? best = null;
            float minDiff = float.MaxValue;
            for (int k = minKey; k <= maxKey; k++)
            {
                if (_exactFrequencyIndex.TryGetValue(k, out var cand))
                {
                    float diff = Math.Abs(cand.FrequencyMhz - freqMhz);
                    if (diff <= toleranceMhz && diff < minDiff)
                    {
                        minDiff = diff;
                        best = cand;
                    }
                }
            }
            if (best != null) return best;

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

        public int LoadFromDataDirectory(string dataDir, IFileIO? fileIO = null, IJsonSerializer? serializer = null)
        {
            fileIO ??= new FileSystemIO();
            string path = fileIO.Combine(dataDir, "radio_distress_signals.json");
            if (!fileIO.FileExists(path)) return 0;
            string json = fileIO.ReadAllText(path);
            return LoadFromJson(json);
        }

        public bool TryTriggerMoralChoice(string signalId, out string moralChoiceId)
        {
            moralChoiceId = string.Empty;
            if (string.IsNullOrEmpty(signalId)) return false;
            if (!_definitions.TryGetValue(signalId, out var def)) return false;
            if (!_activeSignals.TryGetValue(signalId, out var state)) return false;
            if (state.Status == DistressSignalStatus.Inactive || state.Status == DistressSignalStatus.Expired) return false;
            if (state.MoralChoiceResolutionIndex >= 0) return false;
            if (state.IsResolved) return false;
            if (!def.IsGenuineRescue || def.IsTrapOrDeception || def.IsAutomated) return false;
            if (string.IsNullOrEmpty(def.MoralChoiceId)) return false;
            if (state.HighestClarity < 0.25f) return false;

            state.IsMoralChoiceAvailable = true;
            moralChoiceId = def.MoralChoiceId;
            return true;
        }

        public bool ResolveMoralChoice(
            string signalId,
            int choiceIndex,
            MoralChoiceSystem moral,
            MoralChoiceQuestDefinition questDef,
            int day,
            out MoralChoiceResolution? resolution,
            FactionWarSystem? factionWar = null)
        {
            resolution = null;
            if (string.IsNullOrEmpty(signalId) || questDef == null || moral == null) return false;
            if (!_definitions.TryGetValue(signalId, out var def)) return false;
            if (!_activeSignals.TryGetValue(signalId, out var state)) return false;

            // Idempotent: if already resolved, replay resolution without re-applying deltas
            if (state.MoralChoiceResolutionIndex >= 0)
            {
                moral.TryGetResolution(questDef.Id, out resolution);
                resolution ??= moral.Resolve(questDef, choiceIndex, def.RevealedLocation, day);
                return true;
            }

            resolution = moral.Resolve(questDef, choiceIndex, def.RevealedLocation, day);
            state.MoralChoiceResolutionIndex = choiceIndex;

            if (choiceIndex == 0) // Rescue
            {
                state.Status = DistressSignalStatus.Dispatched;
                state.IsDispatched = true;
                state.IsIgnored = false;
            }
            else // Ignore
            {
                state.Status = DistressSignalStatus.ResolvedIgnored;
                state.IsResolved = true;
                state.IsIgnored = true;
                state.ResolutionSummary = "Signal ignored by command; sender_death inevitable.";

                if (factionWar != null)
                {
                    string faction = !string.IsNullOrEmpty(def.SenderFactionId) ? def.SenderFactionId : def.ReputationFactionId;
                    if (!string.IsNullOrEmpty(faction))
                    {
                        int delta = def.ReputationDelta > 0 ? def.ReputationDelta : 15;
                        factionWar.ModifyStanding(faction, -delta);
                    }
                }
            }

            return true;
        }

        public bool CompleteRescue(string signalId, FactionWarSystem? factionWar = null)
        {
            if (string.IsNullOrEmpty(signalId)) return false;
            if (!_definitions.TryGetValue(signalId, out var def)) return false;
            if (!_activeSignals.TryGetValue(signalId, out var state)) return false;

            if (state.IsIgnored || state.Status == DistressSignalStatus.ResolvedIgnored) return false;
            if (state.Status != DistressSignalStatus.Dispatched && state.Status != DistressSignalStatus.ResolvedRescued) return false;

            // Idempotent
            if (state.Status == DistressSignalStatus.ResolvedRescued) return true;

            state.Status = DistressSignalStatus.ResolvedRescued;
            state.IsResolved = true;
            state.ResolutionSummary = "Rescue completed successfully.";

            if (factionWar != null)
            {
                string faction = !string.IsNullOrEmpty(def.SenderFactionId) ? def.SenderFactionId : def.ReputationFactionId;
                if (!string.IsNullOrEmpty(faction))
                {
                    int delta = def.ReputationDelta > 0 ? def.ReputationDelta : 15;
                    factionWar.ModifyStanding(faction, delta);
                }
            }

            return true;
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
                    resolutionType = s.ResolutionSummary,
                    isMoralChoiceAvailable = s.IsMoralChoiceAvailable,
                    moralChoiceResolutionIndex = s.MoralChoiceResolutionIndex,
                    isIgnored = s.IsIgnored
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
                    state.IsMoralChoiceAvailable = entry.isMoralChoiceAvailable;
                    state.MoralChoiceResolutionIndex = entry.moralChoiceResolutionIndex;
                    state.IsIgnored = entry.isIgnored;
                    state.IsTriangulated = state.Status == DistressSignalStatus.Triangulated ||
                                           state.Status == DistressSignalStatus.Dispatched ||
                                           state.Status == DistressSignalStatus.ResolvedRescued;
                }
                else
                {
                    var status = (DistressSignalStatus)entry.status;
                    _activeSignals[entry.signalId] = new ActiveDistressSignal
                    {
                        SignalId = entry.signalId,
                        Status = status,
                        InterceptedDay = entry.interceptedDay,
                        DaysRemaining = entry.daysRemaining,
                        HighestClarity = entry.highestClarity,
                        IsDispatched = entry.isDispatched,
                        IsResolved = entry.isResolved,
                        ResolutionSummary = entry.resolutionType,
                        IsMoralChoiceAvailable = entry.isMoralChoiceAvailable,
                        MoralChoiceResolutionIndex = entry.moralChoiceResolutionIndex,
                        IsIgnored = entry.isIgnored,
                        IsTriangulated = status == DistressSignalStatus.Triangulated ||
                                         status == DistressSignalStatus.Dispatched ||
                                         status == DistressSignalStatus.ResolvedRescued
                    };
                }
            }
        }
    }
}
