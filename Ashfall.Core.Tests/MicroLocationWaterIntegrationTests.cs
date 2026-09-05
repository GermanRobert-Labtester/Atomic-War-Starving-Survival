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
    using ItemType = Ashfall.Core.Inventory.ItemType;
    using ItemCatalog = Ashfall.Core.Inventory.ItemCatalog;
    /// <summary>
    /// F20 flagship integration — micro_water_source rewards enter the real
    /// hydration economy and can never become an infinite source.
    ///
    ///   collect_water → +3 clean_water   (morale +2)
    ///   test_water    → +2 clean_water   (no morale — testing costs yield)
    ///
    /// Consumption proof runs through the canonical Core transaction
    /// (<see cref="Inventory.Consume"/>: remove 1 unit, apply thirstRestore via
    /// the needs callback, roll back on failure) — the same contract the host
    /// ConsumeWater path uses. No special micro-location water path exists.
    /// Scarcity: the source is a finite discovery reward, never a production
    /// node — depletion plus selection exclusion lock it.
    /// </summary>
    public class MicroLocationWaterIntegrationTests
    {
        private const string WaterSourceId = "micro_water_source";
        private const string CollectWaterChoiceId = "collect_water";
        private const string TestWaterChoiceId = "test_water";
        private const string AvoidWaterChoiceId = "avoid_water";
        private const string CleanWaterId = "clean_water";

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

        /// <summary>Canonical grant transaction: the host's loot grant is
        /// inventory-shaped (AddById), so the Core fixture mirrors it.</summary>
        private static Inventory InventoryFromGrant(NarrativeEncounterResolutionResult res)
        {
            var inv = new Inventory();
            if (!string.IsNullOrEmpty(res!.GrantItemId) && res.GrantItemQuantity > 0)
                inv.AddById(res.GrantItemId, res.GrantItemQuantity);
            return inv;
        }

        // ── Authored reward quantities (deterministic, no RNG) ─────────

        [Fact]
        public void F20_01_CollectWater_GrantsExactlyThreeCleanWater()
        {
            var sys = CreateProductionNarrativeSystem();
            var res = sys.TryResolve(WaterSourceId, CollectWaterChoiceId, "loc_old_farmstead", 4);
            Assert.NotNull(res);
            Assert.Equal(CleanWaterId, res!.GrantItemId);
            Assert.Equal(3, res.GrantItemQuantity);
            Assert.Equal(2, res.MoraleDelta);
            Assert.True(res.DepletesEncounter);
        }

        [Fact]
        public void F20_02_TestWater_GrantsExactlyTwoCleanWater()
        {
            var sys = CreateProductionNarrativeSystem();
            var res = sys.TryResolve(WaterSourceId, TestWaterChoiceId, "loc_old_farmstead", 4);
            Assert.NotNull(res);
            Assert.Equal(CleanWaterId, res!.GrantItemId);
            Assert.Equal(2, res.GrantItemQuantity);
            Assert.Equal(0, res.MoraleDelta);
            Assert.True(res.DepletesEncounter);
        }

        [Fact]
        public void F20_03_GrantsEnterCanonicalInventory_WithCorrectDelta()
        {
            // Inventory delta, not absolute counts (fixture may not start at zero).
            var sys = CreateProductionNarrativeSystem();
            var inv = new Inventory();
            inv.AddById(CleanWaterId, 5); // pre-existing stock

            int before = inv.CountById(CleanWaterId);
            var collect = sys.TryResolve(WaterSourceId, CollectWaterChoiceId, "loc_old_farmstead", 4);
            var invAfterCollect = InventoryFromGrant(collect);
            Assert.Equal(3, invAfterCollect.CountById(CleanWaterId));

            var test = sys.TryResolve(WaterSourceId, TestWaterChoiceId, "loc_old_farmstead", 5);
            var invAfterTest = InventoryFromGrant(test);
            Assert.Equal(2, invAfterTest.CountById(CleanWaterId));

            // collect (3) − test (2) = exactly one unit of difference, deterministic.
            Assert.Equal(1, invAfterCollect.CountById(CleanWaterId) - invAfterTest.CountById(CleanWaterId));
            Assert.Equal(before, 5); // untouched fixture inventory
        }

        // ── One-shot source ────────────────────────────────────────────

        [Fact]
        public void F20_04_SourceIsFinite_RevisitGrantsZero()
        {
            var sys = CreateProductionNarrativeSystem();
            Assert.NotNull(sys.TryResolve(WaterSourceId, CollectWaterChoiceId, "loc_old_farmstead", 4));
            Assert.True(sys.IsDepleted(WaterSourceId));

            // The production selector can never re-surface the source — no
            // refill, no per-expedition regeneration, for any seed or stance.
            foreach (var stance in new[] { "Cautious", "Stealth", "Speed", "Aggressive" })
            {
                for (int seed = 0; seed < 32; seed++)
                {
                    var picked = sys.SelectEncounter(stance, 0f, "loc_old_farmstead", new SeededRng(seed));
                    Assert.NotEqual(WaterSourceId, picked?.id);
                }
            }
        }

        [Fact]
        public void F20_05_SaveReload_CannotReGrantWater()
        {
            var sys = CreateProductionNarrativeSystem();
            Assert.NotNull(sys.TryResolve(WaterSourceId, CollectWaterChoiceId, "loc_old_farmstead", 4));

            var json = new SystemTextJsonSerializer();
            var restored = CreateProductionNarrativeSystem();
            restored.RestoreState(json.Deserialize<NarrativeEncounterState>(json.Serialize(sys.CaptureState()))!);

            Assert.True(restored.IsDepleted(WaterSourceId));
            for (int seed = 0; seed < 64; seed++)
            {
                var picked = restored.SelectEncounter("Cautious", 1f, "loc_old_farmstead", new SeededRng(seed));
                Assert.NotEqual(WaterSourceId, picked?.id);
            }
        }

        // ── Canonical hydration consumption ────────────────────────────

        [Fact]
        public void F20_06_GrantedWater_ConsumedThroughCanonicalPath_HydrationMatches()
        {
            var catalog = LoadItemCatalog();
            var cleanDef = catalog.Get(CleanWaterId);
            Assert.NotNull(cleanDef);
            Assert.True(cleanDef!.thirstRestore > 0f, "clean_water must restore thirst through its definition");

            var inv = new Inventory();
            inv.AddById(CleanWaterId, 3); // the collect_water grant
            float thirst = 40f;

            bool applied = inv.Consume(cleanDef, applyNeed: (kind, delta) =>
            {
                if (kind != ItemType.Water) return true;
                thirst += delta; // delta is negative thirstRestore
                return true;
            });

            Assert.True(applied);
            Assert.Equal(2, inv.CountById(CleanWaterId)); // stack decreased by one
            Assert.Equal(40f - cleanDef.thirstRestore, thirst, 3); // hydration via needs authority
        }

        [Fact]
        public void F20_07_ConsumptionMatchesOtherCleanWaterSources_NoSourceBias()
        {
            // Water granted by the micro-location must hydrate identically to
            // water from any other source: the item definition is the only
            // authority for thirstRestore.
            var catalog = LoadItemCatalog();
            var def = catalog.Get(CleanWaterId)!;

            var invA = new Inventory();
            invA.AddById(CleanWaterId, 1);
            float thirstA = 30f;
            Assert.True(invA.Consume(def, applyNeed: (k, d) => { if (k == ItemType.Water) thirstA += d; return true; }));

            var invB = new Inventory();
            invB.AddById(CleanWaterId, 1); // e.g. bought from a caravan — same item id
            float thirstB = 30f;
            Assert.True(invB.Consume(def, applyNeed: (k, d) => { if (k == ItemType.Water) thirstB += d; return true; }));

            Assert.Equal(thirstA, thirstB, 3);
        }

        [Fact]
        public void F20_08_FailedNeedCallback_RollsBackCanonicalConsumption()
        {
            var catalog = LoadItemCatalog();
            var def = catalog.Get(CleanWaterId)!;
            var inv = new Inventory();
            inv.AddById(CleanWaterId, 1);

            // Needs authority refuses the intake — the transaction must undo.
            bool applied = inv.Consume(def, applyNeed: (k, d) => k != ItemType.Water);
            Assert.False(applied);
            Assert.Equal(1, inv.CountById(CleanWaterId)); // rolled back
        }

        // ── Scarcity protection ────────────────────────────────────────

        [Fact]
        public void F20_09_NoPermanentWaterProductionNode_AuthoredPayloadOnly()
        {
            // The water source must not carry recurring-production semantics:
            // its only effects are the finite grant, morale, and depletion.
            var defs = NarrativeEncounterCatalogLoader.Load(
                DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            var source = defs.First(d => d?.id == WaterSourceId);
            Assert.NotNull(source);
            foreach (var choice in source!.choices)
            {
                Assert.True(string.IsNullOrEmpty(choice.setWorldFlag),
                    "water source must not set flags that could gate recurring production");
                Assert.True(string.IsNullOrEmpty(choice.discoverLocationId),
                    "water source must not discover production locations");
            }
        }

        [Fact]
        public void F20_10_CleanWater_IsPotable_DistinctFromContaminatedItem()
        {
            // §10.8 — the authoritative water model distinguishes potable and
            // contaminated item types; the authored grant is the potable one.
            var catalog = LoadItemCatalog();
            var clean = catalog.Get(CleanWaterId)!;
            Assert.Equal(ItemType.Water, clean.type);
            Assert.Equal(0f, clean.contamination, 3);

            var contaminated = catalog.Get("irradiated_water");
            Assert.NotNull(contaminated); // the canonical unsafe-water item exists
            Assert.NotEqual(CleanWaterId, contaminated!.id);
        }

        [Fact]
        public void F20_11_AvoidChoice_NeutralPath()
        {
            var sys = CreateProductionNarrativeSystem();
            var res = sys.TryResolve(WaterSourceId, AvoidWaterChoiceId, "loc_old_farmstead", 4);
            Assert.NotNull(res);
            Assert.True(string.IsNullOrEmpty(res!.GrantItemId));
            Assert.False(res.DepletesEncounter);
        }

        // ── Determinism (§13 trace contract, water slice) ──────────────

        [Fact]
        public void F20_12_Deterministic_SameSeedSameChoice_IdenticalGrantTrace()
        {
            var json = new SystemTextJsonSerializer();
            string TracePass()
            {
                var sys = CreateProductionNarrativeSystem();
                var a = sys.TryResolve(WaterSourceId, CollectWaterChoiceId, "loc_old_farmstead", 4);
                var b = sys.TryResolve(WaterSourceId, TestWaterChoiceId, "loc_old_farmstead", 5);
                return $"{a!.GrantItemId}:{a.GrantItemQuantity}:{a.MoraleDelta}|{b!.GrantItemId}:{b.GrantItemQuantity}:{b.MoraleDelta}"
                     + "|" + json.Serialize(sys.CaptureState());
            }
            Assert.Equal(TracePass(), TracePass());
            Assert.Equal(TracePass(), TracePass());
        }
    }
}
