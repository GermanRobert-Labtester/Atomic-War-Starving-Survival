using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Ashfall.Core.Feedback
{
    /// <summary>
    /// Engine-agnostic catalog of user-facing feedback message templates.
    /// Manages message lookup, category-scoped lookup, safe parameter substitution,
    /// severity routing, and display timing calculation.
    /// </summary>
    public class FeedbackMessageCatalog
    {
        private static readonly Regex ParameterRegex = new Regex(@"\{(\d+)\}", RegexOptions.Compiled);

        private static readonly Dictionary<string, string> CategoryDefaults = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "success", "Success! Operation completed." },
            { "failure", "Operation failed. Check your inputs." },
            { "warning", "Warning: Proceed with caution." },
            { "error", "Error: Something went wrong." },
            { "confirmation", "Are you sure you want to proceed?" },
            { "progress", "Progress: {0}% complete." },
            { "reward", "You've earned a reward!" },
            { "penalty", "Penalty incurred. Check your status." },
            { "status", "Status: Normal." },
            { "alert", "ALERT: Important update available!" },
            { "hint", "Tip: Check your surroundings for clues." },
            { "spoiler", "SPOILER WARNING: This action may reveal major plot points." },
            { "time_pressure", "HURRY! Time is running out!" },
            { "resource_warning", "WARNING: Resource levels are critical!" },
            { "health_warning", "DANGER: Health critical!" },
            { "morale_warning", "WARNING: Morale is dangerously low!" },
            { "relationship", "Relationship status updated." },
            { "faction", "Faction relationship updated." },
            { "world_state", "World state updated." },
            { "system", "System status updated." }
        };

        private readonly List<FeedbackMessageTemplate> _allTemplates = new List<FeedbackMessageTemplate>();
        private readonly Dictionary<string, FeedbackMessageTemplate> _templatesByKey = new Dictionary<string, FeedbackMessageTemplate>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, FeedbackMessageTemplate> _templatesByCategoryAndKey = new Dictionary<string, FeedbackMessageTemplate>(StringComparer.OrdinalIgnoreCase);

        public IReadOnlyList<FeedbackMessageTemplate> AllTemplates => _allTemplates;
        public IReadOnlyDictionary<string, FeedbackMessageTemplate> Templates => _templatesByKey;

        public FeedbackMessageCatalog(IEnumerable<FeedbackMessageTemplate>? templates = null)
        {
            if (templates != null)
            {
                foreach (var t in templates)
                {
                    RegisterTemplate(t);
                }
            }
        }

        public void RegisterTemplate(FeedbackMessageTemplate template)
        {
            if (template != null && !string.IsNullOrEmpty(template.key))
            {
                _allTemplates.Add(template);
                _templatesByKey[template.key] = template;

                if (!string.IsNullOrEmpty(template.category))
                {
                    string composite = $"{template.category}:{template.key}";
                    _templatesByCategoryAndKey[composite] = template;
                }
            }
        }

        public bool TryGetTemplate(string key, out FeedbackMessageTemplate? template)
        {
            return _templatesByKey.TryGetValue(key, out template);
        }

        public bool TryGetTemplate(string category, string key, out FeedbackMessageTemplate? template)
        {
            if (!string.IsNullOrEmpty(category))
            {
                string composite = $"{category}:{key}";
                if (_templatesByCategoryAndKey.TryGetValue(composite, out template))
                    return true;
            }
            return _templatesByKey.TryGetValue(key, out template);
        }

        public FeedbackMessageTemplate? GetTemplate(string key)
        {
            _templatesByKey.TryGetValue(key, out var template);
            return template;
        }

        public FeedbackMessageTemplate? GetTemplate(string category, string key)
        {
            TryGetTemplate(category, key, out var template);
            return template;
        }

        public string GetCategoryDefault(string category)
        {
            if (CategoryDefaults.TryGetValue(category, out var def))
                return def;
            return "Operation status updated.";
        }

        public FeedbackSeverity GetSeverity(string key, FeedbackSeverity defaultSeverity = FeedbackSeverity.Info)
        {
            if (_templatesByKey.TryGetValue(key, out var t) && t != null)
                return t.GetSeverity();
            return defaultSeverity;
        }

        public FeedbackSeverity GetSeverity(string category, string key, FeedbackSeverity defaultSeverity = FeedbackSeverity.Info)
        {
            if (TryGetTemplate(category, key, out var t) && t != null)
                return t.GetSeverity();
            return defaultSeverity;
        }

        public float GetDisplayDuration(string key, float defaultDuration = 3.0f)
        {
            if (_templatesByKey.TryGetValue(key, out var t) && t != null && t.display_duration_seconds > 0)
                return t.display_duration_seconds;
            return defaultDuration;
        }

        public float GetDisplayDuration(string category, string key, float defaultDuration = 3.0f)
        {
            if (TryGetTemplate(category, key, out var t) && t != null && t.display_duration_seconds > 0)
                return t.display_duration_seconds;
            return defaultDuration;
        }

        public string Format(string key, params object[] args)
        {
            if (_templatesByKey.TryGetValue(key, out var template) && template != null)
            {
                return SafeFormat(template.template, args);
            }
            return SafeFormat("Status: {0}", args.Length > 0 ? args : new object[] { key });
        }

        public string FormatCategory(string category, string key, params object[] args)
        {
            if (TryGetTemplate(category, key, out var template) && template != null)
            {
                return SafeFormat(template.template, args);
            }
            string fallback = GetCategoryDefault(category);
            return SafeFormat(fallback, args);
        }

        public static string SafeFormat(string template, params object[]? args)
        {
            if (string.IsNullOrEmpty(template)) return string.Empty;
            if (args == null || args.Length == 0) return template;

            try
            {
                return string.Format(template, args);
            }
            catch (FormatException)
            {
                return ParameterRegex.Replace(template, match =>
                {
                    if (match.Groups.Count > 1 && int.TryParse(match.Groups[1].Value, out int idx))
                    {
                        if (idx >= 0 && idx < args.Length && args[idx] != null)
                        {
                            return args[idx].ToString() ?? string.Empty;
                        }
                    }
                    return string.Empty;
                });
            }
        }
    }
}
