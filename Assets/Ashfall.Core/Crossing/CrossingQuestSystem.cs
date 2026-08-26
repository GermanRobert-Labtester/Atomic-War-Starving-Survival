using System;
using System.Collections.Generic;
#pragma warning disable CS8618
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

using Ashfall.Core.IO;
namespace Ashfall.Core.Crossing
{
    // ── Data model (matches crossing_quests.json) ───────────────

    public class CrossingQuestStage
    {
        [JsonPropertyName("id")] public string id { get; set; } = "";
        [JsonPropertyName("text")] public string text { get; set; } = "";
    }

    public class CrossingQuestChoice
    {
        [JsonPropertyName("id")] public string id { get; set; } = "";
        [JsonPropertyName("text")] public string text { get; set; } = "";
        [JsonPropertyName("set_flag")] public string set_flag { get; set; } = "";
    }

    public class CrossingQuestDef
    {
        [JsonPropertyName("id")] public string id { get; set; } = "";
        [JsonPropertyName("display_name")] public string display_name { get; set; } = "";
        [JsonPropertyName("type")] public string type { get; set; } = "";
        [JsonPropertyName("briefing")] public string briefing { get; set; } = "";
        [JsonPropertyName("prereq_quest_id")] public string prereq_quest_id { get; set; } = "";
        [JsonPropertyName("min_day")] public int min_day { get; set; }
        [JsonPropertyName("stages")] public List<CrossingQuestStage> stages { get; set; } = new();
        [JsonPropertyName("choices")] public List<CrossingQuestChoice> choices { get; set; } = new();
        [JsonPropertyName("knowledge_key")] public string knowledge_key { get; set; } = "";
        [JsonPropertyName("target_location_id")] public string target_location_id { get; set; } = "";
    }

    // ── Runtime state ───────────────────────────────────────────

    public class CrossingStageNarrativeEvent
    {
        public string questId { get; set; } = "";
        public string questDisplayName { get; set; } = "";
        public int stageIndex { get; set; }
        public string stageId { get; set; } = "";
        public string stageText { get; set; } = "";
        public string briefing { get; set; } = "";
        public bool isCompletion { get; set; }
    }

    [Serializable]
    public class CrossingQuestProgress
    {
        public string questId = "";
        public int currentStage;
        public bool started;
        public bool completed;
        public bool failed;
        public string chosenChoiceId = "";
    }

    [Serializable]
    public class CrossingQuestSystemState
    {
        public string systemId = CrossingQuestSystem.SystemId;
        public int lastTickedDay;
        public List<CrossingQuestProgress> quests = new();
        public HashSet<string> setFlags = new();
        public HashSet<string> dispatchedStageEvents = new();
    }

    // ── System ──────────────────────────────────────────────────

    /// <summary>
    /// ASHFALL: NOBODY'S CHARTER — quest runtime for the Crossing.
    /// Loads crossing_quests.json, tracks stage progress, handles choices/flags.
    /// Integrates with VouchAccessSystem for the opening quest and daily auto-start.
    /// Spec: docs/expansions/expansion_04_nobodys_charter_plan.md
    /// </summary>
    public class CrossingQuestSystem
    {
        public const string SystemId = "crossing_quest_system";
        public const string OpeningQuest = "quest_crossing_the_vouch";

        private CrossingQuestSystemState _state = new();
        private IReadOnlyList<CrossingQuestDef> _catalog = Array.Empty<CrossingQuestDef>();

        public event Action<string, int> OnQuestStageChanged;
        public event Action<string> OnQuestStarted;
        public event Action<string> OnQuestCompleted;
        public event Action<string> OnQuestFailed;
        public event Action<string, string> OnFlagSet;
        public event Action<CrossingStageNarrativeEvent> OnStageNarrativeEmitted;
        public event Action<CrossingQuestSystemState> OnStateChanged;

        public CrossingQuestSystemState State => _state;

        public void BindCatalog(IReadOnlyList<CrossingQuestDef> catalog)
        {
            _catalog = catalog ?? Array.Empty<CrossingQuestDef>();
        }

        public CrossingQuestDef? GetDef(string questId)
        {
            if (string.IsNullOrEmpty(questId)) return null;
            for (int i = 0; i < _catalog.Count; i++)
                if (_catalog[i]?.id == questId) return _catalog[i];
            return null;
        }

        public IReadOnlyList<CrossingQuestDef> Catalog => _catalog;

