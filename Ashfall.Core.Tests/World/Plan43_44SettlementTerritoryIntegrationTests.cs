// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Factions;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.World
{
    public sealed class Plan43_44SettlementTerritoryIntegrationTests
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

            return Path.Combine(baseDir, "Assets", "StreamingAssets", "Data");
        }

        [Fact]
        public void SettlementAndTerritoryCatalogs_CrossValidateAllegiancesAndControlPoints()
        {
            string dataDir = ResolveDataDir();
            var fileIO = new FileSystemIO();

            var settlementCatalog = SettlementCatalog.LoadFromDirectory(dataDir, fileIO);
            var territoryCatalog = FactionTerritoryCatalog.LoadFromDirectory(dataDir, fileIO);

            Assert.NotNull(settlementCatalog);
            Assert.NotNull(territoryCatalog);
            Assert.Equal(12, settlementCatalog.SettlementCount);
            Assert.Equal(19, territoryCatalog.TerritoryCount);
            Assert.Equal(5, territoryCatalog.ContestedZoneCount);

            // Verify every settlement maps directly to a territory control point owned by its allegiant faction
            foreach (var settlement in settlementCatalog.Settlements)
            {
                string locId = settlement.GetEffectiveLocationId();
                string allegianceFaction = settlement.GetEffectiveAllegiance();

                Assert.False(string.IsNullOrWhiteSpace(locId), $"Settlement {settlement.Id} must have an effective location ID");
                Assert.False(string.IsNullOrWhiteSpace(allegianceFaction), $"Settlement {settlement.Id} must have an effective allegiance");

                bool territoryFound = territoryCatalog.TryGetTerritoryByFaction(allegianceFaction, out var territory);
                Assert.True(territoryFound, $"Allegiant faction '{allegianceFaction}' for settlement '{settlement.Id}' must have a territory definition");

                // The settlement location must be registered in the territory's control points
                Assert.Contains(locId, territory.control_points);

                // Trade tax and travel safety should be within valid bounds
                Assert.InRange(territory.trade_tax, 0.0f, 1.0f);
                Assert.InRange(territory.travel_safety, 0.0f, 1.0f);
            }
        }

        [Fact]
        public void TerritoryControlSystem_SimulatesSettlementContestAndFortification()
        {
            string dataDir = ResolveDataDir();
            string territoryJson = File.ReadAllText(Path.Combine(dataDir, "faction_territory.json"));
            string supplyLineJson = File.ReadAllText(Path.Combine(dataDir, "supply_lines.json"));

            var territorySystem = TerritoryControlSystem.FromJson(territoryJson, supplyLineJson);
            Assert.NotNull(territorySystem);

            // Nine Rails settlement location check
            string settlementLoc = "loc_settlement_nine_rails";
            var locState = territorySystem.GetLocationState(settlementLoc);
            Assert.NotNull(locState);
            Assert.Equal("faction_the_office", locState!.ControllingFactionId);

            // Fortify Nine Rails
            string? fortifiedLoc = null;
            int fortifiedLevel = -1;
            territorySystem.OnLocationFortifiedSeam = (loc, lvl) =>
            {
                fortifiedLoc = loc;
                fortifiedLevel = lvl;
            };

            bool fortSuccess = territorySystem.FortifyLocation(settlementLoc, 1);
            Assert.True(fortSuccess);
            Assert.Equal(settlementLoc, fortifiedLoc);
            Assert.Equal(1, fortifiedLevel);
            Assert.Equal(1, locState.FortificationLevel);

            // Contest Nine Rails with overwhelming power
            string? contestedLoc = null;
            string? capturedFrom = null;
            string? capturedBy = null;
            territorySystem.OnTerritoryControlChangedSeam = (loc, oldF, newF) =>
            {
                contestedLoc = loc;
                capturedFrom = oldF;
                capturedBy = newF;
            };

            var rng = new SeededRng(1337);
            bool shifted = territorySystem.ContestLocation(
                settlementLoc,
                attackingFactionId: "faction_iron_raiders",
                attackPower: 500, // overwhelming force
                rng: rng,
                currentDay: 5);

            Assert.True(shifted);
            Assert.Equal(settlementLoc, contestedLoc);
            Assert.Equal("faction_the_office", capturedFrom);
            Assert.Equal("faction_iron_raiders", capturedBy);
            Assert.Equal("faction_iron_raiders", locState.ControllingFactionId);
        }

        [Fact]
        public void SettlementCatalog_QuestLifecycle_AndStateSaveRestoreRoundTrip()
        {
            string dataDir = ResolveDataDir();
            var fileIO = new FileSystemIO();

            var catalog = SettlementCatalog.LoadFromDirectory(dataDir, fileIO);
            Assert.NotNull(catalog);

            // Check all quests are available on day 1
            if (catalog.Quests.Count > 0)
            {
                var sampleQuest = catalog.Quests.First();
                Assert.True(catalog.IsQuestAvailable(sampleQuest.Id, currentDay: 1));

                // Complete quest on day 1
                catalog.CompleteQuest(sampleQuest.Id, currentDay: 1);
                Assert.Equal(1, catalog.GetCompletedQuestCount(sampleQuest.Id));
                Assert.False(catalog.IsQuestAvailable(sampleQuest.Id, currentDay: 1));
                Assert.True(catalog.IsQuestAvailable(sampleQuest.Id, currentDay: 1 + sampleQuest.CooldownDays + 1));

                // Round-trip state capture & restore
                var savedState = catalog.CaptureState();
                Assert.NotNull(savedState);
                Assert.True(savedState.CompletedQuestCounts.ContainsKey(sampleQuest.Id));
                Assert.Equal(1, savedState.CompletedQuestCounts[sampleQuest.Id]);

                var restoredCatalog = SettlementCatalog.LoadFromDirectory(dataDir, fileIO);
                restoredCatalog.RestoreState(savedState);

                Assert.Equal(1, restoredCatalog.GetCompletedQuestCount(sampleQuest.Id));
                Assert.False(restoredCatalog.IsQuestAvailable(sampleQuest.Id, currentDay: 1));
                Assert.True(restoredCatalog.IsQuestAvailable(sampleQuest.Id, currentDay: 1 + sampleQuest.CooldownDays + 1));
            }
        }
    }
}
