// SPDX-License-Identifier: MIT
// ============================================================================
// Main Partial : Plan 173 Phase 2 — Radio Program Production host wire
// Subsystems   : catalog load, PsyOps StartCampaign delegate, schedule Resolve
//                delivery adapter, dedicated save section, daily prep tick.
// ============================================================================
using System;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Radio;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private RadioProgramProductionHostSession? _radioProgramProduction;

        /// <summary>Panel-facing session (Plan 173 Phase 3).</summary>
        public RadioProgramProductionHostSession EnsureRadioProgramProductionSession()
        {
            SetupRadioProgramProduction();
            return _radioProgramProduction!;
        }

        private void SetupRadioProgramProduction()
        {
            if (_radioProgramProduction != null) return;

            SetupRadio();
            SetupPsyOps();

            var catalog = RadioProgramCatalogLoader.Load(
                _dataDir,
                new FileSystemIO());

            var stations = _radio?.Stations;
            var inv = _inventory?.Inventory;
            var system = new RadioProgramProductionSystem(catalog, stations, new GodotLog())
            {
                StartPropagandaCampaign = (campaignId, day) =>
                {
                    if (_psyops?.System == null) return false;
                    return _psyops.System.StartCampaign(campaignId, day);
                },
                HasRequiredEquipment = itemIds =>
                {
                    if (inv == null || itemIds == null) return false;
                    for (int i = 0; i < itemIds.Count; i++)
                    {
                        if (string.IsNullOrEmpty(itemIds[i])) continue;
                        if (inv.CountById(itemIds[i]) < 1) return false;
                    }
                    return true;
                },
                TryConsumePrepCost = (itemId, count) =>
                    inv != null && inv.TryConsumeById(itemId, count)
            };

            _radioProgramProduction = new RadioProgramProductionHostSession(system);

            var saved = RadioProgramProductionSaveStore.TryLoad();
            if (saved != null)
            {
                _radioProgramProduction.RestoreSave(saved);
                GD.Print("[Ashfall Godot] Radio program production restored.");
            }
        }

        /// <summary>Start prep for a template; presenter defaults to shelter desk.</summary>
        public string StartRadioProgramPrep(string templateId, string? presenterId = null)
        {
            SetupRadioProgramProduction();
            if (_radioProgramProduction == null) return "Radio program production is unavailable.";
            string presenter = string.IsNullOrEmpty(presenterId) ? "presenter_shelter_desk" : presenterId!;
            return _radioProgramProduction.StartPrep(templateId, presenter, _simDay);
        }

        public string CancelRadioProgramJob(string jobId)
        {
            SetupRadioProgramProduction();
            if (_radioProgramProduction == null) return "Radio program production is unavailable.";
            return _radioProgramProduction.CancelJob(jobId);
        }

        /// <summary>
        /// Deliver a ready job using the host-supplied schedule fact (or last listen).
        /// </summary>
        public string DeliverRadioProgram(string jobId, ScheduledBroadcastResult? delivery = null)
        {
            SetupRadioProgramProduction();
            if (_radioProgramProduction == null) return "Radio program production is unavailable.";

            var fact = delivery ?? _radio?.LastScheduledBroadcast;
            if (fact == null)
                return "No scheduled broadcast fact available for delivery.";

            return _radioProgramProduction.TryDeliver(jobId, fact, _simDay);
        }

        private void SaveRadioProgramProduction()
        {
            if (_radioProgramProduction == null) return;
            CaptureSection(
                "radio_program_production",
                RadioProgramProductionSaveStore.TryCapturePersisted(_radioProgramProduction.CaptureSave()));
        }

        /// <summary>
        /// Daily prep tick + opportunistic delivery for Ready jobs via schedule Resolve
        /// at each job's bound station frequency. Does not own the schedule.
        /// </summary>
        private void TickRadioProgramProduction(int day)
        {
            SetupRadioProgramProduction();
            if (_radioProgramProduction == null) return;

            _radioProgramProduction.System.TickDay(day);

            if (_radio?.ScheduleCoordinator == null) return;

            var ready = _radioProgramProduction.System.GetActiveJobs();
            for (int i = 0; i < ready.Count; i++)
            {
                var job = ready[i];
                if (job.Status != (int)RadioProgramJobStatus.Ready) continue;

                var station = _radio.Stations.GetStation(job.StationId);
                if (station == null) continue;

                var delivery = _radio.ScheduleCoordinator.Resolve(
                    station.FrequencyMhz,
                    day,
                    _radio.Rng);
                if (delivery == null) continue;

                _radioProgramProduction.TryDeliver(job.JobId, delivery, day);
            }
        }
    }
}
