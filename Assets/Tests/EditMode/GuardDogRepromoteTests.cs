using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Survivors;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// REPROMOTE-Pet-001 — HatchDefense alerts Pet_GuardDog on raid start.
    /// </summary>
    [TestFixture]
    public class GuardDogRepromoteTests
    {
        [Test]
        public void GuardDog_Alert_ReturnsFalseWhenMalnourished()
        {
            var dog = new Pet_GuardDog("dog_1");
            // Never fed → TickDay malnourishes.
            dog.TickDay("bunker");
            Assert.IsFalse(dog.CanFightInRaid());
            Assert.IsFalse(dog.Alert("bunker"));
        }

        [Test]
        public void GuardDog_Alert_ReturnsTrueWhenFedMeat()
        {
            var dog = new Pet_GuardDog("dog_1");
            dog.Feed("bunker", "canned_meat", 2);
            dog.TickDay("bunker");
            Assert.IsTrue(dog.CanFightInRaid());
            Assert.IsTrue(dog.Alert("bunker"));
        }

        [Test]
        public void HatchDefense_ResolveRaid_AddsDogBonusWhenAlertReady()
        {
            var dog = new Pet_GuardDog("dog_1");
            dog.Feed("bunker", "meat_ration", 2);
            dog.TickDay("bunker");

            var hatch = new HatchDefenseSystem(
                getSurvivors: () => new List<Survivor>(),
                getDay: () => 40);
            hatch.TryAlertGuardDog = () => dog.Alert("bunker");

            var raid = new RaidEvent
            {
                FactionId = "scavenger_camp",
                Strength = 5f,
                Day = 40,
                Trigger = RaidTrigger.Forced,
                Message = "test raid"
            };
            var result = hatch.ResolveRaid(raid, ignoreDayGate: true);
            Assert.IsTrue(result.Launched);
            Assert.That(result.GuardBonusApplied, Is.GreaterThanOrEqualTo(HatchDefenseSystem.GuardDogFightBonus),
                "Fed guard dog must contribute GuardDogFightBonus on raid start");
            Assert.That(result.WeaponPower, Is.GreaterThanOrEqualTo(HatchDefenseSystem.GuardDogFightBonus),
                "Dog fight bonus is applied to WeaponPower");
        }
    }
}
