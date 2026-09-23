using System;

namespace Ashfall.Core.Expeditions
{
    /// <summary>
    /// Flight weather window condition classification.
    /// </summary>
    public enum FlightWindowCondition
    {
        Optimal = 0,    // Clear skies, mild winds, optimal visibility
        Marginal = 1,   // High winds or light ash/fog, elevated fuel burn and risk
        Hazardous = 2,  // Severe wind shear, radioactive dust storm, or icing
        Grounded = 3    // Impossible flight conditions (gale, blackout storm, zero visibility)
    }

    /// <summary>
    /// Immutable evaluation result produced by <see cref="AerialReconWindowEngine"/>.
    /// </summary>
    public readonly struct FlightWindowEvaluationResult
    {
        public FlightWindowCondition Condition { get; }
        public bool IsLaunchPermitted { get; }
        public int WindShearRiskPermille { get; }
        public int VisibilityRiskPermille { get; }
        public int IcingRiskPermille { get; }
        public int TotalFlightRiskPermille { get; }
        public int EffectiveRangeKm { get; }
        public int AirdropDriftMeters { get; }
        public int AirworthinessWearPermille { get; }
        public string Advisory { get; }

        public FlightWindowEvaluationResult(
            FlightWindowCondition condition,
            bool isLaunchPermitted,
            int windShearRiskPermille,
            int visibilityRiskPermille,
            int icingRiskPermille,
            int totalFlightRiskPermille,
            int effectiveRangeKm,
            int airdropDriftMeters,
            int airworthinessWearPermille,
            string advisory)
        {
            Condition = condition;
            IsLaunchPermitted = isLaunchPermitted;
            WindShearRiskPermille = windShearRiskPermille;
            VisibilityRiskPermille = visibilityRiskPermille;
            IcingRiskPermille = icingRiskPermille;
            TotalFlightRiskPermille = totalFlightRiskPermille;
            EffectiveRangeKm = effectiveRangeKm;
            AirdropDriftMeters = airdropDriftMeters;
            AirworthinessWearPermille = airworthinessWearPermille;
            Advisory = advisory ?? string.Empty;
        }
    }

    /// <summary>
    /// Pure domain engine for aerial reconnaissance flight windows, risk modeling,
    /// and airdrop drift calculations (Expansion 14: Above the Ash).
    /// Operates without engine dependencies or duplicate flight physics.
    /// </summary>
    public static class AerialReconWindowEngine
    {
        public const int PermilleScale = 1000;

