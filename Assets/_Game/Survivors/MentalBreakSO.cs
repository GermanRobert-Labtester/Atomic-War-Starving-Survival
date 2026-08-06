using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// A specific mental-break state a survivor falls into when their morale
    /// has been catastrophically low for too long. The break alters the
    /// survivor's behavior (BingeEater force-consumes food; ViolentParanoia
    /// locks rooms or breaks the radio) and applies a passive morale drain
    /// to the OTHER survivors in the same room until cured.
    ///
    /// Cured by: time (cureHours), high-value Comfort items, or a specific
    /// MedicalBed intervention. Designers can extend the enum of break
    /// types by adding more assets; the system itself is data-driven.
    /// </summary>
    [CreateAssetMenu(fileName = "MentalBreak", menuName = "ASHFALL/Survivor/Mental Break")]
    public class MentalBreakSO : ScriptableObject
    {
        [Header("Identity")]
        public string id;                            // e.g. "binge_eater", "violent_paranoia"
        public string displayName;                   // "Binge Eater"
        [TextArea(2, 4)] public string description;  // Player-facing flavor

        [Header("Trait affinity weights")]
        [Tooltip("Per-trait weight when this break is rolled. A trait with weight 2 is " +
                 "twice as likely as a trait with weight 1, all else equal.")]
        public List<RiskBiasWeight> TraitWeights = new List<RiskBiasWeight>();

        [Header("Behavior (BingeEater)")]
        [Tooltip("Multiplier on the normal EatAction consumption: 1.0 = normal ration, " +
                 "3.0 = triple ration. Used by BingeEater-style breaks.")]
        public float consumptionMultiplier = 1f;

        [Tooltip("Minimum food.hungerRestore a slot must have to be a valid binge target. " +
                 "0 = eat anything; 30 = only eat high-value food. Used by BingeEater.")]
        public float minFoodValueForBinge = 0f;

        [Header("Behavior (ViolentParanoia)")]
        [Tooltip("Per-tick probability of this break triggering a sabotage event " +
                 "(room lockout, radio destruction). 0..1. Used by ViolentParanoia.")]
        [Range(0f, 1f)] public float sabotageChancePerTick = 0f;

        [Header("Passive drain")]
        [Tooltip("Morale drained per game-hour from every OTHER survivor sharing a " +
                 "room with the broken survivor. Default 0 (no drain).")]
        public float passiveMoraleDrainPerHour = 0f;

        [Tooltip("InterpersonalAffinity drained per game-hour from every OTHER " +
                 "survivor toward the broken survivor. Default 0 (no drain). " +
                 "Used by ViolentParanoia to erode trust.")]
        public float affinityDrainPerHour = 0f;

        [Header("Cure")]
        [Tooltip("Game-hours of natural decay before the break resolves on its own. " +
                 "0 = must be cured by Comfort item or MedicalBed intervention.")]
        public float cureHours = 48f;

        [Tooltip("Cure progress removed per high-value Comfort item consumed by the " +
                 "broken survivor. 0 = Comfort items don't help.")]
        public float comfortItemCureAmount = 24f;

        [Tooltip("If true, the break only resolves while the broken survivor is in a " +
                 "medical_bed shelter module (ViolentParanoia-tier).")]
        public bool requiresMedicalBed = false;

        [Tooltip("Canonical mental-break ids used in code (BingeEater / ViolentParanoia / Catatonia / FugueState).")]
        public string idReference;
        public static class Ids
        {
            public const string BingeEater      = "binge_eater";
            public const string ViolentParanoia = "violent_paranoia";
            public const string Catatonia       = "catatonia";
            public const string FugueState      = "fugue_state";
        }
    }

    /// <summary>One row of the trait-affinity table on a MentalBreakSO.</summary>
    [Serializable]
    public class RiskBiasWeight
    {
        public RiskBiasTrait Trait;
        [Range(0f, 10f)] public float Weight = 1f;
    }
}
