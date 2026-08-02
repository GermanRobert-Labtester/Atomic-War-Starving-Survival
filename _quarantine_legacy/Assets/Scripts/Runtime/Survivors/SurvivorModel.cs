using System;
using AtomicWar.Data;
using UnityEngine;

namespace AtomicWar.Runtime.Survivors
{
    public enum SurvivorState
    {
        Idle,
        Sleeping,
        Crafting,
        Guard,
        Scavenging,
        Sick,
        Dead
    }

    /// <summary>
    /// Pure C# model representing an active survivor instance.
    /// </summary>
    public class SurvivorModel
    {
        public string InstanceId { get; }
        public SurvivorData Data { get; }

        public float Health { get; set; }
        public float Hunger { get; set; } // 0 = full, 100 = starving
        public float Fatigue { get; set; } // 0 = rested, 100 = exhausted
        public float Sickness { get; set; } // 0 = healthy, 100 = critical
        public float Morale { get; set; } // 100 = high morale, 0 = mentally broken

        // State tracking flags for event trigger edge detection
        public bool WasStarving { get; set; }
        public bool WasExhausted { get; set; }
        public bool WasSick { get; set; }
        public bool WasMentallyBroken { get; set; }

        public SurvivorState CurrentState { get; set; } = SurvivorState.Idle;

        public SurvivorModel(SurvivorData data)
        {
            InstanceId = Guid.NewGuid().ToString();
            Data = data;
            Health = data.MaxHealth;
            Hunger = 0f;
            Fatigue = 0f;
            Sickness = 0f;
            Morale = 75f;
        }

        public bool IsAlive => Health > 0 && CurrentState != SurvivorState.Dead;
    }
}
