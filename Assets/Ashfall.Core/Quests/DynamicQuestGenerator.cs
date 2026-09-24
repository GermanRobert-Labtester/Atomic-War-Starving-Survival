// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Narrative;

namespace Ashfall.Core.Quests
{
    public enum ProceduralQuestType
    {
        FetchResource = 0,
        EscortSurvivor = 1,
        InvestigateAnomaly = 2,
        DefendOutpost = 3,
        DiplomaticRelay = 4,
        ScoutExploration = 5,
        DisputeResolution = 6
    }

    public enum ProceduralQuestStatus
    {
        Available = 0,
        Active = 1,
        Completed = 2,
        Failed = 3,
        Expired = 4
    }

    [Serializable]
    public sealed class ProceduralQuestTemplate
    {
        public string TemplateId { get; set; } = string.Empty;
        public ProceduralQuestType Type { get; set; } = ProceduralQuestType.FetchResource;
        public string TitleTemplate { get; set; } = string.Empty;
        public string DescriptionTemplate { get; set; } = string.Empty;
        public int BaseDifficulty { get; set; } = 1;
        public string TargetLocationId { get; set; } = string.Empty;
        public string TargetResourceId { get; set; } = string.Empty;
        public int RequiredQuantity { get; set; } = 1;
        public float MoraleReward { get; set; } = 5f;
        public float FactionStandingReward { get; set; } = 0f;
        public string TargetFactionId { get; set; } = string.Empty;
        public string RewardItemId { get; set; } = string.Empty;
        public int RewardItemCount { get; set; }
    }

    [Serializable]
    public sealed class ProceduralQuest
    {
        public string QuestId { get; set; } = string.Empty;
        public string TemplateId { get; set; } = string.Empty;
        public ProceduralQuestType Type { get; set; } = ProceduralQuestType.FetchResource;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int GeneratedDay { get; set; } = 1;
        public int DeadlineDay { get; set; } = 10;
        public string AssignedSurvivorId { get; set; } = string.Empty;
        public ProceduralQuestStatus Status { get; set; } = ProceduralQuestStatus.Available;
        public string TargetLocationId { get; set; } = string.Empty;
        public string TargetResourceId { get; set; } = string.Empty;
        public int RequiredQuantity { get; set; } = 1;
        public int CurrentQuantity { get; set; } = 0;
        public float MoraleReward { get; set; } = 5f;
        public float FactionStandingReward { get; set; } = 0f;
        public string TargetFactionId { get; set; } = string.Empty;
        public string RewardItemId { get; set; } = string.Empty;
        public int RewardItemCount { get; set; }

        public bool IsFulfilled => CurrentQuantity >= RequiredQuantity;
    }

    [Serializable]
    public sealed class DynamicQuestGeneratorState
    {
        public int SchemaVersion { get; set; } = 1;
        public int NextSequence { get; set; } = 1;
        public int LastGenerationDay { get; set; } = 0;
        public int CooldownDays { get; set; } = 3;
        public int MaxActiveQuests { get; set; } = 3;
        public List<ProceduralQuest> Quests { get; set; } = new List<ProceduralQuest>();
        public List<string> CompletedQuestIds { get; set; } = new List<string>();
        public List<string> FailedQuestIds { get; set; } = new List<string>();
    }

    /// <summary>
    /// Plan 171 / C1[30] — Dynamic Quest Generation System.
    /// Procedurally synthesizes emergent, parameterized shelter quests from templates
    /// responding to world events, resource scarcity, and faction relations.
    /// </summary>
    public sealed class DynamicQuestGenerator
    {
        private readonly DynamicQuestGeneratorState _state;
        private readonly List<ProceduralQuestTemplate> _templates = new List<ProceduralQuestTemplate>();

        public event Action<ProceduralQuest>? OnQuestGenerated;
        public event Action<ProceduralQuest>? OnQuestAccepted;
        public event Action<ProceduralQuest>? OnQuestCompleted;
        public event Action<ProceduralQuest>? OnQuestExpired;

        public int AvailableCount => _state.Quests.Count(q => q.Status == ProceduralQuestStatus.Available);
        public int ActiveCount => _state.Quests.Count(q => q.Status == ProceduralQuestStatus.Active);
        public int CompletedCount => _state.CompletedQuestIds.Count;

        public DynamicQuestGenerator(DynamicQuestGeneratorState? state = null)
        {
            _state = state ?? new DynamicQuestGeneratorState();
            InitializeDefaultTemplates();
        }

