// SPDX-License-Identifier: MIT
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Radio;
using Xunit;

namespace Ashfall.Core.Tests.Radio
{
    /// <summary>
    /// Plan 173 Phase 1 — program production consumes SlotId + ScheduledBroadcastResult
    /// + optional PsyOps StartCampaign without owning schedule/receiver.
    /// </summary>
    public sealed class Plan173RadioProgramProductionTests
    {
        private static RadioProgramCatalog CreateCatalog()
        {
            var catalog = new RadioProgramCatalog
            {
                programs = new List<RadioProgramTemplateDef>
                {
                    new RadioProgramTemplateDef
                    {
                        id = "radio_prog_shelter_morning_bulletin",
                        display_name = "Shelter Morning Bulletin",
                        station_id = "station_civil_defense",
                        slot_id = "slot_cd_morning",
                        psyops_campaign_id = "psyops_campaign_scavenger_lantern_hour",
                        prep_ticks_required = 1
                    },
                    new RadioProgramTemplateDef
                    {
                        id = "radio_prog_classroom_evening_story",
                        display_name = "Evening Story",
                        station_id = "station_open_classroom",
                        slot_id = "slot_classroom_evening",
                        prep_ticks_required = 2
                    }
                }
            };
            catalog.Index();
            return catalog;
        }

        private static RadioStationCatalog CreateStations()
        {
            var stations = new RadioStationCatalog();
            stations.Register(new RadioStationDefinition
            {
                StationId = "station_civil_defense",
                DisplayName = "Civil Defense",
                FrequencyMhz = 88.5f,
                Schedule = new List<RadioProgramSlot>
                {
                    new RadioProgramSlot { SlotId = "slot_cd_morning", StartHour = 6, EndHour = 11 }
                }
            });
            stations.Register(new RadioStationDefinition
            {
                StationId = "station_open_classroom",
                DisplayName = "Open Classroom",
                FrequencyMhz = 91.3f,
                Schedule = new List<RadioProgramSlot>
                {
                    new RadioProgramSlot { SlotId = "slot_classroom_evening", StartHour = 19, EndHour = 23 }
                }
            });
            return stations;
        }

        private static ScheduledBroadcastResult OkDelivery(string stationId, string broadcastId = "bcast_test") =>
            new ScheduledBroadcastResult
            {
                HasTransmission = true,
                StationId = stationId,
                BroadcastId = broadcastId,
                SignalStrength = 7,
                VuStrength = 0.7f
            };

        [Fact]
        public void StartPrep_BlocksUnknownSlot()
        {
            var catalog = CreateCatalog();
            catalog.programs[0].slot_id = "slot_does_not_exist";
            catalog.Index();
            var system = new RadioProgramProductionSystem(catalog, CreateStations());

            var res = system.StartPrep("radio_prog_shelter_morning_bulletin", "survivor_announcer", 1);
            Assert.Equal(ActionResult.StatusKind.Blocked, res.Status);
            Assert.Equal("unknown_slot", res.FailureCode);
        }

        [Fact]
        public void PrepTick_ThenDeliver_RequiresScheduledBroadcastSuccess()
        {
            var system = new RadioProgramProductionSystem(CreateCatalog(), CreateStations());
            var start = system.StartPrep("radio_prog_shelter_morning_bulletin", "survivor_announcer", 1);
            Assert.Equal(ActionResult.StatusKind.Success, start.Status);

            var job = system.GetActiveJobs()[0];
            Assert.Equal((int)RadioProgramJobStatus.Preparing, job.Status);

            system.TickDay(2);
            Assert.Equal((int)RadioProgramJobStatus.Ready, job.Status);

            var jammed = system.TryDeliver(job.JobId, new ScheduledBroadcastResult
            {
                HasTransmission = true,
                IsJammed = true,
                StationId = "station_civil_defense"
            }, 2);
            Assert.Equal(ActionResult.StatusKind.Blocked, jammed.Status);
            Assert.Equal("delivery_jammed", jammed.FailureCode);
            Assert.Equal((int)RadioProgramJobStatus.Ready, job.Status);

            var delivered = system.TryDeliver(job.JobId, OkDelivery("station_civil_defense", "bcast_morning"), 2);
            Assert.Equal(ActionResult.StatusKind.Success, delivered.Status);
            Assert.Equal((int)RadioProgramJobStatus.Delivered, job.Status);
            Assert.Equal("bcast_morning", job.LastDeliveryBroadcastId);
            Assert.Equal(1, system.State.TotalDelivered);
            Assert.Single(system.State.FollowUps);
        }

