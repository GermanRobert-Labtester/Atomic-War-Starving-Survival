using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Xunit;

namespace Ashfall.Core.Tests
{
        public class ExpeditionSystemTests : System.IDisposable
        {
            public void Dispose() => ExpeditionDefinitionRegistry.Clear();

            private static ExpeditionDefinition DemoDef(string id = "loc_demo_site", int distance = 4, int danger = 2)
            {
                var def = new ExpeditionDefinition
                {
                    id = id,
                    displayName = "Demo Site",
                    distanceTicks = distance,
                    dangerLevel = danger,
                    encounterChancePerTick = 0.10f,
                    baseStaminaDrainPerHour = 2.0f,
                    lootCategories = new List<string> { "scrap_metal", "clean_water", "bandages" }
                };
                ExpeditionDefinitionRegistry.Register(def);
                return def;
            }

        private static SeededRng Rng(int seed) => new SeededRng(seed);

        [Fact]
        public void Start_GuardsInvalidInputs()
        {
            var sys = new ExpeditionSystem();
            Assert.False(sys.Start(null, "sv_mae", 1));
            Assert.False(sys.Start(DemoDef(), "", 1));
            Assert.False(sys.Start(DemoDef(), null, 1));
            Assert.Equal(0, sys.ActiveCount);
        }

        [Fact]
        public void Start_OneExpeditionPerSurvivor()
        {
            var sys = new ExpeditionSystem();
            Assert.True(sys.Start(DemoDef(), "sv_mae", 1));
            Assert.False(sys.Start(DemoDef(), "sv_mae", 1));
            Assert.Equal(1, sys.ActiveCount);
        }

        [Fact]
        public void Outbound_ArrivesAfterDistanceTicks()
        {
            var sys = new ExpeditionSystem();
            sys.Start(DemoDef(distance: 3), "sv_mae", 1);
            ExpeditionState Live() => new List<ExpeditionState>(sys.Active.Values)[0];
            sys.TickHours(1f, Rng(7));
            Assert.Equal((int)ExpeditionPhase.Outbound, Live().phase);
            sys.TickHours(1f, Rng(7));
            Assert.Equal((int)ExpeditionPhase.Outbound, Live().phase);
            sys.TickHours(1f, Rng(7));
            Assert.Equal((int)ExpeditionPhase.Looting, Live().phase);
        }

        [Fact]
        public void SpeedStance_TravelsOneAndAHalfTimesFaster()
        {
            var stealth = new ExpeditionSystem();
            stealth.Start(DemoDef(distance: 3), "sv_a", 1, ExpeditionStance.Stealth);
            var speed = new ExpeditionSystem();
            speed.Start(DemoDef(distance: 3), "sv_b", 1, ExpeditionStance.Speed);

            stealth.TickHours(1f, Rng(1));
            speed.TickHours(1f, Rng(1));
            stealth.TickHours(1f, Rng(1));
            speed.TickHours(1f, Rng(1));

            var st = new List<ExpeditionState>(stealth.Active.Values)[0];
            var sp = new List<ExpeditionState>(speed.Active.Values)[0];
            Assert.Equal(2, st.travelTicksCompleted);
            // Unity parity: Mathf.RoundToInt(1.5) == 2 ticks per speed tick.
            Assert.Equal(4, sp.travelTicksCompleted);
        }

        [Fact]
        public void Looting_AutoRetreatsAfterThreeTicksUnlessPushingLuck()
        {
            var sys = new ExpeditionSystem();
            sys.Start(DemoDef(distance: 1, danger: 0), "sv_mae", 1);
            sys.TickHours(1f, Rng(3));  // arrive
            sys.TickHours(1f, Rng(3));  // loot 1
            sys.TickHours(1f, Rng(3));  // loot 2
            sys.TickHours(1f, Rng(3));  // loot 3 -> auto-retreat
            var exp = new List<ExpeditionState>(sys.Active.Values)[0];
            Assert.Equal((int)ExpeditionPhase.Inbound, exp.phase);
            Assert.Equal(3, exp.lootingTicksCompleted);
        }

