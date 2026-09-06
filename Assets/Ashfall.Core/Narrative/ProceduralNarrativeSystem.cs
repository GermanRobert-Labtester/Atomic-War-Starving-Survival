// SPDX-License-Identifier: MIT
// ASHFALL Core: deterministic structured procedural quest generation.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Ashfall.Core.IO;
using Ashfall.Core.Quests;

namespace Ashfall.Core.Narrative
{
    [Serializable]
    public sealed class QuestTemplateDef
    {
        [JsonPropertyName("id")] public string Id { get; set; } = string.Empty;
        [JsonPropertyName("category")] public string Category { get; set; } = string.Empty;
        [JsonPropertyName("weight")] public float Weight { get; set; } = 1f;
        [JsonPropertyName("minimum_day")] public int MinimumDay { get; set; }
        [JsonPropertyName("maximum_day")] public int MaximumDay { get; set; } = -1;
        [JsonPropertyName("required_world_tags")] public List<string> RequiredWorldTags { get; set; } = new List<string>();
        [JsonPropertyName("blocked_world_tags")] public List<string> BlockedWorldTags { get; set; } = new List<string>();
        [JsonPropertyName("actor_role_slots")] public List<string> ActorRoleSlots { get; set; } = new List<string>();
        [JsonPropertyName("location_constraints")] public List<string> LocationConstraints { get; set; } = new List<string>();
        [JsonPropertyName("objective_modules")] public List<string> ObjectiveModules { get; set; } = new List<string>();
        [JsonPropertyName("reward_modules")] public List<string> RewardModules { get; set; } = new List<string>();
        [JsonPropertyName("failure_modules")] public List<string> FailureModules { get; set; } = new List<string>();
        [JsonPropertyName("time_limit_days")] public int TimeLimitDays { get; set; }
        [JsonPropertyName("follow_up_template_ids")] public List<string> FollowUpTemplateIds { get; set; } = new List<string>();
        [JsonPropertyName("merge_policy")] public string MergePolicy { get; set; } = "never";
        [JsonPropertyName("cooldown_days")] public int CooldownDays { get; set; }
        [JsonPropertyName("max_concurrent_instances")] public int MaxConcurrentInstances { get; set; } = 1;
        [JsonPropertyName("title_key")] public string TitleKey { get; set; } = string.Empty;
        [JsonPropertyName("description_key")] public string DescriptionKey { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class QuestTemplateCatalog
    {
        [JsonPropertyName("schema_version")] public int SchemaVersion { get; set; } = 1;
        [JsonPropertyName("templates")] public List<QuestTemplateDef> Templates { get; set; } = new List<QuestTemplateDef>();
    }

    public static class QuestTemplateCatalogLoader
    {
        public const string FileName = "quest_templates.json";

        public static List<QuestTemplateDef> Load(string dataDir, IFileIO files, IJsonSerializer serializer)
        {
            var result = new List<QuestTemplateDef>();
            if (files == null || serializer == null || string.IsNullOrWhiteSpace(dataDir)) return result;
            string path = files.Combine(dataDir, FileName);
            if (!files.FileExists(path)) return result;
            try
            {
                var catalog = JsonSerializer.Deserialize<QuestTemplateCatalog>(files.ReadAllText(path), SystemTextJsonSerializer.Options);
                var seen = new HashSet<string>(StringComparer.Ordinal);
                foreach (var template in catalog?.Templates ?? new List<QuestTemplateDef>())
                    if (template != null && !string.IsNullOrWhiteSpace(template.Id) && seen.Add(template.Id)) result.Add(template);
            }
            catch (Exception ex)
            {
                CatalogDiagnostics.Warn(FileName, "QuestTemplateCatalogLoader", ex);
            }
            return result;
        }

        public static bool Validate(IEnumerable<QuestTemplateDef> templates, out string error)
        {
            error = string.Empty;
            var allTemplates = (templates ?? Enumerable.Empty<QuestTemplateDef>()).Where(t => t != null).ToList();
            var allIds = new HashSet<string>(StringComparer.Ordinal);
            foreach (var template in allTemplates)
                if (template != null && !string.IsNullOrWhiteSpace(template.Id)) allIds.Add(template.Id);
            var ids = new HashSet<string>(StringComparer.Ordinal);
            var knownModules = new HashSet<string>(new[]
                { "repair_pipe", "trace_contamination", "rescue_agent", "find_missing_person", "secure_filter_media" }, StringComparer.Ordinal);
            foreach (var template in allTemplates)
            {
                if (template == null || string.IsNullOrWhiteSpace(template.Id)
                    || template.Weight < 0f || template.MinimumDay < 0 || template.MaximumDay >= 0 && template.MaximumDay < template.MinimumDay
                    || template.TimeLimitDays < 0 || template.CooldownDays < 0 || template.MaxConcurrentInstances <= 0)
                { error = "Invalid or duplicate quest template definition."; return false; }
                foreach (var module in template.ObjectiveModules ?? new List<string>())
                    if (!knownModules.Contains(module)) { error = "Unknown quest objective module: " + module; return false; }
                foreach (var followUp in template.FollowUpTemplateIds ?? new List<string>())
                    if (string.IsNullOrWhiteSpace(followUp) || !allIds.Contains(followUp))
                    { error = "Quest template has an unknown follow-up id: " + template.Id; return false; }
            }
            return true;
        }
    }

    [Serializable]
    public sealed class NarrativeWorldSnapshot
    {
        public int day;
        public List<string> worldTags = new List<string>();
        public List<string> aliveSurvivorIds = new List<string>();
        public List<string> knownLocationIds = new List<string>();
        public List<string> obtainableItemIds = new List<string>();
        public List<string> protectedActorIds = new List<string>();
        public List<string> protectedLocationIds = new List<string>();
        public List<QuestLogEntry> activeQuests = new List<QuestLogEntry>();
    }

    [Serializable]
    public sealed class ProceduralNarrativeState
    {
        public string systemId = "procedural_narrative";
        public Dictionary<string, int> templateCooldowns = new Dictionary<string, int>(StringComparer.Ordinal);
        public List<RivalryState> rivalries = new List<RivalryState>();
        public List<string> recentTemplateIds = new List<string>();
        public int generationSequence;
    }

    /// <summary>
    /// Single persisted payload for the procedural narrative coordinator and
    /// the shared quest read model. Keeping these two states together prevents
    /// a generated quest from being restored without its cooldown/provenance.
    /// </summary>
    [Serializable]
    public sealed class ProceduralNarrativeSaveState
    {
        public ProceduralNarrativeState narrative = new ProceduralNarrativeState();
        public QuestRuntimeState quests = new QuestRuntimeState();
    }

    [Serializable]
    public sealed class RivalryState
    {
        public string playerActorId = string.Empty;
        public string npcActorId = string.Empty;
        public string originIncidentId = string.Empty;
        public float intensity01;
        public int lastEncounterDay;
        public string status = "active";
    }

    [Serializable]
    public sealed class ProceduralQuestDraft
    {
        public QuestInstanceState quest = new QuestInstanceState();
        public string templateId = string.Empty;
        public string rejectionReason = string.Empty;
    }

    /// <summary>
    /// Generates structured quest instances from an immutable campaign snapshot.
    /// Eligibility is pure and consumes zero RNG; only the accepted selection and
    /// deterministic bindings consume the caller-owned narrative RNG stream.
    /// </summary>
    public sealed class ProceduralNarrativeSystem
    {
        public const string SystemId = "procedural_narrative";
        private readonly Dictionary<string, QuestTemplateDef> _templates = new Dictionary<string, QuestTemplateDef>(StringComparer.Ordinal);
        private readonly ILog _log;
        private ProceduralNarrativeState _state;
        private int _maxActiveQuests = 4;

        public ProceduralNarrativeState State => _state;
        public IReadOnlyDictionary<string, QuestTemplateDef> Templates => _templates;
        public event Action<QuestInstanceState>? OnQuestGenerated;
        public event Action<QuestInstanceState, QuestInstanceState>? OnQuestMerged;
        public event Action<RivalryState>? OnRivalryCreated;

        public ProceduralNarrativeSystem(ILog? log = null, ProceduralNarrativeState? state = null)
        {
            _log = log ?? NullLog.Instance;
            _state = state ?? new ProceduralNarrativeState();
        }

        public void SetActiveQuestCap(int cap) => _maxActiveQuests = Math.Max(1, cap);

        public void LoadTemplateCatalog(IEnumerable<QuestTemplateDef> templates)
        {
            _templates.Clear();
            foreach (var template in templates ?? Enumerable.Empty<QuestTemplateDef>())
                if (template != null && !string.IsNullOrWhiteSpace(template.Id) && !_templates.ContainsKey(template.Id)) _templates[template.Id] = template;
        }

        public IReadOnlyList<QuestTemplateDef> GetEligibleTemplates(NarrativeWorldSnapshot snapshot)
        {
            if (snapshot == null) return new List<QuestTemplateDef>();
            return _templates.Values.Where(template => IsTemplateEligible(template, snapshot)).OrderBy(t => t.Id, StringComparer.Ordinal).ToList();
        }

        public bool IsTemplateEligible(QuestTemplateDef template, NarrativeWorldSnapshot snapshot)
        {
            if (template == null || snapshot == null || template.Weight <= 0f || snapshot.day < template.MinimumDay
                || template.MaximumDay >= 0 && snapshot.day > template.MaximumDay) return false;
            var tags = new HashSet<string>(snapshot.worldTags ?? new List<string>(), StringComparer.Ordinal);
            if ((template.RequiredWorldTags ?? new List<string>()).Any(tag => !tags.Contains(tag))) return false;
            if ((template.BlockedWorldTags ?? new List<string>()).Any(tags.Contains)) return false;
            if (_state.templateCooldowns.TryGetValue(template.Id, out int cooldownDay) && snapshot.day < cooldownDay) return false;
            int activeCount = (snapshot.activeQuests ?? new List<QuestLogEntry>()).Count(q => q != null && q.status == QuestLifecycleState.Active && q.titleKey == template.TitleKey);
            if (activeCount >= template.MaxConcurrentInstances) return false;
            var actorRoles = template.ActorRoleSlots ?? new List<string>();
            var locationConstraints = template.LocationConstraints ?? new List<string>();
            var objectiveModules = template.ObjectiveModules ?? new List<string>();
            var aliveSurvivors = snapshot.aliveSurvivorIds ?? new List<string>();
            var knownLocations = snapshot.knownLocationIds ?? new List<string>();
            var protectedActors = snapshot.protectedActorIds ?? new List<string>();
            var protectedLocations = snapshot.protectedLocationIds ?? new List<string>();
            if (aliveSurvivors.Count < actorRoles.Count) return false;
            if (knownLocations.Count < locationConstraints.Count) return false;
            if (actorRoles.Any(role => string.IsNullOrWhiteSpace(role))) return false;
            if (locationConstraints.Any(location => string.IsNullOrWhiteSpace(location))) return false;
            if (protectedActors.Count > 0 && actorRoles.Count > 0
                && !aliveSurvivors.Any(id => !protectedActors.Contains(id, StringComparer.Ordinal))) return false;
            if (protectedLocations.Count > 0
                && !knownLocations.Any(id => !protectedLocations.Contains(id, StringComparer.Ordinal))) return false;
            if (objectiveModules.Any(module => module == "secure_filter_media")
                && !(snapshot.obtainableItemIds ?? new List<string>()).Contains("item_water_filter_advanced", StringComparer.Ordinal)) return false;
            return objectiveModules.All(module => IsSupportedModule(module));
        }

        public bool TryGenerate(NarrativeWorldSnapshot snapshot, ISeededRng rng, out ProceduralQuestDraft draft)
        {
            draft = new ProceduralQuestDraft();
            if (snapshot == null || rng == null) { draft.rejectionReason = "missing_input"; return false; }
            var active = (snapshot.activeQuests ?? new List<QuestLogEntry>()).Count(q => q != null && q.status == QuestLifecycleState.Active);
            if (active >= _maxActiveQuests) { draft.rejectionReason = "active_cap"; return false; }
            var eligible = GetEligibleTemplates(snapshot);
            if (eligible.Count == 0) { draft.rejectionReason = "no_eligible_template"; return false; }
            double totalWeight = eligible.Sum(t => Math.Max(0f, t.Weight));
            double roll = rng.NextDouble() * totalWeight;
            QuestTemplateDef selected = eligible[eligible.Count - 1];
            foreach (var template in eligible)
            {
                roll -= Math.Max(0f, template.Weight);
                if (roll < 0d) { selected = template; break; }
            }

            var actorPool = (snapshot.aliveSurvivorIds ?? new List<string>()).Where(id => !(snapshot.protectedActorIds ?? new List<string>()).Contains(id, StringComparer.Ordinal))
                .OrderBy(id => id, StringComparer.Ordinal).ToList();
            var locationPool = (snapshot.knownLocationIds ?? new List<string>()).Where(id => !(snapshot.protectedLocationIds ?? new List<string>()).Contains(id, StringComparer.Ordinal))
                .OrderBy(id => id, StringComparer.Ordinal).ToList();
            var selectedActorRoles = selected.ActorRoleSlots ?? new List<string>();
            var selectedLocations = selected.LocationConstraints ?? new List<string>();
            var selectedObjectives = selected.ObjectiveModules ?? new List<string>();
            if (actorPool.Count < selectedActorRoles.Count || locationPool.Count < selectedLocations.Count)
            {
                draft.templateId = selected.Id;
                draft.rejectionReason = "binding_pool_empty";
                return false;
            }

            var quest = new QuestInstanceState
            {
                instanceId = "procedural_quest_" + (++_state.generationSequence).ToString("D4"),
                definitionId = selected.Id,
                sourceKind = QuestSourceKind.Procedural,
                titleKey = selected.TitleKey,
                descriptionKey = selected.DescriptionKey,
                createdDay = snapshot.day,
                expiryDay = selected.TimeLimitDays > 0 ? snapshot.day + selected.TimeLimitDays : -1,
                generationSeed = rng.Seed
            };
            for (int i = 0; i < selectedActorRoles.Count; i++) quest.actorBindings[selectedActorRoles[i]] = actorPool[i];
            for (int i = 0; i < selectedLocations.Count; i++) quest.locationBindings.Add(locationPool[i]);
            for (int i = 0; i < selectedObjectives.Count; i++)
                quest.objectiveStates.Add(new QuestObjectiveRuntimeState
                {
                    objectiveId = quest.instanceId + "_objective_" + i.ToString("D2"),
                    moduleId = selectedObjectives[i],
                    requiredAmount = 1
                });
            quest.rewardBindings = new List<string>(selected.RewardModules ?? new List<string>());
            quest.failureConsequences = new List<string>(selected.FailureModules ?? new List<string>());
            _state.templateCooldowns[selected.Id] = snapshot.day + selected.CooldownDays;
            _state.recentTemplateIds.Add(selected.Id);
            if (_state.recentTemplateIds.Count > 12) _state.recentTemplateIds.RemoveAt(0);
            draft.templateId = selected.Id;
            draft.quest = quest;
            OnQuestGenerated?.Invoke(quest);
            return true;
        }

        public bool TryMerge(QuestInstanceState first, QuestInstanceState second, out QuestInstanceState merged)
        {
            merged = new QuestInstanceState();
            if (first == null || second == null || first.sourceKind != QuestSourceKind.Procedural || second.sourceKind != QuestSourceKind.Procedural
                || first.locationBindings.Count == 0 || second.locationBindings.Count == 0
                || !string.Equals(first.locationBindings[0], second.locationBindings[0], StringComparison.Ordinal)) return false;
            if (!_templates.TryGetValue(first.definitionId, out var a) || !_templates.TryGetValue(second.definitionId, out var b)
                || a.MergePolicy != "same_location" || b.MergePolicy != "same_location") return false;
            merged = new QuestInstanceState
            {
                instanceId = first.instanceId + "_merge_" + second.instanceId,
                definitionId = "merged_procedural_quest",
                sourceKind = QuestSourceKind.Procedural,
                titleKey = first.titleKey,
                descriptionKey = first.descriptionKey,
                createdDay = Math.Min(first.createdDay, second.createdDay),
                expiryDay = MinDeadline(first.expiryDay, second.expiryDay),
                mergeGroupId = first.instanceId + "+" + second.instanceId,
                childInstanceIds = new List<string> { first.instanceId, second.instanceId },
                actorBindings = new Dictionary<string, string>(first.actorBindings, StringComparer.Ordinal),
                locationBindings = new List<string>(first.locationBindings),
                rewardBindings = first.rewardBindings.Concat(second.rewardBindings).ToList(),
                failureConsequences = first.failureConsequences.Concat(second.failureConsequences).ToList()
            };
            merged.objectiveStates.AddRange(first.objectiveStates.Select(CloneObjective));
            merged.objectiveStates.AddRange(second.objectiveStates.Select(CloneObjective));
            OnQuestMerged?.Invoke(first, merged);
            return true;
        }

        public RivalryState CreateRivalry(string playerActorId, string npcActorId, string incidentId, int day, float intensity01 = 0.25f)
        {
            var existing = _state.rivalries.FirstOrDefault(r => r.playerActorId == playerActorId && r.npcActorId == npcActorId);
            if (existing != null) return existing;
            var rivalry = new RivalryState
            {
                playerActorId = playerActorId ?? string.Empty,
                npcActorId = npcActorId ?? string.Empty,
                originIncidentId = incidentId ?? string.Empty,
                intensity01 = Math.Clamp(intensity01, 0f, 1f),
                lastEncounterDay = day
            };
            _state.rivalries.Add(rivalry);
            OnRivalryCreated?.Invoke(rivalry);
            return rivalry;
        }

        public ProceduralNarrativeState CaptureState()
            => new ProceduralNarrativeState
            {
                systemId = _state.systemId,
                templateCooldowns = new Dictionary<string, int>(_state.templateCooldowns, StringComparer.Ordinal),
                rivalries = _state.rivalries.Select(CloneRivalry).ToList(),
                recentTemplateIds = new List<string>(_state.recentTemplateIds),
                generationSequence = _state.generationSequence
            };

        public void RestoreState(ProceduralNarrativeState saved)
        {
            if (saved == null) return;
            _state = new ProceduralNarrativeState
            {
                systemId = string.IsNullOrWhiteSpace(saved.systemId) ? SystemId : saved.systemId,
                templateCooldowns = saved.templateCooldowns != null ? new Dictionary<string, int>(saved.templateCooldowns, StringComparer.Ordinal) : new Dictionary<string, int>(StringComparer.Ordinal),
                rivalries = saved.rivalries?.Where(r => r != null).Select(CloneRivalry).ToList() ?? new List<RivalryState>(),
                recentTemplateIds = saved.recentTemplateIds != null ? new List<string>(saved.recentTemplateIds) : new List<string>(),
                generationSequence = Math.Max(0, saved.generationSequence)
            };
        }

        private static bool IsSupportedModule(string module)
            => module == "repair_pipe" || module == "trace_contamination" || module == "rescue_agent"
                || module == "find_missing_person" || module == "secure_filter_media";

        private static int MinDeadline(int first, int second)
            => first < 0 ? second : second < 0 ? first : Math.Min(first, second);

        private static QuestObjectiveRuntimeState CloneObjective(QuestObjectiveRuntimeState source)
            => new QuestObjectiveRuntimeState { objectiveId = source.objectiveId, moduleId = source.moduleId, completed = source.completed, progress = source.progress, requiredAmount = source.requiredAmount };

        private static RivalryState CloneRivalry(RivalryState source)
            => new RivalryState { playerActorId = source.playerActorId, npcActorId = source.npcActorId, originIncidentId = source.originIncidentId, intensity01 = Math.Clamp(source.intensity01, 0f, 1f), lastEncounterDay = source.lastEncounterDay, status = source.status };
    }
}
