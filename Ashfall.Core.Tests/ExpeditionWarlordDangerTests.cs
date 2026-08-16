using System;
using System.Collections.Generic;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Warlords;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Warlord road-danger integration with the expedition system: hostile
    /// warlord territory must raise the encounter chance of real sorties,
    /// deterministically, without changing the seeded roll stream shape.
    /// </summary>
    public class ExpeditionWarlordDangerTests
    {
        private static ExpeditionDefinition Def(string locationId, float chance = 0.14f) => new ExpeditionDefinition
        {
            id = locationId,
            displayName = locationId,
            distanceTicks = 4,
            dangerLevel = 5,
            encounterChancePerTick = chance
        };

        private static int TickToCompletion(ExpeditionSystem engine, int seed)
        {
            var rng = new SeededRng(seed);
            for (int tick = 0; tick < 60 && engine.ActiveCount > 0; tick++)
                engine.TickHours(1f, rng);
            return engine.ActiveCount; // 0 when completed
        }

        [Fact]
        public void Expedition_MultiplierZero_NeverTriggersEncounters()
        {
            var engine = new ExpeditionSystem();
            engine.SetEncounterChanceMultiplier(_ => 0f);
            Assert.True(engine.Start(Def("loc_test_road"), "survivor_a", 200, ExpeditionStance.Speed));
            TickToCompletion(engine, 808);
            Assert.Equal(0, engine.ActiveCount);
            // Encounter count is only observable via state events; assert the
            // system simply never rolls past a zero threshold by re-running and
            // checking no OnEncounterTriggered ever fired.
            int fired = 0;
            var engine2 = new ExpeditionSystem();
            engine2.SetEncounterChanceMultiplier(_ => 0f);
            engine2.OnEncounterTriggered += _ => fired++;
            Assert.True(engine2.Start(Def("loc_test_road"), "survivor_b", 200, ExpeditionStance.Speed));
            TickToCompletion(engine2, 808);
            Assert.Equal(0, fired);
        }

        [Fact]
        public void Expedition_HigherMultiplier_IsMonotonicUnderSameSeed()
        {
            // Same seed ⇒ identical rng draws; a higher threshold can only ever
            // trigger equal-or-more encounters (deterministic, monotonic).
            int CountEncounters(float multiplier)
            {
                var engine = new ExpeditionSystem();
                engine.SetEncounterChanceMultiplier(_ => multiplier);
                int fired = 0;
                engine.OnEncounterTriggered += _ => fired++;
                Assert.True(engine.Start(Def("loc_test_road"), "survivor_x", 200, ExpeditionStance.Speed));
                TickToCompletion(engine, 4242);
                return fired;
            }

            int low = CountEncounters(0.5f);
            int baseCount = CountEncounters(1.0f);
            int high = CountEncounters(3.0f);
            Assert.True(baseCount >= low, "multiplier 1.0 meets at least as often as 0.5");
            Assert.True(high >= baseCount, "multiplier 3.0 meets at least as often as 1.0");
        }

        [Fact]
        public void Expedition_MultiplierSaturatesAtOne_AndIsDeterministic()
        {
            var a = new ExpeditionSystem();
            a.SetEncounterChanceMultiplier(_ => 1000f);
            var b = new ExpeditionSystem();
            b.SetEncounterChanceMultiplier(_ => 1000f);
            int fa = 0, fb = 0;
            a.OnEncounterTriggered += _ => fa++;
            b.OnEncounterTriggered += _ => fb++;
            Assert.True(a.Start(Def("loc_test_road"), "survivor_a", 200, ExpeditionStance.Speed));
            Assert.True(b.Start(Def("loc_test_road"), "survivor_b", 200, ExpeditionStance.Speed));
            TickToCompletion(a, 9001);
            TickToCompletion(b, 9001);
            Assert.Equal(fa, fb);           // same seed ⇒ identical encounter count
            Assert.True(fa > 0, "saturated multiplier still triggers encounters");
        }

        [Fact]
        public void Warlord_ControlledGround_RaisesSortieEncounterChance()
        {
            var catalog = WarlordDoctrineTests.LoadCatalogForTests();
            var warlord = new WarlordDoctrineSystem(catalog, 2026);

            // Home (controlled) is hostile ground: danger +0.35.
            Assert.Equal(0.35f, warlord.TravelDangerModifier("loc_toll_house"));
            float mult = 1f + warlord.TravelDangerModifier("loc_toll_house");
            Assert.Equal(1.35f, mult);

            // A neutral location has no warlord pressure.
            Assert.Equal(0f, warlord.TravelDangerModifier("loc_grain_silo"));

            // Compose through the expedition hook and verify the roll stream is
            // deterministic for the composed multiplier.
            var engine = new ExpeditionSystem();
            engine.SetEncounterChanceMultiplier(loc => 1f + warlord.TravelDangerModifier(loc));
            int fired = 0;
            engine.OnEncounterTriggered += _ => fired++;
            Assert.True(engine.Start(Def("loc_toll_house"), "survivor_w", 200, ExpeditionStance.Speed));
            TickToCompletion(engine, 555);
            int firedA = fired;

            var engine2 = new ExpeditionSystem();
            engine2.SetEncounterChanceMultiplier(loc => 1f + warlord.TravelDangerModifier(loc));
            int fired2 = 0;
            engine2.OnEncounterTriggered += _ => fired2++;
            Assert.True(engine2.Start(Def("loc_toll_house"), "survivor_w2", 200, ExpeditionStance.Speed));
            TickToCompletion(engine2, 555);
            Assert.Equal(firedA, fired2); // same seed + same danger ⇒ same encounters
        }

        [Fact]
        public void Warlord_AnnexedGround_RaisesDangerOnSorties()
        {
            var catalog = WarlordDoctrineTests.LoadCatalogForTests();
            var warlord = new WarlordDoctrineSystem(catalog, 2026);

            // Craft the weighbridge as controlled (the state DTO is the save authority).
            var st = warlord.CaptureState();
            st.Territory("loc_weighbridge").state = (int)WarlordTerritoryState.Controlled;
            warlord.RestoreState(st);

            float mult = 1f + warlord.TravelDangerModifier("loc_weighbridge");
            Assert.Equal(1.35f, mult);

            // Contested ground is dangerous but less so.
            st = warlord.CaptureState();
            st.Territory("loc_weighbridge").state = (int)WarlordTerritoryState.Contested;
            warlord.RestoreState(st);
            Assert.Equal(1.20f, 1f + warlord.TravelDangerModifier("loc_weighbridge"));
        }
    }
}
