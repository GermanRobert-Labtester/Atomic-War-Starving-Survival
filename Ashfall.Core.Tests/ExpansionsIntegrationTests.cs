using System.IO;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Combat;

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
            Assert.True(report.QuestCount >= 10);
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
        public void DiseaseExpansionHeadlessSmoke()
        {
            // The migrated Disease Expansion: catalog loads, quarantine stalls
            // the vector, outbreaks declare/contain, protocols block vectors,
            // outcomes resolve deterministically, save round-trips.
            var report = DiseaseHeadlessDemo.Run(DataDir());
            Assert.True(report.Passed, report.Summary);
            Assert.Equal(0, report.FailedCount);
        }

        [Fact]
        public void MasterSession_DiseaseWiredAndOutbreakTick()
        {
            // The orchestrator (ExpansionMasterSession.Load) wires the Disease
            // engine + catalog; the master daily tick advances the ward.
            var session = ExpansionMasterSession.Load(DataDir());
            var disease = session.Disease;
            Assert.NotNull(disease);
            Assert.True(disease.Catalog.Count >= 4, "disease catalog bound by the orchestrator");
            Assert.True(disease.Catalog.GetById("disease_cholera") != null);

            // The daily tick progresses the ward deterministically.
            for (int d = 1; d <= 30; d++)
                session.TickDaily(Ashfall.Core.WeatherKind.Overcast, -12f);
            Assert.True(disease.State.rngSeed != 0, "disease engine ticked through the orchestrator");
        }

        [Fact]
        public void CombatExpansionHeadlessSmoke()
        {
            // The migrated Combat Expansion: JSON catalog authorises the weapon/
            // ammo/material tables; the vertical-slice demo proves ballistics,
            // weapon condition/jam, determinism and save round-trip.
            var report = CombatHeadlessDemo.Run();
            Assert.True(report.Passed, report.Summary);
            Assert.Equal(0, report.FailedCount);
        }

        [Fact]
        public void MasterExpansionSuiteAllPass()
        {
            var report = ExpansionMasterSession.RunAllSelfTests(DataDir());
            Assert.True(report.Passed, report.Summary);
            Assert.True(report.PassedCount > 40);
            Assert.Equal(0, report.FailedCount);
        }

        [Fact]
        public void MasterSession_FoundryWiredAndConsequencesTick()
        {
            // The orchestrator (ExpansionMasterSession.Load) wires the Foundry:
            // static catalogs, blueprint cycle, treaty anchors, daily tick.
            var session = ExpansionMasterSession.Load(DataDir());
            var foundry = session.SilentFoundry;
            Assert.NotNull(foundry);
            Assert.True(foundry.Catalog.ProductCount >= 8, "foundry catalog bound by the orchestrator");
            Assert.Equal(4, foundry.State.maintenanceCycleDays); // blueprint anchor

            // Day-agnostic treaty assessment through the master daily tick.
            foundry.Unlock(1);
            for (int d = 1; d <= 280; d++)
                session.TickDaily(Ashfall.Core.WeatherKind.Overcast, -12f);
            Assert.True(foundry.IsConsequenceApplied("treaty_brine_pipe_and_iodine_exchange", 280),
                "master daily tick reaches the day-280 accord consequence");
            Assert.True(foundry.GuildStanding < 0f, "standing consequence applied via the orchestrator");
        }
    }
}
