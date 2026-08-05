using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Shelter;

namespace AtomicWar._Game.Events
{
    [Serializable]
    public class ActiveDelayedConsequence
    {
        public string EventId;
        public string ChoiceId;
        public float RemainingHours;
        public DelayedConsequence Consequence;
    }

    /// <summary>
    /// Data-driven engine that evaluates, triggers, and resolves GameEvents based on
    /// weighted conditions and presents choices with immediate and delayed consequences.
    /// Save/load safe.
    /// </summary>
    public partial class EventRunner
    {
        private readonly List<GameEvent> _pool = new List<GameEvent>();
        private readonly Dictionary<string, float> _cooldowns = new Dictionary<string, float>();
        private readonly List<ActiveDelayedConsequence> _activeConsequences = new List<ActiveDelayedConsequence>();

        // ── Scheduled-event queue (Prompt #43 — delayed narrative chains) ──
        private readonly List<ScheduledEvent> _scheduledEvents = new List<ScheduledEvent>();

        // Hot-path scratch buffers — avoid per-tick / per-SelectEvent GC.
        private readonly List<GameEvent> _selectValidBuffer = new List<GameEvent>(32);
        private readonly List<string> _cooldownKeyBuffer = new List<string>(16);

        public IReadOnlyList<GameEvent> Pool => _pool;
        public IReadOnlyList<ActiveDelayedConsequence> ActiveConsequences => _activeConsequences;
        /// <summary>Read-only view of the pending deferred narrative events.</summary>
        public IReadOnlyList<ScheduledEvent> ScheduledEvents => _scheduledEvents;

        public event Action<GameEvent, EventContext> OnEventTriggered;
        public event Action<GameEvent, EventChoice, EventContext> OnChoiceApplied;
        public event Action<ActiveDelayedConsequence, EventContext> OnDelayedConsequenceResolved;
        /// <summary>Fired when a scheduled narrative event is dequeued and triggered on its day.</summary>
        public event Action<ScheduledEvent, GameEvent, EventContext> OnScheduledEventFired;

        public float DefaultCooldownHours = 24f;

        public void SetPool(IReadOnlyList<GameEvent> pool)
        {
            _pool.Clear();
            if (pool != null)
            {
                _pool.AddRange(pool);
            }
        }



        /// <summary>
        /// Choices actually offered: drops trait/trust/flag gates and BeliefCheck.HideIfFails.
        /// Callers presenting choices should use this (or <see cref="GetPresentedChoices"/>)
        /// instead of iterating gameEvent.choices directly.
        /// </summary>

        /// <summary>
        /// Full presentation list: available, grayed-out (gate fail + HideIfGatesFail=false),
        /// or omitted when hidden. Powers branching event UI.
        /// </summary>

        /// <summary>Visible rows only (available + grayed), never hidden.</summary>

        /// <summary>
        /// Find a choice by id among available (non-hidden, gate-passed) options.
        /// </summary>

        /// <summary>
        /// Belief-weighted auto-selection among a game event's choices, for callers that
        /// pick on a survivor's behalf (e.g. AI-controlled companions) rather than
        /// presenting a player with a menu. Choices whose BeliefCheck passes get their
        /// weight scaled by BeliefCheck.WeightMultiplier — this is how a Paranoid
        /// survivor "demands iodine, just in case." Not used for player-facing choice UI.
        /// </summary>

        public void Run(GameEvent gameEvent, EventContext context = null)
        {
            if (gameEvent == null) return;
            _cooldowns[gameEvent.id] = DefaultCooldownHours;
            gameEvent.Apply();
            OnEventTriggered?.Invoke(gameEvent, context);
        }

        // ── Scheduled narrative event queue (Prompt #43) ──────────────────

        /// <summary>
        /// Enqueue a GameEvent by id to fire on a specific campaign day.
        /// Duplicate schedules for the same eventId+day are silently ignored.
        /// </summary>
        public void ScheduleEvent(string eventId, int executeOnDay, string originFlag = null)
        {
            if (string.IsNullOrEmpty(eventId) || executeOnDay <= 0) return;
            // Prevent duplicate scheduling of the same event on the same day.
            for (int i = 0; i < _scheduledEvents.Count; i++)
            {
                if (_scheduledEvents[i].EventId == eventId && _scheduledEvents[i].ExecuteOnDay == executeOnDay)
                    return;
            }
            _scheduledEvents.Add(new ScheduledEvent(eventId, executeOnDay, originFlag));
        }

