// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Ashfall.Core.IO;
using Ashfall.Core.Memorial;
using Ashfall.Core.Survivors;

namespace Ashfall.Core.Shelter
{
    [Serializable]
    public sealed class SocialOutcome
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; } = string.Empty;

        [JsonPropertyName("morale_delta")]
        public int MoraleDelta { get; set; }

        [JsonPropertyName("relationship_delta")]
        public int RelationshipDelta { get; set; }

        [JsonPropertyName("memory_tag")]
        public string MemoryTag { get; set; } = string.Empty;

        [JsonPropertyName("can_mediate")]
        public bool CanMediate { get; set; }

        [JsonPropertyName("mediation_skill_id")]
        public string MediationSkillId { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class SocialEventDefinition
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; } = string.Empty;

        [JsonPropertyName("room_tags")]
        public List<string> RoomTags { get; set; } = new List<string>();

        [JsonPropertyName("required_room_ids")]
        public List<string> RequiredRoomIds { get; set; } = new List<string>();

        [JsonPropertyName("minimum_occupants")]
        public int MinimumOccupants { get; set; } = 2;

        [JsonPropertyName("cooldown_days")]
        public int CooldownDays { get; set; } = 3;

        [JsonPropertyName("base_weight")]
        public int BaseWeight { get; set; } = 100;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("outcomes")]
        public List<SocialOutcome> Outcomes { get; set; } = new List<SocialOutcome>();
    }

    [Serializable]
    public sealed class ShelterSocialEventCatalogData
    {
        [JsonPropertyName("schema_version")]
        public int SchemaVersion { get; set; } = 1;

        [JsonPropertyName("events")]
        public List<SocialEventDefinition> Events { get; set; } = new List<SocialEventDefinition>();
    }

    [Serializable]
    public sealed class SurvivorPrivacyProfile
    {
        public string SurvivorId { get; set; } = string.Empty;
        public string AssignedRoomId { get; set; } = string.Empty;
        public int PrivacyFatiguePermille { get; set; } = 0; // 0 - 1000
        public int LastSolitaryRestDay { get; set; } = -1;
    }

    [Serializable]
    public sealed class SocialIncidentRecord
    {
        public string IncidentId { get; set; } = string.Empty;
        public string EventId { get; set; } = string.Empty;
        public string RoomId { get; set; } = string.Empty;
        public List<string> ParticipantIds { get; set; } = new List<string>();
        public string OutcomeId { get; set; } = string.Empty;
        public int Day { get; set; }
        public bool IsMediated { get; set; }
        public string MediatorId { get; set; } = string.Empty;
        public bool Resolved { get; set; }
    }

    [Serializable]
    public sealed class ShelterSocialSave
    {
        public string systemId = ShelterSocialDynamicsSystem.SystemId;
        public int schemaVersion = 1;
        public Dictionary<string, SurvivorPrivacyProfile> privacyProfiles = new(StringComparer.Ordinal);
        public List<SocialIncidentRecord> recentIncidents = new List<SocialIncidentRecord>();
        public Dictionary<string, int> eventCooldowns = new(StringComparer.Ordinal);
        public int currentDay;
    }

    public sealed class ShelterSocialDynamicsSystem
    {
        public const string SystemId = "shelter_social_dynamics";

        private ShelterSocialSave _state = new ShelterSocialSave();
        private readonly Dictionary<string, SocialEventDefinition> _catalog = new(StringComparer.Ordinal);
        private readonly SurvivorRelationsSystem? _relations;
        private readonly NeedsSystem? _needs;
        private readonly MemorialSystem? _memorial;
        private readonly ISeededRng _rng;
        private readonly ILog _log;

        private Func<string, string, float>? _mediatorSkillProvider; // (survivorId, skillId) -> 0..1+

        public ShelterSocialSave State => _state;
        public IReadOnlyDictionary<string, SocialEventDefinition> Catalog => _catalog;

        public event Action<SocialIncidentRecord>? OnIncidentTriggered;
        public event Action<SocialIncidentRecord>? OnIncidentMediated;
        public event Action? OnSocialStateChanged;

        public ShelterSocialDynamicsSystem(
            ISeededRng rng,
            SurvivorRelationsSystem? relations = null,
            NeedsSystem? needs = null,
            MemorialSystem? memorial = null,
            ILog? log = null)
        {
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _relations = relations;
            _needs = needs;
            _memorial = memorial;
            _log = log ?? NullLog.Instance;
        }

        public void BindMediatorSkillProvider(Func<string, string, float> provider)
        {
            _mediatorSkillProvider = provider;
        }

        public void LoadCatalog(ShelterSocialEventCatalogData? data)
        {
            if (data?.Events == null) return;
            _catalog.Clear();
            foreach (var ev in data.Events)
            {
                if (!string.IsNullOrEmpty(ev.Id))
                    _catalog[ev.Id] = ev;
            }
        }

        public void LoadCatalog(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return;
            var serializer = new SystemTextJsonSerializer();
            var data = serializer.Deserialize<ShelterSocialEventCatalogData>(json);
            LoadCatalog(data);
        }

        public SurvivorPrivacyProfile GetOrCreatePrivacyProfile(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) survivorId = "dweller_unknown";
            if (_state.privacyProfiles.TryGetValue(survivorId, out var profile))
                return profile;

            var created = new SurvivorPrivacyProfile
            {
                SurvivorId = survivorId,
                PrivacyFatiguePermille = 0
            };
            _state.privacyProfiles[survivorId] = created;
            return created;
        }

        public void RegisterSurvivorRoom(string survivorId, string roomId)
        {
            var profile = GetOrCreatePrivacyProfile(survivorId);
            profile.AssignedRoomId = roomId;
            OnSocialStateChanged?.Invoke();
        }

        public SocialIncidentRecord? EvaluateRoomDynamics(string roomId, IReadOnlyList<string> occupantIds, int day)
        {
            _state.currentDay = day;
            if (occupantIds == null || occupantIds.Count == 0) return null;

            // Room-specific privacy adjustment
            if (roomId == "room_quarters_private")
            {
                foreach (var occupant in occupantIds)
                {
                    var profile = GetOrCreatePrivacyProfile(occupant);
                    profile.PrivacyFatiguePermille = Math.Max(0, profile.PrivacyFatiguePermille - 250);
                    profile.LastSolitaryRestDay = day;
                }
            }
            else if (roomId == "room_bunks_crowded")
            {
                foreach (var occupant in occupantIds)
                {
                    var profile = GetOrCreatePrivacyProfile(occupant);
                    profile.PrivacyFatiguePermille = Math.Min(1000, profile.PrivacyFatiguePermille + 120);
                }
            }

            // Find eligible events
            var eligible = new List<SocialEventDefinition>();
            foreach (var ev in _catalog.Values)
            {
                if (ev.MinimumOccupants > occupantIds.Count) continue;
                if (ev.RequiredRoomIds.Count > 0 && !ev.RequiredRoomIds.Contains(roomId)) continue;
                if (_state.eventCooldowns.TryGetValue(ev.Id, out int cd) && cd > day) continue;
                eligible.Add(ev);
            }

            if (eligible.Count == 0) return null;

            var chosen = eligible[_rng.Next(0, eligible.Count)];
            if (chosen.Outcomes.Count == 0) return null;

            var outcome = chosen.Outcomes[0]; // Primary initial outcome

            var incident = new SocialIncidentRecord
            {
                IncidentId = $"inc_{chosen.Id}_{day}_{_rng.Next(1000, 9999)}",
                EventId = chosen.Id,
                RoomId = roomId,
                ParticipantIds = new List<string>(occupantIds),
                OutcomeId = outcome.Id,
                Day = day,
                Resolved = !outcome.CanMediate
            };

            _state.eventCooldowns[chosen.Id] = day + chosen.CooldownDays;
            _state.recentIncidents.Add(incident);

            // Apply outcomes to authorities
            ApplyOutcome(outcome, occupantIds);

            OnIncidentTriggered?.Invoke(incident);
            OnSocialStateChanged?.Invoke();
            return incident;
        }

        private void ApplyOutcome(SocialOutcome outcome, IReadOnlyList<string> participantIds)
        {
            if (_relations != null && participantIds.Count >= 2 && outcome.RelationshipDelta != 0)
            {
                for (int i = 0; i < participantIds.Count; i++)
                {
                    for (int j = i + 1; j < participantIds.Count; j++)
                    {
                        _relations.ModifyAffinity(participantIds[i], participantIds[j], outcome.RelationshipDelta);
                    }
                }
            }
        }

        public ActionResult TryMediateIncident(string incidentId, string mediatorId)
        {
            var incident = _state.recentIncidents.Find(i => i.IncidentId == incidentId);
            if (incident == null) return ActionResult.Failed("unknown_incident", "social.unknown_incident");
            if (incident.Resolved) return ActionResult.Blocked("already_resolved", "social.incident_already_resolved");

            if (!_catalog.TryGetValue(incident.EventId, out var eventDef))
                return ActionResult.Failed("unknown_event", "social.unknown_event");

            var outcome = eventDef.Outcomes.Find(o => o.Id == incident.OutcomeId);
            if (outcome == null || !outcome.CanMediate)
                return ActionResult.Blocked("cannot_mediate", "social.cannot_mediate");

            float skillLevel = 0.5f;
            if (_mediatorSkillProvider != null && !string.IsNullOrEmpty(outcome.MediationSkillId))
            {
                skillLevel = _mediatorSkillProvider(mediatorId, outcome.MediationSkillId);
            }

            // High skill guarantees mediation success
            bool success = skillLevel >= 0.3f || _rng.NextDouble() < 0.6f;

            if (success)
            {
                incident.IsMediated = true;
                incident.MediatorId = mediatorId;
                incident.Resolved = true;

                // Find positive mediated outcome if available
                var mediatedOutcome = eventDef.Outcomes.Find(o => o.Id != outcome.Id) ?? outcome;
                ApplyOutcome(mediatedOutcome, incident.ParticipantIds);

                OnIncidentMediated?.Invoke(incident);
                OnSocialStateChanged?.Invoke();
                return ActionResult.Success("social.mediation_success");
            }
            else
            {
                incident.Resolved = true;
                OnSocialStateChanged?.Invoke();
                return ActionResult.Failed("mediation_failed", "social.mediation_failed");
            }
        }

        public ActionResult TriggerCommunalGathering(string messHallRoomId, IReadOnlyList<string> attendeeIds, int day)
        {
            _state.currentDay = day;
            if (attendeeIds == null || attendeeIds.Count < 2)
                return ActionResult.Blocked("insufficient_dwellers", "social.need_more_attendees");

            // Cohesion bonus across all attendees
            if (_relations != null)
            {
                for (int i = 0; i < attendeeIds.Count; i++)
                {
                    for (int j = i + 1; j < attendeeIds.Count; j++)
                    {
                        _relations.ModifyAffinity(attendeeIds[i], attendeeIds[j], 6f);
                        _relations.ModifyTrust(attendeeIds[i], attendeeIds[j], 4f);
                    }
                }
            }

            OnSocialStateChanged?.Invoke();
            return ActionResult.Success("social.communal_gathering_held");
        }

        public void TickDay(int day)
        {
            _state.currentDay = day;

            // Passive decay / update
            foreach (var profile in _state.privacyProfiles.Values)
            {
                if (profile.AssignedRoomId == "room_quarters_private")
                {
                    profile.PrivacyFatiguePermille = Math.Max(0, profile.PrivacyFatiguePermille - 200);
                }
            }

            OnSocialStateChanged?.Invoke();
        }

        public ShelterSocialSave CaptureState()
        {
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(_state);
            return s.Deserialize<ShelterSocialSave>(json) ?? new ShelterSocialSave();
        }

        public void RestoreState(ShelterSocialSave? saved)
        {
            if (saved == null)
            {
                _state = new ShelterSocialSave();
                return;
            }

            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(saved);
            _state = s.Deserialize<ShelterSocialSave>(json) ?? new ShelterSocialSave();
            OnSocialStateChanged?.Invoke();
        }
    }
}
