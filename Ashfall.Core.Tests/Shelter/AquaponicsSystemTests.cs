// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.IO;
using Ashfall.Core.Shelter;
using Xunit;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;

namespace Ashfall.Core.Tests.Shelter
{
    public class AquaponicsSystemTests
    {
        private static string FindDataDir()
        {
            string? dir = new DirectoryInfo(AppContext.BaseDirectory).FullName;
            for (int i = 0; i < 10 && dir != null; i++)
            {
                string probe = Path.Combine(dir, "Assets", "StreamingAssets", "Data", "aquaponics_system_catalog.json");
                if (File.Exists(probe))
                    return Path.Combine(dir, "Assets", "StreamingAssets", "Data");
                dir = Directory.GetParent(dir)?.FullName;
            }

            string cwd = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data");
            if (File.Exists(Path.Combine(cwd, "aquaponics_system_catalog.json")))
                return cwd;

            throw new DirectoryNotFoundException(
                "Assets/StreamingAssets/Data/aquaponics_system_catalog.json not found from " + AppContext.BaseDirectory);
        }

        private static AquaponicsCatalog LoadCatalog()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var catalog = AquaponicsCatalogLoader.Load(FindDataDir(), files, json);
            AquaponicsCatalogLoader.Validate(catalog);
            return catalog;
        }

        private static InventoryContainer MakeInventory(params (string id, int qty)[] items)
        {
            var inv = new InventoryContainer { Capacity = 128, MaxWeight = 1000f };
            foreach (var (id, qty) in items)
                Assert.True(inv.TryProduce(id, qty), $"failed stocking {id}");
            return inv;
        }

        private static AquaponicsSystem Create(
            int seed = 87,
            InventoryContainer? inventory = null,
            Func<string, float>? power = null)
        {
            var sys = new AquaponicsSystem(new SeededRng(seed), inventory, power);
            sys.LoadCatalog(LoadCatalog());
            return sys;
        }

        private static void BootstrapHealthyLoop(AquaponicsSystem sys, string tankId = "tank_a")
        {
            Assert.True(sys.CommissionTank(tankId, "tank_raft_small", "room_greenhouse").IsSuccess);
            Assert.True(sys.StockFish(tankId, "species_rad_tilapia", 4f, day: 1).IsSuccess);
            Assert.True(sys.Feed(tankId, "feed_insect_meal", 3f, day: 1).IsSuccess);
        }

        [Fact]
        public void Catalog_Loads_WithSchemaAndNoDuplicateIds()
        {
            var catalog = LoadCatalog();
            Assert.True(catalog.schema_version >= 1);
            Assert.NotEmpty(catalog.tank_classes);
            Assert.NotEmpty(catalog.species);
            Assert.NotEmpty(catalog.biofilters);
            Assert.NotEmpty(catalog.plant_profiles);
        }

        [Fact]
        public void Catalog_MissingFile_ReturnsEmptyIdleCatalog()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            string tmp = Path.Combine(Path.GetTempPath(), "ashfall_aqua_missing_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tmp);
            try
            {
                var catalog = AquaponicsCatalogLoader.Load(tmp, files, json);
                Assert.Empty(catalog.tank_classes);
                var sys = new AquaponicsSystem(new SeededRng(1));
                sys.LoadCatalog(catalog);
                sys.TickDay(1); // idle, no crash
            }
            finally
            {
                Directory.Delete(tmp, recursive: true);
            }
        }

        [Fact]
        public void FeedAndPower_GrowBiomass_Deterministically()
        {
            var invA = MakeInventory(("item_insect_larvae_meal", 40));
            var invB = MakeInventory(("item_insect_larvae_meal", 40));
            var a = Create(87, invA, _ => 1f);
            var b = Create(87, invB, _ => 1f);
            BootstrapHealthyLoop(a);
            BootstrapHealthyLoop(b);
            float startA = a.Snapshot("tank_a").BiomassKg;

            for (int day = 2; day <= 12; day++)
            {
                // Continuous rationing — a single pulse starves before growth compounds.
                if (day % 2 == 0)
                {
                    a.Feed("tank_a", "feed_insect_meal", 1.5f, day);
                    b.Feed("tank_a", "feed_insect_meal", 1.5f, day);
                }
                a.TickDay(day, temperatureModifier: 1f);
                b.TickDay(day, temperatureModifier: 1f);
            }

            float biomassA = a.Snapshot("tank_a").BiomassKg;
            float biomassB = b.Snapshot("tank_a").BiomassKg;
            Assert.True(biomassA > startA, $"expected growth, start={startA} got={biomassA}");
            Assert.Equal(biomassA, biomassB, 4);
        }

