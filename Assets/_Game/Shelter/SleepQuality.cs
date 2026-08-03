using System;
using UnityEngine;
using AtomicWar._Game.Shelter.Modules;

namespace AtomicWar._Game.Shelter
{
    /// <summary>
    /// Ambient inputs for one sleep cycle. Built by AI / tests from shelter,
    /// temperature, power grid, and bed occupancy.
    /// </summary>
    [Serializable]
    public struct SleepConditions
    {
        /// <summary>Indoor temperature °C at the sleep location.</summary>
        public float IndoorTemperatureC;

        /// <summary>Shelter air quality 0..100 (low = high CO2 / foul air).</summary>
        public float AirQuality;

        /// <summary>Diesel CO ppm (Prompt #20 air fouling); high values act as CO2 headache proxy.</summary>
        public float CarbonMonoxidePpm;

        /// <summary>True when a diesel generator is running in the same or adjacent room.</summary>
        public bool DieselNoiseAdjacent;

        /// <summary>0..1 bed comfort; ignored when HasBed is false.</summary>
        public float ComfortLevel;

        /// <summary>False = sleeping on the floor.</summary>
        public bool HasBed;

        /// <summary>Optional room id for diagnostics / UI.</summary>
        public string SleepRoomId;
    }

    /// <summary>
    /// Outcome of one sleep cycle: quality modifier and need deltas.
    /// FatigueRestored is the amount subtracted from Needs.Fatigue.
    /// </summary>
    [Serializable]
    public struct SleepResult
    {
        /// <summary>0..1 environmental sleep quality after all modifiers.</summary>
        public float Quality;

        /// <summary>Fatigue points recovered this cycle.</summary>
        public float FatigueRestored;

        /// <summary>Morale delta (negative = debuff).</summary>
        public float MoraleDelta;

        /// <summary>Health delta (negative = headache from foul air).</summary>
        public float HealthDelta;

        /// <summary>True when sleep occurred on the floor.</summary>
        public bool SleptOnFloor;

        /// <summary>True when diesel noise capped quality.</summary>
        public bool NoiseCapped;

        /// <summary>True when temperature was in the freezing band.</summary>
        public bool Freezing;

        /// <summary>True when atmosphere triggered a health penalty.</summary>
        public bool AtmosphereHeadache;
    }

    /// <summary>
    /// Environmental sleep quality (Prompt #32). Fatigue recovery is no longer flat:
    /// temperature, foul air, diesel noise, and bed comfort reshape each sleep cycle.
    /// </summary>
    public static class SleepQualitySystem
    {
        public const float BaseFatigueRecovery = 60f;

        /// <summary>Diesel generator in adjacent/same room caps quality at this value.</summary>
        public const float NoiseQualityCap = 0.5f;

        /// <summary>Floor sleep multiplies fatigue recovery (50% slower ⇒ ×0.5).</summary>
        public const float FloorRecoveryMultiplier = 0.5f;

        public const float FloorMoralePenalty = -8f;

        /// <summary>At or below this °C, temperature multiplier is FreezingTempMultiplier.</summary>
        public const float FreezingTempC = 0f;

        /// <summary>
        /// Temperature factor at FreezingTempC. Combined with a full bed and diesel
        /// noise (cap 50%), quality stays at 0.3 → 30% of base fatigue recovery.
        /// </summary>
        public const float FreezingTempMultiplier = 0.3f;

        /// <summary>At or above this °C (and below IdealTempMaxC), temperature factor is 1.</summary>
        public const float IdealTempMinC = 12f;

        public const float IdealTempMaxC = 26f;

        /// <summary>Above this, temperature factor softens slightly (stuffy heat).</summary>
        public const float HotTempSoftCapC = 32f;

        public const float HotTempMultiplier = 0.85f;

        /// <summary>Air quality at or below this (or high CO) causes a headache Health hit.</summary>
        public const float HighCo2AirQualityThreshold = 30f;

        public const float HighCo2PpmThreshold = 25f;

        public const float AtmosphereHealthPenalty = -6f;

        public const float PoorSleepQualityThreshold = 0.5f;

        public const float PoorSleepMoralePenalty = -10f;

        public const string DefaultSleepRoomId = "quarters";
        public const string DefaultGeneratorRoomId = "plant";

