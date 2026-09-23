// SPDX-License-Identifier: MIT
// ============================================================================
// Ashfall Core : Expansion 19 — The Bitter Air
// Subsystem    : Chemical Plume Atmospheric Dispersion & Filter Wear Engine
// Authority    : docs/expansions/wave2/expansion_19_the_bitter_air_plan.md
//                UNBLOCK-05_EXPANSION_WAVES_C3_EN_GATE.md §3.1, §5.3
// ============================================================================
using System;

namespace Ashfall.Core.Combat
{
    /// <summary>
    /// Toxicity tier representing biological and respiratory severity of the chemical/biological agent.
    /// </summary>
    public enum PlumeToxicityTier
    {
        Trace = 0,
        Elevated = 1,
        Severe = 2,
        Lethal = 3
    }

    /// <summary>
    /// Shelter indoor air quality classifications.
    /// </summary>
    public enum AirQualityBand
    {
        Pristine = 0,
        Tainted = 1,
        Hazardous = 2,
        CriticalToxicity = 3
    }

    /// <summary>
    /// State of an active drifting chemical/biological plume.
    /// </summary>
    public sealed class ChemicalPlumeState
    {
        public string PlumeId { get; set; } = string.Empty;
        public string AgentId { get; set; } = string.Empty;
        public int SectorX { get; set; }
        public int SectorY { get; set; }
        public int DensityPermille { get; set; } = 500; // 0..1000
        public int RemainingLifespanTicks { get; set; } = 24;
        public PlumeToxicityTier ToxicityTier { get; set; } = PlumeToxicityTier.Elevated;

        public ChemicalPlumeState Clone() => new ChemicalPlumeState
        {
            PlumeId = PlumeId,
            AgentId = AgentId,
            SectorX = SectorX,
            SectorY = SectorY,
            DensityPermille = DensityPermille,
            RemainingLifespanTicks = RemainingLifespanTicks,
            ToxicityTier = ToxicityTier
        };
    }

    /// <summary>
    /// Weather vector providing wind velocity and precipitation conditions for dispersion.
    /// </summary>
    public readonly struct WeatherDispersionVector
    {
        public int WindSpeedKph { get; }
        public int WindDirectionDeg { get; } // 0..359
        public int PrecipitationIntensityPermille { get; } // 0..1000 (rain/snow washout)

        public WeatherDispersionVector(int windSpeedKph, int windDirectionDeg, int precipitationIntensityPermille)
        {
            WindSpeedKph = Math.Max(0, windSpeedKph);
            WindDirectionDeg = Math.Clamp(windDirectionDeg, 0, 359);
            PrecipitationIntensityPermille = Math.Clamp(precipitationIntensityPermille, 0, 1000);
        }
    }

    /// <summary>
    /// Evaluation outcome for shelter indoor air quality and filtration strain.
    /// </summary>
    public readonly struct ShelterAirQualityResult
    {
        public AirQualityBand IndoorAirQuality { get; }
        public int IndoorContaminantDensityPermille { get; }
        public int FilterWearDeltaPermille { get; }
        public bool AirlockBreachWarning { get; }

        public ShelterAirQualityResult(
            AirQualityBand indoorAirQuality,
            int indoorContaminantDensityPermille,
            int filterWearDeltaPermille,
            bool airlockBreachWarning)
        {
            IndoorAirQuality = indoorAirQuality;
            IndoorContaminantDensityPermille = Math.Clamp(indoorContaminantDensityPermille, 0, 1000);
            FilterWearDeltaPermille = Math.Max(0, filterWearDeltaPermille);
            AirlockBreachWarning = airlockBreachWarning;
        }
    }

    /// <summary>
    /// Evaluation outcome for personal gas mask / respirator protection and canister attrition.
    /// </summary>
    public readonly struct RespiratorProtectionResult
    {
        public bool Protected { get; }
        public int CanisterWearDeltaPermille { get; }
        public int EffectiveExposureDose { get; }
        public bool CanisterDepleted { get; }

        public RespiratorProtectionResult(
            bool protectedFromToxicity,
            int canisterWearDeltaPermille,
            int effectiveExposureDose,
            bool canisterDepleted)
        {
            Protected = protectedFromToxicity;
            CanisterWearDeltaPermille = Math.Max(0, canisterWearDeltaPermille);
            EffectiveExposureDose = Math.Max(0, effectiveExposureDose);
            CanisterDepleted = canisterDepleted;
        }
    }

    /// <summary>
    /// Pure domain engine modeling atmospheric plume dispersion, precipitation washout,
    /// shelter air filtration degradation, and respirator canister attrition.
    /// Zero engine references, integer permille determinism.
    /// </summary>
    public static class ChemicalPlumeDispersionEngine
    {
        public const int MaxDensityPermille = 1000;
        public const int BaseDissipationRatePermille = 40; // 4% baseline dissipation per tick

