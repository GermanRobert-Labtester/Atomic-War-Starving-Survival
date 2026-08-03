using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using AtomicWar._Game.Core;
using AtomicWar._Game.Events;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Inventory;

namespace AtomicWar.Tests.PlayMode
{
    /// <summary>
    /// PlayMode tests for Prompt #43: Delayed Narrative Chains / Silent Knock.
    ///
    /// Tests:
    /// 1. ScheduleEvent enqueues correctly; TickDay fires only on matching day.
    /// 2. Part 1 "Open" choice sets stranger_inside flag and schedules Part 2a on Day 37.
    /// 3. Part 1 "Ignore" choice sets stranger_ignored flag and schedules Part 2b on Day 38.
    /// 4. TickDay(37) fires Part 2a when stranger_inside is set.
    /// 5. Giving irradiated water sets the irradiated flag, Part 3 resolves as ambush.
    /// 6. Giving clean water sets the clean flag, Part 3 resolves as real cache.
    /// 7. Scheduled queue survives CaptureScheduledState / RestoreScheduledState round-trip.
    /// 8. Duplicate ScheduleEvent calls for same id+day are silently ignored.
    /// </summary>
    [TestFixture]
    public class NarrativeChainPlayModeTests
    {
        private EventRunner _runner;
        private EventContext _ctx;
        private Survivor _survivor;

        // Helper: build a minimal GameEvent with a given id
        private GameEvent MakeEvent(string id, int minDay = 1)
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = id;
            ev.minDay = minDay;
            return ev;
        }

        // Helper: build a choice with one scheduleEvent effect
        private EventChoice MakeSchedulingChoice(string choiceId, string eventId, int day,
            string flagId = null, bool flagValue = true)
        {
            var choice = new EventChoice { ChoiceId = choiceId };
            var effect = new EventEffect
            {
                ScheduleEventId = eventId,
                ScheduleOnDay   = day
            };
            if (!string.IsNullOrEmpty(flagId))
            {
                effect.SetWorldFlag   = flagId;
                effect.WorldFlagValue = flagValue;
            }
            choice.Effects.Add(effect);
            return choice;
        }

        [SetUp]
        public void SetUp()
        {
            _runner = new EventRunner();
            _survivor = new Survivor { Id = "sv_test", DisplayName = "Tester" };
            _ctx = new EventContext(_survivor);
        }

        // ─────────────────────────────────────────────────────────────────────
        // 1. ScheduleEvent + TickDay basic mechanics
        // ─────────────────────────────────────────────────────────────────────

        [UnityTest]
        public IEnumerator ScheduleEvent_TickDay_FiresOnCorrectDay()
        {
            var ev = MakeEvent("silent_knock_part1", 35);
            _runner.SetPool(new List<GameEvent> { ev });
            _runner.ScheduleEvent("silent_knock_part1", 35);

            Assert.AreEqual(1, _runner.ScheduledEvents.Count, "Queue should hold 1 entry.");

            // Day 34 — should not fire
            _runner.TickDay(34, _ctx);
            Assert.AreEqual(1, _runner.ScheduledEvents.Count, "Should not fire before the scheduled day.");

            bool fired = false;
            _runner.OnScheduledEventFired += (se, ge, c) => fired = true;

            // Day 35 — should fire and dequeue
            _runner.TickDay(35, _ctx);
            yield return null;

            Assert.IsTrue(fired, "OnScheduledEventFired should have fired on Day 35.");
            Assert.AreEqual(0, _runner.ScheduledEvents.Count, "Queue should be empty after firing.");

            ScriptableObject.DestroyImmediate(ev);
        }

        // ─────────────────────────────────────────────────────────────────────
        // 2. Part 1 "Open" choice schedules Part 2a (Day 37) + sets flag
        // ─────────────────────────────────────────────────────────────────────

