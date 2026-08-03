using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Core;

namespace AtomicWar.Tests.PlayMode
{
    [TestFixture]
    public class MoralDilemmaPlayModeTests
    {
        private MoralDilemmaSystem _dilemmaSystem;
        private Inventory _inventory;
        private List<Survivor> _survivors;

        [SetUp]
        public void SetUp()
        {
            _dilemmaSystem = new MoralDilemmaSystem();
            _inventory = new Inventory { Capacity = 10, MaxWeight = 100f };

            var s1 = new Survivor { Id = "normal_survivor", DisplayName = "Normal" };
            s1.Needs.Hunger = 95f;
            s1.Needs.Morale = 80f;

            var s2 = new Survivor { Id = "psychopath_survivor", DisplayName = "Psychopath" };
            s2.Needs.Hunger = 92f;
            s2.Needs.Morale = 80f;
            s2.Traits.Add("Psychopath");

            var s3 = new Survivor { Id = "survivalist_survivor", DisplayName = "Survivalist" };
            s3.Needs.Hunger = 91f;
            s3.Needs.Morale = 80f;
            s3.Traits.Add("Survivalist");

            var dead = new Survivor { Id = "deceased_survivor", DisplayName = "Deceased", State = SurvivorState.Dead };

            _survivors = new List<Survivor> { s1, s2, s3, dead };
        }

        [UnityTest]
        public IEnumerator CriticalHunger_WithZeroFood_TriggersMoralDilemma()
        {
            Assert.That(MoralDilemmaSystem.GetStoredFoodCount(_inventory), Is.EqualTo(0));

            bool triggered = _dilemmaSystem.CheckForDilemmaTrigger(_survivors, _inventory, day: 5);
            yield return null;

            Assert.That(triggered, Is.True, "Moral dilemma must trigger when Hunger >= 90 and shelter food is 0.");
            Assert.That(_dilemmaSystem.ActiveDilemma, Is.Not.Null);
            Assert.That(_dilemmaSystem.ActiveDilemma.CriticalHunger, Is.EqualTo(95f));
            Assert.That(_dilemmaSystem.ActiveDilemma.LivingSurvivorCount, Is.EqualTo(3));
            Assert.That(_dilemmaSystem.ActiveDilemma.DeadSurvivorCount, Is.EqualTo(1));
        }

        [UnityTest]
        public IEnumerator Butchering_RestoresFood_AppliesTraitMorale_And_InflictsTrauma()
        {
            _dilemmaSystem.CheckForDilemmaTrigger(_survivors, _inventory, day: 5);
            yield return null;

            var result = _dilemmaSystem.ResolveChoice(DesperateChoiceKind.Butchering, _survivors, _inventory);
            yield return null;

            Assert.That(result, Is.Not.Null);
            Assert.That(_dilemmaSystem.ActiveDilemma.IsResolved, Is.True);

            // Assert food restored to inventory
            Assert.That(MoralDilemmaSystem.GetStoredFoodCount(_inventory), Is.GreaterThan(0), "Butchering must restore food to inventory.");

            // Survivor 0 (Normal): -40 morale penalty (80 -> 40), grants cannibalism_trauma
            var normal = _survivors[0];
            Assert.That(normal.Needs.Morale, Is.EqualTo(40f).Within(1e-4f), "Normal survivor should suffer -40 morale penalty.");
            Assert.That(normal.HasTrauma("cannibalism_trauma"), Is.True, "Normal survivor should be granted cannibalism trauma.");

            // Survivor 1 (Psychopath): -20 morale penalty (80 -> 60, 50% reduced), NO trauma
            var psycho = _survivors[1];
            Assert.That(psycho.Needs.Morale, Is.EqualTo(60f).Within(1e-4f), "Psychopath survivor should suffer 50% reduced morale penalty (-20).");
            Assert.That(psycho.HasTrauma("cannibalism_trauma"), Is.False, "Psychopath survivor must NOT receive cannibalism trauma.");

            // Survivor 2 (Survivalist): -20 morale penalty (80 -> 60, 50% reduced), grants cannibalism_trauma
            var survivalist = _survivors[2];
            Assert.That(survivalist.Needs.Morale, Is.EqualTo(60f).Within(1e-4f), "Survivalist survivor should suffer 50% reduced morale penalty (-20).");
            Assert.That(survivalist.HasTrauma("cannibalism_trauma"), Is.True, "Survivalist survivor receives trauma unless Psychopath.");
        }
    }
}
