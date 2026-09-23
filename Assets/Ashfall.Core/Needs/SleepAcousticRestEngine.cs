// SPDX-License-Identifier: MIT
// ============================================================================
// Ashfall Core : Expansion 41 — The Quiet
// Subsystem    : Sleep Quality, Soundproofing & Shelter Crowding Engine
// Authority    : docs/expansions/wave6/expansion_41_the_quiet_plan.md
//                WAVE6_INDEX.md
// ============================================================================
using System;

namespace Ashfall.Core.Needs
{
    /// <summary>
    /// Classification band of acoustic and physical sleeping environment.
    /// </summary>
    public enum SleepEnvironmentBand
    {
        Unbearable    = 0, // Industrial noise/severe crowding; near-zero restorative sleep
        Disturbed     = 1, // Frequent noise spikes, heavy bunk crowding; partial recovery
        Adequate      = 2, // Standard shelter dorm; baseline recovery with modest fatigue clearing
        Restful       = 3, // Soundproofed, low density; enhanced restorative sleep
        DeepSanctuary = 4  // Acoustic isolation, private dark-rest room; maximum restorative sleep
    }

    /// <summary>
    /// Status of compliance with shelter quiet hours (22:00 to 06:00).
    /// </summary>
    public enum QuietHoursCompliance
    {
        ViolatedSevere = 0, // Heavy machinery, shouting, alarms in sleeping sector
        ViolatedMinor  = 1, // Foot traffic, quiet whispering, low hum
        Compliant      = 2  // Dark, silent hours observed
    }

    /// <summary>
    /// Mutable state record of a shelter sleeping quarter or rest space.
    /// Extends ShelterNoiseSystem, NeedsSystem, and ShelterAssignmentSystem without creating parallel need stores.
    /// </summary>
    public sealed class SleepingQuarterState
    {
        public string RoomId                        { get; set; } = string.Empty;
        public int RoomAreaSquareMetres             { get; set; } = 20;
        public int AssignedOccupants                { get; set; } = 4;
        public int WallSoundproofingPermille        { get; set; } = 600;
        public int DoorSoundproofingPermille        { get; set; } = 500;
        public int AmbientNoiseLevelDecibels        { get; set; } = 55;
        public int DarknessQualityPermille          { get; set; } = 750;
        public bool HasSensoryReliefKit             { get; set; } = false;

        public SleepingQuarterState Clone() => new SleepingQuarterState
        {
            RoomId                   = RoomId,
            RoomAreaSquareMetres     = RoomAreaSquareMetres,
            AssignedOccupants        = AssignedOccupants,
            WallSoundproofingPermille = WallSoundproofingPermille,
            DoorSoundproofingPermille = DoorSoundproofingPermille,
            AmbientNoiseLevelDecibels = AmbientNoiseLevelDecibels,
            DarknessQualityPermille  = DarknessQualityPermille,
            HasSensoryReliefKit      = HasSensoryReliefKit
        };
    }

    /// <summary>
    /// Immutable result of a nocturnal sleep evaluation.
    /// </summary>
    public readonly struct SleepQualityResult
    {
        public int SleepQualityIndexPermille             { get; }
        public SleepEnvironmentBand EnvironmentBand       { get; }
        public int NetDecibelsAtBunk                     { get; }
        public int CrowdingPenaltyPermille               { get; }
        public int FatigueRestorationMultiplierPermille   { get; }
        public int MoraleRestorationBonus                { get; }

        public SleepQualityResult(
            int sleepQualityIndexPermille,
            SleepEnvironmentBand environmentBand,
            int netDecibelsAtBunk,
            int crowdingPenaltyPermille,
            int fatigueRestorationMultiplierPermille,
            int moraleRestorationBonus)
        {
            SleepQualityIndexPermille            = Math.Clamp(sleepQualityIndexPermille, 0, 1000);
            EnvironmentBand                      = environmentBand;
            NetDecibelsAtBunk                    = Math.Max(0, netDecibelsAtBunk);
            CrowdingPenaltyPermille              = Math.Clamp(crowdingPenaltyPermille, 0, 1000);
            FatigueRestorationMultiplierPermille = Math.Clamp(fatigueRestorationMultiplierPermille, 100, 2000);
            MoraleRestorationBonus               = moraleRestorationBonus;
        }
    }

    /// <summary>
    /// Pure domain engine governing sleep quality evaluation, acoustic decibel attenuation,
    /// quiet hours compliance, sleeping quarter crowding penalties, and fatigue restoration multipliers.
    /// Extends ShelterNoiseSystem and NeedsSystem seams.
    /// Zero engine references, integer permille determinism.
    /// </summary>
    public static class SleepAcousticRestEngine
    {
        public const int IdealSleepDecibels = 35;
        public const int MaxTolerableSleepDecibels = 75;

