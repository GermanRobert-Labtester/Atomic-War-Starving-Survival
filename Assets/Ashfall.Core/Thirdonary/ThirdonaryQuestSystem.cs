using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Thirdonary
{
    /// <summary>
    /// Engine-agnostic Thirdonary quest system: small environmental, procedural,
    /// and contextual quests that fill the wasteland with discoverable moments.
    ///
    /// Follows the same lifecycle pattern as ExpansionQuestSystem:
    /// BindCatalog → TickDay → StartQuest → MakeChoice → CompleteQuest/FailQuest.
    /// </summary>
    public sealed class ThirdonaryQuestSystem
    {
        public const string SystemId = "thirdonary_quest";
        public const string QuestIdPrefix = "quest_third_";

        private ThirdonaryState _state = new ThirdonaryState();
        private IReadOnlyList<ThirdonaryQuestDef> _catalog = Array.Empty<ThirdonaryQuestDef>();

        public event Action<ThirdonaryQuestDef>? OnQuestStarted;
        public event Action<ThirdonaryQuestDef>? OnQuestCompleted;
        public event Action<ThirdonaryQuestDef>? OnQuestFailed;
        public event Action<ThirdonaryState>? OnStateChanged;

        public ThirdonaryState State => _state;
        public int QuestsCompleted => _state.completed_quest_ids.Count;
        public int QuestsFailed => _state.failed_quest_ids.Count;

        public void BindCatalog(IReadOnlyList<ThirdonaryQuestDef> catalog)
        {
            _catalog = catalog ?? Array.Empty<ThirdonaryQuestDef>();
        }

        public ThirdonaryQuestDef? GetDefinition(string questId)
        {
            if (_catalog == null || string.IsNullOrEmpty(questId)) return null;
            for (int i = 0; i < _catalog.Count; i++)
            {
                if (string.Equals(_catalog[i].id, questId, StringComparison.Ordinal))
                    return _catalog[i];
            }
            return null;
        }

        public ThirdonaryProgress? GetProgress(string questId)
        {
            if (_state?.quests == null || string.IsNullOrEmpty(questId)) return null;
            for (int i = 0; i < _state.quests.Count; i++)
            {
                if (string.Equals(_state.quests[i].quest_id, questId, StringComparison.Ordinal))
                    return _state.quests[i];
            }
            return null;
        }

        public bool IsStarted(string questId) => GetProgress(questId)?.started == true;
        public bool IsCompleted(string questId) => _state.completed_quest_ids.Contains(questId);
        public bool IsFailed(string questId) => _state.failed_quest_ids.Contains(questId);

        public bool IsOnCooldown(string questId, int currentDay)
        {
            var progress = GetProgress(questId);
            if (progress == null) return false;
            var def = GetDefinition(questId);
            if (def == null) return false;

            // cooldown_days == 0 means one-shot: once resolved, never re-trigger
            if (def.cooldown_days <= 0)
                return progress.completed || progress.failed;

            return progress.last_completed_day >= 0 &&
                   (currentDay - progress.last_completed_day) < def.cooldown_days;
        }

        public void StartQuest(string questId, int day)
        {
            var def = GetDefinition(questId);
            if (def == null) return;
            if (IsStarted(questId)) return;

            var progress = GetProgress(questId);
            if (progress != null)
            {
                if (progress.completed || progress.failed)
                {
                    progress.started = true;
                    progress.completed = false;
                    progress.failed = false;
                    progress.day_started = day;
                    progress.day_resolved = -1;
                    progress.chosen_choice_id = string.Empty;
                }
                else
                {
                    return;
                }
            }
            else
            {
                progress = new ThirdonaryProgress
                {
                    quest_id = questId,
                    started = true,
                    day_started = day
                };
                _state.quests.Add(progress);
            }

            OnQuestStarted?.Invoke(def);
            RaiseStateChanged();
        }

        public void CompleteQuest(string questId, int day)
        {
            var progress = GetProgress(questId);
            if (progress == null || !progress.started || progress.completed) return;

            progress.completed = true;
            progress.day_resolved = day;
            progress.last_completed_day = day;
            if (!_state.completed_quest_ids.Contains(questId))
                _state.completed_quest_ids.Add(questId);

            var def = GetDefinition(questId);
            if (def != null)
                OnQuestCompleted?.Invoke(def);
            RaiseStateChanged();
        }

        public void FailQuest(string questId, int day)
        {
            var progress = GetProgress(questId);
            if (progress == null || !progress.started || progress.failed) return;

            progress.failed = true;
            progress.day_resolved = day;
            progress.last_completed_day = day;
            if (!_state.failed_quest_ids.Contains(questId))
                _state.failed_quest_ids.Add(questId);

            var def = GetDefinition(questId);
            if (def != null)
                OnQuestFailed?.Invoke(def);
            RaiseStateChanged();
        }

        public void MakeChoice(string questId, string choiceId, int day)
        {
            var progress = GetProgress(questId);
            if (progress == null || !progress.started) return;

            progress.chosen_choice_id = choiceId;
            RaiseStateChanged();
        }

        /// <summary>
        /// Tick for a new day. Evaluates all catalog quests against the provided
        /// world state and starts eligible ones. Returns the IDs of newly started quests.
        /// </summary>
        public List<string> TickDay(ThirdonaryWorldState worldState)
        {
            var started = new List<string>();
            if (_catalog == null || _catalog.Count == 0) return started;
            if (worldState == null) return started;

            int day = worldState.CurrentDay;

            for (int i = 0; i < _catalog.Count; i++)
            {
                var def = _catalog[i];
                if (def == null || string.IsNullOrEmpty(def.id)) continue;

                if (!IsQuestEligible(def, day, worldState)) continue;

                StartQuest(def.id, day);
                if (IsStarted(def.id))
                    started.Add(def.id);
            }

            return started;
        }

        private bool IsQuestEligible(ThirdonaryQuestDef def, int day, ThirdonaryWorldState worldState)
        {
            if (def.min_day > 0 && day < def.min_day) return false;
            if (def.max_day > 0 && day > def.max_day) return false;

            if (IsOnCooldown(def.id, day)) return false;

            var progress = GetProgress(def.id);
            if (progress != null && progress.started && !progress.completed && !progress.failed)
                return false;

            if (def.trigger_flags != null && def.trigger_flags.Count > 0)
            {
                for (int f = 0; f < def.trigger_flags.Count; f++)
                {
                    if (!worldState.ActiveFlags.Contains(def.trigger_flags[f]))
                        return false;
                }
            }

            return true;
        }

        public List<ThirdonaryQuestDef> GetAvailableQuests(ThirdonaryWorldState worldState)
        {
            var result = new List<ThirdonaryQuestDef>();
            if (_catalog == null || worldState == null) return result;

            int day = worldState.CurrentDay;
            for (int i = 0; i < _catalog.Count; i++)
            {
                var def = _catalog[i];
                if (def != null && IsQuestEligible(def, day, worldState))
                    result.Add(def);
            }
            return result;
        }

        public List<ThirdonaryQuestDef> GetActiveQuests()
        {
            var result = new List<ThirdonaryQuestDef>();
            if (_state?.quests == null || _catalog == null) return result;

            for (int i = 0; i < _state.quests.Count; i++)
            {
                var p = _state.quests[i];
                if (p.started && !p.completed && !p.failed)
                {
                    var def = GetDefinition(p.quest_id);
                    if (def != null) result.Add(def);
                }
            }
            return result;
        }

        public ThirdonaryState CaptureState()
        {
            return Clone(_state);
        }

        public void RestoreState(ThirdonaryState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            if (!string.Equals(state.system_id, SystemId, StringComparison.Ordinal))
            {
                throw new ArgumentException(
                    $"State belongs to system '{state.system_id}', expected '{SystemId}'.", nameof(state));
            }
            if (state.schema_version > 1)
            {
                throw new NotSupportedException(
                    $"Future thirdonary save schema {state.schema_version}; supported schema is 1.");
            }
            _state = Clone(state);
            RaiseStateChanged();
        }

        private void RaiseStateChanged() => OnStateChanged?.Invoke(_state);

        private static ThirdonaryState Clone(ThirdonaryState source)
        {
            var copy = new ThirdonaryState
            {
                system_id = source.system_id,
                schema_version = source.schema_version,
                quests = new List<ThirdonaryProgress>(),
                completed_quest_ids = new List<string>(source.completed_quest_ids ?? new List<string>()),
                failed_quest_ids = new List<string>(source.failed_quest_ids ?? new List<string>())
            };
            if (source.quests != null)
            {
                foreach (var q in source.quests)
                {
                    copy.quests.Add(new ThirdonaryProgress
                    {
                        quest_id = q.quest_id,
                        started = q.started,
                        completed = q.completed,
                        failed = q.failed,
                        day_started = q.day_started,
                        day_resolved = q.day_resolved,
                        chosen_choice_id = q.chosen_choice_id,
                        last_completed_day = q.last_completed_day
                    });
                }
            }
            return copy;
        }
    }
}
