using System;
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
    {
        public ChemicalDependencySystem Engine { get; }
        public VigilStateMachine Vigil { get; }

        public float TotalMoraleDrain { get; private set; }
        public float ActiveCraftingPenalty { get; private set; }
        public float ActiveCombatPenalty { get; private set; }

        public string LastEvent { get; private set; } = string.Empty;

        public event Action StateChanged;

        public MedicalHostSession(ChemicalDependencySystem engine = null!, VigilStateMachine vigil = null!)
        {
            Engine = engine ?? new ChemicalDependencySystem();
            Vigil = vigil ?? new VigilStateMachine();
            Engine.OnMoraleDrainRequested += (sv, amount) =>
            {
                TotalMoraleDrain += amount;
                StateChanged?.Invoke();
            };
            Engine.OnCraftingPenaltyChanged += (sv, factor) =>
            {
                ActiveCraftingPenalty = factor;
                StateChanged?.Invoke();
            };
            Engine.OnCombatPenaltyChanged += (sv, factor) =>
            {
                ActiveCombatPenalty = factor;
                StateChanged?.Invoke();
            };
            Engine.OnDependencyFormed += (sv, item) =>
            {
                LastEvent = $"Dependency formed: {sv} on {item}.";
                StateChanged?.Invoke();
            };
            Engine.OnDetoxCompleted += (sv, item) =>
            {
                LastEvent = $"Detox complete: {sv} clean of {item}.";
                StateChanged?.Invoke();
            };
            Engine.OnStateChanged += () => StateChanged?.Invoke();
            Vigil.OnVigilStarted += id => { LastEvent = $"Vigil begun for {id}."; StateChanged?.Invoke(); };
            Vigil.OnNameRecited += (name, count) => { LastEvent = $"Name recited: {name} ({count})"; StateChanged?.Invoke(); };
            Vigil.OnPhantomKnock += () => { LastEvent = "Phantom knock heard."; StateChanged?.Invoke(); };
            Vigil.OnVigilCompleted += skipped => { LastEvent = $"Vigil completed (skipped: {skipped})"; StateChanged?.Invoke(); };
        }

        public void AddCareEntry(string survivorId, string treatmentDetails)
        {
            LastEvent = $"Medical care: {survivorId} — {treatmentDetails}";
            StateChanged?.Invoke();
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
            return session;
        }

        // ── Demo actions ─────────────────────────────────────────────

        public string DoseDemo(string survivorId, string itemId, ChemicalDependencyKind kind)
        {
            Engine.OnSubstanceConsumed(survivorId, itemId, kind);
            return $"Registered one dose of {itemId} for {survivorId} " +
                   $"(level {Engine.DependencyLevel(survivorId, itemId):F2}).";
        }

        public string BeginDetoxDemo(string survivorId, string itemId, bool managed)
        {
            bool ok = managed
                ? Engine.BeginManagedDetox(survivorId, itemId)
                : Engine.BeginColdTurkey(survivorId, itemId);
            return ok ? $"Detox begun for {survivorId} ({itemId})." : "Detox refused (below threshold or unknown).";
        }

        public string TickDemo(float hours)
        {
            foreach (var sv in new System.Collections.Generic.List<string>(Engine.Ledger.Keys))
                Engine.TickHours(sv, hours);
            return $"Ticked {hours}h: morale drained {TotalMoraleDrain:F1}, " +
                   $"crafting penalty {ActiveCraftingPenalty:P0}, " +
                   $"combat penalty {ActiveCombatPenalty:P0}.";
        }

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

        public string StartVigilDemo(string dwellerId, string[] names)
        {
            Vigil.StartVigil(dwellerId, names);
            return $"Vigil begun for {dwellerId} ({names.Length} names, {Vigil.DurationSeconds}s).";
        }

        public string TickVigilDemo(float seconds)
        {
            Vigil.Tick(seconds);
            if (!Vigil.IsActive) return $"Vigil ended. Recited {Vigil.RecitedCount}/{Vigil.Names.Count} names.";
            return $"Vigil ticking: {Vigil.ElapsedSeconds:F0}/{Vigil.DurationSeconds:F0}s, " +
                   $"{Vigil.RecitedCount}/{Vigil.Names.Count} names recited.";
        }

        public string SkipVigilDemo()
        {
            if (!Vigil.IsActive) return "No active vigil.";
            Vigil.Skip();
            return "Vigil skipped.";
        }

        public string VigilStatusLine()
        {
            if (!Vigil.IsActive && !Vigil.IsCompleted) return "Vigil: idle";
            if (Vigil.IsCompleted) return $"Vigil: completed (skipped: {Vigil.WasSkipped})";
            return $"Vigil: {Vigil.DwellerId} · {Vigil.ElapsedSeconds:F0}/{Vigil.DurationSeconds:F0}s · " +
                   $"{Vigil.RecitedCount}/{Vigil.Names.Count} names" +
                   (Vigil.PhantomKnockFired ? " · phantom knock" : "");
        }

        public VigilSaveState CaptureVigilSave() => Vigil.CaptureState();
        public void RestoreVigilSave(VigilSaveState state) => Vigil.RestoreState(state);
    }
}
