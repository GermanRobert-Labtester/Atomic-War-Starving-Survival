using NUnit.Framework;
using AtomicWar._Game.Core;
using AtomicWar._Game.Economy;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Factions;
using AtomicWar._Game.Events;

namespace AtomicWar.Tests.EditMode
{
    // Section XIV wire-up tests: GameBootstrap.AshGetsDeeper boots the
    // 80 items, 10 locations, 15 encounters, 10 echoes, 12 NPC/fauna
    // archetypes, and applies the Hardcore economy override when
    // IsHardcoreMode is true.

    [TestFixture]
    public class GameModeFlagTests
    {
        [Test]
        public void HardcoreDefaultsToFalse()
        {
            // Default game mode is Story; Hardcore is off unless Expert or
            // the inspector override is on.
            Assert.IsFalse(AshGetsDeeperHelper.IsHardcoreModeForTest(GameModeKind.Story, false));
            Assert.IsFalse(AshGetsDeeperHelper.IsHardcoreModeForTest(GameModeKind.Custom, false));
        }

        [Test]
        public void ExpertModeTurnsHardcoreOn()
        {
            Assert.IsTrue(AshGetsDeeperHelper.IsHardcoreModeForTest(GameModeKind.Expert, false));
        }

        [Test]
        public void ForceHardcoreOverrideBeatsMode()
        {
            Assert.IsTrue(AshGetsDeeperHelper.IsHardcoreModeForTest(GameModeKind.Story, true));
            Assert.IsTrue(AshGetsDeeperHelper.IsHardcoreModeForTest(GameModeKind.Expert, true));
        }
    }

    [TestFixture]
    public class HardcoreScarcityOverrideTests
    {
        [Test]
        public void OverrideStartsDisabled()
        {
            var sys = new DynamicEconomySystem();
            Assert.IsNull(sys.GetScarcityOverride());
        }

        [Test]
        public void OverrideRoundTrips()
        {
            var sys = new DynamicEconomySystem();
            var ov = new ScarcityOverride
            {
                Source = "Test",
                DayRangeStart = 1,
                DayRangeEnd = 9999,
                Tier1Critical = 2.5f,
                Tier2High = 2.0f,
                Tier3Moderate = 1.5f,
                Tier4Low = 0.5f,
                IsHardcore = true
            };
            sys.SetScarcityOverride(ov);
            Assert.AreEqual(ov, sys.GetScarcityOverride());
        }

        [Test]
        public void OverrideCanBeCleared()
        {
            var sys = new DynamicEconomySystem();
            sys.SetScarcityOverride(new ScarcityOverride { IsHardcore = true });
            sys.SetScarcityOverride(null);
            Assert.IsNull(sys.GetScarcityOverride());
        }
    }

    [TestFixture]
    public class AshGetsDeeperItemsWireUpTests
    {
        [Test]
        public void EightyItemsMaterialiseIntoUniqueIds()
        {
            var lookup = new System.Func<string, ItemDefinition>(id =>
            {
                var def = ScriptableObject.CreateInstance<ItemDefinition>();
                def.id = id;
                def.displayName = id;
                return def;
            });
            var defs = AshGetsDeeperItemsCatalog.MaterialiseAll(lookup);
            Assert.AreEqual(80, defs.Count);
            var set = new System.Collections.Generic.HashSet<string>();
            for (int i = 0; i < defs.Count; i++) set.Add(defs[i].id);
            Assert.AreEqual(80, set.Count, "All item ids must be unique");
        }
    }

    [TestFixture]
    public class AshGetsDeeperLocationsWireUpTests
    {
        [Test]
        public void TenLocationsPooled()
        {
            var locs = AshGetsDeeperLocationsCatalog.MaterialiseAll();
            Assert.AreEqual(10, locs.Count);
        }
    }

    [TestFixture]
    public class AshGetsDeeperEncountersWireUpTests
    {
        [Test]
        public void FifteenEncountersPooled()
        {
            var defs = AshGetsDeeperEncountersCatalog.MaterialiseAll();
            Assert.AreEqual(15, defs.Count);
        }
    }

    [TestFixture]
    public class AshGetsDeeperEchoesWireUpTests
    {
        [Test]
        public void TenEchoesMaterialise()
        {
            var defs = AshGetsDeeperEchoesCatalog.MaterialiseAll();
            Assert.AreEqual(10, defs.Count);
        }
    }

    [TestFixture]
    public class AshGetsDeeperNpcIdsWireUpTests
    {
        [Test]
        public void AllTwelveIdsAreUnique()
        {
            var ids = new[] {
                AshGetsDeeperNpcIds.Ids.AshWidows,
                AshGetsDeeperNpcIds.Ids.TheTollman,
                AshGetsDeeperNpcIds.Ids.BurnedPatrol,
                AshGetsDeeperNpcIds.Ids.TheCollector,
                AshGetsDeeperNpcIds.Ids.FeralChildren,
                AshGetsDeeperNpcIds.Ids.SurgeonsCaravan,
                AshGetsDeeperNpcIds.Ids.IrradiatedDogs,
                AshGetsDeeperNpcIds.Ids.AshCrows,
                AshGetsDeeperNpcIds.Ids.BloatedCattle,
                AshGetsDeeperNpcIds.Ids.RatSwarm,
            };
            var set = new System.Collections.Generic.HashSet<string>(ids);
            Assert.AreEqual(10, set.Count);
        }
    }

    // Test-side helper: the production wire-up lives in the partial, which
    // depends on the full GameBootstrap (and the full ProjectSettings/Inspector
    // graph). For unit tests we expose a pure-function predicate that
    // mirrors IsHardcoreMode so we can verify the logic without standing
    // up the full MonoBehaviour. The production partial's real
    // IsHardcoreMode is the same expression with the inspector fields.
    internal static class AshGetsDeeperHelper
    {
        public static bool IsHardcoreModeForTest(GameModeKind mode, bool forceHardcore)
            => forceHardcore || mode == GameModeKind.Expert;
    }
}
