using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using AtomicWar._Game.Economy;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// Barter screen: dynamically prices every line from WorldPhase + faction Trust
    /// via DynamicEconomySystem. Shows leader / aggression / parley after a hatch
    /// repel. Bind/Open/TryConfirm/TryDemandParley are the API GameBootstrap and tests use.
    /// </summary>
    public class TradeScreenUI : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public string ActiveFactionId { get; private set; }
        public TradeStance Stance { get; private set; }
        public WorldPhase Phase { get; private set; }

        public float PlayerOfferValue { get; private set; }
        public float FactionAskValue { get; private set; }
        public bool IsFair { get; private set; }

        /// <summary>Runtime leader name for the open faction.</summary>
        public string LeaderName { get; private set; } = string.Empty;
        /// <summary>Effective raid aggression 0..1.</summary>
        public float Aggression { get; private set; }
        public int SuccessionGeneration { get; private set; }
        public int ConsecutiveRepels { get; private set; }
        public bool HasSurrendered { get; private set; }
        public bool CanDemandParley { get; private set; }
        /// <summary>One-line status under the header (leader, aggression, parley).</summary>
        public string FactionStatusStrip { get; private set; } = string.Empty;
        /// <summary>Last parley / surrender message for the player.</summary>
        public string LastParleyMessage { get; private set; } = string.Empty;

        public IReadOnlyList<BarterLine> PlayerOffers => _playerOffers;
        public IReadOnlyList<BarterLine> FactionAsks => _factionAsks;

        public event Action<bool> OnTradeClosed; // true if a trade completed
        public event Action OnQuoteChanged;
        public event Action<FactionSurrenderResult> OnParleyResolved;

        private DynamicEconomySystem _economy;
        private Inventory.Inventory _playerInv;
        private Inventory.Inventory _factionStock;
        private readonly List<BarterLine> _playerOffers = new List<BarterLine>();
        private readonly List<BarterLine> _factionAsks = new List<BarterLine>();

        public void Bind(DynamicEconomySystem economy)
        {
            _economy = economy;
        }

        /// <summary>Open a trade session with a faction stock inventory.</summary>
        public bool Open(
            string factionId,
            Inventory.Inventory playerInventory,
            Inventory.Inventory factionStock)
        {
            if (_economy == null || string.IsNullOrEmpty(factionId)) return false;
            if (playerInventory == null || factionStock == null) return false;

            Stance = _economy.GetStance(factionId);
            if (Stance == TradeStance.HostileRaid || Stance == TradeStance.Refuse || Stance == TradeStance.Rob)
            {
                // Hostile/rob/refuse: screen can still open for messaging / parley, but confirm is blocked.
            }

            ActiveFactionId = factionId;
            _playerInv = playerInventory;
            _factionStock = factionStock;
            _playerOffers.Clear();
            _factionAsks.Clear();
            LastParleyMessage = string.Empty;
            Phase = _economy.CurrentPhase;
            IsOpen = true;
            Recalculate();
            return true;
        }

        public void Close(bool traded = false)
        {
            IsOpen = false;
            ActiveFactionId = null;
            _playerOffers.Clear();
            _factionAsks.Clear();
            PlayerOfferValue = 0f;
            FactionAskValue = 0f;
            IsFair = false;
            LeaderName = string.Empty;
            Aggression = 0f;
            SuccessionGeneration = 0;
            ConsecutiveRepels = 0;
            HasSurrendered = false;
            CanDemandParley = false;
            FactionStatusStrip = string.Empty;
            OnTradeClosed?.Invoke(traded);
        }

        public void SetPlayerOffer(ItemDefinition item, int amount)
        {
            Upsert(_playerOffers, item, amount);
            Recalculate();
        }

        public void SetFactionAsk(ItemDefinition item, int amount)
        {
            Upsert(_factionAsks, item, amount);
            Recalculate();
        }

        public void ClearOffers()
        {
            _playerOffers.Clear();
            _factionAsks.Clear();
            Recalculate();
        }

        /// <summary>
        /// Recompute barter totals from current phase + trust. Safe to call after
        /// WorldPhase advances while the screen is open.
        /// </summary>
        public void Recalculate()
        {
            if (_economy == null || string.IsNullOrEmpty(ActiveFactionId))
            {
                PlayerOfferValue = 0f;
                FactionAskValue = 0f;
                IsFair = false;
                LeaderName = string.Empty;
                Aggression = 0f;
                FactionStatusStrip = string.Empty;
                CanDemandParley = false;
                return;
            }

            Phase = _economy.CurrentPhase;
            Stance = _economy.GetStance(ActiveFactionId);
            PlayerOfferValue = _economy.EvaluateOffer(_playerOffers, ActiveFactionId, playerSelling: true);
            FactionAskValue = _economy.EvaluateOffer(_factionAsks, ActiveFactionId, playerSelling: false);
            IsFair = _economy.IsFairTrade(
                _playerOffers, _factionAsks, ActiveFactionId, out _, out _);

            RefreshFactionStrip();
            OnQuoteChanged?.Invoke();
        }

        /// <summary>Commit the current offer if fair and stance allows trade.</summary>
        public bool TryConfirmTrade()
        {
            if (!IsOpen || _economy == null) return false;
            if (!_economy.WillTrade(ActiveFactionId)) return false;

            bool ok = _economy.TryExecuteTrade(
                _playerInv, _factionStock, _playerOffers, _factionAsks, ActiveFactionId);
            if (ok)
            {
                Close(traded: true);
            }
            return ok;
        }

        /// <summary>
        /// Demand parley / surrender after holding the hatch (repel streak ≥ 1).
        /// Keybind [P] when the trade screen is open.
        /// </summary>
        public bool TryDemandParley()
        {
            if (!IsOpen || _economy == null || string.IsNullOrEmpty(ActiveFactionId))
                return false;

            var result = _economy.DemandParley(ActiveFactionId);
            LastParleyMessage = result.Message ?? string.Empty;
            Recalculate();
            OnParleyResolved?.Invoke(result);
            return result.Applied;
        }

        /// <summary>Human-readable quote for HUD / tests (phase, trust, leader, parley).</summary>
        public string BuildQuoteSummary()
        {
            if (!IsOpen || _economy == null) return string.Empty;
            var sb = new StringBuilder();
            float trust = _economy.GetTrust(ActiveFactionId);
            var fac = _economy.GetFaction(ActiveFactionId);
            string name = fac != null ? fac.displayName : ActiveFactionId;
            sb.AppendLine($"Trade — {name}  [P] parley when available");
            sb.AppendLine(FactionStatusStrip);
            sb.AppendLine($"Phase: {Phase}  Trust: {trust:0}  Stance: {Stance}");
            // ECON-002 — no raw trade digits for the player (policy: qualitative only).
            sb.AppendLine($"You offer: {Item_TradeValues.FormatWorthLabel(PlayerOfferValue)}");
            sb.AppendLine($"They ask:  {Item_TradeValues.FormatWorthLabel(FactionAskValue)}");
            sb.Append(IsFair ? "Deal is fair." : "Deal is short.");
            if (CanDemandParley)
                sb.AppendLine().Append("[P] Demand parley / surrender — they flinched at the hatch.");
            else if (HasSurrendered)
                sb.AppendLine().Append(
                    "They stood down. Hatch-hold softens barter (better sell / cheaper buy).");
            if (!string.IsNullOrEmpty(LastParleyMessage))
                sb.AppendLine().Append(LastParleyMessage);
            return sb.ToString();
        }

        /// <summary>
        /// Per-line unit values for the active quote (player-selling basis for offers,
        /// player-buying basis for asks).
        /// </summary>
        public float GetDisplayedUnitValue(ItemDefinition item, bool fromPlayerOffer)
        {
            if (_economy == null || item == null || string.IsNullOrEmpty(ActiveFactionId))
                return 0f;
            return _economy.GetBarterUnitValue(item, ActiveFactionId, playerSelling: fromPlayerOffer);
        }

        private void RefreshFactionStrip()
        {
            LeaderName = _economy.GetLeaderName(ActiveFactionId);
            Aggression = _economy.GetRaidAggression(ActiveFactionId);
            SuccessionGeneration = _economy.GetSuccessionGeneration(ActiveFactionId);
            ConsecutiveRepels = _economy.GetConsecutiveRepels(ActiveFactionId);
            HasSurrendered = _economy.HasSurrendered(ActiveFactionId);
            CanDemandParley = _economy.CanDemandParley(ActiveFactionId);

            var sb = new StringBuilder();
            sb.Append("Leader: ").Append(LeaderName);
            if (SuccessionGeneration > 0)
                sb.Append(" (gen ").Append(SuccessionGeneration).Append(')');
            sb.Append("  ·  Aggression ").Append(Aggression.ToString("0.00"));
            if (ConsecutiveRepels > 0)
                sb.Append("  ·  Hatch holds ×").Append(ConsecutiveRepels);
            if (HasSurrendered)
                sb.Append("  ·  STOOD DOWN");
            else if (CanDemandParley)
                sb.Append("  ·  PARLEY READY [P]");
            else if (Stance == TradeStance.HostileRaid)
                sb.Append("  ·  hostile — hold the hatch to force parley");

            FactionStatusStrip = sb.ToString();
        }

        private static void Upsert(List<BarterLine> list, ItemDefinition item, int amount)
        {
            if (item == null) return;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Item != null && list[i].Item.id == item.id)
                {
                    if (amount <= 0) list.RemoveAt(i);
                    else list[i] = new BarterLine(item, amount);
                    return;
                }
            }
            if (amount > 0) list.Add(new BarterLine(item, amount));
        }
    }
}
