using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.IO;

namespace Ashfall.Core
{
    /// <summary>
    /// ASHFALL — Expansion Quest System
    /// Simple quest system for expansion quests that don't require the full QuestlineDefinition structure.
    /// </summary>
    [Serializable]
    public class ExpansionQuestEntry
    {
        public string id = string.Empty;
        public string title = string.Empty;
        public string description = string.Empty;
        public string type = string.Empty;
        public int minDay = 0;
        public int maxDay = 365;
        public string factionTag = string.Empty;
        public string synopsis = string.Empty;
        public List<string> prerequisites = new List<string>();
        public List<ExpansionQuestChoice> choices = new List<ExpansionQuestChoice>();
    }

    [Serializable]
    public class ExpansionQuestChoice
    {
        public string id = string.Empty;
        public string text = string.Empty;
        public List<ExpansionQuestEffect> effects = new List<ExpansionQuestEffect>();
        public string consequences = string.Empty;
    }

    [Serializable]
    public class ExpansionQuestEffect
    {
        public string type = string.Empty;
        public string target = string.Empty;
        public int value = 0;
    }

    [Serializable]
    public class ExpansionQuestSystemState
    {
        public string systemId = ExpansionQuestSystem.SystemId;
        public List<ExpansionQuestProgress> quests = new List<ExpansionQuestProgress>();
        public List<string> completedQuestIds = new List<string>();
        public List<string> failedQuestIds = new List<string>();
    }

    [Serializable]
    public class ExpansionQuestProgress
    {
        public string questId = string.Empty;
        public string currentChoiceId = string.Empty;
        public bool started = false;
        public bool completed = false;
        public bool failed = false;
        public int dayStarted = 0;
        public int dayResolved = -1;
    }

    public class ExpansionQuestSystem
    {
        public const string SystemId = "expansion_quest_system";

        private ExpansionQuestSystemState _state = new ExpansionQuestSystemState();
        private IReadOnlyList<ExpansionQuestEntry> _catalog = Array.Empty<ExpansionQuestEntry>();

        public event Action<ExpansionQuestEntry> OnQuestStarted;
        public event Action<ExpansionQuestEntry> OnQuestCompleted;
        public event Action<ExpansionQuestEntry> OnQuestFailed;
        public event Action<ExpansionQuestSystemState> OnStateChanged;

        public ExpansionQuestSystemState State => _state;

        public void BindCatalog(IReadOnlyList<ExpansionQuestEntry> catalog)
        {
            _catalog = catalog ?? Array.Empty<ExpansionQuestEntry>();
        }

        public void StartQuest(string questId, int day)
        {
            var def = GetDefinition(questId);
            if (def == null) return;

            if (IsStarted(questId) || IsCompleted(questId) || IsFailed(questId)) return;

            var progress = new ExpansionQuestProgress
            {
                questId = questId,
                started = true,
                dayStarted = day
            };
            _state.quests.Add(progress);
            OnQuestStarted?.Invoke(def);
            RaiseStateChanged();
        }

        public void CompleteQuest(string questId, int day)
        {
            var progress = GetProgress(questId);
            if (progress == null) return;

            progress.completed = true;
            progress.dayResolved = day;
            _state.completedQuestIds.Add(questId);
            var def = GetDefinition(questId);
            if (def != null)
                OnQuestCompleted?.Invoke(def);
            RaiseStateChanged();
        }

        public void FailQuest(string questId, int day)
        {
            var progress = GetProgress(questId);
            if (progress == null) return;

            progress.failed = true;
            progress.dayResolved = day;
            _state.failedQuestIds.Add(questId);
            var def = GetDefinition(questId);
            if (def != null)
                OnQuestFailed?.Invoke(def);
            RaiseStateChanged();
        }

        public void MakeChoice(string questId, string choiceId, int day)
        {
            var progress = GetProgress(questId);
            if (progress == null) return;

            progress.currentChoiceId = choiceId;
            var def = GetDefinition(questId);
            if (def != null)
            {
                var choice = def.choices.Find(c => c.id == choiceId);
                if (choice != null)
                {
                    ApplyEffects(def, choice, day);
                }
            }
            RaiseStateChanged();
        }

        private void ApplyEffects(ExpansionQuestEntry def, ExpansionQuestChoice choice, int day)
        {
            foreach (var effect in choice.effects)
            {
                // Apply effects based on type
                switch (effect.type)
                {
                    case "complete_quest":
                        CompleteQuest(effect.target, day);
                        break;
                    case "fail_quest":
                        FailQuest(effect.target, day);
                        break;
                    case "start_quest":
                        StartQuest(effect.target, day);
                        break;
                }
            }
        }

        public ExpansionQuestEntry GetDefinition(string questId)
        {
            if (_catalog == null) return null;
            for (int i = 0; i < _catalog.Count; i++)
            {
                if (_catalog[i].id == questId)
                    return _catalog[i];
            }
            return null;
        }

        public ExpansionQuestProgress GetProgress(string questId)
        {
            if (_state == null || _state.quests == null) return null;
            for (int i = 0; i < _state.quests.Count; i++)
            {
                if (_state.quests[i].questId == questId)
                    return _state.quests[i];
            }
            return null;
        }

        public bool IsStarted(string questId) => GetProgress(questId)?.started == true;
        public bool IsCompleted(string questId) => _state.completedQuestIds.Contains(questId);
        public bool IsFailed(string questId) => _state.failedQuestIds.Contains(questId);
        public bool IsAvailable(string questId) => GetDefinition(questId) != null;

