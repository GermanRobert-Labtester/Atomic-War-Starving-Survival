using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class PhysicalTherapyState
    {
        public string action_id = "action_physical_therapy";
        public string survivor_id = "";
        public string limb_type = "";
        public int days_completed = 0;
        public int days_required = 7;
        public float efficiency_percent = 0f;
        public bool is_therapy_active = false;
    }

    /// <summary>
    /// Prompt #835: Physical Therapy.
    /// A newly equipped BionicLimb starts at 0 % efficiency. Seven days of
    /// daily therapy sessions (walking, gripping) calibrate the neural links
    /// to 100 %. Skipping a day loses one day of progress. The survivor
    /// cannot perform heavy labour during therapy days.
    /// Plain C# class, not a MonoBehaviour.
    /// </summary>
    public class Action_PhysicalTherapy
    {
        // ── Constants ──────────────────────────────────────────────────
        public const int DAYS_REQUIRED = 7;
        private const float EFFICIENCY_PER_DAY = 100f / DAYS_REQUIRED; // ~14.3 %

        // ── Events ─────────────────────────────────────────────────────
        public event Action<string, string> OnTherapyStarted;         // survivorId, limbType
        public event Action<string, int, float> OnDayCompleted;       // survivorId, day, efficiency
        public event Action<string> OnTherapyComplete;                // survivorId
        public event Action<string> OnSessionSkipped;                 // survivorId

        // ── State ──────────────────────────────────────────────────────
        private string _survivorId;
        private string _limbType = "";
        private int _daysCompleted;
        private float _efficiencyPercent;
        private bool _isTherapyActive;

        // ── Public API ─────────────────────────────────────────────────

        /// <summary>
        /// Begin physical therapy for a survivor's new bionic limb.
        /// Resets progress to 0.
        /// </summary>
        public void StartTherapy(string survivorId, string limbType)
        {
            if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(limbType)) return;

            _survivorId = survivorId;
            _limbType = limbType;
            _daysCompleted = 0;
            _efficiencyPercent = 0f;
            _isTherapyActive = true;

            OnTherapyStarted?.Invoke(survivorId, limbType);
        }

        /// <summary>
        /// Call once per in-game day to record a completed therapy session.
        /// Adds ~14.3 % efficiency per day.
        /// </summary>
        public void TickDay()
        {
            if (!_isTherapyActive) return;

            _daysCompleted++;
            _efficiencyPercent = Mathf.Clamp(_daysCompleted * EFFICIENCY_PER_DAY, 0f, 100f);

            OnDayCompleted?.Invoke(_survivorId, _daysCompleted, _efficiencyPercent);

            if (_daysCompleted >= DAYS_REQUIRED)
            {
                _isTherapyActive = false;
                _efficiencyPercent = 100f;
                OnTherapyComplete?.Invoke(_survivorId);
            }
        }

        /// <summary>
        /// Skip a therapy session — lose one day of progress (min 0).
        /// </summary>
        public void SkipSession()
        {
            if (!_isTherapyActive) return;

            _daysCompleted = Mathf.Max(0, _daysCompleted - 1);
            _efficiencyPercent = Mathf.Clamp(_daysCompleted * EFFICIENCY_PER_DAY, 0f, 100f);

            OnSessionSkipped?.Invoke(_survivorId);
        }

        /// <summary>Returns the current limb efficiency (0-100 %).</summary>
        public float GetEfficiency()
        {
            return _efficiencyPercent;
        }

        /// <summary>Returns true if all 7 days are completed.</summary>
        public bool IsComplete()
        {
            return !_isTherapyActive && _daysCompleted >= DAYS_REQUIRED;
        }

        /// <summary>Returns true if therapy is currently in progress.</summary>
        public bool IsActive()
        {
            return _isTherapyActive;
        }

        // ── Save / Load ────────────────────────────────────────────────

        public PhysicalTherapyState CaptureState()
        {
            return new PhysicalTherapyState
            {
                action_id = "action_physical_therapy",
                survivor_id = _survivorId ?? "",
                limb_type = _limbType,
                days_completed = _daysCompleted,
                days_required = DAYS_REQUIRED,
                efficiency_percent = _efficiencyPercent,
                is_therapy_active = _isTherapyActive
            };
        }

        public void RestoreState(PhysicalTherapyState saved)
        {
            if (saved == null) return;
            _survivorId = saved.survivor_id;
            _limbType = saved.limb_type;
            _daysCompleted = saved.days_completed;
            _efficiencyPercent = saved.efficiency_percent;
            _isTherapyActive = saved.is_therapy_active;
        }
    }
}