        [UnityTest]
        public IEnumerator Part1_OpenChoice_SchedulesPart2a_OnDay37()
        {
            var part1 = MakeEvent("silent_knock_part1", 35);
            part1.choices.Add(MakeSchedulingChoice(
                choiceId:  "open_hatch",
                eventId:   "silent_knock_part2a_wakes",
                day:       37,
                flagId:    NarrativeChainEngine.FlagStrangerInside,
                flagValue: true
            ));
            _runner.SetPool(new List<GameEvent> { part1 });

            _runner.ApplyChoice(part1, part1.choices[0], _ctx);
            yield return null;

            Assert.IsTrue(_ctx.GetFlag(NarrativeChainEngine.FlagStrangerInside),
                "stranger_inside flag should be set after 'Open' choice.");
            Assert.AreEqual(1, _runner.ScheduledEvents.Count,
                "One scheduled event should be in the queue.");
            Assert.AreEqual("silent_knock_part2a_wakes", _runner.ScheduledEvents[0].EventId);
            Assert.AreEqual(37, _runner.ScheduledEvents[0].ExecuteOnDay);

            ScriptableObject.DestroyImmediate(part1);
        }

        // ─────────────────────────────────────────────────────────────────────
        // 3. Part 1 "Ignore" choice schedules Part 2b (Day 38) + sets flag
        // ─────────────────────────────────────────────────────────────────────

        [UnityTest]
        public IEnumerator Part1_IgnoreChoice_SchedulesPart2b_OnDay38()
        {
            var part1 = MakeEvent("silent_knock_part1", 35);
            part1.choices.Add(MakeSchedulingChoice(
                choiceId:  "ignore_hatch",
                eventId:   "silent_knock_part2b_scraping",
                day:       38,
                flagId:    NarrativeChainEngine.FlagStrangerIgnored,
                flagValue: true
            ));
            _runner.SetPool(new List<GameEvent> { part1 });

            _runner.ApplyChoice(part1, part1.choices[0], _ctx);
            yield return null;

            Assert.IsTrue(_ctx.GetFlag(NarrativeChainEngine.FlagStrangerIgnored),
                "stranger_ignored flag should be set after 'Ignore' choice.");
            Assert.AreEqual(1, _runner.ScheduledEvents.Count);
            Assert.AreEqual("silent_knock_part2b_scraping", _runner.ScheduledEvents[0].EventId);
            Assert.AreEqual(38, _runner.ScheduledEvents[0].ExecuteOnDay);

            ScriptableObject.DestroyImmediate(part1);
        }

        // ─────────────────────────────────────────────────────────────────────
        // 4. TickDay(37) fires Part 2a when stranger_inside is set
        // ─────────────────────────────────────────────────────────────────────

        [UnityTest]
        public IEnumerator TickDay37_FiresPart2a_WhenStrangerInsideIsSet()
        {
            var part2a = MakeEvent("silent_knock_part2a_wakes", 37);
            _runner.SetPool(new List<GameEvent> { part2a });
            _runner.ScheduleEvent("silent_knock_part2a_wakes", 37);
            _ctx.SetFlag(NarrativeChainEngine.FlagStrangerInside, true);

            ScheduledEvent? firedScheduled = null;
            GameEvent firedGameEvent = null;
            _runner.OnScheduledEventFired += (se, ge, c) =>
            {
                firedScheduled = se;
                firedGameEvent = ge;
            };

            _runner.TickDay(37, _ctx);
            yield return null;

            Assert.IsNotNull(firedScheduled, "Part 2a should have fired on Day 37.");
            Assert.AreEqual("silent_knock_part2a_wakes", firedScheduled.Value.EventId);
            Assert.AreEqual(part2a, firedGameEvent, "Correct GameEvent reference should be passed.");
            Assert.AreEqual(0, _runner.ScheduledEvents.Count, "Queue should be empty after firing.");

            ScriptableObject.DestroyImmediate(part2a);
        }

        // ─────────────────────────────────────────────────────────────────────
        // 5. Irradiated water choice → ambush encounter in Part 3
        // ─────────────────────────────────────────────────────────────────────

        [UnityTest]
        public IEnumerator IrradiatedWater_GivenToStranger_ResolvesPart3AsAmbush()
        {
            // Simulate Part 2a "give irradiated water" effect
            _ctx.SetFlag(NarrativeChainEngine.FlagStrangerInside, true);
            _ctx.SetFlag(NarrativeChainEngine.FlagGivenIrradiatedWater, true);
            _ctx.SetFlag(NarrativeChainEngine.FlagStrangerHasCoordinates, true);

            yield return null;

            var outcome = NarrativeChainEngine.EvaluateOutcome(_ctx);
            Assert.AreEqual(StrangerCacheOutcome.FactionAmbush, outcome,
                "Irradiated water given → outcome should be FactionAmbush.");

            var encounter = NarrativeChainEngine.BuildOutcomeEncounter(outcome);
            Assert.IsNotNull(encounter, "Ambush encounter SO should be built.");
            Assert.AreEqual(NarrativeChainEngine.EncounterIdAmbush, encounter.id);
            Assert.AreEqual(EncounterCategory.Combat, encounter.category,
                "Ambush should be a Combat encounter.");

            ScriptableObject.DestroyImmediate(encounter);
        }