        /// <summary>
        /// Plan 171 production adapter. The canonical procedural narrative
        /// system owns JSON templates, eligibility, deterministic bindings,
        /// cooldowns, and quest lifecycle. This bridge only hands its accepted
        /// candidate to the existing QuestRuntimeCoordinator; it never copies
        /// lifecycle state into DynamicQuestGeneratorState.
        /// </summary>
        public static bool TryGenerateAndRegisterCanonicalCandidate(
            ProceduralNarrativeSystem templateAuthority,
            QuestRuntimeCoordinator runtime,
            NarrativeWorldSnapshot snapshot,
            ISeededRng rng,
            out ProceduralQuestDraft draft)
        {
            draft = new ProceduralQuestDraft();
            if (templateAuthority == null || runtime == null || snapshot == null || rng == null)
            {
                draft.rejectionReason = "missing_input";
                return false;
            }

            if (!templateAuthority.TryGenerate(snapshot, rng, out draft)) return false;
            return runtime.Register(draft.quest);
        }

        private void InitializeDefaultTemplates()
        {
            _templates.Add(new ProceduralQuestTemplate
            {
                TemplateId = "tmpl_fetch_medical",
                Type = ProceduralQuestType.FetchResource,
                TitleTemplate = "Urgent Supplies: Medicine",
                DescriptionTemplate = "A stock of essential medicines must be procured from the surrounding ruins.",
                TargetResourceId = "item_medkit",
                RequiredQuantity = 2,
                MoraleReward = 8f
            });

            _templates.Add(new ProceduralQuestTemplate
            {
                TemplateId = "tmpl_investigate_radio",
                Type = ProceduralQuestType.InvestigateAnomaly,
                TitleTemplate = "Investigate Signal Anomaly",
                DescriptionTemplate = "A repetitive transmission was detected near the communication relay.",
                TargetLocationId = "loc_radio_depot",
                RequiredQuantity = 1,
                MoraleReward = 5f
            });

            _templates.Add(new ProceduralQuestTemplate
            {
                TemplateId = "tmpl_diplomatic_courier",
                Type = ProceduralQuestType.DiplomaticRelay,
                TitleTemplate = "Diplomatic Message Delivery",
                DescriptionTemplate = "Deliver sealed terms to the forward outpost.",
                TargetLocationId = "loc_forward_outpost",
                TargetFactionId = "faction_central_garrison",
                FactionStandingReward = 5f,
                MoraleReward = 3f
            });
        }

        public void RegisterTemplate(ProceduralQuestTemplate template)
        {
            if (template == null || string.IsNullOrEmpty(template.TemplateId)) return;
            _templates.RemoveAll(t => string.Equals(t.TemplateId, template.TemplateId, StringComparison.OrdinalIgnoreCase));
            _templates.Add(template);
        }

        /// <summary>
        /// Plan 171 — replaces the built-in templates with an already-validated
        /// authored catalog. The host uses the strict
        /// <see cref="DynamicQuestTemplateCatalogLoader"/> and this seam, so the
        /// built-in defaults cannot mask an authored typo.
        /// </summary>
        public void BindAuthoredTemplates(IEnumerable<ProceduralQuestTemplate> templates)
        {
            if (templates == null) return;
            _templates.Clear();
            foreach (var template in templates)
            {
                if (template != null && !string.IsNullOrEmpty(template.TemplateId))
                    _templates.Add(template);
            }
        }

        /// <summary>Read-only census of the live generator (Plan 171).</summary>
        public DynamicQuestGeneratorCensus GetCensus()
        {
            return new DynamicQuestGeneratorCensus(
                _templates.Count,
                _state.Quests.Count,
                AvailableCount,
                ActiveCount,
                _state.CompletedQuestIds.Count,
                _state.FailedQuestIds.Count,
                _state.Quests.Count(q => q.Status == ProceduralQuestStatus.Expired));
        }

