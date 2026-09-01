using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Events;
using Ashfall.Core.Journal;

namespace AtomicWar.GodotApp.Host
{
    /// <summary>
    /// Dynamic state owned by the host event adapter. The catalog/read-model session does
    /// not mirror this ledger; campaign persistence snapshots it through HostEventSaveStore.
    /// </summary>
    [Serializable]
    public class HostEventState
    {
        public List<string> triggeredEventIds = new List<string>();
        public Dictionary<string, int> eventTriggerDays = new Dictionary<string, int>();
        public string lastDispatchedEvent = string.Empty;
    }

    /// <summary>
    /// Host event adapter bridging Core's IEventBus and authored event triggers
    /// (year_of_ash_events.json) to Godot HUD notifications, Journal system entries,
    /// and save state tracking.
    /// </summary>
    public class HostEventAdapter
    {
        public const string EventThinMarginDisclosure = "event_the_thin_margin_disclosure";
        public const string EventThirstySeason = "event_the_thirsty_season";
        public const string EventOsteophageExplanation = "event_osteophage_explanation";
        public const string EventMeasurementBroadcast = "event_measurement_broadcast";

        private readonly IEventBus _eventBus;
        private readonly JournalSystem? _journal;
        private readonly HostEventState _state;
        private bool _disposed;

        public event Action<string, string>? OnEventDispatched;
        public event Action? StateChanged;

        public HostEventAdapter(IEventBus eventBus, JournalSystem? journal = null, HostEventState? state = null)
        {
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _journal = journal;
            // Always retain an adapter-owned state object. A caller-supplied load result is
            // copied below so mutable progress cannot be shared with the persistence layer.
            _state = new HostEventState();
            if (state != null)
            {
                RestoreState(state);
            }

            SubscribeBus();
        }

        public HostEventState State => _state;
        public IReadOnlyList<string> TriggeredEventIds => _state.triggeredEventIds;
        public string LastDispatchedEvent => _state.lastDispatchedEvent;

        private void SubscribeBus()
        {
            _eventBus.Subscribe(EventThinMarginDisclosure, HandleThinMarginDisclosure);
            _eventBus.Subscribe(EventThirstySeason, HandleThirstySeason);
            _eventBus.Subscribe(EventOsteophageExplanation, HandleOsteophageExplanation);
            _eventBus.Subscribe(EventMeasurementBroadcast, HandleMeasurementBroadcast);
        }

        public bool HasTriggered(string eventId) => _state.triggeredEventIds.Contains(eventId);

        public int GetTriggerDay(string eventId)
        {
            return _state.eventTriggerDays.TryGetValue(eventId, out int day) ? day : -1;
        }

        public void TriggerEvent(string eventId, int currentDay)
        {
            if (_disposed) return;
            if (string.IsNullOrEmpty(eventId)) return;
            if (!_state.triggeredEventIds.Contains(eventId))
            {
                _state.triggeredEventIds.Add(eventId);
                _state.eventTriggerDays[eventId] = currentDay;
            }

            _eventBus.Publish(eventId, currentDay);
        }

        /// <summary>
        /// Evaluates simulation state triggers for the four authored events.
        /// </summary>
        public void EvaluateTriggers(
            int day,
            bool hydroAuditDone,
            bool hydroSeized,
            bool osteophageInquiry,
            bool coldCountBroadcast)
        {
            // Event 1: The Thin Margin, Disclosed (Hydro-Barons Approach B audit result)
            if (hydroAuditDone && !HasTriggered(EventThinMarginDisclosure))
            {
                TriggerEvent(EventThinMarginDisclosure, day);
            }

            // Event 2: The Thirsty Season (Hydro-Barons Approach C aftermath)
            if (hydroSeized && !HasTriggered(EventThirstySeason))
            {
                TriggerEvent(EventThirstySeason, day);
            }

            // Event 3: Osteophage Explanation
            if (osteophageInquiry && !HasTriggered(EventOsteophageExplanation))
            {
                TriggerEvent(EventOsteophageExplanation, day);
            }

            // Event 4: Cold Count 142.850 MHz broadcast transmission
            if (coldCountBroadcast && !HasTriggered(EventMeasurementBroadcast))
            {
                TriggerEvent(EventMeasurementBroadcast, day);
            }

            // Gated fallback triggers per catalog day definitions
            if (day >= 210 && hydroAuditDone && !HasTriggered(EventThinMarginDisclosure))
                TriggerEvent(EventThinMarginDisclosure, day);
            if (day >= 225 && hydroSeized && !HasTriggered(EventThirstySeason))
                TriggerEvent(EventThirstySeason, day);
            if (day >= 205 && osteophageInquiry && !HasTriggered(EventOsteophageExplanation))
                TriggerEvent(EventOsteophageExplanation, day);
            if (day >= 250 && coldCountBroadcast && !HasTriggered(EventMeasurementBroadcast))
                TriggerEvent(EventMeasurementBroadcast, day);
        }

