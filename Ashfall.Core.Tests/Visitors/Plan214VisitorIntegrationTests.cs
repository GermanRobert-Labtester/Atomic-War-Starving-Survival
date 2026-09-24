// SPDX-License-Identifier: MIT
using System.IO;
using System.Linq;
using Ashfall.Core.Visitors;
using Xunit;

namespace Ashfall.Core.Tests.Visitors
{
    public sealed class Plan214VisitorIntegrationTests
    {
        [Fact]
        public void LoadCatalog_LoadsAllTemplates_FromValidJson()
        {
            var system = new VisitorIntegrationSystem();
            string path = Path.Combine("..", "..", "..", "..", "Assets", "StreamingAssets", "Data", "visitor_templates.json");
            if (!File.Exists(path))
            {
                path = Path.Combine("Assets", "StreamingAssets", "Data", "visitor_templates.json");
            }
            Assert.True(File.Exists(path), $"Catalog not found at {path}");

            string json = File.ReadAllText(path);
            system.LoadCatalog(json);

            Assert.Equal(5, system.Templates.Count);
            var refugee = system.GetTemplate("visitor_tpl_distressed_refugee");
            Assert.NotNull(refugee);
            Assert.Equal(VisitorType.Refugee, refugee.ParseType());
            Assert.Equal(HousingType.TemporaryBunk, refugee.ParseHousing());
            Assert.Equal(14, refugee.BaseIntegrationDays);

            var trader = system.GetTemplate("visitor_tpl_itinerant_trader");
            Assert.NotNull(trader);
            Assert.Equal(VisitorType.Trader, trader.ParseType());
            Assert.Equal(HousingType.GuestSuite, trader.ParseHousing());
        }

        [Fact]
        public void AdmitVisitor_InitializesWithProcessingStatus()
        {
            var system = new VisitorIntegrationSystem();

            VisitorRecord? admittedEvent = null;
            system.OnVisitorAdmitted += v => admittedEvent = v;

            var visitor = system.AdmitVisitor(
                name: "Kaelen Mercer",
                type: VisitorType.Refugee,
                admittedBy: "gate_guard_1",
                currentDay: 4,
                notes: "Exhausted from storm");

            Assert.NotNull(visitor);
            Assert.Equal(admittedEvent, visitor);
            Assert.Equal("Kaelen Mercer", visitor.Name);
            Assert.Equal(VisitorType.Refugee, visitor.Type);
            Assert.Equal(VisitorStatus.Processing, visitor.Status);
            Assert.Equal("gate_guard_1", visitor.AdmittedBy);
            Assert.Equal(0f, visitor.IntegrationProgress);
            Assert.Single(system.GetActiveVisitors());
        }

        [Fact]
        public void AdmitFromTemplate_CreatesMatchingVisitor()
        {
            var system = new VisitorIntegrationSystem();
            system.LoadCatalog(@"{
                ""schema_version"": 1,
                ""templates"": [
                    {
                        ""id"": ""visitor_tpl_itinerant_trader"",
                        ""name"": ""Wasteland Caravan Trader"",
                        ""type"": ""trader"",
                        ""base_integration_days"": 3,
                        ""default_housing"": ""guest_suite"",
                        ""daily_food_consumption"": 1.2,
                        ""daily_water_consumption"": 1.5,
                        ""description"": ""Passing trader.""
                    }
                ]
            }");

            var visitor = system.AdmitFromTemplate(
                templateId: "visitor_tpl_itinerant_trader",
                name: "Silas Crane",
                admittedBy: "airlock_operator",
                currentDay: 2);

            Assert.NotNull(visitor);
            Assert.Equal("Silas Crane", visitor.Name);
            Assert.Equal(VisitorType.Trader, visitor.Type);
            Assert.Equal(HousingType.GuestSuite, visitor.Housing);
            Assert.Equal(5, visitor.DepartureDay); // 2 + 3
        }

        [Fact]
        public void AssignHousing_UpdatesRoomAndHousingType()
        {
            var system = new VisitorIntegrationSystem();
            var visitor = system.AdmitVisitor("Elena Voss", VisitorType.Defector, "officer_3", currentDay: 1);

            bool success = system.AssignHousing(visitor.VisitorId, "room_shared_bunks_b", HousingType.SharedQuarter, currentDay: 1);

            Assert.True(success);
            Assert.Equal("room_shared_bunks_b", visitor.AssignedRoomId);
            Assert.Equal(HousingType.SharedQuarter, visitor.Housing);
        }

        [Fact]
        public void IntegrationTasks_AccelerateIntegrationProgress()
        {
            var system = new VisitorIntegrationSystem();
            var visitor = system.AdmitVisitor("Tarek", VisitorType.Refugee, "guard_1", currentDay: 1);

            var task1 = system.AssignIntegrationTask(visitor.VisitorId, "orientation", "dweller_mentor", currentDay: 1);
            Assert.NotNull(task1);
            Assert.False(task1.IsCompleted);

            bool completed = system.CompleteIntegrationTask(task1.TaskId, currentDay: 2);
            Assert.True(completed);
            Assert.True(task1.IsCompleted);
            Assert.Equal(2, task1.CompletedDay);
            Assert.Equal(25f, visitor.IntegrationProgress);
        }

