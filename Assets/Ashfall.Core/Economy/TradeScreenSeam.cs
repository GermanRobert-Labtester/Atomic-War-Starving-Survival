namespace Ashfall.Core.Economy
{
    using System;
    using System.Collections.Generic;
#pragma warning disable CS8618

    /// <summary>
    /// Verdict of the arbitrator's scale at the negotiation table.
    /// Labels are part of the verified gate surface (FAIR / SHORT).
    /// </summary>
    public enum TradeFairness
    {
        /// <summary>Both sides of the table are bare — a deliberate, closed posture.</summary>
        EmptyTable,
        /// <summary>The player's side outweighs or matches the faction's demands.</summary>
        Fair,
        /// <summary>The player's side is lighter than the faction's demands.</summary>
        Short
    }

    /// <summary>Canonical fairness labels. Gates check the FAIR / SHORT substrings.</summary>
    public static class TradeFairnessLabels
    {
        public const string EmptyTable = "EMPTY TABLE";
        public const string Fair = "DEAL IS FAIR";
        public const string Short = "OFFER SHORT";

        public static string For(TradeFairness fairness)
        {
            switch (fairness)
            {
                case TradeFairness.Fair: return Fair;
                case TradeFairness.Short: return Short;
                default: return EmptyTable;
            }
        }
    }

    /// <summary>
    /// Qualitative worth vocabulary for the trade screen (ECON-002: no raw digits
    /// on the offer edges). Shared by both hosts so thresholds never fork.
    /// </summary>
    public static class TradeWorthLabels
    {
        public static string Format(float value)
        {
            if (value <= 0f) return "None";
            if (value < 20f) return "Sparse";
            if (value < 60f) return "Modest";
            if (value < 150f) return "Substantial";
            return "Generous";
        }
    }

    /// <summary>
    /// Core pricing for biological offerings. One source of truth so the Godot
    /// panel, the presenter, and scenario mocks cannot disagree.
    /// </summary>
    public static class TradePricing
    {
        public static float BioUnitValue(BiologicalTradeItem item)
        {
            return ((int)item + 1) * 25f;
        }
    }

    /// <summary>One line on the table: an item pushed across, or an item guarded.</summary>
    public sealed class TradeLineData
    {
        public string ItemId { get; }
        public string DisplayName { get; }
        public int Quantity { get; }
        public string WorthLabel { get; }
        public float TotalValue { get; }

        public TradeLineData(string itemId, string displayName, int quantity, float unitValue)
        {
            ItemId = itemId ?? string.Empty;
            DisplayName = displayName ?? ItemId;
            Quantity = Math.Max(0, quantity);
            TotalValue = unitValue * Quantity;
            WorthLabel = TradeWorthLabels.Format(TotalValue);
        }
    }

    /// <summary>One active price shock badge for the "news from outside" strip.</summary>
    public sealed class ShockBadgeData
    {
        public PriceShockKind Kind { get; }
        public float Multiplier { get; }
        public string Note { get; }

        public ShockBadgeData(PriceShockKind kind, float multiplier, string note)
        {
            Kind = kind;
            Multiplier = multiplier;
            Note = note ?? string.Empty;
        }
    }

    /// <summary>One scarcity multiplier entry for the "news from outside" strip.</summary>
    public sealed class ScarcityBandData
    {
        public string ItemId { get; }
        public string DisplayName { get; }
        public float Multiplier { get; }

        public ScarcityBandData(string itemId, string displayName, float multiplier)
        {
            ItemId = itemId ?? string.Empty;
            DisplayName = displayName ?? ItemId;
            Multiplier = multiplier;
        }
    }

    /// <summary>
    /// Act 0 seam: the engine-agnostic read model for the Negotiation Table.
    /// Exposes every field the 14 verified probes check. Views paint from this
    /// and only this; they never touch simulation types directly.
    /// </summary>
    public interface ITradeScreenViewModel
    {
        bool IsOpen { get; }

        // The trader's presence
        string FactionId { get; }
        string FactionName { get; }
        string LeaderName { get; }
        int SuccessionGeneration { get; }

        // Posture / tell
        TradeStance Stance { get; }
        string StanceBadgeText { get; }
        string StanceTellId { get; }
        string StanceTellLine { get; }

        // Ledger-edge meters
        float Trust { get; }
        float Aggression { get; }
        int ConsecutiveRepels { get; }
        bool HasSurrendered { get; }
        bool CanDemandParley { get; }

        // The world intruding on the deal
        string WorldPhaseLabel { get; }
        int WorldDay { get; }
        IReadOnlyList<ShockBadgeData> ShockBadges { get; }
        IReadOnlyList<ScarcityBandData> ScarcityMultipliers { get; }

        // The two edges of the table
        IReadOnlyList<TradeLineData> PlayerOffers { get; }
        IReadOnlyList<TradeLineData> FactionDemands { get; }
        IReadOnlyDictionary<BiologicalTradeItem, int> BiologicalOffers { get; }
        float PlayerOfferValue { get; }
        float FactionAskValue { get; }

        // The arbitrator's scale
        TradeFairness Fairness { get; }
        string FairnessLabel { get; }
        bool CanConfirm { get; }

        // The room's radio
        string RadioTickerLine { get; }

        /// <summary>Raised whenever any field above changes.</summary>
        event Action Changed;
    }

    /// <summary>
    /// Act 0 seam: where player intent leaves the presentation layer.
    /// Implemented by presenters (routing to an ITradeExecutionSink) and by
    /// mock scenario bindings on the skin track.
    /// </summary>
    public interface ITradeIntentSink
    {
        bool TryConfirmTrade();
        bool TryDemandParley();
        void Close(bool traded);
    }

    /// <summary>
    /// Frozen core interface for executing a confirmed trade against live
    /// simulation state. Hosts adapt their economy system to this; the
    /// presenter never mutates providers directly.
    /// </summary>
    public interface ITradeExecutionSink
    {
        bool WillTrade(string factionId);
        bool TryExecuteTrade(
            string factionId,
            IReadOnlyDictionary<string, int> playerOffers,
            IReadOnlyDictionary<string, int> factionAsks,
            IReadOnlyDictionary<BiologicalTradeItem, int> biologicalOffers);
        bool TryDemandParley(string factionId);
    }

    /// <summary>
    /// Mutable, engine-agnostic implementation of the read model. The presenter
    /// (live track) and the scenario mocks (skin track) both write this; views
    /// subscribe to <see cref="Changed"/>.
    /// </summary>
    public sealed class TradeScreenViewModel : ITradeScreenViewModel
    {
        private readonly List<TradeLineData> _playerOffers = new();
        private readonly List<TradeLineData> _factionDemands = new();
        private readonly Dictionary<BiologicalTradeItem, int> _bioOffers = new();
        private readonly List<ShockBadgeData> _shocks = new();
        private readonly List<ScarcityBandData> _scarcity = new();

        public event Action Changed;

        public bool IsOpen { get; private set; }
        public string FactionId { get; private set; } = string.Empty;
        public string FactionName { get; private set; } = string.Empty;
        public string LeaderName { get; private set; } = string.Empty;
        public int SuccessionGeneration { get; private set; }
        public TradeStance Stance { get; private set; } = TradeStance.Refuse;
        public string StanceBadgeText { get; private set; } = "[ STANCE: REFUSE ]";
        public string StanceTellId { get; private set; } = string.Empty;
        public string StanceTellLine { get; private set; } = string.Empty;
        public float Trust { get; private set; }
        public float Aggression { get; private set; }
        public int ConsecutiveRepels { get; private set; }
        public bool HasSurrendered { get; private set; }
        public bool CanDemandParley { get; private set; }
        public string WorldPhaseLabel { get; private set; } = string.Empty;
        public int WorldDay { get; private set; } = 1;
        public IReadOnlyList<TradeLineData> PlayerOffers => _playerOffers;
        public IReadOnlyList<TradeLineData> FactionDemands => _factionDemands;
        public IReadOnlyDictionary<BiologicalTradeItem, int> BiologicalOffers => _bioOffers;
        public IReadOnlyList<ShockBadgeData> ShockBadges => _shocks;
        public IReadOnlyList<ScarcityBandData> ScarcityMultipliers => _scarcity;
        public float PlayerOfferValue { get; private set; }
        public float FactionAskValue { get; private set; }
        public TradeFairness Fairness { get; private set; } = TradeFairness.EmptyTable;
        public string FairnessLabel => TradeFairnessLabels.For(Fairness);
        public bool CanConfirm { get; private set; }
        public string RadioTickerLine { get; private set; } = string.Empty;

        public TradeScreenViewModel SetOpen(bool open) { IsOpen = open; Bump(); return this; }
        public TradeScreenViewModel SetFaction(string id, string name, string leader, int generation)
        {
            FactionId = id ?? string.Empty;
            FactionName = name ?? FactionId;
            LeaderName = leader ?? string.Empty;
            SuccessionGeneration = generation;
            Bump();
            return this;
        }
        public TradeScreenViewModel SetStance(TradeStance stance)
        {
            Stance = stance;
            StanceBadgeText = $"[ STANCE: {stance.ToString().ToUpperInvariant()} ]";
            Bump();
            return this;
        }
        public TradeScreenViewModel SetTell(string tellId, string tellLine)
        {
            StanceTellId = tellId ?? string.Empty;
            StanceTellLine = tellLine ?? string.Empty;
            Bump();
            return this;
        }
        public TradeScreenViewModel SetMeters(float trust, float aggression)
        {
            Trust = trust;
            Aggression = aggression;
            Bump();
            return this;
        }
        public TradeScreenViewModel SetFactionPresence(int repels, bool surrendered, bool canParley)
        {
            ConsecutiveRepels = repels;
            HasSurrendered = surrendered;
            CanDemandParley = canParley;
            Bump();
            return this;
        }
        public TradeScreenViewModel SetWorld(string phaseLabel, int day)
        {
            WorldPhaseLabel = phaseLabel ?? string.Empty;
            WorldDay = Math.Max(1, day);
            Bump();
            return this;
        }
        public TradeScreenViewModel SetShockBadges(IEnumerable<ShockBadgeData> badges)
        {
            _shocks.Clear();
            if (badges != null) _shocks.AddRange(badges);
            Bump();
            return this;
        }
        public TradeScreenViewModel SetScarcityBands(IEnumerable<ScarcityBandData> bands)
        {
            _scarcity.Clear();
            if (bands != null) _scarcity.AddRange(bands);
            Bump();
            return this;
        }
        public TradeScreenViewModel SetTable(
            IEnumerable<TradeLineData> playerOffers,
            IEnumerable<TradeLineData> factionDemands,
            IReadOnlyDictionary<BiologicalTradeItem, int> biologicalOffers,
            bool willTrade)
        {
            _playerOffers.Clear();
            if (playerOffers != null) _playerOffers.AddRange(playerOffers);
            _factionDemands.Clear();
            if (factionDemands != null) _factionDemands.AddRange(factionDemands);
            _bioOffers.Clear();
            if (biologicalOffers != null)
            {
                foreach (var pair in biologicalOffers)
                {
                    if (pair.Value > 0) _bioOffers[pair.Key] = pair.Value;
                }
            }

            PlayerOfferValue = 0f;
            foreach (var line in _playerOffers) PlayerOfferValue += line.TotalValue;
            foreach (var pair in _bioOffers) PlayerOfferValue += TradePricing.BioUnitValue(pair.Key) * pair.Value;
            FactionAskValue = 0f;
            foreach (var line in _factionDemands) FactionAskValue += line.TotalValue;

            if (_playerOffers.Count == 0 && _factionDemands.Count == 0 && _bioOffers.Count == 0)
            {
                Fairness = TradeFairness.EmptyTable;
                CanConfirm = false;
            }
            else
            {
                Fairness = PlayerOfferValue >= FactionAskValue ? TradeFairness.Fair : TradeFairness.Short;
                CanConfirm = Fairness == TradeFairness.Fair && willTrade;
            }
            Bump();
            return this;
        }
        public TradeScreenViewModel SetRadioTicker(string line)
        {
            RadioTickerLine = line ?? string.Empty;
            Bump();
            return this;
        }

        private void Bump()
        {
            Changed?.Invoke();
        }
    }
}
