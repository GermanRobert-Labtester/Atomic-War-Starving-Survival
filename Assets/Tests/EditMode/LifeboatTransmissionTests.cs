using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Events;
using AtomicWar._Game.Survivors;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Prompt #20 — Lifeboat Transmission: Day ≥ 80 contact, choose exactly one
    /// living survivor to extract; rest die; bittersweet LifeboatPartialExtraction
    /// victory; mutually exclusive with full RescueExtractionSuccess.
    /// </summary>
    [TestFixture]
    public class LifeboatTransmissionTests
    {
        private int _day;
        private List<Survivor> _survivors;
        private List<Object> _toDestroy;

        [SetUp]
        public void SetUp()
        {
            _day = 80;
            _toDestroy = new List<Object>();
            _survivors = new List<Survivor>
            {
                MakeSurvivor("s_mara", "Mara"),
                MakeSurvivor("s_ren", "Ren"),
                MakeSurvivor("s_jon", "Jon")
            };
        }

        [TearDown]
        public void TearDown()
        {
            if (_toDestroy == null) return;
            for (int i = 0; i < _toDestroy.Count; i++)
            {
                if (_toDestroy[i] != null)
                    Object.DestroyImmediate(_toDestroy[i]);
            }
            _toDestroy = null;
        }

        private static Survivor MakeSurvivor(string id, string name)
        {
            var s = new Survivor
            {
                Id = id,
                DisplayName = name,
                State = SurvivorState.Idle,
                LifetimeRadiationExposure = 8f
            };
            s.Needs.Morale = 50f;
            s.Needs.Health = 80f;
            s.Needs.Hunger = 40f;
            return s;
        }

        private (LifeboatTransmissionSystem boat, EndgameEngine engine, VictoryProjectManager victory)
            MakeStack(bool terminalVictory = false)
        {
            var engine = new EndgameEngine(GameModeKind.Story, 120);
            var victory = new VictoryProjectManager();
            if (terminalVictory)
            {
                // Force terminal via vehicle-less loss path is hard; use ApplyLifeboat reverse —
                // instead mark extraction and tick day 100 after grant.
            }

            var boat = new LifeboatTransmissionSystem();
            boat.Bind(
                getDay: () => _day,
                getSurvivors: () => _survivors,
                isCampaignTerminal: () => victory.IsTerminal || engine.Result.IsTerminal,
                endgame: engine,
                victory: victory);
            return (boat, engine, victory);
        }

        [Test]
        public void BeforeDay80_CannotOffer()
        {
            _day = 79;
            var (boat, _, _) = MakeStack();
            Assert.IsFalse(boat.CanOfferContact());
            Assert.IsNull(boat.TickDay(_day, _survivors));
        }

        [Test]
        public void Day80_OffersContactWithOneChoicePerLivingSurvivor()
        {
            var (boat, _, _) = MakeStack();
            GameEvent offered = null;
            boat.OnContactOffered += ev => offered = ev;

            var ev = boat.TickDay(80, _survivors);
            Assert.IsNotNull(ev);
            Assert.IsNotNull(offered);
            Assert.That(ev.id, Is.EqualTo(LifeboatTransmissionSystem.EventId));
            Assert.That(ev.choices.Count, Is.EqualTo(3));
            Assert.That(ev.choices.Exists(c => c.ChoiceId == "send_s_mara"));
            Assert.That(ev.choices.Exists(c => c.ChoiceId == "send_s_ren"));
            Assert.That(ev.choices.Exists(c => c.ChoiceId == "send_s_jon"));
            Assert.IsTrue(boat.HasContacted);
            Assert.IsTrue(boat.HasOffered);

            // Second tick does not re-offer.
            Assert.IsNull(boat.TickDay(81, _survivors));

            _toDestroy.Add(ev);
        }

        [Test]
        public void ResolveSend_ExtractsOne_KillsRest_BittersweetVictory()
        {
            var (boat, engine, victory) = MakeStack();
            EndgameSummaryData summary = null;
            victory.OnEndgameTriggered += s => summary = s;

            var chronicle = new List<(int day, string desc, string who)>();
            boat.OnMoralRecord += (d, desc, who) => chronicle.Add((d, desc, who));

            var ev = boat.OfferContact(_survivors, 82);
            Assert.IsNotNull(ev);

            Assert.IsTrue(boat.ResolveSend("s_ren"));
            Assert.IsTrue(boat.IsResolved);
            Assert.That(boat.ExtractedSurvivorId, Is.EqualTo("s_ren"));
            Assert.That(boat.ExtractedSurvivorName, Is.EqualTo("Ren"));
            Assert.That(boat.LeftBehindIds.Count, Is.EqualTo(2));

            // Fates
            Assert.That(_survivors.Find(s => s.Id == "s_ren").IsAlive, Is.True);
            Assert.That(_survivors.Find(s => s.Id == "s_mara").IsAlive, Is.False);
            Assert.That(_survivors.Find(s => s.Id == "s_jon").IsAlive, Is.False);
            Assert.That(_survivors.Find(s => s.Id == "s_mara").State, Is.EqualTo(SurvivorState.Dead));

            // EndgameEngine
            Assert.IsTrue(engine.Result.IsTerminal);
            Assert.IsTrue(engine.Result.IsVictory);
            Assert.That(engine.Result.ConditionKind,
                Is.EqualTo(EndgameConditionKind.LifeboatPartialExtraction));
            Assert.That(engine.Result.OutcomeSummary, Does.Contain("Ren"));
            Assert.That(engine.Result.OutcomeSummary, Does.Contain("2"));

            // VictoryProject
            Assert.IsTrue(victory.IsTerminal);
            Assert.That(victory.State, Is.EqualTo(EndgameState.Lifeboat));
            Assert.IsNotNull(summary);
            Assert.That(summary.OutcomeTitle, Is.EqualTo("LIFEBOAT"));
            Assert.That(summary.LivingCount, Is.EqualTo(1));
            Assert.That(summary.DeadCount, Is.EqualTo(2));

            // Chronicle recorded who left and who stayed
            Assert.That(chronicle.Count, Is.GreaterThanOrEqualTo(3));
            Assert.That(chronicle[0].desc, Does.Contain("Ren").IgnoreCase);

            // Cannot resolve twice
            Assert.IsFalse(boat.ResolveSend("s_mara"));

            _toDestroy.Add(ev);
        }

        [Test]
        public void Lifeboat_BlocksFullRescueExtractionSuccess()
        {
            var (boat, engine, victory) = MakeStack();
            boat.OfferContact(_survivors, 80);
            Assert.IsTrue(boat.ResolveSend("s_mara"));

            // Full extraction path must not re-trigger a different victory.
            victory.GrantMilitaryIntel(VictoryProjectManager.IntelRequiredForExtraction, day: 20);
            var chopper = victory.TickDay(VictoryProjectManager.ChopperArrivalDay, _survivors);
            // TickDay returns the existing summary when already terminal — state must stay Lifeboat.
            Assert.That(victory.State, Is.EqualTo(EndgameState.Lifeboat),
                "Victory already terminal via Lifeboat — no Rescued stack");
            if (chopper != null)
                Assert.That(chopper.State, Is.EqualTo(EndgameState.Lifeboat));

            // EndgameEngine Evaluate also no-ops when terminal
            bool again = engine.Evaluate(
                currentDay: 100,
                survivors: _survivors,
                shelter: null,
                isExtractionUnlocked: true,
                isHydroponicsOperational: true,
                totalDeathsRecorded: 2);
            Assert.IsTrue(again, "Already terminal returns true");
            Assert.That(engine.Result.ConditionKind,
                Is.EqualTo(EndgameConditionKind.LifeboatPartialExtraction),
                "Must not overwrite Lifeboat with RescueExtractionSuccess");
        }

        [Test]
        public void FullRescueAlreadyTerminal_BlocksLifeboat()
        {
            var engine = new EndgameEngine();
            var victory = new VictoryProjectManager();
            victory.GrantMilitaryIntel(VictoryProjectManager.IntelRequiredForExtraction, day: 10);
            var allAlive = new List<Survivor>
            {
                MakeSurvivor("a", "A"),
                MakeSurvivor("b", "B")
            };
            var rescued = victory.TickDay(VictoryProjectManager.ChopperArrivalDay, allAlive);
            Assert.IsNotNull(rescued);
            Assert.That(victory.State, Is.EqualTo(EndgameState.Rescued));

            // Also terminal on engine via full extraction evaluate
            engine.Evaluate(100, allAlive, null, true, false, 0);
            Assert.That(engine.Result.ConditionKind,
                Is.EqualTo(EndgameConditionKind.RescueExtractionSuccess));

            var boat = new LifeboatTransmissionSystem();
            boat.Bind(
                getDay: () => 90,
                getSurvivors: () => allAlive,
                isCampaignTerminal: () => victory.IsTerminal || engine.Result.IsTerminal,
                endgame: engine,
                victory: victory);

            Assert.IsFalse(boat.CanOfferContact(90, allAlive));
            Assert.IsNull(boat.OfferContact(allAlive, 90));
            Assert.IsFalse(boat.ResolveSend("a"));
        }

        [Test]
        public void ApplyChoiceFromEvent_RoutesSendChoice()
        {
            var (boat, engine, victory) = MakeStack();
            var ev = boat.CreateContactEvent(_survivors, 80);
            var choice = ev.choices.Find(c => c.ChoiceId == "send_s_jon");
            Assert.IsTrue(boat.ApplyChoiceFromEvent(ev, choice));
            Assert.That(boat.ExtractedSurvivorId, Is.EqualTo("s_jon"));
            Assert.That(engine.Result.ConditionKind,
                Is.EqualTo(EndgameConditionKind.LifeboatPartialExtraction));
            Assert.That(victory.State, Is.EqualTo(EndgameState.Lifeboat));
            _toDestroy.Add(ev);
        }

        [Test]
        public void CaptureRestore_PreservesResolvedState()
        {
            var (boat, _, _) = MakeStack();
            boat.OfferContact(_survivors, 80);
            boat.ResolveSend("s_mara");
            var save = boat.CaptureState();

            var (boat2, _, _) = MakeStack();
            // Reset survivors for clean restore of boat flags only
            boat2.RestoreState(save);
            Assert.IsTrue(boat2.IsResolved);
            Assert.IsTrue(boat2.HasContacted);
            Assert.That(boat2.ExtractedSurvivorId, Is.EqualTo("s_mara"));
            Assert.That(boat2.LeftBehindIds.Count, Is.EqualTo(2));
            Assert.IsFalse(boat2.CanOfferContact(100, _survivors));
        }

        [Test]
        public void NoLivingSurvivors_CannotOffer()
        {
            foreach (var s in _survivors)
                s.State = SurvivorState.Dead;
            var (boat, _, _) = MakeStack();
            Assert.IsFalse(boat.CanOfferContact(90, _survivors));
        }

        [Test]
        public void SingleSurvivor_CanTakeTheOnlySeat()
        {
            _survivors = new List<Survivor> { MakeSurvivor("solo", "Solo") };
            var (boat, engine, victory) = MakeStack();
            boat.OfferContact(_survivors, 85);
            Assert.IsTrue(boat.ResolveSend("solo"));
            Assert.That(_survivors[0].IsAlive, Is.True);
            Assert.That(boat.LeftBehindIds.Count, Is.EqualTo(0));
            Assert.That(engine.Result.ConditionKind,
                Is.EqualTo(EndgameConditionKind.LifeboatPartialExtraction));
            Assert.That(victory.State, Is.EqualTo(EndgameState.Lifeboat));
        }

        [Test]
        public void EngineApplyLifeboat_IsVictoryBittersweet()
        {
            var engine = new EndgameEngine();
            Assert.IsTrue(engine.ApplyLifeboatPartialExtraction(88, "Mara", 2));
            Assert.IsTrue(engine.Result.IsVictory);
            Assert.IsFalse(engine.Result.IsDefeat);
            Assert.That(engine.Result.DaysSurvived, Is.EqualTo(88));
            Assert.That(engine.Result.OutcomeSummary, Does.Contain("Mara"));
            Assert.That(engine.Result.OutcomeSummary, Does.Contain("concrete").IgnoreCase);
            // Second apply blocked
            Assert.IsFalse(engine.ApplyLifeboatPartialExtraction(90, "Ren", 1));
        }
    }
}
