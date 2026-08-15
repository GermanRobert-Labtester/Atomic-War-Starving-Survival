using NUnit.Framework;
using AtomicWar._Game.Factions;

namespace AtomicWar.Tests.EditMode
{
    // Lore bible 05_FACTIONS — behaviour of the five new Current state classes.
    [TestFixture]
    public class CurrentsNpcTests
    {
        [Test]
        public void LamplightersFirstAskRefusesAndKeepsAccess()
        {
            var npc = new NPC_Lamplighters();
            bool withdrew = false;
            npc.OnAccessWithdrawn += _ => withdrew = true;

            bool answer = npc.RequestDarkNight();
            Assert.IsFalse(answer, "the refusal is not negotiable");
            Assert.IsTrue(npc.State.accessGranted);
            Assert.IsFalse(withdrew);
        }

        [Test]
        public void LamplightersSecondAskWithdrawsAccessAndCountsDownElevenDays()
        {
            var npc = new NPC_Lamplighters();
            int lampsOut = 0;
            npc.OnLampsOut += _ => lampsOut++;

            npc.RequestDarkNight();
            npc.RequestDarkNight();

            Assert.IsFalse(npc.State.accessGranted);
            Assert.AreEqual(11, npc.State.lampsOutCountdown);

            for (int i = 0; i < 10; i++) npc.Tick(1f);
            Assert.AreEqual(1, npc.State.lampsOutCountdown, "ten days in, one lamp remains");
            Assert.AreEqual(0, lampsOut);

            npc.Tick(1f);
            Assert.AreEqual(0, npc.State.lampsOutCountdown);
            Assert.AreEqual(1, lampsOut, "the last lamp goes out on the eleventh day");
        }

        [Test]
        public void QuietHouseRecordsTheLieExactlyAsGiven()
        {
            var npc = new NPC_QuietHouse();
            var intake = npc.AcceptTheDyingWithLie("Anna Voss", "She loved the sea.", 44);
            Assert.AreEqual("Anna Voss", intake.personName);
            Assert.AreEqual("She loved the sea.", intake.trueThing);
            Assert.IsFalse(intake.toldTrue, "the House writes the tag exactly as given, but the game must know it was a lie");
            Assert.AreEqual(1, npc.State.intakes.Count);
        }

        [Test]
        public void GrainExchangeQuietensAfterFourDecliningSeasons()
        {
            var npc = new NPC_GrainExchange();
            bool quieted = false;
            npc.OnExchangeQuieted += _ => quieted = true;

            // Without the player at the board, nothing declines.
            npc.TickSeasonalDecline();
            Assert.AreEqual(4, npc.State.attendees);

            npc.PlayerSetsBoard();
            npc.TickSeasonalDecline(); // 3 attendees
            npc.TickSeasonalDecline(); // 2
            npc.TickSeasonalDecline(); // 1
            Assert.IsFalse(quieted, "three seasons in, a trader still comes");

            npc.TickSeasonalDecline(); // 0
            Assert.IsTrue(quieted, "roughly ninety days later, nobody comes up there any more");
            Assert.IsTrue(npc.State.exchangeQuieted);
        }

        [Test]
        public void TallyEnforcesExactlyAsWritten()
        {
            var npc = new NPC_Tally();
            int enforced = 0;
            npc.OnContractEnforced += (_, _) => enforced++;

            npc.WriteContract("the_player", "forty litres of fuel", "sixty days", "fixed", "the generator", 80);
            npc.WriteContract("the_player", "two filters", "thirty days", "fixed", "the air room", 120);

            var due = npc.EnforceDueContracts(80);
            Assert.AreEqual(1, due.Count, "only the contract due on day 80 is enforced");
            Assert.AreEqual(1, enforced);

            var again = npc.EnforceDueContracts(120);
            Assert.AreEqual(1, again.Count);
            Assert.AreEqual(2, enforced);
            Assert.AreEqual(0, npc.EnforceDueContracts(200).Count, "no contract enforced twice");
        }

        [Test]
        public void UndertowChartDistributionEndsTheBusinessModel()
        {
            var npc = new NPC_Undertow();
            int circulated = 0;
            npc.OnChartCirculated += _ => circulated++;

            Assert.AreEqual(0.1f, npc.State.salvageAccidentRisk, 0.001f);
            npc.ChartDistributed();
            npc.ChartDistributed(); // idempotent

            Assert.AreEqual(1, circulated, "the event fires once");
            Assert.AreEqual(0.5f, npc.State.salvageAccidentRisk, 0.001f,
                "after the chart circulates, accidents are hard to explain by water alone");
            Assert.IsTrue(npc.State.chartDistributed);
        }

        [Test]
        public void AllFiveStatesRoundTrip()
        {
            var lamplighters = new NPC_Lamplighters();
            lamplighters.RequestDarkNight();
            var lamplightersRestored = new NPC_Lamplighters();
            lamplightersRestored.RestoreState(lamplighters.CaptureState());
            Assert.AreEqual(1, lamplightersRestored.State.darkNightRequests);

            var quietHouse = new NPC_QuietHouse();
            quietHouse.AcceptTheDying("N", "T", 9);
            var quietHouseRestored = new NPC_QuietHouse();
            quietHouseRestored.RestoreState(quietHouse.CaptureState());
            Assert.AreEqual(1, quietHouseRestored.State.intakes.Count);

            var grain = new NPC_GrainExchange();
            grain.PlayerSetsBoard();
            var grainRestored = new NPC_GrainExchange();
            grainRestored.RestoreState(grain.CaptureState());
            Assert.IsTrue(grainRestored.State.playerControlsBoard);

            var tally = new NPC_Tally();
            tally.WriteContract("a", "d", "t", "r", "f", 5);
            var tallyRestored = new NPC_Tally();
            tallyRestored.RestoreState(tally.CaptureState());
            Assert.AreEqual(1, tallyRestored.State.contracts.Count);

            var undertow = new NPC_Undertow();
            undertow.ChartDistributed();
            var undertowRestored = new NPC_Undertow();
            undertowRestored.RestoreState(undertow.CaptureState());
            Assert.IsTrue(undertowRestored.State.chartDistributed);
        }
    }
}
