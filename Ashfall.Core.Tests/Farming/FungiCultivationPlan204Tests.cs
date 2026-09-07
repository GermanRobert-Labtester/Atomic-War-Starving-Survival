// SPDX-License-Identifier: MIT
// Plan 204: substrate preparation, thermal band, flush cycles, contamination
// spread boundaries, disposal transactions, old-save baseline, determinism.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Farming;
using Ashfall.Core.Inventory;
using Xunit;

namespace Ashfall.Core.Tests.Farming
{
    public class FungiCultivationPlan204Tests
    {
        private UndergroundFloraCatalog CreateTestCatalog(float contaminationRisk = 0.15f)
        {
            return new UndergroundFloraCatalog
            {
                strains = new List<FungusStrainDef>
                {
                    new FungusStrainDef
                    {
                        strain_id = "strain_edible",
                        category = "Edible",
                        growth_days = 2,
                        moisture_min = 0.3f,
                        moisture_max = 0.95f,
                        temperature_min = 10f,
                        temperature_max = 20f,
                        darkness_required = true,
                        spore_hazard = 0.05f,
                        yield_item_id = "harvested_mushrooms_subterranean",
                        yield_count = 6,
                        flush_count = 3
                    },
                    new FungusStrainDef
                    {
                        strain_id = "strain_single_flush",
                        category = "Edible",
                        growth_days = 2,
                        moisture_min = 0.3f,
                        moisture_max = 0.95f,
                        temperature_min = 4f,
                        temperature_max = 32f,
                        darkness_required = true,
                        spore_hazard = 0.05f,
                        yield_item_id = "harvested_mushrooms_subterranean",
                        yield_count = 5,
                        flush_count = 1
                    }
                },
                substrates = new List<SubstrateDef>
                {
                    new SubstrateDef
                    {
                        substrate_id = "substrate_test",
                        nutrition_multiplier = 1.0f,
                        moisture_retention = 0.8f,
                        contamination_risk = contaminationRisk
                    }
                }
            };
        }

        private static (FungiCultivationSystem sys, Inventory.Inventory inv) MakeSystem(int seed, UndergroundFloraCatalog catalog)
        {
            var inv = new Inventory.Inventory();
            inv.AddById("clean_water", 50);
            inv.AddById("fuel", 50);
            inv.AddById("scrap_wood", 50);
            inv.AddById("fungus_spores_common", 50);
            var sys = new FungiCultivationSystem(new SeededRng(seed), inv);
            sys.RegisterCatalog(catalog);
            return (sys, inv);
        }

        /// <summary>
        /// Finds a campaign day where cold preparation does not fail. Wave F:
        /// prep rolls are day/plot-derived (fresh-seed house pattern), so the
        /// deterministic variation axis is the day, not the constructor seed.
        /// </summary>
        private static int FindDayWithSuccessfulPrep(UndergroundFloraCatalog catalog)
        {
            for (int day = 1; day <= 64; day++)
            {
                var (sys, _) = MakeSystem(1, catalog);
                sys.DayProvider = () => day;
                sys.EnsurePlot("plot_p", "room_a");
                var res = sys.PrepareSubstrate("plot_p", useHeat: false);
                var plot = sys.State.plots[0];
                if (res.IsSuccess &&
                    (plot.substratePreparation == SubstratePreparation.Prepared ||
                     plot.substratePreparation == SubstratePreparation.Clean))
                    return day;
            }
            throw new InvalidOperationException("No deterministic day produced a successful preparation in 1..64.");
        }

        // ── Substrate preparation ───────────────────────────────────────

        [Fact]
        public void PrepareSubstrate_ConsumesWater_SetsBoundedOutcome()
        {
            var (sys, inv) = MakeSystem(11, CreateTestCatalog());
            sys.EnsurePlot("plot_1", "room_a");

            var res = sys.PrepareSubstrate("plot_1", useHeat: false);

            Assert.True(res.IsSuccess);
            Assert.Equal(49, inv.CountById("clean_water"));
            var prep = sys.State.plots[0].substratePreparation;
            Assert.True(prep == SubstratePreparation.Prepared || prep == SubstratePreparation.Clean
                     || prep == SubstratePreparation.Compromised,
                $"Unexpected preparation state: {prep}");
        }

