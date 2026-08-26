using System;
using Ashfall.Core;
using Ashfall.Core.Foundry;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class SaltMineExtractionSystemTests
    {
        private static SaltMineVeinState DemoVein(string id = "vein_01", float ore = 1000f, int maxWorkers = 4)
        {
            return new SaltMineVeinState
            {
                veinId = id,
                displayName = "Demo Vein",
                isUnlocked = false,
                remainingOre = ore,
                extractionRate = 10f,
                maxWorkers = maxWorkers,
                assignedWorkers = 0,
                drillCondition = 1.0f,
                pumpPressure = 1.0f
            };
        }

        private static SeededRng Rng(int seed) => new SeededRng(seed);

        // ── Vein management ──────────────────────────────────────────

        [Fact]
        public void RegisterVein_CreatesVein()
        {
            var sys = new SaltMineExtractionSystem();
            Assert.True(sys.RegisterVein(DemoVein()));
            Assert.NotNull(sys.GetVein("vein_01"));
        }

        [Fact]
        public void RegisterVein_RejectsDuplicate()
        {
            var sys = new SaltMineExtractionSystem();
            Assert.True(sys.RegisterVein(DemoVein()));
            Assert.False(sys.RegisterVein(DemoVein()));
        }

        [Fact]
        public void UnlockVein_SetsUnlocked()
        {
            var sys = new SaltMineExtractionSystem();
            sys.RegisterVein(DemoVein());
            Assert.True(sys.UnlockVein("vein_01"));
            Assert.True(sys.GetVein("vein_01")!.isUnlocked);
        }

        [Fact]
        public void UnlockVein_RaisesOnMineOpened()
        {
            var sys = new SaltMineExtractionSystem();
            sys.RegisterVein(DemoVein());
            string? openedId = null;
            sys.OnMineOpened += id => openedId = id;
            sys.UnlockVein("vein_01");
            Assert.Equal("vein_01", openedId);
        }

        [Fact]
        public void AssignWorkers_ClampsToMax()
        {
            var sys = new SaltMineExtractionSystem();
            sys.RegisterVein(DemoVein(maxWorkers: 3));
            sys.UnlockVein("vein_01");
            Assert.True(sys.AssignWorkers("vein_01", 10));
            Assert.Equal(3, sys.GetVein("vein_01")!.assignedWorkers);
        }

        [Fact]
        public void AssignWorkers_RejectsWhenShutdown()
        {
            var sys = new SaltMineExtractionSystem();
            sys.RegisterVein(DemoVein());
            sys.UnlockVein("vein_01");
            sys.GetVein("vein_01")!.isShutdown = true;
            Assert.False(sys.AssignWorkers("vein_01", 2));
        }

        // ── Daily tick ───────────────────────────────────────────────

        [Fact]
        public void TickDaily_ProducesSalt()
        {
            var sys = new SaltMineExtractionSystem();
            sys.RegisterVein(DemoVein());
            sys.UnlockVein("vein_01");
            sys.AssignWorkers("vein_01", 2);
            sys.TickDaily(1, Rng(1));
            Assert.True(sys.State.saltStorage > 0f);
        }

        [Fact]
        public void TickDaily_ProducesBrine()
        {
            var sys = new SaltMineExtractionSystem();
            sys.RegisterVein(DemoVein());
            sys.UnlockVein("vein_01");
            sys.AssignWorkers("vein_01", 2);
            sys.TickDaily(1, Rng(1));
            Assert.True(sys.State.brineStorage > 0f);
        }

        [Fact]
        public void TickDaily_ProducesSulfur()
        {
            var sys = new SaltMineExtractionSystem();
            sys.RegisterVein(DemoVein());
            sys.UnlockVein("vein_01");
            sys.AssignWorkers("vein_01", 2);
            sys.TickDaily(1, Rng(1));
            Assert.True(sys.State.sulfurStorage > 0f);
        }

        [Fact]
        public void TickDaily_DrillWears()
        {
            var sys = new SaltMineExtractionSystem();
            sys.RegisterVein(DemoVein());
            sys.UnlockVein("vein_01");
            sys.AssignWorkers("vein_01", 2);
            float drillBefore = sys.GetVein("vein_01")!.drillCondition;
            sys.TickDaily(1, Rng(1));
            Assert.True(sys.GetVein("vein_01")!.drillCondition < drillBefore);
        }

        [Fact]
        public void TickDaily_PumpWears()
        {
            var sys = new SaltMineExtractionSystem();
            sys.RegisterVein(DemoVein());
            sys.UnlockVein("vein_01");
            sys.AssignWorkers("vein_01", 2);
            float pumpBefore = sys.GetVein("vein_01")!.pumpPressure;
            sys.TickDaily(1, Rng(1));
            Assert.True(sys.GetVein("vein_01")!.pumpPressure < pumpBefore);
        }

        [Fact]
        public void TickDaily_ContaminationIncreases()
        {
            var sys = new SaltMineExtractionSystem();
            sys.RegisterVein(DemoVein());
            sys.UnlockVein("vein_01");
            sys.AssignWorkers("vein_01", 4);
            sys.TickDaily(1, Rng(1));
            Assert.True(sys.GetVein("vein_01")!.contamination > 0f);
        }

        [Fact]
        public void TickDaily_NoProductionWhenNoWorkers()
        {
            var sys = new SaltMineExtractionSystem();
            sys.RegisterVein(DemoVein());
            sys.UnlockVein("vein_01");
            // No workers assigned
            sys.TickDaily(1, Rng(1));
            Assert.Equal(0f, sys.State.saltStorage);
        }

        [Fact]
        public void TickDaily_NoProductionWhenPoweredOff()
        {
            var sys = new SaltMineExtractionSystem();
            sys.RegisterVein(DemoVein());
            sys.UnlockVein("vein_01");
            sys.AssignWorkers("vein_01", 2);
            sys.SetPower(false);
            sys.TickDaily(1, Rng(1));
            Assert.Equal(0f, sys.State.saltStorage);
        }

        [Fact]
        public void TickDaily_DrillFailureShutsDownMine()
        {
            var sys = new SaltMineExtractionSystem();
            sys.RegisterVein(DemoVein());
            sys.UnlockVein("vein_01");
            sys.AssignWorkers("vein_01", 2);
            sys.GetVein("vein_01")!.drillCondition = 0.01f; // about to fail
            bool drillFailed = false;
            sys.OnDrillFailure += _ => drillFailed = true;
            sys.TickDaily(1, Rng(1));
            Assert.True(drillFailed);
            Assert.True(sys.GetVein("vein_01")!.isShutdown);
        }

        [Fact]
        public void TickDaily_PumpFailureShutsDownMine()
        {
            var sys = new SaltMineExtractionSystem();
            sys.RegisterVein(DemoVein());
            sys.UnlockVein("vein_01");
            sys.AssignWorkers("vein_01", 2);
            sys.GetVein("vein_01")!.pumpPressure = 0.19f; // below threshold
            bool pumpFailed = false;
            sys.OnPumpFailure += _ => pumpFailed = true;
            sys.TickDaily(1, Rng(1));
            Assert.True(pumpFailed);
            Assert.True(sys.GetVein("vein_01")!.isShutdown);
        }

        [Fact]
        public void TickDaily_ReducesOre()
        {
            var sys = new SaltMineExtractionSystem();
            sys.RegisterVein(DemoVein(ore: 1000f));
            sys.UnlockVein("vein_01");
            sys.AssignWorkers("vein_01", 2);
            float oreBefore = sys.GetVein("vein_01")!.remainingOre;
            sys.TickDaily(1, Rng(1));
            Assert.True(sys.GetVein("vein_01")!.remainingOre < oreBefore);
        }

        // ── Treaty delivery ──────────────────────────────────────────

        [Fact]
        public void DeliverToTreaty_AcceptsWhenStockSufficient()
        {
            var sys = new SaltMineExtractionSystem();
            sys.RegisterVein(DemoVein(ore: 10000f));
            sys.UnlockVein("vein_01");
            sys.AssignWorkers("vein_01", 4);
            // Produce enough brine
            for (int i = 0; i < 20; i++) sys.TickDaily(i, Rng(i));
            Assert.True(sys.State.brineStorage >= SaltMineExtractionSystem.TreatyBrineQuotaBarrels);
            var record = sys.DeliverToTreaty(20);
            Assert.NotNull(record);
            Assert.True(record!.accepted);
        }

        [Fact]
        public void DeliverToTreaty_MissesWhenStockInsufficient()
        {
            var sys = new SaltMineExtractionSystem();
            // No production
            var record = sys.DeliverToTreaty(1);
            Assert.NotNull(record);
            Assert.False(record!.accepted);
        }

        [Fact]
        public void DeliverToTreaty_ConsumesStock()
        {
            var sys = new SaltMineExtractionSystem();
            sys.RegisterVein(DemoVein(ore: 10000f));
            sys.UnlockVein("vein_01");
            sys.AssignWorkers("vein_01", 4);
            for (int i = 0; i < 20; i++) sys.TickDaily(i, Rng(i));
            float brineBefore = sys.State.brineStorage;
            sys.DeliverToTreaty(20);
            Assert.True(sys.State.brineStorage < brineBefore);
        }

        [Fact]
        public void DeliverToTreaty_RaisesOnTreatyDeliveryAccepted()
        {
            var sys = new SaltMineExtractionSystem();
            sys.RegisterVein(DemoVein(ore: 10000f));
            sys.UnlockVein("vein_01");
            sys.AssignWorkers("vein_01", 4);
            for (int i = 0; i < 20; i++) sys.TickDaily(i, Rng(i));
            bool accepted = false;
            sys.OnTreatyDeliveryAccepted += _ => accepted = true;
            sys.DeliverToTreaty(20);
            Assert.True(accepted);
        }

        [Fact]
        public void IsTreatyFulfilled_ReturnsTrueAfterDelivery()
        {
            var sys = new SaltMineExtractionSystem();
            sys.RegisterVein(DemoVein(ore: 10000f));
            sys.UnlockVein("vein_01");
            sys.AssignWorkers("vein_01", 4);
            for (int i = 0; i < 20; i++) sys.TickDaily(i, Rng(i));
            Assert.False(sys.IsTreatyFulfilled(20));
            sys.DeliverToTreaty(20);
            Assert.True(sys.IsTreatyFulfilled(20));
        }

        // ── Maintenance ──────────────────────────────────────────────

        [Fact]
        public void ReplaceDrill_RestoresCondition()
        {
            var sys = new SaltMineExtractionSystem();
            sys.RegisterVein(DemoVein());
            sys.UnlockVein("vein_01");
            sys.GetVein("vein_01")!.drillCondition = 0.1f;
            sys.ReplaceDrill("vein_01");
            Assert.Equal(1.0f, sys.GetVein("vein_01")!.drillCondition);
        }

        [Fact]
        public void RepairPump_RestoresPressure()
        {
            var sys = new SaltMineExtractionSystem();
            sys.RegisterVein(DemoVein());
            sys.UnlockVein("vein_01");
            sys.GetVein("vein_01")!.pumpPressure = 0.1f;
            sys.RepairPump("vein_01");
            Assert.Equal(1.0f, sys.GetVein("vein_01")!.pumpPressure);
        }

        [Fact]
        public void ReplaceDrill_ReopensShutdownMine()
        {
            var sys = new SaltMineExtractionSystem();
            sys.RegisterVein(DemoVein());
            sys.UnlockVein("vein_01");
            sys.GetVein("vein_01")!.isShutdown = true;
            sys.GetVein("vein_01")!.drillCondition = 0f;
            sys.GetVein("vein_01")!.pumpPressure = 0.5f;
            bool reopened = false;
            sys.OnMineOpened += _ => reopened = true;
            sys.ReplaceDrill("vein_01");
            Assert.True(reopened);
            Assert.False(sys.GetVein("vein_01")!.isShutdown);
        }

        // ── Determinism ──────────────────────────────────────────────

        [Fact]
        public void SameSeed_SameProduction()
        {
            var a = new SaltMineExtractionSystem();
            a.RegisterVein(DemoVein());
            a.UnlockVein("vein_01");
            a.AssignWorkers("vein_01", 2);
            a.TickDaily(1, Rng(42));

            var b = new SaltMineExtractionSystem();
            b.RegisterVein(DemoVein());
            b.UnlockVein("vein_01");
            b.AssignWorkers("vein_01", 2);
            b.TickDaily(1, Rng(42));

            Assert.Equal(a.State.saltStorage, b.State.saltStorage);
            Assert.Equal(a.State.brineStorage, b.State.brineStorage);
        }

        // ── Save/Load ────────────────────────────────────────────────

        [Fact]
        public void CaptureRestore_RoundTrips()
        {
            var sys = new SaltMineExtractionSystem();
            sys.RegisterVein(DemoVein());
            sys.UnlockVein("vein_01");
            sys.AssignWorkers("vein_01", 2);
            sys.TickDaily(1, Rng(1));

            var state = sys.CaptureState();
            var sys2 = new SaltMineExtractionSystem();
            sys2.RestoreState(state);

            Assert.Equal(sys.State.saltStorage, sys2.State.saltStorage);
            Assert.Equal(sys.State.brineStorage, sys2.State.brineStorage);
            Assert.Single(sys2.State.veins);
        }

        [Fact]
        public void CaptureState_OrdinalOrdered()
        {
            var sys = new SaltMineExtractionSystem();
            sys.RegisterVein(DemoVein("vein_z"));
            sys.RegisterVein(DemoVein("vein_a"));
            var state = sys.CaptureState();
            Assert.Equal("vein_a", state.veins[0].veinId);
            Assert.Equal("vein_z", state.veins[1].veinId);
        }

        [Fact]
        public void Checksum_Stable()
        {
            var sys = new SaltMineExtractionSystem();
            sys.RegisterVein(DemoVein());
            sys.UnlockVein("vein_01");
            sys.AssignWorkers("vein_01", 2);
            sys.TickDaily(1, Rng(1));
            string before = SaveChecksum.Compute(sys.CaptureState());

            var sys2 = new SaltMineExtractionSystem();
            sys2.RestoreState(sys.CaptureState());
            string after = SaveChecksum.Compute(sys2.CaptureState());

            Assert.Equal(before, after);
        }

        [Fact]
        public void Deliveries_SurviveSaveLoad()
        {
            var sys = new SaltMineExtractionSystem();
            sys.RegisterVein(DemoVein(ore: 10000f));
            sys.UnlockVein("vein_01");
            sys.AssignWorkers("vein_01", 4);
            for (int i = 0; i < 20; i++) sys.TickDaily(i, Rng(i));
            sys.DeliverToTreaty(20);

            var state = sys.CaptureState();
            var sys2 = new SaltMineExtractionSystem();
            sys2.RestoreState(state);

            Assert.Single(sys2.State.deliveries);
            Assert.True(sys2.State.deliveries[0].accepted);
        }
    }
}