        /// <summary>Quests available given current day, prereqs, and flags.</summary>
        public List<CrossingQuestDef> GetAvailableQuests(int currentDay)
        {
            var available = new List<CrossingQuestDef>();
            for (int i = 0; i < _catalog.Count; i++)
            {
                var def = _catalog[i];
                if (def == null) continue;
                if (IsQuestCompleted(def.id)) continue;
                if (def.min_day > currentDay) continue;
                if (!string.IsNullOrEmpty(def.prereq_quest_id) && !IsQuestCompleted(def.prereq_quest_id)) continue;
                available.Add(def);
            }
            return available;
        }

        public bool IsQuestCompleted(string questId)
        {
            var progress = GetProgress(questId);
            return progress != null && progress.completed;
        }

        public bool IsQuestFailed(string questId)
        {
            var progress = GetProgress(questId);
            return progress != null && progress.failed;
        }

        public bool IsQuestStarted(string questId)
        {
            var progress = GetProgress(questId);
            return progress != null && progress.started;
        }

        public CrossingQuestProgress? GetProgress(string questId)
        {
            for (int i = 0; i < _state.quests.Count; i++)
                if (_state.quests[i].questId == questId) return _state.quests[i];
            return null;
        }

        /// <summary>
        /// Authoritative daily tick for Crossing quests.
        /// Idempotent against repeated ticks on the same day and after save/load.
        /// Automatically starts eligible quests once prerequisites and day threshold are met.
        /// </summary>
        public void TickDaily(int currentDay, bool hasVouchAccess = false)
        {
            if (_state.lastTickedDay == currentDay) return;
            _state.lastTickedDay = currentDay;

            for (int i = 0; i < _catalog.Count; i++)
            {
                var def = _catalog[i];
                if (def == null) continue;
                if (IsQuestStarted(def.id) || IsQuestCompleted(def.id) || IsQuestFailed(def.id)) continue;
                if (def.min_day > currentDay) continue;
                if (!string.IsNullOrEmpty(def.prereq_quest_id) && !IsQuestCompleted(def.prereq_quest_id)) continue;

                // Post-vouch quests require either active vouch or opening quest completion
                if (def.id != OpeningQuest && !hasVouchAccess && !IsQuestCompleted(OpeningQuest)) continue;

                StartQuest(def.id, currentDay);
            }

            RaiseChanged();
        }

        /// <summary>Start a quest. Returns false if prereqs not met, already started, completed, or failed.</summary>
        public bool StartQuest(string questId, int currentDay)
        {
            if (string.IsNullOrEmpty(questId)) return false;
            var def = GetDef(questId);
            if (def == null) return false;
            if (IsQuestStarted(questId) || IsQuestCompleted(questId) || IsQuestFailed(questId)) return false;
            if (def.min_day > currentDay) return false;
            if (!string.IsNullOrEmpty(def.prereq_quest_id) && !IsQuestCompleted(def.prereq_quest_id)) return false;

            var progress = new CrossingQuestProgress
            {
                questId = questId,
                currentStage = 0,
                started = true,
                completed = false,
                failed = false
            };
            _state.quests.Add(progress);
            OnQuestStarted?.Invoke(questId);
            EmitStageNarrative(def, 0, isCompletion: false);
            RaiseChanged();
            return true;
        }

        /// <summary>Marks an active quest as failed.</summary>
        public bool FailQuest(string questId)
        {
            var progress = GetProgress(questId);
            if (progress == null || progress.completed || progress.failed) return false;
            progress.failed = true;
            OnQuestFailed?.Invoke(questId);
            RaiseChanged();
            return true;
        }

        /// <summary>Advance to the next stage. Returns the new stage index, or -1 if quest completed.</summary>
        public int AdvanceStage(string questId)
        {
            var progress = GetProgress(questId);
            if (progress == null || progress.completed || progress.failed) return -1;
            var def = GetDef(questId);
            if (def == null) return -1;

            progress.currentStage++;
            if (progress.currentStage >= def.stages.Count)
            {
                progress.completed = true;
                OnQuestCompleted?.Invoke(questId);
                EmitStageNarrative(def, progress.currentStage, isCompletion: true);

                // The opening quest completion softens the gate
                if (questId == OpeningQuest)
                    OnOpeningQuestCompleted?.Invoke();
            }
            else
            {
                OnQuestStageChanged?.Invoke(questId, progress.currentStage);
                EmitStageNarrative(def, progress.currentStage, isCompletion: false);
            }
            RaiseChanged();
            return progress.completed ? -1 : progress.currentStage;
        }