        [Fact]
        public void PushLuck_ExtendsLooting()
        {
            var sys = new ExpeditionSystem();
            sys.Start(DemoDef(distance: 1, danger: 0), "sv_mae", 1);
            sys.TickHours(1f, Rng(3));
            Assert.True(sys.PushLuck("sv_mae"));
            for (int i = 0; i < 4; i++) sys.TickHours(1f, Rng(3));
            var exp = new List<ExpeditionState>(sys.Active.Values)[0];
            Assert.Equal((int)ExpeditionPhase.Looting, exp.phase);
            Assert.True(exp.isPushingLuck);
        }

        [Fact]
        public void Loot_IsCappedByCarryingCapacity()
        {
            var sys = new ExpeditionSystem();
            var def = DemoDef(distance: 1, danger: 3); // high loot chance
            sys.Start(def, "sv_mae", 1);
            sys.TickHours(1f, Rng(5));
            for (int i = 0; i < 30; i++)
            {
                sys.TickHours(1f, Rng(5));
                var exp = new List<ExpeditionState>(sys.Active.Values)[0];
                if (exp.phase != (int)ExpeditionPhase.Looting) break;
            }
            var done = new List<ExpeditionState>(sys.Active.Values)[0];
            Assert.True(done.currentWeightKg <= done.maxLootCapacityKg);
        }

        [Fact]
        public void Completion_ReturnsToShelterAndClearsActive()
        {
            var sys = new ExpeditionSystem();
            sys.Start(DemoDef(distance: 2, danger: 0), "sv_mae", 1);
            sys.TickHours(1f, Rng(1));
            sys.TickHours(1f, Rng(1));  // arrive
            sys.TickHours(1f, Rng(1));
            sys.TickHours(1f, Rng(1));
            sys.TickHours(1f, Rng(1));  // loot 3 -> retreat
            sys.TickHours(1f, Rng(1));  // inbound 1
            sys.TickHours(1f, Rng(1));  // inbound 2 -> completed
            Assert.Equal(0, sys.ActiveCount);
        }

        [Fact]
        public void StaminaExhaustion_FailsTheExpedition()
        {
            var sys = new ExpeditionSystem();
            var def = DemoDef(distance: 30, danger: 0);
            def.baseStaminaDrainPerHour = 120f; // drains in one tick
            sys.Start(def, "sv_mae", 1);
            string reason = null;
            sys.OnExpeditionFailed += (s, r) => reason = r;
            sys.TickHours(1f, Rng(1));
            Assert.NotNull(reason);
            Assert.Contains("exhaustion", reason);
            Assert.Equal(0, sys.ActiveCount);
        }

        [Fact]
        public void Determinism_SameSeedSameLoot()
        {
            var runA = new ExpeditionSystem();
            runA.Start(DemoDef(distance: 1, danger: 2), "sv_a", 1);
            for (int i = 0; i < 8; i++) runA.TickHours(1f, Rng(99));

            var runB = new ExpeditionSystem();
            runB.Start(DemoDef(distance: 1, danger: 2), "sv_b", 1);
            for (int i = 0; i < 8; i++) runB.TickHours(1f, Rng(99));

            var lootA = new List<string>();
            foreach (var kv in runA.Active) foreach (var l in kv.Value.loot) lootA.Add(l.itemId + ":" + l.quantity);
            var lootB = new List<string>();
            foreach (var kv in runB.Active) foreach (var l in kv.Value.loot) lootB.Add(l.itemId + ":" + l.quantity);
            Assert.Equal(string.Join(",", lootA), string.Join(",", lootB));
        }

