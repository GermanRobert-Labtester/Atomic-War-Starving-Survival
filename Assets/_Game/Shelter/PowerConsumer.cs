using System;
using UnityEngine;

namespace AtomicWar._Game.Shelter
{
    /// <summary>
    /// A module load on the shelter power grid. Priority 1 = critical (shed last);
    /// priority 5 = luxury (shed first). Player sets priority and request toggle;
    /// the network may shed (IsShed) to prevent blackout.
    /// </summary>
    [Serializable]
    public class PowerConsumer
    {
        public string ModuleId;
        public string DisplayName;
        public float Watts;
        [Range(1, 5)] public int Priority = 3;
        /// <summary>Player wants this module powered.</summary>
        public bool IsRequested = true;
        /// <summary>Network auto-shed this load to stay under generation.</summary>
        public bool IsShed;
        /// <summary>True when the player locked priority (UI toggle system).</summary>
        public bool PriorityLocked;

        public PowerConsumer() { }

        public PowerConsumer(string moduleId, string displayName, float watts, int priority)
        {
            ModuleId = moduleId;
            DisplayName = displayName;
            Watts = Mathf.Max(0f, watts);
            Priority = Mathf.Clamp(priority, 1, 5);
            IsRequested = true;
            IsShed = false;
        }

        /// <summary>Currently drawing from the grid.</summary>
        public bool IsPowered => IsRequested && !IsShed && Watts > 0f;

        /// <summary>Effective draw this tick (0 if shed or not requested).</summary>
        public float EffectiveDraw => IsPowered ? Watts : 0f;
    }
}
