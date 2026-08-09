using AtomicWar._Game.Shelter;
using System;
using UnityEngine;

namespace AtomicWar._Game.Shelter.Modules
{
    /// <summary>
    /// ScriptableObject definition for the decontamination station module.
    /// Data-driven parameters: decon rate, water cost per hour, residual contamination
    /// floor (imperfect decon), process time per item, and whether the station is
    /// operational.
    /// </summary>
    [CreateAssetMenu(fileName = "DeconStationModule", menuName = "ASHFALL/Shelter/Decon Station Module")]
    public class DeconStationModuleSO : ShelterModule
    {
        [Header("Decontamination Station Parameters")]
        public float DeconRatePerHour = 0.5f;

        [Header("Decon Cost")]
        [Tooltip("Units of clean water consumed per hour of decon operation")]
        public float WaterCostPerHour = 2f;

        [Header("Residual Floor")]
        [Tooltip("Minimum contamination level items can be cleaned to (0..1). Imperfect decon.")]
        [Range(0f, 0.5f)]
        public float ResidualFloor = 0.05f;

        [Tooltip("Hours to fully process one item (if water is not the bottleneck)")]
        public float ProcessTimePerItem = 1f;
    }
}