        [Fact]
        public void PrepareSubstrate_Heated_ConsumesFuel_AndBlockedWithoutIt()
        {
            var (sys, inv) = MakeSystem(11, CreateTestCatalog());
            sys.EnsurePlot("plot_1", "room_a");
            inv.RemoveById("fuel", inv.CountById("fuel"));

            var blocked = sys.PrepareSubstrate("plot_1", useHeat: true);
            Assert.False(blocked.IsSuccess);

            inv.AddById("fuel", 5);
            var res = sys.PrepareSubstrate("plot_1", useHeat: true);
            Assert.True(res.IsSuccess);
            Assert.Equal(4, inv.CountById("fuel"));
        }

        [Fact]
        public void PrepareSubstrate_BlockedOnPlantedPlot()
        {
            var (sys, _) = MakeSystem(11, CreateTestCatalog());
            sys.EnsurePlot("plot_1", "room_a");
            Assert.True(sys.CultivateSpores("plot_1", "strain_edible", "substrate_test", 1).IsSuccess);

            var res = sys.PrepareSubstrate("plot_1", useHeat: false);
            Assert.False(res.IsSuccess);
        }

        [Fact]
        public void PrepareSubstrate_PreparedSubstrate_ReducesStartingContamination()
        {
            var catalog = CreateTestCatalog();
            int prepDay = FindDayWithSuccessfulPrep(catalog);

            var (prepped, _) = MakeSystem(1, catalog);
            prepped.DayProvider = () => prepDay;
            prepped.EnsurePlot("plot_p", "room_a");
            prepped.PrepareSubstrate("plot_p", useHeat: false);
            Assert.True(prepped.CultivateSpores("plot_p", "strain_edible", "substrate_test", 1).IsSuccess);

            var (untreated, _) = MakeSystem(1, catalog);
            untreated.EnsurePlot("plot_u", "room_a");
            Assert.True(untreated.CultivateSpores("plot_u", "strain_edible", "substrate_test", 1).IsSuccess);

            float preppedC = prepped.State.plots[0].contamination;
            float untreatedC = untreated.State.plots[0].contamination;
            Assert.True(preppedC < untreatedC,
                $"Prepared contamination {preppedC} should be below untreated {untreatedC}");
            Assert.Equal(0.15f * SubstratePreparation.RiskMultiplier(SubstratePreparation.Untreated), untreatedC, 3);
        }

        [Fact]
        public void PreparationModifiers_AreBoundedAndDocumented()
        {
            Assert.Equal(1.0f, SubstratePreparation.RiskMultiplier(SubstratePreparation.Untreated), 3);
            Assert.Equal(0.5f, SubstratePreparation.RiskMultiplier(SubstratePreparation.Prepared), 3);
            Assert.Equal(0.25f, SubstratePreparation.RiskMultiplier(SubstratePreparation.Clean), 3);
            Assert.Equal(1.5f, SubstratePreparation.RiskMultiplier(SubstratePreparation.Compromised), 3);
            Assert.Equal(1.2f, SubstratePreparation.GrowthMultiplier(SubstratePreparation.Clean), 3);
            Assert.Equal(0.8f, SubstratePreparation.GrowthMultiplier(SubstratePreparation.Compromised), 3);
        }

        // ── Temperature band ────────────────────────────────────────────

        [Fact]
        public void TemperatureOutsideBand_SlowsGrowth()
        {
            var (warm, _) = MakeSystem(42, CreateTestCatalog());
            warm.EnsurePlot("plot_t", "room_a");
            Assert.True(warm.CultivateSpores("plot_t", "strain_edible", "substrate_test", 1).IsSuccess);

            var (cold, _) = MakeSystem(42, CreateTestCatalog());
            cold.EnsurePlot("plot_t", "room_a");
            Assert.True(cold.CultivateSpores("plot_t", "strain_edible", "substrate_test", 1).IsSuccess);

            warm.TickDay(1, roomIsDark: true, roomTemperatureC: 15f);
            cold.TickDay(1, roomIsDark: true, roomTemperatureC: 2f); // below temperature_min 10

            var warmPlot = warm.State.plots[0];
            var coldPlot = cold.State.plots[0];
            Assert.True(warmPlot.growthStage > coldPlot.growthStage,
                $"Warm growth {warmPlot.growthStage} should exceed cold growth {coldPlot.growthStage}");
            Assert.True(coldPlot.growthStage > 0f, "Cold stalls growth but does not stop it entirely.");
        }

