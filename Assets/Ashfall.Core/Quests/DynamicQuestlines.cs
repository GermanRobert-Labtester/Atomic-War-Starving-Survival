// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Ashfall.Core.IO;

namespace Ashfall.Core.Quests
{
    public enum DynamicQuestStatus
    {
        Active = 0,
        Completed = 1,
        Failed = 2,
        Expired = 3
    }

    [Serializable]
    public sealed class DynamicQuestInstance
    {
        [JsonPropertyName("quest_id")]
        public string QuestId { get; set; } = string.Empty;

        [JsonPropertyName("incident_id")]
        public string IncidentId { get; set; } = string.Empty;

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("target_location_id")]
        public string TargetLocationId { get; set; } = string.Empty;

        [JsonPropertyName("target_survivor_ids")]
        public List<string> TargetSurvivorIds { get; set; } = new List<string>();

        [JsonPropertyName("trigger_day")]
        public int TriggerDay { get; set; }

        [JsonPropertyName("deadline_day")]
        public int? DeadlineDay { get; set; }

        [JsonPropertyName("current_stage_index")]
        public int CurrentStageIndex { get; set; }

        [JsonPropertyName("stages")]
        public List<string> Stages { get; set; } = new List<string>();

        [JsonPropertyName("progress_current")]
        public int ProgressCurrent { get; set; }

        [JsonPropertyName("progress_required")]
        public int ProgressRequired { get; set; }