        [Fact]
        public void PowerLoss_CrashesDissolvedOxygen()
        {
            var inv = MakeInventory(("item_insect_larvae_meal", 10));
            float power = 1f;
            var sys = Create(11, inv, _ => power);
            BootstrapHealthyLoop(sys);
            sys.TickDay(2, temperatureModifier: 1f);
            float doBefore = sys.Snapshot("tank_a").DissolvedOxygen;

            power = 0f;
            for (int day = 3; day <= 6; day++)
                sys.TickDay(day, temperatureModifier: 1f);

            var snap = sys.Snapshot("tank_a");
            Assert.True(snap.PowerStarved);
            Assert.True(snap.DissolvedOxygen < doBefore, $"DO did not crash: before={doBefore} after={snap.DissolvedOxygen}");
            Assert.True(snap.DissolvedOxygen < 3.5f);
        }

        [Fact]
        public void HarvestFish_GrantsCanonicalFoodItem()
        {
            var inv = MakeInventory(("item_insect_larvae_meal", 30));
            var sys = Create(21, inv, _ => 1f);
            BootstrapHealthyLoop(sys);
            // Force harvestable biomass.
            var tank = sys.State.tanks[0];
            tank.biomassKg = 6f;

            int before = inv.CountById("item_aquaponic_fish");
            var harvest = sys.HarvestFish("tank_a", biomassKg: 2f, day: 5);
            Assert.True(harvest.Success, harvest.FailureCode);
            Assert.Equal("item_aquaponic_fish", harvest.ItemId);
            Assert.True(harvest.Amount >= 1);
            Assert.Equal(before + harvest.Amount, inv.CountById("item_aquaponic_fish"));
            Assert.True(sys.Snapshot("tank_a").BiomassKg < 6f);
        }

        [Fact]
        public void HarvestPlants_RequiresNitratePool()
        {
            var inv = MakeInventory(("item_insect_larvae_meal", 5));
            var sys = Create(31, inv, _ => 1f);
            BootstrapHealthyLoop(sys);
            var early = sys.HarvestPlants("tank_a", day: 2);
            Assert.False(early.Success);

            sys.State.tanks[0].nitratePool = 4f;
            sys.State.tanks[0].plantHealth = 1f;
            var harvest = sys.HarvestPlants("tank_a", day: 3);
            Assert.True(harvest.Success, harvest.FailureCode);
            Assert.Equal("crop_leafy_green", harvest.ItemId);
            Assert.True(inv.CountById("crop_leafy_green") >= 2);
        }

        [Fact]
        public void SaveRoundTrip_PreservesMidCycleEcology()
        {
            var inv = MakeInventory(("item_insect_larvae_meal", 20), ("item_biofilter_media", 2));
            var sys = Create(41, inv, _ => 1f);
            BootstrapHealthyLoop(sys);
            for (int day = 2; day <= 8; day++)
                sys.TickDay(day, temperatureModifier: 0.9f);

            var snap = sys.Snapshot("tank_a");
            var restored = Create(99, MakeInventory(("item_insect_larvae_meal", 1)), _ => 1f);
            restored.RestoreState(sys.CaptureState());
            var after = restored.Snapshot("tank_a");

            Assert.Equal(snap.BiomassKg, after.BiomassKg, 4);
            Assert.Equal(snap.DissolvedOxygen, after.DissolvedOxygen, 4);
            Assert.Equal(snap.NLoad, after.NLoad, 4);
            Assert.Equal(snap.BiofilterHealth, after.BiofilterHealth, 4);
            Assert.Equal(snap.DiseaseBandOrdinal, after.DiseaseBandOrdinal);
        }

