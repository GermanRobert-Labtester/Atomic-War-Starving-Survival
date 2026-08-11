using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Survivors
{
    [Serializable]
    public class PTSDState
    {
        public string trait_id = "trait_ptsd";
        public string survivor_id = "";
        public string trigger_type = "";
        public bool panic_active = false;
        public float panic_duration_hours = 2f;
        public float panic_hours_remaining = 0f;
        public bool hidden_under_bed = false;
    }

    /// <summary>
    /// Prompt #832: PTSD Triggers.
    /// A survivor associates a specific event (thunder, gunfire, explosion,
    /// screaming) with trauma. When that event occurs the survivor suffers an
    /// instant panic attack — drops everything, hides under the nearest bed,
    /// and cannot act for 2 hours.
    /// Plain C# class, not a MonoBehaviour.
    /// </summary>
    public class Trait_PTSD
    {
        // ── Constants ──────────────────────────────────────────────────
        private const float PANIC_DURATION_HOURS = 2f;

        // ── Events ─────────────────────────────────────────────────────
        public event Action<string, string> OnTriggerAssigned;   // survivorId, triggerType
        public event Action<string> OnPanicStarted;              // survivorId
        public event Action<string, string> OnItemDropped;       // survivorId, itemId
        public event Action<string> OnHidingUnderBed;            // survivorId
        public event Action<string> OnPanicEnded;                // survivorId

        // ── State ──────────────────────────────────────────────────────
        private string _survivorId;
        private string _triggerType = "";
        private bool _panicActive;
        private float _panicHoursRemaining;
        private bool _hiddenUnderBed;

        // ── Public API ─────────────────────────────────────────────────

        /// <summary>
        /// Assign a trauma trigger to a survivor.
        /// Valid types: "thunder", "gunfire", "explosion", "screaming".
        /// </summary>
        public void AssignTrigger(string survivorId, string triggerType)
        {
            if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(triggerType)) return;
            _survivorId = survivorId;
            _triggerType = triggerType;
            OnTriggerAssigned?.Invoke(survivorId, triggerType);
        }

        /// <summary>
        /// Check whether a world event matches this survivor's trigger.
        /// If it does, a panic attack starts immediately.
        /// Returns true if triggered.
        /// </summary>
        public bool CheckTrigger(string currentEvent)
        {
            if (string.IsNullOrEmpty(currentEvent)) return false;
            if (_panicActive) return false;
            if (!string.Equals(currentEvent, _triggerType, StringComparison.OrdinalIgnoreCase))
                return false;

            StartPanic();
            return true;
        }

        /// <summary>
        /// Begin the panic attack sequence — item drop, hide, 2-hour lockout.
        /// Caller is responsible for invoking OnItemDropped with the held item.
        /// </summary>
        public void StartPanic()
        {
            if (string.IsNullOrEmpty(_survivorId)) return;

            _panicActive = true;
            _panicHoursRemaining = PANIC_DURATION_HOURS;
            _hiddenUnderBed = true;

            OnPanicStarted?.Invoke(_survivorId);
            OnHidingUnderBed?.Invoke(_survivorId);
        }

        /// <summary>
        /// Drop a held item during panic. Fires OnItemDropped.
        /// </summary>
        public void DropItem(string itemId)
        {
            if (string.IsNullOrEmpty(itemId) || !_panicActive) return;
            OnItemDropped?.Invoke(_survivorId, itemId);
        }

        /// <summary>Call once per in-game hour to tick down the panic timer.</summary>
        public void TickHour()
        {
            if (!_panicActive) return;

            _panicHoursRemaining -= 1f;
            if (_panicHoursRemaining <= 0f)
            {
                EndPanic();
            }
        }

        /// <summary>Returns true if the survivor is currently panicking.</summary>
        public bool IsPanicking()
        {
            return _panicActive;
        }

        /// <summary>End the panic attack early or when the timer expires.</summary>
        public void EndPanic()
        {
            if (!_panicActive) return;

            _panicActive = false;
            _panicHoursRemaining = 0f;
            _hiddenUnderBed = false;

            OnPanicEnded?.Invoke(_survivorId);
        }

        /// <summary>Returns the assigned trigger type, or empty string.</summary>
        public string GetTriggerType()
        {
            return _triggerType;
        }

        // ── Save / Load ────────────────────────────────────────────────

        public PTSDState CaptureState()
        {
            return new PTSDState
            {
                trait_id = "trait_ptsd",
                survivor_id = _survivorId ?? "",
                trigger_type = _triggerType,
                panic_active = _panicActive,
                panic_duration_hours = PANIC_DURATION_HOURS,
                panic_hours_remaining = _panicHoursRemaining,
                hidden_under_bed = _hiddenUnderBed
            };
        }

        public void RestoreState(PTSDState saved)
        {
            if (saved == null) return;
            _survivorId = saved.survivor_id;
            _triggerType = saved.trigger_type;
            _panicActive = saved.panic_active;
            _panicHoursRemaining = saved.panic_hours_remaining;
            _hiddenUnderBed = saved.hidden_under_bed;
        }
    }
}
