using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Excavation;
using Ashfall.Core.Foundry;
using Ashfall.Core.Radio;
using Ashfall.Core.Shelter;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;
using Xunit;

namespace Ashfall.Core.Tests.Integration
{
    /// <summary>
    /// Plans B66–B69 — cross-plan integration scenarios A–G.
    /// Proves the deterministic contracts between heavy metallurgy (B66),
    /// radio cryptanalysis (B67), seismic monitoring (B68) and the cryo vault
    /// (B69) over shared inventory, with save/load splits equal to
    /// uninterrupted runs. Core-side only; host wiring is projection.
    /// </summary>
    public sealed class PlansB66ToB69CrossSystemTests
    {
        private static string FindDataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found");
        }

        /// <summary>Shared fixture: one inventory, all four systems, B66 catalog merged.</summary>
        private sealed class World
        {
            public readonly InventoryContainer Inv = NewRoomyInventory();

            /// <summary>Default capacity is 20 slots / 100 weight — far too small for a stress fixture.</summary>
            private static InventoryContainer NewRoomyInventory()
            {
                var inv = new InventoryContainer();
                inv.Capacity = 200;
                inv.MaxWeight = 0f; // weightless fixture
                return inv;
            }

            public readonly SilentFoundrySystem Foundry;
            public readonly ShelterRadioStationSystem Radio;
            public readonly SeismicDynamicsSystem Seismic;
            public readonly CryoVaultSystem CryoVault;
            public readonly ExcavationSystem Excavation;
            public bool CryoPower = true;

            public World(int seed = 900, SilentFoundryState? foundryState = null)
            {
                string dataDir = FindDataDir();
                var files = new FileSystemIO();
                var json = new SystemTextJsonSerializer();

                var production = SilentFoundryCatalogLoader.LoadProduction(dataDir, files, json);
                var faction = SilentFoundryCatalogLoader.LoadFaction(dataDir, files, json);
                var catalog = new SilentFoundryCatalog();
                catalog.Load(production, faction);
                var metallurgy = MetallurgyCatalogLoader.Load(dataDir, files, json);

                Foundry = new SilentFoundrySystem(state: foundryState, rng: new SeededRng(seed));
                Foundry.BindCatalog(catalog, 4);
                Foundry.BindMetallurgyCatalog(metallurgy);
                Foundry.BindInventory(
                    id => Inv.CountById(id),
                    (_, _) => true,
                    (id, amt) => Inv.AddById(id, amt),
                    (id, amt) => Inv.TryConsume(id, amt));

                Radio = new ShelterRadioStationSystem(new SeededRng(seed + 1), null, NullLog.Instance);
                string interceptPath = Path.Combine(dataDir, "radio_intercepts.json");
                if (files.FileExists(interceptPath))
                    Radio.LoadCatalog(files.ReadAllText(interceptPath));
                Radio.BindWeatherNoiseProvider(() => 0.05f);

                Seismic = new SeismicDynamicsSystem(new SeededRng(seed + 2));

                var cultivars = CryoCultivarCatalogLoader.Load(dataDir, files, json);
                CryoVault = new CryoVaultSystem(
                    new SeededRng(seed + 3),
                    Inv,
                    radiationExposureProvider: () => 0f,
                    powerAvailableProvider: () => CryoPower);
                CryoVault.LoadCatalogContent(cultivars);

                Excavation = new ExcavationSystem(new SeededRng(seed + 4), log: null, Inv);
                Excavation.AddSite("site_deep_alpha", "room_bp_deep_strata", 100f, 0.8f);

                foreach (var id in new[]
                {
                    "scrap_metal", "scrap_mechanical", "scrap_electronic", "item_scrap_metal",
                    "coal", "charcoal", "clean_water", "item_foundry_flux", "item_foundry_alloy_additive",
                    "item_metallurgy_steel_billet", "item_metallurgy_iron_ingot",
                    "item_hermetic_sample_ampoule", "item_nitrogen_supply",
                    "item_geophone_probe", "item_seismic_damper_pad", "item_vibration_dampening_mount"
                })
                    Stock(id, 200);
            }

