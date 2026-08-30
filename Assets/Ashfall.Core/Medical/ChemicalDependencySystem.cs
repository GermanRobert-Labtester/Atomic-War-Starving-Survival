using System;
using System.Collections.Generic;
#pragma warning disable CS8618
using Ashfall.Core.PlayerCommand;

namespace Ashfall.Core.Medical
{
    public enum ChemicalDependencyKind
    {
        Opioid,
        Alcohol,
        Stimulant,
        Sedative
    }

    /// <summary>Serialized dependency of one survivor on one substance.</summary>
    [Serializable]
    public class ChemicalDependencyState
    {
        public string itemId = string.Empty;
        public float dependencyLevel = 0f;      // 0..1
        public string kind = "Opioid";
        public bool inManagedDetox = false;
        public bool inColdTurkey = false;       // Unity abused progress<0; we flag it (see migration notes)
        public float detoxProgressHours = 0f;
    }

    /// <summary>Serialized snapshot of every survivor's dependencies.</summary>
    [Serializable]
    public class ChemicalDependencyLedgerState
    {
        public string systemId = ChemicalDependencySystem.SystemId;
        public List<SurvivorDependencyList> survivors = new List<SurvivorDependencyList>();
    }

    [Serializable]
    public class SurvivorDependencyList
    {
        public string survivorId = string.Empty;
        public List<ChemicalDependencyState> dependencies = new List<ChemicalDependencyState>();
    }

    /// <summary>
    /// Engine-agnostic port of the Unity ChemicalDependencySystem
    /// (Assets/_Game/Medical/ChemicalDependencySystem.cs): substance-specific
    /// dependency tracking for opioids, alcohol, stimulants and sedatives,
    /// with managed detox and cold-turkey withdrawal profiles. Survivor-
    /// agnostic (operates on ids); the host subscribes to the effect events
    /// to apply morale/penalties in its own domain. All constants match the
    /// Unity source 1:1. Save/load per the house pattern.
    /// </summary>
    public class ChemicalDependencySystem
    {
        public const string SystemId = "chemical_dependency_system";

        public const float DependencyThreshold = 0.3f;
        public const float DependencyIncreasePerDose = 0.15f;
        public const float DependencyDecayPerDayClean = 0.05f;
        public const float MaxDependencyLevel = 1f;

        public const float ColdTurkeyWithdrawalDurationHours = 72f;
        public const float ManagedDetoxDurationHours = 120f;
        public const float ColdTurkeyTremorCraftingPenalty = 0.40f;
        public const float ColdTurkeyTremorCombatPenalty = 0.30f;
        public const float ColdTurkeyMoraleDrainPerHour = 3f;
        public const float ManagedDetoxMoraleDrainPerHour = 1f;
        public const float DetoxSuccessThresholdHours = 96f;

        public static readonly Dictionary<ChemicalDependencyKind, float> KindBaseSeverity =
            new Dictionary<ChemicalDependencyKind, float>
            {
                { ChemicalDependencyKind.Opioid, 0.9f },
                { ChemicalDependencyKind.Alcohol, 0.7f },
                { ChemicalDependencyKind.Stimulant, 0.6f },
                { ChemicalDependencyKind.Sedative, 0.5f }
            };

        private readonly Dictionary<string, List<ChemicalDependencyState>> _ledger =
            new Dictionary<string, List<ChemicalDependencyState>>();

        // ── Events (hosts apply the effects in their own domain) ──────
        public event Action<string, string> OnDependencyFormed;        // survivorId, itemId
        public event Action<string, string> OnWithdrawalStarted;       // survivorId, itemId
        public event Action<string, string> OnDetoxCompleted;          // survivorId, itemId
        public event Action<string, string> OnDetoxFailed;             // survivorId, itemId
        public event Action<string, float> OnMoraleDrainRequested;     // survivorId, amount
        public event Action<string, float> OnCraftingPenaltyChanged;   // survivorId, factor
        public event Action<string, float> OnCombatPenaltyChanged;     // survivorId, factor
        // Plan 09 9B — stress-driven relapse hook. Fired exactly once per
        // ReportStress(...) call, regardless of how many dependencies were
        // actually nudged. Lets subscribers (telemetry, narrative, audio)
        // observe the input independently of the system's internal nudges.
        public event Action<string, string, float> OnStressReported;   // survivorId, source, magnitude
        // Fired when a dependencyLevel bumped by stress crosses back up to
        // >= DependencyThreshold from below — analogous to OnDependencyFormed
        // but distinguishable by source of the trigger.
        public event Action<string, string, ChemicalDependencyKind> OnDependencyReFormedByStress;
        public event Action OnStateChanged;