        /// <summary>
        /// Calculates net decibels reaching the bunk after room soundproofing attenuation.
        /// </summary>
        public static int CalculateAcousticAttenuation(
            int sourceDecibels,
            int wallSoundproofingPermille,
            int doorSoundproofingPermille)
        {
            sourceDecibels            = Math.Max(0, sourceDecibels);
            wallSoundproofingPermille = Math.Clamp(wallSoundproofingPermille, 0, 1000);
            doorSoundproofingPermille = Math.Clamp(doorSoundproofingPermille, 0, 1000);

            // Combined soundproofing attenuation (max ~40 dB reduction for high-grade baffling)
            int effectiveProofing = (wallSoundproofingPermille * 70 + doorSoundproofingPermille * 30) / 100;
            int attenuationDb = (effectiveProofing * 40) / 1000;

            return Math.Max(20, sourceDecibels - attenuationDb);
        }

        /// <summary>
        /// Evaluates crowding density penalty based on area per occupant.
        /// </summary>
        public static int CalculateCrowdingDensity(int occupants, int roomAreaSqMetres)
        {
            occupants        = Math.Max(1, occupants);
            roomAreaSqMetres = Math.Max(1, roomAreaSqMetres);

            // Square decimetres per person
            int areaPerPersonSqM = (roomAreaSqMetres * 10) / occupants;

            // Ideal: >= 5.0 sq metres per person (50 in scaled units). Under 2.0 sq metres is acute crowding
            if (areaPerPersonSqM >= 50) return 0;
            if (areaPerPersonSqM <= 15) return 800;

            int deficit = 50 - areaPerPersonSqM;
            return Math.Clamp((deficit * 800) / 35, 0, 800);
        }

        /// <summary>
        /// Evaluates comprehensive sleep quality and restorative modifiers for an occupant.
        /// </summary>
        public static SleepQualityResult EvaluateSleepQuality(
            SleepingQuarterState quarter,
            bool isQuietHours,
            QuietHoursCompliance compliance,
            int hoursSlept)
        {
            if (quarter == null) throw new ArgumentNullException(nameof(quarter));

            hoursSlept = Math.Clamp(hoursSlept, 1, 16);

            // Net acoustic level inside room
            int netDecibels = CalculateAcousticAttenuation(
                quarter.AmbientNoiseLevelDecibels,
                quarter.WallSoundproofingPermille,
                quarter.DoorSoundproofingPermille);

            // Acoustic penalty: decibels over 35 reduce sleep score
            int noiseDelta = Math.Max(0, netDecibels - IdealSleepDecibels);
            int noisePenalty = Math.Clamp(noiseDelta * 18, 0, 800);

            // Crowding penalty
            int crowdingPenalty = CalculateCrowdingDensity(quarter.AssignedOccupants, quarter.RoomAreaSquareMetres);

            // Quiet hours modifier
            int complianceScore = compliance switch
            {
                QuietHoursCompliance.Compliant      => 200,
                QuietHoursCompliance.ViolatedMinor  => 50,
                _                                   => -200
            };

            // Darkness and comfort bonuses
            int darknessBonus = (quarter.DarknessQualityPermille * 150) / 1000;
            int sensoryBonus  = quarter.HasSensoryReliefKit ? 100 : 0;

            // Composite sleep score
            int baseScore = 650;
            int sleepScore = Math.Clamp(
                baseScore - noisePenalty - (crowdingPenalty / 2) + complianceScore + darknessBonus + sensoryBonus,
                0, 1000);

            // Classify environment band
            SleepEnvironmentBand band = sleepScore switch
            {
                >= 850 => SleepEnvironmentBand.DeepSanctuary,
                >= 650 => SleepEnvironmentBand.Restful,
                >= 450 => SleepEnvironmentBand.Adequate,
                >= 250 => SleepEnvironmentBand.Disturbed,
                _      => SleepEnvironmentBand.Unbearable
            };

            // Fatigue restoration multiplier (1000 = baseline 1.0x)
            int durationFactor = Math.Min(1000, (hoursSlept * 1000) / 8);
            int multiplier = (sleepScore * durationFactor) / 1000;
            multiplier = Math.Clamp(multiplier + 200, 200, 1600); // 0.2x to 1.6x

            // Morale bonus or penalty
            int moraleBonus = band switch
            {
                SleepEnvironmentBand.DeepSanctuary => 4,
                SleepEnvironmentBand.Restful       => 2,
                SleepEnvironmentBand.Adequate      => 0,
                SleepEnvironmentBand.Disturbed     => -2,
                _                                  => -6
            };

            return new SleepQualityResult(
                sleepScore,
                band,
                netDecibels,
                crowdingPenalty,
                multiplier,
                moraleBonus);
        }

        /// <summary>
        /// Computes net fatigue recovery points based on base points and sleep quality multiplier.
        /// </summary>
        public static int ComputeNetFatigueRecovery(int baseFatigueRecovery, int fatigueMultiplierPermille)
        {
            baseFatigueRecovery      = Math.Max(0, baseFatigueRecovery);
            fatigueMultiplierPermille = Math.Clamp(fatigueMultiplierPermille, 100, 2000);

            return (baseFatigueRecovery * fatigueMultiplierPermille) / 1000;
        }
    }
}