        [Fact]
        public void Tick_IsIdempotent_ForSameDay()
        {
            var inv = MakeInventory(("item_insect_larvae_meal", 10));
            var sys = Create(51, inv, _ => 1f);
            BootstrapHealthyLoop(sys);
            sys.TickDay(2);
            float biomass = sys.Snapshot("tank_a").BiomassKg;
            sys.TickDay(2);
            Assert.Equal(biomass, sys.Snapshot("tank_a").BiomassKg, 5);
        }

        [Fact]
        public void NutrientExport_TracksPlantUptake()
        {
            var inv = MakeInventory(("item_insect_larvae_meal", 20));
            var sys = Create(61, inv, _ => 1f);
            BootstrapHealthyLoop(sys);
            sys.State.tanks[0].nLoad = 5f;
            sys.State.tanks[0].biofilterHealth = 100f;
            sys.TickDay(2, temperatureModifier: 1f);
            var export = sys.GetNutrientExport();
            Assert.True(export.NitrateAvailable >= 0f);
            Assert.Equal(AquaponicsSystem.SystemId, export.SourceSystemId);
        }

        [Fact]
        public void Soak_SixtyDays_RemainsViableWithFeed()
        {
            var inv = MakeInventory(("item_insect_larvae_meal", 200), ("item_biofilter_media", 20));
            var sys = Create(71, inv, _ => 1f);
            Assert.True(sys.CommissionTank("tank_a", "tank_raft_standard", "room_greenhouse").IsSuccess);
            Assert.True(sys.StockFish("tank_a", "species_rad_tilapia", 8f, day: 1).IsSuccess);

            for (int day = 1; day <= 60; day++)
            {
                if (day % 3 == 0)
                    sys.Feed("tank_a", "feed_insect_meal", 2f, day);
                if (day % 12 == 0)
                    sys.BackwashBiofilter("tank_a", day);
                if (sys.Snapshot("tank_a").DiseaseBandOrdinal >= 2)
                    sys.TreatDisease("tank_a", day);
                sys.TickDay(day, temperatureModifier: 1f);
            }

            var snap = sys.Snapshot("tank_a");
            Assert.True(snap.BiomassKg > 1f, $"loop collapsed: biomass={snap.BiomassKg}");
            Assert.True(snap.DissolvedOxygen > 0.5f, $"DO collapsed: {snap.DissolvedOxygen}");
        }

        [Fact]
        public void Soak_OneHundredTwentyDays_DoesNotCrash()
        {
            var inv = MakeInventory(("item_insect_larvae_meal", 400), ("item_biofilter_media", 40));
            var sys = Create(81, inv, _ => 1f);
            Assert.True(sys.CommissionTank("tank_a", "tank_raft_standard", "room_greenhouse").IsSuccess);
            Assert.True(sys.StockFish("tank_a", "species_ash_carp", 10f, day: 1).IsSuccess);

            for (int day = 1; day <= 120; day++)
            {
                if (day % 2 == 0)
                    sys.Feed("tank_a", "feed_insect_meal", 1.5f, day);
                if (day % 10 == 0)
                    sys.BackwashBiofilter("tank_a", day);
                sys.TickDay(day, temperatureModifier: 0.95f);
            }

            Assert.True(sys.Snapshot("tank_a").BiomassKg >= 0f);
            Assert.NotNull(sys.GetNutrientExport());
        }

        [Fact]
        public void Backwash_RequiresMediaItem()
        {
            var inv = MakeInventory(("item_insect_larvae_meal", 5));
            var sys = Create(91, inv, _ => 1f);
            BootstrapHealthyLoop(sys);
            sys.State.tanks[0].biofilterHealth = 40f;
            var blocked = sys.BackwashBiofilter("tank_a", day: 2);
            Assert.False(blocked.IsSuccess);
            Assert.Equal("insufficient_media", blocked.FailureCode);

            Assert.True(inv.TryProduce("item_biofilter_media", 1));
            var ok = sys.BackwashBiofilter("tank_a", day: 3);
            Assert.True(ok.IsSuccess, ok.MessageKey);
            Assert.True(sys.Snapshot("tank_a").BiofilterHealth > 40f);
        }
    }
}
