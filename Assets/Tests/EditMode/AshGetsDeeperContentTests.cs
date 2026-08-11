using NUnit.Framework;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Data;
using AtomicWar._Game.Events;
using AtomicWar._Game.Factions;
using AtomicWar._Game.Economy;

namespace AtomicWar.Tests.EditMode
{
    // Section XIV tests: every catalog in the "Ash Gets Deeper" batch
    // exposes a builder + Materialise; the test asserts the row counts and
    // the canonical id strings match the brief.

    [TestFixture]
    public class AshGetsDeeperItemsCatalogTests
    {
        [Test]
        public void EightyItems()
        {
            var specs = AshGetsDeeperItemsCatalog.BuildAll();
            Assert.AreEqual(80, specs.Count);
        }

        [Test]
        public void AllIdsAreUniqueAndNonEmpty()
        {
            var specs = AshGetsDeeperItemsCatalog.BuildAll();
            var set = new System.Collections.Generic.HashSet<string>();
            for (int i = 0; i < specs.Count; i++)
            {
                Assert.IsFalse(string.IsNullOrEmpty(specs[i].Id), "row " + i + " has empty id");
                Assert.IsTrue(set.Add(specs[i].Id), "duplicate id: " + specs[i].Id);
            }
            Assert.AreEqual(80, set.Count);
        }

        [Test]
        public void MaterialiseRoundTrips()
        {
            var defs = AshGetsDeeperItemsCatalog.MaterialiseAll(id => null);
            Assert.AreEqual(80, defs.Count);
            Assert.AreEqual("morphine_ampoule", defs[0].id);
        }

        [Test]
        public void EachRowHasNonEmptyDescription()
        {
            var specs = AshGetsDeeperItemsCatalog.BuildAll();
            for (int i = 0; i < specs.Count; i++)
            {
                Assert.IsFalse(string.IsNullOrEmpty(specs[i].Description),
                    "row " + specs[i].Id + " missing description");
            }
        }
    }

    [TestFixture]
    public class AshGetsDeeperLocationsCatalogTests
    {
        [Test]
        public void TenLocations()
        {
            Assert.AreEqual(10, AshGetsDeeperLocationsCatalog.BuildAll().Count);
        }

        [Test]
        public void HighwayPileupMatchesTurn1Reference()
        {
            // quest_mechanic_highway_heart (turn 1) references "highway_pileup";
            // the catalog id must agree.
            var specs = AshGetsDeeperLocationsCatalog.BuildAll();
            var pileup = specs.Find(s => s.Id == AshGetsDeeperLocationsCatalog.Ids.HighwayPileup);
            Assert.IsNotNull(pileup);
            Assert.AreEqual(5, pileup.DangerLevel);
            Assert.AreEqual(20f, pileup.BaseRadsPerHour, 0.001f);
        }
    }

    [TestFixture]
    public class AshGetsDeeperEncountersCatalogTests
    {
        [Test]
        public void FifteenEncounters()
        {
            Assert.AreEqual(15, AshGetsDeeperEncountersCatalog.BuildAll().Count);
        }

        [Test]
        public void TriggerNoteEmbeddedInBodyText()
        {
            var defs = AshGetsDeeperEncountersCatalog.MaterialiseAll();
            var surge = defs.Find(e => e.id == AshGetsDeeperEncountersCatalog.Ids.PowerSurge);
            Assert.IsNotNull(surge);
            Assert.IsTrue(surge.bodyText.Contains("[Trigger: PowerGrid > 80%]"),
                "PowerSurge should embed the trigger note in bodyText");
        }

        [Test]
        public void MinDayMatchesBrief()
        {
            var specs = AshGetsDeeperEncountersCatalog.BuildAll();
            var whoEats = specs.Find(s => s.Id == AshGetsDeeperEncountersCatalog.Ids.WhoEats);
            Assert.AreEqual(10, whoEats.MinDay);
            Assert.AreEqual(1.5f, whoEats.Weight, 0.001f);
        }
    }

    [TestFixture]
    public class AshGetsDeeperEchoesCatalogTests
    {
        [Test]
        public void TenEchoes()
        {
            Assert.AreEqual(10, AshGetsDeeperEchoesCatalog.BuildAll().Count);
        }