        /// <summary>
        /// Advances atmospheric dispersion and washout for an active plume.
        /// </summary>
        public static void AdvancePlumeDispersion(
            ChemicalPlumeState plume,
            WeatherDispersionVector weather)
        {
            if (plume == null) throw new ArgumentNullException(nameof(plume));
            if (plume.RemainingLifespanTicks <= 0 || plume.DensityPermille <= 0)
            {
                plume.DensityPermille = 0;
                plume.RemainingLifespanTicks = 0;
                return;
            }

            // Wind drift: sector displacement (approximate 8-way compass vector)
            if (weather.WindSpeedKph >= 15)
            {
                // Drift displacement per tick (high wind moves plume across sectors)
                int dir = weather.WindDirectionDeg;
                int dx = 0;
                int dy = 0;

                if (dir >= 338 || dir < 23) { dy = 1; }
                else if (dir >= 23 && dir < 68) { dx = 1; dy = 1; }
                else if (dir >= 68 && dir < 113) { dx = 1; }
                else if (dir >= 113 && dir < 158) { dx = 1; dy = -1; }
                else if (dir >= 158 && dir < 203) { dy = -1; }
                else if (dir >= 203 && dir < 248) { dx = -1; dy = -1; }
                else if (dir >= 248 && dir < 293) { dx = -1; }
                else { dx = -1; dy = 1; }

                plume.SectorX += dx;
                plume.SectorY += dy;
            }

            // Washout: rain and snow accelerate plume decay (up to 150 permille additional decay per tick)
            int washoutDecay = (weather.PrecipitationIntensityPermille * 150) / 1000;

            // Total dissipation: base + precipitation washout
            int totalDissipation = BaseDissipationRatePermille + washoutDecay;
            plume.DensityPermille = Math.Max(0, plume.DensityPermille - totalDissipation);
            plume.RemainingLifespanTicks = Math.Max(0, plume.RemainingLifespanTicks - 1);

            if (plume.DensityPermille == 0)
            {
                plume.RemainingLifespanTicks = 0;
            }
        }

        /// <summary>
        /// Evaluates indoor shelter air infiltration and filter wear under external plume presence.
        /// </summary>
        public static ShelterAirQualityResult EvaluateShelterAirInfiltration(
            int outdoorDensityPermille,
            PlumeToxicityTier toxicity,
            bool isFiltrationPowered,
            int shelterFilterConditionPermille)
        {
            int outdoorDensity = Math.Clamp(outdoorDensityPermille, 0, MaxDensityPermille);
            int filterCondition = Math.Clamp(shelterFilterConditionPermille, 0, 1000);

            if (outdoorDensity == 0)
            {
                return new ShelterAirQualityResult(AirQualityBand.Pristine, 0, 0, false);
            }

            int toxicityMultiplier = (int)toxicity + 1; // 1..4

            if (isFiltrationPowered && filterCondition > 0)
            {
                // Active filtration scrubs up to 95% of contaminant, scaled by filter condition
                int scrubRatePermille = (filterCondition * 950) / 1000;
                int indoorDensity = (outdoorDensity * (1000 - scrubRatePermille)) / 1000;

                // Filter wear delta per tick: scales with outdoor density and toxicity
                long rawWear = ((long)outdoorDensity * toxicityMultiplier * 15) / 1000;
                int filterWear = Math.Clamp((int)rawWear, 1, 100);

                AirQualityBand band = indoorDensity switch
                {
                    < 50 => AirQualityBand.Pristine,
                    < 250 => AirQualityBand.Tainted,
                    < 700 => AirQualityBand.Hazardous,
                    _ => AirQualityBand.CriticalToxicity
                };

                return new ShelterAirQualityResult(band, indoorDensity, filterWear, indoorDensity >= 250);
            }
            else
            {
                // Unpowered or absent filtration: outdoor contaminant breaches ventilation intakes
                int indoorDensity = (outdoorDensity * 800) / 1000; // 80% indoor equilibrium without power

                AirQualityBand band = indoorDensity switch
                {
                    < 50 => AirQualityBand.Pristine,
                    < 250 => AirQualityBand.Tainted,
                    < 700 => AirQualityBand.Hazardous,
                    _ => AirQualityBand.CriticalToxicity
                };

                return new ShelterAirQualityResult(band, indoorDensity, 0, true);
            }
        }

        /// <summary>
        /// Evaluates personal respirator / gas mask canister protection for an individual survivor.
        /// </summary>
        public static RespiratorProtectionResult EvaluateRespiratorProtection(
            int ambientDensityPermille,
            PlumeToxicityTier toxicity,
            int canisterConditionPermille)
        {
            int density = Math.Clamp(ambientDensityPermille, 0, MaxDensityPermille);
            int canister = Math.Clamp(canisterConditionPermille, 0, 1000);

            if (density == 0)
            {
                return new RespiratorProtectionResult(true, 0, 0, false);
            }

            int toxicityMultiplier = (int)toxicity + 1; // 1..4

            if (canister > 0)
            {
                // Canister wears down based on ambient density and toxicity
                long rawWear = ((long)density * toxicityMultiplier * 25) / 1000;
                int wearDelta = Math.Clamp((int)rawWear, 1, 200);
                int remainingCanister = Math.Max(0, canister - wearDelta);
                bool depleted = remainingCanister == 0;

                // If canister has at least 100 permille condition, it filters 100%
                if (canister >= 100)
                {
                    return new RespiratorProtectionResult(true, wearDelta, 0, depleted);
                }
                else
                {
                    // Degraded canister: partial breakthrough
                    int leakagePermille = 100 - canister; // 1..99 permille leak
                    int dose = (density * toxicityMultiplier * leakagePermille) / 1000;
                    return new RespiratorProtectionResult(dose < 10, wearDelta, dose, depleted);
                }
            }
            else
            {
                // No canister: full exposure
                int dose = (density * toxicityMultiplier) / 10;
                return new RespiratorProtectionResult(false, 0, dose, true);
            }
        }
    }
}
