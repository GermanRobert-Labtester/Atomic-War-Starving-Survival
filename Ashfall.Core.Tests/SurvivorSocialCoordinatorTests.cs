using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Survivors;
using Ashfall.Core.StartingLevel;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Seeded multi-day integration test: the five social-mechanics systems
    /// affect visible survivor decisions (morale, affinity, resentment,
    /// leadership stress, skill atrophy) and survive a save round-trip.
    /// </summary>
    public class SurvivorSocialCoordinatorTests
    {
        private static SurvivorNeedsState MakeSurvivor(string id, float morale = 50f, float health = 100f)
            => new SurvivorNeedsState { Id = id, Morale = morale, Health = health, IsAlive = true };

        private static (SurvivorSocialCoordinator coord, List<SurvivorNeedsState> roster) Build(int seed)
        {
            var needs = new NeedsSystem();
            var relations = new SurvivorRelationsSystem(new SeededRng(seed), null);
            var roster = new DutyRosterSystem();
            roster.Unlock(1);

            var survivors = new List<SurvivorNeedsState>
            {
                MakeSurvivor("sv_alpha", morale: 15f),  // low morale → atrophy risk
                MakeSurvivor("sv_bravo", morale: 50f),
                MakeSurvivor("sv_charlie", morale: 50f),
            };

            // Register survivors in NeedsSystem so Modify(id, ...) can find them.
            for (int i = 0; i < survivors.Count; i++)
                needs.Register(survivors[i]);

            // Assign two survivors to the same shift (friction + co-shift bonus).
            roster.Assign(DutyRosterSystem.RoleNightWatch, "sv_alpha");
            roster.Assign(DutyRosterSystem.RoleNightWatch, "sv_bravo");
            roster.Assign(DutyRosterSystem.RoleMess, "sv_charlie");

            var coord = new SurvivorSocialCoordinator(
                new SeededRng(seed),
                needs,
                relations,
                roster,
                () => 1);

            // Conflicting beliefs → friction between alpha and bravo.
            coord.RegisterBelief("sv_alpha", "military_discipline");
            coord.RegisterBelief("sv_bravo", "pacifist");
            coord.RegisterBelief("sv_charlie", "religious_faith");

            return (coord, survivors);
        }

        [Fact]
        public void TickDay_LeaderGetsMoreRations_CreatesResentment()
        {
            var (coord, survivors) = Build(42);
            coord.RationPolicy = RationPolicy.Half;
            coord.SetAliveSurvivors(new[] { "sv_alpha", "sv_bravo", "sv_charlie" });
            Assert.True(coord.DesignateLeader("sv_charlie"));

            // Tick 10 days — resentment builds toward the leader (more rations).
            for (int day = 1; day <= 10; day++)
                coord.TickDay(day, survivors);

            var state = coord.Ration.GetState("sv_alpha");
            Assert.NotNull(state);
            Assert.True(state.resentmentLevel > 0f, "alpha resents the leader for getting more rations");
            Assert.Equal("sv_charlie", state.resentmentTargetId);
        }

        [Fact]
        public void TickDay_ConflictingBeliefs_DrainAffinity()
        {
            var (coord, survivors) = Build(42);
            coord.RationPolicy = RationPolicy.Standard;
            // No leader → equal rations → no resentment noise; pure friction test.

            for (int day = 1; day <= 5; day++)
                coord.TickDay(day, survivors);

            // alpha and bravo are on the same shift with conflicting beliefs.
            float affinity = coord.Friction.GetAffinity("sv_alpha", "sv_bravo");
            Assert.True(affinity < 0f, "conflicting beliefs drain affinity below zero");
        }

        [Fact]
        public void TickDay_LeadershipStress_AccumulatesOnDeath()
        {
            var (coord, survivors) = Build(42);
            coord.SetAliveSurvivors(new[] { "sv_alpha", "sv_bravo", "sv_charlie" });
            coord.DesignateLeader("sv_alpha");

            coord.OnSurvivorDied("sv_bravo");

            float stress = coord.Leadership.GetLeaderStress("sv_alpha");
            Assert.True(stress >= LeadershipSystem.LeaderStressPerDeath,
                "leader stress accumulates when a survivor dies");
        }

        [Fact]
        public void TickDay_LowMorale_TriggersAtrophy()
        {
            var (coord, survivors) = Build(42);
            // sv_alpha starts at morale 15 (below the 20 threshold).
            // AtrophyWindowDays = 14, so tick 15 days.
            for (int day = 1; day <= 15; day++)
                coord.TickDay(day, survivors);

            Assert.True(coord.Atrophy.IsAtrophied("sv_alpha", "medical"),
                "medical skill atrophies after 14 days of low morale");
            Assert.True(coord.Atrophy.IsAtrophied("sv_alpha", "crafting"),
                "crafting skill atrophies after 14 days of low morale");
        }

        [Fact]
        public void OnSharedHazardEndured_FormsBondAndBoostsAffinity()
        {
            var (coord, survivors) = Build(42);
            coord.SetAliveSurvivors(new[] { "sv_alpha", "sv_bravo", "sv_charlie" });

            coord.OnSharedHazardEndured(
                new List<string> { "sv_alpha", "sv_bravo" },
                "fallout_storm");

            Assert.True(coord.TraumaBond.HasBond("sv_alpha", "sv_bravo"),
                "shared hazard forms a trauma bond");
            Assert.True(coord.TraumaBond.GetBondStrength("sv_alpha", "sv_bravo") > 0f,
                "bond strength is positive");
        }

        [Fact]
        public void CaptureRestore_RoundTripsAllFiveSystems()
        {
            var (coord, survivors) = Build(42);
            coord.RationPolicy = RationPolicy.Half;
            coord.DesignateLeader("sv_charlie");
            coord.SetAliveSurvivors(new[] { "sv_alpha", "sv_bravo", "sv_charlie" });
            coord.OnSharedHazardEndured(
                new List<string> { "sv_alpha", "sv_bravo" }, "fallout_storm");

            // Tick 20 days to accumulate state across all systems.
            for (int day = 1; day <= 20; day++)
                coord.TickDay(day, survivors);

            // Snapshot the live state.
            string leaderBefore = coord.Leadership.CurrentLeaderId;
            float stressBefore = coord.Leadership.GetLeaderStress("sv_charlie");
            float resentmentBefore = coord.Ration.GetState("sv_alpha")?.resentmentLevel ?? 0f;
            float affinityBefore = coord.Friction.GetAffinity("sv_alpha", "sv_bravo");
            float bondBefore = coord.TraumaBond.GetBondStrength("sv_alpha", "sv_bravo");
            bool atrophiedBefore = coord.Atrophy.IsAtrophied("sv_alpha", "medical");

            // Capture.
            var save = coord.CaptureState();
            Assert.NotNull(save);
            Assert.NotNull(save.leadership);
            Assert.NotNull(save.friction);
            Assert.NotNull(save.ration);
            Assert.NotNull(save.trauma);
            Assert.NotNull(save.atrophy);

            // Restore into a fresh coordinator.
            var (coord2, survivors2) = Build(42);
            coord2.RestoreState(save);

            Assert.Equal(leaderBefore, coord2.Leadership.CurrentLeaderId);
            Assert.Equal(stressBefore, coord2.Leadership.GetLeaderStress("sv_charlie"));
            Assert.Equal(resentmentBefore, coord2.Ration.GetState("sv_alpha")?.resentmentLevel ?? 0f);
            Assert.Equal(affinityBefore, coord2.Friction.GetAffinity("sv_alpha", "sv_bravo"));
            Assert.Equal(bondBefore, coord2.TraumaBond.GetBondStrength("sv_alpha", "sv_bravo"));
            Assert.Equal(atrophiedBefore, coord2.Atrophy.IsAtrophied("sv_alpha", "medical"));
        }

        [Fact]
        public void BuildReadModel_ReflectsAllSystems()
        {
            var (coord, survivors) = Build(42);
            coord.RationPolicy = RationPolicy.Half;
            coord.SetAliveSurvivors(new[] { "sv_alpha", "sv_bravo", "sv_charlie" });
            coord.DesignateLeader("sv_charlie");
            coord.OnSharedHazardEndured(
                new List<string> { "sv_alpha", "sv_bravo" }, "raid");

            for (int day = 1; day <= 5; day++)
                coord.TickDay(day, survivors);

            var rm = coord.BuildReadModel();

            Assert.Equal("sv_charlie", rm.leaderId);
            Assert.True(rm.leaderStress >= 0f);
            Assert.Equal(3, rm.entries.Count);

            var alpha = rm.entries.FirstOrDefault(e => e.survivorId == "sv_alpha");
            Assert.NotNull(alpha);
            Assert.Equal("military_discipline", alpha.belief);
            Assert.True(alpha.bondCount >= 1, "alpha has a bond with bravo");
            Assert.Equal("sv_bravo", alpha.strongestBondPartnerId);
            Assert.True(alpha.resentmentLevel > 0f, "alpha resents the leader");
            Assert.Equal("sv_charlie", alpha.resentmentTargetId);
        }

        [Fact]
        public void TickDay_Deterministic_SameSeedSameResult()
        {
            var (coordA, survivorsA) = Build(99);
            var (coordB, survivorsB) = Build(99);

            coordA.RationPolicy = RationPolicy.Half;
            coordB.RationPolicy = RationPolicy.Half;
            coordA.SetAliveSurvivors(new[] { "sv_alpha", "sv_bravo", "sv_charlie" });
            coordB.SetAliveSurvivors(new[] { "sv_alpha", "sv_bravo", "sv_charlie" });
            coordA.DesignateLeader("sv_charlie");
            coordB.DesignateLeader("sv_charlie");

            for (int day = 1; day <= 10; day++)
            {
                coordA.TickDay(day, survivorsA);
                coordB.TickDay(day, survivorsB);
            }

            Assert.Equal(
                coordA.Ration.GetState("sv_alpha")?.resentmentLevel ?? 0f,
                coordB.Ration.GetState("sv_alpha")?.resentmentLevel ?? 0f);
            Assert.Equal(
                coordA.Friction.GetAffinity("sv_alpha", "sv_bravo"),
                coordB.Friction.GetAffinity("sv_alpha", "sv_bravo"));
        }
    }
}