        [Fact]
        public void TickDay_AdvancesIntegration_AndTriggersDepartureWhenScheduled()
        {
            var system = new VisitorIntegrationSystem();
            var trader = system.AdmitVisitor(
                name: "Marcus Cole",
                type: VisitorType.Trader,
                admittedBy: "guard_1",
                currentDay: 1,
                plannedDurationDays: 2);

            Assert.Equal(3, trader.DepartureDay); // 1 + 2
            Assert.Equal(VisitorStatus.Processing, trader.Status);

            // Day 2 tick: progress increases +5%
            system.TickDay(2);
            Assert.Equal(5f, trader.IntegrationProgress);
            Assert.Equal(VisitorStatus.Processing, trader.Status);

            // Day 3 tick: planned departure triggered
            system.TickDay(3);
            Assert.Equal(VisitorStatus.Departed, trader.Status);
            Assert.Single(system.Departures);
            Assert.Equal("Marcus Cole", system.Departures[0].VisitorName);
        }

        [Fact]
        public void RecruitVisitor_TransitionsToRecruited()
        {
            var system = new VisitorIntegrationSystem();
            var refugee = system.AdmitVisitor("Mira Quinn", VisitorType.Refugee, "overseer", currentDay: 1);

            VisitorRecord? recruitedEvent = null;
            system.OnVisitorRecruited += v => recruitedEvent = v;

            bool success = system.RecruitVisitor(refugee.VisitorId, currentDay: 10);

            Assert.True(success);
            Assert.Equal(recruitedEvent, refugee);
            Assert.Equal(VisitorStatus.Recruited, refugee.Status);
            Assert.Empty(system.GetActiveVisitors()); // Recruited is no longer active visitor
            Assert.Single(system.Departures);
            Assert.Equal(DepartureType.Recruited, system.Departures[0].DepartureType);
        }

        [Fact]
        public void AdmitVisitor_WithSourceIdentity_RoundTripsAndResolves()
        {
            var system = new VisitorIntegrationSystem();
            var visitor = system.AdmitVisitor(
                name: "Meridian Trader",
                type: VisitorType.Trader,
                admittedBy: "sentry_1",
                currentDay: 3,
                sourceVisitorId: "trader_meridian_01");

            Assert.Equal("trader_meridian_01", visitor.SourceVisitorId);
            Assert.Same(visitor, system.GetVisitorBySource("trader_meridian_01"));
            Assert.Null(system.GetVisitorBySource("trader_unknown"));

            var restored = new VisitorIntegrationSystem();
            restored.RestoreState(system.CaptureState());
            var restoredVisitor = restored.GetVisitorBySource("trader_meridian_01");
            Assert.NotNull(restoredVisitor);
            Assert.Equal(VisitorType.Trader, restoredVisitor.Type);
        }

        [Fact]
        public void GetVisitorBySource_ReturnsDepartedStayForIdempotentHandoff()
        {
            // The host de-duplicates admission on the source id, so the lookup
            // must still resolve a stay that has already closed.
            var system = new VisitorIntegrationSystem();
            var visitor = system.AdmitVisitor("Mira", VisitorType.Refugee, "g", 1, sourceVisitorId: "arrival_7");
            system.DepartVisitor(visitor.VisitorId, DepartureType.Voluntary, "left", 4);

            Assert.NotNull(system.GetVisitorBySource("arrival_7"));
            Assert.Empty(system.GetActiveVisitors());
        }

        [Fact]
        public void CaptureAndRestoreState_RoundTripsAllVisitorData()
        {
            var system = new VisitorIntegrationSystem();
            var v = system.AdmitVisitor("Gareth", VisitorType.Exile, "guard_2", currentDay: 2);
            system.AssignHousing(v.VisitorId, "bunk_3", HousingType.TemporaryBunk, currentDay: 2);
            system.AssignIntegrationTask(v.VisitorId, "medical_check", "medic_1", currentDay: 2);

            var state = system.CaptureState();
            Assert.Single(state.Visitors);
            Assert.Single(state.Tasks);

            var restored = new VisitorIntegrationSystem();
            restored.RestoreState(state);

            Assert.Single(restored.Visitors);
            var restoredVisitor = restored.GetVisitor(v.VisitorId);
            Assert.NotNull(restoredVisitor);
            Assert.Equal("Gareth", restoredVisitor.Name);
            Assert.Equal(VisitorType.Exile, restoredVisitor.Type);
            Assert.Equal("bunk_3", restoredVisitor.AssignedRoomId);
            Assert.Single(restored.Tasks);
            Assert.Equal("medical_check", restored.Tasks[0].TaskType);
        }
    }
}
