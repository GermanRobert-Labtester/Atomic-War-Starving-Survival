// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 197: Faction Diplomacy & Treaty System — Integration Tests
// Verifies treaty templates catalog loading, relation/reputation gating, treaty
// ratification, mission dispatch and completion, violations, and save/restore.
// ============================================================================
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Ashfall.Core.Diplomacy;

namespace Ashfall.Core.Tests.Diplomacy
{
    public sealed class Plan197FactionDiplomacyIntegrationTests : CatalogTestBase
    {
        [Fact]
        public void LoadCatalog_LoadsAllSixTreatyTemplates()
        {
            var system = new FactionDiplomacySystem();
            string path = Path.Combine(DataDirectory, "treaty_templates.json");
            Assert.True(File.Exists(path), $"treaty_templates.json must exist at {path}");

            system.LoadCatalog(File.ReadAllText(path));

            var templates = system.GetAllTemplates();
            Assert.Equal(6, templates.Count);

            var nonAggression = system.GetTemplate("non_aggression");
            Assert.NotNull(nonAggression);
            Assert.Equal("peace", nonAggression.category);
            Assert.Equal(60, nonAggression.base_duration_days);
            Assert.Equal(20, nonAggression.reputation_requirement);
            Assert.Equal("neutral", nonAggression.relation_requirement);
            Assert.NotEmpty(nonAggression.terms);
            Assert.Equal("non_hostility", nonAggression.terms[0].term_type);

            var defense = system.GetTemplate("mutual_defense");
            Assert.NotNull(defense);
            Assert.Equal("allied", defense.relation_requirement);
            Assert.Equal(60, defense.reputation_requirement);
        }

        [Fact]
        public void ProposeTreaty_EnforcesRelationAndReputationPrerequisites()
        {
            var system = new FactionDiplomacySystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "treaty_templates.json")));

            // Hostile relation cannot sign mutual_defense (requires allied)
            system.SetRelation("faction_iron_claws", -60, "hostile");
            var result = system.ProposeTreaty("faction_iron_claws", "mutual_defense", day: 5);
            Assert.False(result.Success);
            Assert.Contains("does not meet required", result.Message);

            // Meets relation requirement (neutral for non_aggression)
            system.SetRelation("faction_traders", 5, "neutral");
            var resultNonAggression = system.ProposeTreaty("faction_traders", "non_aggression", day: 5);
            Assert.True(resultNonAggression.Success);
            Assert.NotNull(resultNonAggression.Treaty);
            Assert.Equal("active", resultNonAggression.Treaty.Status);
        }

        [Fact]
        public void ProposeTreaty_RatifiesTreatyAndBoostsReputation()
        {
            var system = new FactionDiplomacySystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "treaty_templates.json")));

            ActiveTreatyRecord? signed = null;
            system.OnTreatySigned += t => signed = t;

            system.SetRelation("faction_caravan", 20, "friendly");
            int initialRep = system.GlobalReputation;

            var (success, _, treaty) = system.ProposeTreaty("faction_caravan", "trade_alliance", day: 10);
            Assert.True(success);
            Assert.NotNull(treaty);
            Assert.NotNull(signed);
            Assert.Equal(1, system.ActiveTreatyCount);
            Assert.True(system.HasActiveTreaty("faction_caravan", "trade_alliance"));
            Assert.Equal(initialRep + 2, system.GlobalReputation);

            // Cannot ratify duplicate active treaty of same type
            var duplicate = system.ProposeTreaty("faction_caravan", "trade_alliance", day: 11);
            Assert.False(duplicate.Success);
            Assert.Contains("already active", duplicate.Message);
        }

        [Fact]
        public void TickDay_ExpiresTreatiesAndAdvancesMissions()
        {
            var system = new FactionDiplomacySystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "treaty_templates.json")));

            system.SetRelation("faction_scavengers", 0, "neutral");
            var (_, _, treaty) = system.ProposeTreaty("faction_scavengers", "non_aggression", day: 1);
            Assert.NotNull(treaty);

            // Dispatch a mission (duration 2 days)
            var mission = system.DispatchMission("negotiate_treaty", "faction_scavengers", "surv_envoy", day: 1, durationDays: 2, envoySkill: 80);
            Assert.Equal("in_progress", mission.Status);

            ActiveTreatyRecord? expiredTreaty = null;
            system.OnTreatyExpired += t => expiredTreaty = t;
            DiplomaticMissionRecord? completedMission = null;
            system.OnMissionCompleted += m => completedMission = m;

            // Tick to day 3: mission should complete
            system.TickDay(3);
            Assert.NotNull(completedMission);
            Assert.Equal("completed", completedMission.Status);

            // Treaty duration is 60 days. On day 61, it should expire
            system.TickDay(61);
            Assert.NotNull(expiredTreaty);
            Assert.Equal("expired", treaty.Status);
            Assert.Equal(0, system.ActiveTreatyCount);
        }

        [Fact]
        public void ViolateAndRenounceTreaty_AppliesPenaltiesCorrectly()
        {
            var system = new FactionDiplomacySystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "treaty_templates.json")));

            system.SetRelation("faction_settlers", 30, "friendly");
            var (_, _, treaty) = system.ProposeTreaty("faction_settlers", "trade_alliance", day: 1);
            Assert.NotNull(treaty);

            bool violatedEventFired = false;
            system.OnTreatyViolated += (t, v) => violatedEventFired = true;

            int repBefore = system.GlobalReputation;
            bool violated = system.ViolateTreaty(treaty.TreatyId, "attacked_ally", "player", day: 5);
            Assert.True(violated);
            Assert.True(violatedEventFired);
            Assert.Equal("violated", treaty.Status);
            Assert.Equal(1, system.TotalViolationCount);
            Assert.Equal(repBefore - 25, system.GlobalReputation);

            var rel = system.GetOrCreateRelation("faction_settlers");
            Assert.Equal("hostile", rel.RelationLevel);
            Assert.True(rel.Trust < 0);
        }

        [Fact]
        public void SaveRestoreState_PreservesDiplomaticRelationsAndTreaties()
        {
            var system = new FactionDiplomacySystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "treaty_templates.json")));

            system.SetRelation("faction_delta", 40, "friendly");
            system.ProposeTreaty("faction_delta", "trade_alliance", day: 2);
            system.DispatchMission("renew_treaty", "faction_delta", "surv_diplomat", day: 2, durationDays: 4);

            var captured = system.CaptureState();
            Assert.NotNull(captured);
            Assert.Single(captured.ActiveTreaties);
            Assert.Single(captured.Missions);

            var restoredSystem = new FactionDiplomacySystem();
            restoredSystem.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "treaty_templates.json")));
            restoredSystem.RestoreState(captured);

            Assert.Equal(system.GlobalReputation, restoredSystem.GlobalReputation);
            Assert.Equal(1, restoredSystem.ActiveTreatyCount);
            Assert.True(restoredSystem.HasActiveTreaty("faction_delta", "trade_alliance"));

            var restoredRel = restoredSystem.GetOrCreateRelation("faction_delta");
            Assert.Equal("friendly", restoredRel.RelationLevel);
            Assert.Equal(1, restoredSystem.GetMissions().Count);
        }
    }
}
