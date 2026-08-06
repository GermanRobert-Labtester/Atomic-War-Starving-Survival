using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class SheriffState
    {
        public string roleId = "role_sheriff";
        public float orderGenerated = 0.3f;
        public float affinityPenalty = -0.1f;
        public string currentSheriffId;
    }

    /// <summary>
    /// The Sheriff role imposes order on the shelter at the cost of social
    /// bonds. While active, the sheriff generates order every hour but
    /// slowly erodes affinity with every other survivor.
    /// Plain C# class, not a MonoBehaviour.
    /// </summary>
    public class Role_Sheriff
    {
        // ── Events ──────────────────────────────────────────────────────
        public event Action<string> OnSheriffAssigned;                          // survivorId
        public event Action<string> OnSheriffRemoved;                           // survivorId
        public event Action<string, float> OnOrderGenerated;                    // sheriffId, order
        public event Action<string, string, float> OnAffinityReduced;          // sheriffId, otherSurvivorId, penalty

        // ── State ───────────────────────────────────────────────────────
        private string _currentSheriffId;
        private float _orderRate = 0.3f;
        private float _affinityPenalty = -0.1f;

        // ── Public API ──────────────────────────────────────────────────

        /// <summary>
        /// Assigns the Sheriff role to a survivor. Immediately generates an
        /// initial order pulse and applies the affinity penalty to every
        /// other survivor in the shelter.
        /// </summary>
        public void AssignSheriff(string survivorId, List<string> allSurvivorIds)
        {
            if (string.IsNullOrEmpty(survivorId)) return;

            // Remove existing sheriff first, if any
            if (!string.IsNullOrEmpty(_currentSheriffId) && _currentSheriffId != survivorId)
            {
                string old = _currentSheriffId;
                _currentSheriffId = null;
                OnSheriffRemoved?.Invoke(old);
            }

            _currentSheriffId = survivorId;
            OnSheriffAssigned?.Invoke(survivorId);

            // Initial order pulse
            OnOrderGenerated?.Invoke(survivorId, _orderRate);

            // Initial affinity penalty with everyone else
            if (allSurvivorIds != null)
            {
                for (int i = 0; i < allSurvivorIds.Count; i++)
                {
                    string otherId = allSurvivorIds[i];
                    if (otherId == survivorId) continue;
                    OnAffinityReduced?.Invoke(survivorId, otherId, _affinityPenalty);
                }
            }
        }

        /// <summary>
        /// Removes the Sheriff role from whoever currently holds it.
        /// </summary>
        public void RemoveSheriff()
        {
            if (string.IsNullOrEmpty(_currentSheriffId)) return;

            string removed = _currentSheriffId;
            _currentSheriffId = null;
            OnSheriffRemoved?.Invoke(removed);
        }

        /// <summary>
        /// Called once per in-game hour. The active sheriff generates order
        /// and continues to erode affinity with all other survivors.
        /// </summary>
        public void TickHour(List<string> allSurvivorIds)
        {
            if (string.IsNullOrEmpty(_currentSheriffId)) return;

            // Ongoing order generation
            OnOrderGenerated?.Invoke(_currentSheriffId, _orderRate);

            // Ongoing affinity drain
            if (allSurvivorIds == null) return;
            for (int i = 0; i < allSurvivorIds.Count; i++)
            {
                string otherId = allSurvivorIds[i];
                if (otherId == _currentSheriffId) continue;
                OnAffinityReduced?.Invoke(_currentSheriffId, otherId, _affinityPenalty);
            }
        }

        /// <summary>
        /// Returns the survivorId of the current sheriff, or null if none.
        /// </summary>
        public string GetCurrentSheriff()
        {
            return _currentSheriffId;
        }

        // ── Save / Load ─────────────────────────────────────────────────

        public SheriffState CaptureState()
        {
            return new SheriffState
            {
                roleId = "role_sheriff",
                orderGenerated = _orderRate,
                affinityPenalty = _affinityPenalty,
                currentSheriffId = _currentSheriffId
            };
        }

        public void RestoreState(SheriffState saved)
        {
            if (saved == null)
            {
                _currentSheriffId = null;
                _orderRate = 0.3f;
                _affinityPenalty = -0.1f;
                return;
            }

            _currentSheriffId = saved.currentSheriffId;
            _orderRate = saved.orderGenerated;
            _affinityPenalty = saved.affinityPenalty;
        }
    }
}