        [Fact]
        public void HostRoomTemperatureOverride_ProjectedIntoPlots()
        {
            var (sys, _) = MakeSystem(42, CreateTestCatalog());
            sys.EnsurePlot("plot_freezing", "room_cold");
            Assert.True(sys.CultivateSpores("plot_freezing", "strain_edible", "substrate_test", 1).IsSuccess);

            // Host projects real thermal-room state: this room is frozen.
            sys.TickDay(1, roomIsDark: true, roomTemperatureOverride: roomId => roomId == "room_cold" ? -3f : 15f);

            var plot = sys.State.plots[0];
            Assert.True(plot.growthStage < 0.5f / 2f + 0.01f, "Frozen room should stall growth to the reduced rate.");
        }

        // ── Flush cycle ─────────────────────────────────────────────────

        private static void TickToReady(FungiCultivationSystem sys, FungiPlotState plot, int startDay, Inventory.Inventory inv, float roomTemp = 15f)
        {
            // Hold moisture in-band but below the 0.85 bloom-roll threshold so the
            // deterministic bloom RNG cannot intrude on the flush-cycle scenario.
            int day = startDay;
            while (!plot.isHarvestReady && day < startDay + 30)
            {
                plot.moisture = 0.7f;
                sys.TickDay(day++, roomIsDark: true, roomTemperatureC: roomTemp);
            }
            Assert.True(plot.isHarvestReady, $"Plot did not reach harvest readiness by day {day}.");
        }

        [Fact]
        public void FlushCycle_ThreeFlushes_ThenFallow_WithTaperingYield()
        {
            var (sys, inv) = MakeSystem(42, CreateTestCatalog());
            sys.EnsurePlot("bed", "room_a");
            Assert.True(sys.CultivateSpores("bed", "strain_edible", "substrate_test", 1).IsSuccess);
            var plot = sys.State.plots[0];
            Assert.Equal(3, plot.remainingFlushes);

            // Flush 1 — full yield.
            TickToReady(sys, plot, 1, inv);
            Assert.True(sys.HarvestPlot("bed").IsSuccess);
            Assert.Equal(6, inv.CountById("harvested_mushrooms_subterranean"));
            Assert.NotNull(plot.strainId);          // still colonized
            Assert.Equal(2, plot.remainingFlushes);
            Assert.Equal(0.5f, plot.growthStage, 2); // re-fruit stage

            // Flush 2 — 60% yield (3.6 → 4).
            TickToReady(sys, plot, 10, inv);
            Assert.True(sys.HarvestPlot("bed").IsSuccess);
            Assert.Equal(10, inv.CountById("harvested_mushrooms_subterranean")); // 6 + 4
            Assert.Equal(1, plot.remainingFlushes);

            // Flush 3 — 40% yield (2.4 → 2), then fallow.
            TickToReady(sys, plot, 20, inv);
            Assert.True(sys.HarvestPlot("bed").IsSuccess);
            Assert.Equal(12, inv.CountById("harvested_mushrooms_subterranean")); // 6 + 4 + 2
            Assert.Null(plot.strainId);
            Assert.Equal(0, plot.remainingFlushes);
            Assert.Equal(SubstratePreparation.Untreated, plot.substratePreparation);
        }

        [Fact]
        public void LegacySingleFlush_HarvestResetsToFallow()
        {
            var (sys, inv) = MakeSystem(42, CreateTestCatalog());
            sys.EnsurePlot("bed", "room_a");
            Assert.True(sys.CultivateSpores("bed", "strain_single_flush", "substrate_test", 1).IsSuccess);
            var plot = sys.State.plots[0];

            TickToReady(sys, plot, 1, inv);
            Assert.True(sys.HarvestPlot("bed").IsSuccess);
            Assert.Equal(5, inv.CountById("harvested_mushrooms_subterranean"));
            Assert.Null(plot.strainId); // historical behavior preserved
        }