        public bool IsAvailable(string questId, int day)
        {
            var def = GetDefinition(questId);
            if (def == null) return false;
            if (day < def.minDay || day > def.maxDay) return false;
            if (IsStarted(questId) || IsCompleted(questId) || IsFailed(questId)) return false;

            foreach (var prereq in def.prerequisites)
            {
                if (!IsCompleted(prereq))
                    return false;
            }
            return true;
        }

        public List<ExpansionQuestChoice> GetChoices(string questId)
        {
            var def = GetDefinition(questId);
            if (def == null) return new List<ExpansionQuestChoice>();
            return def.choices ?? new List<ExpansionQuestChoice>();
        }

        public List<ExpansionQuestEntry> GetAvailableQuests(int day)
        {
            var result = new List<ExpansionQuestEntry>();
            if (_catalog == null) return result;

            for (int i = 0; i < _catalog.Count; i++)
            {
                var def = _catalog[i];
                if (def == null) continue;
                if (day >= def.minDay && day <= def.maxDay && !IsStarted(def.id) && !IsCompleted(def.id) && !IsFailed(def.id))
                {
                    bool prereqsMet = true;
                    foreach (var prereq in def.prerequisites)
                    {
                        if (!IsCompleted(prereq))
                        {
                            prereqsMet = false;
                            break;
                        }
                    }
                    if (prereqsMet)
                        result.Add(def);
                }
            }
            return result;
        }

        public List<ExpansionQuestEntry> GetActiveQuests()
        {
            var result = new List<ExpansionQuestEntry>();
            if (_state == null || _state.quests == null || _catalog == null) return result;

            for (int i = 0; i < _state.quests.Count; i++)
            {
                var progress = _state.quests[i];
                if (progress.started && !progress.completed && !progress.failed)
                {
                    var def = GetDefinition(progress.questId);
                    if (def != null)
                        result.Add(def);
                }
            }
            return result;
        }

        /// <summary>
        /// Tick the quest system for a new day. Checks for quest availability.
        /// </summary>
        public string TickDay(int day)
        {
            if (_catalog == null || _catalog.Count == 0) return "No expansion quests loaded";

            int startedCount = 0;
            for (int i = 0; i < _catalog.Count; i++)
            {
                var def = _catalog[i];
                if (def == null || string.IsNullOrEmpty(def.id)) continue;

                // Check if quest should start automatically on its minDay
                if (day >= def.minDay && day <= def.maxDay && !IsStarted(def.id) && !IsCompleted(def.id) && !IsFailed(def.id))
                {
                    StartQuest(def.id, day);
                    startedCount++;
                }
            }

            return $"Expansion quests ticked: {startedCount} new quests started";
        }

        public ExpansionQuestSystemState CaptureState()
        {
            return new ExpansionQuestSystemState
            {
                systemId = _state.systemId,
                quests = _state.quests.Select(q => new ExpansionQuestProgress
                {
                    questId = q.questId,
                    currentChoiceId = q.currentChoiceId,
                    started = q.started,
                    completed = q.completed,
                    failed = q.failed,
                    dayStarted = q.dayStarted,
                    dayResolved = q.dayResolved
                }).ToList(),
                completedQuestIds = new List<string>(_state.completedQuestIds),
                failedQuestIds = new List<string>(_state.failedQuestIds)
            };
        }

        public void RestoreState(ExpansionQuestSystemState state)
        {
            if (state == null) return;
            _state = new ExpansionQuestSystemState
            {
                systemId = state.systemId ?? SystemId,
                quests = state.quests?.Select(q => new ExpansionQuestProgress
                {
                    questId = q.questId ?? string.Empty,
                    currentChoiceId = q.currentChoiceId ?? string.Empty,
                    started = q.started,
                    completed = q.completed,
                    failed = q.failed,
                    dayStarted = q.dayStarted,
                    dayResolved = q.dayResolved
                }).ToList() ?? new List<ExpansionQuestProgress>(),
                completedQuestIds = state.completedQuestIds ?? new List<string>(),
                failedQuestIds = state.failedQuestIds ?? new List<string>()
            };
            RaiseStateChanged();
        }

        private void RaiseStateChanged() => OnStateChanged?.Invoke(_state);
    }

    /// <summary>
    /// Catalog loader for expansion quests.
    /// </summary>
    public static class ExpansionQuestCatalogLoader
    {
        [Serializable]
        public class ExpansionQuestContainer
        {
            public int schema_version = 1;
            public List<ExpansionQuestEntry> quests = new List<ExpansionQuestEntry>();
        }

        public static List<ExpansionQuestEntry> Load(string dataDir, IFileIO? fileIO = null, IJsonSerializer? serializer = null)
        {
            fileIO ??= new FileSystemIO();
            serializer ??= new SystemTextJsonSerializer();

            var result = new List<ExpansionQuestEntry>();

            // Load all expansion quest files
            string[] files = {
                "quests_expansion_05.json",
                "quests_expansion_06.json"
            };

            foreach (var file in files)
            {
                string path = System.IO.Path.Combine(dataDir, file);
                if (!fileIO.FileExists(path)) continue;

                string json = fileIO.ReadAllText(path);
                if (string.IsNullOrEmpty(json)) continue;

                try
                {
                    var container = serializer.Deserialize<ExpansionQuestContainer>(json);
                    if (container?.quests != null)
                        result.AddRange(container.quests);
                }
                catch (Exception ex)
                {
                    CatalogDiagnostics.Warn(file, "unknown", ex);
                }
            }

            return result;
        }
    }
}
