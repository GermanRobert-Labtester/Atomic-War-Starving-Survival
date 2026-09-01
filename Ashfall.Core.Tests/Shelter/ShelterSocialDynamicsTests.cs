// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.IO;
using Ashfall.Core.Memorial;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Shelter
{
    public class ShelterSocialDynamicsTests
    {
        private static string GetSocialCatalogJson()
        {
            string path = Path.Combine(AppContext.BaseDirectory, "Assets/StreamingAssets/Data/shelter_social_events.json");
            if (!File.Exists(path))
            {
                path = Path.Combine(AppContext.BaseDirectory, "../../../Assets/StreamingAssets/Data/shelter_social_events.json");
            }
            if (File.Exists(path))
            {
                return File.ReadAllText(path);
            }

            return @"{
  ""schema_version"": 1,
  ""events"": [
    {
      ""id"": ""social_event_bunk_noise_friction"",
      ""display_name"": ""Midnight Bunk Disturbance"",
      ""room_tags"": [""crowded_sleeping""],
      ""required_room_ids"": [""room_bunks_crowded""],
      ""minimum_occupants"": 2,
      ""cooldown_days"": 3,
      ""base_weight"": 120,
      ""description"": ""Restless coughing sparks a quarrel."",
      ""outcomes"": [
        {
          ""id"": ""outcome_bunk_argument_unresolved"",
          ""display_name"": ""Unchecked Argument"",
          ""morale_delta"": -4,
          ""relationship_delta"": -12,
          ""memory_tag"": ""bunk_midnight_quarrel"",
          ""can_mediate"": true,
          ""mediation_skill_id"": ""skill_watchful""
        },
        {
          ""id"": ""outcome_bunk_argument_mediated"",
          ""display_name"": ""Quiet Mediation"",
          ""morale_delta"": 2,
          ""relationship_delta"": 6,
          ""memory_tag"": ""bunk_quarrel_settled"",
          ""can_mediate"": false,
          ""mediation_skill_id"": """"
        }
      ]
    }
  ]
}";
        }

        private static ShelterSocialDynamicsSystem CreateSystem(
            out SurvivorRelationsSystem relations,
            int seed = 42)
        {
            var rng = new SeededRng(seed);
            relations = new SurvivorRelationsSystem(rng);
            var memorial = new MemorialSystem(new MemorialState());

            var system = new ShelterSocialDynamicsSystem(rng, relations, null, memorial);
            system.LoadCatalog(GetSocialCatalogJson());
            return system;
        }

        [Fact]
        public void PrivateQuarters_RelievesPrivacyFatigue()
        {
            var system = CreateSystem(out _);
            var profile = system.GetOrCreatePrivacyProfile("dweller_1");
            profile.PrivacyFatiguePermille = 500;

            system.EvaluateRoomDynamics("room_quarters_private", new[] { "dweller_1" }, 1);
            Assert.Equal(250, profile.PrivacyFatiguePermille); // 500 - 250
            Assert.Equal(1, profile.LastSolitaryRestDay);
        }

        [Fact]
        public void CrowdedBunks_IncreasesPrivacyFatigue_AndTriggersSocialEvent()
        {
            var system = CreateSystem(out var relations);
            var profile = system.GetOrCreatePrivacyProfile("dweller_1");
            profile.PrivacyFatiguePermille = 100;

            var incident = system.EvaluateRoomDynamics("room_bunks_crowded", new[] { "dweller_1", "dweller_2" }, 1);
            Assert.NotNull(incident);
            Assert.Equal("social_event_bunk_noise_friction", incident.EventId);
            Assert.True(profile.PrivacyFatiguePermille > 100);

            // Verified relations delta applied
            var rel = relations.GetOrCreateRelationship("dweller_1", "dweller_2");
            Assert.Equal(-12f, rel.affinity);
        }

        [Fact]
        public void Mediation_SucceedsWithWatchfulSkill_AndRestoresAffinity()
        {
            var system = CreateSystem(out var relations);
            system.BindMediatorSkillProvider((mediator, skill) => mediator == "leader_dweller" ? 1.0f : 0.0f);

            var incident = system.EvaluateRoomDynamics("room_bunks_crowded", new[] { "dweller_1", "dweller_2" }, 1);
            Assert.NotNull(incident);

            var res = system.TryMediateIncident(incident.IncidentId, "leader_dweller");
            Assert.Equal(ActionResult.StatusKind.Success, res.Status);
            Assert.True(incident.IsMediated);
            Assert.True(incident.Resolved);

            var rel = relations.GetOrCreateRelationship("dweller_1", "dweller_2");
            Assert.Equal(-6f, rel.affinity); // -12 + 6 mediated outcome
        }

        [Fact]
        public void CommunalGathering_IncreasesAffinityAndTrust()
        {
            var system = CreateSystem(out var relations);
            var res = system.TriggerCommunalGathering("room_common_mess_hall", new[] { "dweller_1", "dweller_2", "dweller_3" }, 1);
            Assert.Equal(ActionResult.StatusKind.Success, res.Status);

            var rel12 = relations.GetOrCreateRelationship("dweller_1", "dweller_2");
            Assert.Equal(6f, rel12.affinity);
            Assert.Equal(4f, rel12.trust);
        }

        [Fact]
        public void SaveRestore_PreservesPrivacyProfilesAndIncidents()
        {
            var system = CreateSystem(out _);
            system.RegisterSurvivorRoom("dweller_1", "room_quarters_private");
            var profile = system.GetOrCreatePrivacyProfile("dweller_1");
            profile.PrivacyFatiguePermille = 330;

            var incident = system.EvaluateRoomDynamics("room_bunks_crowded", new[] { "dweller_2", "dweller_3" }, 1);
            Assert.NotNull(incident);

            var save = system.CaptureState();
            var system2 = CreateSystem(out _);
            system2.RestoreState(save);

            var restoredProfile = system2.GetOrCreatePrivacyProfile("dweller_1");
            Assert.Equal("room_quarters_private", restoredProfile.AssignedRoomId);
            Assert.Equal(330, restoredProfile.PrivacyFatiguePermille);

            var restoredIncident = system2.State.recentIncidents.Find(i => i.IncidentId == incident.IncidentId);
            Assert.NotNull(restoredIncident);
        }

        [Fact]
        public void DeterministicReplay_GeneratesIdenticalEvents()
        {
            var sysA = CreateSystem(out _, seed: 555);
            var sysB = CreateSystem(out _, seed: 555);

            var incA = sysA.EvaluateRoomDynamics("room_bunks_crowded", new[] { "dweller_1", "dweller_2" }, 1);
            var incB = sysB.EvaluateRoomDynamics("room_bunks_crowded", new[] { "dweller_1", "dweller_2" }, 1);

            Assert.Equal(incA?.EventId, incB?.EventId);
            Assert.Equal(incA?.OutcomeId, incB?.OutcomeId);
        }
    }
}
