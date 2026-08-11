using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Data;
using System.Collections.Generic;

using AtomicWar._Game.Factions;

namespace AtomicWar.Tests.EditMode
{
    public class FixedMapAndVisitorDeckTests
    {
        [Test]
        public void VisitorRNGSystem_Initializes20FixedLocations_WithRandomVisitors()
        {
            var visitorSystem = new VisitorRNGSystem();
            visitorSystem.InitializeMap(seed: 12345);

            Assert.AreEqual(20, visitorSystem.Nodes.Count);
            Assert.IsTrue(visitorSystem.Nodes.ContainsKey("general_hospital"));
            Assert.IsFalse(string.IsNullOrEmpty(visitorSystem.Nodes["general_hospital"].assignedVisitorCardId));
        }

        [Test]
        public void LocationStateRuinSystem_AppliesModifiersCorrectly()
        {
            var ruinSystem = new LocationStateRuinSystem();
            var node = new FixedNodeState { locationId = "general_hospital" };

            var halfBurned = ruinSystem.ApplyRuinModifier(node, LocationStateModifier.HalfBurned);
            Assert.AreEqual(0.5f, halfBurned.ashCharcoalLootRatio);
            Assert.AreEqual(20f, halfBurned.ambientHeatDelta);

            var exploded = ruinSystem.ApplyRuinModifier(node, LocationStateModifier.Exploded);
            Assert.IsTrue(exploded.hasMassiveDebris);
            Assert.AreEqual(4.0f, exploded.radiationMultiplier);

            var abandoned = ruinSystem.ApplyRuinModifier(node, LocationStateModifier.Abandoned);
            Assert.IsTrue(abandoned.forceZeroNpcs);
            Assert.AreEqual("visitor_abandoned", node.assignedVisitorCardId);
        }

        [Test]
        public void SkirmishEncounter_SimulatesBackgroundWait_GeneratesCorpsesAndDepletesAmmo()
        {
            var skirmishEngine = new SkirmishEncounter();
            var state = skirmishEngine.CreateSkirmish("military_base", "military", "terrorists", countA: 5, countB: 5);

            Assert.IsFalse(state.isResolved);
            var outcome = skirmishEngine.ExecuteAction("military_base", SkirmishPlayerAction.Wait, new System.Random(42));

            Assert.IsNotNull(outcome);
            Assert.IsTrue(state.isResolved);
            Assert.IsTrue(outcome.totalCorpses > 0);
            Assert.IsTrue(outcome.totalAmmoWasted > 0);
        }

        [Test]
        public void NPC_MilitaryPatrol_TollMechanic_FunctionsCorrectly()
        {
            var patrol = new NPC_MilitaryPatrol();
            patrol.InitiateEncounter(playerIsArmed: false);

            int food = 3;
            int meds = 1;
            bool paid = patrol.PayToll(ref food, ref meds);

            Assert.IsTrue(paid);
            Assert.IsFalse(patrol.State.isHostileToPlayer);
            Assert.AreEqual(1, food);
        }

        [Test]
        public void NPC_Conscripts_SurrenderMechanic_TriggersOnKill()
        {
            var conscripts = new NPC_Conscripts();
            conscripts.KillOneConscript();

            Assert.IsTrue(conscripts.State.isSurrendered);
            Assert.AreEqual(3, conscripts.State.riflesDroppedCount);
        }

        [Test]
        public void NPC_SpecOps_Flashbang_StunsPlayerAndDrainsStamina()
        {
            var specOps = new NPC_SpecOps();
            bool used = specOps.TryUseFlashbang(out float stun, out float staminaDrain);

            Assert.IsTrue(used);
            Assert.AreEqual(3.0f, stun);
            Assert.AreEqual(40.0f, staminaDrain);
        }

        [Test]
        public void NPC_PsychopathPair_FrenzyEnrage_TriggersOnPartnerDeath()
        {
            var pair = new NPC_PsychopathPair();
            pair.KillSniper();

            Assert.IsTrue(pair.State.isFrenzyActive);
            Assert.AreEqual(2.0f, pair.State.damageMultiplier);
            Assert.IsTrue(pair.State.isImmuneToPain);
        }

        [Test]
        public void Visitor_AbandonedState_GeneratesMaxStructuralHazards()
        {
            var abandoned = new Visitor_AbandonedState();
            var hazards = abandoned.GenerateStructuralHazards();

            Assert.AreEqual(0, abandoned.Effect.npcCount);
            Assert.AreEqual(3.0f, abandoned.Effect.structuralHazardMultiplier);
            Assert.Contains("cave_in", hazards);
        }
    }
}
