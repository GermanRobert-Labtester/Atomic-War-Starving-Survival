using System;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Cement Mixing Action (Prompt #581). Upgrading walls to concrete isn't
    /// instant — requires Water, Gravel, and Limestone. Once applied the wall
    /// is "Wet" and provides zero defense until fully cured (48 h). Raiding a
    /// wet wall is trivially easy.
    /// Save/load safe. Plain C#.
    /// </summary>
    /// <summary>DEMOTE-Action-remaining — dormant ghost; not Boot/Save wired until a host calls APIs.</summary>
    public class Action_MixCement
    {
        public const string ActionId = "action_mix_cement";

        public const int WaterRequired = 5;
        public const int GravelRequired = 3;
        public const int LimestoneRequired = 2;
        public const float CuringHours = 48f;
        public const float WallDefenseWhenWet = 0f;

        // -- Runtime state --
        public float HoursRemaining { get; private set; }
        public bool IsCuring { get; private set; }
        public bool IsCured { get; private set; }
        public bool IsWet { get; private set; }

        // -- Events --
        public event Action OnCementMixed;
        public event Action OnCuringStarted;
        public event Action OnCementCured;
        public event Action OnWetWallBreached;

        public Action_MixCement() { }

        /// <summary>
        /// Attempt to start mixing cement. Returns true if the survivor has
        /// enough resources and mixing begins.
        /// </summary>
        public bool StartMixing(int waterAvail, int gravelAvail, int limestoneAvail)
        {
            if (IsCuring || IsCured) return false;
            if (waterAvail < WaterRequired) return false;
            if (gravelAvail < GravelRequired) return false;
            if (limestoneAvail < LimestoneRequired) return false;

            IsCuring = true;
            IsWet = true;
            HoursRemaining = CuringHours;

            OnCementMixed?.Invoke();
            OnCuringStarted?.Invoke();
            return true;
        }

        /// <summary>
        /// Advance the curing clock. Once hours reach zero the wall is fully
        /// hardened and provides normal defense.
        /// </summary>
        public void TickHour(float hours)
        {
            if (!IsCuring || IsCured) return;

            HoursRemaining -= hours;
            if (HoursRemaining <= 0f)
            {
                HoursRemaining = 0f;
                IsCuring = false;
                IsWet = false;
                IsCured = true;
                OnCementCured?.Invoke();
            }
        }

        /// <summary>
        /// Returns the effective wall defense value. Wet concrete provides
        /// zero defense; cured concrete delegates to the caller's base value.
        /// </summary>
        public float GetWallDefense(float baseCuredDefense = 100f)
        {
            if (IsWet) return WallDefenseWhenWet;
            if (IsCured) return baseCuredDefense;
            return 0f; // not started
        }

        /// <summary>True while wall is wet and offers no raid protection.</summary>
        public bool IsWallVulnerable()
        {
            bool vulnerable = IsWet && !IsCured;
            if (vulnerable)
                OnWetWallBreached?.Invoke();
            return vulnerable;
        }

        // -----------------------------------------------------------------
        // Save / Load
        // -----------------------------------------------------------------

        public MixCementSave CaptureState()
        {
            return new MixCementSave
            {
                HoursRemaining = HoursRemaining,
                IsCuring = IsCuring,
                IsCured = IsCured,
                IsWet = IsWet
            };
        }

        public void RestoreState(MixCementSave save)
        {
            if (save == null) return;
            HoursRemaining = save.HoursRemaining;
            IsCuring = save.IsCuring;
            IsCured = save.IsCured;
            IsWet = save.IsWet;
        }
    }

    [Serializable]
    public class MixCementSave
    {
        public float HoursRemaining;
        public bool IsCuring;
        public bool IsCured;
        public bool IsWet;
    }
}
