using System.IO;
using Xunit;
using Ashfall.Core;

namespace Ashfall.Core.Tests
{
    public class ExpansionsIntegrationTests
    {
        private static string DataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found))
                return found;
            if (CatalogLocator.TryFindDataDirectory(System.AppContext.BaseDirectory, out found))
                return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        [Fact]
        public void HoldfastExpansionHeadlessSmoke()
        {
            var report = HoldfastHeadlessDemo.Run(DataDir());
            Assert.True(report.Passed, report.Summary);
            Assert.True(report.LocationCount >= 26);
            Assert.Equal(10, report.QuestCount);
        }

        [Fact]
        public void DutyRosterExpansionHeadlessSmoke()
        {
            var report = DutyRosterHeadlessDemo.Run(DataDir());
            Assert.True(report.Passed, report.Summary);
            Assert.True(report.LocationCount >= 4);
            Assert.True(report.QuestCount >= 2);
        }

        [Fact]
        public void StandingRecordExpansionHeadlessSmoke()
        {
            var report = StandingRecordHeadlessDemo.Run(DataDir());
            Assert.True(report.Passed, report.Summary);
            Assert.True(report.LocationCount >= 2);
        }

        [Fact]
        public void NobodysCharterExpansionHeadlessSmoke()
        {
            var report = CrossingHeadlessDemo.Run(DataDir());
            Assert.True(report.Passed, report.Summary);
            Assert.True(report.LocationCount >= 7);
            Assert.True(report.QuestCount >= 2);
        }

        [Fact]
        public void NobodysCharter_Sprint0_VouchCardAndSchema()
        {
            var session = CrossingSession.Load(DataDir());

            // The opening quest must exist before first_weigh is legal, and
            // must match the bible card spec (expansion_04 §4.1).
            var vouch = session.Catalog.GetQuest(CrossingIds.TheVouch);
            Assert.NotNull(vouch);
            Assert.Equal("lore_nc_the_vouch", vouch.knowledge_key);
            Assert.True(vouch.min_day >= 70, "bible soft gate: Day 70+ (or grievance or Ostrowski)");

            // All Crossing cards must fit the live danger / rads bands.
            for (int i = 0; i < session.Catalog.Locations.Count; i++)
            {
                var loc = session.Catalog.Locations[i];
                if (loc == null) continue;
                Assert.InRange(loc.dangerLevel,
                    CrossingCatalogLoader.MinDanger, CrossingCatalogLoader.MaxDanger);
                Assert.InRange(loc.baseRadsPerHour,
                    CrossingCatalogLoader.MinRads, CrossingCatalogLoader.MaxRads);
            }

            // The records room must not spoil the Charter reveal.
            // (Engine-agnostic CrossingIds has no RecordsRoom constant yet;
            // the host master id is CrossingIds.Locations.RecordsRoom.)
            var records = session.Catalog.GetLocation("loc_crossing_records_room");
            Assert.NotNull(records);
            Assert.DoesNotContain("three pages", records.description);
            Assert.DoesNotContain("original Charter", records.description);
        }

        [Fact]
        public void MasterExpansionSuiteAllPass()
        {
            var report = ExpansionMasterSession.RunAllSelfTests(DataDir());
            Assert.True(report.Passed, report.Summary);
            Assert.True(report.PassedCount > 40);
            Assert.Equal(0, report.FailedCount);
        }
    }
}
