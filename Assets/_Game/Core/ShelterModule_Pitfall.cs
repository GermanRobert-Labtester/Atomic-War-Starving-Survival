using System;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Shelter Module — Pitfall Trap (Prompt #605). Dug inside a bunker
    /// hallway. Instantly kills the first 3 raiders that step on it. Corpses
    /// and loot are crushed and unrecoverable. After capacity is exhausted the
    /// pit must be cleared and re-dug.
    /// Save/load safe. Plain C#.
    /// </summary>
    public class ShelterModule_Pitfall
    {
        public const string ModuleId = "shelter_module_pitfall";

        public const int KillCapacity = 3;
        public const bool LootRecoverable = false;
        public const float ConstructionHours = 24f;

        // -- Runtime state --
        public int RaidersKilled { get; private set; }
        public bool IsActive { get; private set; }

        // -- Events --
        public event Action<int> OnRaiderKilled;       // totalKilled
        public event Action OnPitfallExhausted;
        public event Action OnLootLost;

        public ShelterModule_Pitfall() { }

        /// <summary>
        /// Mark the pitfall as active (construction complete).
        /// </summary>
        public void Activate()
        {
            IsActive = true;
            RaidersKilled = 0;
        }

        /// <summary>
        /// Attempt to kill a raider with the pitfall. Returns (killed, lootLost).
        /// If the pitfall has remaining capacity the raider dies; otherwise the
        /// trap is exhausted and has no effect.
        /// </summary>
        public (bool killed, bool lootLost) TryKillRaider()
        {
            if (!IsActive || IsExhausted()) return (false, false);

            RaidersKilled++;
            OnRaiderKilled?.Invoke(RaidersKilled);
            OnLootLost?.Invoke();

            bool exhausted = IsExhausted();
            if (exhausted)
            {
                IsActive = false;
                OnPitfallExhausted?.Invoke();
            }

            return (true, true); // loot is always crushed
        }

        /// <summary>True when kill capacity is reached.</summary>
        public bool IsExhausted()
        {
            return RaidersKilled >= KillCapacity;
        }

        /// <summary>
        /// Clear bodies and re-dig the pitfall trap. Resets kill count and
        /// reactivates the module.
        /// </summary>
        public void Reset()
        {
            RaidersKilled = 0;
            IsActive = true;
        }

        // -----------------------------------------------------------------
        // Save / Load
        // -----------------------------------------------------------------

        public PitfallSave CaptureState()
        {
            return new PitfallSave
            {
                RaidersKilled = RaidersKilled,
                IsActive = IsActive
            };
        }

        public void RestoreState(PitfallSave save)
        {
            if (save == null) return;
            RaidersKilled = save.RaidersKilled;
            IsActive = save.IsActive;
        }
    }

    [Serializable]
    public class PitfallSave
    {
        public int RaidersKilled;
        public bool IsActive;
    }
}
