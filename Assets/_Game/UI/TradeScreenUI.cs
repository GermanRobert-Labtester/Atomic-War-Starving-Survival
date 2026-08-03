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
    /// via DynamicEconomySystem. Presentation is data-driven (no hardcoded values);
    /// Bind/Open/TryConfirm are the API GameBootstrap and tests use.
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

        public IReadOnlyList<BarterLine> PlayerOffers => _playerOffers;
        public IReadOnlyList<BarterLine> FactionAsks => _factionAsks;

        public event Action<bool> OnTradeClosed; // true if a trade completed
        public event Action OnQuoteChanged;

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
                // Hostile/rob/refuse: screen can still open for messaging, but confirm is blocked.
            }

            ActiveFactionId = factionId;
            _playerInv = playerInventory;
            _factionStock = factionStock;
            _playerOffers.Clear();
            _factionAsks.Clear();
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
                return;
            }

            Phase = _economy.CurrentPhase;
            Stance = _economy.GetStance(ActiveFactionId);
            PlayerOfferValue = _economy.EvaluateOffer(_playerOffers, ActiveFactionId, playerSelling: true);
            FactionAskValue = _economy.EvaluateOffer(_factionAsks, ActiveFactionId, playerSelling: false);
            IsFair = _economy.IsFairTrade(
                _playerOffers, _factionAsks, ActiveFactionId, out _, out _);
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

        /// <summary>Human-readable quote for HUD / tests (phase, trust, totals).</summary>
        public string BuildQuoteSummary()
        {
            if (!IsOpen || _economy == null) return string.Empty;
            var sb = new StringBuilder();
            float trust = _economy.GetTrust(ActiveFactionId);
            var fac = _economy.GetFaction(ActiveFactionId);
            string name = fac != null ? fac.displayName : ActiveFactionId;
            sb.AppendLine($"Trade — {name}");
            sb.AppendLine($"Phase: {Phase}  Trust: {trust:0}  Stance: {Stance}");
            sb.AppendLine($"You offer: {PlayerOfferValue:0.0}");
            sb.AppendLine($"They ask:  {FactionAskValue:0.0}");
            sb.Append(IsFair ? "Deal is fair." : "Deal is short.");
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
