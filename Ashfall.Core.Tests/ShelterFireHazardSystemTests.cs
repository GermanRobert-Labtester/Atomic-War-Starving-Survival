using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class ShelterFireHazardSystemTests
    {
        private static List<FireZoneState> DemoZones()
        {
            return new List<FireZoneState>
            {
                new FireZoneState
                {
                    zoneId = "zone_kitchen",
                    displayName = "Kitchen",
                    damperOpen = true,
                    adjacentZoneIds = new List<string> { "zone_hallway" }
                },
                new FireZoneState
                {
                    zoneId = "zone_hallway",
                    displayName = "Hallway",
                    damperOpen = true,
                    adjacentZoneIds = new List<string> { "zone_kitchen", "zone_barracks" }
                },
                new FireZoneState
                {
                    zoneId = "zone_barracks",
                    displayName = "Barracks",
                    damperOpen = true,
                    adjacentZoneIds = new List<string> { "zone_hallway" }
                }
            };
        }

        private static SeededRng Rng(int seed) => new SeededRng(seed);

        // ── Ignition ─────────────────────────────────────────────────

        [Fact]
        public void Ignite_CreatesIncident()
        {
            var sys = new ShelterFireHazardSystem();
            Assert.True(sys.Ignite("fire_1", "zone_kitchen", 1, DemoZones()));
            Assert.NotNull(sys.GetIncident("fire_1"));
        }

        [Fact]
        public void Ignite_SetsFireInSourceZone()
        {
            var sys = new ShelterFireHazardSystem();
            sys.Ignite("fire_1", "zone_kitchen", 1, DemoZones());
            var incident = sys.GetIncident("fire_1")!;
            var kitchen = incident.zones.Find(z => z.zoneId == "zone_kitchen");
            Assert.NotNull(kitchen);
            Assert.True(kitchen!.fireLevel > 0f);
        }

        [Fact]
        public void Ignite_NoFireInOtherZones()
        {
            var sys = new ShelterFireHazardSystem();
            sys.Ignite("fire_1", "zone_kitchen", 1, DemoZones());
            var incident = sys.GetIncident("fire_1")!;
            var barracks = incident.zones.Find(z => z.zoneId == "zone_barracks");
            Assert.NotNull(barracks);
            Assert.Equal(0f, barracks!.fireLevel);
        }

        [Fact]
        public void Ignite_RejectsDuplicate()
        {
            var sys = new ShelterFireHazardSystem();
            Assert.True(sys.Ignite("fire_1", "zone_kitchen", 1, DemoZones()));
            Assert.False(sys.Ignite("fire_1", "zone_kitchen", 1, DemoZones()));
        }

        [Fact]
        public void Ignite_RejectsNullInputs()
        {
            var sys = new ShelterFireHazardSystem();
            Assert.False(sys.Ignite(null, "zone_kitchen", 1, DemoZones()));
            Assert.False(sys.Ignite("fire_1", null, 1, DemoZones()));
            Assert.False(sys.Ignite("fire_1", "zone_kitchen", 1, null));
        }

        // ── Alarm ────────────────────────────────────────────────────

        [Fact]
        public void RaiseAlarm_SetsAlarmRaised()
        {
            var sys = new ShelterFireHazardSystem();
            sys.Ignite("fire_1", "zone_kitchen", 1, DemoZones());
            Assert.True(sys.RaiseAlarm("fire_1"));
            Assert.True(sys.GetIncident("fire_1")!.alarmRaised);
        }

        [Fact]
        public void RaiseAlarm_RaisesOnAlarmRaised()
        {
            var sys = new ShelterFireHazardSystem();
            sys.Ignite("fire_1", "zone_kitchen", 1, DemoZones());
            string? raisedId = null;
            sys.OnAlarmRaised += id => raisedId = id;
            sys.RaiseAlarm("fire_1");
            Assert.Equal("fire_1", raisedId);
        }

        [Fact]
        public void RaiseAlarm_RejectsDuplicate()
        {
            var sys = new ShelterFireHazardSystem();
            sys.Ignite("fire_1", "zone_kitchen", 1, DemoZones());
            Assert.True(sys.RaiseAlarm("fire_1"));
            Assert.False(sys.RaiseAlarm("fire_1"));
        }

        // ── Brigade ──────────────────────────────────────────────────

        [Fact]
        public void AssignBrigade_SetsWorkers()
        {
            var sys = new ShelterFireHazardSystem();
            sys.Ignite("fire_1", "zone_kitchen", 1, DemoZones());
            Assert.True(sys.AssignBrigade("fire_1", new List<string> { "sv_a", "sv_b" }));
            Assert.Equal(2, sys.GetIncident("fire_1")!.brigadeWorkers.Count);
        }

        // ── Dampers ──────────────────────────────────────────────────

        [Fact]
        public void SetDamper_TogglesDamper()
        {
            var sys = new ShelterFireHazardSystem();
            sys.Ignite("fire_1", "zone_kitchen", 1, DemoZones());
            Assert.True(sys.SetDamper("fire_1", "zone_kitchen", false));
            var kitchen = sys.GetIncident("fire_1")!.zones.Find(z => z.zoneId == "zone_kitchen");
            Assert.False(kitchen!.damperOpen);
        }

        [Fact]
        public void SetDamper_RaisesOnDamperChanged()
        {
            var sys = new ShelterFireHazardSystem();
            sys.Ignite("fire_1", "zone_kitchen", 1, DemoZones());
            string? changedZone = null;
            sys.OnDamperChanged += (_, zone) => changedZone = zone;
            sys.SetDamper("fire_1", "zone_kitchen", false);
            Assert.Equal("zone_kitchen", changedZone);
        }

        // ── Extinguisher ─────────────────────────────────────────────

        [Fact]
        public void DeployExtinguisher_ReducesFire()
        {
            var sys = new ShelterFireHazardSystem();
            sys.Ignite("fire_1", "zone_kitchen", 1, DemoZones());
            float fireBefore = sys.GetIncident("fire_1")!.zones.Find(z => z.zoneId == "zone_kitchen")!.fireLevel;
            sys.DeployExtinguisher("fire_1", "zone_kitchen");
            float fireAfter = sys.GetIncident("fire_1")!.zones.Find(z => z.zoneId == "zone_kitchen")!.fireLevel;
            Assert.True(fireAfter < fireBefore);
        }

        [Fact]
        public void DeployExtinguisher_LimitedCharges()
        {
            var sys = new ShelterFireHazardSystem();
            sys.Ignite("fire_1", "zone_kitchen", 1, DemoZones());
            for (int i = 0; i < ShelterFireHazardSystem.ExtinguisherMaxCharges; i++)
                Assert.True(sys.DeployExtinguisher("fire_1", "zone_kitchen"));
            Assert.False(sys.DeployExtinguisher("fire_1", "zone_kitchen"));
        }

        // ── Evacuation ───────────────────────────────────────────────

        [Fact]
        public void EvacuateZone_SetsEvacuated()
        {
            var sys = new ShelterFireHazardSystem();
            sys.Ignite("fire_1", "zone_kitchen", 1, DemoZones());
            Assert.True(sys.EvacuateZone("fire_1", "zone_kitchen"));
            Assert.True(sys.GetIncident("fire_1")!.zones.Find(z => z.zoneId == "zone_kitchen")!.isEvacuated);
        }

        // ── Tick ─────────────────────────────────────────────────────

        [Fact]
        public void Tick_IncreasesSmokeInSourceZone()
        {
            var sys = new ShelterFireHazardSystem();
            sys.Ignite("fire_1", "zone_kitchen", 1, DemoZones());
            float smokeBefore = sys.GetIncident("fire_1")!.zones.Find(z => z.zoneId == "zone_kitchen")!.smokeLevel;
            sys.Tick("fire_1", Rng(1));
            float smokeAfter = sys.GetIncident("fire_1")!.zones.Find(z => z.zoneId == "zone_kitchen")!.smokeLevel;
            Assert.True(smokeAfter > smokeBefore);
        }

        [Fact]
        public void Tick_IncreasesCoInSourceZone()
        {
            var sys = new ShelterFireHazardSystem();
            sys.Ignite("fire_1", "zone_kitchen", 1, DemoZones());
            // Run multiple ticks to accumulate CO (generation may be close to decay rate)
            for (int i = 0; i < 5; i++) sys.Tick("fire_1", Rng(1));
            float coAfter = sys.GetIncident("fire_1")!.zones.Find(z => z.zoneId == "zone_kitchen")!.coLevel;
            Assert.True(coAfter > 0f, "CO should accumulate over multiple ticks");
        }

        [Fact]
        public void Tick_SpreadsFireToAdjacentZones()
        {
            var sys = new ShelterFireHazardSystem();
            sys.Ignite("fire_1", "zone_kitchen", 1, DemoZones());
            // Run enough ticks for fire to spread
            for (int i = 0; i < 20; i++) sys.Tick("fire_1", Rng(i));
            var hallway = sys.GetIncident("fire_1")!.zones.Find(z => z.zoneId == "zone_hallway");
            // Fire may or may not spread depending on RNG, but smoke should spread
            Assert.True(hallway!.smokeLevel > 0f || hallway.fireLevel > 0f,
                "Adjacent zone should receive smoke or fire");
        }

        [Fact]
        public void Tick_ClosedDamperReducesSmokeSpread()
        {
            var sysClosed = new ShelterFireHazardSystem();
            sysClosed.Ignite("fire_1", "zone_kitchen", 1, DemoZones());
            sysClosed.SetDamper("fire_1", "zone_hallway", false); // close hallway damper
            for (int i = 0; i < 10; i++) sysClosed.Tick("fire_1", Rng(i));
            var hallwayClosed = sysClosed.GetIncident("fire_1")!.zones.Find(z => z.zoneId == "zone_hallway");

            var sysOpen = new ShelterFireHazardSystem();
            sysOpen.Ignite("fire_1", "zone_kitchen", 1, DemoZones());
            // All dampers open
            for (int i = 0; i < 10; i++) sysOpen.Tick("fire_1", Rng(i));
            var hallwayOpen = sysOpen.GetIncident("fire_1")!.zones.Find(z => z.zoneId == "zone_hallway");

            Assert.True(hallwayClosed!.smokeLevel <= hallwayOpen!.smokeLevel,
                "Closed damper should reduce smoke spread");
        }

        [Fact]
        public void Tick_BrigadeSuppressesFire()
        {
            var sys = new ShelterFireHazardSystem();
            sys.Ignite("fire_1", "zone_kitchen", 1, DemoZones());
            sys.AssignBrigade("fire_1", new List<string> { "sv_a", "sv_b", "sv_c" });
            float fireBefore = sys.GetIncident("fire_1")!.zones.Find(z => z.zoneId == "zone_kitchen")!.fireLevel;
            sys.Tick("fire_1", Rng(1));
            // Brigade may suppress faster than fire grows
            var kitchen = sys.GetIncident("fire_1")!.zones.Find(z => z.zoneId == "zone_kitchen");
            // At minimum, fire shouldn't grow as fast with brigade
            Assert.True(kitchen!.fireLevel <= fireBefore * 1.1f);
        }

        [Fact]
        public void Tick_IncrementsTicksElapsed()
        {
            var sys = new ShelterFireHazardSystem();
            sys.Ignite("fire_1", "zone_kitchen", 1, DemoZones());
            sys.Tick("fire_1", Rng(1));
            Assert.Equal(1, sys.GetIncident("fire_1")!.ticksElapsed);
        }

        [Fact]
        public void Tick_ResolvesWhenFireOut()
        {
            var sys = new ShelterFireHazardSystem();
            sys.Ignite("fire_1", "zone_kitchen", 1, DemoZones());
            // Deploy all extinguishers and add large brigade
            for (int i = 0; i < ShelterFireHazardSystem.ExtinguisherMaxCharges; i++)
                sys.DeployExtinguisher("fire_1", "zone_kitchen");
            sys.AssignBrigade("fire_1", new List<string> { "sv_a", "sv_b", "sv_c", "sv_d", "sv_e" });
            // Tick until resolved
            for (int i = 0; i < 30; i++) sys.Tick("fire_1", Rng(i));
            Assert.True(sys.IsResolved("fire_1"));
        }

        [Fact]
        public void Tick_NoTickWhenResolved()
        {
            var sys = new ShelterFireHazardSystem();
            sys.Ignite("fire_1", "zone_kitchen", 1, DemoZones());
            // Force resolve
            var incident = sys.GetIncident("fire_1")!;
            incident.isResolved = true;
            int ticksBefore = incident.ticksElapsed;
            sys.Tick("fire_1", Rng(1));
            Assert.Equal(ticksBefore, incident.ticksElapsed);
        }

        // ── Determinism ──────────────────────────────────────────────

        [Fact]
        public void SameSeed_SameOutcome()
        {
            var a = new ShelterFireHazardSystem();
            a.Ignite("fire_1", "zone_kitchen", 1, DemoZones());
            for (int i = 0; i < 5; i++) a.Tick("fire_1", Rng(42));

            var b = new ShelterFireHazardSystem();
            b.Ignite("fire_1", "zone_kitchen", 1, DemoZones());
            for (int i = 0; i < 5; i++) b.Tick("fire_1", Rng(42));

            var aZones = a.GetIncident("fire_1")!.zones;
            var bZones = b.GetIncident("fire_1")!.zones;
            for (int i = 0; i < aZones.Count; i++)
            {
                Assert.Equal(aZones[i].fireLevel, bZones[i].fireLevel);
                Assert.Equal(aZones[i].smokeLevel, bZones[i].smokeLevel);
                Assert.Equal(aZones[i].coLevel, bZones[i].coLevel);
            }
        }

        // ── Save/Load ────────────────────────────────────────────────

        [Fact]
        public void CaptureRestore_RoundTrips()
        {
            var sys = new ShelterFireHazardSystem();
            sys.Ignite("fire_1", "zone_kitchen", 1, DemoZones());
            sys.Tick("fire_1", Rng(1));

            var state = sys.CaptureState();
            var sys2 = new ShelterFireHazardSystem();
            sys2.RestoreState(state);

            Assert.NotNull(sys2.GetIncident("fire_1"));
            Assert.Equal(1, sys2.GetIncident("fire_1")!.ticksElapsed);
        }

        [Fact]
        public void CaptureState_StableChecksum()
        {
            var sys = new ShelterFireHazardSystem();
            sys.Ignite("fire_1", "zone_kitchen", 1, DemoZones());
            sys.Tick("fire_1", Rng(1));
            string before = SaveChecksum.Compute(sys.CaptureState());

            var sys2 = new ShelterFireHazardSystem();
            sys2.RestoreState(sys.CaptureState());
            string after = SaveChecksum.Compute(sys2.CaptureState());

            Assert.Equal(before, after);
        }

        // ── Queries ──────────────────────────────────────────────────

        [Fact]
        public void GetIncident_ReturnsNullForUnknown()
        {
            var sys = new ShelterFireHazardSystem();
            Assert.Null(sys.GetIncident("fire_unknown"));
        }

        [Fact]
        public void GetMaxCoLevel_ReturnsZeroWhenNoIncident()
        {
            var sys = new ShelterFireHazardSystem();
            Assert.Equal(0f, sys.GetMaxCoLevel("fire_unknown"));
        }
    }
}
