// SPDX-License-Identifier: MIT
// ============================================================================
// ASHFALL Plan 186 — Shelter Maintenance & Degradation System Host Wiring.
// ShelterMaintenanceSystem is the sole authority for component condition
// tracking, daily degradation ticks, maintenance interventions, and failure alerts.
// ============================================================================

using System;
using Godot;
using Ashfall.Core;
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
        /// Returns the weather stress multiplier for shelter component degradation,
        /// derived from the canonical live weather kind. Unknown or missing weather
        /// stays neutral rather than inventing stress.
        /// </summary>
        private float GetWeatherStressMultiplier()
        {
            var kind = _world?.Weather?.Current;
            if (kind == null) return 1.0f;
            switch (kind.Value)
            {
                case WeatherKind.FalloutStorm:
                case WeatherKind.RadHail:
                case WeatherKind.GlassStorm:
                case WeatherKind.EMPStorm:
                    return 1.5f;
                case WeatherKind.Blizzard:
                case WeatherKind.BlackRain:
                case WeatherKind.AcidSnow:
                case WeatherKind.BlackSnow:
                case WeatherKind.IceStorm:
                case WeatherKind.BloodRain:
                case WeatherKind.AshLightning:
                    return 1.25f;
                case WeatherKind.Ashfall:
                case WeatherKind.BioFog:
                case WeatherKind.ParticulateFog:
                case WeatherKind.ThermalInversion:
                    return 1.1f;
                default:
                    return 1.0f;
            }
        }

        /// <summary>
        /// Returns the radiation stress multiplier for shelter component degradation,
        /// derived from the average lifetime dose across the living roster. Dosimeter
        /// records are transient runtime state, so this read never mutates saves.
        /// </summary>
        private float GetRadiationStressMultiplier()
        {
            var roster = _survivors?.RosterState;
            var radiation = _survivors?.Radiation;
            if (roster == null || radiation == null) return 1.0f;
            float total = 0f;
            int count = 0;
            for (int i = 0; i < roster.Count; i++)
            {
                var s = roster[i];
                if (s == null || string.IsNullOrEmpty(s.Id) || !s.IsAlive) continue;
                total += radiation.GetDosimeter(s.Id).LifetimeDose;
                count++;
            }
            if (count == 0) return 1.0f;
            float average = total / count;
            if (average >= 200f) return 1.5f;
            if (average >= 50f) return 1.25f;
            return 1.0f;
        }
    }
}