        public IReadOnlyList<ProceduralQuest> GenerateQuests(int currentDay, ISeededRng? rng = null)
        {
            if (_state.LastGenerationDay > 0 && currentDay - _state.LastGenerationDay < _state.CooldownDays)
            {
                return Array.Empty<ProceduralQuest>();
            }

            int currentNonCompleted = _state.Quests.Count(q =>
                q.Status == ProceduralQuestStatus.Available || q.Status == ProceduralQuestStatus.Active);

            if (currentNonCompleted >= _state.MaxActiveQuests)
            {
                return Array.Empty<ProceduralQuest>();
            }

            var generated = new List<ProceduralQuest>();
            int availableSlots = _state.MaxActiveQuests - currentNonCompleted;

            for (int i = 0; i < availableSlots && i < _templates.Count; i++)
            {
                int templateIndex = rng != null ? rng.Next(0, _templates.Count) : i;
                var tmpl = _templates[templateIndex];

                // Avoid duplicate available templates
                if (_state.Quests.Any(q => q.TemplateId == tmpl.TemplateId && q.Status == ProceduralQuestStatus.Available))
                    continue;

                var quest = new ProceduralQuest
                {
                    QuestId = $"pquest_{_state.NextSequence++}",
                    TemplateId = tmpl.TemplateId,
                    Type = tmpl.Type,
                    Title = tmpl.TitleTemplate,
                    Description = tmpl.DescriptionTemplate,
                    GeneratedDay = currentDay,
                    DeadlineDay = currentDay + 7,
                    Status = ProceduralQuestStatus.Available,
                    TargetLocationId = tmpl.TargetLocationId,
                    TargetResourceId = tmpl.TargetResourceId,
                    RequiredQuantity = tmpl.RequiredQuantity,
                    CurrentQuantity = 0,
                    MoraleReward = tmpl.MoraleReward,
                    FactionStandingReward = tmpl.FactionStandingReward,
                    TargetFactionId = tmpl.TargetFactionId,
                    RewardItemId = tmpl.RewardItemId,
                    RewardItemCount = tmpl.RewardItemCount
                };

                _state.Quests.Add(quest);
                generated.Add(quest);
                OnQuestGenerated?.Invoke(quest);
            }

            if (generated.Count > 0)
            {
                _state.LastGenerationDay = currentDay;
            }

            return generated;
        }

        public bool AcceptQuest(string questId, string assignedSurvivorId = "")
        {
            var quest = _state.Quests.FirstOrDefault(q =>
                string.Equals(q.QuestId, questId, StringComparison.OrdinalIgnoreCase) &&
                q.Status == ProceduralQuestStatus.Available);

            if (quest == null) return false;

            quest.Status = ProceduralQuestStatus.Active;
            quest.AssignedSurvivorId = assignedSurvivorId ?? string.Empty;
            OnQuestAccepted?.Invoke(quest);
            return true;
        }

        public bool ProgressQuest(string questId, int amount = 1)
        {
            var quest = _state.Quests.FirstOrDefault(q =>
                string.Equals(q.QuestId, questId, StringComparison.OrdinalIgnoreCase) &&
                q.Status == ProceduralQuestStatus.Active);

            if (quest == null) return false;

            quest.CurrentQuantity += Math.Max(1, amount);
            return true;
        }

        public bool CompleteQuest(string questId, int currentDay)
        {
            var quest = _state.Quests.FirstOrDefault(q =>
                string.Equals(q.QuestId, questId, StringComparison.OrdinalIgnoreCase) &&
                q.Status == ProceduralQuestStatus.Active);

            if (quest == null || !quest.IsFulfilled) return false;

            quest.Status = ProceduralQuestStatus.Completed;
            _state.CompletedQuestIds.Add(quest.QuestId);
            OnQuestCompleted?.Invoke(quest);
            return true;
        }

        public void CheckDeadlines(int currentDay)
        {
            for (int i = _state.Quests.Count - 1; i >= 0; i--)
            {
                var q = _state.Quests[i];
                if (q.Status == ProceduralQuestStatus.Available || q.Status == ProceduralQuestStatus.Active)
                {
                    if (currentDay > q.DeadlineDay)
                    {
                        if (q.Status == ProceduralQuestStatus.Active)
                        {
                            q.Status = ProceduralQuestStatus.Failed;
                            _state.FailedQuestIds.Add(q.QuestId);
                        }
                        else
                        {
                            q.Status = ProceduralQuestStatus.Expired;
                        }

                        OnQuestExpired?.Invoke(q);
                    }
                }
            }
        }

