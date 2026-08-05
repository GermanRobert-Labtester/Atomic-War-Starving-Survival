using System;
using UnityEngine;

namespace AtomicWar._Game.Shelter
{
    /// <summary>
    /// Runtime state of an installed power source. Save/load safe primitives;
    /// definition re-bound after load via SourceId.
    /// </summary>
    [Serializable]
    public class PowerSourceInstance
    {
        public string SourceId;
        public bool IsEnabled = true;
        /// <summary>On-board fuel for diesel sources (separate from heater fuel).</summary>
        public float Fuel;
        /// <summary>
        /// Prompt #200 — Thermodynamics burn mult from last loader (0.8 = 20% longer).
        /// </summary>
        public float FuelBurnMultiplier = 1f;
        /// <summary>
        /// Mechanical condition 0..100. Below the ignition threshold, overworked
        /// diesel gens / makeshift heaters may spark a room fire (Internal Horror).
        /// </summary>
        public float Durability = 100f;
        /// <summary>Survivor currently pedaling a bicycle generator (null if none).</summary>
        public string PedalingSurvivorId;
        /// <summary>Room where this source is installed (noise adjacency for sleep quality).</summary>
        public string RoomId;

        [NonSerialized]
        private PowerSourceSO _definition;

        public PowerSourceSO Definition
        {
            get => _definition;
            set
            {
                _definition = value;
                if (value != null && string.IsNullOrEmpty(SourceId))
                    SourceId = value.SourceId;
            }
        }

        public PowerSourceInstance() { }

        public PowerSourceInstance(PowerSourceSO definition, float initialFuel = 0f)
        {
            Definition = definition;
            SourceId = definition != null ? definition.SourceId : string.Empty;
            Fuel = Mathf.Max(0f, initialFuel);
            IsEnabled = true;
        }

        public void AddFuel(float amount) => AddFuel(amount, 1f);

        public void AddFuel(float amount, float burnMultiplier)
        {
            if (amount <= 0f) return;
            Fuel = Mathf.Max(0f, Fuel + amount);
            if (burnMultiplier > 0f)
                FuelBurnMultiplier = burnMultiplier;
        }

        public float EffectiveFuelBurnMultiplier =>
            FuelBurnMultiplier > 0f ? FuelBurnMultiplier : 1f;

        public void AssignPedaler(string survivorId)
        {
            PedalingSurvivorId = survivorId;
        }

        public void ClearPedaler()
        {
            PedalingSurvivorId = null;
        }

        public bool HasPedaler => !string.IsNullOrEmpty(PedalingSurvivorId);
    }
}
