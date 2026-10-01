// SPDX-License-Identifier: MIT
// ============================================================================
// Water quality profile host composition. The Core WaterQualityProfileEngine is
// the sole authority over contaminant profiles, purity-tier derivation,
// treatment yield, filter wear, and health risk. The host owns only the
// per-source assay ledger.
// ============================================================================

using Ashfall.Core;
using Ashfall.Core.Water;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private WaterQualityProfileHostSession? _waterQuality;
        private bool _waterQualityDirty;

        public WaterQualityProfileHostSession? WaterQualityProfileSession => _waterQuality;

        public void SetupWaterQualityProfile()
        {
            if (_waterQuality != null) return;
            var saved = WaterQualityProfileSaveStore.TryLoad();
            _waterQuality = WaterQualityProfileHostSession.Create(saved);
            _waterQuality.StateChanged += () => _waterQualityDirty = true;
        }

        /// <summary>Assays one water source and records its derived purity tier.</summary>
        public WaterQualitySourceEntry AssayWaterSource(
            string sourceId, WaterSourcePurityTier tier, int floodContaminationPermille, int day)
        {
            SetupWaterQualityProfile();
            var entry = _waterQuality!.AssaySource(sourceId, tier, floodContaminationPermille, day);
            _waterQualityDirty = true;
            return entry;
        }

        public WaterTreatmentYield TreatWaterSource(
            string sourceId, TreatmentMode mode, int filterIntegrityPermille, int day)
        {
            SetupWaterQualityProfile();
            var yield = _waterQuality!.TreatSource(sourceId, mode, filterIntegrityPermille, day);
            _waterQualityDirty = true;
            return yield;
        }

        public (int Sources, int Unsafe) GetWaterQualityReadout()
        {
            SetupWaterQualityProfile();
            return (_waterQuality!.SourceCount, _waterQuality.UnsafeSourceCount);
        }

        public void SaveWaterQualityProfile()
        {
            if (_waterQuality == null) return;
            var state = _waterQuality.CaptureState();
            if (CaptureSection(WaterQualityProfileSaveStore.SectionName, WaterQualityProfileSaveStore.TryCapturePersisted(state)))
                _waterQualityDirty = false;
        }

        public void ResetWaterQualityProfile()
        {
            _waterQuality = null;
            _waterQualityDirty = false;
        }
    }
}
