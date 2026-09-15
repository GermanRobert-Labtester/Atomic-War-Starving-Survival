// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Radio
{
    /// <summary>Closed, validated trigger grammar for distress follow-ups.
    /// No arbitrary script strings — every value maps to one exactly-once
    /// lifecycle transition of the rescue-mission authority.</summary>
    public static class SignalFollowUpTriggers
    {
        /// <summary>The player dispatched an expedition to the parent signal.</summary>
        public const string Answered = "answered";

        /// <summary>The rescue authority resolved the rescue as successful.</summary>
        public const string RescueSuccess = "rescue_success";

        /// <summary>The expedition arrived after the sender died / past the deadline.</summary>
        public const string RescueFailed = "rescue_failed";

        /// <summary>A discovered, actionable parent signal expired unanswered
        /// (explicit decline via moral choice or deadline expiry).</summary>
        public const string Expired = "expired";

        /// <summary>The player dispatched into an authored trap and the ambush
        /// was actually encountered (authored trap aftermath is legal content).</summary>
        public const string AmbushEncountered = "ambush_encountered";

        /// <summary>Validation set — the only accepted trigger_condition values.</summary>
        public static readonly string[] All =
        {
            Answered, RescueSuccess, RescueFailed, Expired, AmbushEncountered
        };

        public static bool IsValid(string? trigger)
        {
            if (string.IsNullOrWhiteSpace(trigger)) return false;
            foreach (var t in All)
                if (string.Equals(t, trigger, StringComparison.OrdinalIgnoreCase)) return true;
            return false;
        }
    }

    /// <summary>
    /// Authored follow-up definition on a distress signal (additive
    /// <c>follow_up_signals</c> catalog field). A follow-up is a delayed,
    /// self-contained transmission payload caused by a qualifying parent
    /// outcome — it is NOT a message stage (stages progress the same signal
    /// identity by time; follow-ups are event-caused new transmissions).
    /// Wave 3 uses self-contained payloads, NOT catalog-backed signal
    /// references, so follow-up chains/cycles are structurally impossible;
    /// if catalog-backed references are added later, cycle validation
    /// becomes mandatory before any authored data ships.
    /// </summary>
    [Serializable]
    public sealed class SignalFollowUpDefinition
    {
        /// <summary>Stable dedupe identity (unique across the corpus). Dedupe
        /// key = parentSignalId + ":" + id — never text, audio, or title.</summary>
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("trigger_condition")]
        public string TriggerCondition { get; set; } = string.Empty;

        /// <summary>Campaign days after the qualifying event. 0 = same day.
        /// Negative values are validator-rejected.</summary>
        [JsonPropertyName("delay_days")]
        public int DelayDays { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;

        /// <summary>Reception clarity of the follow-up transmission (0..1).
        /// Required in authored data; default is a sane mid-high value.</summary>
        [JsonPropertyName("clarity")]
        public float Clarity { get; set; } = 0.9f;

        /// <summary>Optional player-facing clue, same rules as stage hints.</summary>
        [JsonPropertyName("outcome_hint")]
        public string OutcomeHint { get; set; } = string.Empty;

        /// <summary>Tasks 9–12 Wave 4 — optional audio cue for this follow-up
        /// transmission, resolved exactly like other radio content. Empty =
        /// text-only. Presentation only.</summary>
        [JsonPropertyName("audio_cue")]
        public string AudioCue { get; set; } = string.Empty;
    }

    /// <summary>Save DTO for one pending follow-up (campaign-day due date).</summary>
    [Serializable]
    public sealed class PendingSignalFollowUpEntry
    {
        public string parentSignalId = string.Empty;
        public string followUpId = string.Empty;
        public int dueDay;
    }

    /// <summary>Save DTO for the whole follow-up scheduler state.</summary>
    [Serializable]
    public sealed class SignalFollowUpSaveState
    {
        public int version = 1;
        public List<PendingSignalFollowUpEntry> pending = new List<PendingSignalFollowUpEntry>();
        public List<string> firedKeys = new List<string>();
    }

    /// <summary>
    /// Tasks 9–12 Wave 3 — radio-owned follow-up scheduler.
    ///
    /// <para>Scheduling contract: qualifying parent event on campaign day D
    /// schedules each matching authored follow-up for D + delay_days.
    /// Campaign day only — never wall clock. Scheduling is exactly-once per
    /// (parent, follow-up) via the pending set; firing is exactly-once via
    /// the persisted fired-key ledger (mirrors ClaimedReceipts). Loading on
    /// or after the due day fires on the next tick. Same-day order is
    /// deterministic: (dueDay, parentSignalId, followUpId) ordinal sort.</para>
    ///
    /// <para>Core owns the schedule/fire facts and raises
    /// <see cref="OnFollowUpFired"/>; the host applies presentation effects.
    /// No quest mutation, no trust mutation, no audio calls.</para>
    /// </summary>
    public sealed class DistressFollowUpScheduler
    {
        private readonly RadioDistressSystem _definitions;
        private readonly DistressRescueMissionManager? _missions;

        /// <summary>pendingKey → dueDay. Key = parentSignalId:followUpId.</summary>
        private readonly Dictionary<string, int> _pending =
            new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        /// <summary>Exactly-once fired ledger (persisted).</summary>
        private readonly HashSet<string> _fired = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>Raised when a due follow-up transmission fires. Core fact;
        /// the host applies presentation (event line, signal log, audio).</summary>
        public event Action<string, SignalFollowUpDefinition, int>? OnFollowUpFired;

        /// <summary>The most recent fired follow-up (presentation projection —
        /// transient, never persisted; the fired ledger is the authority).</summary>
        public (string ParentSignalId, SignalFollowUpDefinition FollowUp, int Day)? LastFired { get; private set; }

        /// <summary>One pending follow-up row for truthful presentation.</summary>
        public readonly struct PendingFollowUpView
        {
            public string ParentSignalId { get; }
            public string FollowUpId { get; }
            public int DueDay { get; }

            public PendingFollowUpView(string parentSignalId, string followUpId, int dueDay)
            {
                ParentSignalId = parentSignalId;
                FollowUpId = followUpId;
                DueDay = dueDay;
            }
        }

        /// <summary>Deterministically ordered pending follow-ups (dueDay, then
        /// parent id, then follow-up id) for panel/log presentation. Read-only
        /// projection of the authoritative pending set — mutates nothing.</summary>
        public List<PendingFollowUpView> GetPendingView()
        {
            var list = new List<PendingFollowUpView>(_pending.Count);
            foreach (var kvp in _pending)
            {
                int sep = kvp.Key.IndexOf(':');
                if (sep <= 0) continue;
                list.Add(new PendingFollowUpView(kvp.Key.Substring(0, sep), kvp.Key.Substring(sep + 1), kvp.Value));
            }
            list.Sort((a, b) =>
            {
                int c = a.DueDay.CompareTo(b.DueDay);
                if (c != 0) return c;
                c = string.Compare(a.ParentSignalId, b.ParentSignalId, StringComparison.Ordinal);
                if (c != 0) return c;
                return string.Compare(a.FollowUpId, b.FollowUpId, StringComparison.Ordinal);
            });
            return list;
        }

        /// <summary>Current campaign day, set by the host BEFORE the lifecycle
        /// ticks that can raise scheduling events (lifecycle events carry no
        /// day, so the scheduler must know the day to compute due dates).
        /// Transient — never persisted; pending due dates are absolute days.</summary>
        public int CurrentDay { get; private set; }

        /// <summary>Host calls this at the start of its day advance, before
        /// the mission tick, so schedules compute due = day + delay_days.</summary>
        public void SetDay(int day)
        {
            CurrentDay = Math.Max(0, day);
        }

        public DistressFollowUpScheduler(
            RadioDistressSystem definitions,
            DistressRescueMissionManager? missions = null)
        {
            _definitions = definitions ?? throw new ArgumentNullException(nameof(definitions));
            _missions = missions;
        }

        /// <summary>Subscribes the scheduler to the mission manager's
        /// exactly-once lifecycle events. Called by the host after both
        /// objects exist; safe to call once per scheduler lifetime.</summary>
        public void BindToMissionEvents()
        {
            if (_missions == null) return;
            _missions.OnStageChanged += OnMissionStageChanged;
            _missions.OnIgnoreConsequence += OnIgnoreConsequence;
        }

        private void OnMissionStageChanged(DistressRescueMission mission, DistressRescueMissionStage stage)
        {
            switch (stage)
            {
                case DistressRescueMissionStage.Dispatched:
                    ScheduleForTrigger(mission.SignalId, SignalFollowUpTriggers.Answered);
                    break;
                case DistressRescueMissionStage.TerminalRescued:
                    ScheduleForTrigger(mission.SignalId, SignalFollowUpTriggers.RescueSuccess);
                    break;
                case DistressRescueMissionStage.TerminalAmbush:
                    ScheduleForTrigger(mission.SignalId, SignalFollowUpTriggers.AmbushEncountered);
                    break;
                case DistressRescueMissionStage.TerminalFailed:
                    // Distinguish late arrival (rescue_failed) from legacy
                    // deadline expiry (expired) by the arrival guard.
                    ScheduleForTrigger(mission.SignalId,
                        mission.ArrivalResolved ? SignalFollowUpTriggers.RescueFailed : SignalFollowUpTriggers.Expired);
                    break;
            }
        }

        private void OnIgnoreConsequence(DistressRescueMission mission, List<string> tokens, string factionId, int day)
        {
            // Consequence-bearing expiry: the discovered, actionable signal
            // expired unanswered — the same "expired" trigger semantics.
            ScheduleForTrigger(mission.SignalId, SignalFollowUpTriggers.Expired);
        }

        /// <summary>
        /// Schedules every authored follow-up of the parent signal matching
        /// the trigger. Exactly-once: a key already pending or already fired
        /// is skipped. Returns the number of follow-ups newly scheduled.
        /// </summary>
        public int ScheduleForTrigger(string parentSignalId, string trigger)
        {
            if (string.IsNullOrWhiteSpace(parentSignalId) || string.IsNullOrWhiteSpace(trigger)) return 0;
            var def = _definitions.GetDefinition(parentSignalId);
            if (def == null || def.FollowUpSignals.Count == 0) return 0;

            int scheduled = 0;
            foreach (var followUp in def.FollowUpSignals)
            {
                if (followUp == null || string.IsNullOrWhiteSpace(followUp.Id)) continue;
                if (!string.Equals(followUp.TriggerCondition, trigger, StringComparison.OrdinalIgnoreCase)) continue;
                if (followUp.DelayDays < 0) continue; // validator-rejected; never schedule
                string key = Key(parentSignalId, followUp.Id);
                if (_fired.Contains(key) || _pending.ContainsKey(key)) continue;
                _pending[key] = CurrentDay + followUp.DelayDays;
                scheduled++;
            }
            return scheduled;
        }

        /// <summary>
        /// Daily tick: fires every pending follow-up whose due day has
        /// arrived (campaign day, not wall clock). Deterministic same-day
        /// order. Loading on or after the due day fires on the next tick.
        /// </summary>
        public void TickDaily(int currentDay)
        {
            if (_pending.Count == 0) return;

            // Deterministic order: dueDay, then parentSignalId, then followUpId.
            var due = new List<(string Key, string Parent, string FollowUpId, int DueDay)>();
            foreach (var kvp in _pending)
            {
                int sep = kvp.Key.IndexOf(':');
                if (sep <= 0) continue;
                string parent = kvp.Key.Substring(0, sep);
                string followUpId = kvp.Key.Substring(sep + 1);
                if (currentDay >= kvp.Value)
                    due.Add((kvp.Key, parent, followUpId, kvp.Value));
            }
            due.Sort((a, b) =>
            {
                int c = a.DueDay.CompareTo(b.DueDay);
                if (c != 0) return c;
                c = string.Compare(a.Parent, b.Parent, StringComparison.Ordinal);
                if (c != 0) return c;
                return string.Compare(a.FollowUpId, b.FollowUpId, StringComparison.Ordinal);
            });

            foreach (var (key, parent, followUpId, _) in due)
            {
                _pending.Remove(key);
                if (!_fired.Add(key)) continue; // exactly-once fire
                var def = _definitions.GetDefinition(parent);
                SignalFollowUpDefinition? followUp = null;
                if (def != null)
                    foreach (var f in def.FollowUpSignals)
                        if (f != null && string.Equals(f.Id, followUpId, StringComparison.OrdinalIgnoreCase))
                        { followUp = f; break; }
                if (followUp == null) continue; // definition removed — pending entry expires silently
                LastFired = (parent, followUp, currentDay);
                OnFollowUpFired?.Invoke(parent, followUp, currentDay);
            }
        }

        public IReadOnlyCollection<string> PendingKeys => _pending.Keys;
        public IReadOnlyCollection<string> FiredKeys => _fired;

        public static string Key(string parentSignalId, string followUpId)
            => $"{parentSignalId}:{followUpId}";

        public SignalFollowUpSaveState CaptureState()
        {
            var state = new SignalFollowUpSaveState();
            foreach (var kvp in _pending)
            {
                int sep = kvp.Key.IndexOf(':');
                if (sep <= 0) continue;
                state.pending.Add(new PendingSignalFollowUpEntry
                {
                    parentSignalId = kvp.Key.Substring(0, sep),
                    followUpId = kvp.Key.Substring(sep + 1),
                    dueDay = kvp.Value
                });
            }
            state.pending.Sort((a, b) =>
            {
                int c = a.dueDay.CompareTo(b.dueDay);
                if (c != 0) return c;
                c = string.Compare(a.parentSignalId, b.parentSignalId, StringComparison.Ordinal);
                if (c != 0) return c;
                return string.Compare(a.followUpId, b.followUpId, StringComparison.Ordinal);
            });
            state.firedKeys = new List<string>(_fired);
            state.firedKeys.Sort(StringComparer.Ordinal);
            return state;
        }

        public void RestoreState(SignalFollowUpSaveState? state)
        {
            if (state == null) return; // old save / absent section → empty default
            _pending.Clear();
            _fired.Clear();
            if (state.pending != null)
            {
                foreach (var entry in state.pending)
                {
                    if (entry == null || string.IsNullOrWhiteSpace(entry.parentSignalId)
                        || string.IsNullOrWhiteSpace(entry.followUpId)) continue;
                    string key = Key(entry.parentSignalId, entry.followUpId);
                    _pending[key] = Math.Max(0, entry.dueDay);
                }
            }
            if (state.firedKeys != null)
                foreach (var key in state.firedKeys)
                    if (!string.IsNullOrWhiteSpace(key)) _fired.Add(key);
        }
    }
}
