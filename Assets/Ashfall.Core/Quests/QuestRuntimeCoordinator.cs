// SPDX-License-Identifier: MIT
// ASHFALL Core: one player-facing quest runtime read model.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Quests
{
    public enum QuestSourceKind
    {
        Static,
        Procedural,
        DynamicEvent,
        Holdfast,
        Expansion
    }

    public enum QuestLifecycleState
    {
        Offered,
        Active,
        Completed,
        Failed,
        Expired,
        Abandoned
    }

    [Serializable]
    public sealed class QuestObjectiveRuntimeState
    {
        public string objectiveId = string.Empty;
        public string moduleId = string.Empty;
        public bool completed;
        public int progress;
        public int requiredAmount = 1;
    }

    [Serializable]
    public sealed class QuestInstanceState
    {
        public string instanceId = string.Empty;
        public string definitionId = string.Empty;
        public QuestSourceKind sourceKind;
        public string titleKey = string.Empty;
        public string descriptionKey = string.Empty;
        public int createdDay;
        public int expiryDay = -1;
        public QuestLifecycleState status = QuestLifecycleState.Active;
        public Dictionary<string, string> actorBindings = new Dictionary<string, string>(StringComparer.Ordinal);
        public List<string> locationBindings = new List<string>();
        public List<QuestObjectiveRuntimeState> objectiveStates = new List<QuestObjectiveRuntimeState>();
        public List<string> rewardBindings = new List<string>();
        public List<string> failureConsequences = new List<string>();
        public int generationSeed;
        public string parentQuestInstanceId = string.Empty;
        public string mergeGroupId = string.Empty;
        public List<string> childInstanceIds = new List<string>();
    }

    [Serializable]
    public sealed class QuestRuntimeState
    {
        public string systemId = "quest_runtime";
        public List<QuestInstanceState> quests = new List<QuestInstanceState>();
    }

    [Serializable]
    public sealed class QuestLogEntry
    {
        public string instanceId = string.Empty;
        public QuestSourceKind sourceKind;
        public string titleKey = string.Empty;
        public string descriptionKey = string.Empty;
        public QuestLifecycleState status;
        public int expiryDay = -1;
        public List<QuestObjectiveRuntimeState> objectives = new List<QuestObjectiveRuntimeState>();
        public List<string> locationIds = new List<string>();
        public List<string> actorIds = new List<string>();
        public bool isProcedural;
    }

    /// <summary>
    /// Aggregates authored and generated quest instances without taking ownership
    /// of mature domain-specific quest logic. The coordinator owns lifecycle,
    /// deadlines, and the player-facing read model.
    /// </summary>
    public sealed class QuestRuntimeCoordinator
    {
        private QuestRuntimeState _state;
        private readonly ILog _log;

        public QuestRuntimeState State => _state;
        public event Action<QuestInstanceState>? OnQuestRegistered;
        public event Action<QuestInstanceState>? OnQuestCompleted;
        public event Action<QuestInstanceState>? OnQuestFailed;
        public event Action<QuestInstanceState>? OnQuestExpired;

        public QuestRuntimeCoordinator(ILog? log = null, QuestRuntimeState? state = null)
        {
            _log = log ?? NullLog.Instance;
            _state = state ?? new QuestRuntimeState();
        }

        public bool Register(QuestInstanceState quest)
        {
            if (!IsValid(quest) || _state.quests.Any(q => q != null && q.instanceId == quest.instanceId)) return false;
            _state.quests.Add(CloneQuest(quest));
            OnQuestRegistered?.Invoke(_state.quests[_state.quests.Count - 1]);
            return true;
        }

        public QuestInstanceState? Find(string instanceId)
            => _state.quests.FirstOrDefault(q => q != null && string.Equals(q.instanceId, instanceId, StringComparison.Ordinal));

        public IReadOnlyList<QuestLogEntry> BuildReadModel()
        {
            return _state.quests.Where(q => q != null).OrderBy(q => q.createdDay).ThenBy(q => q.instanceId, StringComparer.Ordinal)
                .Select(q => new QuestLogEntry
                {
                    instanceId = q.instanceId,
                    sourceKind = q.sourceKind,
                    titleKey = q.titleKey,
                    descriptionKey = q.descriptionKey,
                    status = q.status,
                    expiryDay = q.expiryDay,
                    objectives = q.objectiveStates?.Select(CloneObjective).ToList() ?? new List<QuestObjectiveRuntimeState>(),
                    locationIds = q.locationBindings != null ? new List<string>(q.locationBindings) : new List<string>(),
                    actorIds = q.actorBindings?.Values.OrderBy(id => id, StringComparer.Ordinal).ToList() ?? new List<string>(),
                    isProcedural = q.sourceKind == QuestSourceKind.Procedural
                }).ToList();
        }

        public bool Complete(string instanceId)
        {
            var quest = Find(instanceId);
            if (quest == null || quest.status != QuestLifecycleState.Active || HasIncompleteObjectives(quest)) return false;
            quest.status = QuestLifecycleState.Completed;
            OnQuestCompleted?.Invoke(quest);
            return true;
        }

        public bool Fail(string instanceId)
        {
            var quest = Find(instanceId);
            if (quest == null || quest.status != QuestLifecycleState.Active) return false;
            quest.status = QuestLifecycleState.Failed;
            OnQuestFailed?.Invoke(quest);
            return true;
        }

        public void Tick(int day)
        {
            foreach (var quest in _state.quests.Where(q => q != null).ToList())
            {
                if (quest.status != QuestLifecycleState.Active || quest.expiryDay < 0 || day < quest.expiryDay) continue;
                quest.status = QuestLifecycleState.Expired;
                OnQuestExpired?.Invoke(quest);
            }
        }

        public QuestRuntimeState CaptureState()
            => new QuestRuntimeState
            {
                systemId = _state.systemId,
                quests = _state.quests.Where(q => q != null).Select(CloneQuest).ToList()
            };

        public void RestoreState(QuestRuntimeState saved)
        {
            if (saved == null) return;
            _state = new QuestRuntimeState
            {
                systemId = string.IsNullOrWhiteSpace(saved.systemId) ? "quest_runtime" : saved.systemId,
                quests = saved.quests?.Where(q => q != null).Select(CloneQuest).ToList() ?? new List<QuestInstanceState>()
            };
        }

        private static bool IsValid(QuestInstanceState quest)
            => quest != null && !string.IsNullOrWhiteSpace(quest.instanceId)
                && !string.IsNullOrWhiteSpace(quest.definitionId)
                && quest.objectiveStates != null
                && quest.objectiveStates.All(o => o != null && !string.IsNullOrWhiteSpace(o.objectiveId));

        private static bool HasIncompleteObjectives(QuestInstanceState quest)
            => quest.objectiveStates != null && quest.objectiveStates.Any(o => o != null && !o.completed);

        private static QuestInstanceState CloneQuest(QuestInstanceState source)
            => new QuestInstanceState
            {
                instanceId = source.instanceId,
                definitionId = source.definitionId,
                sourceKind = source.sourceKind,
                titleKey = source.titleKey,
                descriptionKey = source.descriptionKey,
                createdDay = source.createdDay,
                expiryDay = source.expiryDay,
                status = source.status,
                actorBindings = source.actorBindings != null
                    ? new Dictionary<string, string>(source.actorBindings, StringComparer.Ordinal)
                    : new Dictionary<string, string>(StringComparer.Ordinal),
                locationBindings = source.locationBindings != null ? new List<string>(source.locationBindings) : new List<string>(),
                objectiveStates = source.objectiveStates?.Where(o => o != null).Select(CloneObjective).ToList() ?? new List<QuestObjectiveRuntimeState>(),
                rewardBindings = source.rewardBindings != null ? new List<string>(source.rewardBindings) : new List<string>(),
                failureConsequences = source.failureConsequences != null ? new List<string>(source.failureConsequences) : new List<string>(),
                generationSeed = source.generationSeed,
                parentQuestInstanceId = source.parentQuestInstanceId ?? string.Empty,
                mergeGroupId = source.mergeGroupId ?? string.Empty,
                childInstanceIds = source.childInstanceIds != null ? new List<string>(source.childInstanceIds) : new List<string>()
            };

        private static QuestObjectiveRuntimeState CloneObjective(QuestObjectiveRuntimeState source)
            => new QuestObjectiveRuntimeState
            {
                objectiveId = source.objectiveId,
                moduleId = source.moduleId,
                completed = source.completed,
                progress = source.progress,
                requiredAmount = source.requiredAmount
            };
    }
}
