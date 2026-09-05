// SPDX-License-Identifier: MIT
// ASHFALL survivor personal quest core authority (Plan 83 / Task B24).

using System;
using System.Collections.Generic;
using Ashfall.Core.Campaign;

namespace Ashfall.Core.Quests
{
    public enum PersonalQuestStatus
    {
        Active = 0,
        Completed = 1,
        Failed = 2,
        Abandoned = 3
    }

    [Serializable]
    public sealed class PersonalQuestChoiceDef
    {
        public string choice_id { get; set; } = string.Empty;
        public string label { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public float morale_delta { get; set; }
        public string reward_item_id { get; set; } = string.Empty;
        public int reward_amount { get; set; }
        public int next_stage { get; set; } = -1; // -1 indicates completion
        public string consequence_summary { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class PersonalQuestStageDef
    {
        public int stage_index { get; set; }
        public string title { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public string requirement_kind { get; set; } = string.Empty; // e.g. "days_elapsed", "deliver_item"
        public string target_id { get; set; } = string.Empty;
        public int target_count { get; set; }
        public List<PersonalQuestChoiceDef> choices { get; set; } = new();
    }

    [Serializable]
    public sealed class PersonalQuestDef
    {
        public string id { get; set; } = string.Empty;
        public string title { get; set; } = string.Empty;
        public string category { get; set; } = string.Empty;
        public string required_trait { get; set; } = string.Empty;
        public string summary { get; set; } = string.Empty;
        public List<PersonalQuestStageDef> stages { get; set; } = new();
    }

    [Serializable]
    public sealed class PersonalQuestCatalogData
    {
        public int schema_version { get; set; } = 1;
        public List<PersonalQuestDef> quests { get; set; } = new();
    }

    [Serializable]
    public sealed class PersonalQuestInstance
    {
        public string questId { get; set; } = string.Empty;
        public string survivorId { get; set; } = string.Empty;
        public int currentStage { get; set; }
        public PersonalQuestStatus status { get; set; }
        public int progressCount { get; set; }
        public int startedDay { get; set; }
        public int resolvedDay { get; set; }
        public string failureReason { get; set; } = string.Empty;
        public List<string> selectedChoices { get; set; } = new();
    }

    [Serializable]
    public sealed class PersonalQuestSaveState
    {
        public int schema_version { get; set; } = 1;
        public string systemId { get; set; } = PersonalQuestSystem.SystemId;
        public List<PersonalQuestInstance> activeQuests { get; set; } = new();
        public List<PersonalQuestInstance> completedQuests { get; set; } = new();
    }

    public sealed class PersonalQuestSystem
    {
        public const string SystemId = "personal_quests";

        private readonly Dictionary<string, PersonalQuestDef> _catalog = new(StringComparer.Ordinal);
        private PersonalQuestSaveState _state = new();
        private readonly ISeededRng _rng;
        private readonly ILog _log;

        public event Action<PersonalQuestInstance>? OnQuestStarted;
        public event Action<PersonalQuestInstance, int>? OnStageAdvanced;
        public event Action<PersonalQuestInstance, PersonalQuestChoiceDef>? OnChoiceMade;
        public event Action<PersonalQuestInstance>? OnQuestCompleted;
        public event Action<PersonalQuestInstance, string>? OnQuestFailed;

        public PersonalQuestSaveState State => _state;
        public IReadOnlyDictionary<string, PersonalQuestDef> Catalog => _catalog;

        public PersonalQuestSystem(ISeededRng? rng = null, ILog? log = null)
        {
            _rng = rng ?? new SeededRng(42);
            _log = log ?? NullLog.Instance;
        }

        public void LoadCatalog(string json, IJsonSerializer serializer)
        {
            if (string.IsNullOrEmpty(json)) return;
            var data = serializer.Deserialize<PersonalQuestCatalogData>(json);
            if (data?.quests == null) return;
            _catalog.Clear();
            foreach (var q in data.quests)
            {
                if (!string.IsNullOrEmpty(q.id))
                    _catalog[q.id] = q;
            }
        }

        public PersonalQuestInstance? GetActiveQuest(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return null;
            for (int i = 0; i < _state.activeQuests.Count; i++)
            {
                if (string.Equals(_state.activeQuests[i].survivorId, survivorId, StringComparison.Ordinal))
                    return _state.activeQuests[i];
            }
            return null;
        }

        public IReadOnlyList<PersonalQuestInstance> ActiveQuests => _state.activeQuests;
        public IReadOnlyList<PersonalQuestInstance> CompletedQuests => _state.completedQuests;

        public bool TryTriggerQuest(string survivorId, string trait, int day)
        {
            if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(trait)) return false;
            if (GetActiveQuest(survivorId) != null) return false;

            PersonalQuestDef? matched = null;
            foreach (var q in _catalog.Values)
            {
                if (string.Equals(q.required_trait, trait, StringComparison.OrdinalIgnoreCase))
                {
                    bool alreadyUsed = false;
                    for (int i = 0; i < _state.completedQuests.Count; i++)
                    {
                        if (_state.completedQuests[i].questId == q.id && _state.completedQuests[i].survivorId == survivorId)
                        {
                            alreadyUsed = true;
                            break;
                        }
                    }
                    if (!alreadyUsed)
                    {
                        matched = q;
                        break;
                    }
                }
            }

            if (matched == null) return false;

            var instance = new PersonalQuestInstance
            {
                questId = matched.id,
                survivorId = survivorId,
                currentStage = 0,
                status = PersonalQuestStatus.Active,
                progressCount = 0,
                startedDay = day,
                resolvedDay = -1
            };
            _state.activeQuests.Add(instance);
            OnQuestStarted?.Invoke(instance);
            return true;
        }

        public bool ProgressRequirement(string survivorId, string requirementKind, int amount, string? targetId = null)
        {
            var instance = GetActiveQuest(survivorId);
            if (instance == null || instance.status != PersonalQuestStatus.Active) return false;
            if (!_catalog.TryGetValue(instance.questId, out var def)) return false;
            if (instance.currentStage < 0 || instance.currentStage >= def.stages.Count) return false;

            var stage = def.stages[instance.currentStage];
            if (!string.Equals(stage.requirement_kind, requirementKind, StringComparison.OrdinalIgnoreCase))
                return false;

            if (!string.IsNullOrEmpty(stage.target_id) && targetId != null &&
                !string.Equals(stage.target_id, targetId, StringComparison.OrdinalIgnoreCase))
                return false;

            instance.progressCount += amount;
            return true;
        }

        public bool ChooseOption(string survivorId, string choiceId, int currentDay, out PersonalQuestChoiceDef? chosenDef)
        {
            chosenDef = null;
            var instance = GetActiveQuest(survivorId);
            if (instance == null || instance.status != PersonalQuestStatus.Active) return false;
            if (!_catalog.TryGetValue(instance.questId, out var def)) return false;
            if (instance.currentStage < 0 || instance.currentStage >= def.stages.Count) return false;

            var stage = def.stages[instance.currentStage];
            PersonalQuestChoiceDef? match = null;
            for (int i = 0; i < stage.choices.Count; i++)
            {
                if (string.Equals(stage.choices[i].choice_id, choiceId, StringComparison.Ordinal))
                {
                    match = stage.choices[i];
                    break;
                }
            }

            if (match == null) return false;

            chosenDef = match;
            instance.selectedChoices.Add(choiceId);
            OnChoiceMade?.Invoke(instance, match);

            if (match.next_stage < 0 || match.next_stage >= def.stages.Count)
            {
                instance.status = PersonalQuestStatus.Completed;
                instance.resolvedDay = currentDay;
                _state.activeQuests.Remove(instance);
                _state.completedQuests.Add(instance);
                OnQuestCompleted?.Invoke(instance);
            }
            else
            {
                instance.currentStage = match.next_stage;
                instance.progressCount = 0;
                OnStageAdvanced?.Invoke(instance, instance.currentStage);
            }

            return true;
        }

        public bool FailQuest(string survivorId, string reason, int currentDay)
        {
            var instance = GetActiveQuest(survivorId);
            if (instance == null) return false;

            instance.status = PersonalQuestStatus.Failed;
            instance.failureReason = reason ?? "failed";
            instance.resolvedDay = currentDay;
            _state.activeQuests.Remove(instance);
            _state.completedQuests.Add(instance);
            OnQuestFailed?.Invoke(instance, instance.failureReason);
            return true;
        }

        public void TickDay(int day, IList<DayStateChangeEvent>? events = null)
        {
            for (int i = _state.activeQuests.Count - 1; i >= 0; i--)
            {
                var inst = _state.activeQuests[i];
                if (inst.status != PersonalQuestStatus.Active) continue;
                if (!_catalog.TryGetValue(inst.questId, out var def)) continue;
                if (inst.currentStage < 0 || inst.currentStage >= def.stages.Count) continue;

                var stage = def.stages[inst.currentStage];
                if (string.Equals(stage.requirement_kind, "days_elapsed", StringComparison.OrdinalIgnoreCase))
                {
                    inst.progressCount++;
                    events?.Add(new DayStateChangeEvent("personal_quest_progressed", SystemId, inst.questId, inst.survivorId, inst.progressCount));
                }
            }
        }

        public PersonalQuestSaveState CaptureState()
        {
            var copy = new PersonalQuestSaveState
            {
                schema_version = _state.schema_version,
                systemId = _state.systemId
            };
            for (int i = 0; i < _state.activeQuests.Count; i++)
            {
                var a = _state.activeQuests[i];
                copy.activeQuests.Add(new PersonalQuestInstance
                {
                    questId = a.questId,
                    survivorId = a.survivorId,
                    currentStage = a.currentStage,
                    status = a.status,
                    progressCount = a.progressCount,
                    startedDay = a.startedDay,
                    resolvedDay = a.resolvedDay,
                    failureReason = a.failureReason,
                    selectedChoices = new List<string>(a.selectedChoices)
                });
            }
            for (int i = 0; i < _state.completedQuests.Count; i++)
            {
                var c = _state.completedQuests[i];
                copy.completedQuests.Add(new PersonalQuestInstance
                {
                    questId = c.questId,
                    survivorId = c.survivorId,
                    currentStage = c.currentStage,
                    status = c.status,
                    progressCount = c.progressCount,
                    startedDay = c.startedDay,
                    resolvedDay = c.resolvedDay,
                    failureReason = c.failureReason,
                    selectedChoices = new List<string>(c.selectedChoices)
                });
            }
            return copy;
        }

        public void RestoreState(PersonalQuestSaveState saved)
        {
            if (saved == null) return;
            _state = new PersonalQuestSaveState
            {
                schema_version = saved.schema_version,
                systemId = saved.systemId
            };
            if (saved.activeQuests != null)
            {
                for (int i = 0; i < saved.activeQuests.Count; i++)
                {
                    var a = saved.activeQuests[i];
                    _state.activeQuests.Add(new PersonalQuestInstance
                    {
                        questId = a.questId,
                        survivorId = a.survivorId,
                        currentStage = a.currentStage,
                        status = a.status,
                        progressCount = a.progressCount,
                        startedDay = a.startedDay,
                        resolvedDay = a.resolvedDay,
                        failureReason = a.failureReason,
                        selectedChoices = new List<string>(a.selectedChoices ?? new List<string>())
                    });
                }
            }
            if (saved.completedQuests != null)
            {
                for (int i = 0; i < saved.completedQuests.Count; i++)
                {
                    var c = saved.completedQuests[i];
                    _state.completedQuests.Add(new PersonalQuestInstance
                    {
                        questId = c.questId,
                        survivorId = c.survivorId,
                        currentStage = c.currentStage,
                        status = c.status,
                        progressCount = c.progressCount,
                        startedDay = c.startedDay,
                        resolvedDay = c.resolvedDay,
                        failureReason = c.failureReason,
                        selectedChoices = new List<string>(c.selectedChoices ?? new List<string>())
                    });
                }
            }
        }
    }
}
