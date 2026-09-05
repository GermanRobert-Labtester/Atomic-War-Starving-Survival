using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Survivors
{
    /// <summary>Typed emotional channel for morale contagion (Plan 154.2).</summary>
    public enum MoraleEmotion
    {
        Hope = 0,
        Despair = 1,
        Panic = 2
    }

    /// <summary>Parses catalog emotion_type labels into the typed channel.</summary>
    public static class MoraleEmotionNames
    {
        public static bool TryParse(string label, out MoraleEmotion emotion)
        {
            emotion = MoraleEmotion.Despair;
            if (string.IsNullOrEmpty(label)) return false;
            switch (label.Trim().ToLowerInvariant())
            {
                case "hope": emotion = MoraleEmotion.Hope; return true;
                case "despair": emotion = MoraleEmotion.Despair; return true;
                case "panic": emotion = MoraleEmotion.Panic; return true;
                default: return false;
            }
        }

        public static string Label(MoraleEmotion emotion) => emotion switch
        {
            MoraleEmotion.Hope => "hope",
            MoraleEmotion.Panic => "panic",
            _ => "despair"
        };
    }

    /// <summary>
    /// One active contagion source. Catalog values are snapshotted at start so the
    /// instance is self-contained across saves and catalog edits.
    /// </summary>
    public sealed class ContagionSourceState
    {
        public string eventId = string.Empty;          // contagion_*
        public int emotion;                            // (int) MoraleEmotion
        public string sourceSurvivorId = string.Empty; // empty = ambient/environmental
        public float intensity;                        // remaining source strength 0..1
        public float bondMultiplier = 1f;
        public float proximityMultiplier = 1f;
        public float recoveryPerDay = 0.2f;
        public int startedDay;
        public int expiresDay;
    }

    /// <summary>Per-survivor accumulated contagion exposure, decays daily.</summary>
    public sealed class SurvivorContagionPressureState
    {
        public string survivorId = string.Empty;
        public float hopePressure;
        public float despairPressure;
        public float panicPressure;
        public int lastBreakdownDay = -1;
        /// <summary>Transition bookkeeping: true while morale sits in the breakdown band.</summary>
        public bool wasInBreakdownBand;
        public int isolationEndsDay = -1; // >= 0 while socially isolated
    }

    /// <summary>Sustained-pressure ledger per duty-role subgroup (schism eligibility).</summary>
    public sealed class SubgroupSchismPressureState
    {
        public string subgroupId = string.Empty;
        public int consecutivePressureDays;
    }

    /// <summary>Serialized contagion state (checksummed via MoraleContagionSaveCodec).</summary>
    public sealed class MoraleContagionState
    {
        public int schemaVersion = 1;
        public List<ContagionSourceState> activeSources = new List<ContagionSourceState>();
        public List<SurvivorContagionPressureState> survivors = new List<SurvivorContagionPressureState>();
        public List<SubgroupSchismPressureState> subgroupPressure = new List<SubgroupSchismPressureState>();
        public int schismCooldownUntilDay = -1;
        public int lastSchismDay = -1;
    }

    /// <summary>Breakdown raised when a survivor's wellbeing crosses below the canonical threshold.</summary>
    public sealed class MoraleBreakdownEvent
    {
        public string SurvivorId { get; set; } = string.Empty;
        public int Day { get; set; }
        public float StressInput { get; set; }
        public MoraleEmotion DominantEmotion { get; set; }
    }

    /// <summary>
    /// Schism raised when a real duty-role subgroup sustains majority despair
    /// pressure (Plan 154.12/154.13). Payload carries stable ids only.
    /// </summary>
    public sealed class MoraleSchismEvent
    {
        public string SubgroupId { get; set; } = string.Empty;
        public MoraleEmotion Emotion { get; set; }
        public int AffectedCount { get; set; }
        public int MemberCount { get; set; }
        public int TriggerDay { get; set; }
    }

    /// <summary>Influence readout for a survivor (UI surface, Plan 154.11).</summary>
    public sealed class MoraleInfluenceSummary
    {
        public string SurvivorId { get; set; } = string.Empty;
        public float HopePressure { get; set; }
        public float DespairPressure { get; set; }
        public float PanicPressure { get; set; }
        public bool IsIsolated { get; set; }
        public List<ActiveInfluence> Influences { get; set; } = new List<ActiveInfluence>();
    }

    public sealed class ActiveInfluence
    {
        public string EventId { get; set; } = string.Empty;
        public MoraleEmotion Emotion { get; set; }
        public string SourceSurvivorId { get; set; } = string.Empty;
        public float Strength { get; set; }
    }

    /// <summary>
    /// Narrow ports the contagion system needs from the host. Gameplay logic stays
    /// here; the hosts wire these to the canonical authorities (NeedsSystem,
    /// ShelterAssignmentSystem, DutyRosterSystem, TraumaBondSystem,
    /// MentalHealthCrisisSystem) — contagion never duplicates them.
    /// </summary>
    public sealed class MoraleContagionPorts
    {
        /// <summary>Alive roster ids in stable order.</summary>
        public Func<IReadOnlyList<string>> AliveSurvivors { get; set; } =
            () => Array.Empty<string>();
        /// <summary>Canonical morale read (higher = worse, 0..100).</summary>
        public Func<string, float> GetMorale { get; set; } = _ => 50f;
        /// <summary>Canonical morale mutation (the ONLY morale writer reachable from contagion).</summary>
        public Action<string, float> ApplyMoraleDelta { get; set; } = (_, _) => { };
        /// <summary>True when both survivors are actively assigned to the same room.</summary>
        public Func<string, string, bool> AreInSameRoom { get; set; } = (_, _) => false;
        /// <summary>Survivor's duty-role subgroup id, or empty when none.</summary>
        public Func<string, string> GetDutyRole { get; set; } = _ => string.Empty;
        /// <summary>Social bond strength between two survivors, 0..1 (0 = strangers).</summary>
        public Func<string, string, float> GetBondStrength { get; set; } = (_, _) => 0f;
        /// <summary>Whether the HopeBeacon room is built and currently operating.</summary>
        public Func<bool> IsHopeBeaconActive { get; set; } = () => false;
        /// <summary>Canonical unassignment for social isolation (room authority).</summary>
        public Action<string, int> UnassignSurvivor { get; set; } = (_, _) => { };
        /// <summary>Canonical duty-role removal for social isolation (roster authority).</summary>
        public Action<string> ClearDutyRole { get; set; } = _ => { };
        /// <summary>Canonical mental-health breakdown route (crisis authority).</summary>
        public Action<string, float> TriggerBreakdown { get; set; } = (_, _) => { };
        /// <summary>Optional per-survivor contagion resistance 0..1 (traits/beliefs); null-safe.</summary>
        public Func<string, float>? GetContagionResistance { get; set; }
    }

    /// <summary>
    /// Settlement-level social propagation of emotional pressure (Flagship XI —
    /// Plan 154). Amplifies existing survivor morale states through the canonical
    /// social graph; owns ONLY contagion channel state. Deterministic: ordinal
    /// iteration, all deltas buffered and committed after evaluation, no RNG.
    /// </summary>
    public interface IMoraleContagion
    {
        void EvaluateDailyContagion(int day);
        MoraleInfluenceSummary GetInfluenceSummary(string survivorId);
        bool TryApplySocialIsolation(string survivorId, int day, int durationDays);
        bool EndSocialIsolation(string survivorId, int day);
        bool StartContagionEvent(string eventId, string sourceSurvivorId, int day);
        MoraleContagionState CaptureState();
        void RestoreState(MoraleContagionState state);
    }

    public sealed class MoraleContagionSystem : IMoraleContagion
    {
        public const string SystemId = "morale_contagion";

        // Plan 154.8 — canonical low-wellbeing threshold. Morale polarity is
        // INVERTED (higher = worse, NeedsSystem.cs:6-8): wellbeing <10% maps to
        // Morale >= 90. Transition-based; never fires while already below.
        public const float DespairBreakdownMorale = 90f;
        public const int BreakdownCooldownDays = 7;
        public const int BreakdownStressFloor = 40;

        // Plan 154.12 — schism: majority despair pressure, sustained, cooldown-gated.
        public const float SchismAffectedFraction = 0.5f;
        public const float SchismMemberDespairThreshold = 0.5f;
        public const int SchismSustainDays = 3;
        public const int SchismCooldownDays = 21;
        public const int SchismMinSubgroupSize = 2;

        // Daily channel→morale conversion (gentle by design; one-shot world events
        // remain the loud morale movers — contagion spreads, it does not crush).
        public const float DespairMoralePerPressure = 1.5f;
        public const float PanicMoralePerPressure = 1.0f;
        public const float HopeMoralePerPressure = 1.5f;

        public const float DefaultContagionResistance = 0.2f;
        public const float PressureDecayPerDay = 0.8f;   // multiplicative
        public const float PressureCap = 2f;
        public const float IsolationCostMoralePerDay = 1f; // isolation has its own cost
        public const float IsolationInfluenceFactor = 0f;  // isolation cuts influence entirely
        public const int MaxActiveSources = 12;

        // Proximity factors (Plan 154.5 — social influence score, not Euclidean radius).
        public const float SameRoomFactor = 1f;
        public const float SameShiftFactor = 0.8f;
        public const float SettlementBaselineFactor = 0.25f;
        public const float StrangerBondFloor = 0.3f;

        private readonly ContagionEventsCatalogContainer _catalog;
        private readonly MoraleContagionPorts _ports;
        private readonly MoraleContagionState _state = new MoraleContagionState();
        private bool _suppressEvents;
        private int _lastKnownDay;

        /// <summary>Raised when a survivor crosses into the canonical breakdown band.</summary>
        public event Action<MoraleBreakdownEvent>? OnMoraleBreakdown;
        /// <summary>Raised exactly once per qualifying sustained subgroup state.</summary>
        public event Action<MoraleSchismEvent>? OnMoraleSchismTriggered;
        /// <summary>Raised when isolation begins or ends for a survivor.</summary>
        public event Action<string, bool>? OnIsolationChanged;

        public MoraleContagionSystem(ContagionEventsCatalogContainer catalog, MoraleContagionPorts ports)
        {
            _catalog = catalog ?? new ContagionEventsCatalogContainer();
            _ports = ports ?? new MoraleContagionPorts();
        }

        public MoraleContagionState State => _state;

        // ------------------------------------------------------------------ API

        /// <summary>
        /// Instantiates a contagion source from the catalog. Returns false for
        /// unknown ids, unknown emotions, exhausted source slots, or (when a
        /// source survivor is named) a survivor that is not alive.
        /// </summary>
        public bool StartContagionEvent(string eventId, string sourceSurvivorId, int day)
        {
            if (string.IsNullOrEmpty(eventId)) return false;
            var def = FindDef(eventId);
            if (def == null) return false;
            if (!MoraleEmotionNames.TryParse(def.emotion_type, out var emotion)) return false;
            if (_state.activeSources.Count >= MaxActiveSources) return false;

            if (!string.IsNullOrEmpty(sourceSurvivorId) &&
                !_ports.AliveSurvivors().Contains(sourceSurvivorId))
                return false;

            // Idempotent per (event, source): a funeral grief does not stack.
            for (int i = 0; i < _state.activeSources.Count; i++)
            {
                var existing = _state.activeSources[i];
                if (existing == null) continue;
                if (!string.Equals(existing.eventId, eventId, StringComparison.Ordinal)) continue;
                if (string.Equals(existing.sourceSurvivorId, sourceSurvivorId ?? string.Empty, StringComparison.Ordinal))
                {
                    existing.intensity = Math.Max(existing.intensity, Clamp01(def.base_intensity));
                    existing.expiresDay = day + Math.Max(1, def.duration_days);
                    return true;
                }
            }

            _state.activeSources.Add(new ContagionSourceState
            {
                eventId = def.id,
                emotion = (int)emotion,
                sourceSurvivorId = sourceSurvivorId ?? string.Empty,
                intensity = Clamp01(def.base_intensity),
                bondMultiplier = Math.Max(0f, def.bond_multiplier),
                proximityMultiplier = Math.Max(0f, def.proximity_multiplier),
                recoveryPerDay = Clamp01(def.recovery_per_day),
                startedDay = day,
                expiresDay = day + Math.Max(1, def.duration_days)
            });
            return true;
        }

        /// <summary>
        /// One deterministic propagation day: buffer every influence delta first,
        /// commit after evaluation — same-tick feedback is impossible (Plan 154.6).
        /// </summary>
        public void EvaluateDailyContagion(int day)
        {
            if (day > _lastKnownDay) _lastKnownDay = day;
            var alive = _ports.AliveSurvivors();
            if (alive.Count == 0) { DecaySourcesOnly(day); return; }

            PruneDeadSurvivors(alive);
            TickIsolationExpiry(day, alive);
            TickHopeBeacon(day);

            // 1. Buffer: ordinal sources × ordinal recipients, deltas only.
            var buffered = new Dictionary<string, float[]>(); // survivorId -> [hope, despair, panic]
            var sources = _state.activeSources.OrderBy(s => s.eventId, StringComparer.Ordinal)
                                              .ThenBy(s => s.sourceSurvivorId, StringComparer.Ordinal)
                                              .ToList();
            var orderedAlive = alive.OrderBy(id => id, StringComparer.Ordinal).ToList();

            foreach (var source in sources)
            {
                foreach (var recipientId in orderedAlive)
                {
                    float[] weights = InfluenceWeights(source, recipientId);
                    float influence = source.intensity * weights[0] * weights[1] * weights[2] * weights[3];
                    if (influence <= 0f) continue;

                    if (!buffered.TryGetValue(recipientId, out var channels))
                    {
                        channels = new float[3];
                        buffered[recipientId] = channels;
                    }
                    channels[(int)source.emotion] += Math.Min(influence, 1f);
                }
            }

            // 2. Decay accumulated pressure, then commit buffered deltas.
            foreach (var survivor in _state.survivors)
            {
                survivor.hopePressure = ClampCap(survivor.hopePressure * PressureDecayPerDay);
                survivor.despairPressure = ClampCap(survivor.despairPressure * PressureDecayPerDay);
                survivor.panicPressure = ClampCap(survivor.panicPressure * PressureDecayPerDay);
            }
            foreach (var pair in buffered)
            {
                var survivor = GetOrCreate(pair.Key);
                survivor.hopePressure = ClampCap(survivor.hopePressure + pair.Value[(int)MoraleEmotion.Hope]);
                survivor.despairPressure = ClampCap(survivor.despairPressure + pair.Value[(int)MoraleEmotion.Despair]);
                survivor.panicPressure = ClampCap(survivor.panicPressure + pair.Value[(int)MoraleEmotion.Panic]);
            }

            // 3. Isolation carries its own social cost (Plan 154.9).
            foreach (var survivor in _state.survivors)
            {
                if (survivor.isolationEndsDay >= day)
                    _ports.ApplyMoraleDelta(survivor.survivorId, +IsolationCostMoralePerDay);
            }

            // 4. Channel pressure → canonical morale (despair up, panic up, hope down).
            foreach (var survivor in _state.survivors)
            {
                float delta = survivor.despairPressure * DespairMoralePerPressure
                            + survivor.panicPressure * PanicMoralePerPressure
                            - survivor.hopePressure * HopeMoralePerPressure;
                if (MathF.Abs(delta) > 0.01f)
                    _ports.ApplyMoraleDelta(survivor.survivorId, delta);
            }

            // 5. Breakdown crossings + schism ledger (post-commit state only).
            DetectBreakdowns(day, orderedAlive);
            EvaluateSchisms(day, orderedAlive);

            DecaySourcesOnly(day);
        }

        private void DecaySourcesOnly(int day)
        {
            for (int i = _state.activeSources.Count - 1; i >= 0; i--)
            {
                var source = _state.activeSources[i];
                if (source == null) { _state.activeSources.RemoveAt(i); continue; }
                source.intensity = Math.Max(0f, source.intensity - source.recoveryPerDay);
                if (source.intensity <= 0.01f || day >= source.expiresDay)
                    _state.activeSources.RemoveAt(i);
            }
        }

        /// <summary>
        /// Social influence score for source → recipient. Output weights:
        /// [eligibility, proximity, bond, resistance] (Plan 154.4/154.5).
        /// </summary>
        internal float[] InfluenceWeights(ContagionSourceState source, string recipientId)
        {
            // Eligibility: self is immune; the isolated are cut off entirely
            // (inbound and outbound — the curtain works both ways).
            if (string.Equals(recipientId, source.sourceSurvivorId, StringComparison.Ordinal))
                return new float[] { 0f, 0f, 0f, 0f };
            if (source.sourceSurvivorId.Length > 0 && IsIsolated(source.sourceSurvivorId))
                return new float[] { 0f, 0f, 0f, 0f };
            if (recipientId.Length > 0 && IsIsolated(recipientId))
                return new float[] { IsolationInfluenceFactor, 0f, 0f, 0f };

            // Proximity: shared room > shared shift > settlement baseline.
            float proximity;
            if (source.sourceSurvivorId.Length == 0)
                proximity = SettlementBaselineFactor; // ambient sources reach the whole holdfast
            else if (_ports.AreInSameRoom(source.sourceSurvivorId, recipientId))
                proximity = SameRoomFactor;
            else if (!string.IsNullOrEmpty(_ports.GetDutyRole(recipientId)) &&
                     string.Equals(_ports.GetDutyRole(source.sourceSurvivorId),
                                   _ports.GetDutyRole(recipientId), StringComparison.Ordinal))
                proximity = SameShiftFactor;
            else
                proximity = SettlementBaselineFactor;

            // Bond sensitivity: strangers still share walls; close bonds carry weight.
            float bond = StrangerBondFloor;
            if (source.sourceSurvivorId.Length > 0)
                bond = Math.Clamp(_ports.GetBondStrength(source.sourceSurvivorId, recipientId), 0f, 1f);
            bond = (StrangerBondFloor + (1f - StrangerBondFloor) * bond) * source.bondMultiplier;

            // Recipient resistance (traits/beliefs; host-supplied when known).
            float resistance = _ports.GetContagionResistance?.Invoke(recipientId) ?? DefaultContagionResistance;

            return new[] { 1f, proximity * source.proximityMultiplier, bond, 1f - Math.Clamp(resistance, 0f, 1f) };
        }

        private void DetectBreakdowns(int day, List<string> orderedAlive)
        {
            foreach (var survivorId in orderedAlive)
            {
                float morale = _ports.GetMorale(survivorId);
                bool inBand = morale >= DespairBreakdownMorale;
                var survivor = Find(survivorId);

                if (!inBand)
                {
                    // Leaving the band re-arms the transition (Plan 154.8).
                    if (survivor != null) survivor.wasInBreakdownBand = false;
                    continue;
                }

                survivor ??= GetOrCreate(survivorId);
                if (survivor.wasInBreakdownBand) continue;      // already in band: no re-fire
                if (survivor.lastBreakdownDay >= 0 &&
                    day - survivor.lastBreakdownDay < BreakdownCooldownDays) continue;

                survivor.wasInBreakdownBand = true;
                survivor.lastBreakdownDay = day;
                var evt = new MoraleBreakdownEvent
                {
                    SurvivorId = survivorId,
                    Day = day,
                    StressInput = Math.Max(morale, BreakdownStressFloor + survivor.panicPressure * 50f),
                    DominantEmotion = DominantEmotionOf(survivor)
                };
                _ports.TriggerBreakdown(survivorId, evt.StressInput);
                if (!_suppressEvents) OnMoraleBreakdown?.Invoke(evt);
            }
        }

        private void EvaluateSchisms(int day, List<string> orderedAlive)
        {
            if (day < _state.schismCooldownUntilDay) return;

            // Group alive survivors by duty-role subgroup (the real crew authority).
            var groups = new SortedDictionary<string, List<string>>(StringComparer.Ordinal);
            foreach (var survivorId in orderedAlive)
            {
                string role = _ports.GetDutyRole(survivorId);
                if (string.IsNullOrEmpty(role)) continue;
                if (!groups.TryGetValue(role, out var members))
                {
                    members = new List<string>();
                    groups[role] = members;
                }
                members.Add(survivorId);
            }

            var qualifying = new List<MoraleSchismEvent>();
            foreach (var pair in groups)
            {
                if (pair.Value.Count < SchismMinSubgroupSize) continue;
                int affected = pair.Value.Count(id =>
                {
                    var s = Find(id);
                    return s != null && s.despairPressure >= SchismMemberDespairThreshold;
                });
                float fraction = (float)affected / pair.Value.Count;

                var ledger = _state.subgroupPressure.FirstOrDefault(l =>
                    string.Equals(l.subgroupId, pair.Key, StringComparison.Ordinal));
                if (ledger == null)
                {
                    ledger = new SubgroupSchismPressureState { subgroupId = pair.Key };
                    _state.subgroupPressure.Add(ledger);
                }

                if (fraction >= SchismAffectedFraction)
                    ledger.consecutivePressureDays++;
                else
                    ledger.consecutivePressureDays = 0;

                if (ledger.consecutivePressureDays >= SchismSustainDays)
                {
                    qualifying.Add(new MoraleSchismEvent
                    {
                        SubgroupId = pair.Key,
                        Emotion = MoraleEmotion.Despair,
                        AffectedCount = affected,
                        MemberCount = pair.Value.Count,
                        TriggerDay = day
                    });
                    ledger.consecutivePressureDays = 0;
                }
            }

            if (qualifying.Count == 0) return;

            // One schism per day, lexicographically first subgroup — stable under repeats.
            qualifying.Sort((a, b) => string.CompareOrdinal(a.SubgroupId, b.SubgroupId));
            var fired = qualifying[0];
            _state.schismCooldownUntilDay = day + SchismCooldownDays;
            _state.lastSchismDay = day;

            // Contagion applies pressure and reports; the host narrates and any
            // cohesion consequences flow through the canonical bond authorities.
            if (!_suppressEvents) OnMoraleSchismTriggered?.Invoke(fired);
        }

        /// <summary>Social isolation (Plan 154.9): cut contagion links via canonical assignment/roster authorities.</summary>
        public bool TryApplySocialIsolation(string survivorId, int day, int durationDays)
        {
            if (string.IsNullOrEmpty(survivorId)) return false;
            if (!_ports.AliveSurvivors().Contains(survivorId)) return false;
            if (durationDays < 1) return false;
            var survivor = GetOrCreate(survivorId);
            if (survivor.isolationEndsDay >= day && survivor.isolationEndsDay >= 0) return false; // already isolated

            _ports.UnassignSurvivor(survivorId, day);
            _ports.ClearDutyRole(survivorId);
            survivor.isolationEndsDay = day + durationDays;
            if (day > _lastKnownDay) _lastKnownDay = day;
            if (!_suppressEvents) OnIsolationChanged?.Invoke(survivorId, true);
            return true;
        }

        public bool EndSocialIsolation(string survivorId, int day)
        {
            var survivor = Find(survivorId);
            if (survivor == null || survivor.isolationEndsDay < 0) return false;
            survivor.isolationEndsDay = -1;
            if (day > _lastKnownDay) _lastKnownDay = day;
            if (!_suppressEvents) OnIsolationChanged?.Invoke(survivorId, false);
            return true;
        }

        public MoraleInfluenceSummary GetInfluenceSummary(string survivorId)
        {
            var summary = new MoraleInfluenceSummary { SurvivorId = survivorId ?? string.Empty };
            var survivor = Find(survivorId ?? string.Empty);
            if (survivor != null)
            {
                summary.HopePressure = survivor.hopePressure;
                summary.DespairPressure = survivor.despairPressure;
                summary.PanicPressure = survivor.panicPressure;
                summary.IsIsolated = IsIsolated(survivor.survivorId);
            }

            foreach (var source in _state.activeSources.OrderBy(s => s.eventId, StringComparer.Ordinal))
            {
                var weights = InfluenceWeights(source, survivorId ?? string.Empty);
                float strength = source.intensity * weights[0] * weights[1] * weights[2] * weights[3];
                if (strength <= 0f) continue;
                summary.Influences.Add(new ActiveInfluence
                {
                    EventId = source.eventId,
                    Emotion = (MoraleEmotion)source.emotion,
                    SourceSurvivorId = source.sourceSurvivorId,
                    Strength = strength
                });
            }
            return summary;
        }

        // -------------------------------------------------------------- beacon

        private void TickHopeBeacon(int day)
        {
            if (!_ports.IsHopeBeaconActive()) return;
            // The beacon is a standing ambient hope source (contagion_hope_beacon);
            // StartContagionEvent is idempotent per (event, source), so this keeps
            // exactly one alive while the beacon operates.
            StartContagionEvent("contagion_hope_beacon", string.Empty, day);
        }

        private void TickIsolationExpiry(int day, IReadOnlyList<string> alive)
        {
            foreach (var survivor in _state.survivors)
            {
                if (survivor.isolationEndsDay >= 0 && survivor.isolationEndsDay < day
                    && alive.Contains(survivor.survivorId))
                {
                    survivor.isolationEndsDay = -1;
                    if (!_suppressEvents) OnIsolationChanged?.Invoke(survivor.survivorId, false);
                }
            }
        }

        private void PruneDeadSurvivors(IReadOnlyList<string> alive)
        {
            for (int i = _state.survivors.Count - 1; i >= 0; i--)
                if (!alive.Contains(_state.survivors[i].survivorId))
                    _state.survivors.RemoveAt(i);
            for (int i = _state.subgroupPressure.Count - 1; i >= 0; i--)
            {
                string role = _state.subgroupPressure[i].subgroupId;
                bool anyMember = alive.Any(id => string.Equals(_ports.GetDutyRole(id), role, StringComparison.Ordinal));
                if (!anyMember) _state.subgroupPressure.RemoveAt(i);
            }
        }

        // ------------------------------------------------------------- helpers

        private ContagionEventDef? FindDef(string eventId)
        {
            foreach (var def in _catalog.contagion_events)
                if (def != null && string.Equals(def.id, eventId, StringComparison.Ordinal))
                    return def;
            return null;
        }

        private SurvivorContagionPressureState Find(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return null;
            foreach (var survivor in _state.survivors)
                if (survivor != null && string.Equals(survivor.survivorId, survivorId, StringComparison.Ordinal))
                    return survivor;
            return null;
        }

        private SurvivorContagionPressureState GetOrCreate(string survivorId)
        {
            var existing = Find(survivorId);
            if (existing != null) return existing;
            var created = new SurvivorContagionPressureState { survivorId = survivorId };
            _state.survivors.Add(created);
            return created;
        }

        private bool IsIsolated(string survivorId)
        {
            var survivor = Find(survivorId);
            return survivor != null
                && survivor.isolationEndsDay >= 0
                && survivor.isolationEndsDay >= _lastKnownDay;
        }

        private static MoraleEmotion DominantEmotionOf(SurvivorContagionPressureState survivor)
        {
            if (survivor.panicPressure >= survivor.despairPressure && survivor.panicPressure > survivor.hopePressure)
                return MoraleEmotion.Panic;
            if (survivor.hopePressure > survivor.despairPressure) return MoraleEmotion.Hope;
            return MoraleEmotion.Despair;
        }

        private static float Clamp01(float v) => Math.Clamp(v, 0f, 1f);
        private static float ClampCap(float v) => Math.Clamp(v, 0f, PressureCap);

        // ----------------------------------------------------------- persistence

        /// <summary>Captures the authoritative contagion state (no derived links).</summary>
        public MoraleContagionState CaptureState()
        {
            // Deep copy so later mutation cannot alias the captured snapshot.
            var copy = new MoraleContagionState
            {
                schemaVersion = _state.schemaVersion,
                schismCooldownUntilDay = _state.schismCooldownUntilDay,
                lastSchismDay = _state.lastSchismDay
            };
            foreach (var source in _state.activeSources)
                copy.activeSources.Add(new ContagionSourceState
                {
                    eventId = source.eventId,
                    emotion = source.emotion,
                    sourceSurvivorId = source.sourceSurvivorId,
                    intensity = source.intensity,
                    bondMultiplier = source.bondMultiplier,
                    proximityMultiplier = source.proximityMultiplier,
                    recoveryPerDay = source.recoveryPerDay,
                    startedDay = source.startedDay,
                    expiresDay = source.expiresDay
                });
            foreach (var survivor in _state.survivors)
                copy.survivors.Add(new SurvivorContagionPressureState
                {
                    survivorId = survivor.survivorId,
                    hopePressure = survivor.hopePressure,
                    despairPressure = survivor.despairPressure,
                    panicPressure = survivor.panicPressure,
                    lastBreakdownDay = survivor.lastBreakdownDay,
                    wasInBreakdownBand = survivor.wasInBreakdownBand,
                    isolationEndsDay = survivor.isolationEndsDay
                });
            foreach (var ledger in _state.subgroupPressure)
                copy.subgroupPressure.Add(new SubgroupSchismPressureState
                {
                    subgroupId = ledger.subgroupId,
                    consecutivePressureDays = ledger.consecutivePressureDays
                });
            return copy;
        }

        /// <summary>
        /// NON-OPERATIVE restore: reconstructs persisted state only. Never spreads
        /// morale, fires breakdowns/schisms, or re-applies isolation costs.
        /// </summary>
        public void RestoreState(MoraleContagionState state)
        {
            _state.activeSources.Clear();
            _state.survivors.Clear();
            _state.subgroupPressure.Clear();
            _state.schismCooldownUntilDay = -1;
            _state.lastSchismDay = -1;
            if (state == null) return;

            foreach (var source in state.activeSources ?? new List<ContagionSourceState>())
                if (source != null && !string.IsNullOrEmpty(source.eventId))
                    _state.activeSources.Add(new ContagionSourceState
                    {
                        eventId = source.eventId,
                        emotion = source.emotion,
                        sourceSurvivorId = source.sourceSurvivorId ?? string.Empty,
                        intensity = Clamp01(source.intensity),
                        bondMultiplier = Math.Max(0f, source.bondMultiplier),
                        proximityMultiplier = Math.Max(0f, source.proximityMultiplier),
                        recoveryPerDay = Clamp01(source.recoveryPerDay),
                        startedDay = source.startedDay,
                        expiresDay = source.expiresDay
                    });
            foreach (var survivor in state.survivors ?? new List<SurvivorContagionPressureState>())
                if (survivor != null && !string.IsNullOrEmpty(survivor.survivorId))
                    _state.survivors.Add(new SurvivorContagionPressureState
                    {
                        survivorId = survivor.survivorId,
                        hopePressure = ClampCap(survivor.hopePressure),
                        despairPressure = ClampCap(survivor.despairPressure),
                        panicPressure = ClampCap(survivor.panicPressure),
                        lastBreakdownDay = survivor.lastBreakdownDay,
                        wasInBreakdownBand = survivor.wasInBreakdownBand,
                        isolationEndsDay = survivor.isolationEndsDay
                    });
            foreach (var ledger in state.subgroupPressure ?? new List<SubgroupSchismPressureState>())
                if (ledger != null && !string.IsNullOrEmpty(ledger.subgroupId))
                    _state.subgroupPressure.Add(new SubgroupSchismPressureState
                    {
                        subgroupId = ledger.subgroupId,
                        consecutivePressureDays = Math.Max(0, ledger.consecutivePressureDays)
                    });
            _state.schismCooldownUntilDay = state.schismCooldownUntilDay;
            _state.lastSchismDay = state.lastSchismDay;
        }

        /// <summary>Test/host hook: suppress event raising around bulk restores.</summary>
        public void SetEventsSuppressed(bool suppressed) => _suppressEvents = suppressed;
    }
}
