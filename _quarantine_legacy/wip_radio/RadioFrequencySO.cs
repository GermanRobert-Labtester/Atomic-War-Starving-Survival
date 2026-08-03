using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Data
{
    /// <summary>
    /// Frequency type classification for radio broadcasts.
    /// </summary>
    public enum RadioFrequencyType
    {
        Civilian,
        Military,
        NumbersStation,
        Emergency,
        Unknown
    }

    /// <summary>
    /// ScriptableObject defining a radio frequency with its broadcast pool and availability.
    /// Each frequency has a day range when it's active and a pool of associated broadcasts.
    /// </summary>
    [CreateAssetMenu(fileName = "NewRadioFrequency", menuName = "ASHFALL/Data/Radio Frequency")]
    public class RadioFrequencySO : ScriptableObject
    {
        [Header("Frequency Identity")]
        public string id;
        public string displayName;
        [Tooltip("Frequency in MHz (e.g., 88.5, 102.1)")]
        public float frequencyMHz;
        public RadioFrequencyType type;

        [Header("Availability")]
        [Tooltip("Day when this frequency becomes active")]
        public int activeFromDay = 0;
        [Tooltip("Day when this frequency goes silent (-1 = never)")]
        public int activeUntilDay = -1;

        [Header("Signal Characteristics")]
        [Tooltip("Base signal strength (0..1) before weather/damage modifiers")]
        [Range(0f, 1f)]
        public float baseSignalStrength = 0.7f;
        [Tooltip("Signal degradation factor (0..1). Higher = more susceptible to interference")]
        [Range(0f, 1f)]
        public float interferenceSusceptibility = 0.3f;

        [Header("Broadcast Pool")]
        public List<RadioBroadcastSO> broadcasts = new List<RadioBroadcastSO>();

        /// <summary>
        /// Check if this frequency is active on the given day.
        /// </summary>
        public bool IsActiveOnDay(int day)
        {
            if (day < activeFromDay) return false;
            if (activeUntilDay >= 0 && day > activeUntilDay) return false;
            return true;
        }

        /// <summary>
        /// Get a random broadcast from the pool. Returns null if pool is empty.
        /// </summary>
        public RadioBroadcastSO GetRandomBroadcast(Random rng)
        {
            if (broadcasts == null || broadcasts.Count == 0 || rng == null) return null;
            int index = rng.Next(broadcasts.Count);
            return broadcasts[index];
        }
    }
}