        public IReadOnlyDictionary<string, List<ChemicalDependencyState>> Ledger => _ledger;

        // ── Consumption ───────────────────────────────────────────────

        public void OnSubstanceConsumed(string survivorId, string itemId, ChemicalDependencyKind kind)
        {
            if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(itemId)) return;

            var deps = GetOrCreate(survivorId);
            var dep = Find(deps, itemId);
            if (dep == null)
            {
                deps.Add(new ChemicalDependencyState
                {
                    itemId = itemId,
                    dependencyLevel = DependencyIncreasePerDose,
                    kind = kind.ToString()
                });
                RaiseChanged();
                return;
            }

            dep.dependencyLevel = Math.Min(MaxDependencyLevel, dep.dependencyLevel + DependencyIncreasePerDose);
            if (dep.dependencyLevel >= DependencyThreshold && !dep.inManagedDetox)
                OnDependencyFormed?.Invoke(survivorId, itemId);
            RaiseChanged();
        }

        // ── Stress-driven relapse (Plan 09 9B Core) ──────────────

        /// <summary>
        /// Report an external stress source for a survivor (guilt spike,
        /// combat trauma ration cut, etc). Each dependency currently carried
        /// is nudged by a per-kind, magnitude-clamped delta; survivors in
        /// managed detox or cold-turkey are left alone because the withdrawal
        /// path is its own state machine. Returns the number of dependencies
        /// the call actually bumped, so callers can route follow-up effects
        /// (morale drainage, narrative beats) only when something moved.
        /// </summary>
        /// <remarks>
        /// This call is idempotent re: state-change notifications: no mutation
        /// means no <see cref="OnStateChanged"/>; the input-echo
        /// <see cref="OnStressReported"/> always fires once so telemetry does
        /// not have to special-case empty ledgers.
        /// </remarks>
        public int ReportStress(string survivorId, string source, float magnitude)
        {
            // Always echo the input once.
            OnStressReported?.Invoke(
                survivorId ?? string.Empty,
                source ?? string.Empty,
                magnitude);

            if (string.IsNullOrEmpty(survivorId)) return 0;
            if (!_ledger.TryGetValue(survivorId, out var deps) || deps.Count == 0)
                return 0;

            int nudged = 0;
            for (int i = 0; i < deps.Count; i++)
            {
                var dep = deps[i];
                // Detox owns the survivor's mental state machine; do not
                // double-count stress as dependency growth — the withdrawal
                // log already drains morale.
                if (dep.inColdTurkey || dep.inManagedDetox) continue;
                if (dep.dependencyLevel <= 0f) continue;

                ChemicalDependencyKind kind = ChemicalDependencyKind.Opioid;
                if (Enum.TryParse(dep.kind, out ChemicalDependencyKind parsed)) kind = parsed;

                float delta = StressRelapseRules.ComputeDelta(magnitude, kind);
                if (delta <= 0f) continue;

                float before = dep.dependencyLevel;
                float after = Math.Min(MaxDependencyLevel, before + delta);
                if (after <= before) continue; // saturated — nothing moved
                dep.dependencyLevel = after;
                nudged++;

                // Cross-up event: dependency just re-formed from a stressed
                // clean baseline. Distinct from OnDependencyFormed so narrative
                // + audio can tell "first relapse after a bad week" from
                // "first dose taken this morning".
                if (before < DependencyThreshold && after >= DependencyThreshold)
                    OnDependencyReFormedByStress?.Invoke(survivorId, dep.itemId, kind);
            }
            if (nudged > 0) RaiseChanged();
            return nudged;
        }

        // ── Detox programs ────────────────────────────────────────────

        public bool BeginManagedDetox(string survivorId, string itemId)
        {
            var dep = Require(survivorId, itemId);
            if (dep == null) return false;
            if (dep.dependencyLevel < DependencyThreshold) return false;
            dep.inManagedDetox = true;
            dep.inColdTurkey = false; // switching programs leaves cold-turkey mode
            dep.detoxProgressHours = 0f;
            OnWithdrawalStarted?.Invoke(survivorId, itemId);
            RaiseChanged();
            return true;
        }

