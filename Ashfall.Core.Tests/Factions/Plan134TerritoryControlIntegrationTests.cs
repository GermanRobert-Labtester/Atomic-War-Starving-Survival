#nullable enable
// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Factions;
using Xunit;

namespace Ashfall.Core.Tests.Factions
{
    public sealed class Plan134TerritoryControlIntegrationTests
    {
        private static string ResolveCatalogPath(string filename)
        {
            string path = Path.Combine(AppContext.BaseDirectory, "Data", filename);
            if (!File.Exists(path))
            {
                path = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data", filename));
            }
            if (!File.Exists(path))
            {
                path = Path.Combine("Assets", "StreamingAssets", "Data", filename);
            }
            return path;
        }

        [Fact]
        public void Catalogs_LoadAndParseTerritoriesAndSupplyLines()
        {
            string territoryPath = ResolveCatalogPath("faction_territory.json");
            string supplyLinePath = ResolveCatalogPath("supply_lines.json");

            Assert.True(File.Exists(territoryPath), $"Territory catalog missing at {territoryPath}");
            Assert.True(File.Exists(supplyLinePath), $"Supply lines catalog missing at {supplyLinePath}");

            var system = TerritoryControlSystem.FromJson(
                File.ReadAllText(territoryPath),
                File.ReadAllText(supplyLinePath));

            var territories = system.GetAllTerritories();
            Assert.NotEmpty(territories);
            Assert.Contains(territories, t => t.Faction == "faction_the_office");

            var supplyLines = system.GetAllSupplyLines();
            Assert.NotEmpty(supplyLines);
            Assert.Contains(supplyLines, l => l.OwningFactionId == "faction_the_office");
        }

        [Fact]
        public void LocationState_InitializesFromTerritories_AndSupportsFortification()
        {
            string territoryPath = ResolveCatalogPath("faction_territory.json");
            string supplyLinePath = ResolveCatalogPath("supply_lines.json");

            var system = TerritoryControlSystem.FromJson(
                File.ReadAllText(territoryPath),
                File.ReadAllText(supplyLinePath));

            var locState = system.GetLocationState("loc_settlement_nine_rails");
            Assert.NotNull(locState);
            Assert.Equal("faction_the_office", locState!.ControllingFactionId);
            Assert.Equal(0, locState.FortificationLevel);

            string? fortifiedLoc = null;
            int newFortLevel = 0;
            system.OnLocationFortifiedSeam = (loc, lvl) =>
            {
                fortifiedLoc = loc;
                newFortLevel = lvl;
            };

            // Fortify to level 1 (+10 control strength)
            int initialStrength = locState.ControlStrength;
            bool fortified = system.FortifyLocation("loc_settlement_nine_rails", 1);
            Assert.True(fortified);
            Assert.Equal("loc_settlement_nine_rails", fortifiedLoc);
            Assert.Equal(1, newFortLevel);
            Assert.Equal(1, locState.FortificationLevel);
            Assert.Equal(Math.Min(100, initialStrength + 10), locState.ControlStrength);
        }

        [Fact]
        public void ContestLocation_DeterministicallyResolvesControlShift()
        {
            string territoryPath = ResolveCatalogPath("faction_territory.json");
            string supplyLinePath = ResolveCatalogPath("supply_lines.json");

            var system = TerritoryControlSystem.FromJson(
                File.ReadAllText(territoryPath),
                File.ReadAllText(supplyLinePath));

            string? shiftedLoc = null;
            string? oldFaction = null;
            string? newFaction = null;
            system.OnTerritoryControlChangedSeam = (loc, oldF, newF) =>
            {
                shiftedLoc = loc;
                oldFaction = oldF;
                newFaction = newF;
            };

            // Attack with overwhelming power and favorable seed
            var rng = new SeededRng(100);
            bool captured = system.ContestLocation("loc_weighbridge", "faction_iron_raiders", attackPower: 250, rng: rng, currentDay: 10);
            Assert.True(captured);
            Assert.Equal("loc_weighbridge", shiftedLoc);
            Assert.Equal("faction_the_office", oldFaction);
            Assert.Equal("faction_iron_raiders", newFaction);

            var updatedState = system.GetLocationState("loc_weighbridge");
            Assert.NotNull(updatedState);
            Assert.Equal("faction_iron_raiders", updatedState!.ControllingFactionId);
            Assert.False(updatedState.IsContested);
        }

        [Fact]
        public void RaidSupplyLine_DisruptsCorridor_AndDegradesDestinationControl()
        {
            string territoryPath = ResolveCatalogPath("faction_territory.json");
            string supplyLinePath = ResolveCatalogPath("supply_lines.json");

            var system = TerritoryControlSystem.FromJson(
                File.ReadAllText(territoryPath),
                File.ReadAllText(supplyLinePath));

            string? disruptedLine = null;
            SupplyLineStatus reportedStatus = SupplyLineStatus.Active;
            system.OnSupplyLineStatusChangedSeam = (id, st) =>
            {
                disruptedLine = id;
                reportedStatus = st;
            };

            var lineState = system.GetSupplyLineState("supply_office_rail_trunk");
            Assert.NotNull(lineState);
            Assert.Equal(SupplyLineStatus.Active, lineState!.Status);

            var rng = new SeededRng(42);
            bool raided = system.RaidSupplyLine("supply_office_rail_trunk", raidIntensity: 90, rng: rng);
            Assert.True(raided);
            Assert.Equal("supply_office_rail_trunk", disruptedLine);
            Assert.Equal(SupplyLineStatus.Severed, lineState.Status);
            Assert.Equal(SupplyLineStatus.Severed, reportedStatus);

            // Restoration restores line to Active
            bool restored = system.RestoreSupplyLine("supply_office_rail_trunk");
            Assert.True(restored);
            Assert.Equal(SupplyLineStatus.Active, lineState.Status);
        }

        [Fact]
        public void TickDay_DeliversCargo_AndReinforcesDestinationControl()
        {
            string territoryPath = ResolveCatalogPath("faction_territory.json");
            string supplyLinePath = ResolveCatalogPath("supply_lines.json");

            var system = TerritoryControlSystem.FromJson(
                File.ReadAllText(territoryPath),
                File.ReadAllText(supplyLinePath));

            string? deliveredLine = null;
            int deliveredCargo = 0;
            system.OnSupplyLineDeliveredSeam = (id, amt) =>
            {
                deliveredLine = id;
                deliveredCargo = amt;
            };

            var destState = system.GetLocationState("loc_cut_arsenal_ruin");
            Assert.NotNull(destState);
            int prevStrength = destState!.ControlStrength;

            system.TickDay(currentDay: 5);

            var lineState = system.GetSupplyLineState("supply_office_rail_trunk");
            Assert.NotNull(lineState);
            Assert.Equal(5, lineState!.LastDeliveredDay);
            Assert.True(lineState.TotalDelivered > 0);
            Assert.True(destState.ControlStrength >= prevStrength);
        }
    }
}
