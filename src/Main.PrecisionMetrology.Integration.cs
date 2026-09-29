// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.IO;
using Ashfall.Core.Random;
using Ashfall.Core.Shelter;
using Godot;
using System;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private PrecisionMetrologySystem? _precisionMetrology;
        private bool _precisionMetrologyDirty;
        private bool _precisionMetrologySeismicWired;

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

            // Plan 71: room_workshop_precision — the calibration drift process is
            // machine-driven; while the precision workshop is unpowered the
            // daily drift tick pauses AND the projected workshop grade reads
            // zero (calibration work unavailable downstream).
            bool precisionPowered = _powerGrid?.System?.IsRoomPowered("room_workshop_precision") ?? true;
            if (precisionPowered)
                _precisionMetrology.TickDay(day);

            // Keep registered room Calibration in lockstep with metrology truth.
            EnsureShelterWorkshop();
            if (_shelterWorkshop != null)
            {
                _precisionMetrology.ProjectToWorkshop(_shelterWorkshop);
                if (!precisionPowered)
                {
                    // Unpowered precision machinery presents zero calibration to
                    // its consumers (ballistics workbench grade path).
                    _shelterWorkshop.GetOrCreateMachineState("room_workshop_precision").Calibration = 0f;
                }
            }

            if (_precisionMetrologyDirty) SavePrecisionMetrology();
        }

    }
}