        /// <summary>
        /// Called once per campaign day. Dequeues and fires all ScheduledEvents
        /// whose ExecuteOnDay == currentDay. Events are looked up by id in the pool.
        /// If the event's <see cref="GameEvent.CanTrigger"/> fails (missing eventFlags,
        /// day/hour gates, etc.), it is dequeued without presenting.
        /// </summary>
        public void TickDay(int currentDay, EventContext context = null)
        {
            if (_scheduledEvents.Count == 0) return;

            // Keep the context's day consistent with the day being ticked.
            // The dequeue below matches on the currentDay argument, but the
            // CanTrigger gate reads context.CurrentDay (which defaults to 1).
            // When the two disagreed, an event scheduled for its own minDay
            // failed the "CurrentDay < MinDay" check and was dropped -- dequeued
            // without ever presenting, with only a Debug.Log to show for it, so
            // a stage of a narrative arc could vanish mid-chain. currentDay is
            // the authoritative value at this call site.
            if (context != null) context.CurrentDay = currentDay;

            for (int i = _scheduledEvents.Count - 1; i >= 0; i--)
            {
                var scheduled = _scheduledEvents[i];
                if (scheduled.ExecuteOnDay != currentDay) continue;

                _scheduledEvents.RemoveAt(i);

                // Propagate the origin flag into context before triggering.
                if (context != null && !string.IsNullOrEmpty(scheduled.OriginFlag))
                    context.SetFlag(scheduled.OriginFlag, true);

                // Look up the GameEvent in the pool.
                GameEvent gameEvent = FindInPool(scheduled.EventId);

                // Flag / condition gates on multi-stage arcs (eventFlags, minDay, …).
                if (gameEvent != null && context != null && !gameEvent.CanTrigger(context))
                {
                    UnityEngine.Debug.Log(
                        $"[EventRunner] Scheduled event '{scheduled.EventId}' on day {currentDay} " +
                        "skipped — CanTrigger failed (eventFlags / conditions).");
                    OnScheduledEventFired?.Invoke(scheduled, null, context);
                    continue;
                }

                // Even if not in pool, raise the signal so tests / bootstrap can hear it.
                OnScheduledEventFired?.Invoke(scheduled, gameEvent, context);

                if (gameEvent != null)
                {
                    Run(gameEvent, context);
                }
                else
                {
                    // Raise a bare EventBus signal so systems can react even without a pool entry.
                    // (e.g., bootstrap injects an ad-hoc GameEvent for the chain part.)
                    UnityEngine.Debug.LogWarning(
                        $"[EventRunner] Scheduled event '{scheduled.EventId}' fired on day {currentDay} " +
                        "but was not found in the pool. Wiring OnScheduledEventFired only.");
                }
            }
        }

        /// <summary>
        /// Find a GameEvent by id in the current pool. Returns the FIRST
        /// match (linear scan from index 0). If two events share the same
        /// id, only the first is reachable — the rest are silently
        /// shadowed. Run <c>Tools/ASHFALL/Validate Event Ids</c> to
        /// detect this condition at design time.
        /// </summary>
        public GameEvent FindInPool(string eventId)
        {
            if (string.IsNullOrEmpty(eventId)) return null;
            for (int i = 0; i < _pool.Count; i++)
            {
                if (_pool[i] != null && _pool[i].id == eventId)
                    return _pool[i];
            }
            return null;
        }

        // ── Save / restore for the scheduled-event queue ──────────────────

        public ScheduledEventSave CaptureScheduledState()
        {
            return new ScheduledEventSave { Queue = _scheduledEvents.ToArray() };
        }

        public void RestoreScheduledState(ScheduledEventSave save)
        {
            _scheduledEvents.Clear();
            if (save?.Queue == null) return;
            for (int i = 0; i < save.Queue.Length; i++)
            {
                if (!string.IsNullOrEmpty(save.Queue[i].EventId))
                    _scheduledEvents.Add(save.Queue[i]);
            }
        }

        private struct BloodForWaterBleedSpec
        {
            public string ChoiceId;
            public string Text;
            public float MoraleDelta;
            public string FactionId;
            public float TrustDelta;
            public string RequiredTrait;
        }

        private static EventChoice MakeBloodForWaterBleedChoice(in BloodForWaterBleedSpec spec)
        {
            return new EventChoice
            {
                ChoiceId = spec.ChoiceId,
                Text = spec.Text,
                MoraleDelta = spec.MoraleDelta,
                FactionId = spec.FactionId,
                TrustDelta = spec.TrustDelta,
                RequiredTrait = spec.RequiredTrait,
                HideIfGatesFail = true,
                SetEventFlags = new List<string> { FlagBloodDrawn },
                Effects = new List<EventEffect>
                {
                    new EventEffect { ItemId = "clean_water", ItemAmount = BloodForWaterCleanWaterReward },
                    new EventEffect { ItemId = "iodine_pills", ItemAmount = BloodForWaterIodinePillsReward }
                }
            };
        }


        // -----------------------------------------------------------------
        // Factories — trait/trust-gated branching events
        // -----------------------------------------------------------------

    }
}
