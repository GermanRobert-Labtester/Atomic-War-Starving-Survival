// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Linq;
using Ashfall.Core.Expeditions;
using Xunit;

namespace Ashfall.Core.Tests.Plan160Colony
{
    public sealed class Plan160ColonyIntegrationTests
    {
        private static string ResolveDataPath(string filename)
        {
            string[] candidates = new[]
            {
                Path.Combine(AppContext.BaseDirectory, "../../../../Assets/StreamingAssets/Data", filename),
                Path.Combine(AppContext.BaseDirectory, "../../../Assets/StreamingAssets/Data", filename),
                Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data", filename),
                Path.Combine(Directory.GetCurrentDirectory(), "../Assets/StreamingAssets/Data", filename)
            };
            foreach (var c in candidates)
            {
                if (File.Exists(c)) return Path.GetFullPath(c);
            }
            return Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data", filename);
        }

        [Fact]
        public void CatalogIntegrity_ColonyBlueprintsJson_LoadsTypesAndBuildings()
        {
            string path = ResolveDataPath("colony_blueprints.json");
            Assert.True(File.Exists(path), $"Catalog missing at: {path}");

            string json = File.ReadAllText(path);
            var system = new ColonySystem();
            system.LoadCatalog(json);

            Assert.True(system.AuthoredTypes.Count >= 5, "Expected at least 5 colony types.");
            Assert.True(system.AuthoredBuildings.Count >= 6, "Expected at least 6 colony buildings.");

            var fortress = system.AuthoredTypes.FirstOrDefault(t => t.TypeId == "fortress");
            Assert.NotNull(fortress);
            Assert.Equal(80f, fortress.BaseDefense);

            var watchtower = system.AuthoredBuildings.FirstOrDefault(b => b.BuildingId == "col_bldg_watchtower");
            Assert.NotNull(watchtower);
            Assert.Equal(25f, watchtower.DefenseBonus);
        }

        [Fact]
        public void EstablishColony_AppliesBlueprintBaselineDefenseAndCapacity()
        {
            string path = ResolveDataPath("colony_blueprints.json");
            var system = new ColonySystem();
            if (File.Exists(path)) system.LoadCatalog(File.ReadAllText(path));

            ColonyOutpost? establishedFromSeam = null;
            system.OnColonyEstablishedSeam = c => establishedFromSeam = c;

            var fort = system.EstablishColony("loc_iron_crag", "Iron Hold", ColonyType.Fortress, new[] { "garrison_1", "garrison_2" }, initialSupplies: 75f, currentDay: 4);
            var farm = system.EstablishColony("loc_green_valley", "Verdant Fields", ColonyType.FarmingCommune, new[] { "farmer_1" }, initialSupplies: 40f, currentDay: 4);

            Assert.NotNull(establishedFromSeam);
            Assert.Equal("Iron Hold", fort.Name);
            Assert.Equal(80f, fort.DefenseRating);
            Assert.Equal(30f, farm.DefenseRating);
            Assert.Equal(2, system.TotalColonyCount);
        }

        [Fact]
        public void ConstructBuilding_ExpandsCapacity_IncreasesDefenseAndMorale()
        {
            string path = ResolveDataPath("colony_blueprints.json");
            var system = new ColonySystem();
            if (File.Exists(path)) system.LoadCatalog(File.ReadAllText(path));

            var colony = system.EstablishColony("loc_watch_ridge", "Ridge Outpost", ColonyType.Outpost, new[] { "scout_alpha" }, initialSupplies: 50f);
            float initialDefense = colony.TotalDefense;
            int initialCapacity = colony.TotalCapacity;

            ColonyBuilding? builtFromSeam = null;
            system.OnBuildingConstructedSeam = (c, b) => builtFromSeam = b;

            var watchtower = system.ConstructBuilding(colony.ColonyId, "col_bldg_watchtower", currentDay: 2);
            var bunkhouse = system.ConstructBuilding(colony.ColonyId, "col_bldg_bunkhouse", currentDay: 3);

            Assert.NotNull(watchtower);
            Assert.NotNull(bunkhouse);
            Assert.NotNull(builtFromSeam);
            Assert.Equal(2, colony.Buildings.Count);
            Assert.True(colony.TotalDefense > initialDefense);
            Assert.True(colony.TotalCapacity > initialCapacity);
        }

