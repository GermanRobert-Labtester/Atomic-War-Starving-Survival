namespace Ashfall.Core.Economy
{
    using System;

    /// <summary>
    /// Engine-agnostic faction stance queries. Implemented by FactionStanceEngine and DynamicEconomySystem.
    /// </summary>
    public interface IFactionStanceProvider
    {
        float GetTrust(string factionId);
        float GetEffectiveTrust(string factionId);
        float ModifyTrust(string factionId, float delta);
        void SetTrust(string factionId, float value);
        TradeStance GetStance(string factionId);
        bool WillTrade(string factionId);
        bool WillShareIntel(string factionId);
        float GetRaidAggression(string factionId);
        void SetRaidAggression(string factionId, float value);
        bool IsFactionActive(string factionId);
    }

    /// <summary>
    /// Surface for querying active transient price shocks.
    /// Engine-agnostic: backed by HardcoreEconomyTuning overlay.
    /// </summary>
    public interface IPriceShockProvider
    {
        bool TryGetPriceShock(PriceShockKind kind, int dayOffsetFromShockStart, out PriceShockRule rule);
        float GetScarcityMultiplier(int day, string itemId);
    }

    /// <summary>
    /// Trade event notification surface for reactive UI binding.
    /// Engine-agnostic event signatures.
    /// </summary>
    public interface ITradeEvents
    {
        event Action<string, float, float> OnTrustChanged;
        event Action<FactionRaidResult> OnRaidResolved;
        event Action<FactionSuccessionResult> OnFactionSuccession;
        event Action<FactionSurrenderResult> OnFactionSurrender;
        event Action OnEconomyChanged;
        event Action<bool> OnBarterOnlyModeChanged;
    }

    /// <summary>
    /// Immutable faction threshold data. Hosts populate this from FactionSO
    /// or any other data source; the core engine never touches UnityEngine.
    /// </summary>
    public struct FactionThresholds
    {
        public string FactionId { get; set; }
        public float RaidThreshold { get; set; }
        public float RobThreshold { get; set; }
        public float MinTrustToTrade { get; set; }
        public float IntelShareThreshold { get; set; }
        public float RaidAggression { get; set; }
        public bool TrustInversion { get; set; }
        public float HealthyRadiationCeiling { get; set; }
        public float HighRadiationFloor { get; set; }

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
