// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : SoilReclamationProfileSaveStore
// Core Engine: Ashfall.Core.Farming.SoilReclamationProfileEngine
// Host Caller: Main.SoilReclamationProfile
// Purpose    : Expansion 15 The Deep Root — open-ground soil reclamation. The
//              Core engine is the sole authority over amendment chemistry,
//              fertility evaluation, germination viability, and mutation risk;
//              the host owns only the plot ledger and its persistence.
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Farming;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Persisted soil state: one entry per reclaimed plot with its live chemistry.
    /// </summary>
    [Serializable]
    public sealed class PlotSoilEntry
    {
        public string plotId = string.Empty;
        public int salinityPermille;
        public int radionuclideLoadPermille;
        public int organicMatterPermille;
        public int phTenths;
        public SoilQualityTier qualityTier;
        public int germinationViabilityPermille;
        public int cropMutationRiskPermille;
        public int yieldMultiplierPermille;
        public bool isCultivable;
        public SoilAmendment lastAmendment = SoilAmendment.None;
        public int lastAmendmentUnits;
        public int cycleCount;
    }

    [Serializable]
    public sealed class SoilReclamationProfileSaveState
    {
        public int schema_version = 1;
        public int currentDay;
        public List<PlotSoilEntry> plots = new List<PlotSoilEntry>();
    }

    public static class SoilReclamationProfileSaveStore
    {
        public const string FileName = "soil_reclamation_profile_save.json";
        public const string SectionName = "soil_reclamation_profile";

        private static readonly SaveStore<SoilReclamationProfileSaveState> s_store =
            SaveStoreHub.Checksummed<SoilReclamationProfileSaveState>(FileName, nameof(SoilReclamationProfileSaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapturePersisted(SoilReclamationProfileSaveState state) => s_store.CaptureBare(state);
        public static SoilReclamationProfileSaveState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(SoilReclamationProfileSaveState state) => s_store.TrySave(state);
        public static SoilReclamationProfileSaveState? TryLoad() => s_store.TryLoad();
    }

    /// <summary>Host session composing the pure Core soil reclamation engine.</summary>
    public sealed class SoilReclamationProfileHostSession : HostSessionBase
    {
        private readonly SoilReclamationProfileSaveState _state;

        public SoilReclamationProfileHostSession(SoilReclamationProfileSaveState? state = null)
        {
            _state = state ?? new SoilReclamationProfileSaveState();
            _state.plots ??= new List<PlotSoilEntry>();
        }

        public static SoilReclamationProfileHostSession Create(SoilReclamationProfileSaveState? state = null) =>
            new SoilReclamationProfileHostSession(state);

        public string LastEvent { get; private set; } = string.Empty;

        public int CurrentDay => _state.currentDay;
        public int PlotCount => _state.plots.Count;
        public IReadOnlyList<PlotSoilEntry> Plots => _state.plots;

        public PlotSoilEntry? FindPlot(string plotId) => _state.plots.FirstOrDefault(p =>
            string.Equals(p.plotId, plotId, StringComparison.Ordinal));

        /// <summary>
        /// Registers an open-ground plot from host-surveyed raw chemistry and
        /// immediately evaluates it through the Core engine.
        /// </summary>
        public PlotSoilEntry RegisterPlot(
            string plotId,
            int salinityPermille,
            int radionuclideLoadPermille,
            int organicMatterPermille,
            int phTenths)
        {
            var existing = FindPlot(plotId);
            if (existing != null) return existing;

            var plot = new PlotSoilEntry
            {
                plotId = plotId ?? string.Empty,
                salinityPermille = Clamp(salinityPermille),
                radionuclideLoadPermille = Clamp(radionuclideLoadPermille),
                organicMatterPermille = Clamp(organicMatterPermille),
                phTenths = Math.Max(30, Math.Min(110, phTenths))
            };
            ApplyEngine(plot, SoilAmendment.None, 0);
            _state.plots.Add(plot);
            LastEvent = $"Plot {plotId} surveyed: {plot.qualityTier} (germination {plot.germinationViabilityPermille} permille).";
            RaiseStateChanged();
            return plot;
        }

        /// <summary>
        /// Applies one amendment cycle to a plot. The Core engine decides the
        /// resulting chemistry and tier; the host only records it.
        /// </summary>
        public SoilEvaluationResult ApplyAmendment(string plotId, SoilAmendment amendment, int quantityUnits, int cycleCount = 1)
        {
            var plot = FindPlot(plotId) ?? throw new ArgumentException($"Unknown plot '{plotId}'.", nameof(plotId));
            var result = ApplyEngine(plot, amendment, quantityUnits);
            plot.cycleCount += Math.Max(1, cycleCount);
            LastEvent = $"Plot {plotId}: {amendment} x{Math.Max(1, cycleCount)} -> {plot.qualityTier} (yield {plot.yieldMultiplierPermille} permille).";
            RaiseStateChanged();
            return result;
        }

        private static SoilEvaluationResult ApplyEngine(PlotSoilEntry plot, SoilAmendment amendment, int quantityUnits)
        {
            var result = SoilReclamationProfileEngine.Evaluate(
                plot.salinityPermille,
                plot.radionuclideLoadPermille,
                plot.organicMatterPermille,
                plot.phTenths,
                amendment,
                quantityUnits);

            plot.salinityPermille = result.NetSalinityPermille;
            plot.radionuclideLoadPermille = result.NetRadionuclideLoadPermille;
            plot.organicMatterPermille = result.NetOrganicMatterPermille;
            plot.phTenths = result.NetPhTenths;
            plot.qualityTier = result.QualityTier;
            plot.germinationViabilityPermille = result.GerminationViabilityPermille;
            plot.cropMutationRiskPermille = result.CropMutationRiskPermille;
            plot.yieldMultiplierPermille = result.YieldMultiplierPermille;
            plot.isCultivable = result.IsCultivable;
            plot.lastAmendment = amendment;
            plot.lastAmendmentUnits = Math.Max(0, quantityUnits);
            return result;
        }

        public int AdvanceDay(int day)
        {
            _state.currentDay = day;
            LastEvent = $"Soil reclamation advanced to day {day} across {_state.plots.Count} plot(s).";
            RaiseStateChanged();
            return _state.plots.Count;
        }

        public int CultivablePlotCount => _state.plots.Count(p => p.isCultivable);
        public int BarrenPlotCount => _state.plots.Count(p => p.qualityTier == SoilQualityTier.BarrenAsh);

        private static int Clamp(int value) => Math.Max(0, Math.Min(1000, value));

        public SoilReclamationProfileSaveState CaptureState()
        {
            var copy = new SoilReclamationProfileSaveState
            {
                schema_version = _state.schema_version,
                currentDay = _state.currentDay
            };
            foreach (var p in _state.plots) copy.plots.Add(Clone(p));
            return copy;
        }

        private static PlotSoilEntry Clone(PlotSoilEntry p) => new PlotSoilEntry
        {
            plotId = p.plotId,
            salinityPermille = p.salinityPermille,
            radionuclideLoadPermille = p.radionuclideLoadPermille,
            organicMatterPermille = p.organicMatterPermille,
            phTenths = p.phTenths,
            qualityTier = p.qualityTier,
            germinationViabilityPermille = p.germinationViabilityPermille,
            cropMutationRiskPermille = p.cropMutationRiskPermille,
            yieldMultiplierPermille = p.yieldMultiplierPermille,
            isCultivable = p.isCultivable,
            lastAmendment = p.lastAmendment,
            lastAmendmentUnits = p.lastAmendmentUnits,
            cycleCount = p.cycleCount
        };

        public void RestoreState(SoilReclamationProfileSaveState? state)
        {
            _state.currentDay = state?.currentDay ?? 0;
            _state.plots.Clear();
            if (state == null) return;
            _state.schema_version = state.schema_version;
            foreach (var p in state.plots ?? new List<PlotSoilEntry>())
                if (p != null) _state.plots.Add(Clone(p));
            LastEvent = "Restored soil reclamation state.";
            RaiseStateChanged();
        }

        public bool TrySave() => SoilReclamationProfileSaveStore.TrySave(CaptureState());

        public bool TryLoad()
        {
            var state = SoilReclamationProfileSaveStore.TryLoad();
            if (state == null) return false;
            RestoreState(state);
            return true;
        }
    }
}
