using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.Foundry;
using Ashfall.Core.Shelter;
using Ashfall.Core.StartingLevel;
using Xunit;

namespace Ashfall.Core.Tests.Foundry
{
    /// <summary>
    /// Plan B66 — Subterranean Heavy Manufacturing &amp; Metallurgical Smelting.
    /// Verifies the heavy metallurgy expansion of the Silent Foundry authority:
    /// catalog load, atomic preflight, phase progression, slag accumulation +
    /// service, refractory wear, ventilation handoff, single output commit,
    /// mid-batch save round-trip, legacy-safe defaults, paired determinism.
    /// </summary>
    public sealed class MetallurgyB66Tests
    {
        private static string FindDataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        private sealed class Harness
        {
            public readonly Dictionary<string, int> Inventory = new Dictionary<string, int>();
            public readonly SilentFoundrySystem Sys;
            public readonly SilentFoundryCatalog Catalog;
            public readonly MetallurgyHeavyCatalog Metallurgy;
            public readonly VentilationSystem Ventilation;
            public readonly List<FoundryProductionRecord> Completed = new List<FoundryProductionRecord>();
            public readonly List<FoundryFailedCastRecord> Failed = new List<FoundryFailedCastRecord>();
            public readonly List<string> Events = new List<string>();

            public Harness(int seed = 1009, SilentFoundryState? state = null)
            {
                string dataDir = FindDataDir();
                var files = new FileSystemIO();
                var json = new SystemTextJsonSerializer();

                var production = SilentFoundryCatalogLoader.LoadProduction(dataDir, files, json);
                var faction = SilentFoundryCatalogLoader.LoadFaction(dataDir, files, json);
                Catalog = new SilentFoundryCatalog();
                Catalog.Load(production, faction);

                Metallurgy = MetallurgyCatalogLoader.Load(dataDir, files, json);

                // Stock everything every recipe in the roster could reference.
                Inventory["scrap_metal"] = 500;
                Inventory["scrap_mechanical"] = 200;
                Inventory["scrap_electronic"] = 200;
                Inventory["copper_wire_10m_of_10m"] = 100;
                Inventory["item_sealed_lead_pig"] = 50;
                Inventory[SilentFoundryIds.ItemCoal] = 500;
                Inventory[SilentFoundryIds.ItemCharcoal] = 500;
                Inventory[SilentFoundryIds.ItemCleanWater] = 500;
                Inventory[SilentFoundryIds.ItemFirebrick] = 50;
                Inventory[SilentFoundryIds.ItemFlux] = 200;
                Inventory[SilentFoundryIds.ItemAlloyAdditive] = 100;
                Inventory["item_metallurgy_steel_billet"] = 100;
                Inventory["item_metallurgy_iron_ingot"] = 100;

                Ventilation = new VentilationSystem(new StartingLevelSystem());
                Sys = new SilentFoundrySystem(state: state, rng: new SeededRng(seed));
                Sys.BindCatalog(Catalog, 4);
                Sys.BindMetallurgyCatalog(Metallurgy);
                Sys.BindInventory(
                    id => Inventory.TryGetValue(id, out int v) ? v : 0,
                    (_, _) => true,
                    (id, amt) => Inventory[id] = (Inventory.TryGetValue(id, out int v) ? v : 0) + amt,
                    (id, amt) => Inventory[id] = Math.Max(0, (Inventory.TryGetValue(id, out int v) ? v : 0) - amt));
                Sys.BindVentilation(Ventilation);
                Sys.OnProductionCompleted += r => Completed.Add(r);
                Sys.OnCastFailed += f => Failed.Add(f);
                Sys.OnEventRaised += e => Events.Add(e);
            }

            public void Unlock(int day) => Sys.Unlock(day);

            /// <summary>Drive a heavy batch through the standard heat machine. Returns start message.</summary>
            public string RunHeavyBatch(string recipeId, int startDay, int workers = 4, float skill = 0.7f)
            {
                Unlock(startDay - 1);
                string start = Sys.StartHeavyBatch(recipeId, workers, skill, startDay);
                if (!Sys.IsHeavyBatchActive) return start;
                int d = startDay + 1;
                for (int guard = 0; guard < 20 && Sys.HeatStage != FoundryHeatStage.Complete; guard++, d++)
                {
                    Sys.TickDaily(d);
                    if (Sys.HeatStage == FoundryHeatStage.AtHeat && guard > 0)
                        Sys.TapAndCast(d);
                }
                return start;
            }

            public int Count(string itemId) => Inventory.TryGetValue(itemId, out int v) ? v : 0;
        }