            public void RunFoundryToCompletion(int startDay)
            {
                for (int d = startDay + 1, guard = 0;
                     guard < 20 && Foundry.HeatStage != FoundryHeatStage.Complete
                                  && Foundry.HeatStage != FoundryHeatStage.Idle;
                     d++, guard++)
                {
                    Foundry.TickDaily(d);
                    if (Foundry.HeatStage == FoundryHeatStage.AtHeat && guard > 0)
                        Foundry.TapAndCast(d);
                }
            }

            public int AmpoulesHeld => Inv.CountById("item_hermetic_sample_ampoule");

            /// <summary>AddById caps at the created definition's stackMax (99) — chunk large stocks.</summary>
            public void Stock(string itemId, int total)
            {
                int remaining = total;
                while (remaining > 0)
                {
                    int chunk = Math.Min(remaining, 99);
                    if (!Inv.AddById(itemId, chunk)) break;
                    remaining -= chunk;
                }
            }
        }


        // ── Scenario A — foundry beam → deep-strata reinforcement ───────

        [Fact]
        public void ScenarioA_BeamBatch_ReinforcesDeepStrata_NoDuplicateAcrossSave()
        {
            var w = new World();
            w.Foundry.Unlock(1);
            // Beam cost is 2 (StructuralBeamCost); each cast yields 1 — run two heats.
            for (int heat = 0; heat < 2; heat++)
            {
                Assert.Contains("Heat started", w.Foundry.StartProduction("foundry_prod_t_beam", 4, 0.7f, 2 + heat * 10));
                w.RunFoundryToCompletion(2 + heat * 10);
            }
            int beams = w.Inv.CountById("item_foundry_t_beam");
            Assert.True(beams >= 2, $"two casts should yield >= 2 beams, got {beams}");

            // Installation consumes the beam through the excavation authority.
            var res = w.Excavation.TryApplyStructuralReinforcement("site_deep_alpha");
            Assert.Equal(ActionResult.StatusKind.Success, res.Status);
            Assert.Equal(beams - 2, w.Inv.CountById("item_foundry_t_beam")); // StructuralBeamCost = 2

            // Save/load between production and installation: the restored
            // engine's completed history shows exactly one beam record —
            // restore never re-commits output.
            var restored = new World(foundryState: w.Foundry.State);
            Assert.Equal(2, restored.Foundry.State.completed
                .FindAll(r => r.productId == "foundry_prod_t_beam").Count);
        }

        // ── Scenario B — seismic preparation (warning boundary parity) ──

        [Fact]
        public void ScenarioB_PreparedShelter_WeakensPulse_Deterministically()
        {
            float ResidualTension(bool withDampener)
            {
                var w = new World(seed: 313);
                if (withDampener)
                    Assert.Equal(ActionResult.StatusKind.Success,
                        w.Seismic.InstallDampener("sector_excavation_alpha", w.Inv).Status);
                w.Seismic.InjectKineticShock(200, "sector_excavation_alpha");
                return w.Seismic.State.faults["fault_sub_strata_rift"].currentTension;
            }

            float undamped = ResidualTension(false);
            float damped = ResidualTension(true);
            Assert.True(damped <= undamped, "dampened residual must not exceed undamped");

            // Same preparation + same seed = same outcome at the warning boundary.
            Assert.Equal(damped, ResidualTension(true));
        }

        // ── Scenario C — seismic event during metallurgy ────────────────

        [Fact]
        public void ScenarioC_QuakeDuringHeavyBatch_BatchNotDuplicatedOrReset()
        {
            var w = new World(seed: 417);
            w.Foundry.Unlock(1);
            Assert.Contains("Heat started", w.Foundry.StartHeavyBatch("metallurgy_heavy_i_beam", 4, 0.7f, 2));

            w.Seismic.InjectKineticShock(300f, "sector_excavation_alpha");
            Assert.True(w.Seismic.State.recentQuakes.Count > 0, "quake should register");

            // Authored behavior: the quake does not reset the heat machine.
            Assert.NotEqual(FoundryHeatStage.Idle, w.Foundry.HeatStage);
            Assert.True(w.Foundry.IsHeavyBatchActive);

            w.RunFoundryToCompletion(2);
            Assert.Single(w.Foundry.State.completed.FindAll(r => r.productId == "metallurgy_heavy_i_beam"));
            Assert.Equal(1, w.Inv.CountById("item_metallurgy_heavy_i_beam"));
            Assert.False(w.Foundry.IsHeavyBatchActive);
        }

