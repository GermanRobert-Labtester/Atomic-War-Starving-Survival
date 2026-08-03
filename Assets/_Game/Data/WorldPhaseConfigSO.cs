using UnityEngine;

namespace AtomicWar._Game.Data
{
    /// <summary>
    /// Single-instance designer config for WorldPhaseSystem: when the atomic exchange
    /// happens and how hard it hits morale. Optional — WorldPhaseSystem falls back to
    /// these same defaults when no asset is assigned, so it stays constructible in
    /// tests without asset loading.
    /// </summary>
    [CreateAssetMenu(fileName = "WorldPhaseConfig", menuName = "ASHFALL/Data/World Phase Config")]
    public class WorldPhaseConfigSO : ScriptableObject
    {
        [Tooltip("Campaign day the atomic exchange happens (Flashpoint). Nuclear Winter begins the day after.")]
        public int flashpointDay = 30;

        [Tooltip("Permanent morale penalty applied to every survivor at the exchange.")]
        public float exchangeMoraleHit = 25f;
    }
}
