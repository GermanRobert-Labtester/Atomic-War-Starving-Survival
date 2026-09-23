// SPDX-License-Identifier: MIT
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Radio;
using Xunit;

namespace Ashfall.Core.Tests.Radio
{
    public sealed class RadioAudienceResponseTests
    {
        private static RadioProgramCatalog CreateTestCatalog()
        {
            var catalog = new RadioProgramCatalog
            {
                programs = new List<RadioProgramTemplateDef>
                {
                    new RadioProgramTemplateDef
                    {
                        id = "prog_news",
                        display_name = "Morning Bulletin",
                        station_id = "station_civic",
                        slot_id = "slot_morning",
                        genre = "civilian_news",
                        prep_ticks_required = 1
                    },
                    new RadioProgramTemplateDef
                    {
                        id = "prog_story",
                        display_name = "Night Story",
                        station_id = "station_civic",
                        slot_id = "slot_night",
                        genre = "storytelling",
                        prep_ticks_required = 1
                    },
                    new RadioProgramTemplateDef
                    {
                        id = "prog_entertainment",
                        display_name = "Shelter Comedy Hour",
                        station_id = "station_civic",
                        slot_id = "slot_afternoon",
                        genre = "entertainment",
                        prep_ticks_required = 1
                    }
                }
            };
            catalog.Index();
            return catalog;
        }

        private static RadioStationCatalog CreateTestStations()
        {
            var stations = new RadioStationCatalog();
            stations.Register(new RadioStationDefinition
            {
                StationId = "station_civic",
                DisplayName = "Civic Radio",
                FrequencyMhz = 95.5f,
                Schedule = new List<RadioProgramSlot>
                {
                    new RadioProgramSlot { SlotId = "slot_morning", StartHour = 6, EndHour = 10 },
                    new RadioProgramSlot { SlotId = "slot_afternoon", StartHour = 12, EndHour = 16 },
                    new RadioProgramSlot { SlotId = "slot_night", StartHour = 20, EndHour = 23 }
                }
            });
            return stations;
        }

        [Fact]
        public void PresenterCapability_ScalesMoraleDelta()
        {
            var catalog = CreateTestCatalog();
            var system = new RadioProgramProductionSystem(catalog, CreateTestStations());

            system.PresenterCapabilityProvider = presenterId => presenterId == "pro_speaker" ? 1.5f : 0.8f;

            // Story base is 3.5. With pro_speaker (1.5), expected = 5.25
            system.StartPrep("prog_story", "pro_speaker", 1);
            system.TickDay(2);
            var job = system.GetActiveJobs()[0];

            var delivery = new ScheduledBroadcastResult
            {
                HasTransmission = true,
                StationId = "station_civic",
                BroadcastId = "bcast_story",
                SignalStrength = 7
            };

            var deliverResult = system.TryDeliver(job.JobId, delivery, 2);
            Assert.Equal(ActionResult.StatusKind.Success, deliverResult.Status);
            Assert.Equal(5.25f, job.AudienceMoraleDelta);
            Assert.Equal("District", job.AudienceReachGrade);
            Assert.True(job.OpportunitySpawned);
        }

        [Theory]
        [InlineData(4, "Local", false)]
        [InlineData(6, "District", true)]
        [InlineData(9, "Regional", true)]
        public void AudienceReach_And_OpportunityCreation_DerivedFromSignalStrength(int signal, string expectedReach, bool expectedOpportunity)
        {
            var catalog = CreateTestCatalog();
            var system = new RadioProgramProductionSystem(catalog, CreateTestStations());
            system.PresenterCapabilityProvider = _ => 1.0f;

            system.StartPrep("prog_news", "regular_announcer", 1);
            system.TickDay(2);
            var job = system.GetActiveJobs()[0];

            var delivery = new ScheduledBroadcastResult
            {
                HasTransmission = true,
                StationId = "station_civic",
                BroadcastId = "bcast_news",
                SignalStrength = signal
            };

            var res = system.TryDeliver(job.JobId, delivery, 2);
            Assert.Equal(ActionResult.StatusKind.Success, res.Status);
            Assert.Equal(expectedReach, job.AudienceReachGrade);
            Assert.Equal(expectedOpportunity, job.OpportunitySpawned);
        }

        [Fact]
        public void Callbacks_FiredOnDelivery_ForMoraleAndReputation()
        {
            var catalog = CreateTestCatalog();
            var system = new RadioProgramProductionSystem(catalog, CreateTestStations());
            system.PresenterCapabilityProvider = _ => 1.2f;

            float appliedMorale = 0f;
            system.ApplyShelterMoraleDelta = m => appliedMorale += m;

            int repDeltasFired = 0;
            system.ApplyFactionReputationDelta = (faction, rep) => repDeltasFired++;

            AudienceResponseResult? responseResult = null;
            system.OnAudienceResponseCalculated += (j, resp) => responseResult = resp;

            system.StartPrep("prog_entertainment", "comedian", 1);
            system.TickDay(2);
            var job = system.GetActiveJobs()[0];

            var delivery = new ScheduledBroadcastResult
            {
                HasTransmission = true,
                StationId = "station_civic",
                BroadcastId = "bcast_ent",
                SignalStrength = 8
            };

            var res = system.TryDeliver(job.JobId, delivery, 2);
            Assert.Equal(ActionResult.StatusKind.Success, res.Status);

            // 4.0f base * 1.2f capability = 4.8f
            Assert.Equal(4.8f, appliedMorale);
            Assert.NotNull(responseResult);
            Assert.Equal("Regional", responseResult!.ReachGrade);
            Assert.True(responseResult.OpportunityCreated);
        }

        [Fact]
        public void FollowUpHook_ResolutionLifecycle()
        {
            var catalog = CreateTestCatalog();
            var system = new RadioProgramProductionSystem(catalog, CreateTestStations());

            system.StartPrep("prog_news", "reporter", 1);
            system.TickDay(2);
            var job = system.GetActiveJobs()[0];

            system.TryDeliver(job.JobId, new ScheduledBroadcastResult
            {
                HasTransmission = true,
                StationId = "station_civic",
                BroadcastId = "bcast_hook",
                SignalStrength = 7
            }, 2);

            var unres = system.GetUnresolvedFollowUps();
            Assert.Single(unres);
            var hookId = unres[0].HookId;

            // Resolve hook
            var resolveRes = system.ResolveFollowUpHook(hookId, "scout_dispatched", 3);
            Assert.Equal(ActionResult.StatusKind.Success, resolveRes.Status);
            Assert.Equal("scout_dispatched", unres[0].ResolutionAction);
            Assert.Equal(3, unres[0].ResolvedDay);
            Assert.True(unres[0].Resolved);

            // Now unresolved list is empty
            Assert.Empty(system.GetUnresolvedFollowUps());

            // Attempting to resolve again should be blocked
            var secondResolve = system.ResolveFollowUpHook(hookId, "re_dispatch", 4);
            Assert.Equal(ActionResult.StatusKind.Blocked, secondResolve.Status);
            Assert.Equal("already_resolved", secondResolve.FailureCode);
        }
    }
}
