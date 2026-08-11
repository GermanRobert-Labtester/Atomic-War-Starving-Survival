using System;

namespace AtomicWar._Game.AI.Actions
{
    /// <summary>
    /// Cover Tracks Action (Prompt #606). A toggle on the Expedition Map.
    /// When active, travel time triples and Fatigue drains faster, but Ambush
    /// Chance and Tracking both drop to zero. Useful for evading pursuit at
    /// the cost of speed and endurance.
    /// Save/load safe. Plain C#.
    /// </summary>
    /// <summary>DEMOTE-Action-batch — dormant ghost; not Boot/Save wired until a host calls APIs.</summary>
    public class Action_CoverTracks
    {
        public const string ActionId = "action_cover_tracks";

        public const float TravelTimeMultiplier = 3f;
        public const float FatigueDrainMultiplier = 2f;
        public const float AmbushChanceOverride = 0f;
        public const float TrackingOverride = 0f;

        // -- Runtime state --
        public bool IsActive { get; private set; }

        // -- Events --
        public event Action<bool> OnCoverTracksToggled;

        public Action_CoverTracks() { }

        /// <summary>Enable or disable the cover-tracks toggle.</summary>
        public void Toggle(bool enable)
        {
            if (IsActive == enable) return;
            IsActive = enable;
            OnCoverTracksToggled?.Invoke(IsActive);
        }

        /// <summary>Effective travel-time multiplier (1 when inactive).</summary>
        public float GetTravelTimeMultiplier()
        {
            return IsActive ? TravelTimeMultiplier : 1f;
        }

        /// <summary>Effective fatigue drain multiplier (1 when inactive).</summary>
        public float GetFatigueMultiplier()
        {
            return IsActive ? FatigueDrainMultiplier : 1f;
        }

        /// <summary>
        /// Effective ambush chance. Zero while covering tracks; otherwise the
        /// base chance passed in.
        /// </summary>
        public float GetAmbushChance(float baseAmbushChance)
        {
            return IsActive ? AmbushChanceOverride : baseAmbushChance;
        }

        /// <summary>
        /// Effective tracking level. Zero while covering tracks; otherwise the
        /// base tracking value passed in.
        /// </summary>
        public float GetTrackingLevel(float baseTracking)
        {
            return IsActive ? TrackingOverride : baseTracking;
        }

        // -----------------------------------------------------------------
        // Save / Load
        // -----------------------------------------------------------------

        public CoverTracksSave CaptureState()
        {
            return new CoverTracksSave
            {
                IsActive = IsActive
            };
        }

        public void RestoreState(CoverTracksSave save)
        {
            if (save == null) return;
            IsActive = save.IsActive;
        }
    }

    [Serializable]
    public class CoverTracksSave
    {
        public bool IsActive;
    }
}