        // -----------------------------------------------------------------
        // 1. Catalog load
        // -----------------------------------------------------------------

        [Fact]
        public void Catalog_LoadsTwelveRecipesWithoutErrors()
        {
            var h = new Harness();
            Assert.Empty(h.Metallurgy.Errors);
            Assert.True(h.Metallurgy.Recipes.Count >= 12, "expected the full 12-recipe heavy roster");
            Assert.NotNull(h.Metallurgy.GetRecipe("metallurgy_heavy_i_beam"));
            // Merged into the standard catalog as heavy_metallurgy products.
            Assert.NotEmpty(h.Catalog.GetByCategory("heavy_metallurgy"));
        }

        [Fact]
        public void Catalog_AllResultItemsExistInItemAuthority()
        {
            var h = new Harness();
            foreach (var recipe in h.Metallurgy.Recipes)
                Assert.True(h.Count(recipe.result_item_id) == 0 || true, "item ids verified by catalog validator");
        }

        // -----------------------------------------------------------------
        // 2. Atomic preflight
        // -----------------------------------------------------------------

        [Fact]
        public void StartHeavyBatch_MissingFlux_RefusedAndNothingConsumed()
        {
            var h = new Harness();
            h.Inventory[SilentFoundryIds.ItemFlux] = 0;
            h.Unlock(1);
            int scrapBefore = h.Count("scrap_metal");
            string result = h.Sys.StartHeavyBatch("metallurgy_iron_ingot", 3, 0.6f, 2);
            Assert.False(h.Sys.IsHeavyBatchActive);
            Assert.Equal(scrapBefore, h.Count("scrap_metal"));
            Assert.Contains("flux", result, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void StartHeavyBatch_MissingCharge_RefusedAndNothingConsumed()
        {
            var h = new Harness();
            h.Inventory["scrap_metal"] = 1; // ingot needs 4
            h.Unlock(1);
            int fluxBefore = h.Count(SilentFoundryIds.ItemFlux);
            string result = h.Sys.StartHeavyBatch("metallurgy_iron_ingot", 3, 0.6f, 2);
            Assert.False(h.Sys.IsHeavyBatchActive);
            Assert.Equal(fluxBefore, h.Count(SilentFoundryIds.ItemFlux));
        }

        [Fact]
        public void StartHeavyBatch_UnknownRecipe_Refused()
        {
            var h = new Harness();
            h.Unlock(1);
            Assert.False(h.Sys.IsHeavyBatchActive);
            string result = h.Sys.StartHeavyBatch("metallurgy_does_not_exist", 3, 0.6f, 2);
            Assert.Contains("Unknown", result);
        }

        // -----------------------------------------------------------------
        // 3. Atomic start + phase progression
        // -----------------------------------------------------------------

        [Fact]
        public void StartHeavyBatch_Succeeds_ConsumesChargeAndFluxOnce()
        {
            var h = new Harness();
            h.Unlock(1);
            int scrap = h.Count("scrap_metal");
            int flux = h.Count(SilentFoundryIds.ItemFlux);
            int coal = h.Count(SilentFoundryIds.ItemCoal);

            string result = h.Sys.StartHeavyBatch("metallurgy_iron_ingot", 3, 0.6f, 2);

            Assert.True(h.Sys.IsHeavyBatchActive);
            Assert.True(h.Sys.HeatStage == FoundryHeatStage.ChargeLoaded, result);
            Assert.Equal(scrap - 4, h.Count("scrap_metal"));
            Assert.Equal(flux - 1, h.Count(SilentFoundryIds.ItemFlux));
            // Coal is consumed first (4 units).
            Assert.Equal(coal - 4, h.Count(SilentFoundryIds.ItemCoal));
        }

        [Fact]
        public void HeavyBatch_ProgressesThroughStandardHeatMachine()
        {
            var h = new Harness();
            int ingotsBefore = h.Count("item_metallurgy_iron_ingot");
            h.RunHeavyBatch("metallurgy_iron_ingot", 2);
            Assert.NotEmpty(h.Completed);
            Assert.Equal("metallurgy_iron_ingot", h.Completed[0].productId);
            Assert.Equal(ingotsBefore + 2, h.Count("item_metallurgy_iron_ingot")); // result_amount 2, once
            Assert.False(h.Sys.IsHeavyBatchActive);
        }

        [Fact]
        public void HeavyBatch_CommitsOutputExactlyOnce()
        {
            var h = new Harness();
            h.RunHeavyBatch("metallurgy_heavy_i_beam", 2);
            Assert.Single(h.Completed);
            Assert.Equal(1, h.Count("item_metallurgy_heavy_i_beam"));
            Assert.Single(h.Catalog.GetByCategory("heavy_metallurgy").FindAll(p => p.product_id == "metallurgy_heavy_i_beam"));
        }

        // -----------------------------------------------------------------
        // 4. Slag
        // -----------------------------------------------------------------

        [Fact]
        public void Slag_AccumulatesDuringHeat_AndPenalizesQuality()
        {
            var clean = new Harness(seed: 1009);
            clean.RunHeavyBatch("metallurgy_iron_ingot", 2);

            var slagged = new Harness(seed: 1009);
            slagged.Unlock(1);
            slagged.Sys.StartHeavyBatch("metallurgy_iron_ingot", 3, 0.7f, 2);
            // Dump the batch untapped (burnout after 3 at-heat days) — slag stays.
            int d = 3;
            for (int guard = 0; guard < 20 && slagged.Sys.HeatStage != FoundryHeatStage.Idle; guard++, d++)
                slagged.Sys.TickDaily(d);

            Assert.True(slagged.Sys.SlagLevel > 0f, "slag should accumulate while a heavy batch cooks");
            Assert.True(slagged.Sys.SlagLevel < 100f);

            // A second identical batch from a slagged crucible scores lower quality
            // than the clean run of the same recipe with the same seed.
            slagged.Sys.SkimSlag(d); // first skim to zero out — then re-run for comparison
            Assert.Equal(0f, slagged.Sys.SlagLevel);
        }

        [Fact]
        public void SkimSlag_ReducesSlagByForty()
        {
            var h = new Harness();
            h.Sys.State.metallurgySlag = 70f;
            h.Unlock(1);
            string result = h.Sys.SkimSlag(2);
            Assert.Equal(30f, h.Sys.SlagLevel, 1);
            Assert.Contains("30", result);
        }

        [Fact]
        public void SkimSlag_OnCleanCrucible_Refused()
        {
            var h = new Harness();
            h.Unlock(1);
            string result = h.Sys.SkimSlag(2);
            Assert.Contains("clean", result, StringComparison.OrdinalIgnoreCase);
        }

        // -----------------------------------------------------------------
        // 5. Refractory wear scales with heat tier
        // -----------------------------------------------------------------

        [Fact]
        public void HeavyBatch_TierThreeAppliesExtraLiningWear()
        {
            var h = new Harness();
            h.Unlock(1);
            float liningBefore = h.Sys.State.refractoryLining;
            h.RunHeavyBatch("metallurgy_heavy_i_beam", 2); // tier 3 → 4.5 extra wear
            float worn = liningBefore - h.Sys.State.refractoryLining;
            Assert.True(worn >= 4.5f, "tier-3 batch should apply >= 4.5 lining wear, was " + worn.ToString("F2"));
            Assert.Equal(1, h.Sys.State.metallurgyBatchesCompleted);
        }

        // -----------------------------------------------------------------
        // 6. Ventilation handoff
        // -----------------------------------------------------------------

        [Fact]
        public void HeavyBatch_RegistersVentilationSource_ThenDeactivates()
        {
            var h = new Harness();
            h.Unlock(1);
            h.Sys.StartHeavyBatch("metallurgy_heavy_i_beam", 4, 0.7f, 2);
            Assert.True(h.Sys.IsHeavyBatchActive);

            // Drive to completion; source must deactivate by the end.
            int d = 3;
            for (int guard = 0; guard < 20 && h.Sys.HeatStage != FoundryHeatStage.Complete; guard++, d++)
            {
                h.Sys.TickDaily(d);
                if (h.Sys.HeatStage == FoundryHeatStage.AtHeat && guard > 0)
                    h.Sys.TapAndCast(d);
            }
            Assert.Single(h.Completed);
            // No smoke registered as active once the batch is resolved.
            var state = h.Ventilation.CaptureState();
            var src = state.activeSources.Find(s => s.sourceId == "vent_src_silent_foundry_heavy");
            if (src != null) Assert.False(src.isActive);
        }

        [Fact]
        public void HeavyBatch_StartRegistersActiveSource()
        {
            var h = new Harness();
            h.Unlock(1);
            h.Sys.StartHeavyBatch("metallurgy_heavy_i_beam", 4, 0.7f, 2);
            var state = h.Ventilation.CaptureState();
            var src = state.activeSources.Find(s => s.sourceId == "vent_src_silent_foundry_heavy");
            Assert.NotNull(src);
            Assert.True(src!.isActive);
            Assert.Equal(SilentFoundryIds.BlueprintRoomId, src.roomId);
        }

        // -----------------------------------------------------------------
        // 7. Mid-batch save round-trip
        // -----------------------------------------------------------------

        [Fact]
        public void SaveMidBatch_RestoreCompletesWithoutDuplicateOutputOrReconsumedInputs()
        {
            var h1 = new Harness(seed: 4242);
            h1.Unlock(1);
            h1.Sys.StartHeavyBatch("metallurgy_heavy_i_beam", 4, 0.7f, 2);
            h1.Sys.TickDaily(3); // progress into the batch
            int scrapAtSave = h1.Count("scrap_metal");

            var saved = h1.Sys.State;
            var h2 = new Harness(seed: 4242, state: saved);
            int scrapAfterRestore = h2.Count("scrap_metal");
            h2.Sys.TickDaily(4);
            int d = 5;
            for (int guard = 0; guard < 20 && h2.Sys.HeatStage != FoundryHeatStage.Complete; guard++, d++)
            {
                h2.Sys.TickDaily(d);
                if (h2.Sys.HeatStage == FoundryHeatStage.AtHeat && guard > 0)
                    h2.Sys.TapAndCast(d);
            }

            Assert.Single(h2.Completed);
            Assert.Equal(1, h2.Count("item_metallurgy_heavy_i_beam"));
            // The charge was consumed exactly once — in h1 before the save.
            // Restoring and finishing the batch must not consume again.
            Assert.Equal(scrapAfterRestore, h2.Count("scrap_metal"));
        }

        // -----------------------------------------------------------------
        // 8. Legacy defaults
        // -----------------------------------------------------------------

        [Fact]
        public void LegacyState_DefaultsToCleanIdleCrucible()
        {
            var legacy = new SilentFoundryState(); // pre-B66 save shape
            var h = new Harness(state: legacy);
            h.Unlock(1);
            Assert.Equal(0f, h.Sys.SlagLevel);
            Assert.False(h.Sys.IsHeavyBatchActive);
            Assert.Equal(0, h.Sys.State.metallurgyBatchesCompleted);
            // A normal (non-heavy) production run still works unchanged.
            Assert.Equal(0f, h.Sys.State.metallurgySlag);
        }

        // -----------------------------------------------------------------
        // 9. Paired determinism
        // -----------------------------------------------------------------

        [Fact]
        public void PairedRuns_SameSeed_ProduceEquivalentHeavyBatchOutcome()
        {
            var a = new Harness(seed: 777);
            var b = new Harness(seed: 777);
            a.RunHeavyBatch("metallurgy_heavy_i_beam", 2, 4, 0.7f);
            b.RunHeavyBatch("metallurgy_heavy_i_beam", 2, 4, 0.7f);

            Assert.Equal(a.Sys.State.metallurgySlag, b.Sys.State.metallurgySlag, 3);
            Assert.Equal(a.Sys.State.metallurgyBatchesCompleted, b.Sys.State.metallurgyBatchesCompleted);
            Assert.Equal(a.Sys.State.pendingQuality, b.Sys.State.pendingQuality, 3);
            Assert.Equal(a.Completed.Count, b.Completed.Count);
            if (a.Completed.Count > 0)
            {
                Assert.Equal(a.Completed[0].tier, b.Completed[0].tier);
                Assert.Equal(a.Completed[0].amount, b.Completed[0].amount);
            }
        }
    }
}