        public DynamicQuestGeneratorState CaptureState()
        {
            var captured = new DynamicQuestGeneratorState
            {
                SchemaVersion = _state.SchemaVersion,
                NextSequence = _state.NextSequence,
                LastGenerationDay = _state.LastGenerationDay,
                CooldownDays = _state.CooldownDays,
                MaxActiveQuests = _state.MaxActiveQuests,
                Quests = new List<ProceduralQuest>(_state.Quests.Count),
                CompletedQuestIds = new List<string>(_state.CompletedQuestIds),
                FailedQuestIds = new List<string>(_state.FailedQuestIds)
            };

            for (int i = 0; i < _state.Quests.Count; i++)
            {
                var q = _state.Quests[i];
                captured.Quests.Add(new ProceduralQuest
                {
                    QuestId = q.QuestId,
                    TemplateId = q.TemplateId,
                    Type = q.Type,
                    Title = q.Title,
                    Description = q.Description,
                    GeneratedDay = q.GeneratedDay,
                    DeadlineDay = q.DeadlineDay,
                    AssignedSurvivorId = q.AssignedSurvivorId,
                    Status = q.Status,
                    TargetLocationId = q.TargetLocationId,
                    TargetResourceId = q.TargetResourceId,
                    RequiredQuantity = q.RequiredQuantity,
                    CurrentQuantity = q.CurrentQuantity,
                    MoraleReward = q.MoraleReward,
                    FactionStandingReward = q.FactionStandingReward,
                    TargetFactionId = q.TargetFactionId,
                    RewardItemId = q.RewardItemId,
                    RewardItemCount = q.RewardItemCount
                });
            }

            return captured;
        }

        public void RestoreState(DynamicQuestGeneratorState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            // Schema gate: a newer payload must not be silently down-cast.
            if (state.SchemaVersion > 1)
                throw new InvalidOperationException(
                    $"DynamicQuestGeneratorState schema {state.SchemaVersion} is newer than supported (1).");
            if (state.SchemaVersion < 1)
                state.SchemaVersion = 1;

            _state.SchemaVersion = state.SchemaVersion;
            _state.NextSequence = state.NextSequence;
            _state.LastGenerationDay = state.LastGenerationDay;
            _state.CooldownDays = state.CooldownDays;
            _state.MaxActiveQuests = state.MaxActiveQuests;
            _state.Quests.Clear();
            _state.CompletedQuestIds.Clear();
            _state.FailedQuestIds.Clear();

            if (state.Quests != null)
            {
                for (int i = 0; i < state.Quests.Count; i++)
                {
                    var q = state.Quests[i];
                    _state.Quests.Add(new ProceduralQuest
                    {
                        QuestId = q.QuestId,
                        TemplateId = q.TemplateId,
                        Type = q.Type,
                        Title = q.Title,
                        Description = q.Description,
                        GeneratedDay = q.GeneratedDay,
                        DeadlineDay = q.DeadlineDay,
                        AssignedSurvivorId = q.AssignedSurvivorId,
                        Status = q.Status,
                        TargetLocationId = q.TargetLocationId,
                        TargetResourceId = q.TargetResourceId,
                        RequiredQuantity = q.RequiredQuantity,
                        CurrentQuantity = q.CurrentQuantity,
                        MoraleReward = q.MoraleReward,
                        FactionStandingReward = q.FactionStandingReward,
                        TargetFactionId = q.TargetFactionId,
                        RewardItemId = q.RewardItemId,
                        RewardItemCount = q.RewardItemCount
                    });
                }
            }

            if (state.CompletedQuestIds != null)
            {
                _state.CompletedQuestIds.AddRange(state.CompletedQuestIds);
            }

            if (state.FailedQuestIds != null)
            {
                _state.FailedQuestIds.AddRange(state.FailedQuestIds);
            }
        }
    }

    /// <summary>
    /// Read-only census of the live dynamic quest generator (Plan 171). Exposed
    /// for the architecture scanner and the host self-test probe.
    /// </summary>
    public struct DynamicQuestGeneratorCensus
    {
        public int TemplateCount { get; }
        public int TotalQuests { get; }
        public int AvailableQuests { get; }
        public int ActiveQuests { get; }
        public int CompletedQuests { get; }
        public int FailedQuests { get; }
        public int ExpiredQuests { get; }

        public DynamicQuestGeneratorCensus(
            int templateCount,
            int totalQuests,
            int availableQuests,
            int activeQuests,
            int completedQuests,
            int failedQuests,
            int expiredQuests)
        {
            TemplateCount = templateCount;
            TotalQuests = totalQuests;
            AvailableQuests = availableQuests;
            ActiveQuests = activeQuests;
            CompletedQuests = completedQuests;
            FailedQuests = failedQuests;
            ExpiredQuests = expiredQuests;
        }
    }
}
