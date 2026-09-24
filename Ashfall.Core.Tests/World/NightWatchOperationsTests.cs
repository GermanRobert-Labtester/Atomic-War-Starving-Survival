// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.Defense;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.World
{
    public sealed class NightWatchOperationsTests
    {
        private static string DataFile(string name)
        {
            string candidate = Path.Combine(AppContext.BaseDirectory, "../../../..", "Assets/StreamingAssets/Data", name);
            if (File.Exists(candidate)) return candidate;
            return Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data", name);
        }

        private static string CatalogJson() => File.ReadAllText(DataFile("night_watch_operations.json"));

        [Fact]
        public void AuthoredCatalog_LoadsCompleteOperationalSurface()
        {
            var result = NightWatchOperationsCatalogLoader.LoadFromJson(CatalogJson());
            Assert.True(result.Success, string.Join("; ", result.Errors));
            var catalog = Assert.IsType<NightWatchOperationsCatalog>(result.Catalog);
            Assert.Equal(15, catalog.posts.Count);
            Assert.Equal(12, catalog.routes.Count);
            Assert.Equal(10, catalog.gate_rules.Count);
            Assert.Equal(12, catalog.detection_profiles.Count);
            Assert.Equal(8, catalog.alarm_protocols.Count);
            Assert.Equal(10, catalog.drills.Count);
            Assert.NotNull(catalog.Post("watch_post_main_gate"));
            Assert.NotNull(catalog.Route("watch_route_inner_ring"));
            Assert.NotNull(catalog.Drill("watch_drill_alarm_bell"));
        }

        [Fact]
        public void Loader_RejectsMalformedRowsAndUnknownReferences()
        {
            string json = CatalogJson();
            json = json.Replace("\"schema_version\": 1", "\"schema_version\": 2", StringComparison.Ordinal);
            json = json.Replace("\"sector_id\":\"gate\"", "\"sector_id\":\"orbit\"", StringComparison.Ordinal);
            json = json.Replace("\"drill_id\":\"watch_drill_post_manning\"", "\"drill_id\":\"watch_drill_missing\"", StringComparison.Ordinal);
            var result = NightWatchOperationsCatalogLoader.LoadFromJson(json);
            Assert.False(result.Success);
            Assert.Contains(result.Errors, x => x.Contains("schema_version"));
            Assert.Contains(result.Errors, x => x.Contains("unknown sector"));
        }

        [Fact]
        public void WorldLocationValidation_ReportsUnknownPostAndWaypoint()
        {
            var result = NightWatchOperationsCatalogLoader.LoadFromJson(CatalogJson());
            var errors = new List<string>();
            NightWatchOperationsCatalogLoader.ValidateWorldLocations(
                result.Catalog,
                new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "loc_holdfast" },
                errors);
            Assert.NotEmpty(errors);
            Assert.Contains(errors, x => x.Contains("watch_post_main_gate"));
            Assert.Contains(errors, x => x.Contains("watch_route_inner_ring"));
        }

        [Fact]
        public void Projection_UsesSignedEngineAndRepresentsContestedRisk()
        {
            var facts = new NightWatchSectorFacts
            {
                SectorId = "gate",
                PerimeterMetres = 1200,
                AssignedPatrollers = 4,
                AverageFatiguePermille = 100,
                NightVisionPermille = 500,
                ActivePostCount = 2,
                PostConditionPermille = 1000,
                AlarmOnline = true,
                GateStaffPermille = 1000,
                GateMechanismPermille = 1000,
                GateDrillRecencyPermille = 1000,
                SoundSensorCount = 2,
                AcousticConfidencePermille = 500
            };
            var ready = NightWatchReadinessProjection.Evaluate(facts);
            Assert.True(ready.PatrolCoverage.MeetsReadinessThreshold);
            Assert.True(ready.GateReadiness.IsGateReady);
            Assert.True(ready.OverallReadinessPermille >= 700);

            facts.TerritoryContested = true;
            facts.AssignedPatrollers = 0;
            var blind = NightWatchReadinessProjection.Evaluate(facts);
            Assert.Equal(PatrolCoverageGrade.Blind, blind.PatrolCoverage.CoverageGrade);
            Assert.True(blind.TerritoryContested);
            Assert.True(blind.OverallReadinessPermille < ready.OverallReadinessPermille);
        }

        [Fact]
        public void PerimeterDefense_WatchOperationsRoundTripAndDailyMaintenance()
        {
            var catalog = NightWatchOperationsCatalogLoader.LoadFromJson(CatalogJson()).Catalog!;
            var system = new PerimeterDefenseSystem(
                Array.Empty<PerimeterDefenseDefinition>(),
                new Ashfall.Core.Inventory.Inventory(),
                new Ashfall.Core.SeededRng(36));
            system.BindWatchCatalog(catalog);
            Assert.True(system.SetWatchPostCondition("watch_post_main_gate", 500).IsSuccess);
            Assert.False(system.RecordWatchDebrief("watch_route_inner_ring", 3).IsSuccess);
            Assert.True(system.RecordWatchRoute("watch_route_inner_ring", 4, debriefed: false).IsSuccess);
            Assert.True(system.RecordWatchDrill("watch_drill_alarm_bell", 4, passed: true).IsSuccess);
            Assert.True(system.RecordWatchDrill("watch_drill_blackout", 4, passed: false).IsSuccess);
            system.TickDay(5, severeWeather: true);

            var saved = system.CaptureState();
            var serializer = new Ashfall.Core.SystemTextJsonSerializer();
            var persisted = serializer.Deserialize<PerimeterDefenseSave>(serializer.Serialize(saved));
            Assert.NotNull(persisted);
            var restored = new PerimeterDefenseSystem(
                Array.Empty<PerimeterDefenseDefinition>(),
                new Ashfall.Core.Inventory.Inventory(),
                new Ashfall.Core.SeededRng(99));
            restored.BindWatchCatalog(catalog);
            restored.RestoreState(persisted);

            Assert.Equal(495, restored.FindWatchPost("watch_post_main_gate")!.condition_permille);
            Assert.Equal(1, restored.FindWatchRoute("watch_route_inner_ring")!.rounds_completed);
            Assert.True(restored.FindWatchRoute("watch_route_inner_ring")!.debrief_pending);
            Assert.Equal(1, restored.FindWatchDrill("watch_drill_alarm_bell")!.passes);
            Assert.Equal(1, restored.FindWatchDrill("watch_drill_blackout")!.failures);

            var newer = restored.CaptureState();
            newer.watch_operations.schema_version = 99;
            Assert.Throws<InvalidOperationException>(() => restored.RestoreState(newer));
        }

        [Fact]
        public void WatchAlarmCanBeEstablishedBeforePhysicalPerimeterConstruction()
        {
            var catalog = NightWatchOperationsCatalogLoader.LoadFromJson(CatalogJson()).Catalog!;
            var system = new PerimeterDefenseSystem(
                Array.Empty<PerimeterDefenseDefinition>(),
                new Ashfall.Core.Inventory.Inventory(),
                new Ashfall.Core.SeededRng(361));
            system.BindWatchCatalog(catalog);

            var armed = system.ToggleSectorAlarm("gate");
            Assert.True(armed.IsSuccess, armed.FailureCode);
            Assert.True(system.FindSector("gate")!.alarm_armed);
            var disarmed = system.ToggleSectorAlarm("gate");
            Assert.True(disarmed.IsSuccess, disarmed.FailureCode);
            Assert.False(system.FindSector("gate")!.alarm_armed);
        }

        [Fact]
        public void OlderPerimeterSaveWithoutWatchOperationsGetsSafeDefaults()
        {
            var catalog = NightWatchOperationsCatalogLoader.LoadFromJson(CatalogJson()).Catalog!;
            var system = new PerimeterDefenseSystem(
                Array.Empty<PerimeterDefenseDefinition>(),
                new Ashfall.Core.Inventory.Inventory(),
                new Ashfall.Core.SeededRng(1));
            system.BindWatchCatalog(catalog);
            system.RestoreState(new PerimeterDefenseSave { schema_version = 2 });
            Assert.Equal(15, system.WatchPosts.Count);
            Assert.All(system.WatchPosts, x => Assert.InRange(x.condition_permille, 0, 1000));
            Assert.Equal(900, system.FindWatchPost("watch_post_main_gate")!.condition_permille);
        }
    }
}
