// SPDX-License-Identifier: MIT
// ============================================================================
// Soil reclamation host composition (Expansion 15: The Deep Root). The Core
// SoilReclamationProfileEngine is the sole authority over amendment chemistry,
// fertility evaluation, germination viability, and mutation risk. The host owns
// only the plot ledger and supplies the surveyed raw chemistry facts.
// ============================================================================

using System;
using Ashfall.Core.Farming;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private SoilReclamationProfileHostSession? _soilReclamation;
        private bool _soilReclamationDirty;

        public SoilReclamationProfileHostSession? SoilReclamationProfileSession => _soilReclamation;

        public void SetupSoilReclamationProfile()
        {
            if (_soilReclamation != null) return;
            var saved = SoilReclamationProfileSaveStore.TryLoad();
            _soilReclamation = SoilReclamationProfileHostSession.Create(saved);
            _soilReclamation.StateChanged += () => _soilReclamationDirty = true;
        }

        /// <summary>Surveys an open-ground plot from host-owned chemistry facts.</summary>
        public PlotSoilEntry RegisterSoilPlot(
            string plotId,
            int salinityPermille,
            int radionuclideLoadPermille,
            int organicMatterPermille,
            int phTenths)
        {
            SetupSoilReclamationProfile();
            var plot = _soilReclamation!.RegisterPlot(plotId, salinityPermille, radionuclideLoadPermille, organicMatterPermille, phTenths);
            _soilReclamationDirty = true;
            return plot;
        }

        /// <summary>
        /// Applies one amendment cycle. Amendment and quantity come from the
        /// existing inventory owner; the Core engine decides every chemistry
        /// and tier outcome.
        /// </summary>
        public SoilEvaluationResult ApplySoilAmendment(
            string plotId,
            SoilAmendment amendment,
            int quantityUnits,
            int cycleCount = 1)
        {
            SetupSoilReclamationProfile();
            var result = _soilReclamation!.ApplyAmendment(plotId, amendment, quantityUnits, cycleCount);
            _soilReclamationDirty = true;
            return result;
        }

        public int AdvanceSoilReclamationDay(int day)
        {
            SetupSoilReclamationProfile();
            int plotted = _soilReclamation!.AdvanceDay(day);
            _soilReclamationDirty = true;
            return plotted;
        }

        public (int Plots, int Cultivable, int Barren) GetSoilReclamationProfileReadout()
        {
            SetupSoilReclamationProfile();
            return (_soilReclamation!.PlotCount, _soilReclamation.CultivablePlotCount, _soilReclamation.BarrenPlotCount);
        }

        public void SaveSoilReclamationProfile()
        {
            if (_soilReclamation == null) return;
            var state = _soilReclamation.CaptureState();
            SoilReclamationProfileSaveStore.TrySave(state);
            if (CaptureSection(SoilReclamationProfileSaveStore.SectionName, SoilReclamationProfileSaveStore.TryCapturePersisted(state)))
                _soilReclamationDirty = false;
        }

        public void FlushSoilReclamationProfileIfDirty()
        {
            if (_soilReclamationDirty) SaveSoilReclamationProfile();
        }

        public void ResetSoilReclamationProfile()
        {
            _soilReclamation = null;
            _soilReclamationDirty = false;
        }
    }
}
