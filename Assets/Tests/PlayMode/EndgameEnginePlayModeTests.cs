using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using AtomicWar._Game.Core;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Shelter.Modules;
using AtomicWar._Game.Survivors;

namespace AtomicWar.Tests.PlayMode
{
    [TestFixture]
    public class EndgameEnginePlayModeTests
    {
        [UnityTest]
        public IEnumerator AllSurvivorsDeceased_TriggersDefeatCondition()
        {
            var engine = new EndgameEngine(GameModeKind.Story, 120);
            var survivors = new List<Survivor>
            {
                new Survivor { Id = "sv_1", DisplayName = "Elena" },
                new Survivor { Id = "sv_2", DisplayName = "Marcus" }
            };

            foreach (var sv in survivors)
            {
                sv.State = SurvivorState.Dead;
            }

            bool eventFired = false;
            CampaignEndedEvent recordedEvt = default;
            engine.OnCampaignEnded += evt =>
            {
                eventFired = true;
                recordedEvt = evt;
            };

            bool halted = engine.Evaluate(
                currentDay: 15,
                survivors: survivors,
                shelter: null,
                isExtractionUnlocked: false,
                isHydroponicsOperational: false,
                totalDeathsRecorded: 2);

            Assert.That(halted, Is.True, "Evaluate should return true on terminal endgame condition.");
            Assert.That(eventFired, Is.True, "OnCampaignEnded event must fire when all survivors are dead.");
            Assert.That(recordedEvt.ConditionKind, Is.EqualTo(EndgameConditionKind.AllSurvivorsDeceased));
            Assert.That(recordedEvt.IsVictory, Is.False);
            Assert.That(engine.Result.IsDefeat, Is.True);
            yield return null;
        }

        [UnityTest]
        public IEnumerator RescueExtractionSuccess_Day60_TriggersVictoryCondition()
        {
            var engine = new EndgameEngine(GameModeKind.Story, 120);
            var survivors = new List<Survivor>
            {
                new Survivor { Id = "sv_1", DisplayName = "Elena" }
            };

            bool eventFired = false;
            CampaignEndedEvent recordedEvt = default;
            engine.OnCampaignEnded += evt =>
            {
                eventFired = true;
                recordedEvt = evt;
            };

            bool halted = engine.Evaluate(
                currentDay: 60,
                survivors: survivors,
                shelter: null,
                isExtractionUnlocked: true,
                isHydroponicsOperational: false,
                totalDeathsRecorded: 0);

            Assert.That(halted, Is.True, "Evaluate should return true on Day 60 extraction victory.");
            Assert.That(eventFired, Is.True, "OnCampaignEnded event must fire on Day 60 rescue extraction.");
            Assert.That(recordedEvt.ConditionKind, Is.EqualTo(EndgameConditionKind.RescueExtractionSuccess));
            Assert.That(recordedEvt.IsVictory, Is.True);
            Assert.That(engine.Result.IsVictory, Is.True);
            yield return null;
        }

        [UnityTest]
        public IEnumerator BunkerStructuralCollapse_ZeroFilterAndShielding_TriggersDefeatCondition()
        {
            var engine = new EndgameEngine(GameModeKind.Story, 120);
            var survivors = new List<Survivor>
            {
                new Survivor { Id = "sv_1", DisplayName = "Elena" }
            };

            var shelter = new Shelter();
            shelter.AddModule(new ShelterModuleInstance("air_filtration", 1) { FilterHealth = 0f });
            shelter.AddModule(new ShelterModuleInstance("radiation_shielding", 1) { FilterHealth = 0f });

            bool eventFired = false;
            CampaignEndedEvent recordedEvt = default;
            engine.OnCampaignEnded += evt =>
            {
                eventFired = true;
                recordedEvt = evt;
            };

            bool halted = engine.Evaluate(
                currentDay: 20,
                survivors: survivors,
                shelter: shelter,
                isExtractionUnlocked: false,
                isHydroponicsOperational: false,
                totalDeathsRecorded: 0);

            Assert.That(halted, Is.True, "Evaluate should return true on bunker structural collapse.");
            Assert.That(eventFired, Is.True, "OnCampaignEnded event must fire on structural collapse.");
            Assert.That(recordedEvt.ConditionKind, Is.EqualTo(EndgameConditionKind.BunkerStructuralCollapse));
            Assert.That(recordedEvt.IsVictory, Is.False);
            Assert.That(engine.Result.IsDefeat, Is.True);
            yield return null;
        }

        [UnityTest]
        public IEnumerator LongTermSelfSufficiency_Day100ZeroDeathsHydroponics_TriggersVictoryCondition()
        {
            var engine = new EndgameEngine(GameModeKind.Story, 120);
            var survivors = new List<Survivor>
            {
                new Survivor { Id = "sv_1", DisplayName = "Elena" }
            };

            bool eventFired = false;
            CampaignEndedEvent recordedEvt = default;
            engine.OnCampaignEnded += evt =>
            {
                eventFired = true;
                recordedEvt = evt;
            };

            bool halted = engine.Evaluate(
                currentDay: 100,
                survivors: survivors,
                shelter: null,
                isExtractionUnlocked: false,
                isHydroponicsOperational: true,
                totalDeathsRecorded: 0);

            Assert.That(halted, Is.True, "Evaluate should return true on 100-day self-sufficiency victory.");
            Assert.That(eventFired, Is.True, "OnCampaignEnded event must fire on self-sufficiency victory.");
            Assert.That(recordedEvt.ConditionKind, Is.EqualTo(EndgameConditionKind.LongTermSelfSufficiency));
            Assert.That(recordedEvt.IsVictory, Is.True);
            Assert.That(engine.Result.IsVictory, Is.True);
            yield return null;
        }

        [UnityTest]
        public IEnumerator CampaignResult_CustomAndExpertModes_ConfigureCorrectParameters()
        {
            var storyEngine = new EndgameEngine(GameModeKind.Story);
            Assert.That(storyEngine.Result.TargetDurationDays, Is.EqualTo(120));
            Assert.That(storyEngine.Result.StartCalendarDate.Month, Is.EqualTo(8));
            Assert.That(storyEngine.Result.StartCalendarDate.Day, Is.EqualTo(25));

            var expertEngine = new EndgameEngine(GameModeKind.Expert);
            Assert.That(expertEngine.Result.TargetDurationDays, Is.EqualTo(180));

            var custom60Engine = new EndgameEngine(GameModeKind.Custom, 60);
            Assert.That(custom60Engine.Result.TargetDurationDays, Is.EqualTo(60));

            var custom180Engine = new EndgameEngine(GameModeKind.Custom, 180);
            Assert.That(custom180Engine.Result.TargetDurationDays, Is.EqualTo(180));

            yield return null;
        }
    }
}
