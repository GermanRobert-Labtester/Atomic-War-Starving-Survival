using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Disease;
using Ashfall.Core.Medical;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Shelter
{
    /// <summary>
    /// SHELTER_EMP_MEDICAL_POWER Phase 2: power dependency of the water
    /// treatment batch model and quarantine isolation quality.
    /// </summary>
    public sealed class WaterAndQuarantinePowerTests
    {
        // ── Water treatment power gate ──────────────────────────────────

        [Fact]
        public void Water_UnpoweredTick_PausesBatch_ProgressFrozen()
        {
            var system = new WaterTreatmentSystem();
            system.AddWater(WaterType.Raw, 50f);
            system.AddCharcoal(100f);
            var start = system.StartTreatment(TreatmentMode.CharcoalFiltration, 10f);
            Assert.True(start.IsSuccess, start.FailureCode);

            // Unpowered day: batch pauses, progress frozen, job retained.
            system.TickDay(1, 0f);
            Assert.True(system.IsProcessing);
            Assert.Equal(0f, system.State.processingProgress);
        }

        [Fact]
        public void Water_PartialPower_AdvancesProportionally()
        {
            var system = new WaterTreatmentSystem();
            system.AddWater(WaterType.Raw, 50f);
            system.AddCharcoal(100f);
            system.StartTreatment(TreatmentMode.CharcoalFiltration, 10f);

            system.TickDay(1, 0.5f);
            Assert.True(system.IsProcessing);
            Assert.Equal(5f, system.State.processingProgress, 2); // half the 10-unit target
        }

        [Fact]
        public void Water_DefaultTick_Powered_LegacyCallersUnaffected()
        {
            var system = new WaterTreatmentSystem();
            system.AddWater(WaterType.Raw, 50f);
            system.AddCharcoal(100f);
            system.StartTreatment(TreatmentMode.CharcoalFiltration, 10f);
            system.TickDay(1); // legacy single-arg call: full-power day completes the batch
            Assert.False(system.IsProcessing);
            Assert.True(system.CleanWater > 0f, "batch should yield clean water at full power");
        }

        [Fact]
        public void Water_Unpowered_StartBlocked_WithReasonCodes()
        {
            var system = new WaterTreatmentSystem();
            system.AddWater(WaterType.Raw, 50f);
            system.AddCharcoal(100f);
            system.TickDay(1, 0f); // plant learns it has no power today

            var result = system.StartTreatment(TreatmentMode.CharcoalFiltration, 10f);
            Assert.False(result.IsSuccess);
            Assert.Equal("power_unavailable", result.FailureCode);
            Assert.Equal("watertreat.no_power", result.MessageKey);
        }

        [Fact]
        public void Water_Powered_StartSucceeds()
        {
            var system = new WaterTreatmentSystem();
            system.AddWater(WaterType.Raw, 50f);
            system.AddCharcoal(100f);
            system.TickDay(1, 1f);
            Assert.True(system.StartTreatment(TreatmentMode.CharcoalFiltration, 10f).IsSuccess);
        }

        // ── Quarantine ventilation power ────────────────────────────────

        /// <summary>Fixture mirroring DiseaseQuarantineCoordinatorTests:
        /// failing consumption forces the 0.10 baseline quality so the
        /// containment bonus (0.3) is the only variable between runs.</summary>
        private static (DiseaseQuarantineCoordinator Coord, DiseaseSystem Disease, List<(int Day, int Iso, float Avg)> Burden)
            MakeCoordinator(Func<bool>? powerCheck)
        {
            var beds = new List<MedicalBed>
            {
                new MedicalBed("bed_iso_1", "Isolation 1", MedicalBedCategory.Isolation, isolation: true)
            };
            var ward = new MedicalWardSystem(new MedicalWardState(), beds, new List<MedicalProcedureDef>());
            var disease = new DiseaseSystem(new DiseaseSystemState(), new SeededRng(4242));
            disease.BindCatalog(DiseaseCatalogLoader.Load(
                System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data"),
                new FileSystemIO(), new SystemTextJsonSerializer()));

            var burden = new List<(int, int, float)>();
            var coord = new DiseaseQuarantineCoordinator(
                ward, disease,
                tryConsumeItem: (item, qty) => false,   // empty stores → 0.10 baseline
                containmentProvider: () => new ContainmentCapability { EfficacyBonus = 0.3f },
                isolationPowerCheck: powerCheck);
            coord.OnDailyBurdenProcessed += (day, iso, avg) => burden.Add((day, iso, avg));
            return (coord, disease, burden);
        }

        [Fact]
        public void Quarantine_PowerOn_ContainmentBonusApplies()
        {
            var (coord, disease, burden) = MakeCoordinator(powerCheck: () => true);
            disease.Infect("index_pt", DiseaseIds.ZoonoticFlu, 1);
            Assert.True(coord.ExecuteAssignIsolation("index_pt", 1).Success);

            coord.TickDaily(1);
            var (day, iso, avg) = Assert.Single(burden);
            Assert.Equal(1, iso);
            Assert.Equal(0.40f, avg, 2); // 0.10 baseline + 0.3 bonus, clamped
        }

        [Fact]
        public void Quarantine_PowerOff_ContainmentBonusWithheld()
        {
            var (coord, disease, burden) = MakeCoordinator(powerCheck: () => false);
            disease.Infect("index_pt", DiseaseIds.ZoonoticFlu, 1);
            Assert.True(coord.ExecuteAssignIsolation("index_pt", 1).Success);

            coord.TickDaily(1);
            var (day, iso, avg) = Assert.Single(burden);
            Assert.Equal(1, iso);
            Assert.Equal(0.10f, avg, 2); // baseline only — ventilation offline
        }

        [Fact]
        public void Quarantine_NullDelegate_LegacyBehavior_BonusApplies()
        {
            var (coord, disease, burden) = MakeCoordinator(powerCheck: null);
            disease.Infect("index_pt", DiseaseIds.ZoonoticFlu, 1);
            Assert.True(coord.ExecuteAssignIsolation("index_pt", 1).Success);

            coord.TickDaily(1);
            Assert.Equal(0.40f, burden[0].Avg, 2);
        }
    }
}
