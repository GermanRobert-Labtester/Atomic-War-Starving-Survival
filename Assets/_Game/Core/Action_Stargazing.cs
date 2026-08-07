using System;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Stargazing Action (Prompt #598). A survivor climbs out of the hatch at
    /// night to look at the sky. Consumes time and risks hypothermia. A clear
    /// sky grants Hope; if the orbital platform is visible the survivor falls
    /// into Nihilism instead.
    /// Save/load safe. Plain C# (stateless action — no persistent state beyond
    /// the single execution, but save class provided for mid-action interrupt).
    /// </summary>
    
    [Serializable]
    public class Action_StargazingSave
    {
        public string systemId = "action_stargazing";
    }
public class Action_Stargazing
    {
        public const string ActionId = "action_stargazing";
        public const string AfflictionHypothermia = "hypothermia";

        public const float HoursRequired = 2f;
        public const float HypothermiaChance = 0.15f;
        public const float HopeGain = 20f;
        public const float NihilismGain = -25f;

        // -- Events --
        public event Action<float, string> OnStargazingCompleted;  // moraleChange, afflictionId|null
        public event Action<float> OnHopeGained;                   // amount
        public event Action<float> OnNihilismGained;                // amount (negative)
        public event Action OnHypothermiaContracted;

        public Action_Stargazing() { }

        /// <summary>
        /// Execute the stargazing action. Night is required. Returns the
        /// morale change and an affliction id (null if no affliction).
        /// </summary>
        public (float moraleChange, string afflictionId) ExecuteStargazing(
            bool isNight,
            bool isSkyClear,
            bool orbitalPlatformVisible,
            Random rng)
        {
            if (!isNight) return (0f, null);

            float moraleChange = 0f;
            string afflictionId = null;

            // Sky observation.
            if (isSkyClear && !orbitalPlatformVisible)
            {
                moraleChange += HopeGain;
                OnHopeGained?.Invoke(HopeGain);
            }
            else if (orbitalPlatformVisible)
            {
                moraleChange += NihilismGain;
                OnNihilismGained?.Invoke(NihilismGain);
            }

            // Hypothermia risk (exposure to nuclear-winter night air).
            float roll = (float)(rng != null ? rng.NextDouble() : 0.0);
            if (roll < HypothermiaChance)
            {
                afflictionId = AfflictionHypothermia;
                OnHypothermiaContracted?.Invoke();
            }

            OnStargazingCompleted?.Invoke(moraleChange, afflictionId);
            return (moraleChange, afflictionId);
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public Action_StargazingSave CaptureState() => new Action_StargazingSave();

        public void RestoreState(Action_StargazingSave saved) { _ = saved; }

}
}
