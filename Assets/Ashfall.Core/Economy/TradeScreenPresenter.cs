namespace Ashfall.Core.Economy
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Track B (Nerves): maps the frozen core interfaces
    /// (IFactionStanceProvider / IPriceShockProvider) onto the Act 0 seam
    /// (ITradeScreenViewModel) and exposes the existing TradeScreenUI surface
    /// (Open, Close, SetPlayerOffer, SetFactionAsk, Recalculate, TryConfirmTrade,
    /// TryDemandParley, BuildQuoteSummary) for API parity.
    ///
    /// Zero-mutation invariant: the presenter only ever calls read methods on
    /// the stance provider (Get*/WillTrade/TryGet*). Trust, aggression and all
    /// other simulation state are never written through this class; execution
    /// is routed exclusively through the injected ITradeExecutionSink.
    /// </summary>
    public sealed class TradeScreenPresenter : ITradeIntentSink
    {
        private readonly IFactionStanceProvider _stance;
        private readonly IPriceShockProvider _shocks;
        private readonly ITradeTellProvider _tells;
        private readonly ITradeExecutionSink _execution;
        private readonly ISeededRng _rng;
        private readonly Func<string, float> _unitPrice;
        private readonly Func<string, string> _displayName;

        private readonly Dictionary<string, int> _playerOfferCounts = new();
        private readonly Dictionary<string, int> _factionAskCounts = new();
        private readonly Dictionary<BiologicalTradeItem, int> _bioOfferCounts = new();
        private readonly List<string> _watchedItems = new();

        private string _worldPhaseLabel = string.Empty;
        private int _worldDay = 1;

        public TradeScreenViewModel ViewModel { get; } = new TradeScreenViewModel();

        public TradeScreenPresenter(
            IFactionStanceProvider stanceProvider,
            IPriceShockProvider priceShockProvider = null!,
            ITradeTellProvider tells = null!,
            ISeededRng rng = null!,
            Func<string, float> unitPriceLookup = null!,
            Func<string, string> displayNameLookup = null!,
            ITradeExecutionSink executionSink = null!)
        {
            _stance = stanceProvider;
            _shocks = priceShockProvider;
            _tells = tells;
            _rng = rng;
            _execution = executionSink;
            _unitPrice = unitPriceLookup ?? (_ => 10f);
            _displayName = displayNameLookup ?? (id => (id ?? string.Empty).Replace('_', ' '));
        }

        /// <summary>World context for the news strip (phase label + day).</summary>
        public void SetWorldContext(string phaseLabel, int day)
        {
            _worldPhaseLabel = phaseLabel ?? string.Empty;
            _worldDay = Math.Max(1, day);
        }

        /// <summary>Items whose scarcity multipliers show in the news strip.</summary>
        public void SetWatchedItems(IEnumerable<string> itemIds)
        {
            _watchedItems.Clear();
            if (itemIds != null)
            {
                foreach (var id in itemIds) _watchedItems.Add(id);
            }
        }

        // ── TradeScreenUI parity surface ─────────────────────────────

        public bool Open(string factionId, string factionName, string leaderName, int successionGeneration)
        {
            if (_stance != null && !_stance.IsFactionActive(factionId)) return false;

            ViewModel.SetOpen(true);
            ViewModel.SetFaction(factionId, factionName, leaderName, successionGeneration);
            ClearOffers();
            Recalculate();
            return true;
        }

        public void Close(bool traded = false)
        {
            ViewModel.SetOpen(false);
        }

        public void SetPlayerOffer(string itemId, int count)
        {
            if (string.IsNullOrEmpty(itemId)) return;
            if (count <= 0) _playerOfferCounts.Remove(itemId);
            else _playerOfferCounts[itemId] = count;
            Recalculate();
        }

        public void SetFactionAsk(string itemId, int count)
        {
            if (string.IsNullOrEmpty(itemId)) return;
            if (count <= 0) _factionAskCounts.Remove(itemId);
            else _factionAskCounts[itemId] = count;
            Recalculate();
        }

        public void SetBiologicalOffer(BiologicalTradeItem item, int count)
        {
            if (count <= 0) _bioOfferCounts.Remove(item);
            else _bioOfferCounts[item] = count;
            Recalculate();
        }

        public void ClearOffers()
        {
            _playerOfferCounts.Clear();
            _factionAskCounts.Clear();
            _bioOfferCounts.Clear();
            Recalculate();
        }

        /// <summary>
        /// Re-maps the providers onto the view model. Read-only against every
        /// provider; raises exactly one Changed event through the VM batching.
        /// </summary>
        public void Recalculate()
        {
            string factionId = ViewModel.FactionId;

            var stance = TradeStance.Refuse;
            float trust = 0f;
            float aggression = 0f;
            bool willTrade = false;

            if (_stance != null)
            {
                stance = _stance.GetStance(factionId);
                trust = _stance.GetEffectiveTrust(factionId);
                aggression = _stance.GetRaidAggression(factionId);
                willTrade = _stance.WillTrade(factionId);
            }

            ViewModel.SetStance(stance);
            ViewModel.SetMeters(trust, aggression);
            ViewModel.SetWorld(_worldPhaseLabel, _worldDay);

            if (_tells != null && _tells.TrySelectTell(stance, trust, _rng, out var tell))
            {
                ViewModel.SetTell(tell.Id, tell.Line);
            }

            ViewModel.SetShockBadges(CollectShockBadges());
            ViewModel.SetScarcityBands(CollectScarcityBands());
            ViewModel.SetTable(
                BuildLines(_playerOfferCounts),
                BuildLines(_factionAskCounts),
                _bioOfferCounts,
                willTrade);
        }

        public bool TryConfirmTrade()
        {
            if (!ViewModel.CanConfirm) return false;
            if (_execution != null)
            {
                // Snapshot the table: the sink owns its copy, and clearing our
                // state afterwards must never empty a dictionary we already gave away.
                if (!_execution.TryExecuteTrade(
                        ViewModel.FactionId,
                        new Dictionary<string, int>(_playerOfferCounts),
                        new Dictionary<string, int>(_factionAskCounts),
                        new Dictionary<BiologicalTradeItem, int>(_bioOfferCounts)))
                {
                    return false;
                }
            }
            ClearOffers();
            return true;
        }

        public bool TryDemandParley()
        {
            if (_execution != null)
            {
                return _execution.TryDemandParley(ViewModel.FactionId);
            }
            return ViewModel.CanDemandParley;
        }

        /// <summary>Qualitative, multi-line quote summary (ECON-002: no raw digits).</summary>
        public string BuildQuoteSummary()
        {
            var sb = new StringBuilder();
            sb.AppendLine("THE NEGOTIATION TABLE");
            sb.AppendLine($"{ViewModel.FactionName} · {ViewModel.LeaderName} (gen {ViewModel.SuccessionGeneration})");

            if (ViewModel.PlayerOffers.Count == 0 && ViewModel.BiologicalOffers.Count == 0)
            {
                sb.AppendLine("OFFER: your edge of the table is bare.");
            }
            else
            {
                sb.Append("OFFER: ");
                sb.AppendLine(FormatLines(ViewModel.PlayerOffers));
                if (ViewModel.BiologicalOffers.Count > 0)
                {
                    sb.Append("OFFER (the drawer): ");
                    sb.AppendLine(JoinBioLines());
                }
            }

            if (ViewModel.FactionDemands.Count == 0)
            {
                sb.AppendLine("DEMAND: their edge of the table is bare.");
            }
            else
            {
                sb.Append("DEMAND: ");
                sb.AppendLine(FormatLines(ViewModel.FactionDemands));
            }

            sb.Append("SCALE: ").AppendLine(ViewModel.FairnessLabel);
            return sb.ToString();
        }

        // ── Internals ────────────────────────────────────────────────

        private static string FormatLines(IReadOnlyList<TradeLineData> lines)
        {
            var parts = new List<string>();
            foreach (var line in lines)
            {
                parts.Add($"{line.Quantity}x {line.DisplayName} — {line.WorthLabel}");
            }
            return string.Join(", ", parts);
        }

        private string JoinBioLines()
        {
            var parts = new List<string>();
            foreach (var pair in ViewModel.BiologicalOffers)
            {
                parts.Add($"{pair.Value}x {pair.Key}");
            }
            return string.Join(", ", parts);
        }

        private List<TradeLineData> BuildLines(Dictionary<string, int> counts)
        {
            var lines = new List<TradeLineData>();
            foreach (var pair in counts)
            {
                if (pair.Value > 0)
                {
                    lines.Add(new TradeLineData(pair.Key, _displayName(pair.Key), pair.Value, _unitPrice(pair.Key)));
                }
            }
            return lines;
        }

        private List<ShockBadgeData> CollectShockBadges()
        {
            var badges = new List<ShockBadgeData>();
            if (_shocks == null) return badges;

            foreach (PriceShockKind kind in Enum.GetValues(typeof(PriceShockKind)))
            {
                if (_shocks.TryGetPriceShock(kind, _worldDay, out var rule))
                {
                    badges.Add(new ShockBadgeData(rule.Kind, rule.Multiplier, rule.Trigger));
                }
            }
            return badges;
        }

        private List<ScarcityBandData> CollectScarcityBands()
        {
            var bands = new List<ScarcityBandData>();
            if (_shocks == null) return bands;

            foreach (var itemId in _watchedItems)
            {
                float multiplier = _shocks.GetScarcityMultiplier(_worldDay, itemId);
                bands.Add(new ScarcityBandData(itemId, _displayName(itemId), multiplier));
            }
            return bands;
        }
    }
}
