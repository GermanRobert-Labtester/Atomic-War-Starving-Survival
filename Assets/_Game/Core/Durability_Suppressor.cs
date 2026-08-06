using System;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Suppressor Durability (Prompt #603). Suppressors are improvised from
    /// oil filters, pillows, etc. and break after 5–10 shots. If the suppressor
    /// breaks mid-firefight the weapon produces massive noise, drawing every
    /// nearby enemy.
    /// Save/load safe. Plain C#.
    /// </summary>
    public class Durability_Suppressor
    {
        public const string SuppressorId = "durability_suppressor";
        public const float NoiseOnBreak = 100f;
        public const bool IsImprovised = true;

        public const int MinShots = 5;
        public const int MaxShots = 10;

        // -- Runtime state --
        public int MaxShotsRolled { get; private set; }
        public int ShotsRemaining { get; private set; }
        public bool IsBroken { get; private set; }

        // -- Events --
        public event Action OnSuppressorInstalled;
        public event Action<int> OnShotFired;        // shotsRemaining
        public event Action OnSuppressorBroken;
        public event Action<float> OnNoiseAlert;     // noiseLevel

        public Durability_Suppressor() { }

        /// <summary>
        /// Install a fresh improvised suppressor. <paramref name="maxShotsRoll"/>
        /// should be in [5, 10].
        /// </summary>
        public void InstallSuppressor(int maxShotsRoll)
        {
            MaxShotsRolled = Math.Max(MinShots, Math.Min(MaxShots, maxShotsRoll));
            ShotsRemaining = MaxShotsRolled;
            IsBroken = false;
            OnSuppressorInstalled?.Invoke();
        }

        /// <summary>
        /// Fire one shot through the suppressor. Returns true if the suppressor
        /// broke on this shot.
        /// </summary>
        public bool FireShot()
        {
            if (IsBroken)
            {
                OnNoiseAlert?.Invoke(NoiseOnBreak);
                return true;
            }

            ShotsRemaining--;
            OnShotFired?.Invoke(ShotsRemaining);

            if (ShotsRemaining <= 0)
            {
                IsBroken = true;
                OnSuppressorBroken?.Invoke();
                OnNoiseAlert?.Invoke(NoiseOnBreak);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns the current stealth status. True only if the suppressor is
        /// installed and not broken.
        /// </summary>
        public bool GetStealthStatus()
        {
            return !IsBroken && ShotsRemaining > 0;
        }

        /// <summary>Noise produced when the suppressor breaks.</summary>
        public float GetNoiseOnBreak() => NoiseOnBreak;

        // -----------------------------------------------------------------
        // Save / Load
        // -----------------------------------------------------------------

        public SuppressorSave CaptureState()
        {
            return new SuppressorSave
            {
                MaxShotsRolled = MaxShotsRolled,
                ShotsRemaining = ShotsRemaining,
                IsBroken = IsBroken
            };
        }

        public void RestoreState(SuppressorSave save)
        {
            if (save == null) return;
            MaxShotsRolled = save.MaxShotsRolled;
            ShotsRemaining = save.ShotsRemaining;
            IsBroken = save.IsBroken;
        }
    }

    [Serializable]
    public class SuppressorSave
    {
        public int MaxShotsRolled;
        public int ShotsRemaining;
        public bool IsBroken;
    }
}
