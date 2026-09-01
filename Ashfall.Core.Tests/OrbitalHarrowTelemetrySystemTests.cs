using System;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.IO;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class OrbitalHarrowTelemetrySystemTests
    {
        private static string ResolveDataDir()
        {
            string baseDir = AppContext.BaseDirectory;
            string probe = Path.Combine(baseDir, "StreamingAssets", "Data");
            if (Directory.Exists(probe)) return probe;

            probe = Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data");
            if (Directory.Exists(probe)) return Path.GetFullPath(probe);

            probe = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data");
            if (Directory.Exists(probe)) return Path.GetFullPath(probe);

            return string.Empty;
        }

        [Fact]
        public void ActivateTelemetry_EnablesSystem()
        {
            var oh = Create(out _);
            oh.ActivateTelemetry(1);
            Assert.True(oh.State.telemetryActive);
        }

        [Fact]
        public void ScheduleImpact_CreatesWarning()
        {
            var oh = Create(out _);
            oh.ScheduleImpact(10, 5, 25f);
            Assert.Single(oh.State.warnings);
            Assert.True(oh.HasPendingImpact);
        }

        [Fact]
        public void Brace_MitigatesImpact()
        {
            var oh = Create(out _);
            oh.ScheduleImpact(10, 5, 25f);
            var r = oh.Brace("concrete", 5);
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            Assert.True(oh.State.isBraced);
        }

        [Fact]
        public void TickDay_OnImpactDay_Resolves()
        {
            var oh = Create(out _);
            oh.ScheduleImpact(10, 5, 25f);
            bool resolved = false;
            oh.OnImpactResolved += (_, _) => resolved = true;
            oh.TickDay(10);
            Assert.True(resolved);
            Assert.False(oh.HasPendingImpact);
        }

        [Fact]
        public void Brace_WhenNoImpact_Blocks()
        {
            var oh = Create(out _);
            var r = oh.Brace("concrete", 5);
            Assert.Equal(ActionResult.StatusKind.Blocked, r.Status);
        }

        [Fact]
        public void CaptureRestoreState_PreservesImpact()
        {
            var oh = Create(out _);
            oh.ScheduleImpact(10, 5, 25f);
            var state = oh.CaptureState();
            Assert.Equal(10, state.nextImpactDay);

            var oh2 = Create(out _);
            oh2.RestoreState(state);
            Assert.True(oh2.HasPendingImpact);
        }

        [Fact]
        public void TelemetryCatalog_ContainsTwelveCanonicalEvents()
        {
            string dataDir = ResolveDataDir();
            var events = OrbitalHarrowCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());

            Assert.NotNull(events);
            Assert.Equal(12, events.Count);

            // 4 kinetic, 2 cluster, 2 emp, 2 dead hand, 2 false alarm
            var kinetic = events.Where(e => e.id.Contains("kinetic")).ToList();
            Assert.Equal(4, kinetic.Count);

            var cluster = events.Where(e => e.id.Contains("cluster")).ToList();
            Assert.Equal(2, cluster.Count);

            var emp = events.Where(e => e.id.Contains("emp")).ToList();
            Assert.Equal(2, emp.Count);

            var deadHand = events.Where(e => e.id.Contains("dead_hand")).ToList();
            Assert.Equal(2, deadHand.Count);

            var falseAlarms = events.Where(e => e.is_false_positive).ToList();
            Assert.Equal(2, falseAlarms.Count);
        }

        [Fact]
        public void FalsePositiveEvents_ResolveWithoutDamageOrBreach()
        {
            string dataDir = ResolveDataDir();
            var events = OrbitalHarrowCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            var falseAlarm = events.First(e => e.id == "event_orbital_radar_ducting_false_alarm");

            var oh = Create(out var armor);
            oh.ActivateTelemetry(1);
            oh.ScheduleEventDef(falseAlarm, day: 4, gridX: 5);

            OrbitalImpactReport? report = null;
            oh.OnImpactDetailed += r => report = r;

            oh.TickDay(4);

            Assert.NotNull(report);
            Assert.False(report.AnyBreached);
            Assert.Equal(0f, report.TotalPenetrationDamage);
            Assert.Equal(0f, report.PowerGridDisruption);

            var cell = armor.GetCell(5);
            Assert.NotNull(cell);
            Assert.Equal(100f, cell.currentDurability); // 0 damage dealt
        }

        [Fact]
        public void DeadHandEvents_CarryRadioHooksAndRevealSites()
        {
            string dataDir = ResolveDataDir();
            var events = OrbitalHarrowCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            var deadHand = events.First(e => e.id == "event_orbital_dead_hand_repeating_ping");

            Assert.False(string.IsNullOrWhiteSpace(deadHand.radio_hook_text));
            Assert.Equal("loc_excavation_command_vault", deadHand.revealed_site_id);

            var oh = Create(out _);
            oh.ActivateTelemetry(1);
            oh.ScheduleEventDef(deadHand, day: 5, gridX: 5);

            oh.TickDay(5);

            Assert.Contains("loc_excavation_command_vault", oh.RevealedSites);
        }

        private static OrbitalHarrowTelemetrySystem Create(out SkyLayerArmorSystem armor)
        {
            armor = new SkyLayerArmorSystem();
            armor.SetCellArmor(5, CeilingMaterialTier.ReinforcedConcrete, 0.5f);
            return new OrbitalHarrowTelemetrySystem(armor, new SeededRng(42));
        }
    }
}