        [Test]
        public void AllEchoesHaveAuthorAndText()
        {
            var defs = AshGetsDeeperEchoesCatalog.MaterialiseAll();
            for (int i = 0; i < defs.Count; i++)
            {
                Assert.IsFalse(string.IsNullOrEmpty(defs[i].id));
                Assert.IsFalse(string.IsNullOrEmpty(defs[i].title));
                Assert.IsTrue(defs[i].text.Length > 50, "echo " + defs[i].id + " too short");
            }
        }
    }

    [TestFixture]
    public class AshGetsDeeperNpcIdsTests
    {
        [Test]
        public void SixHostileOrAmbiguousNpcs()
        {
            var ids = new[] {
                AshGetsDeeperNpcIds.Ids.AshWidows,
                AshGetsDeeperNpcIds.Ids.TheTollman,
                AshGetsDeeperNpcIds.Ids.BurnedPatrol,
                AshGetsDeeperNpcIds.Ids.TheCollector,
                AshGetsDeeperNpcIds.Ids.FeralChildren,
                AshGetsDeeperNpcIds.Ids.SurgeonsCaravan,
            };
            var set = new System.Collections.Generic.HashSet<string>(ids);
            Assert.AreEqual(6, set.Count);
        }

        [Test]
        public void FourFauna()
        {
            var ids = new[] {
                AshGetsDeeperNpcIds.Ids.IrradiatedDogs,
                AshGetsDeeperNpcIds.Ids.AshCrows,
                AshGetsDeeperNpcIds.Ids.BloatedCattle,
                AshGetsDeeperNpcIds.Ids.RatSwarm,
            };
            var set = new System.Collections.Generic.HashSet<string>(ids);
            Assert.AreEqual(4, set.Count);
        }

        [Test]
        public void BurnedPatrolMatchesTurn1Reference()
        {
            // quest_garrison_last_order (turn 1) references "npc_burned_patrol";
            // the catalog id must agree.
            Assert.AreEqual("npc_burned_patrol", AshGetsDeeperNpcIds.Ids.BurnedPatrol);
        }
    }

    [TestFixture]
    public class NewNpcClassStateTests
    {
        [Test]
        public void AshWidowsRoundTrip()
        {
            var npc = new NPC_AshWidows();
            npc.SetActive(true);
            npc.SetHostile(false);
            Assert.IsTrue(npc.State.isActive);
            Assert.IsFalse(npc.State.isHostile);
            var s = npc.CaptureState();
            var npc2 = new NPC_AshWidows();
            npc2.RestoreState(s);
            Assert.IsTrue(npc2.State.isActive);
        }

        [Test]
        public void IrradiatedDogsSighting()
        {
            var f = new Fauna_IrradiatedDogs();
            f.Sighted(5);
            Assert.IsTrue(f.State.isPresent);
            Assert.AreEqual(5, f.State.count);
            f.Clear();
            Assert.IsFalse(f.State.isPresent);
            Assert.AreEqual(0, f.State.count);
        }
    }

    [TestFixture]
    public class HardcoreEconomyTuningTests
    {
        [Test]
        public void FourScarcityTiers()
        {
            Assert.AreEqual(4, HardcoreEconomyTuning.ScarcityMultipliers.Count);
        }

        [Test]
        public void FiveFactionTradePreferences()
        {
            Assert.AreEqual(5, HardcoreEconomyTuning.FactionTradePreferences.Count);
        }

        [Test]
        public void FourPriceShockEvents()
        {
            Assert.AreEqual(4, HardcoreEconomyTuning.PriceShockEvents.Count);
        }

        [Test]
        public void Day1CriticalScarcityMultipliesBy2_5()
        {
            float mult = HardcoreEconomyTuning.GetScarcityMultiplierForDay(5, "clean_water");
            Assert.AreEqual(2.5f, mult, 0.001f);
        }

        [Test]
        public void Day90LowScarcityMultipliesBy0_5()
        {
            float mult = HardcoreEconomyTuning.GetScarcityMultiplierForDay(90, "book");
            Assert.AreEqual(0.5f, mult, 0.001f);
        }

        [Test]
        public void FactionWarMultiplierIs3_0()
        {
            var rule = HardcoreEconomyTuning.PriceShockEvents[2];
            Assert.AreEqual(HardcoreEconomyTuning.PriceShock.FactionWar, rule.Kind);
            Assert.AreEqual(3.0f, rule.Multiplier, 0.001f);
        }
    }
}
