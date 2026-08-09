using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class TBIState
    {
        public string affliction_id = "affliction_tbi";
        public string survivor_id = "";
        public float severity = 0f;
        public int head_trauma_count = 0;
        public float item_drop_chance = 0f;
        public bool speech_slur = false;
    }

    /// <summary>
    /// Prompt #837: Traumatic Brain Injury (TBI).
    /// Caused by repeated head trauma (3+ hits). Speech slurs in Journal/UI,
    /// items are randomly dropped during hauling. Irreversible.
    /// Severity: 0.3 = slight slur, 0.6 = moderate drops, 1.0 = severe.
    /// Plain C# class, not a MonoBehaviour.
    /// </summary>
    public class Affliction_TBI
    {
        // ── Constants ──────────────────────────────────────────────────
        private const int TRAUMA_THRESHOLD = 3;
        private const float SEVERITY_PER_HIT = 0.1f;
        private const float MAX_SEVERITY = 1f;
        private const float ITEM_DROP_MULTIPLIER = 0.3f;

        // ── Events ─────────────────────────────────────────────────────
        public event Action<string, int> OnHeadTraumaAdded;        // survivorId, count
        public event Action<string, float> OnSeverityIncreased;    // survivorId, severity
        public event Action<string, string> OnItemDropped;         // survivorId, itemId
        public event Action<string> OnSpeechSlurred;               // survivorId

        // ── State ──────────────────────────────────────────────────────
        private string _survivorId;
        private float _severity;
        private int _headTraumaCount;
        private bool _speechSlur;

        private readonly System.Random _rng = AtomicWar._Game.Utilities.SeededRandom.CreateFixed("affliction_tbi");

        // ── Public API ─────────────────────────────────────────────────

        /// <summary>
        /// Record a head trauma event. After 3+ hits, TBI manifests and
        /// severity increases with each subsequent hit.
        /// </summary>
        public void AddHeadTrauma(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return;

            _survivorId = survivorId;
            _headTraumaCount++;

            OnHeadTraumaAdded?.Invoke(survivorId, _headTraumaCount);

            if (_headTraumaCount >= TRAUMA_THRESHOLD)
            {
                float oldSeverity = _severity;
                _severity = Mathf.Min(MAX_SEVERITY, (_headTraumaCount - TRAUMA_THRESHOLD + 1) * SEVERITY_PER_HIT * 3f);

                if (_severity > oldSeverity)
                    OnSeverityIncreased?.Invoke(survivorId, _severity);

                if (_severity >= 0.3f && !_speechSlur)
                {
                    _speechSlur = true;
                    OnSpeechSlurred?.Invoke(survivorId);
                }
            }
        }

        /// <summary>Returns the current severity (0-1).</summary>
        public float CheckSeverity()
        {
            return _severity;
        }

        /// <summary>
        /// Returns the chance (0-1) that the survivor drops a carried item
        /// during hauling. Formula: severity * 0.3.
        /// </summary>
        public float GetItemDropChance()
        {
            return _severity * ITEM_DROP_MULTIPLIER;
        }

        /// <summary>Returns true if the survivor's speech is slurred.</summary>
        public bool IsSpeechSlurred()
        {
            return _speechSlur;
        }

        /// <summary>
        /// Roll to see if the survivor drops a carried item. Returns true
        /// if the item is dropped.
        /// </summary>
        public bool TryDropItem(string carriedItemId)
        {
            if (string.IsNullOrEmpty(carriedItemId)) return false;
            if (_severity <= 0f) return false;

            float chance = GetItemDropChance();
            float roll = (float)_rng.NextDouble();

            if (roll < chance)
            {
                OnItemDropped?.Invoke(_survivorId, carriedItemId);
                return true;
            }

            return false;
        }

        /// <summary>TBI is irreversible — always returns false.</summary>
        public bool IsIrreversible() => true;

        // ── Save / Load ────────────────────────────────────────────────

        public TBIState CaptureState()
        {
            return new TBIState
            {
                affliction_id = "affliction_tbi",
                survivor_id = _survivorId ?? "",
                severity = _severity,
                head_trauma_count = _headTraumaCount,
                item_drop_chance = GetItemDropChance(),
                speech_slur = _speechSlur
            };
        }

        public void RestoreState(TBIState saved)
        {
            if (saved == null) return;
            _survivorId = saved.survivor_id;
            _severity = saved.severity;
            _headTraumaCount = saved.head_trauma_count;
            _speechSlur = saved.speech_slur;
        }
    }
}