        [Fact]
        public void SupplyLine_ContinuousFlowAndDisruptionMechanics()
        {
            var system = new ColonySystem();
            var colony = system.EstablishColony("loc_waystation", "Relay Post", ColonyType.TradingPost, new[] { "dweller_1" }, initialSupplies: 20f);
            var line = system.EstablishSupplyLine("shelter", colony.ColonyId, cargoCapacity: 80f, dailyFlow: 12f, currentDay: 1);

            float updatedSupplies = 0f;
            system.OnColonySuppliesUpdatedSeam = (c, s) => updatedSupplies = s;

            // Day 2: 20 initial + 12 flow = 32 delivered; net after 1.5 consumption = 30.5
            system.TickDay(currentDay: 2);
            Assert.Equal(30.5f, colony.StoredSupplies);
            Assert.Equal(32.0f, updatedSupplies);

            // Disrupt the line
            SupplyLineStatus updatedStatus = SupplyLineStatus.Active;
            system.OnSupplyLineStatusChangedSeam = (l, s) => updatedStatus = s;
            system.SetSupplyLineStatus(line.LineId, SupplyLineStatus.Disrupted);

            Assert.Equal(SupplyLineStatus.Disrupted, updatedStatus);

            // Day 3: flow stopped, consumption continues: 30.5 - 1.5 = 29.0
            system.TickDay(currentDay: 3);
            Assert.Equal(29.0f, colony.StoredSupplies);
        }

        [Fact]
        public void ColonyGarrison_DailyConsumptionAndStarvationDecay()
        {
            var system = new ColonySystem();
            var colony = system.EstablishColony("loc_isolated_shaft", "Deep Shaft", ColonyType.Outpost, new[] { "miner_1", "miner_2", "miner_3", "miner_4" }, initialSupplies: 12f);

            // 4 survivors * 1.5 = 6.0 supplies consumed daily
            system.TickDay(currentDay: 2);
            Assert.Equal(6.0f, colony.StoredSupplies);

            system.TickDay(currentDay: 3);
            Assert.Equal(0.0f, colony.StoredSupplies);

            float preStarvationMorale = colony.MoraleRating;
            // Day 4: starvation tick -> morale should drop by 10
            system.TickDay(currentDay: 4);
            Assert.Equal(preStarvationMorale - 10f, colony.MoraleRating);
        }

        [Fact]
        public void CaptureAndRestoreState_PreservesFullColonyEcosystem()
        {
            string path = ResolveDataPath("colony_blueprints.json");
            var system1 = new ColonySystem();
            if (File.Exists(path)) system1.LoadCatalog(File.ReadAllText(path));

            var colA = system1.EstablishColony("loc_alpha", "Alpha Outpost", ColonyType.Outpost, new[] { "s1", "s2" }, 60f, 1);
            system1.ConstructBuilding(colA.ColonyId, "col_bldg_watchtower", 2);
            var colB = system1.EstablishColony("loc_beta", "Beta Commune", ColonyType.FarmingCommune, new[] { "s3" }, 45f, 2);
            system1.EstablishSupplyLine(colA.ColonyId, colB.ColonyId, 120f, 18f, 3);

            var state = system1.CaptureState();
            Assert.Equal(1, state.SchemaVersion);
            Assert.Equal(2, state.Colonies.Count);
            Assert.Single(state.SupplyLines);
            Assert.Single(state.Colonies[0].Buildings);

            var system2 = new ColonySystem();
            system2.RestoreState(state);

            Assert.Equal(2, system2.TotalColonyCount);
            Assert.Equal(1, system2.ActiveSupplyLineCount);

            var restoredColA = system2.GetColony(colA.ColonyId);
            Assert.NotNull(restoredColA);
            Assert.Equal("Alpha Outpost", restoredColA.Name);
            Assert.Single(restoredColA.Buildings);
            Assert.Equal("col_bldg_watchtower", restoredColA.Buildings[0].DefinitionId);
            Assert.Equal(colA.TotalDefense, restoredColA.TotalDefense);
        }
    }
}
