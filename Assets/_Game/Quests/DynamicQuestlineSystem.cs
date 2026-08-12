using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Quests
{
    /// <summary>
    /// Dynamic Multi-Stage Questline System — tracks active location-based
    /// quests through the Discovery → Investigation → Crisis → Resolution
    /// pattern. Integrates with ExpeditionSystem, EventRunner, and
    /// QuestRegistry.
    ///
    /// Plain C#, save-safe. Each quest has 4 stages with branching choices
    /// at the crisis point (stage 2→3).
    /// </summary>
    [Serializable]
    public class DynamicQuestState
    {
        public string QuestId;
        public string QuestTitle;
        public int CurrentStage; // 0-3
        public int TotalStages = 4;
        public string[] StageDescriptions = new string[4];
        public string TargetLocationId;
        public string[] ObjectiveItemIds;
        public bool[] ObjectivesCompleted;
        public string BranchChosen; // "a" or "b", empty if not yet at crisis
        public bool IsCompleted;
        public bool IsFailed;
        public int DayStarted;
    }

    [Serializable]
    public class DynamicQuestlineSaveState
    {
        public List<DynamicQuestState> ActiveQuests = new List<DynamicQuestState>();
        public List<string> CompletedQuestIds = new List<string>();
    }

    public class DynamicQuestlineSystem
    {
        public event Action<DynamicQuestState> OnQuestStarted;
        public event Action<DynamicQuestState, int> OnStageAdvanced;
        // quest, newStage
        public event Action<DynamicQuestState, string> OnBranchChosen;
        // quest, branchId
        public event Action<DynamicQuestState> OnQuestCompleted;
        public event Action<DynamicQuestState> OnQuestFailed;

        private readonly Dictionary<string, DynamicQuestState> _activeQuests =
            new Dictionary<string, DynamicQuestState>();
        private readonly HashSet<string> _completedQuestIds = new HashSet<string>();
        private DynamicQuestlineSaveState _saveState = new DynamicQuestlineSaveState();

        // ── Host hooks ─────────────────────────────────────────────────
        public Func<int> GetCurrentDay;
        public Action<string, string> FireNarrativeEvent;
        // eventId, context
        public System.Random Rng;

        public IReadOnlyDictionary<string, DynamicQuestState> ActiveQuests => _activeQuests;

        public bool StartQuest(string questId, string questTitle,
            string[] stageDescriptions, string targetLocationId,
            string[] objectiveItemIds)
        {
            if (_activeQuests.ContainsKey(questId)) return false;
            if (_completedQuestIds.Contains(questId)) return false;

            var quest = new DynamicQuestState
            {
                QuestId = questId,
                QuestTitle = questTitle,
                CurrentStage = 0,
                TotalStages = 4,
                StageDescriptions = stageDescriptions ?? new string[4],
                TargetLocationId = targetLocationId,
                ObjectiveItemIds = objectiveItemIds ?? new string[0],
                ObjectivesCompleted = new bool[objectiveItemIds?.Length ?? 0],
                DayStarted = GetCurrentDay?.Invoke() ?? 1
            };

            _activeQuests[questId] = quest;
            OnQuestStarted?.Invoke(quest);
            return true;
        }

        public bool AdvanceStage(string questId)
        {
            if (!_activeQuests.TryGetValue(questId, out var quest)) return false;
            if (quest.IsCompleted || quest.IsFailed) return false;

            int newStage = quest.CurrentStage + 1;
            if (newStage >= quest.TotalStages)
            {
                quest.IsCompleted = true;
                _activeQuests.Remove(questId);
                _completedQuestIds.Add(questId);
                OnQuestCompleted?.Invoke(quest);
                return true;
            }

            quest.CurrentStage = newStage;
            OnStageAdvanced?.Invoke(quest, newStage);
            return true;
        }

        public bool ChooseBranch(string questId, string branchId)
        {
            if (!_activeQuests.TryGetValue(questId, out var quest)) return false;
            if (quest.CurrentStage != 2) return false; // crisis is stage 2→3
            if (!string.IsNullOrEmpty(quest.BranchChosen)) return false;

            quest.BranchChosen = branchId;
            OnBranchChosen?.Invoke(quest, branchId);
            return AdvanceStage(questId); // auto-advance to resolution
        }

        public bool CompleteObjective(string questId, string objectiveItemId)
        {
            if (!_activeQuests.TryGetValue(questId, out var quest)) return false;
            if (quest.ObjectiveItemIds == null) return false;

            for (int i = 0; i < quest.ObjectiveItemIds.Length; i++)
            {
                if (string.Equals(quest.ObjectiveItemIds[i], objectiveItemId,
                    StringComparison.OrdinalIgnoreCase) && !quest.ObjectivesCompleted[i])
                {
                    quest.ObjectivesCompleted[i] = true;

                    // Check if all objectives complete
                    bool allDone = true;
                    for (int j = 0; j < quest.ObjectivesCompleted.Length; j++)
                        if (!quest.ObjectivesCompleted[j]) allDone = false;

                    if (allDone)
                        AdvanceStage(questId);

                    return true;
                }
            }
            return false;
        }

        public void FailQuest(string questId)
        {
            if (!_activeQuests.TryGetValue(questId, out var quest)) return;
            quest.IsFailed = true;
            _activeQuests.Remove(questId);
            OnQuestFailed?.Invoke(quest);
        }

        public DynamicQuestState GetQuest(string questId)
        {
            return _activeQuests.TryGetValue(questId, out var q) ? q : null;
        }

        // ── Save/Load ─────────────────────────────────────────────────

        public DynamicQuestlineSaveState CaptureState()
        {
            _saveState.ActiveQuests.Clear();
            foreach (var kv in _activeQuests)
                _saveState.ActiveQuests.Add(kv.Value);
            _saveState.CompletedQuestIds.Clear();
            _saveState.CompletedQuestIds.AddRange(_completedQuestIds);
            return _saveState;
        }

        public void RestoreState(DynamicQuestlineSaveState state)
        {
            if (state == null) return;
            _activeQuests.Clear();
            _completedQuestIds.Clear();

            if (state.ActiveQuests != null)
            {
                foreach (var q in state.ActiveQuests)
                {
                    if (q != null && !string.IsNullOrEmpty(q.QuestId))
                        _activeQuests[q.QuestId] = q;
                }
            }
            if (state.CompletedQuestIds != null)
            {
                foreach (var id in state.CompletedQuestIds)
                    _completedQuestIds.Add(id);
            }
        }
    }
}
