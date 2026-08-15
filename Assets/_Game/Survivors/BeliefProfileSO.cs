using UnityEngine;
using Ashfall.Core.Journal;

namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Per-trait tunables for BeliefSystem: how fast this trait updates its
    /// perceived radiation risk from experience, how anxiety/numbness build, and
    /// how cautious it is when scoring the Scavenge action. One asset per
    /// RiskBiasTrait value.
    /// </summary>
    [CreateAssetMenu(fileName = "New Belief Profile", menuName = "ASHFALL/Survivors/Belief Profile")]
    public class BeliefProfileSO : ScriptableObject
    {
        public RiskBiasTrait Trait;

        [Header("Experience Update Rates")]
        public float ExperienceGainRate = 0.15f;
        public float ExperienceDecayRate = 0.1f;

        [Header("Trait Multipliers On Observed Events")]
        [Tooltip("How strongly witnessing someone get sick raises this trait's PerceivedRadRisk.")]
        public float SicknessObservedGainMultiplier = 1f;
        [Tooltip("How strongly surviving a 'hot' trip unharmed lowers PerceivedRadRisk. High for Reckless/Denialist.")]
        public float SurvivedHotTripOverconfidenceMultiplier = 1f;
        [Tooltip("How much high instrument uncertainty dampens the survived-hot-trip confidence drop.")]
        public float UncertaintyDampens = 0.5f;

        [Header("Mental Status Rates")]
        public float AnxietyGainRate = 0.05f;
        public float NumbnessGainRate = 0.02f;
        [Tooltip("How prone this trait is to numbness when perceived risk stays low.")]
        public float NumbnessProneness = 0.3f;

        [Header("Scavenge Multiplier")]
        [Tooltip("Baseline Scavenge-multiplier scalar for this trait before risk/uncertainty are applied.")]
        public float RiskBiasFactor = 1f;
        [Tooltip("How much this trait's Scavenge caution responds to instrument uncertainty.")]
        public AnimationCurve ScavengeUncertaintyCurve = AnimationCurve.Linear(0, 0, 1, 1);
    }
}
