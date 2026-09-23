// SPDX-License-Identifier: MIT
// Plan 144: Survivor Autonomy & Initiative System
// Authoritative domain logic for survivor agency, initiative, refusal, and personal goals.

using System;
using System.Collections.Generic;
using Ashfall.Core;

namespace Ashfall.Core.Survivors
{
    public enum AutonomyActionType
    {
        Help = 0,
        Refuse = 1,
        Initiate = 2,
        Express = 3,
        Pursue = 4
    }

    public enum AutonomyTriggerKind
    {
        Relationship = 0,
        Need = 1,
        Emotion = 2,
        Opportunity = 3
    }

    public enum AutonomyOutcome
    {
        Accepted = 0,
        Rejected = 1,
        Overridden = 2,
        Ignored = 3
    }

    [Serializable]
    public sealed class AutonomyActionTemplate
    {
        public string id { get; set; } = string.Empty;
        public string action_type { get; set; } = string.Empty;
        public string trigger_kind { get; set; } = string.Empty;
        public string title { get; set; } = string.Empty;
        public string description_template { get; set; } = string.Empty;
        public float base_probability { get; set; } = 0.1f;
        public int affinity_delta { get; set; }
        public float morale_delta { get; set; }
        public string required_trait { get; set; } = string.Empty;
        public float min_morale { get; set; }
        public float max_morale { get; set; } = 100f;
        public float min_fatigue { get; set; }
    }

    [Serializable]
    public sealed class AutonomyCatalogData
    {
        public int schema_version { get; set; } = 1;
        public List<AutonomyActionTemplate> actions { get; set; } = new();
    }

