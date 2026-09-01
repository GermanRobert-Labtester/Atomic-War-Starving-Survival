using System;
using System.Collections.Generic;
using System.Linq;
#pragma warning disable CS8618
using Ashfall.Core;
using Ashfall.Core.Medical;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Thin Godot-host session for the Medical port (Chemical Dependency).
    /// Wraps the core system, applies the effect events as host-side state
    /// (morale/penalty totals), and persists the ledger. No rules here —
    /// hosts only wire and present.
    /// </summary>
    public sealed class MedicalHostSession
    : HostSessionBase{
        public ChemicalDependencySystem Engine { get; }
        public VigilStateMachine Vigil { get; }

        /// <summary>
        /// Task #133 unified pipeline. Bound by the host after the inventory,
        /// survivors, and Phase-0 sessions exist (BindPipeline). Null until
        /// bound — unbound sessions (headless selftests) keep working unchanged.
        /// </summary>
        public MedicalPipelineCoordinator? Pipeline { get; private set; }

        public float TotalMoraleDrain { get; private set; }
        public float ActiveCraftingPenalty { get; private set; }
        public float ActiveCombatPenalty { get; private set; }

        public string LastEvent { get; private set; } = string.Empty;
        public MedicalHostSession(ChemicalDependencySystem engine = null!, VigilStateMachine vigil = null!)
        {
            Engine = engine ?? new ChemicalDependencySystem();
            Vigil = vigil ?? new VigilStateMachine();
            Engine.OnMoraleDrainRequested += (sv, amount) =>
            {
                TotalMoraleDrain += amount;
                RaiseStateChanged();
            };
            Engine.OnCraftingPenaltyChanged += (sv, factor) =>
            {
                ActiveCraftingPenalty = factor;
                RaiseStateChanged();
            };
            Engine.OnCombatPenaltyChanged += (sv, factor) =>
            {
                ActiveCombatPenalty = factor;
                RaiseStateChanged();
            };
            Engine.OnDependencyFormed += (sv, item) =>
            {
                LastEvent = $"Dependency formed: {sv} on {item}.";
                RaiseStateChanged();
            };
            Engine.OnDetoxCompleted += (sv, item) =>
            {
                LastEvent = $"Detox complete: {sv} clean of {item}.";
                RaiseStateChanged();
            };
            Engine.OnStateChanged += () => RaiseStateChanged();
            Vigil.OnVigilStarted += id => { LastEvent = $"Vigil begun for {id}."; RaiseStateChanged(); };
            Vigil.OnNameRecited += (name, count) => { LastEvent = $"Name recited: {name} ({count})"; RaiseStateChanged(); };
            Vigil.OnPhantomKnock += () => { LastEvent = "Phantom knock heard."; RaiseStateChanged(); };
            Vigil.OnVigilCompleted += skipped => { LastEvent = $"Vigil completed (skipped: {skipped})"; RaiseStateChanged(); };
        }

        public void AddCareEntry(string survivorId, string treatmentDetails)
        {
            LastEvent = $"Medical care: {survivorId} — {treatmentDetails}";
            RaiseStateChanged();
        }

        // ── Plan 60 / D6 — the bedside vigil ────────────────────────────

        /// <summary>
        /// The vigil is the one part of medicine that is allowed to run on real time,
        /// because the point of it is that the player spends some. What reaches the
        /// simulation is only whether it was kept, never how long it took, so the
        /// determinism rule holds (see <see cref="Ashfall.Core.Medical.VigilCare"/>).
        /// </summary>
        private Func<int>? _vigilDay;
        private Ashfall.Core.Flags.IFlagLedger? _vigilFlags;
        private Func<IReadOnlyList<string>>? _vigilNames;
        private bool _vigilBound;

        public bool VigilActive => Vigil != null && Vigil.IsActive;

        /// <summary>0..1 presence of the vigil in progress, for the bedside UI only.</summary>
        public float VigilProgress =>
            Vigil == null || Vigil.DurationSeconds <= 0f ? 0f
            : Math.Clamp(Vigil.ElapsedSeconds / Vigil.DurationSeconds, 0f, 1f);

        /// <summary>
        /// Bind the day, the consequence ledger the record rides in, and the names worth
        /// reciting (the dead the holdfast has kept). Unbound, a vigil can be started
        /// but is never recorded — a ward with no ledger must not silently pretend.
        /// </summary>
        public void BindVigilContext(
            Func<int> dayProvider,
            Ashfall.Core.Flags.IFlagLedger? flags,
            Func<IReadOnlyList<string>>? namesProvider = null)
        {
            _vigilDay = dayProvider;
            _vigilFlags = flags;
            _vigilNames = namesProvider;
            if (_vigilBound) return;
            _vigilBound = true;

            Vigil.OnVigilCompleted += skipped =>
            {
                if (skipped) return;
                string id = Vigil.DwellerId;
                if (string.IsNullOrEmpty(id) || _vigilFlags == null) return;
                Ashfall.Core.Medical.VigilCare.RecordKept(
                    _vigilFlags, id, _vigilDay?.Invoke() ?? 0);
                RaiseStateChanged();
            };
        }

        /// <summary>
        /// Sit with someone who is dying. Returns a host-readable line; refusal is
        /// spoken, not swallowed, because a second vigil at once is a design answer and
        /// not a silent button that does nothing.
        /// </summary>
        public string HoldVigil(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return "No patient named for the vigil.";
            if (Vigil.IsActive) return $"A vigil is already kept for {Vigil.DwellerId}.";

            Vigil.StartVigil(survivorId, _vigilNames?.Invoke() ?? Array.Empty<string>());
            RaiseStateChanged();
            return $"Vigil begun for {survivorId}. Sit with them.";
        }

        /// <summary>Advance the vigil by real elapsed time. Presence only — see above.</summary>
        public void TickVigil(double deltaSeconds)
        {
            if (Vigil == null || !Vigil.IsActive) return;
            if (deltaSeconds <= 0d) return;
            Vigil.Tick((float)deltaSeconds);
        }


        public static MedicalHostSession Create(string dataDir)
        {
            var session = new MedicalHostSession();
            var save = MedicalSaveStore.TryLoad();
            if (save != null)
            {
                session.Engine.RestoreState(save);
                session.LastEvent = "Medical ledger restored from save.";
            }

            // Task #133 chem-dep authority merge: the `medical` section is the
            // canonical ledger. Rows that only exist in the legacy
            // `chemical_dependency` section are merged in; the medical section
            // wins on survivor+item conflicts. Both sections stay in sync on
            // save (they capture the same shared engine), so this merge is
            // idempotent and migration-safe in both directions.
            var legacy = ChemicalDependencySaveStore.TryLoad();
            if (legacy != null)
            {
                int merged = 0;
                foreach (var svList in legacy.survivors)
                {
                    if (svList == null || string.IsNullOrEmpty(svList.survivorId)) continue;
                    var existing = session.Engine.Ledger.TryGetValue(svList.survivorId, out var deps)
                        ? deps : null;
                    foreach (var dep in svList.dependencies)
                    {
                        if (dep == null || string.IsNullOrEmpty(dep.itemId)) continue;
                        bool present = existing != null && existing.Any(d =>
                            string.Equals(d.itemId, dep.itemId, StringComparison.Ordinal));
                        if (present) continue;
                        session.Engine.OnSubstanceConsumed(svList.survivorId, dep.itemId,
                            ParseDependencyKind(dep.kind));
                        // OnSubstanceConsumed starts at one dose; adopt the saved level.
                        var row = session.Engine.Ledger[svList.survivorId]
                            .First(d => string.Equals(d.itemId, dep.itemId, StringComparison.Ordinal));
                        row.dependencyLevel = dep.dependencyLevel;
                        row.inManagedDetox = dep.inManagedDetox;
                        row.inColdTurkey = dep.inColdTurkey;
                        row.detoxProgressHours = dep.detoxProgressHours;
                        merged++;
                    }
                }
                if (merged > 0)
                    session.LastEvent = $"Medical ledger restored; {merged} legacy dependency row(s) merged.";
            }

            var pipelineSave = MedicalPipelineSaveStore.TryLoad();
            if (pipelineSave != null)
            {
                // Pipeline bind happens later (BindPipeline); stash the save so
                // the coordinator restores it when the host completes wiring.
                session._pendingPipelineSave = pipelineSave;
            }
            return session;
        }

        private MedicalPipelineSaveState? _pendingPipelineSave;

        private static ChemicalDependencyKind ParseDependencyKind(string? kind)
        {
            return kind switch
            {
                "Alcohol" => ChemicalDependencyKind.Alcohol,
                "Stimulant" => ChemicalDependencyKind.Stimulant,
                "Sedative" => ChemicalDependencyKind.Sedative,
                _ => ChemicalDependencyKind.Opioid
            };
        }

        /// <summary>
        /// Bind the Task #133 pipeline (built by Main once inventory, survivors,
        /// and Phase-0 exist) and restore its save slice. Idempotent.
        /// </summary>
        public void BindPipeline(MedicalPipelineCoordinator pipeline)
        {
            if (Pipeline != null) return;
            Pipeline = pipeline ?? throw new ArgumentNullException(nameof(pipeline));
            if (_pendingPipelineSave != null)
            {
                Pipeline.RestoreState(_pendingPipelineSave);
                _pendingPipelineSave = null;
            }
            Pipeline.StateChanged += () => RaiseStateChanged();
        }

        /// <summary>Pipeline save capture for the campaign envelope.</summary>
        public MedicalPipelineSaveState? CapturePipelineSave() => Pipeline?.CaptureState();

        // ── Demo actions ─────────────────────────────────────────────

        public string BeginDetoxDemo(string survivorId, string itemId, bool managed)
        {
            bool ok = managed
                ? Engine.BeginManagedDetox(survivorId, itemId)
                : Engine.BeginColdTurkey(survivorId, itemId);
            return ok ? $"Detox begun for {survivorId} ({itemId})." : "Detox refused (below threshold or unknown).";
        }

        // ── Production Runtime Actions ───────────────────────────────
        public void TickHours(float hours)
        {
            foreach (var sv in new System.Collections.Generic.List<string>(Engine.Ledger.Keys))
                Engine.TickHours(sv, hours);
        }

        // ── Demo actions ─────────────────────────────────────────────

        public string StatusLine()
        {
            var sb = new System.Text.StringBuilder();
            sb.Append("Chemical dependencies: ");
            int count = 0;
            foreach (var kv in Engine.Ledger) count += kv.Value.Count;
            sb.Append(count).Append('\n');
            foreach (var kv in Engine.Ledger)
            {
                foreach (var d in kv.Value)
                {
                    sb.Append("  ").Append(kv.Key).Append(" — ").Append(d.itemId)
                      .Append(" level ").Append(d.dependencyLevel.ToString("F2"))
                      .Append(d.inManagedDetox ? " [managed detox]" : d.inColdTurkey ? " [cold turkey]" : "")
                      .Append('\n');
                }
            }
            return sb.ToString().TrimEnd();
        }

        // ── Save / Load ──────────────────────────────────────────────

        public ChemicalDependencyLedgerState CaptureSave() => Engine.CaptureState();
        public void RestoreSave(ChemicalDependencyLedgerState state) => Engine.RestoreState(state);

        // ── Vigil (Exp 07) ──────────────────────────────────────────

        public string SkipVigilDemo()
        {
            if (!Vigil.IsActive) return "No active vigil.";
            Vigil.Skip();
            return "Vigil skipped.";
        }

        public string VigilStatusLine()
        {
            if (!Vigil.IsActive && !Vigil.IsCompleted) return "Vigil: idle";
            if (Vigil.IsCompleted)
                return Vigil.WasSkipped
                    ? "Vigil: left early"
                    : $"Vigil: kept to the end for {Vigil.DwellerId}";
            return $"Vigil: {Vigil.DwellerId} · {Vigil.ElapsedSeconds:F0}/{Vigil.DurationSeconds:F0}s · " +
                   $"{Vigil.RecitedCount}/{Vigil.Names.Count} names" +
                   (Vigil.PhantomKnockFired ? " · phantom knock" : "");
        }

        public VigilSaveState CaptureVigilSave() => Vigil.CaptureState();
        public void RestoreVigilSave(VigilSaveState state) => Vigil.RestoreState(state);
    }
}