        private void HandleThinMarginDisclosure(object? payload)
        {
            string desc = "Audited quota water tests clean; desalination safety margins quietly narrowed for 11 months. Public disclosure forces immediate reform.";
            _state.lastDispatchedEvent = EventThinMarginDisclosure;
            int day = _state.eventTriggerDays.GetValueOrDefault(EventThinMarginDisclosure, 210);
            _journal?.UnlockEventFired(EventThinMarginDisclosure);
            _journal?.TryAddRawEntry(EventThinMarginDisclosure, desc, null!, day);
            OnEventDispatched?.Invoke(EventThinMarginDisclosure, desc);
            StateChanged?.Invoke();
        }

        private void HandleThirstySeason(object? payload)
        {
            string desc = "Desalination Unit 4 seizure destroyed the queue system; sector-wide 14-day water shortfall in effect.";
            _state.lastDispatchedEvent = EventThirstySeason;
            int day = _state.eventTriggerDays.GetValueOrDefault(EventThirstySeason, 225);
            _journal?.UnlockEventFired(EventThirstySeason);
            _journal?.TryAddRawEntry(EventThirstySeason, desc, null!, day);
            OnEventDispatched?.Invoke(EventThirstySeason, desc);
            StateChanged?.Invoke();
        }

        private void HandleOsteophageExplanation(object? payload)
        {
            string desc = "Winter mortality rises. The Osteophages break their silence to explain why the collection work matters.";
            _state.lastDispatchedEvent = EventOsteophageExplanation;
            int day = _state.eventTriggerDays.GetValueOrDefault(EventOsteophageExplanation, 205);
            _journal?.UnlockEventFired(EventOsteophageExplanation);
            _journal?.TryAddRawEntry(EventOsteophageExplanation, desc, null!, day);
            OnEventDispatched?.Invoke(EventOsteophageExplanation, desc);
            StateChanged?.Invoke();
        }

        private void HandleMeasurementBroadcast(object? payload)
        {
            string desc = "The Cold Count transmits isotopic provenance readings on 142.850 MHz.";
            _state.lastDispatchedEvent = EventMeasurementBroadcast;
            int day = _state.eventTriggerDays.GetValueOrDefault(EventMeasurementBroadcast, 250);
            _journal?.UnlockEventFired(EventMeasurementBroadcast);
            _journal?.TryAddRawEntry(EventMeasurementBroadcast, desc, null!, day);
            OnEventDispatched?.Invoke(EventMeasurementBroadcast, desc);
            StateChanged?.Invoke();
        }

        /// <summary>
        /// Detaches this campaign's handlers from the shared event bus. A host
        /// adapter is campaign-scoped; retaining its subscriptions across a
        /// slot reset would dispatch old progress into the new campaign.
        /// </summary>
        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            _eventBus.Unsubscribe(EventThinMarginDisclosure, HandleThinMarginDisclosure);
            _eventBus.Unsubscribe(EventThirstySeason, HandleThirstySeason);
            _eventBus.Unsubscribe(EventOsteophageExplanation, HandleOsteophageExplanation);
            _eventBus.Unsubscribe(EventMeasurementBroadcast, HandleMeasurementBroadcast);
            OnEventDispatched = null;
            StateChanged = null;
        }

        // ── Persistence ───────────────────────────────────────────────

        public HostEventState CaptureState()
        {
            var copy = new HostEventState
            {
                lastDispatchedEvent = _state.lastDispatchedEvent,
                triggeredEventIds = new List<string>(_state.triggeredEventIds),
                eventTriggerDays = new Dictionary<string, int>(_state.eventTriggerDays)
            };
            return copy;
        }

        public void RestoreState(HostEventState? state)
        {
            if (state == null) return;
            _state.triggeredEventIds.Clear();
            if (state.triggeredEventIds != null)
            {
                _state.triggeredEventIds.AddRange(state.triggeredEventIds);
            }

            _state.eventTriggerDays.Clear();
            if (state.eventTriggerDays != null)
            {
                foreach (var kvp in state.eventTriggerDays)
                {
                    _state.eventTriggerDays[kvp.Key] = kvp.Value;
                }
            }

            _state.lastDispatchedEvent = state.lastDispatchedEvent ?? string.Empty;
        }
    }
}
