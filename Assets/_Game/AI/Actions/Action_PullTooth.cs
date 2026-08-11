using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.AI.Actions
{
    [Serializable]
    public class PullToothState
    {
        public string action_id = "action_pull_tooth";
        public string survivor_id = "";
        public bool has_pliers = false;
        public bool has_whiskey = false;
        public bool tooth_pulled = false;
        public float charisma_penalty = -5f;
        public float trauma_spike = 40f;
    }

    /// <summary>
    /// Prompt #836: Dental Surgery — Pull Tooth.
    /// Cures ToothDecay. Requires Pliers and Whiskey. The patient gets a
    /// permanent -5 Charisma penalty (missing teeth) and a massive temporary
    /// +40 Trauma spike. Patient screams, waking others if it is night.
    /// Plain C# class, not a MonoBehaviour.
    /// </summary>
    /// <summary>DEMOTE-Action-remaining — dormant ghost; not Boot/Save wired until a host calls APIs.</summary>
    public class Action_PullTooth
    {
        // ── Constants ──────────────────────────────────────────────────
        public const float CHARISMA_PENALTY = -5f;
        public const float TRAUMA_SPIKE = 40f;

        // ── Events ─────────────────────────────────────────────────────
        public event Action<string> OnExtractionStarted;     // survivorId
        public event Action<string> OnToothPulled;           // survivorId
        public event Action<string, float> OnCharismaReduced; // survivorId, amount
        public event Action<string, float> OnTraumaSpiked;   // survivorId, amount
        public event Action<string> OnPainStopped;           // survivorId

        // ── State ──────────────────────────────────────────────────────
        private string _survivorId;
        private bool _hasPliers;
        private bool _hasWhiskey;
        private bool _toothPulled;

        // ── Public API ─────────────────────────────────────────────────

        /// <summary>
        /// Returns true if both required items are present.
        /// </summary>
        public bool CanPerform(bool pliers, bool whiskey)
        {
            return pliers && whiskey;
        }

        /// <summary>
        /// Perform the extraction. Fires all events in sequence.
        /// Returns true if the extraction succeeded.
        /// </summary>
        public bool PerformExtraction(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return false;
            if (!_hasPliers || !_hasWhiskey)
            {
                Debug.LogWarning("[Action_PullTooth] Missing pliers or whiskey.");
                return false;
            }
            if (_toothPulled)
            {
                Debug.LogWarning("[Action_PullTooth] Tooth already pulled.");
                return false;
            }

            _survivorId = survivorId;
            OnExtractionStarted?.Invoke(survivorId);

            _toothPulled = true;
            OnToothPulled?.Invoke(survivorId);
            OnCharismaReduced?.Invoke(survivorId, CHARISMA_PENALTY);
            OnTraumaSpiked?.Invoke(survivorId, TRAUMA_SPIKE);
            OnPainStopped?.Invoke(survivorId);

            return true;
        }

        /// <summary>Supply the required items before performing extraction.</summary>
        public void SetSupplies(bool pliers, bool whiskey)
        {
            _hasPliers = pliers;
            _hasWhiskey = whiskey;
        }

        /// <summary>Returns the permanent Charisma penalty.</summary>
        public float GetCharismaPenalty()
        {
            return CHARISMA_PENALTY;
        }

        /// <summary>Returns the temporary Trauma spike value.</summary>
        public float GetTraumaSpike()
        {
            return TRAUMA_SPIKE;
        }

        /// <summary>Returns true if the tooth has been pulled (ToothDecay cured).</summary>
        public bool IsCured()
        {
            return _toothPulled;
        }

        // ── Save / Load ────────────────────────────────────────────────

        public PullToothState CaptureState()
        {
            return new PullToothState
            {
                action_id = "action_pull_tooth",
                survivor_id = _survivorId ?? "",
                has_pliers = _hasPliers,
                has_whiskey = _hasWhiskey,
                tooth_pulled = _toothPulled,
                charisma_penalty = CHARISMA_PENALTY,
                trauma_spike = TRAUMA_SPIKE
            };
        }

        public void RestoreState(PullToothState saved)
        {
            if (saved == null) return;
            _survivorId = saved.survivor_id;
            _hasPliers = saved.has_pliers;
            _hasWhiskey = saved.has_whiskey;
            _toothPulled = saved.tooth_pulled;
        }
    }
}