        /// <summary>
        /// Pure evaluation: quality + deltas. Does not mutate the survivor.
        /// </summary>
        public static SleepResult Evaluate(SleepConditions conditions)
        {
            var result = new SleepResult
            {
                SleptOnFloor = !conditions.HasBed
            };

            float comfort = conditions.HasBed
                ? Mathf.Clamp01(conditions.ComfortLevel > 0f ? conditions.ComfortLevel : 1f)
                : 1f; // floor uses recovery mult, not comfort collapse

            float tempMult = TemperatureMultiplier(conditions.IndoorTemperatureC);
            result.Freezing = conditions.IndoorTemperatureC <= FreezingTempC + 0.001f;

            float quality = Mathf.Clamp01(comfort * tempMult);

            if (conditions.DieselNoiseAdjacent)
            {
                if (quality > NoiseQualityCap)
                {
                    quality = NoiseQualityCap;
                    result.NoiseCapped = true;
                }
                else
                {
                    result.NoiseCapped = true; // noise present even if cold already worse
                }
            }

            result.Quality = Mathf.Clamp01(quality);

            float recovery = BaseFatigueRecovery * result.Quality;
            if (!conditions.HasBed)
            {
                recovery *= FloorRecoveryMultiplier;
            }

            result.FatigueRestored = recovery;

            // Morale: floor always hits; poor quality adds a further debuff
            float morale = 0f;
            if (!conditions.HasBed)
            {
                morale += FloorMoralePenalty;
            }

            if (result.Quality < PoorSleepQualityThreshold)
            {
                morale += PoorSleepMoralePenalty;
            }
            else if (result.Quality >= 0.9f && conditions.HasBed)
            {
                morale += 2f; // small lift for a good night
            }

            result.MoraleDelta = morale;

            // Atmosphere: high CO2 / foul air / diesel CO → headache (Health)
            bool foulAir = conditions.AirQuality <= HighCo2AirQualityThreshold
                || conditions.CarbonMonoxidePpm >= HighCo2PpmThreshold;
            if (foulAir)
            {
                result.HealthDelta = AtmosphereHealthPenalty;
                result.AtmosphereHeadache = true;
            }

            return result;
        }

        /// <summary>
        /// Temperature → quality multiplier. Freezing (≤0°C) → 0.3; ideal band → 1.0.
        /// Linear ramp between FreezingTempC and IdealTempMinC.
        /// </summary>
        public static float TemperatureMultiplier(float indoorTempC)
        {
            if (indoorTempC <= FreezingTempC)
            {
                return FreezingTempMultiplier;
            }

            if (indoorTempC < IdealTempMinC)
            {
                float t = (indoorTempC - FreezingTempC) / (IdealTempMinC - FreezingTempC);
                return Mathf.Lerp(FreezingTempMultiplier, 1f, Mathf.Clamp01(t));
            }

            if (indoorTempC <= IdealTempMaxC)
            {
                return 1f;
            }

            if (indoorTempC >= HotTempSoftCapC)
            {
                return HotTempMultiplier;
            }

            float hotT = (indoorTempC - IdealTempMaxC) / (HotTempSoftCapC - IdealTempMaxC);
            return Mathf.Lerp(1f, HotTempMultiplier, Mathf.Clamp01(hotT));
        }