        /// <summary>
        /// Evaluates flight safety, effective range, wear, and airdrop drift for a planned aerial mission.
        /// </summary>
        /// <param name="baseRangeKm">Aircraft rated nominal flight range.</param>
        /// <param name="airworthinessPermille">Current aircraft structural integrity (0..1000).</param>
        /// <param name="windSpeedKmh">Ambient wind speed in km/h.</param>
        /// <param name="visibilityPermille">Atmospheric clarity (1000 = clear, 0 = blackout ash storm).</param>
        /// <param name="temperatureCelsius">Atmospheric temperature at altitude.</param>
        /// <param name="payloadWeightKg">Cargo/sensor payload weight in kg.</param>
        /// <param name="maxPayloadKg">Maximum rated payload capacity in kg.</param>
        /// <returns>Immutable <see cref="FlightWindowEvaluationResult"/>.</returns>
        public static FlightWindowEvaluationResult Evaluate(
            int baseRangeKm,
            int airworthinessPermille,
            int windSpeedKmh,
            int visibilityPermille,
            int temperatureCelsius,
            int payloadWeightKg,
            int maxPayloadKg)
        {
            if (baseRangeKm < 0) baseRangeKm = 0;
            airworthinessPermille = Math.Max(0, Math.Min(PermilleScale, airworthinessPermille));
            windSpeedKmh = Math.Max(0, windSpeedKmh);
            visibilityPermille = Math.Max(0, Math.Min(PermilleScale, visibilityPermille));
            payloadWeightKg = Math.Max(0, payloadWeightKg);
            maxPayloadKg = Math.Max(1, maxPayloadKg);

            // 1. Wind shear risk calculation
            int windRisk;
            if (windSpeedKmh < 25)
            {
                windRisk = (windSpeedKmh * 4); // 0..100 permille
            }
            else if (windSpeedKmh < 60)
            {
                windRisk = 100 + ((windSpeedKmh - 25) * 12); // 100..520 permille
            }
            else
            {
                windRisk = 520 + ((windSpeedKmh - 60) * 25); // severe above 60 km/h
            }
            windRisk = Math.Min(PermilleScale, windRisk);

            // 2. Visibility risk (ash, dust, precipitation)
            int visRisk = (PermilleScale - visibilityPermille) / 2; // up to 500 permille

            // 3. Icing risk (freezing temperatures near/below 0°C with high moisture/cloud)
            int icingRisk = 0;
            if (temperatureCelsius <= 2 && temperatureCelsius >= -15)
            {
                // Peak icing zone between -5 and 0
                icingRisk = 250 - (Math.Abs(temperatureCelsius + 3) * 20);
                if (icingRisk < 50) icingRisk = 50;
            }
            else if (temperatureCelsius < -15)
            {
                icingRisk = 80; // extreme dry cold lower accretion
            }

            // Airworthiness structural risk penalty
            int structuralRisk = 0;
            if (airworthinessPermille < 500)
            {
                structuralRisk = (500 - airworthinessPermille) * 2; // up to 1000 permille
            }

            // Total risk aggregation
            int totalRisk = Math.Min(PermilleScale, (windRisk * 350 + visRisk * 250 + icingRisk * 200 + structuralRisk * 200) / PermilleScale);

            // Flight window classification
            FlightWindowCondition condition;
            bool launchPermitted = true;
            string advisory;

            if (windSpeedKmh >= 80 || visibilityPermille < 150 || airworthinessPermille < 200 || totalRisk >= 750)
            {
                condition = FlightWindowCondition.Grounded;
                launchPermitted = false;
                advisory = "Extreme meteorological hazard or critical structural degradation. Flight aborted.";
            }
            else if (totalRisk >= 450 || windSpeedKmh >= 50 || visibilityPermille < 400 || icingRisk >= 150)
            {
                condition = FlightWindowCondition.Hazardous;
                launchPermitted = true;
                advisory = "Hazardous flight envelope. High probability of navigation drift, icing, or structural failure.";
            }
            else if (totalRisk >= 200 || windSpeedKmh >= 30 || visibilityPermille < 700)
            {
                condition = FlightWindowCondition.Marginal;
                launchPermitted = true;
                advisory = "Marginal flight envelope. Mild turbulence and reduced sensor fidelity.";
            }
            else
            {
                condition = FlightWindowCondition.Optimal;
                launchPermitted = true;
                advisory = "Optimal flight window. Stable winds and clear aerial reconnaissance lanes.";
            }

            // Effective range with payload and wind drag
            int payloadFractionPermille = Math.Min(PermilleScale, (payloadWeightKg * PermilleScale) / maxPayloadKg);
            // Payload reduces range up to 40% (400 permille)
            int payloadRangePenalty = (payloadFractionPermille * 400) / PermilleScale;
            // Headwind/turbulence penalty reduces range up to 25% (250 permille)
            int windRangePenalty = Math.Min(250, (windSpeedKmh * 4));
            int totalRangePenaltyPermille = Math.Min(600, payloadRangePenalty + windRangePenalty);

            int effectiveRangeKm = (baseRangeKm * (PermilleScale - totalRangePenaltyPermille)) / PermilleScale;

            // Airdrop drift calculation (meters): wind speed * altitude/descent factor
            // Baseline 10m drift per 5 km/h wind, magnified in poor visibility
            int drift = (windSpeedKmh * 20) + ((PermilleScale - visibilityPermille) / 5);
            if (drift < 15) drift = 15;

            // Airworthiness wear per sortie
            // Base wear 20 permille + turbulence + icing
            int wear = 20 + (windSpeedKmh / 2) + (icingRisk / 10);
            if (!launchPermitted) wear = 0;

            return new FlightWindowEvaluationResult(
                condition: condition,
                isLaunchPermitted: launchPermitted,
                windShearRiskPermille: windRisk,
                visibilityRiskPermille: visRisk,
                icingRiskPermille: icingRisk,
                totalFlightRiskPermille: totalRisk,
                effectiveRangeKm: effectiveRangeKm,
                airdropDriftMeters: drift,
                airworthinessWearPermille: wear,
                advisory: advisory
            );
        }
    }
}
