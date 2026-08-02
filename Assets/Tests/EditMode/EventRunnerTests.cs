using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Events;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Inventory;

namespace AtomicWar.Tests.EditMode
{
    [TestFixture]
    public class EventRunnerTests
    {
        private const float Eps = 1e-4f;

        private GameEvent _testEvent;
        private EventChoice _choiceImmediate;
        private EventChoice _choiceWithDelayed;

        [SetUp]
        public void SetUp()
        {
            _testEvent = ScriptableObject.CreateInstance<GameEvent>();
            _testEvent.id = "event_irradiated_stranger";
            _testEvent.title = "Irradiated Stranger at the Hatch";
            _testEvent.bodyText = "Heavy knocks echo against the steel airlock.";
            _testEvent.weight = 1f;

            _choiceImmediate = new EventChoice
            {
                ChoiceId = "refuse_entry",
                Text = "Keep the hatch sealed.",
                MoraleDelta = -15f,
                Effects = new List<EventEffect>
                {
                    new EventEffect { SetWorldFlag = "stranger_turned_away", WorldFlagValue = true }
                }
            };

            _choiceWithDelayed = new EventChoice
            {
                ChoiceId = "open_hatch",
                Text = "Open the hatch and let them inside.",
                MoraleDelta = 10f,
                Effects = new List<EventEffect>
                {
                    new EventEffect { TargetNeed = "radiation", NeedDelta = 15f },
                    new EventEffect { SetWorldFlag = "stranger_sheltered", WorldFlagValue = true }
                },
                DelayedConsequence = new DelayedConsequence
                {
                    DelayHours = 24f,
                    Title = "Stranger's Gratitude",
                    Description = "The stranger leaves supplies before departing.",
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { ItemId = "canned_food", ItemAmount = 2 }
                    }
                }
            };

            _testEvent.choices = new List<EventChoice> { _choiceImmediate, _choiceWithDelayed };
        }

        [Test]
        public void EventRunner_FiresForcedEvent_AndAppliesChoiceImmediateEffects()
        {
            var survivor = new Survivor { Id = "s1", DisplayName = "Test" };
            survivor.Needs.Morale = 50f;
            survivor.RadiationDose = 0f;

            var inventory = new Inventory { Capacity = 10 };
            var context = new EventContext(survivor, new Shelter(), inventory);

            var runner = new EventRunner();
            runner.SetPool(new List<GameEvent> { _testEvent });

            bool eventFired = false;
            runner.OnEventTriggered += (ev, ctx) => eventFired = true;

            runner.Run(_testEvent, context);
            Assert.That(eventFired, Is.True);

            runner.ApplyChoice(_testEvent, _choiceWithDelayed, context);

            Assert.That(survivor.Needs.Morale, Is.EqualTo(60f).Within(Eps));
            Assert.That(survivor.RadiationDose, Is.EqualTo(15f).Within(Eps));
            Assert.That(context.GetFlag("stranger_sheltered"), Is.True);
            Assert.That(runner.ActiveConsequences.Count, Is.EqualTo(1));
        }

        [Test]
        public void EventRunner_DelayedConsequence_ResolvesAfterNTicks()
        {
            var survivor = new Survivor { Id = "s1" };
            var inventory = new Inventory { Capacity = 10 };
            var context = new EventContext(survivor, new Shelter(), inventory);

            var runner = new EventRunner();
            runner.ApplyChoice(_testEvent, _choiceWithDelayed, context);

            Assert.That(runner.ActiveConsequences.Count, Is.EqualTo(1));
            Assert.That(inventory.Count(new ItemDefinition { id = "canned_food" }), Is.EqualTo(0));

            bool resolved = false;
            runner.OnDelayedConsequenceResolved += (active, ctx) => resolved = true;

            // Tick 12 hours (halfway)
            runner.Tick(12f, context);
            Assert.That(resolved, Is.False);
            Assert.That(runner.ActiveConsequences.Count, Is.EqualTo(1));
            Assert.That(inventory.Count(new ItemDefinition { id = "canned_food" }), Is.EqualTo(0));

            // Tick remaining 12 hours (total 24h)
            runner.Tick(12f, context);
            Assert.That(resolved, Is.True);
            Assert.That(runner.ActiveConsequences.Count, Is.EqualTo(0));
            Assert.That(inventory.Count(new ItemDefinition { id = "canned_food" }), Is.EqualTo(2));
        }

        [Test]
        public void EventRunner_SelectsEvent_BasedOnWeightedConditions()
        {
            var survivor = new Survivor { Id = "s1" };
            var context = new EventContext(survivor);
            context.CurrentDay = 1;
            context.IsFalloutStorm = false;

            var stormEvent = ScriptableObject.CreateInstance<GameEvent>();
            stormEvent.id = "storm_event";
            stormEvent.weight = 10f;
            stormEvent.conditions = new EventConditions { RequireFalloutStorm = true };

            var normalEvent = ScriptableObject.CreateInstance<GameEvent>();
            normalEvent.id = "normal_event";
            normalEvent.weight = 1f;

            var runner = new EventRunner();
            runner.SetPool(new List<GameEvent> { stormEvent, normalEvent });

            var selected = runner.SelectEvent(context);
            Assert.That(selected, Is.EqualTo(normalEvent)); // Storm event invalid because IsFalloutStorm is false

            context.IsFalloutStorm = true;
            var selectedStorm = runner.SelectEvent(context);
            Assert.That(selectedStorm, Is.Not.Null);
        }
    }
}