        // ── Contamination: threshold, drift, spread boundary ───────────

        [Fact]
        public void ContaminationAtThreshold_BloomsDeterministically()
        {
            var (sys, _) = MakeSystem(42, CreateTestCatalog());
            sys.EnsurePlot("bed", "room_a");
            Assert.True(sys.CultivateSpores("bed", "strain_edible", "substrate_test", 1).IsSuccess);
            var plot = sys.State.plots[0];
            plot.contamination = 1.0f;

            bool bloomed = false;
            sys.OnToxicBloom += (_, __) => bloomed = true;
            sys.TickDay(1, roomIsDark: true);

            Assert.True(plot.hasToxicBloom);
            Assert.True(bloomed);
        }

        [Fact]
        public void BloomSpread_IsBoundedToSameRoom()
        {
            var (sys, _) = MakeSystem(42, CreateTestCatalog());
            var bloomed = sys.EnsurePlot("source", "room_a");
            Assert.True(sys.CultivateSpores("source", "strain_edible", "substrate_test", 1).IsSuccess);
            bloomed.hasToxicBloom = true;

            var neighborSameRoom = sys.EnsurePlot("neighbor", "room_a");
            Assert.True(sys.CultivateSpores("neighbor", "strain_edible", "substrate_test", 1).IsSuccess);
            var otherRoom = sys.EnsurePlot("distant", "room_b");
            Assert.True(sys.CultivateSpores("distant", "strain_edible", "substrate_test", 1).IsSuccess);

            float beforeNeighbor = neighborSameRoom.contamination;
            float beforeOther = otherRoom.contamination;

            sys.TickDay(1, roomIsDark: true);

            // Same-room neighbor absorbs spread pressure (net of the daily 0.02 decay);
            // cross-room plot must only have decayed — no spread may cross rooms.
            Assert.Equal(beforeNeighbor - 0.02f + 0.05f, neighborSameRoom.contamination, 3);
            Assert.Equal(beforeOther - 0.02f, otherRoom.contamination, 4);
        }

        [Fact]
        public void HealthyContamination_DecaysDaily()
        {
            var (sys, _) = MakeSystem(42, CreateTestCatalog());
            sys.EnsurePlot("bed", "room_a");
            Assert.True(sys.CultivateSpores("bed", "strain_edible", "substrate_test", 1).IsSuccess);
            var plot = sys.State.plots[0];
            plot.contamination = 0.5f;

            sys.TickDay(1, roomIsDark: true);

            Assert.Equal(0.48f, plot.contamination, 3);
        }

        // ── Disposal transactions ───────────────────────────────────────

        [Fact]
        public void DisposeBurn_ConsumesFuel_ClearsPlot()
        {
            var (sys, inv) = MakeSystem(42, CreateTestCatalog());
            var plot = sys.EnsurePlot("bed", "room_a");
            Assert.True(sys.CultivateSpores("bed", "strain_edible", "substrate_test", 1).IsSuccess);
            plot.hasToxicBloom = true;

            var res = sys.DisposeInfectedSubstrate("bed", "burn");

            Assert.True(res.IsSuccess);
            Assert.Equal(49, inv.CountById("fuel"));
            Assert.Null(plot.strainId);
            Assert.False(plot.hasToxicBloom);
            Assert.Equal(0f, plot.contamination, 3);
            Assert.Equal(SubstratePreparation.Untreated, plot.substratePreparation);
        }

        [Fact]
        public void DisposeQuarantine_ConsumesSealant_SealsAndContains()
        {
            var (sys, inv) = MakeSystem(42, CreateTestCatalog());
            var plot = sys.EnsurePlot("bed", "room_a");
            Assert.True(sys.CultivateSpores("bed", "strain_edible", "substrate_test", 1).IsSuccess);
            plot.hasToxicBloom = true;

            var res = sys.DisposeInfectedSubstrate("bed", "quarantine");

            Assert.True(res.IsSuccess);
            Assert.Equal(48, inv.CountById("scrap_wood"));
            Assert.True(plot.isQuarantined);
            Assert.Equal(0f, sys.GetSporeHazardInRoom("room_a"), 3); // sealed beds emit nothing

            // Quarantined plots cannot be planted or watered.
            Assert.False(sys.CultivateSpores("bed", "strain_edible", "substrate_test", 2).IsSuccess);
            Assert.False(sys.WaterPlot("bed").IsSuccess);

            // Quarantine blocks spread: a same-room neighbor receives nothing while sealed.
            var neighbor = sys.EnsurePlot("neighbor", "room_a");
            Assert.True(sys.CultivateSpores("neighbor", "strain_edible", "substrate_test", 2).IsSuccess);
            float before = neighbor.contamination;
            sys.TickDay(2, roomIsDark: true);
            Assert.Equal(before - 0.02f, neighbor.contamination, 4); // decay only — quarantine blocks spread
        }

