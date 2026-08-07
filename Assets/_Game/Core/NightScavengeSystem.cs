using System;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Night Scavenging (Prompt #70). TimeOfDay modifiers for expeditions.
    /// Scavenging at night reduces radiation exposure by 10% and avoids Faction
    /// snipers. Requires Flashlight and Batteries. Without light, Encounter risk
    /// triples and loot find chance halves. Save/load safe through ExpeditionState.
    /// Plain C#.
    /// </summary>
    
    [Serializable]
    public class NightScavengeSystemSave
    {
        public string systemId = "night_scavenge_system";
    }
public class NightScavengeSystem
    {
        public const string FlashlightItemId = "flashlight";
        public const string BatteryItemId = "battery";

        /// <summary>Radiation exposure multiplier at night.</summary>
        public const float NightRadiationMultiplier = 0.9f;

        /// <summary>Encounter risk multiplier without flashlight at night.</summary>
        public const float DarkEncounterRiskMultiplier = 3f;

        /// <summary>Loot find multiplier without flashlight at night.</summary>
        public const float DarkLootFindMultiplier = 0.5f;

        /// <summary>Battery consumed per hour of flashlight use.</summary>
        public const float FlashlightBatteryDrainPerHour = 1f;

        /// <summary>Flashlight battery capacity in game-hours.</summary>
        public const float FlashlightBatteryCapacity = 8f;

        /// <summary>Whether the current game hour counts as "night" (18:00-06:00).</summary>
        public static bool IsNightTime(int currentHour)
        {
            return currentHour >= 18 || currentHour < 6;
        }

        /// <summary>
        /// Prepare an expedition for night scavenging. Checks flashlight + battery
        /// availability. Returns true if night-ready.
        /// </summary>
        public static bool PrepareNightScavenge(
            ExpeditionState exp,
            Func<string, int> countItem,
            Func<string, int, bool> consumeItem)
        {
            if (exp == null) return false;

            exp.IsNightScavenge = true;

            // Check for flashlight.
            if (countItem != null && countItem(FlashlightItemId) > 0)
            {
                exp.HasFlashlight = true;
                // Consume 1 battery to power the flashlight.
                if (consumeItem != null && countItem(BatteryItemId) > 0)
                {
                    consumeItem(BatteryItemId, 1);
                    exp.FlashlightBattery = FlashlightBatteryCapacity;
                }
                else
                {
                    // Flashlight without battery = dead weight.
                    exp.HasFlashlight = false;
                }
            }

            return exp.HasFlashlight;
        }

        /// <summary>
        /// Tick flashlight battery drain during night expedition.
        /// </summary>
        public static void TickFlashlight(ExpeditionState exp, float tickHours)
        {
            if (exp == null || !exp.IsNightScavenge || !exp.HasFlashlight) return;
            if (exp.FlashlightBattery <= 0f)
            {
                exp.HasFlashlight = false;
                return;
            }
            exp.FlashlightBattery = Mathf.Max(0f,
                exp.FlashlightBattery - FlashlightBatteryDrainPerHour * tickHours);
            if (exp.FlashlightBattery <= 0f)
                exp.HasFlashlight = false;
        }

        /// <summary>
        /// Get encounter risk multiplier for night scavenging.
        /// </summary>
        public static float GetEncounterRiskMultiplier(ExpeditionState exp)
        {
            if (exp == null || !exp.IsNightScavenge) return 1f;
            return exp.HasFlashlight ? 1f : DarkEncounterRiskMultiplier;
        }

        /// <summary>
        /// Get loot find multiplier for night scavenging.
        /// </summary>
        public static float GetLootFindMultiplier(ExpeditionState exp)
        {
            if (exp == null || !exp.IsNightScavenge) return 1f;
            return exp.HasFlashlight ? 1f : DarkLootFindMultiplier;
        }

        /// <summary>
        /// Get radiation multiplier for night scavenging.
        /// </summary>
        public static float GetRadiationMultiplier(ExpeditionState exp)
        {
            if (exp == null || !exp.IsNightScavenge) return 1f;
            return NightRadiationMultiplier;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public NightScavengeSystemSave CaptureState() => new NightScavengeSystemSave();

        public void RestoreState(NightScavengeSystemSave saved) { _ = saved; }

}
}