        private void EmitStageNarrative(CrossingQuestDef def, int stageIndex, bool isCompletion)
        {
            string eventKey = $"{def.id}:{stageIndex}:{(isCompletion ? "complete" : "stage")}";
            if (_state.dispatchedStageEvents.Contains(eventKey)) return;
            _state.dispatchedStageEvents.Add(eventKey);

            string stageId = "";
            string stageText = "";
            if (isCompletion)
            {
                stageId = "complete";
                stageText = $"[CHARTER RESOLVED] {def.display_name} concluded.";
            }
            else if (def.stages != null && stageIndex >= 0 && stageIndex < def.stages.Count)
            {
                stageId = def.stages[stageIndex].id ?? "";
                stageText = def.stages[stageIndex].text ?? "";
            }

            var evt = new CrossingStageNarrativeEvent
            {
                questId = def.id,
                questDisplayName = def.display_name,
                stageIndex = stageIndex,
                stageId = stageId,
                stageText = stageText,
                briefing = def.briefing,
                isCompletion = isCompletion
            };

            OnStageNarrativeEmitted?.Invoke(evt);
        }

        /// <summary>Make a choice for a quest. Sets the associated flag.</summary>
        public bool MakeChoice(string questId, string choiceId)
        {
            var progress = GetProgress(questId);
            if (progress == null) return false;
            var def = GetDef(questId);
            if (def == null) return false;

            for (int i = 0; i < def.choices.Count; i++)
            {
                var choice = def.choices[i];
                if (choice.id != choiceId) continue;
                progress.chosenChoiceId = choiceId;
                if (!string.IsNullOrEmpty(choice.set_flag))
                {
                    _state.setFlags.Add(choice.set_flag);
                    OnFlagSet?.Invoke(questId, choice.set_flag);
                }
                RaiseChanged();
                return true;
            }
            return false;
        }

        public bool HasFlag(string flag) => _state.setFlags.Contains(flag);

        /// <summary>Event fired when the opening vouch quest is completed.</summary>
        public event Action? OnOpeningQuestCompleted;

        // ── Save / Load ─────────────────────────────────────────

        public CrossingQuestSystemState CaptureState()
        {
            var stateCopy = new CrossingQuestSystemState
            {
                systemId = SystemId,
                lastTickedDay = _state.lastTickedDay,
                quests = new List<CrossingQuestProgress>(_state.quests.Count),
                setFlags = new HashSet<string>(_state.setFlags),
                dispatchedStageEvents = new HashSet<string>(_state.dispatchedStageEvents)
            };

            for (int i = 0; i < _state.quests.Count; i++)
            {
                var q = _state.quests[i];
                if (q == null) continue;
                stateCopy.quests.Add(new CrossingQuestProgress
                {
                    questId = q.questId,
                    currentStage = q.currentStage,
                    started = q.started,
                    completed = q.completed,
                    failed = q.failed,
                    chosenChoiceId = q.chosenChoiceId
                });
            }

            return stateCopy;
        }

        public void RestoreState(CrossingQuestSystemState? saved)
        {
            if (saved == null) return;
            _state.systemId = SystemId;
            _state.lastTickedDay = saved.lastTickedDay;
            _state.quests = saved.quests ?? new();
            _state.setFlags = saved.setFlags ?? new();
            _state.dispatchedStageEvents = saved.dispatchedStageEvents ?? new();
            RaiseChanged();
        }

        private void RaiseChanged() => OnStateChanged?.Invoke(_state);
    }

    // ── Catalog loader ──────────────────────────────────────────

    public static class CrossingQuestCatalogLoader
    {
        public const string FileName = "crossing_quests.json";

        public static List<CrossingQuestDef> Load(string dataDir, IFileIO? fileIO = null, IJsonSerializer? serializer = null)
        {
            fileIO ??= new FileSystemIO();
            serializer ??= new SystemTextJsonSerializer();
            string path = Path.Combine(dataDir, FileName);
            if (!fileIO.FileExists(path)) return new List<CrossingQuestDef>();

            string json = fileIO.ReadAllText(path);
            if (string.IsNullOrEmpty(json)) return new List<CrossingQuestDef>();

            try
            {
                var quests = CatalogLocator.LoadWrappedList<CrossingQuestDef>(json, SystemTextJsonSerializer.Options);
                return quests ?? new List<CrossingQuestDef>();
            }
            catch (Exception ex_CATDIAG)
                                {
                                    CatalogDiagnostics.Warn("<unknown>", "unknown", ex_CATDIAG);
                                    return new List<CrossingQuestDef>();
                                }
        }
    }
}
