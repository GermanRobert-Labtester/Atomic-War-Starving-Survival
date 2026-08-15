namespace Ashfall.Core.Economy
{
    using System;

    /// <summary>
    /// Engine-agnostic faction stance queries.
    /// </summary>
    public interface IFactionStanceProvider
    {
        TradeStance GetStance(string factionId);
        bool WillTrade(string factionId);
        bool WillShareIntel(string factionId);
        float GetTrust(string factionId);
        float GetEffectiveTrust(string factionId);
        float ModifyTrust(string factionId, float delta);
        void SetTrust(string factionId, float value);
        float GetRaidAggression(string factionId);
        void SetRaidAggression(string factionId, float value);
        bool IsFactionActive(string factionId);
    }

    /// <summary>
    /// Surface for querying active transient price shocks.
    /// </summary>
    public interface IPriceShockProvider
    {
        bool TryGetPriceShock(PriceShockKind kind, int dayOffsetFromShockStart, out PriceShockRule rule);
        float GetScarcityMultiplier(int currentDay, string itemId);
    }

    /// <summary>
    /// Trade event notification surface for reactive UI binding.
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
}
