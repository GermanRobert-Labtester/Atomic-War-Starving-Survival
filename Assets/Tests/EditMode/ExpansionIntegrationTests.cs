using System;
using NUnit.Framework;
using UnityEngine;
using Random = System.Random;
using AtomicWar._Game.Survivors;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Integration tests spanning multiple Phase 0-8 systems to verify
    /// cross-system interactions work correctly.
    /// </summary>
    [TestFixture]
    public class ExpansionIntegrationTests
    {
        // ── Trauma Bond System ───────────────────────────────────────

        [Test]
        public void TraumaBond_SharedHazard_FormsBond()
        {
            var system = new TraumaBondSystem
            {
                GetDay = () => 10,
                Rng = new Random(42)
            };
            var svA = new Survivor { Id = "a" };
            var svB = new Survivor { Id = "b" };

            var participants = new System.Collections.Generic.List<Survivor> { svA, svB };
            system.OnSharedHazardEndured(participants, "fallout_storm");

            Assert.AreEqual(1, svA.TraumaBonds.Count);
            Assert.AreEqual("b", svA.TraumaBonds[0].BondedSurvivorId);
            Assert.AreEqual(TraumaBondSystem.BondStrengthPerSharedHazard,
                svA.TraumaBonds[0].BondStrength, 0.01f);
        }

        [Test]
        public void TraumaBond_BondDecaysOverTime()
        {
            var system = new TraumaBondSystem
            {
                GetDay = () => 10,
                Rng = new Random(42)
            };
            var svA = new Survivor { Id = "a" };
            var svB = new Survivor { Id = "b" };

            var participants = new System.Collections.Generic.List<Survivor> { svA, svB };
            system.OnSharedHazardEndured(participants, "fallout_storm");

            float initialStrength = svA.TraumaBonds[0].BondStrength;

            // Decay for 10 days
            system.Tick(svA, 240f);

            float decayedStrength = svA.TraumaBonds[0].BondStrength;
            Assert.Less(decayedStrength, initialStrength,
                "Bond should decay after 10 days without shared activity");
        }

        // ── Moral Branching System ────────────────────────────────────

        [Test]
        public void MoralBranching_FiveEmpathyChoices_BecomesBurdenedCompassion()
        {
            var system = new MoralBranchingSystem();
            var survivor = new Survivor { Id = "s1" };

            // Register 5 empathy-driven choices
            for (int i = 0; i < 5; i++)
                system.RegisterMoralChoice(survivor, isEmpathyChoice: true);

            Assert.AreEqual(MoralBranchDirection.BurdenedCompassion,
                survivor.BranchDirection);
            Assert.Greater(survivor.BurdenedCompassionLevel, 0.5f);
        }

        [Test]
        public void MoralBranching_FivePragmatismChoices_BecomesNumbedResilience()
        {
            var system = new MoralBranchingSystem();
            var survivor = new Survivor { Id = "s1" };

            for (int i = 0; i < 5; i++)
                system.RegisterMoralChoice(survivor, isEmpathyChoice: false);

            Assert.AreEqual(MoralBranchDirection.NumbedResilience,
                survivor.BranchDirection);
            Assert.Greater(survivor.NumbedResilienceLevel, 0.5f);
        }

        [Test]
        public void MoralBranching_AlreadyBranched_NoChange()
        {
            var system = new MoralBranchingSystem();
            var survivor = new Survivor
            {
                Id = "s1",
                BranchDirection = MoralBranchDirection.BurdenedCompassion,
                BurdenedCompassionLevel = 0.8f,
                MoralChoiceCount = 5
            };

            // Try to register another choice
            system.RegisterMoralChoice(survivor, isEmpathyChoice: false);

            // Should remain BurdenedCompassion
            Assert.AreEqual(MoralBranchDirection.BurdenedCompassion,
                survivor.BranchDirection);
        }

        // ── Chemical Dependency System ────────────────────────────────

        [Test]
        public void ChemicalDependency_Consumption_BuildsDependency()
        {
            var system = new ChemicalDependencySystem { Rng = new Random(42) };
            var survivor = new Survivor { Id = "s1" };

            system.OnSubstanceConsumed(survivor, "morphine",
                ChemicalDependencyKind.Opioid);

            Assert.AreEqual(1, survivor.ChemicalDependencies.Count);
            Assert.AreEqual("morphine", survivor.ChemicalDependencies[0].ItemId);
            Assert.Greater(survivor.ChemicalDependencies[0].DependencyLevel, 0f);
        }

        [Test]
        public void ChemicalDependency_ColdTurkey_AppliesCraftingPenalty()
        {
            float craftingPenalty = 0f;
            var system = new ChemicalDependencySystem
            {
                ApplyCraftingPenalty = (sv, p) => craftingPenalty = p,
                Rng = new Random(42)
            };
            var survivor = new Survivor { Id = "s1" };

            // Build dependency first
            system.OnSubstanceConsumed(survivor, "morphine",
                ChemicalDependencyKind.Opioid);
            // Boost to threshold
            survivor.ChemicalDependencies[0] = new ChemicalDependency(
                "morphine", 0.5f, ChemicalDependencyKind.Opioid);

            // Begin cold turkey
            system.BeginColdTurkey(survivor, "morphine");

            // Tick withdrawal
            system.Tick(survivor, 1f);

            Assert.AreEqual(
                ChemicalDependencySystem.ColdTurkeyTremorCraftingPenalty,
                craftingPenalty, 0.01f);
        }

        // ── Guilt Insomnia System ─────────────────────────────────────

        [Test]
        public void GuiltInsomnia_RecordGuilt_IncreasesSeverity()
        {
            var system = new GuiltInsomniaSystem
            {
                GetDay = () => 5,
                Rng = new Random(42)
            };
            var survivor = new Survivor { Id = "s1" };

            system.RecordGuilt(survivor, "cut_ration", 0.8f);

            Assert.AreEqual(1, survivor.GuiltSources.Count);
            Assert.AreEqual(0.8f, survivor.GuiltInsomniaSeverity, 0.01f);
        }

        [Test]
        public void GuiltInsomnia_SleepQualityMultiplier_PenalizedByGuilt()
        {
            var system = new GuiltInsomniaSystem
            {
                GetDay = () => 5,
                Rng = new Random(42)
            };
            var survivor = new Survivor { Id = "s1" };

            system.RecordGuilt(survivor, "execute", 0.9f);

            float multiplier = system.GetSleepQualityMultiplier(survivor);

            // 0.9 * 0.5 = 0.45 penalty → multiplier = 1 - 0.45 = 0.55
            Assert.Less(multiplier, 0.6f,
                $"Sleep quality should be severely penalized, got {multiplier}");
        }

        // ── Ideological Friction ──────────────────────────────────────

        [Test]
        public void IdeologicalFriction_ConflictingProfiles_ReturnsPenalty()
        {
            var system = new IdeologicalFrictionSystem { Rng = new Random(42) };
            var svA = new Survivor
            {
                Id = "a",
                BeliefProfileId = "military_discipline"
            };
            var svB = new Survivor
            {
                Id = "b",
                BeliefProfileId = "pragmatic_individualism"
            };

            float multiplier = system.GetRoommateCompatibilityMultiplier(svA, svB);

            Assert.Less(multiplier, 1f,
                "Conflicting beliefs should reduce sleep quality");
        }

        [Test]
        public void IdeologicalFriction_SameProfile_ReturnsSynergy()
        {
            var system = new IdeologicalFrictionSystem { Rng = new Random(42) };
            var svA = new Survivor
            {
                Id = "a",
                BeliefProfileId = "collectivist_solidarity"
            };
            var svB = new Survivor
            {
                Id = "b",
                BeliefProfileId = "collectivist_solidarity"
            };

            float multiplier = system.GetRoommateCompatibilityMultiplier(svA, svB);

            Assert.Greater(multiplier, 1f,
                "Same beliefs should improve sleep quality");
        }

        // ── Leadership System ─────────────────────────────────────────

        [Test]
        public void Leadership_DesignateLeader_SetsLeaderFlags()
        {
            var system = new LeadershipSystem
            {
                GetSurvivors = () => new System.Collections.Generic.List<Survivor>
                {
                    new Survivor { Id = "leader" }
                }
            };
            var leader = new Survivor { Id = "leader" };

            bool result = system.DesignateLeader(leader);

            Assert.IsTrue(result);
            Assert.IsTrue(leader.IsDesignatedLeader);
            Assert.AreEqual("leader", system.CurrentLeaderId);
        }

        [Test]
        public void Leadership_WitnessDeath_AccumulatesStress()
        {
            var system = new LeadershipSystem
            {
                GetSurvivors = () => new System.Collections.Generic.List<Survivor>
                {
                    new Survivor { Id = "leader" },
                    new Survivor { Id = "follower" }
                }
            };
            var leader = new Survivor { Id = "leader" };
            system.DesignateLeader(leader);

            var dead = new Survivor { Id = "follower" };
            system.OnSurvivorDied(dead);

            Assert.AreEqual(LeadershipSystem.LeaderStressPerDeath,
                leader.LeaderStressAccumulation, 0.01f);
            Assert.AreEqual(1, leader.LeaderDeathsWitnessed);
        }

        // ── Ration Conflict System ────────────────────────────────────

        [Test]
        public void RationConflict_UnequalAllocation_BuildsResentment()
        {
            var survivorA = new Survivor { Id = "a" };
            var survivorB = new Survivor { Id = "b" };
            var survivors = new System.Collections.Generic.List<Survivor>
            {
                survivorA, survivorB
            };

            var system = new RationConflictSystem
            {
                GetRationAllocation = sv => sv.Id == "a" ? 0.3f : 0.7f,
                GetAverageRationAllocation = () => 0.5f,
                GetDay = () => 10,
                Rng = new Random(42)
            };

            // Survivor A gets below average — resentment builds
            system.Tick(survivorA, 24f, survivors);

            Assert.Greater(survivorA.RationResentmentLevel, 0f,
                "Under-allocated survivor should build resentment");
            Assert.AreEqual("b", survivorA.RationResentmentTargetId);
        }

        // ── Trade Specialty System ────────────────────────────────────

        [Test]
        public void TradeSpecialty_CraftMatchingItem_AdvancesMilestone()
        {
            var system = new TradeSpecialtySystem();
            var survivor = new Survivor
            {
                Id = "s1",
                PreWarProfessionId = "electrician"
            };

            system.OnItemCrafted(survivor, "battery_pack");

            Assert.AreEqual(1, survivor.CraftMilestonesCompleted.Count);
            Assert.IsTrue(survivor.CraftMilestonesCompleted[0]
                .Contains("electrician"));
        }

        [Test]
        public void TradeSpecialty_ThreeMilestones_MastersTrade()
        {
            float moraleApplied = 0f;
            var system = new TradeSpecialtySystem
            {
                ApplyMoraleDelta = (sv, d) => moraleApplied = d
            };
            var survivor = new Survivor
            {
                Id = "s1",
                PreWarProfessionId = "nurse"
            };

            system.OnItemCrafted(survivor, "bandage");
            system.OnItemCrafted(survivor, "splint");
            system.OnItemCrafted(survivor, "antiseptic");

            Assert.AreEqual(3, survivor.CraftMilestonesCompleted.Count);
            Assert.AreEqual(
                TradeSpecialtySystem.MasteryMoraleBonus, moraleApplied, 0.01f);
        }

        // ── Final Wish System ─────────────────────────────────────────

        [Test]
        public void FinalWish_DeclareTerminalPrognosis_ActivatesWish()
        {
            var system = new FinalWishSystem { Rng = new Random(42) };
            system.RegisterWish("the_surgeon", FinalWishSystem.WishTeachLesson);

            var survivor = new Survivor
            {
                Id = "s1",
                ArchetypeId = "the_surgeon"
            };

            system.DeclareTerminalPrognosis(survivor);

            Assert.IsTrue(survivor.HasTerminalPrognosis);
            Assert.Greater(survivor.TerminalPrognosisDaysRemaining, 2f);
            Assert.IsTrue(system.HasActiveWish(survivor));
        }
    }
}