        [Fact]
        public void TryDeliver_StartsPropagandaCampaign_WhenWired()
        {
            var system = new RadioProgramProductionSystem(CreateCatalog(), CreateStations());
            string? startedCampaign = null;
            system.StartPropagandaCampaign = (campaignId, day) =>
            {
                startedCampaign = campaignId;
                return true;
            };

            system.StartPrep("radio_prog_shelter_morning_bulletin", "survivor_announcer", 1);
            system.TickDay(2);
            var job = system.GetActiveJobs()[0];
            var res = system.TryDeliver(job.JobId, OkDelivery("station_civil_defense"), 2);

            Assert.Equal(ActionResult.StatusKind.Success, res.Status);
            Assert.True(job.PropagandaStarted);
            Assert.Equal("psyops_campaign_scavenger_lantern_hour", startedCampaign);
        }

        [Fact]
        public void TryDeliver_StationMismatch_Blocked()
        {
            var system = new RadioProgramProductionSystem(CreateCatalog(), CreateStations());
            system.StartPrep("radio_prog_shelter_morning_bulletin", "survivor_announcer", 1);
            system.TickDay(2);
            var job = system.GetActiveJobs()[0];

            var res = system.TryDeliver(job.JobId, OkDelivery("station_open_classroom"), 2);
            Assert.Equal(ActionResult.StatusKind.Blocked, res.Status);
            Assert.Equal("station_mismatch", res.FailureCode);
        }

        [Fact]
        public void CancelJob_BeforeDelivery()
        {
            var system = new RadioProgramProductionSystem(CreateCatalog(), CreateStations());
            system.StartPrep("radio_prog_classroom_evening_story", "survivor_teacher", 1);
            var job = system.GetActiveJobs()[0];
            var cancel = system.CancelJob(job.JobId);
            Assert.Equal(ActionResult.StatusKind.Success, cancel.Status);
            Assert.Equal((int)RadioProgramJobStatus.Cancelled, job.Status);
            Assert.Equal(1, system.State.TotalCancelled);
            Assert.Empty(system.GetActiveJobs());
        }

        [Fact]
        public void CaptureRestore_RoundTripsJobsAndFollowUps()
        {
            var system = new RadioProgramProductionSystem(CreateCatalog(), CreateStations());
            system.StartPropagandaCampaign = (_, _) => true;
            system.StartPrep("radio_prog_shelter_morning_bulletin", "survivor_announcer", 3);
            system.TickDay(4);
            var job = system.GetActiveJobs()[0];
            system.TryDeliver(job.JobId, OkDelivery("station_civil_defense", "bcast_rt"), 4);

            var saved = system.CaptureState();
            var restored = new RadioProgramProductionSystem(CreateCatalog(), CreateStations());
            restored.RestoreState(saved);

            Assert.Equal(1, restored.State.TotalDelivered);
            Assert.Single(restored.State.FollowUps);
            Assert.Equal((int)RadioProgramJobStatus.Delivered, restored.State.Jobs[0].Status);
            Assert.Equal("bcast_rt", restored.State.Jobs[0].LastDeliveryBroadcastId);
            Assert.True(restored.State.Jobs[0].PropagandaStarted);
        }

