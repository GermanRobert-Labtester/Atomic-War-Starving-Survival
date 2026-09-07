// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core;
using Ashfall.Core.IO;
using Ashfall.Core.Random;
using Ashfall.Core.Shelter;
using Godot;

namespace AtomicWar.GodotApp
{
    // ─────────────────────────────────────────────────────────────────
    // Plans B86–B89 host wiring.
    // B89: Core owns grades/certificates/disturbance. Host constructs,
    // ticks, saves, projects into workshop Calibration, and feeds
    // ballistics tooling from the registered consumer — never a
    // hardcoded 0.75f.
    // B87: Core owns aquaponics ecology. Host constructs, ticks, saves,
    // and queries power/inventory — never owns water/power/greenhouse.
    // ─────────────────────────────────────────────────────────────────
    public partial class Main
    {
        private PrecisionMetrologySystem? _precisionMetrology;
        private bool _precisionMetrologyDirty;
        private bool _precisionMetrologySeismicWired;

        private AquaponicsSystem? _aquaponics;
        private bool _aquaponicsDirty;

        private void SetupPrecisionMetrology()
        {
            if (_precisionMetrology != null)
            {
                EnsurePrecisionMetrologySeismicWire();
                return;
            }

            SetupCampaignDay();
            SetupInventory();

            var rng = _campaignDay != null
                ? _campaignDay.Rng.GetStream(CampaignStreamIds.MetrologyCalibrationDrift).Rng
                : new SeededRng(89);

            _precisionMetrology = new PrecisionMetrologySystem(
                rng,
                _inventory?.Inventory,
                new GodotLog());

            string catalogPath = System.IO.Path.Combine(_dataDir, PrecisionMetrologyCatalogLoader.DefaultFileName);
            if (System.IO.File.Exists(catalogPath))
            {
                var files = new FileSystemIO();
                var json = new SystemTextJsonSerializer();
                _precisionMetrology.LoadCatalog(
                    PrecisionMetrologyCatalogLoader.Load(_dataDir, files, json, new GodotLog()));
            }

            var saved = PrecisionMetrologySaveStore.TryLoad();
            if (saved != null) _precisionMetrology.RestoreState(saved);

            _precisionMetrology.OnStateChanged += () => _precisionMetrologyDirty = true;
            _precisionMetrology.OnInstrumentCalibrated += _ => _precisionMetrologyDirty = true;
            _precisionMetrology.OnInstrumentDisturbed += (_, _) => _precisionMetrologyDirty = true;
            _precisionMetrology.OnCertificateIssued += _ => _precisionMetrologyDirty = true;

            EnsurePrecisionMetrologySeismicWire();

            GD.Print("[Ashfall Godot] Precision metrology host ready (plan B89, " +
                     _precisionMetrology.Consumers.Count + " consumers).");
        }

        private void EnsurePrecisionMetrologySeismicWire()
        {
            if (_precisionMetrologySeismicWired) return;
            SetupSeismicDynamics();
            if (_seismicDynamics == null || _precisionMetrology == null) return;

            _seismicDynamics.OnQuakeOccurred += q =>
            {
                if (_precisionMetrology == null) return;
                string key = $"quake_{q.day}_{q.magnitude:F1}";
                _precisionMetrology.ApplyDisturbance(q.magnitude, q.day, key);
                // Project drifted tooling into workshop machines immediately.
                if (_shelterWorkshop != null)
                    _precisionMetrology.ProjectToWorkshop(_shelterWorkshop);
            };
            _precisionMetrologySeismicWired = true;
        }

        private void SavePrecisionMetrology()
        {
            if (_precisionMetrology == null) return;
            if (CaptureSection(PrecisionMetrologySaveStore.SectionName,
                    PrecisionMetrologySaveStore.TryCapturePersisted(_precisionMetrology.CaptureState())))
                _precisionMetrologyDirty = false;
        }

        private void TickPrecisionMetrology(int day)
        {
            SetupPrecisionMetrology();
            if (_precisionMetrology == null) return;

            _precisionMetrology.TickDay(day);

            // Keep registered room Calibration in lockstep with metrology truth.
            EnsureShelterWorkshop();
            if (_shelterWorkshop != null)
                _precisionMetrology.ProjectToWorkshop(_shelterWorkshop);

            if (_precisionMetrologyDirty) SavePrecisionMetrology();
        }

        /// <summary>
        /// Live tooling calibration for the ballistics workbench consumer only.
        /// Uncalibrated / unavailable returns 0 — never borrows another consumer's grade.
        /// </summary>
        private float ResolveBallisticsToolingCalibration()
        {
            SetupPrecisionMetrology();
            if (_precisionMetrology == null) return 0f;
            return _precisionMetrology.QueryToolingCalibration("ballistics_workbench");
        }

        // ── Plan B87 — closed-loop aquaponics ────────────────────────

        private void SetupAquaponics()
        {
            if (_aquaponics != null) return;

            SetupCampaignDay();
            SetupInventory();

            var rng = _campaignDay != null
                ? _campaignDay.Rng.GetStream(CampaignStreamIds.AquaponicsDisease).Rng
                : new SeededRng(87);

            _aquaponics = new AquaponicsSystem(
                rng,
                _inventory?.Inventory,
                roomId => ResolveAquaponicsPowerAvailability(roomId),
                new GodotLog());

            string catalogPath = System.IO.Path.Combine(_dataDir, AquaponicsCatalogLoader.DefaultFileName);
            if (System.IO.File.Exists(catalogPath))
            {
                var files = new FileSystemIO();
                var json = new SystemTextJsonSerializer();
                _aquaponics.LoadCatalog(
                    AquaponicsCatalogLoader.Load(_dataDir, files, json, new GodotLog()));
            }

            var saved = AquaponicsSaveStore.TryLoad();
            if (saved != null) _aquaponics.RestoreState(saved);

            _aquaponics.OnStateChanged += () => _aquaponicsDirty = true;
            _aquaponics.OnDiseaseMilestone += _ => _aquaponicsDirty = true;
            _aquaponics.OnHarvested += _ => _aquaponicsDirty = true;

            GD.Print("[Ashfall Godot] Aquaponics host ready (plan B87, " +
                     _aquaponics.TankClasses.Count + " tank classes).");
        }

        private float ResolveAquaponicsPowerAvailability(string roomId)
        {
            // Power truth stays on the grid. Aquaponics only queries availability.
            _ = roomId;
            if (_powerGrid?.System == null) return 1f;
            return _powerGrid.System.IsBrownout ? 0f : 1f;
        }

        private void SaveAquaponics()
        {
            if (_aquaponics == null) return;
            if (CaptureSection(AquaponicsSaveStore.SectionName,
                    AquaponicsSaveStore.TryCapturePersisted(_aquaponics.CaptureState())))
                _aquaponicsDirty = false;
        }

        private void TickAquaponics(int day)
        {
            SetupAquaponics();
            if (_aquaponics == null) return;

            // Temperature modifier stays a host projection; Core never owns room thermal truth.
            float temperatureModifier = 1f;
            _aquaponics.TickDay(day, temperatureModifier);
            if (_aquaponicsDirty) SaveAquaponics();
        }

        /// <summary>
        /// Explicit nutrient export for greenhouse consumers. Aquaponics never
        /// mutates greenhouse plots; callers pull this projection.
        /// </summary>
        private AquaponicNutrientSource? QueryAquaponicNutrientExport()
        {
            SetupAquaponics();
            return _aquaponics?.GetNutrientExport();
        }
    }
}
