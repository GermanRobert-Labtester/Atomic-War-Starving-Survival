using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Events;
using AtomicWar._Game.Survivors;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Prompt #43 — multi-stage narrative chains via scheduleEvent.
    /// Part 1 choice → day advance → Part 2 fires with eventFlag / TraitGates.
    /// </summary>
    [TestFixture]
    public class NarrativeChainTests
    {
        private EventRunner _runner;
        private List<GameEvent> _pool;
        private string _savesDir;

        [SetUp]
        public void SetUp()
        {
            _runner = new EventRunner();
            _pool = EventRunner.CreateEmissaryChain();
            _runner.SetPool(_pool);
            _savesDir = Path.Combine(Path.GetTempPath(), "ashfall_narrative_chain_" + System.Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(_savesDir);
        }

        [TearDown]
        public void TearDown()
        {
            if (_pool != null)
            {
                for (int i = 0; i < _pool.Count; i++)
                {
                    if (_pool[i] != null)
                        Object.DestroyImmediate(_pool[i]);
                }
            }
            if (Directory.Exists(_savesDir))
            {
                try { Directory.Delete(_savesDir, true); }
                catch { /* best-effort cleanup */ }
            }
        }

        private static Survivor MakeSurvivor(string id, RiskBiasTrait trait, float medical = 0.3f)
        {
            return new Survivor
            {
                Id = id,
                DisplayName = id,
                RiskBias = trait,
                MedicalSkill = medical
            };
        }

        private static EventContext MakeContext(
            int day,
            IList<Survivor> crew,
            Dictionary<string, bool> flags = null,
            float scavengerTrust = 0f)
        {
            var primary = crew != null && crew.Count > 0 ? crew[0] : null;
            var ctx = new EventContext(primary)
            {
                CurrentDay = day,
                CurrentHour = 12f,
                AllSurvivors = crew != null ? new List<Survivor>(crew) : null,
                GetFactionTrust = fid =>
                    fid == EventRunner.EmissaryFactionId ? scavengerTrust : 0f
            };
            if (flags != null)
                ctx.ImportFlags(flags);
            return ctx;
        }

        [Test]
        public void ShareWater_SchedulesReturnFavor_TwoDaysLater()
        {
            var crew = new List<Survivor> { MakeSurvivor("r1", RiskBiasTrait.Realist) };
            var ctx = MakeContext(10, crew);
            var part1 = _runner.FindInPool(EventRunner.EmissaryEventId);
            var share = EventRunner.FindAvailableChoice(part1, ctx, EventRunner.EmissaryShareChoiceId);
            Assert.That(share, Is.Not.Null);

            _runner.ApplyChoice(part1, share, ctx);

            Assert.That(ctx.HasEventFlag(EventRunner.FlagSharedWaterWithEmissary), Is.True);
            Assert.That(_runner.ScheduledEvents.Count, Is.EqualTo(1));
            Assert.That(_runner.ScheduledEvents[0].EventId, Is.EqualTo(EventRunner.EmissaryReturnFavorId));
            Assert.That(_runner.ScheduledEvents[0].ExecuteOnDay,
                Is.EqualTo(10 + EventRunner.EmissaryFavorDelayDays));
        }

        [Test]
        public void ShareWater_TickDay_FiresPart2_WithFlagGates()
        {
            var crew = new List<Survivor>
            {
                MakeSurvivor("c1", RiskBiasTrait.Cautious),
                MakeSurvivor("p1", RiskBiasTrait.Paranoid)
            };
            var ctx = MakeContext(10, crew);
            var part1 = _runner.FindInPool(EventRunner.EmissaryEventId);
            var share = EventRunner.FindAvailableChoice(part1, ctx, EventRunner.EmissaryShareChoiceId);
            _runner.ApplyChoice(part1, share, ctx);

            int fireDay = 10 + EventRunner.EmissaryFavorDelayDays;
            GameEvent fired = null;
            GameEvent runEvent = null;
            _runner.OnScheduledEventFired += (se, ge, c) => fired = ge;
            _runner.OnEventTriggered += (ev, c) => runEvent = ev;

            // Day before — no fire
            _runner.TickDay(fireDay - 1, ctx);
            Assert.That(fired, Is.Null);
            Assert.That(_runner.ScheduledEvents.Count, Is.EqualTo(1));

            // Correct day — Part 2 presents
            _runner.TickDay(fireDay, ctx);
            Assert.That(fired, Is.Not.Null);
            Assert.That(fired.id, Is.EqualTo(EventRunner.EmissaryReturnFavorId));
            Assert.That(runEvent, Is.SameAs(fired));
            Assert.That(_runner.ScheduledEvents.Count, Is.EqualTo(0));

            // Flag gate on event + TraitGate on search_first (Paranoid present)
            var available = EventRunner.GetAvailableChoices(fired, ctx);
            Assert.That(available.Any(c => c.ChoiceId == "accept_gift"), Is.True);
            Assert.That(available.Any(c => c.ChoiceId == "search_first"), Is.True,
                "Paranoid crew unlocks search_first on the favor return.");
        }

        [Test]
        public void LieChoice_SchedulesCaught_ParanoidAftermathChoices()
        {
            var crew = new List<Survivor> { MakeSurvivor("p1", RiskBiasTrait.Paranoid) };
            var ctx = MakeContext(7, crew, scavengerTrust: 0f);
            var part1 = _runner.FindInPool(EventRunner.EmissaryEventId);
            var lie = EventRunner.FindAvailableChoice(part1, ctx, EventRunner.EmissaryLieChoiceId);
            Assert.That(lie, Is.Not.Null);

            _runner.ApplyChoice(part1, lie, ctx);

            Assert.That(ctx.HasEventFlag(EventRunner.FlagLiedPurifierBroken), Is.True);
            Assert.That(_runner.ScheduledEvents[0].EventId, Is.EqualTo(EventRunner.EmissaryReturnCaughtId));
            Assert.That(_runner.ScheduledEvents[0].ExecuteOnDay,
                Is.EqualTo(7 + EventRunner.EmissaryCaughtDelayDays));

            GameEvent part2 = null;
            _runner.OnEventTriggered += (ev, c) => part2 = ev;
            _runner.TickDay(7 + EventRunner.EmissaryCaughtDelayDays, ctx);

            Assert.That(part2, Is.Not.Null);
            Assert.That(part2.id, Is.EqualTo(EventRunner.EmissaryReturnCaughtId));

            var available = EventRunner.GetAvailableChoices(part2, ctx);
            Assert.That(available.Any(c => c.ChoiceId == "double_down_lie"), Is.True);
            Assert.That(available.Any(c => c.ChoiceId == "offer_filter_help"), Is.False,
                "Medical choice hidden without Medical skill.");
            Assert.That(available.Any(c => c.ChoiceId == "admit_and_share"), Is.True);
        }

        [Test]
        public void LieAftermath_MissingFlag_SkipsPresentation()
        {
            // Schedule caught without the prerequisite flag (stale / tampered chain).
            _runner.ScheduleEvent(EventRunner.EmissaryReturnCaughtId, 20);
            var crew = new List<Survivor> { MakeSurvivor("r1", RiskBiasTrait.Realist) };
            var ctx = MakeContext(20, crew); // no flags

            GameEvent presented = null;
            GameEvent runEvent = null;
            _runner.OnScheduledEventFired += (se, ge, c) => presented = ge;
            _runner.OnEventTriggered += (ev, c) => runEvent = ev;

            _runner.TickDay(20, ctx);

            Assert.That(presented, Is.Null, "CanTrigger fail should pass null gameEvent.");
            Assert.That(runEvent, Is.Null, "Part 2 must not Run without required eventFlags.");
            Assert.That(_runner.ScheduledEvents.Count, Is.EqualTo(0), "Stale entry is dequeued.");
        }

        [Test]
        public void ResolveScheduleDay_PrefersRelativeDelay()
        {
            var effect = new EventEffect
            {
                ScheduleEventId = "x",
                ScheduleOnDay = 99,
                ScheduleDelayDays = 3
            };
            var ctx = new EventContext { CurrentDay = 10 };
            Assert.That(EventRunner.ResolveScheduleDay(effect, ctx), Is.EqualTo(13));

            effect.ScheduleDelayDays = 0;
            Assert.That(EventRunner.ResolveScheduleDay(effect, ctx), Is.EqualTo(99));
        }

        [Test]
        public void ScheduledQueue_SurvivesSaveLoad_RoundTrip()
        {
            _runner.ScheduleEvent(EventRunner.EmissaryReturnFavorId, 15, EventRunner.FlagSharedWaterWithEmissary);
            _runner.ScheduleEvent(EventRunner.EmissaryReturnCaughtId, 18, EventRunner.FlagLiedPurifierBroken);

            var gameState = new GameState();
            var saveSys = new SaveSystem(
                gameState,
                weatherSystem: null,
                temperatureSystem: null,
                needsSystem: null,
                radiationSystem: null,
                shelter: null,
                getSurvivors: () => new List<Survivor>(),
                itemLookup: null,
                moduleLookup: null,
                savesDir: _savesDir);
            saveSys.SetEventRunner(_runner);

            Assert.That(saveSys.Save("chain_slot"), Is.True);

            var runner2 = new EventRunner();
            runner2.SetPool(_pool);
            var saveSys2 = new SaveSystem(
                gameState,
                weatherSystem: null,
                temperatureSystem: null,
                needsSystem: null,
                radiationSystem: null,
                shelter: null,
                getSurvivors: () => new List<Survivor>(),
                itemLookup: null,
                moduleLookup: null,
                savesDir: _savesDir);
            saveSys2.SetEventRunner(runner2);

            Assert.That(saveSys2.Load("chain_slot"), Is.True);
            Assert.That(runner2.ScheduledEvents.Count, Is.EqualTo(2));
            Assert.That(runner2.ScheduledEvents[0].EventId, Is.EqualTo(EventRunner.EmissaryReturnFavorId));
            Assert.That(runner2.ScheduledEvents[0].ExecuteOnDay, Is.EqualTo(15));
            Assert.That(runner2.ScheduledEvents[0].OriginFlag, Is.EqualTo(EventRunner.FlagSharedWaterWithEmissary));
            Assert.That(runner2.ScheduledEvents[1].EventId, Is.EqualTo(EventRunner.EmissaryReturnCaughtId));
        }

        [Test]
        public void CreateEmissaryChain_RegistersAllParts()
        {
            Assert.That(_pool.Count, Is.EqualTo(5));
            Assert.That(_runner.FindInPool(EventRunner.EmissaryEventId), Is.Not.Null);
            Assert.That(_runner.FindInPool(EventRunner.EmissaryReturnFavorId), Is.Not.Null);
            Assert.That(_runner.FindInPool(EventRunner.EmissaryReturnCaughtId), Is.Not.Null);
            Assert.That(_runner.FindInPool(EventRunner.EmissaryReturnGrudgeId), Is.Not.Null);
            Assert.That(_runner.FindInPool(EventRunner.EmissaryReturnRaidWarningId), Is.Not.Null);
        }

        [Test]
        public void MedicalOnCaught_UnlocksFilterHelp()
        {
            var medic = MakeSurvivor("m1", RiskBiasTrait.Realist, medical: 0.7f);
            var ctx = MakeContext(12, new List<Survivor> { medic },
                flags: new Dictionary<string, bool>
                {
                    [EventRunner.FlagLiedPurifierBroken] = true
                });

            var part2 = _runner.FindInPool(EventRunner.EmissaryReturnCaughtId);
            Assert.That(part2.CanTrigger(ctx), Is.True);
            var available = EventRunner.GetAvailableChoices(part2, ctx);
            Assert.That(available.Any(c => c.ChoiceId == "offer_filter_help"), Is.True);
            Assert.That(available.Any(c => c.ChoiceId == "double_down_lie"), Is.False);
        }
    }
}