        [Fact]
        public void Encounters_RollOnEveryLegIncludingTravel()
        {
            var sys = new ExpeditionSystem();
            var def = DemoDef(distance: 12, danger: 0);
            def.encounterChancePerTick = 0.99f; // guaranteed per tick
            sys.Start(def, "sv_mae", 1);
            sys.TickHours(1f, Rng(1)); // outbound
            var exp = new List<ExpeditionState>(sys.Active.Values)[0];
            Assert.True(exp.encounterCount >= 1, "encounter must fire during outbound travel");
            for (int i = 0; i < 5; i++) sys.TickHours(1f, Rng(1));
            exp = new List<ExpeditionState>(sys.Active.Values)[0];
            Assert.True(exp.encounterCount >= 5, "encounters keep rolling across legs");
        }

        [Fact]
        public void ExpeditionId_IsUniquePerSurvivorAndTarget()
        {
            var sys = new ExpeditionSystem();
            sys.Start(DemoDef(), "sv_a", 1);
            sys.Start(DemoDef("loc_b", 2, 0), "sv_b", 1);
            var ids = new System.Collections.Generic.HashSet<string>();
            foreach (var kv in sys.Active) ids.Add(kv.Value.expeditionId);
            Assert.Equal(2, ids.Count);
            foreach (var kv in sys.Active)
                Assert.False(string.IsNullOrEmpty(kv.Value.expeditionId));
        }

        [Fact]
        public void Retreat_RaisesStateChanged()
        {
            var sys = new ExpeditionSystem();
            sys.Start(DemoDef(distance: 1, danger: 0), "sv_mae", 1);
            sys.TickHours(1f, Rng(1)); // arrive -> looting
            int raised = 0;
            sys.OnStateChanged += _ => raised++;
            Assert.True(sys.Retreat("sv_mae"));
            Assert.True(raised >= 1);
        }

        [Fact]
        public void CaptureState_ReturnsSnapshotNotLiveState()
        {
            var sys = new ExpeditionSystem();
            sys.Start(DemoDef(), "sv_mae", 1);
            sys.TickHours(1f, Rng(2));
            var snapshot = sys.CaptureState();
            snapshot[0].loot.Add(new ExpeditionLootEntry { itemId = "injected", quantity = 99 });
            var live = new List<ExpeditionState>(sys.Active.Values)[0];
            Assert.DoesNotContain(live.loot, l => l.itemId == "injected");
        }

        [Fact]
        public void CaptureState_EmitsInOrdinalOrder()
        {
            var sys = new ExpeditionSystem();
            sys.Start(DemoDef(), "sv_zed", 1);
            sys.Start(DemoDef("loc_b", 1, 0), "sv_a", 1);
            var snapshot = sys.CaptureState();
            Assert.Equal("sv_a", snapshot[0].survivorId);
            Assert.Equal("sv_zed", snapshot[1].survivorId);
        }

        [Fact]
        public void SaveLoad_RoundTripsAllState()
        {
            var sys = new ExpeditionSystem();
            sys.Start(DemoDef(distance: 1, danger: 2), "sv_mae", 1);
            sys.TickHours(1f, Rng(5));
            sys.TickHours(1f, Rng(5));

            var restored = new ExpeditionSystem();
            restored.RestoreState(sys.CaptureState());

            Assert.Equal(1, restored.ActiveCount);
            var live = new List<ExpeditionState>(restored.Active.Values)[0];
            Assert.Equal("sv_mae", live.survivorId);
            Assert.Equal(live.lootingTicksCompleted, new List<ExpeditionState>(sys.Active.Values)[0].lootingTicksCompleted);
        }

        [Fact]
        public void SaveLoad_ChecksumStable()
        {
            var sys = new ExpeditionSystem();
            sys.Start(DemoDef(distance: 2, danger: 1), "sv_mae", 1);
            for (int i = 0; i < 6; i++) sys.TickHours(1f, Rng(11));
            string before = SaveChecksum.Compute(sys.CaptureState());

            var restored = new ExpeditionSystem();
            restored.RestoreState(sys.CaptureState());
            string after = SaveChecksum.Compute(restored.CaptureState());

            Assert.Equal(before, after);
        }
    }
}
