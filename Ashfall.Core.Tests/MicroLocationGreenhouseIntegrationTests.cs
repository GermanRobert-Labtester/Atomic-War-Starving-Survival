using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    using Inventory = Ashfall.Core.Inventory.Inventory;
    using ItemCatalog = Ashfall.Core.Inventory.ItemCatalog;
    /// <summary>
    /// F18 flagship integration — the ruined greenhouse's rewards are real
    /// agriculture inputs that flow through the canonical planting economy.
    ///
    ///   take_greenhouse_seeds   → 2 × seed_packets  (mixed assorted-vegetable packet)
    ///   open_greenhouse_cabinet → 1 × crop_medicinal_herb (harvest-yield medical item)
    ///
    /// seed_packets is mapped into the ONE canonical crop authority
    /// (GreenhouseExpansionCatalog.CropCatalog — the same lookup every
    /// item_seed_* uses) rather than a micro-location-specific planting
    /// exception. crop_medicinal_herb is the authored clean yield of the
    /// medicinal-herb crop and a canonical recipe ingredient.
    /// </summary>
    public class MicroLocationGreenhouseIntegrationTests
    {
        private const string GreenhouseId = "micro_ruined_greenhouse";
        private const string TakeSeedsChoiceId = "take_greenhouse_seeds";
        private const string OpenCabinetChoiceId = "open_greenhouse_cabinet";
        private const string LeaveChoiceId = "leave_greenhouse";
        private const string SeedPacketsId = "seed_packets";
        private const string MedicinalHerbId = "crop_medicinal_herb";
        private const string TuberCropId = "crop_tuber";

        private static string DataDir()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null && !File.Exists(Path.Combine(dir.FullName, "Ashfall.csproj")))
                dir = dir.Parent!;
            return Path.Combine(dir!.FullName, "Assets", "StreamingAssets", "Data");
        }

        private static NarrativeEncounterSystem CreateProductionNarrativeSystem()
        {
            var sys = new NarrativeEncounterSystem();
            var defs = NarrativeEncounterCatalogLoader.Load(
                DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            sys.RegisterRange(defs);
            return sys;
        }

        private static ItemCatalog LoadItemCatalog()
        {
            return Ashfall.Core.Inventory.ItemCatalogLoader.LoadCatalog(
                DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
        }

        private static GreenhouseSystem CreateGreenhouseFixture(Inventory inventory, int plotCount = 2)
        {
            var sys = new GreenhouseSystem(seed: 1986);
            sys.EnsurePlots(plotCount);
            return sys;
        }

        /// <summary>The canonical host planting transaction
        /// (GreenhouseHostSession.Plant): inventory gate → System.Plant →
        /// consume one seed. Mirrored in Core so the contract is testable.</summary>
        private static bool PlantThroughCanonicalFlow(GreenhouseSystem greenhouse, Inventory inventory, int plot, int day)
        {
            if (inventory.CountById(SeedPacketsId) < 1) return false;
            if (!greenhouse.Plant(plot, SeedPacketsId, day, out var consumedSeedId)) return false;
            Assert.Equal(SeedPacketsId, consumedSeedId);
            inventory.RemoveById(SeedPacketsId, 1);
            return true;
        }

        // ── Authored rewards ───────────────────────────────────────────

        [Fact]
        public void F18_01_TakeGreenhouseSeeds_GrantsExactlyTwoSeedPackets()
        {
            var sys = CreateProductionNarrativeSystem();
            var res = sys.TryResolve(GreenhouseId, TakeSeedsChoiceId, "loc_allotments", 3);
            Assert.NotNull(res);
            Assert.Equal(SeedPacketsId, res!.GrantItemId);
            Assert.Equal(2, res.GrantItemQuantity);
            Assert.Equal(1, res.MoraleDelta);
            Assert.True(res.DepletesEncounter);
        }

        [Fact]
        public void F18_02_OpenGreenhouseCabinet_GrantsExactlyOneMedicinalHerb()
        {
            var sys = CreateProductionNarrativeSystem();
            var res = sys.TryResolve(GreenhouseId, OpenCabinetChoiceId, "loc_allotments", 3);
            Assert.NotNull(res);
            Assert.Equal(MedicinalHerbId, res!.GrantItemId);
            Assert.Equal(1, res.GrantItemQuantity);
            Assert.True(res.DepletesEncounter);
        }

        // ── Item validity + downstream consumption (§11.4) ─────────────

        [Fact]
        public void F18_03_BothRewardItems_ResolveAgainstItemCatalog()
        {
            var catalog = LoadItemCatalog();
            Assert.NotNull(catalog.Get(SeedPacketsId));
            Assert.NotNull(catalog.Get(MedicinalHerbId));
        }

        [Fact]
        public void F18_04_SeedPackets_HasRealAgricultureConsumer()
        {
            // The packet must resolve in the same crop catalog every seed uses.
            var def = GreenhouseExpansionCatalog.CropCatalog.Get(SeedPacketsId);
            Assert.NotNull(def);
            Assert.Equal(TuberCropId, def!.YieldCleanId);
            Assert.False(def.RequiresUnlock, "mixed assorted packet carries no unlock claim");
        }

        [Fact]
        public void F18_05_MedicinalHerb_IsCanonicalCropYield_AndRecipeIngredient()
        {
            // Yield side: the herb is the authored clean yield of the
            // medicinal-herb seed line.
            var herbSeed = GreenhouseExpansionCatalog.CropCatalog.Get(
                GreenhouseExpansionCatalog.Items.SeedMedicinalHerb);
            Assert.NotNull(herbSeed);
            Assert.Equal(MedicinalHerbId, herbSeed!.YieldCleanId);
        }

        [Fact]
        public void F18_06_MedicinalHerb_ReferencedByCanonicalRecipe()
        {
            // recipes.json — craft_dried_herb_packets consumes the herb.
            var json = new SystemTextJsonSerializer();
            var fileIO = new FileSystemIO();
            string path = fileIO.Combine(DataDir(), "recipes.json");
            Assert.True(fileIO.FileExists(path));
            string raw = fileIO.ReadAllText(path);
            Assert.Contains(MedicinalHerbId, raw, StringComparison.Ordinal);
            Assert.Contains("craft_dried_herb_packets", raw, StringComparison.Ordinal);
        }

        // ── Planting integration (§8.9) ────────────────────────────────

        [Fact]
        public void F18_07_GrantedSeeds_PlantThroughNormalFlow_CropGrows()
        {
            var inventory = new Inventory();
            inventory.AddById(SeedPacketsId, 2); // the authored grant
            var greenhouse = CreateGreenhouseFixture(inventory);

            Assert.True(PlantThroughCanonicalFlow(greenhouse, inventory, plot: 0, day: 3));
            Assert.Equal(1, inventory.CountById(SeedPacketsId)); // seed consumed by planting

            // Crop enters the canonical planted/growing state.
            var plots = greenhouse.CaptureState().plots;
            Assert.Equal(SeedPacketsId, plots[0].seedItemId);
        }

        [Fact]
        public void F18_08_GrantedSeeds_AreConsumed_PlantingIsNotFree()
        {
            // §14.3 — generic seeds cannot be planted infinitely: planting
            // consumes the packet exactly like any item_seed_*.
            var inventory = new Inventory();
            inventory.AddById(SeedPacketsId, 2);
            var greenhouse = CreateGreenhouseFixture(inventory, plotCount: 3);

            Assert.True(PlantThroughCanonicalFlow(greenhouse, inventory, 0, 1));
            Assert.True(PlantThroughCanonicalFlow(greenhouse, inventory, 1, 1));
            Assert.Equal(0, inventory.CountById(SeedPacketsId));
            Assert.False(PlantThroughCanonicalFlow(greenhouse, inventory, 2, 1), "no packets left — must refuse");
        }

        [Fact]
        public void F18_09_PlantedPacket_MaturesThroughNormalGrowthTicks()
        {
            var inventory = new Inventory();
            inventory.AddById(SeedPacketsId, 1);
            var greenhouse = CreateGreenhouseFixture(inventory, plotCount: 1);
            Assert.True(PlantThroughCanonicalFlow(greenhouse, inventory, 0, day: 1));

            // Normal tick economy: water + light advance growth; nothing about
            // the packet's origin is special-cased downstream (house fixture
            // convention: generous watering so the plot never dries out).
            greenhouse.Water(0, 60f, tainted: false);
            for (int day = 2; day <= 8; day++)
                greenhouse.TickDay(day, growLightHours: 6f, ashContaminationRate: 0f);

            var plot = greenhouse.CaptureState().plots[0];
            Assert.True(plot.growth > 0f, "growth advanced through canonical ticks");
        }

        // ── Progression safety (§8.8) ──────────────────────────────────

        [Fact]
        public void F18_10_SeedsDoNotBypassGreenhouseGates_UnavailablePlotRefuses()
        {
            var inventory = new Inventory();
            inventory.AddById(SeedPacketsId, 2); // reward owned
            var greenhouse = CreateGreenhouseFixture(inventory, plotCount: 1);

            // Occupied plot: seeds do not make an unavailable action valid.
            Assert.True(PlantThroughCanonicalFlow(greenhouse, inventory, 0, 1));
            Assert.False(greenhouse.Plant(0, SeedPacketsId, 2, out _), "occupied plot must refuse re-plant");
            Assert.False(greenhouse.Plant(99, SeedPacketsId, 2, out _), "unknown plot must refuse");

            // Seed item unknown to the crop authority never plants (the packet
            // gained no private exemption).
            Assert.False(greenhouse.Plant(1, "not_a_real_seed", 2, out _));
        }

        // ── One-shot + persistence ─────────────────────────────────────

        [Fact]
        public void F18_11_RewardsAreOneShot_SaveReloadCannotReGrant()
        {
            var sys = CreateProductionNarrativeSystem();
            Assert.NotNull(sys.TryResolve(GreenhouseId, TakeSeedsChoiceId, "loc_allotments", 3));
            Assert.True(sys.IsDepleted(GreenhouseId)); // whole site exhausted by the depleting choice

            var json = new SystemTextJsonSerializer();
            var restored = CreateProductionNarrativeSystem();
            restored.RestoreState(json.Deserialize<NarrativeEncounterState>(json.Serialize(sys.CaptureState()))!);
            Assert.True(restored.IsDepleted(GreenhouseId));

            for (int seed = 0; seed < 64; seed++)
            {
                var picked = restored.SelectEncounter("Cautious", 0f, "loc_allotments", new SeededRng(seed));
                Assert.NotEqual(GreenhouseId, picked?.id);
            }
        }

        [Fact]
        public void F18_12_LeaveChoice_NeutralNonDepleting()
        {
            var sys = CreateProductionNarrativeSystem();
            var res = sys.TryResolve(GreenhouseId, LeaveChoiceId, "loc_allotments", 3);
            Assert.NotNull(res);
            Assert.True(string.IsNullOrEmpty(res!.GrantItemId));
            Assert.False(res.DepletesEncounter);
        }

        // ── Determinism ────────────────────────────────────────────────

        [Fact]
        public void F18_13_Deterministic_SameSeedSameChoice_IdenticalGrantTrace()
        {
            var json = new SystemTextJsonSerializer();
            string TracePass()
            {
                var sys = CreateProductionNarrativeSystem();
                var a = sys.TryResolve(GreenhouseId, TakeSeedsChoiceId, "loc_allotments", 3);
                var b = sys.TryResolve(GreenhouseId, OpenCabinetChoiceId, "loc_allotments", 4);
                return $"{a!.GrantItemId}:{a.GrantItemQuantity}|{b!.GrantItemId}:{b.GrantItemQuantity}"
                     + "|" + json.Serialize(sys.CaptureState());
            }
            Assert.Equal(TracePass(), TracePass());
            Assert.Equal(TracePass(), TracePass());
        }
    }
}