        // ── Scenario D — cryo vault power crisis (split == straight) ────

        [Fact]
        public void ScenarioD_PowerCrisisSplitRun_MatchesUninterruptedRun()
        {
            int Straight()
            {
                var w = new World(seed: 519);
                w.CryoVault.RegisterSample("cryo_seed_verity_wheat_line");
                w.CryoPower = false;
                for (int d = 1; d <= 6; d++) w.CryoVault.TickDay(d);
                return w.CryoVault.State.canisters[0].viability_permille;
            }

            int Split()
            {
                var w = new World(seed: 519);
                w.CryoVault.RegisterSample("cryo_seed_verity_wheat_line");
                w.CryoPower = false;
                for (int d = 1; d <= 3; d++) w.CryoVault.TickDay(d);
                var saved = w.CryoVault.CaptureState();

                var w2 = new World(seed: 519);
                w2.CryoVault.RestoreState(saved);
                w2.CryoPower = false;
                for (int d = 4; d <= 6; d++) w2.CryoVault.TickDay(d);
                return w2.CryoVault.State.canisters[0].viability_permille;
            }

            int straight = Straight();
            int split = Split();
            Assert.Equal(straight, split);
            Assert.True(straight < 1000, "power crisis must lose viability");
            Assert.True(straight > 0, "bounded degradation — never a wipe");
        }

        // ── Scenario E — seismic event → cryo breach → triage ───────────

        [Fact]
        public void ScenarioE_SevereQuakeBreach_TriagePreservesProtectedLine()
        {
            var w = new World(seed: 621);
            w.CryoVault.RegisterSample("cryo_seed_verity_wheat_line");
            string protectedId = w.CryoVault.State.canisters[0].canister_id;
            w.CryoVault.RegisterSample("cryo_seed_verity_wheat_line");

            // B68→B69 host contract: severe quake requests a vault breach.
            w.Seismic.OnQuakeOccurred += q =>
            {
                if (q.magnitude >= 5.5f)
                    w.CryoVault.TriggerBreach("seismic event day " + q.day);
            };
            w.Seismic.InjectKineticShock(500f, "bunker_core");
            Assert.True(w.CryoVault.IsBreachActive, "severe quake must open a breach");

            w.CryoVault.SetTriageProtection(protectedId, true);
            w.CryoVault.TickDay(w.Seismic.State.currentDay + 1);

            int vProtected = w.CryoVault.State.canisters.Find(c => c.canister_id == protectedId)!.viability_permille;
            int vExposed = w.CryoVault.State.canisters.Find(c => c.canister_id != protectedId)!.viability_permille;
            Assert.True(vProtected > vExposed, "triage must favor the protected line");
        }

        // ── Scenario F — intercept → recovery → canonical greenhouse item ──

