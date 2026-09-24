// SPDX-License-Identifier: MIT
// ============================================================================
// ASHFALL Plan 186 — Shelter Maintenance & Degradation System Host Wiring.
// ShelterMaintenanceSystem is the sole authority for component condition
// tracking, daily degradation ticks, maintenance interventions, and failure alerts.
// ============================================================================

using System;
using Godot;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private ShelterMaintenanceHostSession? _shelterMaintenance;
        private bool _shelterMaintenanceDirty;

        public ShelterMaintenanceHostSession? ShelterMaintenance => _shelterMaintenance;

        public void SetupShelterMaintenance()
        {
            if (_shelterMaintenance != null) return;

            _shelterMaintenance = ShelterMaintenanceHostSession.Create(_dataDir);

            var saved = ShelterMaintenanceSaveStore.TryLoad();
            if (saved != null)
            {
                _shelterMaintenance.RestoreState(saved);
            }

            _shelterMaintenance.StateChanged += () => _shelterMaintenanceDirty = true;
        }

        public void TickShelterMaintenance(int day)
        {
            if (_shelterMaintenance == null) SetupShelterMaintenance();
            if (_shelterMaintenance == null) return;

            float weatherMult = GetWeatherStressMultiplier();
            float radMult = GetRadiationStressMultiplier();
            _shelterMaintenance.TickDay(day, weatherMult, radMult);
        }

        public void SaveShelterMaintenance()
        {
            if (_shelterMaintenance == null) return;
            var state = _shelterMaintenance.CaptureState();
            ShelterMaintenanceSaveStore.TrySave(state);
            if (CaptureSection(ShelterMaintenanceSaveStore.SectionName,
                ShelterMaintenanceSaveStore.TryCapturePersisted(state)))
            {
                _shelterMaintenanceDirty = false;
            }
        }

        public void FlushShelterMaintenanceIfDirty()
        {
            if (_shelterMaintenanceDirty)
                SaveShelterMaintenance();
        }

        public ShelterMaintenanceCensus GetShelterMaintenanceCensus() =>
            _shelterMaintenance?.Census ?? default;

        public void ResetShelterMaintenance()
        {
            _shelterMaintenance = null;
            _shelterMaintenanceDirty = false;
        }

        /// <summary>
        /// Returns the weather stress multiplier for shelter component degradation.
        /// TODO: wire to _weatherSondeHost.ActiveConditions when WeatherHostSession exposes condition strings.
        /// </summary>
        private float GetWeatherStressMultiplier()
        {
            return 1.0f;
        }

        /// <summary>
        /// Returns the radiation stress multiplier for shelter component degradation.
        /// TODO: wire to radiation system when shelter radiation level is exposed.
        /// </summary>
        private float GetRadiationStressMultiplier()
        {
            return 1.0f;
        }
    }
}
