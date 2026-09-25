// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Telemetry
{
    /// <summary>
    /// Plan 46 / C2[20] — Closed vocabulary of local player-action metric types.
    /// Strictly anonymous, deterministic, and free of PII or free text.
    /// </summary>
    public static class PlaySessionActions
    {
        public const string SessionStart = "session_start";
        public const string SessionEnd = "session_end";
        public const string PanelOpened = "panel_opened";
        public const string PanelClosed = "panel_closed";
        public const string Sigil = "sigil";
        public const string RationPolicySet = "ration_policy_set";
        public const string Dispatch = "dispatch";
        public const string ChoiceResolved = "choice_resolved";
        public const string Consume = "consume";
        public const string Save = "save";
        public const string Quit = "quit";
        public const string DayAdvanced = "day_advanced";

        private static readonly HashSet<string> KnownActions = new HashSet<string>(StringComparer.Ordinal)
        {
            SessionStart, SessionEnd, PanelOpened, PanelClosed, Sigil,
            RationPolicySet, Dispatch, ChoiceResolved, Consume, Save, Quit, DayAdvanced
        };

        public static bool IsKnown(string action) => !string.IsNullOrEmpty(action) && KnownActions.Contains(action);
    }

    /// <summary>
    /// Plan 46 / C2[20] — Single line schema for local JSONL session recording under user:// (never over network).
    /// </summary>
    [Serializable]
    public sealed class PlaySessionEvent
    {
        [JsonPropertyName("schema_version")]
        public int SchemaVersion { get; set; } = 1;

        [JsonPropertyName("record_type")]
        public string RecordType { get; set; } = "action"; // "action" or "day_join"

        [JsonPropertyName("session_id")]
        public string SessionId { get; set; } = string.Empty;

        [JsonPropertyName("build_version")]
        public string BuildVersion { get; set; } = string.Empty;

        [JsonPropertyName("preset_id")]
        public string PresetId { get; set; } = "difficulty_standard";

        [JsonPropertyName("seed")]
        public long Seed { get; set; }

        [JsonPropertyName("day")]
        public int Day { get; set; } = 1;

        [JsonPropertyName("t_session_ms")]
        public long TSessionMs { get; set; }

        [JsonPropertyName("action")]
        public string Action { get; set; } = string.Empty;

        [JsonPropertyName("target_id")]
        public string TargetId { get; set; } = string.Empty;

        [JsonPropertyName("outcome")]
        public string Outcome { get; set; } = "ok";

        [JsonPropertyName("sigil")]
        public string Sigil { get; set; } = string.Empty;

        [JsonPropertyName("kind")]
        public string Kind { get; set; } = string.Empty;

        [JsonPropertyName("source_owner_id")]
        public string SourceOwnerId { get; set; } = string.Empty;

        [JsonPropertyName("primary_id")]
        public string PrimaryId { get; set; } = string.Empty;

        [JsonPropertyName("secondary_id")]
        public string SecondaryId { get; set; } = string.Empty;

        [JsonPropertyName("numeric")]
        public float Numeric { get; set; }
    }

    /// <summary>
    /// Pure domain engine-free recorder for local, opt-in player action and state metrics (Plan 46B).
    /// Memory bounded, no wall clock (monotonic session durations only), zero external telemetry network calls.
    /// </summary>
    public sealed class PlaySessionRecorder
    {
        public const int CurrentSchemaVersion = 1;
        public const int MaxBufferedEvents = 2000;

        private readonly string _sessionId;
        private readonly string _buildVersion;
        private long _seed;
        private string _presetId = "difficulty_standard";
        private readonly List<PlaySessionEvent> _buffer = new List<PlaySessionEvent>();
        private readonly object _lock = new object();

        public Action<PlaySessionEvent>? PlaySessionEventRecordedSeam { get; set; }

        public string SessionId => _sessionId;
        public string BuildVersion => _buildVersion;
        public long Seed => _seed;
        public string PresetId => _presetId;
        public int BufferedCount { get { lock (_lock) { return _buffer.Count; } } }

        public PlaySessionRecorder(string sessionId, string buildVersion = "1.0.0")
        {
            _sessionId = string.IsNullOrEmpty(sessionId) ? "local_session" : sessionId;
            _buildVersion = string.IsNullOrEmpty(buildVersion) ? "1.0.0" : buildVersion;
        }

        public void SetContext(long seed, string presetId)
        {
            _seed = seed;
            if (!string.IsNullOrEmpty(presetId))
            {
                _presetId = presetId;
            }
        }

        public PlaySessionEvent Record(string action, string targetId = "", string outcome = "ok", int day = 1, long tSessionMs = 0)
        {
            var evt = new PlaySessionEvent
            {
                SchemaVersion = CurrentSchemaVersion,
                RecordType = "action",
                SessionId = _sessionId,
                BuildVersion = _buildVersion,
                PresetId = _presetId,
                Seed = _seed,
                Day = Math.Max(1, day),
                TSessionMs = Math.Max(0, tSessionMs),
                Action = action ?? string.Empty,
                TargetId = targetId ?? string.Empty,
                Outcome = outcome ?? "ok"
            };

            Enqueue(evt);
            return evt;
        }

        public PlaySessionEvent RecordSigil(string sigil, int day = 1, long tSessionMs = 0)
        {
            var evt = new PlaySessionEvent
            {
                SchemaVersion = CurrentSchemaVersion,
                RecordType = "action",
                SessionId = _sessionId,
                BuildVersion = _buildVersion,
                PresetId = _presetId,
                Seed = _seed,
                Day = Math.Max(1, day),
                TSessionMs = Math.Max(0, tSessionMs),
                Action = PlaySessionActions.Sigil,
                Sigil = sigil ?? string.Empty,
                TargetId = sigil ?? string.Empty,
                Outcome = "observed"
            };

            Enqueue(evt);
            return evt;
        }

        public PlaySessionEvent JoinDay(
            int day,
            long tSessionMs,
            string ownerId,
            string kind,
            string primaryId = "",
            string secondaryId = "",
            float numeric = 0f)
        {
            var evt = new PlaySessionEvent
            {
                SchemaVersion = CurrentSchemaVersion,
                RecordType = "day_join",
                SessionId = _sessionId,
                BuildVersion = _buildVersion,
                PresetId = _presetId,
                Seed = _seed,
                Day = Math.Max(1, day),
                TSessionMs = Math.Max(0, tSessionMs),
                Action = PlaySessionActions.DayAdvanced,
                SourceOwnerId = ownerId ?? string.Empty,
                Kind = kind ?? string.Empty,
                PrimaryId = primaryId ?? string.Empty,
                SecondaryId = secondaryId ?? string.Empty,
                Numeric = numeric,
                Outcome = "consequence"
            };

            Enqueue(evt);
            return evt;
        }

        private void Enqueue(PlaySessionEvent evt)
        {
            lock (_lock)
            {
                if (_buffer.Count >= MaxBufferedEvents)
                {
                    _buffer.RemoveAt(0); // Bounded queue: drop oldest
                }
                _buffer.Add(evt);
            }

            PlaySessionEventRecordedSeam?.Invoke(evt);
        }

        public IReadOnlyList<PlaySessionEvent> Drain()
        {
            lock (_lock)
            {
                var drained = new List<PlaySessionEvent>(_buffer);
                _buffer.Clear();
                return drained;
            }
        }

        public string ToJsonLine(PlaySessionEvent evt)
        {
            if (evt == null) return string.Empty;
            return JsonSerializer.Serialize(evt);
        }
    }

    /// <summary>
    /// Plan 46 / C2[20] — Step in the first-hour player onboarding and survival funnel.
    /// </summary>
    public readonly struct FunnelStepDefinition
    {
        public string StepId { get; }
        public string DisplayName { get; }
        public string TargetActionOrSigil { get; }

        public FunnelStepDefinition(string stepId, string displayName, string targetActionOrSigil)
        {
            StepId = stepId;
            DisplayName = displayName;
            TargetActionOrSigil = targetActionOrSigil;
        }
    }

    /// <summary>
    /// Pure domain evaluator for the 7 canonical first-hour onboarding funnel steps.
    /// </summary>
    public sealed class FirstHourFunnel
    {
        public static readonly IReadOnlyList<FunnelStepDefinition> CanonicalSteps = new List<FunnelStepDefinition>
        {
            new FunnelStepDefinition("guidance_opened", "Guidance Opened", "protocol.ration"),
            new FunnelStepDefinition("first_craft", "First Crafting Action", "inventory.used"),
            new FunnelStepDefinition("first_dispatch", "First Expedition Dispatched", "expedition.dispatched"),
            new FunnelStepDefinition("first_ration_decision", "First Ration Policy Decided", "ration_policy_set"),
            new FunnelStepDefinition("first_storm_survived", "First Storm Survived", "weather.read"),
            new FunnelStepDefinition("first_day_past_tutorial", "First Day Beyond Tutorial", "day_advanced"),
            new FunnelStepDefinition("first_death_witnessed", "First Casualty Witnessed", "survivor_perished"),
            // Live FirstHour journey sigils (UI/UX audit 2026-09-25 follow-up):
            // the funnel previously tracked only the legacy protocol order, so
            // the live water → power → food → duty → dose → research journey
            // (and the two contextual lessons) were invisible to tuning.
            new FunnelStepDefinition("first_water", "First Water Treatment Started", "water.treatment_started"),
            new FunnelStepDefinition("first_power", "First Breaker Toggled", "power.breaker_toggled"),
            new FunnelStepDefinition("first_food", "First Food Ration Consumed", "food.ration_consumed"),
            new FunnelStepDefinition("first_duty", "First Duty Assigned", "duty.assigned"),
            new FunnelStepDefinition("first_dose", "First Dose Reading Opened", "dose.read"),
            new FunnelStepDefinition("first_research", "First Research Started", "research.started"),
        };

        private readonly Dictionary<string, long> _completedSteps = new Dictionary<string, long>(StringComparer.Ordinal);

        public IReadOnlyDictionary<string, long> CompletedSteps => _completedSteps;

        public bool ProcessEvent(PlaySessionEvent evt)
        {
            if (evt == null) return false;

            for (int i = 0; i < CanonicalSteps.Count; i++)
            {
                var step = CanonicalSteps[i];
                if (_completedSteps.ContainsKey(step.StepId)) continue;

                bool matched = false;
                if (!string.IsNullOrEmpty(evt.Sigil) && evt.Sigil.Equals(step.TargetActionOrSigil, StringComparison.OrdinalIgnoreCase))
                {
                    matched = true;
                }
                else if (evt.Action.Equals(step.TargetActionOrSigil, StringComparison.OrdinalIgnoreCase) ||
                         evt.TargetId.Equals(step.TargetActionOrSigil, StringComparison.OrdinalIgnoreCase) ||
                         evt.Kind.Equals(step.TargetActionOrSigil, StringComparison.OrdinalIgnoreCase))
                {
                    matched = true;
                }

                if (matched)
                {
                    _completedSteps[step.StepId] = evt.TSessionMs;
                    return true;
                }
            }

            return false;
        }

        public bool IsStepCompleted(string stepId) =>
            !string.IsNullOrEmpty(stepId) && _completedSteps.ContainsKey(stepId);

        public int CompletedStepCount => _completedSteps.Count;

        /// <summary>
        /// Restores completion timestamps captured by a previous session so the
        /// funnel does not report a step as "first" again after a load. Missing
        /// entries stay open; unknown ids are ignored.
        /// </summary>
        public void RestoreCompletedSteps(IReadOnlyDictionary<string, long>? completedSteps)
        {
            if (completedSteps == null) return;
            foreach (var pair in completedSteps)
            {
                if (string.IsNullOrEmpty(pair.Key)) continue;
                _completedSteps[pair.Key] = Math.Max(0L, pair.Value);
            }
        }
    }

    /// <summary>
    /// Pure domain aggregator over local JSONL play session event streams.
    /// Produces human-readable markdown summaries and funnel completion metrics (Plan 46C).
    /// </summary>
    public sealed class PlaySessionReport
    {
        public int TotalEvents { get; private set; }
        public int ActionEventsCount { get; private set; }
        public int DayJoinEventsCount { get; private set; }
        public int MaxDayReached { get; private set; }
        public int FunnelCompletedCount { get; private set; }
        public int FunnelCompletionRatePermille { get; private set; }
        public Dictionary<string, int> ActionHistogram { get; } = new Dictionary<string, int>(StringComparer.Ordinal);
        public Dictionary<string, int> PanelsOpened { get; } = new Dictionary<string, int>(StringComparer.Ordinal);

        public static PlaySessionReport Aggregate(IEnumerable<PlaySessionEvent> events, FirstHourFunnel? funnel = null)
        {
            var report = new PlaySessionReport();
            var activeFunnel = funnel ?? new FirstHourFunnel();

            if (events == null) return report;

            foreach (var evt in events)
            {
                if (evt == null) continue;
                report.TotalEvents++;

                if (evt.Day > report.MaxDayReached)
                {
                    report.MaxDayReached = evt.Day;
                }

                if (evt.RecordType == "action")
                {
                    report.ActionEventsCount++;
                    if (!string.IsNullOrEmpty(evt.Action))
                    {
                        report.ActionHistogram[evt.Action] = report.ActionHistogram.GetValueOrDefault(evt.Action, 0) + 1;
                    }

                    if (evt.Action == PlaySessionActions.PanelOpened && !string.IsNullOrEmpty(evt.TargetId))
                    {
                        report.PanelsOpened[evt.TargetId] = report.PanelsOpened.GetValueOrDefault(evt.TargetId, 0) + 1;
                    }
                }
                else if (evt.RecordType == "day_join")
                {
                    report.DayJoinEventsCount++;
                }

                activeFunnel.ProcessEvent(evt);
            }

            report.FunnelCompletedCount = activeFunnel.CompletedStepCount;
            int totalSteps = FirstHourFunnel.CanonicalSteps.Count;
            report.FunnelCompletionRatePermille = totalSteps > 0
                ? (report.FunnelCompletedCount * 1000) / totalSteps
                : 0;

            return report;
        }

        public string ToMarkdown()
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("# ASHFALL Local Play Session Telemetry Report");
            sb.AppendLine();
            sb.AppendLine($"- **Total Events Recorded:** {TotalEvents}");
            sb.AppendLine($"- **Player Actions:** {ActionEventsCount}");
            sb.AppendLine($"- **Simulation Consequence Joins:** {DayJoinEventsCount}");
            sb.AppendLine($"- **Max Campaign Day Reached:** {MaxDayReached}");
            sb.AppendLine($"- **First-Hour Funnel Progress:** {FunnelCompletedCount}/{FirstHourFunnel.CanonicalSteps.Count} ({FunnelCompletionRatePermille / 10.0:F1}%)");
            sb.AppendLine();
            sb.AppendLine("## Top Player Actions");
            foreach (var kvp in ActionHistogram)
            {
                sb.AppendLine($"- `{kvp.Key}`: {kvp.Value}");
            }
            sb.AppendLine();
            sb.AppendLine("## Surfaces & Panels Visited");
            foreach (var kvp in PanelsOpened)
            {
                sb.AppendLine($"- `{kvp.Key}`: {kvp.Value}");
            }
            return sb.ToString();
        }
    }
}
