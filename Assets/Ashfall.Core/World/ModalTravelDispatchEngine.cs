// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.World
{
    public enum TravelModality
    {
        FootExcursion = 1,
        GroundConvoy = 2,
        AmphibiousRig = 3,
        AerialReconFlight = 4
    }

    public sealed class ModalTravelDispatchResult
    {
        public bool CanDispatch { get; }
        public TravelModality Modality { get; }
        public int DistanceKm { get; }
        public int EstimatedDurationHours { get; }
        public int FuelRequiredUnits { get; }
        public int WeatherHazardRiskPermille { get; }
        public int TerrainAttritionRiskPermille { get; }
        public string RefusalReason { get; }
        public string Summary { get; }

        public ModalTravelDispatchResult(
            bool canDispatch,
            TravelModality modality,
            int distanceKm,
            int estimatedDurationHours,
            int fuelRequiredUnits,
            int weatherHazardRiskPermille,
            int terrainAttritionRiskPermille,
            string refusalReason,
            string summary)
        {
            CanDispatch = canDispatch;
            Modality = modality;
            DistanceKm = Math.Max(0, distanceKm);
            EstimatedDurationHours = Math.Max(0, estimatedDurationHours);
            FuelRequiredUnits = Math.Max(0, fuelRequiredUnits);
            WeatherHazardRiskPermille = Math.Clamp(weatherHazardRiskPermille, 0, 1000);
            TerrainAttritionRiskPermille = Math.Clamp(terrainAttritionRiskPermille, 0, 1000);
            RefusalReason = refusalReason ?? string.Empty;
            Summary = summary ?? string.Empty;
        }

        public static ModalTravelDispatchResult Refused(TravelModality modality, int distanceKm, string refusalReason)
        {
            return new ModalTravelDispatchResult(
                canDispatch: false,
                modality: modality,
                distanceKm: distanceKm,
                estimatedDurationHours: 0,
                fuelRequiredUnits: 0,
                weatherHazardRiskPermille: 1000,
                terrainAttritionRiskPermille: 1000,
                refusalReason: refusalReason ?? "Dispatch refused.",
                summary: $"Dispatch refused: {refusalReason}"
            );
        }
    }

    /// <summary>
    /// L-P32R / UNBLOCK-04 §2.14 / §5.12: Aviation & Amphibious Wasteland Travel Modal Engine.
    /// Unifies multi-modal travel across wasteland graph routes (Foot, GroundConvoy, AmphibiousRig, AerialReconFlight).
    /// Evaluates route impassability, modal speed multipliers, fuel consumption, and terrain/weather risks.
    /// Engine-free, integer/permille determinism.
    /// </summary>
    public static class ModalTravelDispatchEngine
    {
        public const int MinAerialWeatherWindowPermille = 400; // Below 400 flights are grounded
        public const int CriticalVehicleConditionPermille = 200; // Below 200 vehicles refuse dispatch

        public static (int baseSpeedPermille, int fuelPerKmPermille) GetModalityConstants(TravelModality modality)
        {
            return modality switch
            {
                TravelModality.AerialReconFlight => (6000, 250), // 30 km/h, 0.25 units/km
                TravelModality.GroundConvoy => (2000, 80),      // 10 km/h, 0.08 units/km
                TravelModality.AmphibiousRig => (1400, 120),    // 7 km/h, 0.12 units/km
                TravelModality.FootExcursion => (1000, 0),      // 5 km/h, 0 units/km
                _ => (1000, 0)
            };
        }

        public static ModalTravelDispatchResult EvaluateDispatch(
            MapRoute? route,
            TravelModality modality,
            int vehicleConditionPermille = 1000,
            int fuelAvailableUnits = 0,
            int weatherWindowPermille = 1000)
        {
            if (route == null)
            {
                return ModalTravelDispatchResult.Refused(modality, 0, "Route definition is null.");
            }

            int distance = Math.Max(1, (int)Math.Round(route.DistanceKm));
            vehicleConditionPermille = Math.Clamp(vehicleConditionPermille, 0, 1000);
            weatherWindowPermille = Math.Clamp(weatherWindowPermille, 0, 1000);

            // Vehicle condition check
            if (modality != TravelModality.FootExcursion && vehicleConditionPermille < CriticalVehicleConditionPermille)
            {
                return ModalTravelDispatchResult.Refused(modality, distance, "Vehicle condition is critical (<20%); unsafe for wasteland transit.");
            }

            bool isFlooded = route.IsFlooded || (route.Tags != null && route.Tags.Contains("flooded"));
            bool isMountain = route.Tags != null && route.Tags.Contains("mountain");

            // Modality terrain & weather gates
            switch (modality)
            {
                case TravelModality.FootExcursion:
                    if (isFlooded)
                    {
                        return ModalTravelDispatchResult.Refused(modality, distance, "Deep water flood hazard impassable on foot without amphibious transport.");
                    }
                    break;

                case TravelModality.GroundConvoy:
                    if (isFlooded)
                    {
                        return ModalTravelDispatchResult.Refused(modality, distance, "Ground vehicle cannot cross flooded route without amphibious rig.");
                    }
                    break;

                case TravelModality.AmphibiousRig:
                    if (isMountain)
                    {
                        return ModalTravelDispatchResult.Refused(modality, distance, "Amphibious rig cannot traverse steep mountain cliffs.");
                    }
                    break;

                case TravelModality.AerialReconFlight:
                    if (weatherWindowPermille < MinAerialWeatherWindowPermille)
                    {
                        return ModalTravelDispatchResult.Refused(modality, distance, "Flight grounded due to severe atmospheric hazard / storm window.");
                    }
                    break;
            }

            var (baseSpeed, fuelPerKm) = GetModalityConstants(modality);

            // Fuel requirement
            int fuelRequired = 0;
            if (fuelPerKm > 0)
            {
                fuelRequired = Math.Max(1, (distance * fuelPerKm + 999) / 1000);
                if (fuelAvailableUnits < fuelRequired)
                {
                    return ModalTravelDispatchResult.Refused(modality, distance, $"Insufficient fuel: required {fuelRequired} units, available {fuelAvailableUnits} units.");
                }
            }

            // Duration calculation in hours
            // Baseline 5 km/h on foot: duration = distance / 5 = (distance * 1000) / 5000 = (distance * 1000) / baseSpeed
            // Vehicle condition modifies speed down to 60% if condition is degraded
            int effectiveSpeedPermille = baseSpeed;
            if (modality != TravelModality.FootExcursion)
            {
                int efficiency = 600 + (vehicleConditionPermille * 400) / 1000;
                effectiveSpeedPermille = Math.Max(500, (baseSpeed * efficiency) / 1000);
            }

            // Duration in hours = Math.Max(1, (distance * 5000) / effectiveSpeedPermille)
            int durationHours = Math.Max(1, (distance * 5000) / effectiveSpeedPermille);

            // Hazard and attrition risks
            int weatherRisk = Math.Clamp((int)(route.WeatherHazard * 1000), 0, 1000);
            if (modality == TravelModality.AerialReconFlight)
            {
                weatherRisk = Math.Clamp(1000 - weatherWindowPermille, 50, 1000);
            }

            int attritionRisk = 50;
            if (isMountain) attritionRisk += 100;
            if (vehicleConditionPermille < 500) attritionRisk += 150;
            if (weatherRisk > 500) attritionRisk += 100;
            attritionRisk = Math.Clamp(attritionRisk, 0, 1000);

            string summary = $"{modality} dispatch to route ({distance} km): ~{durationHours}h transit, {fuelRequired} fuel units, {attritionRisk / 10}% attrition risk.";

            return new ModalTravelDispatchResult(
                canDispatch: true,
                modality: modality,
                distanceKm: distance,
                estimatedDurationHours: durationHours,
                fuelRequiredUnits: fuelRequired,
                weatherHazardRiskPermille: weatherRisk,
                terrainAttritionRiskPermille: attritionRisk,
                refusalReason: string.Empty,
                summary: summary
            );
        }
    }
}
