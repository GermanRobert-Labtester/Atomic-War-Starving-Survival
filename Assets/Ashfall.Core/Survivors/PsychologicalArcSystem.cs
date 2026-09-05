// SPDX-License-Identifier: MIT
// ============================================================================
// System     : PsychologicalArcSystem (Plan 164 — Survivor Breakdown Arcs)
// Model      : Fictional game-condition arcs with authored triggers and
//              conditional, non-guaranteed behaviors. Stress is READ from the
//              canonical NeedsSystem (Morale, higher = worse) via an injected
//              reader; this system never writes needs, relations, inventory or
//              fire state directly — every consequence routes through an
//              event/callback consumed by the owning authority (plan §2.9).
// Therapy    : the wired Sanatorium is THE treatment authority. It calls into
//              this system (via the host port) for arc conditions; treatment
//              progress here only advances arc recovery.
// RNG        : injected per tick (psychology.arc_trigger / .arc_behavior /
//              .recovery forks owned by the host).
// ============================================================================
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Survivors
{
    public enum ArcStage
    {
        Latent = 0,
        Emerging = 1,
        Acute = 2,
        Crisis = 3,
        Recovering = 4,
        Resolved = 5
    }

    public enum ArcBehavior
    {
        None = 0,
        StashTransfer = 1,      // hoarding: move one low-value item to private stash
        UnsafeFireRequest = 2,  // fire fixation: REQUEST an incident (fire authority resolves)
        RefuseAssignment = 3,   // persecutory: refuse work / accuse (assignment authority acts)
        WithdrawSelfCare = 4    // shutdown: self-care lapse (needs authority applies)
    }

    [Serializable]
    public sealed class BreakdownArcDef
    {
        public string id { get; set; } = string.Empty;          // arc_*
        public string display_name { get; set; } = string.Empty;
        public int stress_threshold { get; set; } = 90;         // canonical morale (higher = worse)
        public int minimum_stress_days { get; set; } = 6;       // sustained exposure before trigger
        public string behavior { get; set; } = "stash_transfer";
        public float behavior_chance { get; set; } = 0.25f;     // per eligible behavior day
        public int behavior_cooldown_days { get; set; } = 3;
        public int crisis_behavior_min_stage { get; set; } = (int)ArcStage.Crisis;
        public List<string> treatment_tags { get; set; } = new List<string>();
        public int relapse_cooldown_days { get; set; } = 30;
        public List<string> tags { get; set; } = new List<string>();
    }

    [Serializable]
    public sealed class StashEntry
    {
        public string item_id { get; set; } = string.Empty;
        public int count { get; set; }
        public int day { get; set; }
        public bool discovered { get; set; }
    }

    [Serializable]
    public sealed class SurvivorArcState
    {
        public string survivor_id { get; set; } = string.Empty;
        public string arc_id { get; set; } = string.Empty;
        public int stage { get; set; } = (int)ArcStage.Latent;
        public int exposure_days { get; set; }                 // sustained high-stress accumulator
        public int behavior_cooldown_until_day { get; set; } = -1;
        public int treatment_progress { get; set; }
        public List<StashEntry> stash { get; set; } = new List<StashEntry>();
        public bool catharsis_granted;
        public int resolved_day = -1;                           // relapse cooldown anchor
    }

    [Serializable]
    public sealed class PsychologicalArcState
    {
        public string system_id { get; set; } = "psychological_arcs";
        public int schema_version { get; set; } = 1;
        public int last_tick_day { get; set; }
        public List<SurvivorArcState> survivors { get; set; } = new List<SurvivorArcState>();
        /// <summary>Bounded, non-stacking stress resilience from catharsis (0..0.15).</summary>
        public Dictionary<string, float> resilience_bonus { get; set; } = new Dictionary<string, float>(StringComparer.Ordinal);
    }

    [Serializable]
    public sealed class MentalArcCatalogContainer
    {
        public int schema_version { get; set; } = 1;
        public List<BreakdownArcDef> arcs { get; set; } = new List<BreakdownArcDef>();
    }

    public static class MentalArcCatalogLoader
    {
        public const string DefaultFileName = "mental_arcs.json";

        public static MentalArcCatalogContainer Load(
            string dataDir, IFileIO? files = null, IJsonSerializer? json = null)
        {
            files ??= new FileSystemIO();
            json ??= new SystemTextJsonSerializer();
            var path = System.IO.Path.Combine(dataDir ?? string.Empty, DefaultFileName);
            if (!files.FileExists(path)) return new MentalArcCatalogContainer();
            try
            {
                var text = files.ReadAllText(path);
                return json.Deserialize<MentalArcCatalogContainer>(text) ?? new MentalArcCatalogContainer();
            }
            catch (Exception)
            {
                return new MentalArcCatalogContainer();
            }
        }

        public static List<string> Validate(MentalArcCatalogContainer catalog)
        {
            var diags = new List<string>();
            var seen = new HashSet<string>(StringComparer.Ordinal);
            foreach (var a in catalog.arcs)
            {
                if (a == null) continue;
                if (string.IsNullOrEmpty(a.id) || !a.id.StartsWith("arc_", StringComparison.Ordinal))
                    diags.Add($"{a.id}: id must use the arc_ prefix");
                else if (!seen.Add(a.id))
                    diags.Add($"{a.id}: duplicate arc id");
                if (a.stress_threshold < 50 || a.stress_threshold > 100)
                    diags.Add($"{a.id}: stress_threshold must be within [50,100]");
                if (a.minimum_stress_days < 3)
                    diags.Add($"{a.id}: minimum_stress_days must be >= 3 (no one-day triggers)");
                if (a.behavior_chance < 0f || a.behavior_chance > 1f)
                    diags.Add($"{a.id}: behavior_chance must be within [0,1]");
                if (!Enum.TryParse<ArcBehavior>(ToPascal(a.behavior), out _))
                    diags.Add($"{a.id}: unknown behavior '{a.behavior}'");
            }
            diags.Sort(StringComparer.Ordinal);
            return diags;
        }

        private static string ToPascal(string snake)
        {
            if (string.IsNullOrEmpty(snake)) return snake;
            var parts = snake.Split('_');
            var sb = new System.Text.StringBuilder();
            foreach (var p in parts)
            {
                if (p.Length == 0) continue;
                sb.Append(char.ToUpperInvariant(p[0])).Append(p, 1, p.Length - 1);
            }
            return sb.ToString();
        }

        public static ArcBehavior ParseBehavior(string snake) =>
            Enum.TryParse<ArcBehavior>(ToPascal(snake), out var b) ? b : ArcBehavior.None;
    }

    /// <summary>
    /// Long-running psychological crisis arcs. Trigger requires SUSTAINED
    /// canonical stress (never one spike); behaviors are conditional
    /// opportunities, not guaranteed acts; every consequence is routed to its
    /// owning authority through events. Items never vanish — stash transfers
    /// are ledgered and reversible.
    /// </summary>
    public sealed class PsychologicalArcSystem
    {
        public const string SystemId = "psychological_arcs";
        public const float MaxResilienceBonus = 0.15f;
        public const float ResiliencePerCatharsis = 0.05f;
        public const int TreatmentProgressToResolve = 6;
        public const int DaysPerStage = 3;

        private readonly List<BreakdownArcDef> _arcs = new List<BreakdownArcDef>();
        private readonly Dictionary<string, BreakdownArcDef> _arcsById =
            new Dictionary<string, BreakdownArcDef>(StringComparer.Ordinal);
        private readonly PsychologicalArcState _state = new PsychologicalArcState();

        public event Action<string, string>? OnBreakdownArcStarted;
        public event Action<string, string, ArcStage, ArcStage>? OnBreakdownEscalated;
        public event Action<string, ArcBehavior>? OnBreakdownBehaviorOccurred;
        public event Action<string, int>? OnUnsafeFireIncidentRequested;
        public event Action<string, string, int>? OnStashTransferred;
        public event Action<string>? OnStashDiscovered;
        public event Action<string, string>? OnBreakdownResolved;

        /// <summary>Host picks the deterministic low-value item and moves it out of
        /// the shared inventory; the system ledger keeps the audit trail.</summary>
        public Func<string, string, int, (string itemId, int count)?>? TryTransferToStash;

        public PsychologicalArcSystem(IEnumerable<BreakdownArcDef>? arcs)
        {
            if (arcs == null) return;
            foreach (var a in arcs)
            {
                if (a == null || string.IsNullOrEmpty(a.id)) continue;
                _arcs.Add(a);
                _arcsById[a.id] = a;
            }
            _arcs.Sort((x, y) => string.CompareOrdinal(x.id, y.id));
        }

        public string SaveId => SystemId;
        public PsychologicalArcState State => _state;
        public IReadOnlyList<BreakdownArcDef> Arcs => _arcs;
        public BreakdownArcDef? Arc(string arcId) =>
            _arcsById.TryGetValue(arcId ?? string.Empty, out var a) ? a : null;

        private SurvivorArcState? ArcOf(string survivorId)
        {
            foreach (var s in _state.survivors)
                if (string.Equals(s.survivor_id, survivorId, StringComparison.Ordinal)) return s;
            return null;
        }

        private SurvivorArcState Ensure(string survivorId)
        {
            var s = ArcOf(survivorId);
            if (s == null)
            {
                s = new SurvivorArcState { survivor_id = survivorId ?? string.Empty };
                _state.survivors.Add(s);
                _state.survivors.Sort((a, b) => string.CompareOrdinal(a.survivor_id, b.survivor_id));
            }
            return s;
        }

        // ------------------------------------------------------------------
        // Queries (host port + UI)
        // ------------------------------------------------------------------

        public bool HasArc(string survivorId, string arcId)
        {
            var s = ArcOf(survivorId);
            return s != null
                   && string.Equals(s.arc_id, arcId, StringComparison.Ordinal)
                   && (ArcStage)s.stage != ArcStage.Resolved;
        }

        public ArcStage StageOf(string survivorId)
        {
            var s = ArcOf(survivorId);
            return s == null ? ArcStage.Latent : (ArcStage)s.stage;
        }

        public IReadOnlyList<StashEntry> StashOf(string survivorId) =>
            ArcOf(survivorId)?.stash ?? (IReadOnlyList<StashEntry>)Array.Empty<StashEntry>();

        public bool IsEligibleForWork(string survivorId)
        {
            var s = ArcOf(survivorId);
            if (s == null || string.IsNullOrEmpty(s.arc_id)) return true;
            var stage = (ArcStage)s.stage;
            // Only shutdown arcs gate work, and only from Acute onward; every
            // other arc leaves the survivor assignable (plan §7.12-7.13).
            var def = Arc(s.arc_id);
            if (def == null || MentalArcCatalogLoader.ParseBehavior(def.behavior) != ArcBehavior.WithdrawSelfCare)
                return true;
            return stage != ArcStage.Acute && stage != ArcStage.Crisis;
        }

        public float ResilienceBonus(string survivorId) =>
            _state.resilience_bonus.TryGetValue(survivorId ?? string.Empty, out var v) ? v : 0f;

        public int AcuteStressPermille(string survivorId, Func<string, float> readStress)
        {
            float stress = readStress?.Invoke(survivorId) ?? 0f;
            var s = ArcOf(survivorId);
            float stageBoost = s == null || string.IsNullOrEmpty(s.arc_id)
                ? 0f
                : Math.Min(0.3f, ((ArcStage)s.stage) switch
                {
                    ArcStage.Emerging => 0.05f,
                    ArcStage.Acute => 0.15f,
                    ArcStage.Crisis => 0.3f,
                    _ => 0f
                });
            return (int)Math.Clamp((stress / 100f + stageBoost) * 1000f, 0f, 1000f);
        }

        // ------------------------------------------------------------------
        // Daily progression (plan §11.3: runs AFTER needs are finalized)
        // ------------------------------------------------------------------

        /// <summary>
        /// One psychology day. Draw budget: one arc-trigger draw per eligible
        /// unafflicted survivor (sustained exposure met); one behavior draw per
        /// active arc off cooldown at or past its stage floor; one recovery
        /// check per recovering arc — nothing else consumes RNG.
        /// </summary>
        public void TickDay(
            int day,
            IReadOnlyList<string> survivors,
            Func<string, float> readStress,
            ISeededRng triggerRng,
            ISeededRng behaviorRng,
            ISeededRng recoveryRng)
        {
            if (_state.last_tick_day == day) return;
            _state.last_tick_day = day;
            if (survivors == null || readStress == null || _arcs.Count == 0) return;

            var order = new List<string>(survivors);
            order.Sort(StringComparer.Ordinal);

            foreach (var id in order)
            {
                if (string.IsNullOrEmpty(id)) continue;
                var s = Ensure(id);
                float stress = readStress(id);
                float resilience = ResilienceBonus(id);
                bool highStress = stress >= 85f * (1f + resilience);

                // Exposure accumulator (sustained pressure, never one spike).
                s.exposure_days = highStress ? s.exposure_days + 1 : Math.Max(0, s.exposure_days - 1);

                if (string.IsNullOrEmpty(s.arc_id) || (ArcStage)s.stage == ArcStage.Resolved)
                {
                    TickTrigger(day, s, triggerRng);
                }
                else
                {
                    var before = (ArcStage)s.stage;
                    TickActiveArc(day, s, behaviorRng, recoveryRng, highStress, before);
                }
            }
        }

        private void TickTrigger(int day, SurvivorArcState s, ISeededRng triggerRng)
        {
            // Sustained-exposure gate must be met BEFORE any draw is consumed.
            int minDays = int.MaxValue;
            foreach (var a in _arcs)
                minDays = Math.Min(minDays, a.minimum_stress_days);
            if (minDays == int.MaxValue || s.exposure_days < minDays) return;
            if (s.resolved_day >= 0 && day - s.resolved_day < 30) return; // relapse cooldown

            var eligible = new List<BreakdownArcDef>();
            foreach (var a in _arcs)
                if (s.exposure_days >= a.minimum_stress_days)
                    eligible.Add(a);
            if (eligible.Count == 0) return;

            // Deterministic pick: draw over stably-ordered eligible arcs.
            int index = triggerRng.Next(0, eligible.Count);
            var chosen = eligible[index];

            s.arc_id = chosen.id;
            s.stage = (int)ArcStage.Emerging;
            s.treatment_progress = 0;
            s.catharsis_granted = false;
            OnBreakdownArcStarted?.Invoke(s.survivor_id, chosen.id);
        }

        private void TickActiveArc(
            int day, SurvivorArcState s, ISeededRng behaviorRng, ISeededRng recoveryRng,
            bool highStress, ArcStage before)
        {
            var def = Arc(s.arc_id);
            if (def == null) return;

            if ((ArcStage)s.stage == ArcStage.Recovering)
            {
                // Recovery holds without treatment only when stress stays low;
                // sustained high stress relapses one stage. Low stress plus the
                // recovery stream slowly self-heals toward resolution.
                if (highStress && s.exposure_days >= def.minimum_stress_days / 2)
                {
                    s.stage = (int)ArcStage.Acute;
                    OnBreakdownEscalated?.Invoke(s.survivor_id, s.arc_id, ArcStage.Recovering, ArcStage.Acute);
                    return;
                }
                if (!highStress && recoveryRng != null && recoveryRng.NextDouble() < 0.15)
                    ApplyTreatmentProgress(s.survivor_id, 1);
                return;
            }

            // Escalation: sustained exposure advances the arc every DaysPerStage.
            if (highStress)
            {
                int stagesElapsed = Math.Min(3, s.exposure_days / Math.Max(1, def.minimum_stress_days));
                ArcStage target = (ArcStage)Math.Min((int)ArcStage.Crisis, (int)ArcStage.Emerging + stagesElapsed);
                if (target > (ArcStage)s.stage)
                {
                    s.stage = (int)target;
                    OnBreakdownEscalated?.Invoke(s.survivor_id, s.arc_id, before, target);
                }
            }

            // Behavior opportunity: conditional, rare, cooldown-gated. Never
            // guaranteed by diagnosis (plan §2.9).
            if (day > s.behavior_cooldown_until_day
                && (ArcStage)s.stage >= (ArcStage)def.crisis_behavior_min_stage)
            {
                if (behaviorRng.NextDouble() < def.behavior_chance)
                {
                    s.behavior_cooldown_until_day = day + def.behavior_cooldown_days;
                    RunBehavior(day, s, def);
                }
            }
        }

        private void RunBehavior(int day, SurvivorArcState s, BreakdownArcDef def)
        {
            var behavior = MentalArcCatalogLoader.ParseBehavior(def.behavior);
            switch (behavior)
            {
                case ArcBehavior.StashTransfer:
                    var moved = TryTransferToStash?.Invoke(s.survivor_id, def.id, day);
                    if (moved != null && !string.IsNullOrEmpty(moved.Value.itemId) && moved.Value.count > 0)
                    {
                        s.stash.Add(new StashEntry
                        {
                            item_id = moved.Value.itemId,
                            count = moved.Value.count,
                            day = day,
                            discovered = false
                        });
                        OnStashTransferred?.Invoke(s.survivor_id, moved.Value.itemId, moved.Value.count);
                    }
                    // No eligible item / transfer refused: nothing vanishes.
                    break;

                case ArcBehavior.UnsafeFireRequest:
                    // The fire authority decides whether an incident actually
                    // ignites; this is a request, never an ignition.
                    OnUnsafeFireIncidentRequested?.Invoke(s.survivor_id, day);
                    break;

                case ArcBehavior.RefuseAssignment:
                case ArcBehavior.WithdrawSelfCare:
                    // Consequence applied by assignment/needs authorities.
                    break;
            }
            OnBreakdownBehaviorOccurred?.Invoke(s.survivor_id, behavior);
        }

        // ------------------------------------------------------------------
        // Treatment bridge (sanatorium → arc, via the host port)
        // ------------------------------------------------------------------

        /// <summary>Sanatorium therapy progress lands here. Enough progress
        /// resolves the arc and grants the bounded catharsis resilience.</summary>
        public void ApplyTreatmentProgress(string survivorId, int progress)
        {
            var s = ArcOf(survivorId);
            if (s == null || string.IsNullOrEmpty(s.arc_id)) return;
            if ((ArcStage)s.stage is ArcStage.Resolved or ArcStage.Latent) return;

            if ((ArcStage)s.stage != ArcStage.Recovering)
            {
                s.stage = (int)ArcStage.Recovering;
                OnBreakdownEscalated?.Invoke(s.survivor_id, s.arc_id, (ArcStage)s.stage, ArcStage.Recovering);
            }
            s.treatment_progress += Math.Max(0, progress);
            if (s.treatment_progress >= TreatmentProgressToResolve)
                ResolveArc(s);
        }

        /// <summary>Resolve with catharsis: bounded, non-stacking resilience.</summary>
        public void ResolveArc(SurvivorArcState s)
        {
            string arcId = s.arc_id;
            s.stage = (int)ArcStage.Resolved;
            s.resolved_day = _state.last_tick_day;
            s.arc_id = ""; // arc inactive; exposure accumulator stays for relapse context
            s.treatment_progress = 0;

            float bonus = _state.resilience_bonus.TryGetValue(s.survivor_id, out var v) ? v : 0f;
            if (!s.catharsis_granted)
            {
                s.catharsis_granted = true;
                bonus = Math.Min(MaxResilienceBonus, bonus + ResiliencePerCatharsis);
                _state.resilience_bonus[s.survivor_id] = bonus;
            }
            OnBreakdownResolved?.Invoke(s.survivor_id, arcId);
        }

        // ------------------------------------------------------------------
        // Stash intervention (auditability + conservation)
        // ------------------------------------------------------------------

        /// <summary>Search marks the stash discovered (returns the ledger for the
        /// host to move items back through the inventory, atomically).</summary>
        public List<StashEntry> DiscoverStash(string survivorId)
        {
            var s = ArcOf(survivorId);
            if (s == null || s.stash.Count == 0) return new List<StashEntry>();
            bool anyNew = false;
            foreach (var e in s.stash)
            {
                if (!e.discovered) { e.discovered = true; anyNew = true; }
            }
            if (anyNew) OnStashDiscovered?.Invoke(survivorId);
            return new List<StashEntry>(s.stash);
        }

        /// <summary>Host confirms items returned to the shared inventory; the
        /// ledger clears. Nothing has left campaign accounting at any point.</summary>
        public void ConfirmStashReturned(string survivorId)
        {
            var s = ArcOf(survivorId);
            if (s != null) s.stash.Clear();
        }

        // ------------------------------------------------------------------
        // Save
        // ------------------------------------------------------------------

        public PsychologicalArcState CaptureState()
        {
            var copy = new PsychologicalArcState
            {
                last_tick_day = _state.last_tick_day,
                resilience_bonus = new Dictionary<string, float>(_state.resilience_bonus, StringComparer.Ordinal),
                survivors = new List<SurvivorArcState>(_state.survivors.Count)
            };
            foreach (var s in _state.survivors)
                copy.survivors.Add(new SurvivorArcState
                {
                    survivor_id = s.survivor_id,
                    arc_id = s.arc_id,
                    stage = s.stage,
                    exposure_days = s.exposure_days,
                    behavior_cooldown_until_day = s.behavior_cooldown_until_day,
                    treatment_progress = s.treatment_progress,
                    catharsis_granted = s.catharsis_granted,
                    resolved_day = s.resolved_day,
                    stash = new List<StashEntry>(s.stash)
                });
            return copy;
        }

        public void RestoreState(PsychologicalArcState? state)
        {
            if (state == null) return;
            _state.last_tick_day = state.last_tick_day;
            _state.resilience_bonus = state.resilience_bonus != null
                ? new Dictionary<string, float>(state.resilience_bonus, StringComparer.Ordinal)
                : new Dictionary<string, float>(StringComparer.Ordinal);
            _state.survivors = new List<SurvivorArcState>(state.survivors?.Count ?? 0);
            if (state.survivors == null) return;
            foreach (var s in state.survivors)
                _state.survivors.Add(new SurvivorArcState
                {
                    survivor_id = s.survivor_id,
                    arc_id = s.arc_id,
                    stage = s.stage,
                    exposure_days = s.exposure_days,
                    behavior_cooldown_until_day = s.behavior_cooldown_until_day,
                    treatment_progress = s.treatment_progress,
                    catharsis_granted = s.catharsis_granted,
                    resolved_day = s.resolved_day,
                    stash = s.stash != null ? new List<StashEntry>(s.stash) : new List<StashEntry>()
                });
        }
    }
}