        public bool BeginColdTurkey(string survivorId, string itemId)
        {
            var dep = Require(survivorId, itemId);
            if (dep == null) return false;
            if (dep.dependencyLevel < DependencyThreshold) return false;
            dep.inManagedDetox = false;
            dep.inColdTurkey = true;
            dep.detoxProgressHours = 0f;
            OnWithdrawalStarted?.Invoke(survivorId, itemId);
            RaiseChanged();
            return true;
        }

        /// <summary>
        /// Side-effect-free preview of a managed detox start command.
        /// Shares the same validation path as <see cref="BeginManagedDetox"/>.
        /// </summary>
        public CommandPreview PreviewBeginManagedDetox(string survivorId, string itemId, long stateVersion = 0)
        {
            var dep = Require(survivorId, itemId);
            if (dep == null)
                return CommandPreview.Unavailable(PlayerCommandCode.TreatmentStart, "missing_dependency", "medical.missing_dependency", stateVersion);
            if (dep.dependencyLevel < DependencyThreshold)
                return CommandPreview.Unavailable(PlayerCommandCode.TreatmentStart, "below_threshold", "medical.below_threshold", stateVersion);
            if (dep.inManagedDetox)
                return CommandPreview.Unavailable(PlayerCommandCode.TreatmentStart, "already_in_treatment", "medical.already_in_treatment", stateVersion);

            var deltas = new Dictionary<string, double>
            {
                { "dependency_level", -dep.dependencyLevel }
            };

            return CommandPreview.Available(
                PlayerCommandCode.TreatmentStart,
                stateVersion,
                deltas,
                isIrreversible: false,
                messageKey: "medical.preview_managed_detox");
        }

        /// <summary>
        /// Execute a managed detox start using the same validation path as <see cref="PreviewBeginManagedDetox"/>.
        /// Stale previews (state version mismatch) are rejected without mutation.
        /// </summary>
        public CommandResult ExecuteBeginManagedDetox(string survivorId, string itemId, long expectedStateVersion = 0, long currentStateVersion = 0)
        {
            var preview = PreviewBeginManagedDetox(survivorId, itemId, expectedStateVersion);
            if (!preview.IsAvailable)
                return CommandResult.FromPreview(preview);

            if (preview.StateVersion != currentStateVersion)
                return CommandResult.StalePreview(PlayerCommandCode.TreatmentStart, preview.StateVersion, currentStateVersion);

            bool ok = BeginManagedDetox(survivorId, itemId);
            if (!ok)
                return new CommandResult(
                    PlayerCommandCode.TreatmentStart,
                    ActionResult.Failed("execute_failed", "medical.execute_failed"),
                    expectedStateVersion,
                    currentStateVersion);

            var dep = Require(survivorId, itemId);
            var deltas = new Dictionary<string, double>();
            if (dep != null)
                deltas["dependency_level"] = -dep.dependencyLevel;

            return CommandResult.FromSuccess(
                PlayerCommandCode.TreatmentStart,
                ActionResult.Success("medical.managed_detox_started", deltas),
                expectedStateVersion,
                currentStateVersion + 1);
        }

        /// <summary>
        /// Side-effect-free preview of a cold-turkey start command.
        /// Shares the same validation path as <see cref="BeginColdTurkey"/>.
        /// </summary>
        public CommandPreview PreviewBeginColdTurkey(string survivorId, string itemId, long stateVersion = 0)
        {
            var dep = Require(survivorId, itemId);
            if (dep == null)
                return CommandPreview.Unavailable(PlayerCommandCode.TreatmentStart, "missing_dependency", "medical.missing_dependency", stateVersion);
            if (dep.dependencyLevel < DependencyThreshold)
                return CommandPreview.Unavailable(PlayerCommandCode.TreatmentStart, "below_threshold", "medical.below_threshold", stateVersion);
            if (dep.inColdTurkey)
                return CommandPreview.Unavailable(PlayerCommandCode.TreatmentStart, "already_in_treatment", "medical.already_in_treatment", stateVersion);

            var deltas = new Dictionary<string, double>
            {
                { "dependency_level", -dep.dependencyLevel }
            };

            return CommandPreview.Available(
                PlayerCommandCode.TreatmentStart,
                stateVersion,
                deltas,
                isIrreversible: false,
                messageKey: "medical.preview_cold_turkey");
        }

