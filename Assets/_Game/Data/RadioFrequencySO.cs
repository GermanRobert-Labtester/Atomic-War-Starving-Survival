using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Data
{
    /// <summary>
    /// Type of radio frequency (affects intel extraction and availability).
    /// </summary>
    public enum RadioFrequencyType
    {
        Civilian,
        Military,
        Emergency,
        NumbersStation,
        Unknown
    }

    /// <summary>
    /// ScriptableObject defining a radio frequency with its broadcast pool and characteristics.
    /// Frequencies have day ranges when they're active, base signal strength, and interference
    /// susceptibility. Used by RadioTunerSystem to manage tuning and intel extraction.
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

        [Header("Faction intercepts")]
        [Tooltip(
            "Channel tag used by the intercept strip when this frequency is tuned " +
            "(e.g. CH-7 MILBAND). Empty = show no faction intercepts on this band " +
            "(intel still extracts). Leave blank to use DefaultChannelTagForType.")]
        public string interceptChannelTag;

        [Header("Broadcast Pool")]
        public List<RadioBroadcastSO> broadcasts = new List<RadioBroadcastSO>();

        /// <summary>
        /// Canonical ids used by GameBootstrap default frequency table.
        /// </summary>
        public static class Ids
        {
            public const string Civilian = "88.5_civilian";
            public const string Military = "102.1_military";
            public const string Numbers = "99.0_numbers";
            public const string Emergency = "107.0_emergency";
        }

        /// <summary>
        /// Resolve the intercept channel tag for this frequency (explicit field
        /// or type default). Empty means "no faction intercepts on this band".
        /// </summary>
        public string ResolveInterceptChannelTag()
        {
            if (!string.IsNullOrEmpty(interceptChannelTag))
                return interceptChannelTag;
            return DefaultChannelTagForType(type);
        }

        /// <summary>
        /// Default faction-intercept channel for a frequency type.
        /// Military → milband, Civilian → ash road, Emergency/Numbers → stockpile.
        /// </summary>
        public static string DefaultChannelTagForType(RadioFrequencyType type)
        {
            switch (type)
            {
                case RadioFrequencyType.Military: return "CH-7 MILBAND";
                case RadioFrequencyType.Civilian: return "CH-3 ASH ROAD";
                case RadioFrequencyType.Emergency: return "CH-11 STOCKPILE";
                case RadioFrequencyType.NumbersStation: return "CH-11 STOCKPILE";
                default: return string.Empty;
            }
        }

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
        public RadioBroadcastSO GetRandomBroadcast(System.Random rng)
        {
            if (broadcasts == null || broadcasts.Count == 0 || rng == null) return null;
            int index = rng.Next(broadcasts.Count);
            return broadcasts[index];
        }
    }
}