    [Serializable]
    public sealed class AutonomyAction
    {
        public string actionId { get; set; } = string.Empty;
        public string templateId { get; set; } = string.Empty;
        public string actorId { get; set; } = string.Empty;
        public string targetId { get; set; } = string.Empty;
        public AutonomyActionType actionType { get; set; }
        public AutonomyTriggerKind triggerKind { get; set; }
        public string title { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public AutonomyOutcome outcome { get; set; } = AutonomyOutcome.Accepted;
        public int day { get; set; }
        public int affinityDelta { get; set; }
        public float moraleDelta { get; set; }
        public string details { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class SurvivorGoalProgress
    {
        public string goalId { get; set; } = string.Empty;
        public string survivorId { get; set; } = string.Empty;
        public string title { get; set; } = string.Empty;
        public int currentProgress { get; set; }
        public int targetProgress { get; set; } = 3;
        public bool isCompleted { get; set; }
    }

    [Serializable]
    public sealed class SurvivorAutonomySaveState
    {
        public int schema_version { get; set; } = 1;
        public List<AutonomyAction> recentActions { get; set; } = new();
        public Dictionary<string, int> survivorCooldowns { get; set; } = new();
        public Dictionary<string, SurvivorGoalProgress> survivorGoals { get; set; } = new();
        public int totalActionsTriggered { get; set; }
        public int totalOverridesEnforced { get; set; }
    }

    public interface IAutonomySink
    {
        void RecordAutonomyAction(AutonomyAction action);
    }

    /// <summary>
    /// Pure domain authority for survivor autonomy, initiatives, emotional refusals, and personal goals.
    /// Engine-free, deterministic via ISeededRng, with schema-versioned state.
    /// </summary>
    public sealed class SurvivorAutonomySystem
    {
        private readonly ISeededRng _rng;
        private readonly int _cooldownDays;
        private readonly List<AutonomyActionTemplate> _templates = new();
        private readonly Dictionary<string, AutonomyActionTemplate> _templatesById = new(StringComparer.OrdinalIgnoreCase);

        private readonly List<AutonomyAction> _recentActions = new();
        private readonly Dictionary<string, int> _survivorCooldowns = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, SurvivorGoalProgress> _survivorGoals = new(StringComparer.OrdinalIgnoreCase);
        private int _totalActionsTriggered;
        private int _totalOverridesEnforced;

        public const int MaxRecentActionsKept = 50;

        // Seam delegates for runtime observer decoupling
        public Action<AutonomyAction>? OnAutonomyActionExecutedSeam;
        public Action<string, string, int>? OnSurvivorHelpedSeam; // actorId, targetId, affinityDelta
        public Action<string, string>? OnWorkRefusalOverriddenSeam; // actorId, taskTitle
        public Action<string, string>? OnProjectInitiatedSeam; // actorId, projectName

        public IAutonomySink? AutonomySink { get; set; }

        public IReadOnlyList<AutonomyActionTemplate> Templates => _templates;
        public IReadOnlyList<AutonomyAction> RecentActions => _recentActions;
        public int TotalActionsTriggered => _totalActionsTriggered;
        public int TotalOverridesEnforced => _totalOverridesEnforced;

        public SurvivorAutonomySystem(ISeededRng? rng = null, int cooldownDays = 2)
        {
            _rng = rng ?? new SeededRng(144);
            _cooldownDays = Math.Max(1, cooldownDays);
        }

        public void LoadCatalog(string jsonContent)
        {
            if (string.IsNullOrWhiteSpace(jsonContent))
                throw new ArgumentException("Catalog JSON content cannot be empty", nameof(jsonContent));

            _templates.Clear();
            _templatesById.Clear();

            // Lightweight engine-free JSON parsing
            var catalog = ParseCatalog(jsonContent);
            if (catalog?.actions != null)
            {
                foreach (var template in catalog.actions)
                {
                    if (!string.IsNullOrEmpty(template.id))
                    {
                        _templates.Add(template);
                        _templatesById[template.id] = template;
                    }
                }
            }
        }

        public void LoadCatalog(IFileIO fileIO, string path)
        {
            if (fileIO == null) throw new ArgumentNullException(nameof(fileIO));
            if (!fileIO.FileExists(path))
                throw new System.IO.FileNotFoundException($"Autonomy actions catalog not found at {path}");

            LoadCatalog(fileIO.ReadAllText(path));
        }

        public bool IsOnCooldown(string survivorId, int currentDay)
        {
            if (string.IsNullOrEmpty(survivorId)) return false;
            if (_survivorCooldowns.TryGetValue(survivorId, out int lastDay))
            {
                return (currentDay - lastDay) < _cooldownDays;
            }
            return false;
        }

        public AutonomyAction? EvaluateDailyAutonomy(
            string actorId,
            int currentDay,
            float morale,
            float fatigue,
            string[]? traits = null,
            string? targetId = null,
            int pairAffinity = 0)
        {
            if (string.IsNullOrEmpty(actorId)) return null;
            if (IsOnCooldown(actorId, currentDay)) return null;

            var eligible = new List<AutonomyActionTemplate>();
            var traitSet = new HashSet<string>(traits ?? Array.Empty<string>(), StringComparer.OrdinalIgnoreCase);

            for (int i = 0; i < _templates.Count; i++)
            {
                var t = _templates[i];

                // Check need / morale gates
                if (morale < t.min_morale || morale > t.max_morale) continue;
                if (fatigue < t.min_fatigue) continue;

                // Check target requirement (Help usually requires a target, Pursue/Initiate/Express do not)
                bool isHelp = string.Equals(t.action_type, "help", StringComparison.OrdinalIgnoreCase);
                if (isHelp && string.IsNullOrEmpty(targetId)) continue;

                // Check required trait if specified
                if (!string.IsNullOrEmpty(t.required_trait) && !traitSet.Contains(t.required_trait))
                    continue;

                eligible.Add(t);
            }

            if (eligible.Count == 0) return null;

            // Pick candidate template deterministically
            int pickIndex = _rng.Next(0, eligible.Count);
            var selected = eligible[pickIndex];

            // Compute probability modifiers based on traits and state
            float probability = selected.base_probability;

            if (string.Equals(selected.action_type, "help", StringComparison.OrdinalIgnoreCase))
            {
                if (traitSet.Contains("social")) probability += 0.20f;
                if (traitSet.Contains("independent")) probability -= 0.10f;
                if (pairAffinity > 20) probability += 0.15f;
            }
            else if (string.Equals(selected.action_type, "refuse", StringComparison.OrdinalIgnoreCase))
            {
                if (traitSet.Contains("stubborn")) probability += 0.20f;
                if (traitSet.Contains("anxious")) probability += 0.20f;
                if (pairAffinity < -10) probability += 0.15f;
            }
            else if (string.Equals(selected.action_type, "initiate", StringComparison.OrdinalIgnoreCase))
            {
                if (traitSet.Contains("independent")) probability += 0.20f;
                if (traitSet.Contains("ambitious")) probability += 0.10f;
            }
            else if (string.Equals(selected.action_type, "express", StringComparison.OrdinalIgnoreCase))
            {
                if (traitSet.Contains("social")) probability += 0.10f;
                if (traitSet.Contains("anxious")) probability += 0.10f;
            }
            else if (string.Equals(selected.action_type, "pursue", StringComparison.OrdinalIgnoreCase))
            {
                if (traitSet.Contains("ambitious")) probability += 0.20f;
            }

            probability = Math.Max(0.01f, Math.Min(0.95f, probability));

            float roll = _rng.NextFloat();
            if (roll > probability)
            {
                return null; // Not triggered
            }

            // Trigger action!
            AutonomyActionType actionType = ParseActionType(selected.action_type);
            AutonomyTriggerKind triggerKind = ParseTriggerKind(selected.trigger_kind);

            string desc = selected.description_template
                .Replace("{actor}", actorId)
                .Replace("{target}", targetId ?? "a companion");

            var action = new AutonomyAction
            {
                actionId = $"auto_{currentDay}_{actorId}_{_totalActionsTriggered + 1}",
                templateId = selected.id,
                actorId = actorId,
                targetId = targetId ?? string.Empty,
                actionType = actionType,
                triggerKind = triggerKind,
                title = selected.title,
                description = desc,
                outcome = AutonomyOutcome.Accepted,
                day = currentDay,
                affinityDelta = selected.affinity_delta,
                moraleDelta = selected.morale_delta,
                details = $"Triggered with probability {probability:0.00} (rolled {roll:0.00})"
            };

            // Set cooldown
            _survivorCooldowns[actorId] = currentDay;
            _totalActionsTriggered++;

            // Track recent actions
            _recentActions.Add(action);
            if (_recentActions.Count > MaxRecentActionsKept)
            {
                _recentActions.RemoveAt(0);
            }

            // Seams and sinks
            OnAutonomyActionExecutedSeam?.Invoke(action);
            AutonomySink?.RecordAutonomyAction(action);

            if (actionType == AutonomyActionType.Help && !string.IsNullOrEmpty(targetId))
            {
                OnSurvivorHelpedSeam?.Invoke(actorId, targetId, selected.affinity_delta);
            }
            else if (actionType == AutonomyActionType.Initiate)
            {
                OnProjectInitiatedSeam?.Invoke(actorId, selected.title);
            }

            // Auto-advance goal if pursue
            if (actionType == AutonomyActionType.Pursue)
            {
                AdvanceGoal(actorId, 1);
            }

            return action;
        }

        public bool OverrideRefusal(string actionId, bool enforceWork)
        {
            if (string.IsNullOrEmpty(actionId)) return false;

            AutonomyAction? targetAction = null;
            for (int i = _recentActions.Count - 1; i >= 0; i--)
            {
                if (string.Equals(_recentActions[i].actionId, actionId, StringComparison.OrdinalIgnoreCase))
                {
                    targetAction = _recentActions[i];
                    break;
                }
            }

            if (targetAction == null || targetAction.actionType != AutonomyActionType.Refuse)
                return false;

            if (enforceWork)
            {
                targetAction.outcome = AutonomyOutcome.Overridden;
                targetAction.moraleDelta -= 8.0f;
                targetAction.affinityDelta -= 5;
                _totalOverridesEnforced++;
                OnWorkRefusalOverriddenSeam?.Invoke(targetAction.actorId, targetAction.title);
            }
            else
            {
                targetAction.outcome = AutonomyOutcome.Accepted;
            }

            return true;
        }

        public void AssignGoal(string survivorId, string goalId, string title, int targetProgress = 3)
        {
            if (string.IsNullOrEmpty(survivorId)) return;
            _survivorGoals[survivorId] = new SurvivorGoalProgress
            {
                goalId = goalId,
                survivorId = survivorId,
                title = title,
                currentProgress = 0,
                targetProgress = Math.Max(1, targetProgress),
                isCompleted = false
            };
        }

        public bool AdvanceGoal(string survivorId, int amount = 1)
        {
            if (string.IsNullOrEmpty(survivorId)) return false;
            if (!_survivorGoals.TryGetValue(survivorId, out var goal)) return false;

            if (goal.isCompleted) return false;

            goal.currentProgress += Math.Max(1, amount);
            if (goal.currentProgress >= goal.targetProgress)
            {
                goal.isCompleted = true;
            }
            return true;
        }

        public SurvivorGoalProgress? GetGoal(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return null;
            _survivorGoals.TryGetValue(survivorId, out var goal);
            return goal;
        }

        public SurvivorAutonomySaveState CaptureState()
        {
            var state = new SurvivorAutonomySaveState
            {
                schema_version = 1,
                totalActionsTriggered = _totalActionsTriggered,
                totalOverridesEnforced = _totalOverridesEnforced
            };

            foreach (var action in _recentActions)
            {
                state.recentActions.Add(new AutonomyAction
                {
                    actionId = action.actionId,
                    templateId = action.templateId,
                    actorId = action.actorId,
                    targetId = action.targetId,
                    actionType = action.actionType,
                    triggerKind = action.triggerKind,
                    title = action.title,
                    description = action.description,
                    outcome = action.outcome,
                    day = action.day,
                    affinityDelta = action.affinityDelta,
                    moraleDelta = action.moraleDelta,
                    details = action.details
                });
            }

            foreach (var kvp in _survivorCooldowns)
            {
                state.survivorCooldowns[kvp.Key] = kvp.Value;
            }

            foreach (var kvp in _survivorGoals)
            {
                state.survivorGoals[kvp.Key] = new SurvivorGoalProgress
                {
                    goalId = kvp.Value.goalId,
                    survivorId = kvp.Value.survivorId,
                    title = kvp.Value.title,
                    currentProgress = kvp.Value.currentProgress,
                    targetProgress = kvp.Value.targetProgress,
                    isCompleted = kvp.Value.isCompleted
                };
            }

            return state;
        }

        public void RestoreState(SurvivorAutonomySaveState? state)
        {
            _recentActions.Clear();
            _survivorCooldowns.Clear();
            _survivorGoals.Clear();
            _totalActionsTriggered = 0;
            _totalOverridesEnforced = 0;

            if (state == null) return;

            _totalActionsTriggered = state.totalActionsTriggered;
            _totalOverridesEnforced = state.totalOverridesEnforced;

            if (state.recentActions != null)
            {
                foreach (var action in state.recentActions)
                {
                    _recentActions.Add(new AutonomyAction
                    {
                        actionId = action.actionId,
                        templateId = action.templateId,
                        actorId = action.actorId,
                        targetId = action.targetId,
                        actionType = action.actionType,
                        triggerKind = action.triggerKind,
                        title = action.title,
                        description = action.description,
                        outcome = action.outcome,
                        day = action.day,
                        affinityDelta = action.affinityDelta,
                        moraleDelta = action.moraleDelta,
                        details = action.details
                    });
                }
            }

            if (state.survivorCooldowns != null)
            {
                foreach (var kvp in state.survivorCooldowns)
                {
                    _survivorCooldowns[kvp.Key] = kvp.Value;
                }
            }

            if (state.survivorGoals != null)
            {
                foreach (var kvp in state.survivorGoals)
                {
                    _survivorGoals[kvp.Key] = new SurvivorGoalProgress
                    {
                        goalId = kvp.Value.goalId,
                        survivorId = kvp.Value.survivorId,
                        title = kvp.Value.title,
                        currentProgress = kvp.Value.currentProgress,
                        targetProgress = kvp.Value.targetProgress,
                        isCompleted = kvp.Value.isCompleted
                    };
                }
            }
        }

        private static AutonomyActionType ParseActionType(string typeStr)
        {
            if (string.Equals(typeStr, "help", StringComparison.OrdinalIgnoreCase)) return AutonomyActionType.Help;
            if (string.Equals(typeStr, "refuse", StringComparison.OrdinalIgnoreCase)) return AutonomyActionType.Refuse;
            if (string.Equals(typeStr, "initiate", StringComparison.OrdinalIgnoreCase)) return AutonomyActionType.Initiate;
            if (string.Equals(typeStr, "express", StringComparison.OrdinalIgnoreCase)) return AutonomyActionType.Express;
            if (string.Equals(typeStr, "pursue", StringComparison.OrdinalIgnoreCase)) return AutonomyActionType.Pursue;
            return AutonomyActionType.Express;
        }

        private static AutonomyTriggerKind ParseTriggerKind(string kindStr)
        {
            if (string.Equals(kindStr, "relationship", StringComparison.OrdinalIgnoreCase)) return AutonomyTriggerKind.Relationship;
            if (string.Equals(kindStr, "need", StringComparison.OrdinalIgnoreCase)) return AutonomyTriggerKind.Need;
            if (string.Equals(kindStr, "emotion", StringComparison.OrdinalIgnoreCase)) return AutonomyTriggerKind.Emotion;
            if (string.Equals(kindStr, "opportunity", StringComparison.OrdinalIgnoreCase)) return AutonomyTriggerKind.Opportunity;
            return AutonomyTriggerKind.Opportunity;
        }

        private static AutonomyCatalogData ParseCatalog(string json)
        {
            var data = new AutonomyCatalogData();
            // Robust simple extractor for JSON templates
            int actionsIdx = json.IndexOf("\"actions\"", StringComparison.Ordinal);
            if (actionsIdx < 0) return data;

            int arrStart = json.IndexOf('[', actionsIdx);
            if (arrStart < 0) return data;

            int arrEnd = json.LastIndexOf(']');
            if (arrEnd <= arrStart) return data;

            string body = json.Substring(arrStart + 1, arrEnd - arrStart - 1);
            var blocks = SplitJsonObjects(body);

            foreach (var block in blocks)
            {
                var template = new AutonomyActionTemplate
                {
                    id = ExtractString(block, "id"),
                    action_type = ExtractString(block, "action_type"),
                    trigger_kind = ExtractString(block, "trigger_kind"),
                    title = ExtractString(block, "title"),
                    description_template = ExtractString(block, "description_template"),
                    base_probability = ExtractFloat(block, "base_probability", 0.1f),
                    affinity_delta = ExtractInt(block, "affinity_delta", 0),
                    morale_delta = ExtractFloat(block, "morale_delta", 0f),
                    required_trait = ExtractString(block, "required_trait"),
                    min_morale = ExtractFloat(block, "min_morale", 0f),
                    max_morale = ExtractFloat(block, "max_morale", 100f),
                    min_fatigue = ExtractFloat(block, "min_fatigue", 0f)
                };
                if (!string.IsNullOrEmpty(template.id))
                {
                    data.actions.Add(template);
                }
            }

            return data;
        }

        private static List<string> SplitJsonObjects(string content)
        {
            var results = new List<string>();
            int depth = 0;
            int start = -1;
            bool inString = false;

            for (int i = 0; i < content.Length; i++)
            {
                char c = content[i];
                if (c == '"' && (i == 0 || content[i - 1] != '\\'))
                {
                    inString = !inString;
                }
                else if (!inString)
                {
                    if (c == '{')
                    {
                        if (depth == 0) start = i;
                        depth++;
                    }
                    else if (c == '}')
                    {
                        depth--;
                        if (depth == 0 && start >= 0)
                        {
                            results.Add(content.Substring(start, i - start + 1));
                            start = -1;
                        }
                    }
                }
            }

            return results;
        }

        private static string ExtractString(string json, string key)
        {
            string pattern = $"\"{key}\"";
            int idx = json.IndexOf(pattern, StringComparison.Ordinal);
            if (idx < 0) return string.Empty;

            int colon = json.IndexOf(':', idx + pattern.Length);
            if (colon < 0) return string.Empty;

            int quoteStart = json.IndexOf('"', colon + 1);
            if (quoteStart < 0) return string.Empty;

            int quoteEnd = quoteStart + 1;
            while (quoteEnd < json.Length)
            {
                if (json[quoteEnd] == '"' && json[quoteEnd - 1] != '\\')
                    break;
                quoteEnd++;
            }

            if (quoteEnd >= json.Length) return string.Empty;
            return json.Substring(quoteStart + 1, quoteEnd - quoteStart - 1).Trim();
        }

        private static float ExtractFloat(string json, string key, float defaultValue)
        {
            string pattern = $"\"{key}\"";
            int idx = json.IndexOf(pattern, StringComparison.Ordinal);
            if (idx < 0) return defaultValue;

            int colon = json.IndexOf(':', idx + pattern.Length);
            if (colon < 0) return defaultValue;

            int start = colon + 1;
            while (start < json.Length && (char.IsWhiteSpace(json[start]))) start++;

            int end = start;
            while (end < json.Length && (char.IsDigit(json[end]) || json[end] == '.' || json[end] == '-')) end++;

            if (end > start && float.TryParse(json.Substring(start, end - start), System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float result))
            {
                return result;
            }
            return defaultValue;
        }

        private static int ExtractInt(string json, string key, int defaultValue)
        {
            string pattern = $"\"{key}\"";
            int idx = json.IndexOf(pattern, StringComparison.Ordinal);
            if (idx < 0) return defaultValue;

            int colon = json.IndexOf(':', idx + pattern.Length);
            if (colon < 0) return defaultValue;

            int start = colon + 1;
            while (start < json.Length && (char.IsWhiteSpace(json[start]))) start++;

            int end = start;
            while (end < json.Length && (char.IsDigit(json[end]) || json[end] == '-')) end++;

            if (end > start && int.TryParse(json.Substring(start, end - start), System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out int result))
            {
                return result;
            }
            return defaultValue;
        }
    }
}
