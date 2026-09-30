// SPDX-License-Identifier: MIT
// PLAN-CHEMICAL-RECON-TRUTH-183 — chemical plume dispersion host wiring.

using Ashfall.Core.Combat;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private ChemicalPlumeHostSession? _chemicalPlume;
        private bool _chemicalPlumeDirty;

        public ChemicalPlumeHostSession? ChemicalPlume => _chemicalPlume;

        public void SetupChemicalPlume()
        {
            if (_chemicalPlume != null) return;
            _chemicalPlume = new ChemicalPlumeHostSession();
            _chemicalPlume.WeatherProvider = () => new WeatherDispersionVector(
                (int)_weatherSondeHost.System.WindSpeedKph,
                (int)_weatherSondeHost.System.WindDirectionDeg,
                0);
            var saved = ChemicalPlumeSaveStore.TryLoad();
            if (saved != null) _chemicalPlume.RestoreState(saved);
            _chemicalPlume.StateChanged += () => _chemicalPlumeDirty = true;
        }

        public void SaveChemicalPlume()
        {
            if (_chemicalPlume == null) return;
            var state = _chemicalPlume.CaptureState();
            if (CaptureSection("chemical_plume", ChemicalPlumeSaveStore.TryCapturePersisted(state)))
            {
                _chemicalPlumeDirty = false;
            }
        }

        public void FlushChemicalPlumeIfDirty()
        {
            if (_chemicalPlumeDirty) SaveChemicalPlume();
        }

        public void ResetChemicalPlume()
        {
            _chemicalPlume = null;
            _chemicalPlumeDirty = false;
        }

        public void TickChemicalPlume(int day)
        {
            _chemicalPlume?.AdvanceDay();
        }
    }
}
