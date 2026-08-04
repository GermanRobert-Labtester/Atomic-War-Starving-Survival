using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Environment;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Bicycle Logistics (Prompt #68). Equipping a bicycle halves travel time
    /// but requires clear weather. Degrades per expedition tick. If it breaks
    /// mid-expedition, carrying capacity halves and survivor must walk back or
    /// abandon the bike. Requires TirePatchKit for repairs.
    /// Save/load safe through ExpeditionState. Plain C#.
    /// </summary>
    public class BicycleSystem
    {
        public const string BicycleItemId = "bicycle";
        public const string TirePatchKitItemId = "tire_patch_kit";

        /// <summary>Bicycle base durability (game-hours of travel before breakdown).</summary>
        public const float BicycleMaxDurability = 40f;

        /// <summary>Travel time multiplier while bicycle is active.</summary>
        public const float BicycleSpeedMultiplier = 0.5f; // half travel time

        /// <summary>Durability lost per hour of bicycle travel.</summary>
        public const float BicycleDegradePerHour = 1f;

        /// <summary>Durability restored by one TirePatchKit.</summary>
        public const float PatchKitRepairAmount = 25f;

        /// <summary>Carrying capacity multiplier when bicycle breaks.</summary>
        public const float BreakdownCarryMultiplier = 0.5f;

        /// <summary>Fatigue penalty from walking a broken bike back.</summary>
        public const float WalkBackFatiguePerHour = 3f;

        /// <summary>Weather kinds that block bicycle use.</summary>
        public static readonly HashSet<WeatherKind> BlockedWeather = new HashSet<WeatherKind>
        {
            WeatherKind.Blizzard,
            WeatherKind.FalloutStorm,
            WeatherKind.BlackRain
        };

        // -- Events --
        public event Action<ExpeditionState> OnBicycleEquipped;
        public event Action<ExpeditionState> OnBicycleBroken;
        public event Action<ExpeditionState> OnBicycleRepaired;

        public BicycleSystem() { }

        /// <summary>Whether a bicycle can be used in current weather.</summary>
        public static bool CanUseBicycle(WeatherKind weather)
        {
            return !BlockedWeather.Contains(weather);
        }

        /// <summary>
        /// Equip a bicycle for this expedition. Must be called before travel begins.
        /// </summary>
        public bool EquipBicycle(ExpeditionState exp)
        {
            if (exp == null) return false;
            if (exp.BicycleDurability > 0f) return false; // Already equipped.

            exp.BicycleDurability = BicycleMaxDurability;
            exp.HasBicycle = true;
            OnBicycleEquipped?.Invoke(exp);
            return true;
        }

        /// <summary>
        /// Tick bicycle degradation and check for breakdown. Called each expedition tick.
        /// Returns true if the bicycle is still functional after this tick.
        /// </summary>
        public bool TickBicycle(ExpeditionState exp, float travelHours, WeatherKind weather)
        {
            if (exp == null || !exp.HasBicycle || exp.BicycleDurability <= 0f) return false;

            // Weather blocks bicycle use — survivor walks instead, no degradation.
            if (!CanUseBicycle(weather)) return false;

            exp.BicycleDurability = Mathf.Max(0f, exp.BicycleDurability - BicycleDegradePerHour * travelHours);

            if (exp.BicycleDurability <= 0f)
            {
                BreakdownBicycle(exp);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Get effective travel speed multiplier considering bicycle state and weather.
        /// </summary>
        public static float GetSpeedMultiplier(ExpeditionState exp, WeatherKind weather)
        {
            if (exp == null || !exp.HasBicycle || exp.BicycleDurability <= 0f)
                return 1f;
            if (!CanUseBicycle(weather))
                return 1f;
            return BicycleSpeedMultiplier;
        }

        private void BreakdownBicycle(ExpeditionState exp)
        {
            exp.HasBicycle = false;
            // Halve carrying capacity.
            exp.CarryingCapacity *= BreakdownCarryMultiplier;
            // Drop excess loot if over new capacity.
            if (exp.CurrentWeight > exp.CarryingCapacity)
            {
                float excess = exp.CurrentWeight - exp.CarryingCapacity;
                exp.DropLoot(Mathf.Clamp01(excess / Mathf.Max(1f, exp.CurrentWeight)));
            }
            // Fatigue penalty for walking a broken bike.
            if (exp.Survivor != null)
            {
                exp.Survivor.Needs.Fatigue = Mathf.Clamp(
                    exp.Survivor.Needs.Fatigue + WalkBackFatiguePerHour * 2f, 0f, 100f);
            }
            OnBicycleBroken?.Invoke(exp);
        }

        /// <summary>
        /// Repair the bicycle using a TirePatchKit. Can be done mid-expedition or at base.
        /// </summary>
        public bool RepairBicycle(ExpeditionState exp)
        {
            if (exp == null) return false;
            if (exp.BicycleDurability >= BicycleMaxDurability) return false;

            exp.BicycleDurability = Mathf.Min(BicycleMaxDurability,
                exp.BicycleDurability + PatchKitRepairAmount);
            exp.HasBicycle = true;
            OnBicycleRepaired?.Invoke(exp);
            return true;
        }
    }
}