        /// <summary>
        /// Execute a cold-turkey start using the same validation path as <see cref="PreviewBeginColdTurkey"/>.
        /// Stale previews (state version mismatch) are rejected without mutation.
        /// </summary>
        public CommandResult ExecuteBeginColdTurkey(string survivorId, string itemId, long expectedStateVersion = 0, long currentStateVersion = 0)
        {
            var preview = PreviewBeginColdTurkey(survivorId, itemId, expectedStateVersion);
            if (!preview.IsAvailable)
                return CommandResult.FromPreview(preview);

            if (preview.StateVersion != currentStateVersion)
                return CommandResult.StalePreview(PlayerCommandCode.TreatmentStart, preview.StateVersion, currentStateVersion);

            bool ok = BeginColdTurkey(survivorId, itemId);
            if (!ok)
                return new CommandResult(
                    PlayerCommandCode.TreatmentStart,
                    ActionResult.Failed("execute_failed", "medical.execute_failed"),
                    expectedStateVersion,
                    currentStateVersion);

            var dep = Require(survivorId, itemId);
            var deltas = new Dictionary<string, double>();
            if (dep != null)
                deltas["dependency_level"] = -dep.dependencyLevel;

            return CommandResult.FromSuccess(
                PlayerCommandCode.TreatmentStart,
                ActionResult.Success("medical.cold_turkey_started", deltas),
                expectedStateVersion,
                currentStateVersion + 1);
        }

        // ── Tick ──────────────────────────────────────────────────────

        public void TickHours(string survivorId, float gameHours)
        {
            if (string.IsNullOrEmpty(survivorId) || gameHours <= 0f) return;
            if (!_ledger.TryGetValue(survivorId, out var deps)) return;

            for (int i = deps.Count - 1; i >= 0; i--)
            {
                var dep = deps[i];

                if (dep.inColdTurkey)
                {
                    // Cold turkey active (Unity's progress<0 sentinel is legacy-only;
                    // the port flags it explicitly so withdrawal survives more than
                    // one tick — the 72h completion actually fires).
                    float moraleDrain = ColdTurkeyMoraleDrainPerHour * gameHours *
                        Severity(dep.kind);
                    OnMoraleDrainRequested?.Invoke(survivorId, moraleDrain);
                    OnCraftingPenaltyChanged?.Invoke(survivorId, ColdTurkeyTremorCraftingPenalty);
                    OnCombatPenaltyChanged?.Invoke(survivorId, ColdTurkeyTremorCombatPenalty);

                    dep.detoxProgressHours += gameHours;
                    if (dep.detoxProgressHours >= ColdTurkeyWithdrawalDurationHours)
                        CompleteDetox(survivorId, deps, i, dep);
                }
                else if (dep.inManagedDetox)
                {
                    float moraleDrain = ManagedDetoxMoraleDrainPerHour * gameHours *
                        Severity(dep.kind);
                    OnMoraleDrainRequested?.Invoke(survivorId, moraleDrain);

                    dep.detoxProgressHours += gameHours;
                    if (dep.detoxProgressHours >= DetoxSuccessThresholdHours)
                        CompleteDetox(survivorId, deps, i, dep);
                }
                else if (dep.dependencyLevel > 0f)
                {
                    // Natural decay when clean.
                    dep.dependencyLevel = Math.Max(0f,
                        dep.dependencyLevel - DependencyDecayPerDayClean * (gameHours / 24f));
                    if (dep.dependencyLevel <= 0f)
                    {
                        deps.RemoveAt(i);
                        OnDetoxCompleted?.Invoke(survivorId, dep.itemId);
                    }
                }
            }
            RaiseChanged();
        }

        private void CompleteDetox(string survivorId, List<ChemicalDependencyState> deps, int index, ChemicalDependencyState dep)
        {
            dep.dependencyLevel = Math.Max(0f, dep.dependencyLevel - 0.5f);
            dep.detoxProgressHours = 0f;
            dep.inManagedDetox = false;
            dep.inColdTurkey = false;
            OnCraftingPenaltyChanged?.Invoke(survivorId, 0f); // clear penalties
            OnCombatPenaltyChanged?.Invoke(survivorId, 0f);

            if (dep.dependencyLevel < DependencyThreshold)
            {
                deps.RemoveAt(index);
                OnDetoxCompleted?.Invoke(survivorId, dep.itemId);
            }
            else
            {
                OnDetoxFailed?.Invoke(survivorId, dep.itemId);
            }
        }

        // ── Queries ───────────────────────────────────────────────────

