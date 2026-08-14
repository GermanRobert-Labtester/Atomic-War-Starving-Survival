namespace Ashfall.Core.Economy
{
    using System;

    /// <summary>
    /// Immutable faction threshold data. Hosts populate this from FactionSO
    /// or any other data source; the core engine never touches UnityEngine.
    /// </summary>
    public readonly struct FactionThresholds
    {
        public string FactionId { get; }
        public float RaidThreshold { get; }
        public float RobThreshold { get; }
        public float MinTrustToTrade { get; }
        public float IntelShareThreshold { get; }
        public float RaidAggression { get; }
        public bool TrustInversion { get; }
        public float HealthyRadiationCeiling { get; }
        public float HighRadiationFloor { get; }

        public FactionThresholds(
            string factionId,
            float raidThreshold = -50f,
            float robThreshold = -20f,
            float minTrustToTrade = -40f,
            float intelShareThreshold = 40f,
            float raidAggression = 0.5f,
            bool trustInversion = false,
            float healthyRadiationCeiling = 20f,
            float highRadiationFloor = 60f)
        {
            FactionId = factionId ?? string.Empty;
            RaidThreshold = raidThreshold;
            RobThreshold = robThreshold;
            MinTrustToTrade = minTrustToTrade;
            IntelShareThreshold = intelShareThreshold;
            RaidAggression = raidAggression;
            TrustInversion = trustInversion;
            HealthyRadiationCeiling = healthyRadiationCeiling;
            HighRadiationFloor = highRadiationFloor;
        }
    }

    /// <summary>Day gate for trust-inversion factions (Cult of the Glow).</summary>
    public static class FactionStanceConstants
    {
        public const int CultActivationDay = 30;
        public const float MinTrust = -100f;
        public const float MaxTrust = 100f;
        public const float DefaultRaidThreshold = -50f;
        public const float DefaultRobThreshold = -20f;
        public const float DefaultMinTrustToTrade = -40f;
        public const float DefaultIntelShareThreshold = 40f;
    }
}