        [Fact]
        public void AuthoredCatalog_LoadsRealSlotBindings()
        {
            var dataDir = System.IO.Path.GetFullPath(
                System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory,
                    "..", "..", "..", "..", "Assets", "StreamingAssets", "Data"));
            if (!System.IO.File.Exists(System.IO.Path.Combine(dataDir, "radio_programs.json")))
                dataDir = System.IO.Path.GetFullPath("Assets/StreamingAssets/Data");

            Assert.True(System.IO.File.Exists(System.IO.Path.Combine(dataDir, "radio_programs.json")));

            var catalog = RadioProgramCatalogLoader.Load(dataDir, new FileSystemIO());
            Assert.True(catalog.All.Count >= 2);

            var stations = new RadioStationCatalog();
            Assert.True(stations.LoadFromDataDirectory(dataDir) >= 6);

            var system = new RadioProgramProductionSystem(catalog, stations)
            {
                // Slot-binding focus: equipment/cost covered by dedicated cases.
                HasRequiredEquipment = _ => true,
                TryConsumePrepCost = (_, _) => true
            };
            var res = system.StartPrep("radio_prog_shelter_morning_bulletin", "survivor_announcer", 1);
            Assert.Equal(ActionResult.StatusKind.Success, res.Status);
            Assert.Equal("slot_cd_morning", system.GetActiveJobs()[0].SlotId);
            Assert.Contains("radio_headset", catalog.Get("radio_prog_shelter_morning_bulletin")!.required_equipment_item_ids);
        }

        [Fact]
        public void StartPrep_BlocksMissingEquipment()
        {
            var catalog = CreateCatalog();
            catalog.programs[0].required_equipment_item_ids = new List<string> { "radio_headset" };
            catalog.Index();
            var system = new RadioProgramProductionSystem(catalog, CreateStations())
            {
                HasRequiredEquipment = _ => false
            };

            var res = system.StartPrep("radio_prog_shelter_morning_bulletin", "survivor_announcer", 1);
            Assert.Equal(ActionResult.StatusKind.Blocked, res.Status);
            Assert.Equal("missing_equipment", res.FailureCode);
            Assert.Empty(system.GetActiveJobs());
        }

        [Fact]
        public void StartPrep_BlocksMissingPrepCost_AndDoesNotStartJob()
        {
            var catalog = CreateCatalog();
            catalog.programs[0].required_equipment_item_ids = new List<string> { "radio_headset" };
            catalog.programs[0].prep_cost_item_id = "aa_batteries";
            catalog.programs[0].prep_cost_count = 1;
            catalog.Index();
            var system = new RadioProgramProductionSystem(catalog, CreateStations())
            {
                HasRequiredEquipment = _ => true,
                TryConsumePrepCost = (_, _) => false
            };

            var res = system.StartPrep("radio_prog_shelter_morning_bulletin", "survivor_announcer", 1);
            Assert.Equal(ActionResult.StatusKind.Blocked, res.Status);
            Assert.Equal("missing_prep_cost", res.FailureCode);
            Assert.Empty(system.GetActiveJobs());
        }

        [Fact]
        public void StartPrep_ConsumesPrepCost_WhenWired()
        {
            var catalog = CreateCatalog();
            catalog.programs[0].required_equipment_item_ids = new List<string> { "radio_headset" };
            catalog.programs[0].prep_cost_item_id = "aa_batteries";
            catalog.programs[0].prep_cost_count = 1;
            catalog.Index();
            int consumed = 0;
            var system = new RadioProgramProductionSystem(catalog, CreateStations())
            {
                HasRequiredEquipment = ids => ids.Count == 1 && ids[0] == "radio_headset",
                TryConsumePrepCost = (itemId, count) =>
                {
                    if (itemId != "aa_batteries" || count != 1) return false;
                    consumed++;
                    return true;
                }
            };

            var res = system.StartPrep("radio_prog_shelter_morning_bulletin", "survivor_announcer", 1);
            Assert.Equal(ActionResult.StatusKind.Success, res.Status);
            Assert.Equal(1, consumed);
            Assert.Single(system.GetActiveJobs());
        }
    }
}
