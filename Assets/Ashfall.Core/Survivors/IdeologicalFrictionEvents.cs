// SPDX-License-Identifier: MIT
// Plan 148: Ideological Friction → Events & Quests System
// Pure domain authority transforming ideological differences into emergent confrontations, conversion attempts, bunker splits, and mediation quests.

using System;
using System.Collections.Generic;
using Ashfall.Core;

namespace Ashfall.Core.Survivors
{
    public enum IdeologicalEventType
    {
        Confrontation = 0,
        ConversionAttempt = 1,
        BunkerSplit = 2,
        MediationQuest = 3
    }

    public enum IdeologicalMediationChoice
    {
        SideWithActor = 0,
        SideWithTarget = 1,
        StayNeutral = 2,
        BrokerCompromise = 3
    }

    [Serializable]
    public sealed class IdeologicalEventTemplate
    {
        public string id { get; set; } = string.Empty;
        public string event_type { get; set; } = string.Empty;
        public List<string> belief_pair { get; set; } = new();
        public string title { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public float base_probability { get; set; } = 0.2f;
        public float affinity_threshold { get; set; }
        public string consequences_description { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class IdeologicalEventInstance
    {
        public string instanceId { get; set; } = string.Empty;
        public string templateId { get; set; } = string.Empty;
        public IdeologicalEventType eventType { get; set; }
        public string actorId { get; set; } = string.Empty;
        public string actorBelief { get; set; } = string.Empty;
        public string targetId { get; set; } = string.Empty;
        public string targetBelief { get; set; } = string.Empty;
        public int day { get; set; }
        public string title { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public bool isResolved { get; set; }
        public string resolutionDetails { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class IdeologicalFactionState
    {
        public string beliefId { get; set; } = string.Empty;
        public List<string> memberIds { get; set; } = new();
        public string leaderId { get; set; } = string.Empty;
        public bool isRecognized { get; set; }
    }

    [Serializable]
    public sealed class IdeologicalFrictionEventSaveState
    {
        public int schema_version { get; set; } = 1;
        public List<IdeologicalEventInstance> recentEvents { get; set; } = new();
        public Dictionary<string, int> pairCooldowns { get; set; } = new();
        public List<IdeologicalFactionState> activeFactions { get; set; } = new();
        public int totalEventsFired { get; set; }
        public int totalConversionsSucceeded { get; set; }
    }

    public sealed class IdeologicalFrictionEvents
    {
        private readonly List<IdeologicalEventTemplate> _templates = new();
        private readonly List<IdeologicalEventInstance> _recentEvents = new();
        private readonly Dictionary<string, int> _pairCooldowns = new(StringComparer.OrdinalIgnoreCase);
        private readonly List<IdeologicalFactionState> _activeFactions = new();
        private int _totalEventsFired;
        private int _totalConversionsSucceeded;

        public const int CooldownDays = 14;
        public const int MaxRecentEventsKept = 50;

        // Seams
        public Action<IdeologicalEventInstance>? OnIdeologicalEventTriggeredSeam;
        public Action<string, string, string>? OnBeliefConversionSucceededSeam; // targetId, oldBelief, newBelief
        public Action<IdeologicalEventInstance, IdeologicalMediationChoice>? OnIdeologicalMediationResolvedSeam;
        public Action<string, int>? OnBunkerSplitEscalatedSeam; // beliefId, memberCount

        public IReadOnlyList<IdeologicalEventTemplate> Templates => _templates;
        public IReadOnlyList<IdeologicalEventInstance> RecentEvents => _recentEvents;
        public IReadOnlyList<IdeologicalFactionState> ActiveFactions => _activeFactions;
        public int TotalEventsFired => _totalEventsFired;
        public int TotalConversionsSucceeded => _totalConversionsSucceeded;

        public IdeologicalFrictionEvents()
        {
            LoadDefaultTemplates();
        }

        public void LoadCatalog(string jsonContent)
        {
            if (string.IsNullOrWhiteSpace(jsonContent)) return;

            _templates.Clear();
            ParseCatalog(jsonContent);
        }

        public void LoadCatalog(IFileIO fileIO, string path)
        {
            if (fileIO == null) throw new ArgumentNullException(nameof(fileIO));
            if (!fileIO.FileExists(path))
                throw new System.IO.FileNotFoundException($"Ideological events catalog not found at {path}");

            LoadCatalog(fileIO.ReadAllText(path));
        }

        public bool IsPairOnCooldown(string survivorA, string survivorB, int currentDay)
        {
            string key = GetPairKey(survivorA, survivorB);
            if (_pairCooldowns.TryGetValue(key, out int lastDay))
            {
                return (currentDay - lastDay) < CooldownDays;
            }
            return false;
        }

        public IdeologicalEventInstance? CheckDailyFriction(
            string survivorA,
            string beliefA,
            string survivorB,
            string beliefB,
            float pairAffinity,
            int currentDay,
            ISeededRng? rng = null,
            bool forceTrigger = false)
        {
            if (string.IsNullOrEmpty(survivorA) || string.IsNullOrEmpty(survivorB)) return null;
            if (string.IsNullOrEmpty(beliefA) || string.IsNullOrEmpty(beliefB)) return null;
            if (IsPairOnCooldown(survivorA, survivorB, currentDay)) return null;

            bool isConflicting = AreBeliefsConflicting(beliefA, beliefB);
            if (!isConflicting) return null;

            // Select template candidate based on affinity thresholds
            IdeologicalEventTemplate? selected = null;

            if (pairAffinity <= -80f)
            {
                selected = FindTemplate(IdeologicalEventType.MediationQuest, beliefA, beliefB);
            }
            else if (pairAffinity <= -50f)
            {
                selected = FindTemplate(IdeologicalEventType.Confrontation, beliefA, beliefB);
            }
            else if (pairAffinity >= 30f)
            {
                selected = FindTemplate(IdeologicalEventType.ConversionAttempt, beliefA, beliefB);
            }

            if (selected == null) return null;

            if (!forceTrigger)
            {
                // Deterministic roll
                rng ??= new SeededRng(148);
                float roll = rng.NextFloat();
                if (roll > selected.base_probability)
                {
                    return null;
                }
            }

            // Create event instance
            IdeologicalEventType eventType = ParseEventType(selected.event_type);
            string desc = selected.description
                .Replace("{actor}", survivorA)
                .Replace("{target}", survivorB);

            var instance = new IdeologicalEventInstance
            {
                instanceId = $"ideo_{currentDay}_{survivorA}_{survivorB}_{_totalEventsFired + 1}",
                templateId = selected.id,
                eventType = eventType,
                actorId = survivorA,
                actorBelief = beliefA,
                targetId = survivorB,
                targetBelief = beliefB,
                day = currentDay,
                title = selected.title,
                description = desc,
                isResolved = false,
                resolutionDetails = "Pending resolution"
            };

            // Set cooldown
            _pairCooldowns[GetPairKey(survivorA, survivorB)] = currentDay;
            _totalEventsFired++;

            _recentEvents.Add(instance);
            if (_recentEvents.Count > MaxRecentEventsKept)
            {
                _recentEvents.RemoveAt(0);
            }

            OnIdeologicalEventTriggeredSeam?.Invoke(instance);

            return instance;
        }

        public bool ResolveConfrontation(
            string instanceId,
            IdeologicalMediationChoice choice,
            out float affinityDeltaA,
            out float affinityDeltaB,
            out float moraleDelta)
        {
            affinityDeltaA = 0f;
            affinityDeltaB = 0f;
            moraleDelta = 0f;

            var instance = _recentEvents.Find(e => string.Equals(e.instanceId, instanceId, StringComparison.OrdinalIgnoreCase));
            if (instance == null || instance.isResolved)
                return false;

            switch (choice)
            {
                case IdeologicalMediationChoice.SideWithActor:
                    affinityDeltaA = 20f;
                    affinityDeltaB = -30f;
                    moraleDelta = -5f;
                    instance.resolutionDetails = $"Player endorsed {instance.actorId}'s position, alienating {instance.targetId}.";
                    break;
                case IdeologicalMediationChoice.SideWithTarget:
                    affinityDeltaA = -30f;
                    affinityDeltaB = 20f;
                    moraleDelta = -5f;
                    instance.resolutionDetails = $"Player endorsed {instance.targetId}'s position, alienating {instance.actorId}.";
                    break;
                case IdeologicalMediationChoice.StayNeutral:
                    affinityDeltaA = -10f;
                    affinityDeltaB = -10f;
                    moraleDelta = -10f;
                    instance.resolutionDetails = "Player refused to intervene, leaving simmering resentment on both sides.";
                    break;
                case IdeologicalMediationChoice.BrokerCompromise:
                    affinityDeltaA = 10f;
                    affinityDeltaB = 10f;
                    moraleDelta = 5f;
                    instance.resolutionDetails = "Player brokered a principled compromise, restoring mutual respect.";
                    break;
            }

            instance.isResolved = true;
            OnIdeologicalMediationResolvedSeam?.Invoke(instance, choice);
            return true;
        }

        public bool AttemptConversion(
            string actorId,
            string targetId,
            string actorBelief,
            float successProbability,
            ISeededRng rng,
            out string resultMessage)
        {
            if (string.IsNullOrEmpty(actorId) || string.IsNullOrEmpty(targetId))
            {
                resultMessage = "Invalid conversion participants.";
                return false;
            }

            float roll = rng.NextFloat();
            if (roll < successProbability)
            {
                _totalConversionsSucceeded++;
                OnBeliefConversionSucceededSeam?.Invoke(targetId, "previous_belief", actorBelief);
                resultMessage = $"{targetId} was deeply moved by {actorId}'s conviction and embraced {actorBelief}.";
                return true;
            }
            else
            {
                resultMessage = $"{targetId} listened thoughtfully but maintained their foundational worldview.";
                return false;
            }
        }

        public List<IdeologicalFactionState> UpdateBunkerFactions(Dictionary<string, string> survivorBeliefs)
        {
            _activeFactions.Clear();
            if (survivorBeliefs == null || survivorBeliefs.Count == 0) return _activeFactions;

            var groups = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
            foreach (var kvp in survivorBeliefs)
            {
                if (string.IsNullOrEmpty(kvp.Value)) continue;
                if (!groups.TryGetValue(kvp.Value, out var list))
                {
                    list = new List<string>();
                    groups[kvp.Value] = list;
                }
                list.Add(kvp.Key);
            }

            foreach (var kvp in groups)
            {
                if (kvp.Value.Count >= 3)
                {
                    var faction = new IdeologicalFactionState
                    {
                        beliefId = kvp.Key,
                        memberIds = kvp.Value,
                        leaderId = kvp.Value[0],
                        isRecognized = false
                    };
                    _activeFactions.Add(faction);
                }
            }

            // Check if opposing factions both exist
            for (int i = 0; i < _activeFactions.Count; i++)
            {
                for (int j = i + 1; j < _activeFactions.Count; j++)
                {
                    if (AreBeliefsConflicting(_activeFactions[i].beliefId, _activeFactions[j].beliefId))
                    {
                        OnBunkerSplitEscalatedSeam?.Invoke(_activeFactions[i].beliefId, _activeFactions[i].memberIds.Count);
                        OnBunkerSplitEscalatedSeam?.Invoke(_activeFactions[j].beliefId, _activeFactions[j].memberIds.Count);
                    }
                }
            }

            return _activeFactions;
        }

        public IdeologicalFrictionEventSaveState CaptureState()
        {
            var state = new IdeologicalFrictionEventSaveState
            {
                schema_version = 1,
                totalEventsFired = _totalEventsFired,
                totalConversionsSucceeded = _totalConversionsSucceeded
            };

            foreach (var e in _recentEvents)
            {
                state.recentEvents.Add(new IdeologicalEventInstance
                {
                    instanceId = e.instanceId,
                    templateId = e.templateId,
                    eventType = e.eventType,
                    actorId = e.actorId,
                    actorBelief = e.actorBelief,
                    targetId = e.targetId,
                    targetBelief = e.targetBelief,
                    day = e.day,
                    title = e.title,
                    description = e.description,
                    isResolved = e.isResolved,
                    resolutionDetails = e.resolutionDetails
                });
            }

            foreach (var kvp in _pairCooldowns)
            {
                state.pairCooldowns[kvp.Key] = kvp.Value;
            }

            foreach (var f in _activeFactions)
            {
                state.activeFactions.Add(new IdeologicalFactionState
                {
                    beliefId = f.beliefId,
                    memberIds = new List<string>(f.memberIds),
                    leaderId = f.leaderId,
                    isRecognized = f.isRecognized
                });
            }

            return state;
        }

        public void RestoreState(IdeologicalFrictionEventSaveState? state)
        {
            _recentEvents.Clear();
            _pairCooldowns.Clear();
            _activeFactions.Clear();
            _totalEventsFired = 0;
            _totalConversionsSucceeded = 0;

            if (state == null) return;

            _totalEventsFired = state.totalEventsFired;
            _totalConversionsSucceeded = state.totalConversionsSucceeded;

            if (state.recentEvents != null)
            {
                foreach (var e in state.recentEvents)
                {
                    _recentEvents.Add(new IdeologicalEventInstance
                    {
                        instanceId = e.instanceId,
                        templateId = e.templateId,
                        eventType = e.eventType,
                        actorId = e.actorId,
                        actorBelief = e.actorBelief,
                        targetId = e.targetId,
                        targetBelief = e.targetBelief,
                        day = e.day,
                        title = e.title,
                        description = e.description,
                        isResolved = e.isResolved,
                        resolutionDetails = e.resolutionDetails
                    });
                }
            }

            if (state.pairCooldowns != null)
            {
                foreach (var kvp in state.pairCooldowns)
                {
                    _pairCooldowns[kvp.Key] = kvp.Value;
                }
            }

            if (state.activeFactions != null)
            {
                foreach (var f in state.activeFactions)
                {
                    _activeFactions.Add(new IdeologicalFactionState
                    {
                        beliefId = f.beliefId,
                        memberIds = new List<string>(f.memberIds),
                        leaderId = f.leaderId,
                        isRecognized = f.isRecognized
                    });
                }
            }
        }

        private static string GetPairKey(string a, string b)
        {
            return string.CompareOrdinal(a, b) <= 0 ? $"{a}:{b}" : $"{b}:{a}";
        }

        private static bool AreBeliefsConflicting(string a, string b)
        {
            if (IdeologicalFrictionSystem.ConflictGroups.TryGetValue(a, out var confA) && confA.Contains(b))
                return true;
            if (IdeologicalFrictionSystem.ConflictGroups.TryGetValue(b, out var confB) && confB.Contains(a))
                return true;
            return false;
        }

        private IdeologicalEventTemplate? FindTemplate(IdeologicalEventType eventType, string beliefA, string beliefB)
        {
            string typeStr = eventType switch
            {
                IdeologicalEventType.Confrontation => "confrontation",
                IdeologicalEventType.ConversionAttempt => "conversion",
                IdeologicalEventType.BunkerSplit => "split",
                IdeologicalEventType.MediationQuest => "quest",
                _ => "confrontation"
            };

            for (int i = 0; i < _templates.Count; i++)
            {
                var t = _templates[i];
                if (!string.Equals(t.event_type, typeStr, StringComparison.OrdinalIgnoreCase))
                    continue;

                if (t.belief_pair != null && t.belief_pair.Count >= 2)
                {
                    bool match = (string.Equals(t.belief_pair[0], beliefA, StringComparison.OrdinalIgnoreCase) && string.Equals(t.belief_pair[1], beliefB, StringComparison.OrdinalIgnoreCase))
                              || (string.Equals(t.belief_pair[0], beliefB, StringComparison.OrdinalIgnoreCase) && string.Equals(t.belief_pair[1], beliefA, StringComparison.OrdinalIgnoreCase));
                    if (match) return t;
                }
            }

            // Fallback to first matching event type if exact pair is not explicitly defined
            for (int i = 0; i < _templates.Count; i++)
            {
                var t = _templates[i];
                if (string.Equals(t.event_type, typeStr, StringComparison.OrdinalIgnoreCase))
                    return t;
            }

            return null;
        }

        private static IdeologicalEventType ParseEventType(string typeStr)
        {
            if (string.Equals(typeStr, "confrontation", StringComparison.OrdinalIgnoreCase)) return IdeologicalEventType.Confrontation;
            if (string.Equals(typeStr, "conversion", StringComparison.OrdinalIgnoreCase)) return IdeologicalEventType.ConversionAttempt;
            if (string.Equals(typeStr, "split", StringComparison.OrdinalIgnoreCase)) return IdeologicalEventType.BunkerSplit;
            if (string.Equals(typeStr, "quest", StringComparison.OrdinalIgnoreCase)) return IdeologicalEventType.MediationQuest;
            return IdeologicalEventType.Confrontation;
        }

        private void LoadDefaultTemplates()
        {
            _templates.Add(new IdeologicalEventTemplate
            {
                id = "event_theological_dispute",
                event_type = "confrontation",
                belief_pair = new() { "religious_faith", "atheist_rationalist" },
                title = "Theological Clash in the Bunks",
                description = "{actor} and {target} erupted into a bitter dispute over whether the nuclear deluge was divine wrath or atomic incompetence.",
                base_probability = 0.25f,
                affinity_threshold = -50f,
                consequences_description = "Mediation required."
            });
            _templates.Add(new IdeologicalEventTemplate
            {
                id = "event_quiet_evangelism",
                event_type = "conversion",
                belief_pair = new() { "religious_faith", "atheist_rationalist" },
                title = "Late-Night Faith Outreach",
                description = "Bound by growing mutual respect, {actor} gently shared morning prayers and scriptures with receptive {target}.",
                base_probability = 0.15f,
                affinity_threshold = 30f,
                consequences_description = "Worldview conversion attempt."
            });
            _templates.Add(new IdeologicalEventTemplate
            {
                id = "event_formal_mediation_quest",
                event_type = "quest",
                belief_pair = new() { "military_discipline", "pragmatic_individualism" },
                title = "Mediation of the Broken Pact",
                description = "Hostility between {actor} and {target} has reached boiling point (-80 affinity), threatening duty operations.",
                base_probability = 0.40f,
                affinity_threshold = -80f,
                consequences_description = "Urgent mediation quest unlocked."
            });
        }

        private void ParseCatalog(string json)
        {
            int idx = json.IndexOf("\"events\"", StringComparison.Ordinal);
            if (idx < 0) return;
            int arrStart = json.IndexOf('[', idx);
            if (arrStart < 0) return;
            int arrEnd = json.LastIndexOf(']');
            if (arrEnd <= arrStart) return;

            string slice = json.Substring(arrStart + 1, arrEnd - arrStart - 1);
            var blocks = SplitObjects(slice);
            foreach (var b in blocks)
            {
                var t = new IdeologicalEventTemplate
                {
                    id = ExtractString(b, "id"),
                    event_type = ExtractString(b, "event_type"),
                    title = ExtractString(b, "title"),
                    description = ExtractString(b, "description"),
                    base_probability = ExtractFloat(b, "base_probability", 0.2f),
                    affinity_threshold = ExtractFloat(b, "affinity_threshold", 0f),
                    consequences_description = ExtractString(b, "consequences_description")
                };

                // Parse belief_pair array
                int bpIdx = b.IndexOf("\"belief_pair\"", StringComparison.Ordinal);
                if (bpIdx >= 0)
                {
                    int s = b.IndexOf('[', bpIdx);
                    int e = b.IndexOf(']', s);
                    if (s >= 0 && e > s)
                    {
                        string bpSlice = b.Substring(s + 1, e - s - 1);
                        string[] items = bpSlice.Split(',');
                        foreach (var item in items)
                        {
                            string cleaned = item.Replace("\"", "").Trim();
                            if (!string.IsNullOrEmpty(cleaned))
                                t.belief_pair.Add(cleaned);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(t.id))
                {
                    _templates.Add(t);
                }
            }
        }

        private static List<string> SplitObjects(string content)
        {
            var results = new List<string>();
            int depth = 0, start = -1;
            bool inString = false;
            for (int i = 0; i < content.Length; i++)
            {
                char c = content[i];
                if (c == '"' && (i == 0 || content[i - 1] != '\\')) inString = !inString;
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
            string p = $"\"{key}\"";
            int idx = json.IndexOf(p, StringComparison.Ordinal);
            if (idx < 0) return string.Empty;
            int colon = json.IndexOf(':', idx + p.Length);
            if (colon < 0) return string.Empty;
            int q1 = json.IndexOf('"', colon + 1);
            if (q1 < 0) return string.Empty;
            int q2 = json.IndexOf('"', q1 + 1);
            while (q2 < json.Length && json[q2 - 1] == '\\') q2 = json.IndexOf('"', q2 + 1);
            if (q2 < 0) return string.Empty;
            return json.Substring(q1 + 1, q2 - q1 - 1).Trim();
        }

        private static float ExtractFloat(string json, string key, float def)
        {
            string p = $"\"{key}\"";
            int idx = json.IndexOf(p, StringComparison.Ordinal);
            if (idx < 0) return def;
            int colon = json.IndexOf(':', idx + p.Length);
            if (colon < 0) return def;
            int s = colon + 1;
            while (s < json.Length && char.IsWhiteSpace(json[s])) s++;
            int e = s;
            while (e < json.Length && (char.IsDigit(json[e]) || json[e] == '.' || json[e] == '-')) e++;
            if (e > s && float.TryParse(json.Substring(s, e - s), System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float val)) return val;
            return def;
        }
    }
}
