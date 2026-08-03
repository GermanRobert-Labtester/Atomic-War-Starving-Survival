using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Events;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Core
{
    public enum EncounterCategory
    {
        Combat,     // Feral Dogs, Civil War Deserters, Looters
        Hazard,     // Collapsed Rubble, Radiation Hotspot, Trap
        Discovery   // Supply Cache, Abandoned Vehicle
    }

    /// <summary>
    /// ScriptableObject defining an expedition encounter node event.
    /// Encounters support psychological auto-resolution based on RiskBiasTrait
    /// (Reckless auto-engages, Paranoid flees/drops loot) and belief choices.
    /// </summary>
    [CreateAssetMenu(fileName = "NewEncounter", menuName = "ASHFALL/Encounter Event")]
    public class EncounterSO : ScriptableObject
    {
        public string id;
        public string title;
        [TextArea(3, 6)] public string description;

        public EncounterCategory category = EncounterCategory.Hazard;
        public float baseWeight = 1f;

        // Stance weight modifiers
        public float stealthWeightMultiplier = 0.5f;
        public float speedWeightMultiplier = 1.5f;

        // Min danger level required for this encounter
        public float minDangerLevel;

        /// <summary>
        /// When non-empty, this encounter is only eligible for expeditions whose
        /// <c>TargetLocationId</c> matches (Prompt #47 — radio intel → location outcomes).
        /// Empty = valid anywhere (default feral dogs / rubble / etc.).
        /// </summary>
        public string requiredLocationId = string.Empty;

        /// <summary>
        /// When true and <see cref="requiredLocationId"/> matches, fire once on
        /// first arrival at the target (Looting phase start) instead of random roll.
        /// Used for scripted Safe Haven ambush / empty-cache beats.
        /// </summary>
        public bool forceOnArrival;

        // Psychological auto-resolution triggers
        public bool enableAutoResolution = true;
        public RiskBiasTrait autoEngageTrait = RiskBiasTrait.Reckless;
        public RiskBiasTrait autoFleeTrait = RiskBiasTrait.Paranoid;

        public List<EventChoice> choices = new List<EventChoice>();

        /// <summary>
        /// Gets the effective selection weight for a given stance and danger level.
        /// </summary>
        public float GetEffectiveWeight(ExpeditionStance stance, float dangerLevel)
        {
            return GetEffectiveWeight(stance, dangerLevel, locationId: null);
        }

        /// <summary>
        /// Weight for stance + danger, optionally filtered by expedition location.
        /// Location-bound encounters return 0 when the location does not match.
        /// </summary>
        public float GetEffectiveWeight(ExpeditionStance stance, float dangerLevel, string locationId)
        {
            if (dangerLevel < minDangerLevel) return 0f;
            if (!string.IsNullOrEmpty(requiredLocationId))
            {
                if (string.IsNullOrEmpty(locationId)
                    || !string.Equals(requiredLocationId, locationId, StringComparison.Ordinal))
                    return 0f;
            }
            float weight = baseWeight;
            if (stance == ExpeditionStance.Stealth) weight *= stealthWeightMultiplier;
            else if (stance == ExpeditionStance.Speed) weight *= speedWeightMultiplier;
            return Mathf.Max(0f, weight);
        }
    }
}
