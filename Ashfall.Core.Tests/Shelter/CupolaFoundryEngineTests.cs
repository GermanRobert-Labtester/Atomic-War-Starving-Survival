// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Inventory;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Shelter
{
    /// <summary>
    /// Plan 90 — cupola foundry engine: atomic charging, blower gating,
    /// refractory/slag wear, deterministic casting quality, trait modifiers,
    /// persistence without replay, and the excavation beam-consumption loop.
    /// Catalogs used in most tests declare a zero hazard class so wear and
    /// quality assertions are independent of the hazard roll.
    /// </summary>
    public sealed class CupolaFoundryEngineTests
    {
        private const string Feedstock = "scrap_metal";
        private const string Fuel = "coal";
        private const string Flux = "item_foundry_flux";
        private const string Ingot = "item_foundry_grey_iron_ingot";

        private static CupolaFoundryCatalog TestCatalog(float hazardRating = 0f)
        {
            var charge = new CupolaChargeDefinition
            {
                id = "charge_scrap_bulk",
                display_name = "Bulk Scrap Charge",
                feedstock_item_id = Feedstock,
                feedstock_quantity = 6,
                fuel_item_id = Fuel,
                fuel_quantity = 3,
                flux_item_id = Flux,
                flux_quantity = 1,
                required_blower_power_w = 150f,
                heat_band = "MeltReady",
                melt_ticks = 3,
                refractory_wear_per_batch = 2.5f,
                slag_load = 1.0f,
                allowed_mold_ids = new List<string> { "mold_ingot" },
                base_yield_item_id = Ingot,
                base_yield_quantity = 2,
                hazard_rating = hazardRating
            };
            var mold = new FoundryMoldProfile
            {
                id = "mold_ingot",
                display_name = "Ingot Trough Mold",
                output_item_id = Ingot,
                output_quantity = 2,
                metal_units_required = 4,
                quality_target = 55f,
                wear_per_cast = 0.4f
            };
            var maintenance = new CupolaMaintenanceProfile
            {
                id = "cupola_reline_maintenance",
                refractory_item_id = "item_foundry_firebrick",
                refractory_quantity = 2,
                refractory_restore = 35f,
                slag_reduction = 60f,
                descale_item_id = "item_foundry_pickling_reagent",
                descale_quantity = 1,
                descale_slag_reduction = 25f
            };
            return new CupolaFoundryCatalog(
                new[] { charge }, new[] { mold }, maintenance);
        }

        private static Inventory.Inventory StockedInventory()
        {
            var inv = new Inventory.Inventory();
            Assert.True(inv.AddById(Feedstock, 40));
            Assert.True(inv.AddById(Fuel, 40));
            Assert.True(inv.AddById(Flux, 20));
            return inv;
        }

        private static CupolaFoundryEngine Engine(
            Inventory.Inventory? inv = null,
            int seed = 90,
            float hazardRating = 0f,
            Func<float>? power = null,
            Func<string, IReadOnlyList<string>>? traits = null)
        {
            return new CupolaFoundryEngine(
                inv ?? StockedInventory(),
                TestCatalog(hazardRating),
                new SeededRng(seed),
                log: null,
                traitsOf: traits,
                availablePowerWatts: power ?? (() => 500f));
        }

        private static int Count(Inventory.Inventory inv, string itemId)
        {
            int n = 0;
            foreach (var stack in inv.Slots)
            {
                if (string.Equals(stack.Item.id, itemId, StringComparison.OrdinalIgnoreCase))
                    n += stack.Amount;
            }
            return n;
        }

        [Fact]
        public void StartBatch_CommitsBillOnce_AndEntersMelting()
        {
            var inv = StockedInventory();
            var engine = Engine(inv);

            Assert.True(engine.TryStartFoundryBatch("charge_scrap_bulk", "mold_ingot", "surv_a"));

            Assert.Equal(34, Count(inv, Feedstock));   // 40 - 6
            Assert.Equal(37, Count(inv, Fuel));        // 40 - 3
            Assert.Equal(19, Count(inv, Flux));        // 20 - 1
            Assert.Equal(CupolaBatchPhase.Melting, (CupolaBatchPhase)engine.Furnace.batch_phase);
        }

        [Fact]
        public void StartBatch_MissingMaterial_ConsumesNothing()
        {
            var inv = StockedInventory();
            Assert.True(inv.TryConsumeBill(new Dictionary<string, int> { [Flux] = 20 }));
            var engine = Engine(inv);

            Assert.False(engine.TryStartFoundryBatch("charge_scrap_bulk", "mold_ingot"));
            Assert.Equal(40, Count(inv, Feedstock));
            Assert.Equal(40, Count(inv, Fuel));
            Assert.Equal(CupolaBatchPhase.Idle, (CupolaBatchPhase)engine.Furnace.batch_phase);
        }

        [Fact]
        public void StartBatch_UnknownChargeOrMold_Rejected()
        {
            var engine = Engine();
            Assert.False(engine.TryStartFoundryBatch("charge_nope", "mold_ingot"));
            Assert.False(engine.TryStartFoundryBatch("charge_scrap_bulk", "mold_nope"));
            Assert.Equal(CupolaBatchPhase.Idle, (CupolaBatchPhase)engine.Furnace.batch_phase);
        }

        [Fact]
        public void StartBatch_NoBlowerPower_RejectedWithoutConsumption()
        {
            var inv = StockedInventory();
            var engine = Engine(inv, power: () => 100f); // below 150W requirement

            Assert.False(engine.TryStartFoundryBatch("charge_scrap_bulk", "mold_ingot"));
            Assert.Equal(40, Count(inv, Feedstock));
            Assert.Equal(40, Count(inv, Fuel));
        }

        [Fact]
        public void PowerLoss_MidBatch_StallsProgress_WithoutAdvancing()
        {
            var inv = StockedInventory();
            float watts = 500f;
            var engine = Engine(inv, power: () => watts);
            Assert.True(engine.TryStartFoundryBatch("charge_scrap_bulk", "mold_ingot"));

            engine.TickDay(1);
            Assert.Equal(CupolaBatchPhase.Melting, (CupolaBatchPhase)engine.Furnace.batch_phase);

            watts = 0f; // brownout
            engine.TickDay(2);
            Assert.Equal(1, engine.Furnace.melt_progress); // stalled day did not advance
            Assert.Equal(CupolaFailureState.Stalled, (CupolaFailureState)engine.Furnace.failure_state);
            Assert.False(engine.Furnace.blower_available);

            watts = 500f; // power restored — melt resumes
            engine.TickDay(3);
            engine.TickDay(4);
            Assert.Equal(3, engine.Furnace.melt_progress);
            Assert.Equal(CupolaBatchPhase.ReadyToPour, (CupolaBatchPhase)engine.Furnace.batch_phase);
        }

        [Fact]
        public void FullCycle_GrantsMoldOutput_ExactlyOnce()
        {
            var inv = StockedInventory();
            var engine = Engine(inv, seed: 9001);

            Assert.True(engine.TryStartFoundryBatch("charge_scrap_bulk", "mold_ingot"));
            engine.TickDay(1);
            engine.TickDay(2);
            engine.TickDay(3);

            Assert.Equal(CupolaBatchPhase.ReadyToPour, (CupolaBatchPhase)engine.Furnace.batch_phase);
            var result = engine.TryTapMold();
            Assert.NotNull(result);
            Assert.True(result!.granted_quantity >= 0 && result.granted_quantity <= 2);
            Assert.Equal(result.granted_quantity, Count(inv, Ingot)); // granted once, into inventory
            Assert.Equal(CupolaBatchPhase.Idle, (CupolaBatchPhase)engine.Furnace.batch_phase);
            Assert.Equal(1, engine.BatchesCompleted);
        }

        [Fact]
        public void Quality_IsDeterministicUnderSeed()
        {
            var a = RunSeededCast(seed: 4242);
            var b = RunSeededCast(seed: 4242);
            var c = RunSeededCast(seed: 4243);

            Assert.Equal(a!.defect, b!.defect);
            Assert.Equal(a.quality_score, b.quality_score, 3);
            Assert.Equal(a.granted_quantity, b.granted_quantity);
            // A different seed produces a full record to compare against; the
            // draw order is fixed so any divergence would break replay tests.
            Assert.InRange(c!.quality_score, 0f, 100f);
        }

        private static CupolaCastResult? RunSeededCast(int seed)
        {
            var engine = Engine(seed: seed);
            engine.TryStartFoundryBatch("charge_scrap_bulk", "mold_ingot");
            for (int day = 1; day <= 3; day++) engine.TickDay(day);
            return engine.TryTapMold();
        }

        [Fact]
        public void FoundryMasterTrait_RaisesQualityScore()
        {
            var plain = RunCast(workerId: "surv_plain", traits: null);
            var master = RunCast(workerId: "surv_master", traits: new Dictionary<string, IReadOnlyList<string>>
            {
                ["surv_master"] = new[] { CupolaFoundryEngine.TraitFoundryMaster, CupolaFoundryEngine.TraitPatternmaker }
            });

            Assert.True(master!.quality_score > plain!.quality_score,
                $"master {master.quality_score} should exceed plain {plain.quality_score}");
        }

        private static CupolaCastResult? RunCast(string workerId, Dictionary<string, IReadOnlyList<string>>? traits)
        {
            var engine = Engine(seed: 77, traits: traits != null
                ? id => traits.TryGetValue(id, out var t) ? t : Array.Empty<string>()
                : (Func<string, IReadOnlyList<string>>?)null);
            engine.TryStartFoundryBatch("charge_scrap_bulk", "mold_ingot", workerId);
            for (int day = 1; day <= 3; day++) engine.TickDay(day);
            return engine.TryTapMold();
        }

        [Fact]
        public void RefractoryWears_Down_And_MaintenanceRestoresAtomically()
        {
            var inv = StockedInventory();
            Assert.True(inv.AddById("item_foundry_firebrick", 6));
            Assert.True(inv.AddById("item_foundry_pickling_reagent", 4));
            var engine = Engine(inv, seed: 5);

            float startIntegrity = engine.Furnace.refractory_integrity;
            RunTwoCycles(engine);
            Assert.True(engine.Furnace.refractory_integrity < startIntegrity);

            // Insufficient bricks → no change (atomic refusal).
            Assert.True(inv.TryConsumeBill(new Dictionary<string, int> { ["item_foundry_firebrick"] = 5 })); // 1 left, need 2
            float before = engine.Furnace.refractory_integrity;
            Assert.False(engine.TryServiceCupola(includeDescale: true));
            Assert.Equal(before, engine.Furnace.refractory_integrity);

            // Provide bricks → service restores integrity and drops slag.
            Assert.True(inv.AddById("item_foundry_firebrick", 3));
            float slagBefore = engine.Furnace.slag_level;
            Assert.True(engine.TryServiceCupola(includeDescale: true));
            Assert.True(engine.Furnace.refractory_integrity > before);
            Assert.True(engine.Furnace.slag_level < slagBefore);
        }

        private static void RunTwoCycles(CupolaFoundryEngine engine)
        {
            for (int i = 0; i < 2; i++)
            {
                Assert.True(engine.TryStartFoundryBatch("charge_scrap_bulk", "mold_ingot"));
                for (int day = 1; day <= 3; day++) engine.TickDay(day);
                Assert.NotNull(engine.TryTapMold());
            }
        }

        [Fact]
        public void SaveMidBatch_RestoreDoesNotAdvanceOrProduce()
        {
            var invA = StockedInventory();
            var engineA = Engine(invA, seed: 31337);
            Assert.True(engineA.TryStartFoundryBatch("charge_scrap_bulk", "mold_ingot"));
            engineA.TickDay(1);
            var saved = engineA.CaptureState();

            // Fresh engine restores mid-batch state; nothing moves until a real tick.
            var invB = StockedInventory();
            var engineB = Engine(invB, seed: 31337);
            engineB.RestoreState(saved);

            Assert.Equal(engineA.Furnace.melt_progress, engineB.Furnace.melt_progress);
            Assert.Equal(CupolaBatchPhase.Melting, (CupolaBatchPhase)engineB.Furnace.batch_phase);
            Assert.Equal(0, Count(invB, Ingot));     // restore never produced
            Assert.Equal(40, Count(invB, Feedstock)); // restore never consumed

            // Restored batch completes on the same schedule and outcome as a
            // fresh engine restored from the same boundary — replay-safe.
            var b = RunRestoredCycle(StockedInventory(), saved);
            var c = RunRestoredCycle(StockedInventory(), saved);
            Assert.NotNull(b);
            Assert.NotNull(c);
            Assert.Equal(b!.defect, c!.defect);
            Assert.Equal(b.granted_quantity, c.granted_quantity);
        }

        private static CupolaCastResult? RunRestoredCycle(Inventory.Inventory inv, CupolaFoundrySave saved)
        {
            var engine = Engine(inv, seed: 31337);
            engine.RestoreState(saved);
            engine.TickDay(2);
            engine.TickDay(3);
            engine.TickDay(4);
            return engine.TryTapMold();
        }

        [Fact]
        public void RefractoryTooDamaged_BlocksNewBatches()
        {
            var engine = Engine(seed: 11);
            engine.Furnace.refractory_integrity = 10f;

            Assert.False(engine.TryStartFoundryBatch("charge_scrap_bulk", "mold_ingot"));
        }

        [Fact]
        public void HazardRoll_CanChillBatch_ButStaysDeterministic()
        {
            // Hazard-class charge: across many days a chill eventually lands;
            // the exact day is fixed by the seed.
            int? chillDay = null;
            for (int run = 0; run < 2; run++)
            {
                var engine = Engine(seed: 4040, hazardRating: 0.4f);
                Assert.True(engine.TryStartFoundryBatch("charge_scrap_bulk", "mold_ingot"));
                int? localChill = null;
                for (int day = 1; day <= 20; day++)
                {
                    engine.TickDay(day);
                    if ((CupolaFailureState)engine.Furnace.failure_state == CupolaFailureState.Chilled
                        && engine.Furnace.batch_phase == (int)CupolaBatchPhase.Idle)
                    {
                        localChill = day;
                        break;
                    }
                    if ((CupolaBatchPhase)engine.Furnace.batch_phase == CupolaBatchPhase.ReadyToPour)
                        break; // melt survived to pouring
                }
                if (run == 0) chillDay = localChill;
                else Assert.Equal(chillDay, localChill); // identical replay
            }
            Assert.True(chillDay.HasValue, "a 0.4 hazard charge should chill within 20 days from a worn lining");
        }

        [Fact]
        public void ExcavationReinforcement_ConsumesCastBeamsAtomically()
        {
            var inv = new Inventory.Inventory();
            Assert.True(inv.AddById(ExcavationSystem.StructuralBeamItemId, 2));
            var excavation = new ExcavationSystem(new SeededRng(7), log: null, inventory: inv);
            excavation.AddSite("site_gallery_a", "room_cistern", 100f, 0.4f);

            var ok = excavation.TryApplyStructuralReinforcement("site_gallery_a");
            Assert.True(ok.IsSuccess);
            Assert.Equal(0, Count(inv, ExcavationSystem.StructuralBeamItemId)); // both beams set
            var site = excavation.State.sites.Single(s => s.siteId == "site_gallery_a");
            Assert.Equal(1, site.reinforcedBeams);
            Assert.Equal(0.2f, site.structuralRisk, 3);

            // No beams left → clean failure, no state change.
            var fail = excavation.TryApplyStructuralReinforcement("site_gallery_a");
            Assert.False(fail.IsSuccess);
            Assert.Equal(1, site.reinforcedBeams);
        }

        [Fact]
        public void ExcavationReinforcement_WithoutInventory_FailsCleanly()
        {
            var excavation = new ExcavationSystem(new SeededRng(7));
            excavation.AddSite("site_gallery_b", "room_cistern", 100f, 0.4f);

            var result = excavation.TryApplyStructuralReinforcement("site_gallery_b");
            Assert.False(result.IsSuccess);
        }
    }
}