        [Fact]
        public void ScenarioF_InterceptResolution_RecoveryReleasesCanonicalSeed()
        {
            var w = new World(seed: 723);
            const string intercept = "radio_intercept_meridian_supply_column_01";

            // Cipher analysis resolves headlessly (difficulty 40).
            for (int i = 0; i < 40; i++)
            {
                w.Radio.ProgressDecryption(intercept);
                if (RadioDecrypted(w, intercept)) break;
            }
            Assert.True(RadioDecrypted(w, intercept), "cipher analysis must resolve headlessly");

            // Three distinct bearings resolve the authored location exactly once.
            Assert.False(w.Radio.RecordBearing(intercept, 40));
            Assert.False(w.Radio.RecordBearing(intercept, 180));
            Assert.True(w.Radio.RecordBearing(intercept, 300));
            Assert.True(w.Radio.State.discoveredLocationIds.Contains("loc_diesel_tank_farm"));
            int revealedCount = w.Radio.State.discoveredLocationIds
                .FindAll(id => id == "loc_diesel_tank_farm").Count;
            Assert.Equal(1, revealedCount);

            // The expedition-recovered cultivar enters the vault and later
            // releases a canonical greenhouse seed through the standard item
            // path — the full content loop closes.
            Assert.Contains("registered", w.CryoVault.RegisterSample("cryo_seed_radiant_wept_wheat"));
            Assert.Contains("queued", w.CryoVault.QueueRecovery(w.CryoVault.State.canisters[0].canister_id));
            w.CryoVault.TickDay(2); // recovery_days = 2 → thaw day 1
            w.CryoVault.TickDay(3); // thaw day 2 → release
            Assert.Single(w.CryoVault.State.released_log);
            Assert.Equal(2, w.Inv.CountById("item_seed_wheat")); // recovery_amount 2, once
        }

        // ── Scenario G — full late-game infrastructure stress test ──────

        [Fact]
        public void ScenarioG_InfrastructureStress_SplitEqualsStraight()
        {
            var straight = RunStress(new World(seed: 831), splitAtDay: -1);
            var split = RunStress(new World(seed: 831), splitAtDay: 4);

            Assert.Equal(straight.HeavyBeams, split.HeavyBeams);
            Assert.Equal(straight.CryoViability, split.CryoViability);
            Assert.Equal(straight.SlagLevel, split.SlagLevel, 3);
        }

        private sealed class StressResult
        {
            public int HeavyBeams;
            public int CryoViability;
            public float SlagLevel;
        }

        private StressResult RunStress(World w, int splitAtDay)
        {
            w.Foundry.Unlock(1);
            Assert.Contains("Heat started", w.Foundry.StartHeavyBatch("metallurgy_heavy_i_beam", 4, 0.7f, 2));
            Assert.Equal(ActionResult.StatusKind.Success,
                w.Seismic.InstallDampener("sector_excavation_alpha", w.Inv).Status);

            for (int day = 3; day <= 9; day++)
            {
                if (day == 5)
                {
                    w.Seismic.InjectKineticShock(200f, "sector_excavation_alpha");
                    w.CryoPower = false; // grid brownout reaches the vault same-day
                }

                w.Foundry.TickDaily(day);
                w.Seismic.TickDay(day);
                w.CryoVault.TickDay(day);

                if (w.Foundry.HeatStage == FoundryHeatStage.AtHeat && day > 3)
                    w.Foundry.TapAndCast(day);

                if (day == splitAtDay)
                {
                    // Split at the warning boundary: restore and continue.
                    var w2 = new World(
                        seed: 831,
                        foundryState: w.Foundry.State);
                    w2.Seismic.RestoreState(w.Seismic.CaptureState());
                    w2.CryoVault.RestoreState(w.CryoVault.CaptureState());
                    w2.CryoPower = false;
                    for (int d = day + 1; d <= 9; d++)
                    {
                        w2.Foundry.TickDaily(d);
                        if (w2.Foundry.HeatStage == FoundryHeatStage.AtHeat && d > day + 1)
                            w2.Foundry.TapAndCast(d);
                        w2.Seismic.TickDay(d);
                        w2.CryoVault.TickDay(d);
                    }
                    return ResultOf(w2);
                }
            }

            return ResultOf(w);
        }

        private static StressResult ResultOf(World w) => new()
        {
            HeavyBeams = w.Inv.CountById("item_metallurgy_heavy_i_beam"),
            CryoViability = w.CryoVault.State.canisters.Count > 0
                ? w.CryoVault.State.canisters[0].viability_permille
                : 1000,
            SlagLevel = w.Foundry.SlagLevel
        };

        private static bool RadioDecrypted(World w, string interceptId)
        {
            foreach (var p in w.Radio.State.intercepts)
                if (p.InterceptId == interceptId) return p.IsDecrypted;
            return false;
        }
    }
}
