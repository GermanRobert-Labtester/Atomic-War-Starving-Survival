using NUnit.Framework;
using AtomicWar._Game.Events;
using AtomicWar._Game.Factions;

namespace AtomicWar.Tests.EditMode
{
    // Lore bible 05_FACTIONS "Interlocks" — the Currents are written to cross
    // each other, so the world feels like a system rather than a menu.
    [TestFixture]
    public class CurrentsInterlockTests
    {
        [Test]
        public void GrainExchangeArrangesEnforcementThroughTally()
        {
            var exchange = new NPC_GrainExchange();
            var tally = new NPC_Tally();

            var contract = exchange.ArrangeTallyEnforcement(
                tally, "the_player", "forty litres of fuel", "sixty days", "fixed", "the generator", 80);

            Assert.IsNotNull(contract);
            Assert.AreEqual("forty litres of fuel", contract.debt);
            Assert.AreEqual(1, tally.State.contracts.Count, "the Tally holds the real contract");
            Assert.IsTrue(exchange.State.tallyEnforcementArranged);
            Assert.AreEqual(1, exchange.State.tallyContractsArranged);

            // The Exchange itself still has no guards and no charter — that is the point.
            Assert.AreEqual(0, exchange.State.attendees - 4);
        }

        [Test]
        public void KittiwakeChartEndsArchivistIsolationOnce()
        {
            var archivists = new NPC_Archivists();
            int ended = 0;
            archivists.OnIsolationEnded += _ => ended++;

            Assert.IsTrue(archivists.State.isolated, "the Memory Vault is reachable only by boat");
            archivists.EndIsolation();
            archivists.EndIsolation(); // idempotent

            Assert.IsFalse(archivists.State.isolated);
            Assert.AreEqual(1, ended);
        }

        [Test]
        public void SunSeekersGrieveLitNightRoutesOncePerNight()
        {
            var seekers = new NPC_SunSeekers();
            var lamplighters = new NPC_Lamplighters();

            // Night, day 5 — one grievance.
            Assert.IsTrue(seekers.AssessNightLamps(lamplighters, 22f, 5));
            Assert.AreEqual(1, seekers.State.nightLampGrievances);

            // Same night, later hour — no second grievance.
            Assert.IsFalse(seekers.AssessNightLamps(lamplighters, 23f, 5));
            Assert.AreEqual(1, seekers.State.nightLampGrievances);

            // Next night — grievance two.
            Assert.IsTrue(seekers.AssessNightLamps(lamplighters, 21f, 6));
            Assert.AreEqual(2, seekers.State.nightLampGrievances);

            // Daytime — nothing.
            Assert.IsFalse(seekers.AssessNightLamps(lamplighters, 12f, 7));
            Assert.AreEqual(2, seekers.State.nightLampGrievances);
        }

        [Test]
        public void SunSeekersStopGrievingAfterLampsWithdrawn()
        {
            var seekers = new NPC_SunSeekers();
            var lamplighters = new NPC_Lamplighters();

            lamplighters.RequestDarkNight();
            lamplighters.RequestDarkNight(); // access withdrawn, lamps going out

            Assert.IsFalse(seekers.AssessNightLamps(lamplighters, 22f, 9));
            Assert.AreEqual(0, seekers.State.nightLampGrievances);
        }

        [Test]
        public void TwoEndsEventPresentsBothEndpointsAndKeepsFlagsDistinct()
        {
            var ev = EventRunner.CreateTwoEndsEvent();
            Assert.AreEqual("event_two_ends", ev.id);
            Assert.AreEqual(3, ev.choices.Count);
            Assert.AreEqual("two_ends_resolved", ev.conditions.BlockedFlagId);

            var quiet = ev.choices.Find(x => x.ChoiceId == "quiet_house");
            var osteo = ev.choices.Find(x => x.ChoiceId == "osteophages");
            Assert.IsNotNull(quiet);
            Assert.IsNotNull(osteo);
            Assert.AreEqual(-6f, quiet.MoraleDelta, 0.001f);
            Assert.AreEqual(-12f, osteo.MoraleDelta, 0.001f,
                "the airlock is the darker of the two ends, and the game does not say it is wrong");

            bool quietFlag = false, osteoFlag = false;
            foreach (var fx in quiet.Effects)
                if (fx.SetWorldFlag == "sent_to_quiet_house" && fx.WorldFlagValue) quietFlag = true;
            foreach (var fx in osteo.Effects)
                if (fx.SetWorldFlag == "sent_to_osteophages" && fx.WorldFlagValue) osteoFlag = true;
            Assert.IsTrue(quietFlag);
            Assert.IsTrue(osteoFlag);
            Assert.AreNotEqual(
                quiet.Effects[0].SetWorldFlag,
                osteo.Effects[0].SetWorldFlag,
                "the two ends must be distinguishable in the record");
        }
    }
}