        [JsonPropertyName("status")]
        public DynamicQuestStatus Status { get; set; } = DynamicQuestStatus.Active;
    }

    [Serializable]
    public sealed class DynamicQuestSave
    {
        public string systemId = DynamicQuestlineSystem.SystemId;
        public int schemaVersion = 1;
        public List<DynamicQuestInstance> activeInstances = new List<DynamicQuestInstance>();
        public List<string> completedIds = new List<string>();
        public List<string> failedIds = new List<string>();
        public List<string> triggeredIncidentIds = new List<string>();
        public int currentDay;
    }

    /// <summary>
    /// Deterministic, campaign-wide emergency quest runtime.
    /// Manages dynamically triggered crises (miners rescue, radio depot investigations,
    /// armory refurbishments) across the entire campaign lifecycle.
    /// </summary>
    public sealed class DynamicQuestlineSystem
    {
        public const string SystemId = "dynamic_quests";
        public const string RescueMinersQuestId = "quest_rescue_trapped_miners";
        public const string InvestigateRadioDepotQuestId = "quest_investigate_radio_depot";
        public const string ArmoryMunitionsRefurbishQuestId = "quest_armory_munitions_refurbish";

        private DynamicQuestSave _state = new DynamicQuestSave();
        private readonly HashSet<string> _triggeredIncidents = new(StringComparer.Ordinal);
        private readonly ILog _log;

        public DynamicQuestSave State => _state;
        public IReadOnlyList<DynamicQuestInstance> ActiveQuests => _state.activeInstances;
        public IReadOnlyList<string> CompletedIds => _state.completedIds;
        public IReadOnlyList<string> FailedIds => _state.failedIds;

        public event Action<DynamicQuestInstance>? OnQuestTriggered;
        public event Action<DynamicQuestInstance>? OnQuestStageAdvanced;
        public event Action<DynamicQuestInstance>? OnQuestCompleted;
        public event Action<DynamicQuestInstance>? OnQuestFailed;
        public event Action? OnStateChanged;

        public DynamicQuestlineSystem(ILog? log = null)
        {
            _log = log ?? NullLog.Instance;
        }

        public bool HasIncidentTriggered(string incidentId)
        {
            if (string.IsNullOrEmpty(incidentId)) return false;
            return _triggeredIncidents.Contains(incidentId);
        }

        public DynamicQuestInstance? GetActiveQuest(string questId)
        {
            return _state.activeInstances.Find(q => q.QuestId == questId && q.Status == DynamicQuestStatus.Active);
        }

        public DynamicQuestInstance? TriggerRescueMinersQuest(
            string incidentId,
            string sectorId,
            IReadOnlyList<string> trappedSurvivorIds,
            int triggerDay,
            int deadlineDays = 3,
            int requiredLabor = 240)
        {
            if (string.IsNullOrEmpty(incidentId) || HasIncidentTriggered(incidentId))
                return null;

            // Check if already active for this sector
            if (_state.activeInstances.Exists(q => q.QuestId == RescueMinersQuestId && q.TargetLocationId == sectorId && q.Status == DynamicQuestStatus.Active))
                return null;

            _triggeredIncidents.Add(incidentId);
            _state.triggeredIncidentIds.Add(incidentId);

            var inst = new DynamicQuestInstance
            {
                QuestId = RescueMinersQuestId,
                IncidentId = incidentId,
                Title = "Emergency: Trapped Miners Rescue",
                Description = $"Cave-in collapse in {sectorId}! Clear rubble and extract trapped survivors before air runs out.",
                TargetLocationId = sectorId,
                TargetSurvivorIds = new List<string>(trappedSurvivorIds),
                TriggerDay = triggerDay,
                DeadlineDay = triggerDay + deadlineDays,
                CurrentStageIndex = 0,
                Stages = new List<string>
                {
                    "stage_assess_collapse",
                    "stage_stabilize_access",
                    "stage_clear_rubble",
                    "stage_extract_miners"
                },
                ProgressCurrent = 0,
                ProgressRequired = requiredLabor,
                Status = DynamicQuestStatus.Active
            };

            _state.activeInstances.Add(inst);
            OnQuestTriggered?.Invoke(inst);
            OnStateChanged?.Invoke();
            return inst;
        }

        public DynamicQuestInstance? TriggerInvestigateRadioDepotQuest(
            string interceptId,
            string canonicalLocationId,
            int triggerDay,
            int? deadlineDays = null)
        {
            if (string.IsNullOrEmpty(interceptId) || HasIncidentTriggered(interceptId))
                return null;

            if (_state.activeInstances.Exists(q => q.QuestId == InvestigateRadioDepotQuestId && q.TargetLocationId == canonicalLocationId && q.Status == DynamicQuestStatus.Active))
                return null;

            _triggeredIncidents.Add(interceptId);
            _state.triggeredIncidentIds.Add(interceptId);

            var inst = new DynamicQuestInstance
            {
                QuestId = InvestigateRadioDepotQuestId,
                IncidentId = interceptId,
                Title = "Intelligence: Investigate Radio Coordinates",
                Description = $"Radio triangulation verified active supply depot at {canonicalLocationId}. Dispatch expedition to secure cargo.",
                TargetLocationId = canonicalLocationId,
                TargetSurvivorIds = new List<string>(),
                TriggerDay = triggerDay,
                DeadlineDay = deadlineDays.HasValue ? triggerDay + deadlineDays.Value : null,
                CurrentStageIndex = 0,
                Stages = new List<string>
                {
                    "stage_plan_expedition",
                    "stage_travel_to_depot",
                    "stage_secure_supplies"
                },
                ProgressCurrent = 0,
                ProgressRequired = 3,
                Status = DynamicQuestStatus.Active
            };

            _state.activeInstances.Add(inst);
            OnQuestTriggered?.Invoke(inst);
            OnStateChanged?.Invoke();
            return inst;
        }

        public DynamicQuestInstance? TriggerArmoryMunitionsRefurbishQuest(
            string incidentId,
            int triggerDay,
            int weaponsNeedingRepair)
        {
            if (string.IsNullOrEmpty(incidentId) || HasIncidentTriggered(incidentId))
                return null;

            if (_state.activeInstances.Exists(q => q.QuestId == ArmoryMunitionsRefurbishQuestId && q.Status == DynamicQuestStatus.Active))
                return null;

            _triggeredIncidents.Add(incidentId);
            _state.triggeredIncidentIds.Add(incidentId);

            var inst = new DynamicQuestInstance
            {
                QuestId = ArmoryMunitionsRefurbishQuestId,
                IncidentId = incidentId,
                Title = "Maintenance: Armory Readiness Crisis",
                Description = $"Severe firearm wear detected. Service {weaponsNeedingRepair} weapons to restore perimeter readiness.",
                TargetLocationId = "room_workshop",
                TargetSurvivorIds = new List<string>(),
                TriggerDay = triggerDay,
                DeadlineDay = triggerDay + 5,
                CurrentStageIndex = 0,
                Stages = new List<string>
                {
                    "stage_inventory_weapons",
                    "stage_clean_service_firearms",
                    "stage_reload_munitions"
                },
                ProgressCurrent = 0,
                ProgressRequired = Math.Max(1, weaponsNeedingRepair),
                Status = DynamicQuestStatus.Active
            };

            _state.activeInstances.Add(inst);
            OnQuestTriggered?.Invoke(inst);
            OnStateChanged?.Invoke();
            return inst;
        }

        public bool AdvanceQuestProgress(string questId, int progressAmount = 1)
        {
            var inst = GetActiveQuest(questId);
            if (inst == null) return false;

            inst.ProgressCurrent += progressAmount;
            if (inst.ProgressCurrent >= inst.ProgressRequired)
            {
                CompleteQuest(questId);
            }
            else
            {
                // Progress stages proportionally
                int stagesCount = inst.Stages.Count;
                if (stagesCount > 1 && inst.ProgressRequired > 0)
                {
                    int targetStage = Math.Min(stagesCount - 1, (inst.ProgressCurrent * stagesCount) / inst.ProgressRequired);
                    if (targetStage > inst.CurrentStageIndex)
                    {
                        inst.CurrentStageIndex = targetStage;
                        OnQuestStageAdvanced?.Invoke(inst);
                    }
                }
                OnStateChanged?.Invoke();
            }
            return true;
        }

        public bool AdvanceQuestStage(string questId)
        {
            var inst = GetActiveQuest(questId);
            if (inst == null) return false;

            if (inst.CurrentStageIndex < inst.Stages.Count - 1)
            {
                inst.CurrentStageIndex++;
                OnQuestStageAdvanced?.Invoke(inst);
                OnStateChanged?.Invoke();
                return true;
            }
            else
            {
                return CompleteQuest(questId);
            }
        }

        public bool CompleteQuest(string questId)
        {
            var inst = GetActiveQuest(questId);
            if (inst == null) return false;

            inst.Status = DynamicQuestStatus.Completed;
            inst.CurrentStageIndex = Math.Max(0, inst.Stages.Count - 1);
            _state.activeInstances.Remove(inst);
            if (!_state.completedIds.Contains(questId))
                _state.completedIds.Add(questId);

            OnQuestCompleted?.Invoke(inst);
            OnStateChanged?.Invoke();
            return true;
        }

        public bool FailQuest(string questId)
        {
            var inst = GetActiveQuest(questId);
            if (inst == null) return false;

            inst.Status = DynamicQuestStatus.Failed;
            _state.activeInstances.Remove(inst);
            if (!_state.failedIds.Contains(questId))
                _state.failedIds.Add(questId);

            OnQuestFailed?.Invoke(inst);
            OnStateChanged?.Invoke();
            return true;
        }

        public void TickDay(int day)
        {
            _state.currentDay = day;
            var expired = new List<DynamicQuestInstance>();

            foreach (var q in _state.activeInstances)
            {
                if (q.Status == DynamicQuestStatus.Active && q.DeadlineDay.HasValue && q.DeadlineDay.Value <= day)
                {
                    expired.Add(q);
                }
            }

            foreach (var q in expired)
            {
                FailQuest(q.QuestId);
            }
        }

        public DynamicQuestSave CaptureState()
        {
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(_state);
            return s.Deserialize<DynamicQuestSave>(json) ?? new DynamicQuestSave();
        }

        public void RestoreState(DynamicQuestSave? saved)
        {
            if (saved == null)
            {
                _state = new DynamicQuestSave();
                _triggeredIncidents.Clear();
                return;
            }

            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(saved);
            _state = s.Deserialize<DynamicQuestSave>(json) ?? new DynamicQuestSave();

            _triggeredIncidents.Clear();
            foreach (var id in _state.triggeredIncidentIds)
            {
                if (!string.IsNullOrEmpty(id))
                    _triggeredIncidents.Add(id);
            }

            // Restore does NOT fire OnQuestTriggered or OnStateChanged side-effects
        }
    }
}