        // ─────────────────────────────────────────────────────────────────────
        // 6. Clean water choice → real cache encounter in Part 3
        // ─────────────────────────────────────────────────────────────────────

        [UnityTest]
        public IEnumerator CleanWater_GivenToStranger_ResolvesPart3AsRealCache()
        {
            _ctx.SetFlag(NarrativeChainEngine.FlagStrangerInside, true);
            _ctx.SetFlag(NarrativeChainEngine.FlagGivenCleanWater, true);
            _ctx.SetFlag(NarrativeChainEngine.FlagStrangerHasCoordinates, true);

            yield return null;

            var outcome = NarrativeChainEngine.EvaluateOutcome(_ctx);
            Assert.AreEqual(StrangerCacheOutcome.RealCache, outcome,
                "Clean water given → outcome should be RealCache.");

            var encounter = NarrativeChainEngine.BuildOutcomeEncounter(outcome);
            Assert.IsNotNull(encounter, "Cache encounter SO should be built.");
            Assert.AreEqual(NarrativeChainEngine.EncounterIdRealCache, encounter.id);
            Assert.AreEqual(EncounterCategory.Discovery, encounter.category,
                "Real cache should be a Discovery encounter.");

            // Loot choice should provide canned_food
            Assert.AreEqual(1, encounter.choices.Count);
            Assert.AreEqual(1, encounter.choices[0].Effects.Count);
            Assert.AreEqual("canned_food", encounter.choices[0].Effects[0].ItemId);
            Assert.AreEqual(4, encounter.choices[0].Effects[0].ItemAmount);

            ScriptableObject.DestroyImmediate(encounter);
        }

        // ─────────────────────────────────────────────────────────────────────
        // 7. Save/load round-trip preserves the scheduled queue
        // ─────────────────────────────────────────────────────────────────────

        [UnityTest]
        public IEnumerator ScheduledQueue_SaveLoad_RoundTrip_Preserves_Entries()
        {
            _runner.ScheduleEvent("silent_knock_part2a_wakes", 37, "stranger_inside");
            _runner.ScheduleEvent("silent_knock_part3_coordinates", 39);

            yield return null;

            var save = _runner.CaptureScheduledState();
            Assert.AreEqual(2, save.Queue.Length, "Save should capture 2 scheduled events.");

            var newRunner = new EventRunner();
            newRunner.RestoreScheduledState(save);

            Assert.AreEqual(2, newRunner.ScheduledEvents.Count, "Restored queue should have 2 entries.");
            Assert.AreEqual("silent_knock_part2a_wakes", newRunner.ScheduledEvents[0].EventId);
            Assert.AreEqual(37, newRunner.ScheduledEvents[0].ExecuteOnDay);
            Assert.AreEqual("stranger_inside", newRunner.ScheduledEvents[0].OriginFlag);
            Assert.AreEqual("silent_knock_part3_coordinates", newRunner.ScheduledEvents[1].EventId);
            Assert.AreEqual(39, newRunner.ScheduledEvents[1].ExecuteOnDay);
        }

        // ─────────────────────────────────────────────────────────────────────
        // 8. Duplicate ScheduleEvent calls for same id+day are ignored
        // ─────────────────────────────────────────────────────────────────────

        [UnityTest]
        public IEnumerator DuplicateScheduleEvent_IsIgnored()
        {
            _runner.ScheduleEvent("silent_knock_part2a_wakes", 37);
            _runner.ScheduleEvent("silent_knock_part2a_wakes", 37); // duplicate
            _runner.ScheduleEvent("silent_knock_part2a_wakes", 37); // duplicate again

            yield return null;

            Assert.AreEqual(1, _runner.ScheduledEvents.Count,
                "Duplicate scheduling of same event+day should be silently ignored.");
        }
    }
}