        /// <summary>
        /// True when a diesel generator is enabled with fuel (engine noise).
        /// </summary>
        public static bool IsDieselGeneratorRunning(PowerNetwork power)
        {
            if (power == null) return false;
            for (int i = 0; i < power.Sources.Count; i++)
            {
                var src = power.Sources[i];
                if (src == null || !src.IsEnabled || src.Fuel <= 0f) continue;
                var def = src.Definition;
                if (def != null && def.Kind == PowerSourceKind.Diesel)
                    return true;
                if (string.Equals(src.SourceId, "diesel_generator", StringComparison.Ordinal))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Diesel noise affects sleep when the generator is running in the same room
        /// or an adjacent room. Missing room ids: treat a running diesel as adjacent
        /// (conservative — undivided bunker still hears it).
        /// </summary>
        public static bool IsDieselNoiseAffectingSleep(
            PowerNetwork power,
            string sleepRoomId,
            Func<string, string, bool> areRoomsAdjacent)
        {
            if (!IsDieselGeneratorRunning(power)) return false;

            string genRoom = null;
            for (int i = 0; i < power.Sources.Count; i++)
            {
                var src = power.Sources[i];
                if (src == null || !src.IsEnabled || src.Fuel <= 0f) continue;
                var def = src.Definition;
                bool diesel = (def != null && def.Kind == PowerSourceKind.Diesel)
                    || string.Equals(src.SourceId, "diesel_generator", StringComparison.Ordinal);
                if (!diesel) continue;
                genRoom = src.RoomId;
                break;
            }

            if (string.IsNullOrEmpty(sleepRoomId) || string.IsNullOrEmpty(genRoom))
            {
                return true;
            }

            if (string.Equals(sleepRoomId, genRoom, StringComparison.Ordinal))
            {
                return true;
            }

            return areRoomsAdjacent != null && areRoomsAdjacent(sleepRoomId, genRoom);
        }

        /// <summary>
        /// Find an operational bed with free capacity. Binds definition when present.
        /// Increments Occupancy on the chosen instance. Returns false → floor sleep.
        /// </summary>
        public static bool TryClaimBed(
            Shelter shelter,
            out float comfortLevel,
            out string bedRoomId)
        {
            comfortLevel = 0f;
            bedRoomId = null;
            if (shelter?.Modules == null) return false;

            for (int i = 0; i < shelter.Modules.Count; i++)
            {
                var mod = shelter.Modules[i];
                if (mod == null || !mod.IsOperational) continue;

                int capacity = 1;
                float comfort = 1f;
                bool isBed = false;

                if (mod.Definition is BedModuleSO bedSO)
                {
                    isBed = true;
                    capacity = Mathf.Max(1, bedSO.Capacity);
                    comfort = Mathf.Clamp01(bedSO.ComfortLevel);
                }
                else if (string.Equals(mod.ModuleId, BedModuleSO.DefaultModuleId, StringComparison.Ordinal)
                         || string.Equals(mod.ModuleId, "sleeping_bunk", StringComparison.Ordinal))
                {
                    isBed = true;
                    capacity = Mathf.Max(1, mod.Capacity > 0 ? mod.Capacity : 1);
                    comfort = mod.ComfortLevel > 0f ? Mathf.Clamp01(mod.ComfortLevel) : 1f;
                }

                if (!isBed) continue;
                if (mod.Occupancy >= capacity) continue;

                mod.Occupancy++;
                comfortLevel = comfort;
                bedRoomId = string.IsNullOrEmpty(mod.RoomId) ? DefaultSleepRoomId : mod.RoomId;
                return true;
            }

            return false;
        }

        /// <summary>Clear bed occupancy at the start of a sleep evaluation wave.</summary>
        public static void ResetBedOccupancy(Shelter shelter)
        {
            if (shelter?.Modules == null) return;
            for (int i = 0; i < shelter.Modules.Count; i++)
            {
                var mod = shelter.Modules[i];
                if (mod == null) continue;
                if (mod.Definition is BedModuleSO
                    || string.Equals(mod.ModuleId, BedModuleSO.DefaultModuleId, StringComparison.Ordinal)
                    || string.Equals(mod.ModuleId, "sleeping_bunk", StringComparison.Ordinal))
                {
                    mod.Occupancy = 0;
                }
            }
        }

        /// <summary>
        /// Build conditions from live systems for a survivor about to sleep.
        /// </summary>
        public static SleepConditions BuildConditions(
            Shelter shelter,
            PowerNetwork power,
            float indoorTemperatureC,
            string preferredSleepRoomId = null,
            Func<string, string, bool> areRoomsAdjacent = null)
        {
            bool hasBed = TryClaimBed(shelter, out float comfort, out string bedRoom);
            string sleepRoom = !string.IsNullOrEmpty(bedRoom)
                ? bedRoom
                : (preferredSleepRoomId ?? DefaultSleepRoomId);

            float air = shelter != null ? shelter.AirQuality : 100f;
            float co = power != null ? power.CarbonMonoxidePpm : 0f;

            Func<string, string, bool> adjacent = areRoomsAdjacent;
            if (adjacent == null && shelter != null)
            {
                adjacent = shelter.AreRoomsAdjacent;
            }

            bool noise = IsDieselNoiseAffectingSleep(power, sleepRoom, adjacent);

            return new SleepConditions
            {
                IndoorTemperatureC = indoorTemperatureC,
                AirQuality = air,
                CarbonMonoxidePpm = co,
                DieselNoiseAdjacent = noise,
                ComfortLevel = comfort,
                HasBed = hasBed,
                SleepRoomId = sleepRoom
            };
        }
    }
}
