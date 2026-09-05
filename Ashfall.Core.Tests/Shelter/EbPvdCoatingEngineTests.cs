// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.AdvancedMachinery;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Shelter
{
    public sealed class EbPvdCoatingEngineTests
    {
        private static string FindDataDir()
        {
            if (!CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out var dir))
                CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out dir);
            return dir ?? string.Empty;
        }

        /// <summary>All-or-nothing inventory host: validates every line, then consumes.</summary>
        private static Func<IReadOnlyList<InventoryDemand>, bool> MakeConsumer(Dictionary<string, int> inventory)
        {
            return demands =>
            {
                foreach (var d in demands)
                {
                    if (!inventory.TryGetValue(d.ItemId, out int current) || current < d.Quantity)
                        return false;
                }
                foreach (var d in demands)
                {
                    inventory[d.ItemId] -= d.Quantity;
                }
                return true;
            };
        }

        [Fact]
        public void BeamPowerMath_CalculatesDimensionallyCorrectKilowatts()
        {
            float kw = EbPvdCoatingEngine.ComputeBeamPowerKw(20.0f, 0.5f);
            Assert.Equal(10.0f, kw);

            float kw2 = EbPvdCoatingEngine.ComputeBeamPowerKw(25.0f, 1.2f);
            Assert.Equal(30.0f, kw2, 3);
        }

        [Fact]
        public void StartJob_AtomicInventoryConsumption()
        {
            var engine = new EbPvdCoatingEngine();
            var inventory = new Dictionary<string, int>
            {
                { "item_ebpvd_ceramic_target_ingot", 1 },
                { "item_mcraly_bond_coat_powder", 1 }
            };
            var consume = MakeConsumer(inventory);

            bool ok = engine.StartJob("job_01", "ebpvd_tbc_yttria_stabilized_zirconia", "superalloy_blade", "op_1", 1, true, consume, out string err);
            Assert.True(ok, err);
            Assert.Equal(0, inventory["item_ebpvd_ceramic_target_ingot"]);
            Assert.Equal(0, inventory["item_mcraly_bond_coat_powder"]);
            Assert.NotNull(engine.StateDto.ActiveJob);
            Assert.Equal(ProcessState.Running, engine.State);

            // Starting another job while running should be blocked
            bool ok2 = engine.StartJob("job_02", "ebpvd_tbc_yttria_stabilized_zirconia", "superalloy_blade", "op_1", 1, true, consume, out string err2);
            Assert.False(ok2);
            Assert.Equal("job_already_active", err2);
        }

        [Fact]
        public void StartJob_MissingBondCoat_ConsumesNothing()
        {
            var engine = new EbPvdCoatingEngine();
            var inventory = new Dictionary<string, int>
            {
                { "item_ebpvd_ceramic_target_ingot", 2 }
                // bond coat powder absent
            };
            var consume = MakeConsumer(inventory);

            bool ok = engine.StartJob("job_01", "ebpvd_tbc_yttria_stabilized_zirconia", "superalloy_blade", "op_1", 1, true, consume, out string err);
            Assert.False(ok);
            Assert.Equal("insufficient_coating_inputs", err);
            // Atomicity: the failed start must not have consumed the ceramic target
            Assert.Equal(2, inventory["item_ebpvd_ceramic_target_ingot"]);
            Assert.Null(engine.StateDto.ActiveJob);
        }

        [Fact]
        public void StartJob_WhilePaused_IsRejectedUntilResumedOrFailed()
        {
            var engine = new EbPvdCoatingEngine();
            var consume = MakeConsumer(new Dictionary<string, int>());
            engine.StartJob("job_01", "ebpvd_tbc_alumina_barrier", "diesel_injector", "op_1", 1, false, demands => true, out _);

            var severeBrownout = new PowerSupplyContext { AvailablePowerKw = 2.0f, PowerStable = false, BrownoutSeverity = 0.9f };
            engine.Tick(1.0f, severeBrownout, null, new SeededRng(1));
            Assert.Equal(ProcessState.Paused, engine.State);

            // A paused job still holds its consumed materials and progress —
            // starting over it must be rejected, not silently destroy it.
            bool ok = engine.StartJob("job_02", "ebpvd_tbc_alumina_barrier", "diesel_injector", "op_1", 1, false, demands => true, out string err);
            Assert.False(ok);
            Assert.Equal("job_paused_resume_or_fail", err);

            Assert.True(engine.ResumeJob());
            Assert.Equal(ProcessState.Running, engine.State);
        }

        [Fact]
        public void Tick_BrownoutAutoPausesJob()
        {
            var engine = new EbPvdCoatingEngine();
            engine.StartJob("job_01", "ebpvd_tbc_yttria_stabilized_zirconia", "superalloy_blade", "op_1", 1, false, demands => true, out _);

            var severeBrownout = new PowerSupplyContext { AvailablePowerKw = 2.0f, PowerStable = false, BrownoutSeverity = 0.9f };
            var rng = new SeededRng(12345);

            engine.Tick(1.0f, severeBrownout, null, rng);
            Assert.Equal(ProcessState.Paused, engine.State);
        }

        [Fact]
        public void ProcessTick_ProgressesToCompletionDeterministically()
        {
            var engine = new EbPvdCoatingEngine();
            engine.StartJob("job_01", "ebpvd_tbc_alumina_barrier", "superalloy_blade", "op_1", 1, true, demands => true, out _);

            var stablePower = PowerSupplyContext.Full(15.0f);
            var rng = new SeededRng(777);

            // Required hours for alumina barrier is 3.0f
            engine.Tick(3.5f, stablePower, null, rng);

            Assert.Equal(ProcessState.Completed, engine.State);
            Assert.Single(engine.StateDto.CompletedRecords);
            var record = engine.StateDto.CompletedRecords[0];
            Assert.Equal("ebpvd_tbc_alumina_barrier", record.CoatingId);
            Assert.Equal(50.0f, record.ThicknessUm);
            Assert.True(record.Uniformity01 > 0.8f);
        }

        [Fact]
        public void SameSeed_FullLifecycleReplaysIdentically()
        {
            static (EbPvdCoatingEngine engine, float thickness, float spallation) RunLifecycle(int seed)
            {
                var engine = new EbPvdCoatingEngine();
                engine.StartJob("job_r", "ebpvd_tbc_alumina_barrier", "superalloy_blade", "op_1", 1, true, demands => true, out _);
                var rng = new SeededRng(seed);
                engine.Tick(3.5f, PowerSupplyContext.Full(15f), null, rng);
                var rec = engine.StateDto.CompletedRecords.Count > 0 ? engine.StateDto.CompletedRecords[0] : null;
                return (engine, rec?.ThicknessUm ?? -1f, rec?.SpallationRisk01 ?? -1f);
            }

            var run1 = RunLifecycle(20260905);
            var run2 = RunLifecycle(20260905);

            Assert.Equal(run1.engine.State, run2.engine.State);
            Assert.Equal(run1.engine.StateDto.CompletedRecords.Count, run2.engine.StateDto.CompletedRecords.Count);
            Assert.Equal(run1.thickness, run2.thickness);
            Assert.Equal(run1.spallation, run2.spallation);
        }

        [Fact]
        public void CaptureState_DoesNotAliasLiveState()
        {
            var engine = new EbPvdCoatingEngine();
            engine.StartJob("job_01", "ebpvd_tbc_gadolinium_zirconate", "combustor_liner", "op_2", 4, true, demands => true, out _);
            engine.Tick(1.0f, PowerSupplyContext.Full(20f), null, new SeededRng(100));

            var captured = engine.CaptureState();
            Assert.NotNull(captured.ActiveJob);

            // Mutating the captured DTO must not leak into the live engine
            captured.ActiveJob!.ProgressHours = 999f;
            captured.CompletedRecords.Add(new EbPvdCoatedRecord { InstanceId = "fake" });

            Assert.True(engine.StateDto.ActiveJob!.ProgressHours < 10f);
            Assert.Empty(engine.StateDto.CompletedRecords);
        }

        [Fact]
        public void StateCaptureAndRestore_RoundtripsAccurately()
        {
            var engine = new EbPvdCoatingEngine();
            engine.StartJob("job_01", "ebpvd_tbc_gadolinium_zirconate", "combustor_liner", "op_2", 4, true, demands => true, out _);
            engine.Tick(1.0f, PowerSupplyContext.Full(20f), null, new SeededRng(100));

            var captured = engine.CaptureState();
            Assert.NotNull(captured.ActiveJob);
            Assert.Equal(1.0f, captured.OperatingHours);

            var restored = new EbPvdCoatingEngine(captured);
            Assert.NotNull(restored.StateDto.ActiveJob);
            Assert.Equal("job_01", restored.ActiveJobId);
            Assert.Equal(1.0f, restored.StateDto.OperatingHours);
        }

        [Fact]
        public void Catalog_LoadsAndMatchesEngineAuthority()
        {
            string dataDir = FindDataDir();
            Assert.False(string.IsNullOrEmpty(dataDir), "Could not locate StreamingAssets/Data directory");

            var catalog = EbPvdCoatingCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.Equal(3, catalog.coatings.Count);
            Assert.Equal(3, catalog.failure_profiles.Count);
            Assert.Contains(catalog.failure_profiles, p => p.code == "thermal_overheat");

            var engine = new EbPvdCoatingEngine();
            var catalogIds = catalog.ToCoatingDefs().Select(d => d.Id).OrderBy(x => x, StringComparer.Ordinal).ToList();
            var engineIds = engine.Coatings.Select(d => d.Id).OrderBy(x => x, StringComparer.Ordinal).ToList();
            Assert.Equal(catalogIds, engineIds);

            var gadolinium = catalog.ToCoatingDefs().First(d => d.Id == "ebpvd_tbc_gadolinium_zirconate");
            Assert.Equal(150.0f, gadolinium.TargetThicknessUm);

            // Catalog overrides the engine's built-in failure profiles
            engine.SetFailureProfiles(catalog.ToFailureProfiles());
            Assert.Equal(3, engine.FailureProfiles.Count);

            // Substrate classes map coated records to physical result items
            Assert.True(catalog.TryGetSubstrateResultItem("superalloy_blade", out var resultItem));
            Assert.Equal("item_coated_turbine_blade", resultItem);
        }
    }
}
