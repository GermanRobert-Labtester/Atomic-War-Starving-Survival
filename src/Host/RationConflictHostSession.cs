// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : RationConflictSaveStore
// Core State : Ashfall.Core.Survivors.RationConflictSaveState
// Host Caller: Main.RationConflict
// Purpose    : Per-survivor perceived fairness and resentment accrued from
//              unequal ration allocations. ResourceRationingSystem stays the
//              sole allocation authority; NeedsSystem stays the sole morale
//              authority; SurvivorRelationsSystem stays the sole affinity owner.
// ============================================================================

using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Ashfall.Core.Save;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public static class RationConflictSaveStore
    {
        public const string FileName = "ration_conflict_save.json";
        public const string SectionName = "ration_conflict";

        private static readonly SaveStore<RationConflictSaveState> s_store =
            SaveStoreHub.Checksummed<RationConflictSaveState>(FileName, nameof(RationConflictSaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapturePersisted(RationConflictSaveState state) => s_store.CaptureBare(state);
        public static RationConflictSaveState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(RationConflictSaveState state) => s_store.TrySave(state);
        public static RationConflictSaveState? TryLoad() => s_store.TryLoad();
    }

    /// <summary>
    /// Host session over the sealed <see cref="RationConflictSystem"/>.
    ///
    /// <para><b>Authority boundary.</b> <c>ResourceRationingSystem</c> owns how
    /// much each survivor is allocated; <c>NeedsSystem</c> owns morale;
    /// <c>SurvivorRelationsSystem</c> owns pair affinity. This session owns only
    /// perceived fairness and resentment.</para>
    ///
    /// <para><b>Determinism.</b> One injected, campaign-forked RNG draw per theft
    /// attempt; no wall-clock seed and no iteration-order dependence.</para>
    /// </summary>
    public sealed class RationConflictHostSession : HostSessionBase
    {
        private readonly RationConflictSystem _engine;
        private readonly Func<Ashfall.Core.Economy.ResourceRationingSystem?> _rationingProvider;
        private readonly Func<Ashfall.Core.Survivors.NeedsSystem?> _needsProvider;
        private readonly Func<Ashfall.Core.SurvivorRelationsSystem?> _relationsProvider;

        public string LastEvent { get; private set; } = string.Empty;
        public RationConflictSystem Engine => _engine;

        public RationConflictHostSession(
            RationConflictSystem engine,
            Func<Ashfall.Core.Economy.ResourceRationingSystem?> rationingProvider,
            Func<Ashfall.Core.Survivors.NeedsSystem?> needsProvider,
            Func<Ashfall.Core.SurvivorRelationsSystem?> relationsProvider,
            RationConflictSaveState? savedState = null)
        {
            _engine = engine ?? throw new ArgumentNullException(nameof(engine));
            _rationingProvider = rationingProvider ?? throw new ArgumentNullException(nameof(rationingProvider));
            _needsProvider = needsProvider ?? throw new ArgumentNullException(nameof(needsProvider));
            _relationsProvider = relationsProvider ?? throw new ArgumentNullException(nameof(relationsProvider));

            if (savedState != null) _engine.RestoreState(savedState);

            _engine.OnMoraleDelta += ApplyMorale;
            _engine.OnRationConfrontation += RecordConfrontation;
            _engine.OnRationsStolen += RecordTheft;
        }

        /// <summary>
        /// Projects the LIVE rationing owner's per-survivor allocation multiplier
        /// into the conflict engine. No ration tier is recomputed here.
        /// </summary>
        public int SyncAllocationsFromRationing()
        {
            var rationing = _rationingProvider();
            if (rationing == null) return 0;

            // The rationing owner exposes its assignments through its own state
            // projection; this reads them, it never recomputes a tier.
            var state = rationing.CaptureState();
            int synced = 0;
            foreach (var assignment in state.Assignments)
            {
                if (assignment == null || string.IsNullOrWhiteSpace(assignment.SurvivorId)) continue;
                float bonus = assignment.PriorityBonus > 0f ? assignment.PriorityBonus : 1f;
                _engine.SetAllocation(assignment.SurvivorId, bonus);
                RegisterSurvivor(assignment.SurvivorId);
                synced++;
            }
            if (synced > 0) RaiseStateChanged();
            return synced;
        }

        private readonly System.Collections.Generic.HashSet<string> _registered =
            new System.Collections.Generic.HashSet<string>(StringComparer.Ordinal);

        public void RegisterSurvivor(string survivorId)
        {
            if (string.IsNullOrWhiteSpace(survivorId)) return;
            if (_registered.Add(survivorId.Trim())) _engine.RegisterSurvivor(survivorId.Trim());
        }

        public System.Collections.Generic.IReadOnlyCollection<string> RegisteredSurvivors => _registered;

        private void ApplyMorale(string survivorId, float delta, string source)
        {
            var needs = _needsProvider();
            if (needs == null || string.IsNullOrWhiteSpace(survivorId)) return;
            // Exactly once per engine event, through the canonical morale owner.
            needs.Modify(survivorId, Ashfall.Core.Survivors.NeedKind.Morale, delta);
            LastEvent = $"Ration conflict: {source} → {survivorId} ({delta:+0;-0} morale).";
        }

        private void RecordConfrontation(string resenterId, string targetId)
        {
            var relations = _relationsProvider();
            if (relations == null || string.IsNullOrWhiteSpace(targetId)) return;
            // Pair affinity has exactly one producer; this is not a second one.
            relations.ModifyAffinity(resenterId, targetId, -4f);
        }

        private void RecordTheft(string thiefId, string victimId)
        {
            var relations = _relationsProvider();
            if (relations == null || string.IsNullOrWhiteSpace(victimId)) return;
            relations.ModifyAffinity(thiefId, victimId, -12f);
        }

        /// <summary>Advances the conflict authority one day for every registered survivor.</summary>
        public int TickDay()
        {
            int ticked = 0;
            foreach (string id in _registered)
            {
                _engine.Tick(id, 24f);
                ticked++;
            }
            RaiseStateChanged();
            return ticked;
        }

        public RationConflictSaveState CaptureState() => _engine.CaptureState();

        public void RestoreState(RationConflictSaveState? state)
        {
            if (state == null) return;
            _engine.RestoreState(state);
            LastEvent = "Ration conflict state restored from save.";
            RaiseStateChanged();
        }

        public void Clear()
        {
            _engine.RestoreState(new RationConflictSaveState());
            LastEvent = "Ration conflict state cleared.";
            RaiseStateChanged();
        }
    }
}
