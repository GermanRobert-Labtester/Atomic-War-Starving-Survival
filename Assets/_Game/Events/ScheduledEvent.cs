using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Events
{
    /// <summary>
    /// A day-keyed deferred narrative event. The EventRunner holds a queue of these
    /// and fires the matching GameEvent on the exact campaign day. (Prompt #43)
    /// </summary>
    [Serializable]
    public struct ScheduledEvent
    {
        /// <summary>snake_case id matching a GameEvent in the pool or authored chain.</summary>
        public string EventId;
        /// <summary>Campaign day on which this event should fire.</summary>
        public int ExecuteOnDay;
        /// <summary>Optional world flag set when this event was scheduled (for branching).</summary>
        public string OriginFlag;

        public ScheduledEvent(string eventId, int executeOnDay, string originFlag = null)
        {
            EventId = eventId;
            ExecuteOnDay = executeOnDay;
            OriginFlag = originFlag ?? string.Empty;
        }
    }

    /// <summary>
    /// Serialisable snapshot of the scheduled-event queue for SaveSystem round-trips.
    /// </summary>
    [Serializable]
    public class ScheduledEventSave
    {
        public ScheduledEvent[] Queue;
    }
}
