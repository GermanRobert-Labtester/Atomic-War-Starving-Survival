// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : WaterQualityProfileSaveStore
// Core Engine: Ashfall.Core.Water.WaterQualityProfileEngine
// Host Caller: Main.WaterQualityProfile
// Purpose    : Water source purity profiling. The Core engine is the sole
//              authority over contaminant profiles, purity-tier derivation,
//              treatment yield, and health risk; the host owns only the
//              per-source assay ledger and its persistence.
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Save;
using Ashfall.Core.Water;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Persisted per-source assay ledger. Each entry records the last derived
    /// purity tier and the accumulated filter wear applied to that source.
    /// </summary>
    [Serializable]
    public sealed class WaterQualitySourceEntry
    {
        public string sourceId = string.Empty;
        public WaterSourcePurityTier sourceTier;
        public WaterSourcePurityTier resultTier;
        public int floodContaminationPermille;
        public int filterWearPermille;
        public int lastAssayDay = -1;
        public int assayCount;
    }

    [Serializable]
    public sealed class WaterQualityProfileSaveState
    {
        public int schema_version = 1;
        public int currentDay;
        public List<WaterQualitySourceEntry> sources = new List<WaterQualitySourceEntry>();
    }

    public static class WaterQualityProfileSaveStore
    {
        public const string FileName = "water_quality_profile_save.json";
        public const string SectionName = "water_quality_profile";

        private static readonly SaveStore<WaterQualityProfileSaveState> s_store =
            SaveStoreHub.Checksummed<WaterQualityProfileSaveState>(FileName, nameof(WaterQualityProfileSaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapturePersisted(WaterQualityProfileSaveState state) => s_store.CaptureBare(state);
        public static WaterQualityProfileSaveState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(WaterQualityProfileSaveState state) => s_store.TrySave(state);
        public static WaterQualityProfileSaveState? TryLoad() => s_store.TryLoad();
    }

    /// <summary>Host session composing the pure Core water-quality engine.</summary>
    public sealed class WaterQualityProfileHostSession : HostSessionBase
    {
        private readonly WaterQualityProfileSaveState _state;

        public WaterQualityProfileHostSession(WaterQualityProfileSaveState? state = null)
        {
            _state = state ?? new WaterQualityProfileSaveState();
            _state.sources ??= new List<WaterQualitySourceEntry>();
        }

        public static WaterQualityProfileHostSession Create(WaterQualityProfileSaveState? state = null) =>
            new WaterQualityProfileHostSession(state);

        public string LastEvent { get; private set; } = string.Empty;
        public int CurrentDay => _state.currentDay;
        public IReadOnlyList<WaterQualitySourceEntry> Sources => _state.sources;
        public int SourceCount => _state.sources.Count;

        public WaterQualitySourceEntry? FindSource(string sourceId) => _state.sources.FirstOrDefault(s =>
            string.Equals(s.sourceId, sourceId, StringComparison.Ordinal));

        /// <summary>
        /// Assays one water source and records the derived purity tier. The Core engine
        /// decides the contaminant profile; this method only stores the ledger row.
        /// </summary>
        public WaterQualitySourceEntry AssaySource(
            string sourceId,
            WaterSourcePurityTier tier,
            int floodContaminationPermille,
            int day)
        {
            var entry = FindSource(sourceId);
            if (entry == null)
            {
                entry = new WaterQualitySourceEntry { sourceId = sourceId ?? string.Empty, sourceTier = tier };
                _state.sources.Add(entry);
            }

            entry.sourceTier = tier;
            entry.floodContaminationPermille = Math.Max(0, Math.Min(1000, floodContaminationPermille));
            entry.lastAssayDay = day;
            entry.assayCount++;
            _state.currentDay = day;

            var profile = WaterQualityProfileEngine.EvaluateSourceContamination(
                tier, entry.floodContaminationPermille);
            var yield = WaterQualityProfileEngine.CalculateTreatmentYield(tier, TreatmentMode.Idle, 1000);
            entry.resultTier = yield.ResultingPurityTier;
            entry.filterWearPermille = yield.FilterWearPermille;

            LastEvent = $"Assayed {sourceId}: risk {WaterQualityProfileEngine.CalculateHealthRiskPermille(profile)} permille, tier {entry.resultTier}.";
            RaiseStateChanged();
            return entry;
        }

        /// <summary>Applies a treatment pass to a source, accumulating filter wear.</summary>
        public WaterTreatmentYield TreatSource(string sourceId, TreatmentMode mode, int filterIntegrityPermille, int day)
        {
            var entry = FindSource(sourceId);
            if (entry == null) throw new InvalidOperationException($"Unknown water source '{sourceId}'.");

            var yield = WaterQualityProfileEngine.CalculateTreatmentYield(
                entry.sourceTier, mode, Math.Max(0, Math.Min(1000, filterIntegrityPermille)));

            entry.filterWearPermille = Math.Max(0, Math.Min(1000, entry.filterWearPermille + yield.FilterWearPermille));
            entry.resultTier = yield.ResultingPurityTier;
            entry.lastAssayDay = day;
            _state.currentDay = day;

            LastEvent = $"Treated {sourceId} with {mode}: yield {yield.YieldFractionPermille} permille, wear {yield.FilterWearPermille}.";
            RaiseStateChanged();
            return yield;
        }

        public int UnsafeSourceCount => _state.sources.Count(s =>
            s.resultTier == WaterSourcePurityTier.RawSurfaceRunoff ||
            s.resultTier == WaterSourcePurityTier.SumpBrine);

        public WaterQualityProfileSaveState CaptureState()
        {
            var copy = new WaterQualityProfileSaveState
            {
                schema_version = _state.schema_version,
                currentDay = _state.currentDay
            };
            foreach (var s in _state.sources)
            {
                copy.sources.Add(new WaterQualitySourceEntry
                {
                    sourceId = s.sourceId,
                    sourceTier = s.sourceTier,
                    resultTier = s.resultTier,
                    floodContaminationPermille = s.floodContaminationPermille,
                    filterWearPermille = s.filterWearPermille,
                    lastAssayDay = s.lastAssayDay,
                    assayCount = s.assayCount
                });
            }
            return copy;
        }

        public void RestoreState(WaterQualityProfileSaveState state)
        {
            _state.currentDay = state?.currentDay ?? 0;
            _state.sources.Clear();
            if (state == null) return;
            _state.schema_version = state.schema_version;
            if (state.sources != null) _state.sources.AddRange(state.sources);
            LastEvent = "Restored water quality profile state.";
            RaiseStateChanged();
        }

        public bool TrySave() => WaterQualityProfileSaveStore.TrySave(CaptureState());

        public bool TryLoad()
        {
            var state = WaterQualityProfileSaveStore.TryLoad();
            if (state == null) return false;
            RestoreState(state);
            return true;
        }
    }
}