        [Fact]
        public void DisposeDiscard_IsFree_ButRequiresContamination()
        {
            var (sys, inv) = MakeSystem(42, CreateTestCatalog());
            var healthy = sys.EnsurePlot("healthy", "room_a");
            Assert.True(sys.CultivateSpores("healthy", "strain_edible", "substrate_test", 1).IsSuccess);

            var blocked = sys.DisposeInfectedSubstrate("healthy", "discard");
            Assert.False(blocked.IsSuccess); // nothing contaminated — disposal refused

            healthy.hasToxicBloom = true;
            int waterBefore = inv.CountById("clean_water");
            var res = sys.DisposeInfectedSubstrate("healthy", "discard");
            Assert.True(res.IsSuccess);
            Assert.Equal(waterBefore, inv.CountById("clean_water")); // discard costs nothing
            Assert.Null(healthy.strainId);
        }

        [Fact]
        public void PurgeToxicBloom_ReleasesQuarantine()
        {
            var (sys, inv) = MakeSystem(42, CreateTestCatalog());
            var plot = sys.EnsurePlot("bed", "room_a");
            Assert.True(sys.CultivateSpores("bed", "strain_edible", "substrate_test", 1).IsSuccess);
            plot.hasToxicBloom = true;
            sys.DisposeInfectedSubstrate("bed", "quarantine");

            var res = sys.PurgeToxicBloom("bed");

            Assert.True(res.IsSuccess);
            Assert.False(plot.isQuarantined);
            Assert.Null(plot.strainId);
            Assert.Equal(48, inv.CountById("clean_water")); // 50 - 2
        }

        // ── Save safety ─────────────────────────────────────────────────

        [Fact]
        public void OldSaveBaseline_NewFieldsGetSafeDefaults()
        {
            // A pre-Plan-204 state payload (no substratePreparation/flush/quarantine fields).
            string legacyJson = "{\"schema_version\":1,\"plots\":[{\"plotId\":\"plot_old\",\"roomId\":\"room_old\","
                + "\"strainId\":\"strain_edible\",\"substrateId\":\"substrate_test\",\"growthStage\":0.5,"
                + "\"moisture\":0.6,\"sporeDensity\":0.2,\"contamination\":0.1,\"isHarvestReady\":false,"
                + "\"hasToxicBloom\":false,\"plantedDay\":3}],\"totalHarvests\":1,\"totalBlooms\":0}";

            var restored = System.Text.Json.JsonSerializer.Deserialize<FungiCultivationState>(legacyJson);
            Assert.NotNull(restored);

            var inv = new Inventory.Inventory();
            inv.AddById("clean_water", 20);
            inv.AddById("fuel", 20);
            inv.AddById("fungus_spores_common", 10);
            var sys = new FungiCultivationSystem(new SeededRng(42), inv);
            sys.RegisterCatalog(CreateTestCatalog());
            sys.RestoreState(restored);
            var plot = sys.State.plots[0];

            Assert.Equal(SubstratePreparation.Untreated, plot.substratePreparation);
            Assert.Equal(0, plot.remainingFlushes);
            Assert.False(plot.isQuarantined);

            // Legacy active plot still grows, and harvest behaves as the historical single flush.
            sys.EnsurePlot("plot_new", "room_new");
            Assert.True(sys.CultivateSpores("plot_new", "strain_edible", "substrate_test", 4).IsSuccess);
            plot.growthStage = 1.0f;
            plot.isHarvestReady = true;
            var legacyPlot = plot;
            legacyPlot.isHarvestReady = true;
            Assert.True(sys.HarvestPlot("plot_old").IsSuccess);
            Assert.Null(legacyPlot.strainId); // single-flush legacy plot returns to fallow
        }