        public bool HasActiveWithdrawal(string survivorId)
        {
            if (!_ledger.TryGetValue(survivorId, out var deps)) return false;
            for (int i = 0; i < deps.Count; i++)
            {
                if (deps[i].detoxProgressHours != 0f || deps[i].inManagedDetox || deps[i].inColdTurkey)
                    return true;
            }
            return false;
        }

        public float DependencyLevel(string survivorId, string itemId)
        {
            var dep = Require(survivorId, itemId);
            return dep != null ? dep.dependencyLevel : 0f;
        }

        public IReadOnlyList<ChemicalDependencyState> DependenciesFor(string survivorId)
        {
            return _ledger.TryGetValue(survivorId, out var deps) ? deps : EmptyList;
        }

        private static readonly List<ChemicalDependencyState> EmptyList = new List<ChemicalDependencyState>();

        private static float Severity(string kind)
        {
            if (Enum.TryParse(kind, out ChemicalDependencyKind k))
                return KindBaseSeverity.GetValueOrDefault(k, 0.5f);
            return 0.5f;
        }

        private static ChemicalDependencyState? Find(List<ChemicalDependencyState> deps, string itemId)
        {
            for (int i = 0; i < deps.Count; i++)
                if (string.Equals(deps[i].itemId, itemId, StringComparison.Ordinal))
                    return deps[i];
            return null;
        }

        private ChemicalDependencyState? Require(string survivorId, string itemId)
        {
            if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(itemId)) return null;
            if (!_ledger.TryGetValue(survivorId, out var deps)) return null;
            return Find(deps, itemId);
        }

        private List<ChemicalDependencyState> GetOrCreate(string survivorId)
        {
            if (!_ledger.TryGetValue(survivorId, out var deps))
            {
                deps = new List<ChemicalDependencyState>();
                _ledger[survivorId] = deps;
            }
            return deps;
        }

        // ── Save / Load ───────────────────────────────────────────────

        public ChemicalDependencyLedgerState CaptureState()
        {
            var copy = new ChemicalDependencyLedgerState { systemId = SystemId };
            var ids = new List<string>(_ledger.Keys);
            ids.Sort(string.CompareOrdinal);
            for (int i = 0; i < ids.Count; i++)
            {
                var src = _ledger[ids[i]];
                var list = new SurvivorDependencyList { survivorId = ids[i] };
                var ordered = new List<ChemicalDependencyState>(src);
                ordered.Sort((a, b) => string.CompareOrdinal(a.itemId, b.itemId));
                for (int j = 0; j < ordered.Count; j++)
                {
                    var d = ordered[j];
                    list.dependencies.Add(new ChemicalDependencyState
                    {
                        itemId = d.itemId,
                        dependencyLevel = Math.Clamp(d.dependencyLevel, 0f, MaxDependencyLevel),
                        kind = d.kind,
                        inManagedDetox = d.inManagedDetox,
                        inColdTurkey = d.inColdTurkey,
                        detoxProgressHours = d.detoxProgressHours
                    });
                }
                copy.survivors.Add(list);
            }
            return copy;
        }

        public void RestoreState(ChemicalDependencyLedgerState saved)
        {
            _ledger.Clear();
            if (saved == null) return;
            for (int i = 0; i < saved.survivors.Count; i++)
            {
                var s = saved.survivors[i];
                if (s == null || string.IsNullOrEmpty(s.survivorId)) continue;
                var deps = new List<ChemicalDependencyState>();
                if (s.dependencies != null)
                {
                    for (int j = 0; j < s.dependencies.Count; j++)
                    {
                        var d = s.dependencies[j];
                        if (d == null || string.IsNullOrEmpty(d.itemId)) continue;
                        deps.Add(new ChemicalDependencyState
                        {
                            itemId = d.itemId,
                            dependencyLevel = Math.Clamp(d.dependencyLevel, 0f, MaxDependencyLevel),
                            kind = string.IsNullOrEmpty(d.kind) ? nameof(ChemicalDependencyKind.Opioid) : d.kind,
                            inManagedDetox = d.inManagedDetox,
                            // Legacy Unity-style saves marked cold turkey with progress < 0.
                            inColdTurkey = d.inColdTurkey || d.detoxProgressHours < 0f,
                            detoxProgressHours = d.detoxProgressHours < 0f ? 0f : d.detoxProgressHours
                        });
                    }
                }
                _ledger[s.survivorId] = deps;
            }
            RaiseChanged();
        }

        private void RaiseChanged() => OnStateChanged?.Invoke();
    }
}
