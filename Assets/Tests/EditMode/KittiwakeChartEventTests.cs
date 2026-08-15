using NUnit.Framework;
using AtomicWar._Game.Events;
using AtomicWar._Game.Factions;
using AtomicWar._Game.Core;

namespace AtomicWar.Tests.EditMode
{
    // Lore bible 05_FACTIONS §8 — the Kittiwake chart decision event.
    [TestFixture]
    public class KittiwakeChartEventTests
    {
        [Test]
        public void GatesOnFoundAndBlocksAfterResolution()
        {
            var ev = EventRunner.CreateKittiwakeChartEvent();
            Assert.AreEqual("event_kittiwake_chart", ev.id);
            Assert.AreEqual("kittiwake_chart_found", ev.conditions.RequiredFlagId);
            Assert.AreEqual("kittiwake_chart_resolved", ev.conditions.BlockedFlagId);
        }

        [Test]
        public void DistributeChoiceSetsDistributionAndResolutionFlags()
        {
            var ev = EventRunner.CreateKittiwakeChartEvent();
            var c = ev.choices.Find(x => x.ChoiceId == "distribute");
            Assert.IsNotNull(c);
            Assert.AreEqual(0f, c.MoraleDelta, 0.001f,
                "the game does not adjudicate the chart — distribution opens the late game and ends a business model");

            bool distributed = false, resolved = false;
            foreach (var fx in c.Effects)
            {
                if (fx.SetWorldFlag == "kittiwake_chart_distributed" && fx.WorldFlagValue) distributed = true;
                if (fx.SetWorldFlag == "kittiwake_chart_resolved" && fx.WorldFlagValue) resolved = true;
            }
            Assert.IsTrue(distributed);
            Assert.IsTrue(resolved);
        }

        [Test]
        public void KeepChoiceIsTheQuietAlternative()
        {
            var ev = EventRunner.CreateKittiwakeChartEvent();
            var c = ev.choices.Find(x => x.ChoiceId == "keep_it");
            Assert.IsNotNull(c);
            Assert.AreEqual(0f, c.MoraleDelta, 0.001f);
            bool kept = false;
            foreach (var fx in c.Effects)
                if (fx.SetWorldFlag == "kittiwake_chart_kept" && fx.WorldFlagValue) kept = true;
            Assert.IsTrue(kept);
        }

        [Test]
        public void UndertowNotifiedThroughItsOwnApi()
        {
            // The flag handler calls NPC_Undertow.ChartDistributed(); verify the
            // state change that must result when the wiring fires it.
            var npc = new NPC_Undertow();
            npc.ChartDistributed();
            Assert.IsTrue(npc.State.chartDistributed);
            Assert.AreEqual(0.5f, npc.State.salvageAccidentRisk, 0.001f);
        }
    }

    // Lore bible 05_FACTIONS — the five Current NPC states must round-trip
    // through SaveSystem. The production adapters are internal; this test
    // registers the same capture/restore pairs through SaveSystem's public
    // RegisterSaveable API so the save pipeline itself is exercised.
    [TestFixture]
    public class CurrentsSaveRoundTripTests
    {
        private static SaveSystem CreateSaveSystem()
        {
            return new SaveSystem(new SaveSystem.CoreDeps
            {
                GameState = new GameState(),
                SavesDir = System.IO.Path.Combine(UnityEngine.Application.temporaryCachePath, "currents_save_tests")
            });
        }

        [Test]
        public void AllFiveCurrentsRoundTripThroughSaveSystem()
        {
            var save = CreateSaveSystem();

            var lamplighters = new NPC_Lamplighters();
            var quietHouse = new NPC_QuietHouse();
            var grain = new NPC_GrainExchange();
            var tally = new NPC_Tally();
            var undertow = new NPC_Undertow();

            save.RegisterSaveable(lamplighters, "currents_lamplighters",
                n => n.CaptureState(), (n, s) => n.RestoreState(s as NPC_LamplightersState));
            save.RegisterSaveable(quietHouse, "currents_quiet_house",
                n => n.CaptureState(), (n, s) => n.RestoreState(s as NPC_QuietHouseState));
            save.RegisterSaveable(grain, "currents_grain_exchange",
                n => n.CaptureState(), (n, s) => n.RestoreState(s as NPC_GrainExchangeState));
            save.RegisterSaveable(tally, "currents_tally",
                n => n.CaptureState(), (n, s) => n.RestoreState(s as NPC_TallyState));
            save.RegisterSaveable(undertow, "currents_undertow",
                n => n.CaptureState(), (n, s) => n.RestoreState(s as NPC_UndertowState));

            // Mutate state meaningfully before saving.
            lamplighters.RequestDarkNight(); // one refusal; access still granted
            quietHouse.AcceptTheDying("Anna Voss", "She loved the sea.", 44);
            grain.PlayerSetsBoard();
            grain.TickSeasonalDecline();
            tally.WriteContract("the_player", "forty litres of fuel", "sixty days", "fixed", "the generator", 80);
            undertow.ChartDistributed();

            Assert.IsTrue(save.Save("currents_roundtrip"), "save must succeed");

            // Fresh instances restored from disk.
            var lamplighters2 = new NPC_Lamplighters();
            var quietHouse2 = new NPC_QuietHouse();
            var grain2 = new NPC_GrainExchange();
            var tally2 = new NPC_Tally();
            var undertow2 = new NPC_Undertow();

            var save2 = CreateSaveSystem();
            save2.RegisterSaveable(lamplighters2, "currents_lamplighters",
                n => n.CaptureState(), (n, s) => n.RestoreState(s as NPC_LamplightersState));
            save2.RegisterSaveable(quietHouse2, "currents_quiet_house",
                n => n.CaptureState(), (n, s) => n.RestoreState(s as NPC_QuietHouseState));
            save2.RegisterSaveable(grain2, "currents_grain_exchange",
                n => n.CaptureState(), (n, s) => n.RestoreState(s as NPC_GrainExchangeState));
            save2.RegisterSaveable(tally2, "currents_tally",
                n => n.CaptureState(), (n, s) => n.RestoreState(s as NPC_TallyState));
            save2.RegisterSaveable(undertow2, "currents_undertow",
                n => n.CaptureState(), (n, s) => n.RestoreState(s as NPC_UndertowState));

            Assert.IsTrue(save2.Load("currents_roundtrip"), "load must succeed");

            Assert.AreEqual(1, lamplighters2.State.darkNightRequests);
            Assert.IsTrue(lamplighters2.State.accessGranted);
            Assert.AreEqual(1, quietHouse2.State.intakes.Count);
            Assert.AreEqual("She loved the sea.", quietHouse2.State.intakes[0].trueThing);
            Assert.IsTrue(grain2.State.playerControlsBoard);
            Assert.AreEqual(3, grain2.State.attendees);
            Assert.AreEqual(1, tally2.State.contracts.Count);
            Assert.IsTrue(undertow2.State.chartDistributed);
            Assert.AreEqual(0.5f, undertow2.State.salvageAccidentRisk, 0.001f);
        }
    }
}
