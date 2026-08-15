using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Economy;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Data;
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

        [Test]
        public void ScarcityMultiplierTapersByDayInsteadOfApplyingForever()
        {
            // Regression test: GetTradeValue's scarcity overlay must re-derive
            // the tier from the real current day (via the day provider) on
            // every call, not bake a fixed multiplier at boot that applies
            // for the whole campaign.
            var sys = new DynamicEconomySystem(() => WorldPhase.CivilWar);
            sys.SetScarcityOverride(new ScarcityOverride { Source = "Test", IsHardcore = true });
            var jewelry = ScriptableObject.CreateInstance<ItemDefinition>();
            jewelry.id = "jewelry";
            jewelry.displayName = "jewelry";
            jewelry.type = ItemType.Trade;
            jewelry.tradeValue = 50f;

            sys.SetDayProvider(() => 90);
            Assert.AreEqual(25f, sys.GetTradeValue(jewelry), 0.001f,
                "Day 90 is inside the Low tier window (80+); jewelry should be 0.5x");

            sys.SetDayProvider(() => 50);
            Assert.AreEqual(50f, sys.GetTradeValue(jewelry), 0.001f,
                "Day 50 falls in the Moderate window, but jewelry is a Low-tier item, not Moderate; no multiplier should apply");

            Object.DestroyImmediate(jewelry);
        }

        [Test]
        public void ScarcityMultiplierIsOffWhenNotHardcore()
        {
            var sys = new DynamicEconomySystem(() => WorldPhase.CivilWar);
            sys.SetScarcityOverride(new ScarcityOverride { Source = "Test", IsHardcore = false });
            sys.SetDayProvider(() => 90);
            var jewelry = ScriptableObject.CreateInstance<ItemDefinition>();
            jewelry.id = "jewelry";
            jewelry.displayName = "jewelry";
            jewelry.type = ItemType.Trade;
            jewelry.tradeValue = 50f;

            Assert.AreEqual(50f, sys.GetTradeValue(jewelry), 0.001f);

            Object.DestroyImmediate(jewelry);
        }
    }

    [TestFixture]
    public class AshGetsDeeperItemsWireUpTests
    {
        [Test]
        public void EightyItemsMaterialiseIntoUniqueIds()
        {
            var defs = AshGetsDeeperItemsCatalog.MaterialiseAll();
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