        [Fact]
        public void SaveRoundTrip_PreservesPlan204Fields()
        {
            var (sys, _) = MakeSystem(42, CreateTestCatalog());
            sys.EnsurePlot("bed", "room_a");
            sys.PrepareSubstrate("bed", useHeat: true);
            Assert.True(sys.CultivateSpores("bed", "strain_edible", "substrate_test", 1).IsSuccess);
            var plot = sys.State.plots[0];
            plot.contamination = 0.4f;

            var json = System.Text.Json.JsonSerializer.Serialize(sys.State);
            var restored = System.Text.Json.JsonSerializer.Deserialize<FungiCultivationState>(json)!;

            Assert.Equal(2, restored.schema_version);
            var rp = restored.plots[0];
            Assert.Equal(plot.substratePreparation, rp.substratePreparation);
            Assert.Equal(plot.remainingFlushes, rp.remainingFlushes);
            Assert.Equal(plot.isQuarantined, rp.isQuarantined);
            Assert.Equal(plot.contamination, rp.contamination, 3);
        }

        // ── Deterministic replay ────────────────────────────────────────

        [Fact]
        public void DeterministicReplay_SameSeedSameCommands_IdenticalState()
        {
            var (a, invA) = MakeSystem(99, CreateTestCatalog());
            var (b, invB) = MakeSystem(99, CreateTestCatalog());

            foreach (var sys in new[] { (a, invA), (b, invB) })
            {
                var (s, inv) = sys;
                s.EnsurePlot("p1", "room_a");
                s.PrepareSubstrate("p1", useHeat: false);
                s.CultivateSpores("p1", "strain_edible", "substrate_test", 1);
                for (int d = 1; d <= 6; d++)
                {
                    s.WaterPlot("p1");
                    s.TickDay(d, roomIsDark: true, roomTemperatureC: 14f);
                }
                var plot = s.State.plots[0];
                if (plot.isHarvestReady) s.HarvestPlot("p1");
            }

            var pa = a.State.plots[0];
            var pb = b.State.plots[0];
            Assert.Equal(pa.substratePreparation, pb.substratePreparation);
            Assert.Equal(pa.growthStage, pb.growthStage, 4);
            Assert.Equal(pa.contamination, pb.contamination, 4);
            Assert.Equal(pa.sporeDensity, pb.sporeDensity, 4);
            Assert.Equal(pa.hasToxicBloom, pb.hasToxicBloom);
            Assert.Equal(pa.isHarvestReady, pb.isHarvestReady);
            Assert.Equal(pa.remainingFlushes, pb.remainingFlushes);
            Assert.Equal(a.State.totalHarvests, b.State.totalHarvests);
            Assert.Equal(invA.CountById("harvested_mushrooms_subterranean"), invB.CountById("harvested_mushrooms_subterranean"));
        }

        // ── Kitchen routing (data authority) ────────────────────────────

        [Fact]
        public void HarvestItem_IsEdibleInItemCatalog_KitchenRoutable()
        {
            string start = Directory.GetCurrentDirectory();
            Assert.True(CatalogLocator.TryFindDataDirectory(start, out string dataDir),
                "Could not locate Assets/StreamingAssets/Data from " + start);
            string itemsPath = Path.Combine(dataDir, "items.json");
            Assert.True(File.Exists(itemsPath), $"items.json not found at {itemsPath}");

            using var doc = System.Text.Json.JsonDocument.Parse(File.ReadAllText(itemsPath));
            var items = doc.RootElement.GetProperty("items");
            foreach (var item in items.EnumerateArray())
            {
                if (item.TryGetProperty("id", out var id) && id.GetString() == "harvested_mushrooms_subterranean")
                {
                    Assert.Equal("Food", item.GetProperty("type").GetString());
                    Assert.True(item.TryGetProperty("hungerRestore", out var hunger) && hunger.GetDouble() > 0d,
                        "Fungi harvest item must restore hunger to route through the kitchen/nutrition loop.");
                    return;
                }
            }
            Assert.Fail("harvested_mushrooms_subterranean missing from items.json");
        }
    }
}
