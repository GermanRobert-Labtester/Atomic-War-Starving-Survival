using System;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Silent Takedown Action (Prompt #604). Requires a knife and stealth
    /// stance. Instantly kills one enemy silently — but if the survivor's
    /// Strength is too low the enemy struggles, producing noise that triggers
    /// full combat.
    /// Save/load safe. Plain C# (stateless action).
    /// </summary>
    
    [Serializable]
    public class Action_SilentTakedownSave
    {
        public string systemId = "action_silent_takedown";
    }
/// <summary>DEMOTE-Action-remaining — dormant ghost; not Boot/Save wired until a host calls APIs.</summary>
    public class Action_SilentTakedown
    {
        public const string ActionId = "action_silent_takedown";
        public const bool RequiresKnife = true;
        public const bool RequiresStealth = true;
        public const float StrengthThreshold = 40f;
        public const float NoiseOnFailure = 80f;

        // -- Events --
        public event Action OnSilentKill;
        public event Action OnStruggleInitiated;
        public event Action<float> OnCombatAlerted;  // noiseLevel

        public Action_SilentTakedown() { }

        /// <summary>
        /// Check whether prerequisites are met to attempt a silent takedown.
        /// </summary>
        public bool CanExecute(bool hasKnife, bool isStealthed)
        {
            return hasKnife && isStealthed;
        }

        /// <summary>
        /// Execute the takedown. Returns (success, isSilent). Success requires
        /// survivor Strength >= enemy Strength OR survivor Strength >= threshold.
        /// A failed attempt always produces noise.
        /// </summary>
        public (bool success, bool isSilent) Execute(
            float survivorStrength,
            float enemyStrength,
            Random rng)
        {
            bool strongEnough = survivorStrength >= enemyStrength
                             || survivorStrength >= StrengthThreshold;

            if (strongEnough)
            {
                OnSilentKill?.Invoke();
                return (true, true);
            }

            // Struggle — enemy fights back, noise alert.
            OnStruggleInitiated?.Invoke();
            OnCombatAlerted?.Invoke(NoiseOnFailure);
            return (false, false);
        }

        /// <summary>Noise level produced on a failed takedown attempt.</summary>
        public float GetNoiseOnFailure() => NoiseOnFailure;
    
        // ── Save / Load ────────────────────────────────────────────────
        public Action_SilentTakedownSave CaptureState() => new Action_SilentTakedownSave();

        public void RestoreState(Action_SilentTakedownSave saved) { _ = saved; }

}
}
