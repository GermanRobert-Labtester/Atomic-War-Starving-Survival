// SPDX-License-Identifier: MIT
// ============================================================================
// Radiation social bridge host composition. The Core bridge owns dose brackets,
// social penalties, discrimination incidents, and faction standing adjustments;
// this partial composes its catalog and save section. Survivor dose remains the
// dose ledger's authority and faction policy remains the faction owner's.
// ============================================================================

using System;
using Ashfall.Core;
using Ashfall.Core.Radiation;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private RadiationSocialHostSession? _radiationSocial;
        private bool _radiationSocialDirty;

        public RadiationSocialHostSession? RadiationSocialSession => _radiationSocial;

        public void SetupRadiationSocial()
        {
            if (_radiationSocial != null) return;
            var saved = RadiationSocialSaveStore.TryLoad();
            _radiationSocial = RadiationSocialHostSession.Create(saved);
            _radiationSocial.StateChanged += () => _radiationSocialDirty = true;
            _radiationSocial.LoadCatalog(CatalogPath.ResolveDataDir());
        }

        public SurvivorRadiationSocialProfile EvaluateRadiationSocialStance(
            string survivorId, float doseMsv, ISeededRng? rng = null, string? encounteringFactionId = null)
        {
            SetupRadiationSocial();
            var profile = _radiationSocial!.EvaluateSocialStance(survivorId, doseMsv, rng, encounteringFactionId);
            _radiationSocialDirty = true;
            return profile;
        }

        public (int Incidents, int Protests) GetRadiationSocialReadout()
        {
            SetupRadiationSocial();
            if (_radiationSocial == null) return (0, 0);
            return (_radiationSocial.Bridge.TotalDiscriminationIncidents, _radiationSocial.Bridge.TotalFactionProtests);
        }

        public void SaveRadiationSocial()
        {
            if (_radiationSocial == null) return;
            var state = _radiationSocial.CaptureState();
            RadiationSocialSaveStore.TrySave(state);
            if (CaptureSection(RadiationSocialSaveStore.SectionName, RadiationSocialSaveStore.TryCapturePersisted(state)))
                _radiationSocialDirty = false;
        }

        public void FlushRadiationSocialIfDirty()
        {
            if (_radiationSocialDirty) SaveRadiationSocial();
        }

        public void ResetRadiationSocial()
        {
            _radiationSocial = null;
            _radiationSocialDirty = false;
        }
    }
}
