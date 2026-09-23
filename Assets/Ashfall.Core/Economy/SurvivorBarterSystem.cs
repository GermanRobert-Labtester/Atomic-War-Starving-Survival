// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Economy
{
    public enum BarterOfferStatus
    {
        Pending = 0,
        Accepted = 1,
        Countered = 2,
        Rejected = 3,
        Expired = 4,
        Settled = 5
    }

    public enum TradeType
    {
        ItemExchange = 0,
        FavorExchange = 1,
        MixedTrade = 2,
        Gift = 3
    }

    public enum FavorType
    {
        ChoreShift = 0,
        CraftingAssistance = 1,
        WatchDuty = 2,
        MedicalCare = 3,
        ScavengingAssistance = 4,
        GeneralFavor = 5
    }

    [Serializable]
    public sealed class BarterRuleDefinition
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("base_trust_gain_per_trade")]
        public float BaseTrustGainPerTrade { get; set; } = 5f;

        [JsonPropertyName("dispute_trust_penalty")]
        public float DisputeTrustPenalty { get; set; } = 20f;

        [JsonPropertyName("max_active_offers_per_survivor")]
        public int MaxActiveOffersPerSurvivor { get; set; } = 5;

        [JsonPropertyName("offer_expiration_days")]
        public int OfferExpirationDays { get; set; } = 5;

        [JsonPropertyName("favor_deadline_days")]
        public int FavorDeadlineDays { get; set; } = 14;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class BarterRulesCatalogData
    {
        [JsonPropertyName("schema_version")]
        public int SchemaVersion { get; set; } = 1;

        [JsonPropertyName("rules")]
        public List<BarterRuleDefinition> Rules { get; set; } = new List<BarterRuleDefinition>();
    }

    [Serializable]
    public sealed class BarterOffer
    {
        public string OfferId { get; set; } = string.Empty;
        public string OffererId { get; set; } = string.Empty;
        public string TargetSurvivorId { get; set; } = string.Empty;
        public List<string> OfferedItemIds { get; set; } = new List<string>();
        public List<string> RequestedItemIds { get; set; } = new List<string>();
        public FavorType? OfferedFavor { get; set; }
        public FavorType? RequestedFavor { get; set; }
        public int CreatedDay { get; set; } = 1;
        public int ExpiresDay { get; set; } = 6;
        public BarterOfferStatus Status { get; set; } = BarterOfferStatus.Pending;
    }

    [Serializable]
    public sealed class CompletedBarterTrade
    {
        public string TradeId { get; set; } = string.Empty;
        public string OfferId { get; set; } = string.Empty;
        public string TraderAId { get; set; } = string.Empty;
        public string TraderBId { get; set; } = string.Empty;
        public TradeType TradeType { get; set; } = TradeType.ItemExchange;
        public List<string> ItemsFromA { get; set; } = new List<string>();
        public List<string> ItemsFromB { get; set; } = new List<string>();
        public int TradeDay { get; set; } = 1;
    }

    [Serializable]
    public sealed class TradeReputation
    {
        public string TraderAId { get; set; } = string.Empty;
        public string TraderBId { get; set; } = string.Empty;
        public float TrustLevel { get; set; } = 50f;
        public int TradesCompleted { get; set; } = 0;
        public int DisputesCount { get; set; } = 0;
    }

    [Serializable]
    public sealed class FavorObligation
    {
        public string FavorId { get; set; } = string.Empty;
        public string DebtorId { get; set; } = string.Empty;
        public string CreditorId { get; set; } = string.Empty;
        public FavorType Favor { get; set; } = FavorType.GeneralFavor;
        public string Description { get; set; } = string.Empty;
        public int CreatedDay { get; set; } = 1;
        public int DueDay { get; set; } = 15;
        public bool IsFulfilled { get; set; } = false;
        public int FulfilledDay { get; set; } = -1;
    }

    [Serializable]
    public sealed class SurvivorBarterSaveState
    {
        public int SchemaVersion { get; set; } = 1;
        public int NextSequence { get; set; } = 1;
        public string ActiveRuleId { get; set; } = "standard_community_barter";
        public List<BarterOffer> Offers { get; set; } = new List<BarterOffer>();
        public List<CompletedBarterTrade> CompletedTrades { get; set; } = new List<CompletedBarterTrade>();
        public List<TradeReputation> Reputations { get; set; } = new List<TradeReputation>();
        public List<FavorObligation> Favors { get; set; } = new List<FavorObligation>();
    }

    /// <summary>
    /// Plan 213 — Survivor Barter & Informal Economy System.
    /// Manages survivor-to-survivor item exchanges, favor obligations, and pairwise trade reputations.
    /// Pure domain authority; coordinates with personal belongings and inventory through deterministic hooks.
    /// </summary>
    public sealed class SurvivorBarterSystem
    {
        private readonly SurvivorBarterSaveState _state;
        private readonly Dictionary<string, BarterRuleDefinition> _rules =
            new Dictionary<string, BarterRuleDefinition>(StringComparer.OrdinalIgnoreCase);

        public event Action<BarterOffer>? OnOfferCreated;
        public event Action<BarterOffer, CompletedBarterTrade>? OnOfferAccepted;
        public event Action<BarterOffer>? OnOfferRejected;
        public event Action<FavorObligation>? OnFavorFulfilled;
        public event Action<string /*tradeId*/, string /*complainant*/>? OnTradeDisputed;
        public event Action? OnStateChanged;

        // Integration hooks to personal belongings / inventory
        public Func<string /*survivorId*/, string /*itemId*/, bool>? HasPersonalItem { get; set; }
        public Action<string /*fromId*/, string /*toId*/, string /*itemId*/>? TransferPersonalItem { get; set; }
        public Action<string /*traderA*/, string /*traderB*/, float /*trustDelta*/>? OnRelationshipTrustModified { get; set; }

        public IReadOnlyList<BarterOffer> Offers => _state.Offers;
        public IReadOnlyList<CompletedBarterTrade> CompletedTrades => _state.CompletedTrades;
        public IReadOnlyList<TradeReputation> Reputations => _state.Reputations;
        public IReadOnlyList<FavorObligation> Favors => _state.Favors;
        public IReadOnlyCollection<BarterRuleDefinition> Rules => _rules.Values;
        public string ActiveRuleId => _state.ActiveRuleId;

        public BarterRuleDefinition? ActiveRule =>
            _rules.TryGetValue(_state.ActiveRuleId, out var r) ? r : null;

        public SurvivorBarterSystem(SurvivorBarterSaveState? state = null)
        {
            _state = state ?? new SurvivorBarterSaveState();
        }

        public void LoadCatalog(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return;
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var data = JsonSerializer.Deserialize<BarterRulesCatalogData>(json, options);
            if (data != null)
            {
                LoadCatalog(data);
            }
        }

        public void LoadCatalog(BarterRulesCatalogData catalog)
        {
            if (catalog?.Rules == null) return;
            foreach (var r in catalog.Rules)
            {
                if (!string.IsNullOrWhiteSpace(r.Id))
                {
                    _rules[r.Id] = r;
                }
            }
        }

        public BarterRuleDefinition? GetRule(string ruleId)
        {
            if (string.IsNullOrWhiteSpace(ruleId)) return null;
            return _rules.TryGetValue(ruleId, out var r) ? r : null;
        }

        public bool SetRule(string ruleId)
        {
            if (string.IsNullOrWhiteSpace(ruleId)) return false;
            if (_rules.Count > 0 && !_rules.ContainsKey(ruleId)) return false;
            _state.ActiveRuleId = ruleId;
            OnStateChanged?.Invoke();
            return true;
        }

        public BarterOffer? CreateOffer(
            string offererId,
            string targetId,
            IEnumerable<string>? offeredItems = null,
            IEnumerable<string>? requestedItems = null,
            FavorType? offeredFavor = null,
            FavorType? requestedFavor = null,
            int currentDay = 1)
        {
            if (string.IsNullOrWhiteSpace(offererId) || string.IsNullOrWhiteSpace(targetId))
                return null;
            if (string.Equals(offererId, targetId, StringComparison.OrdinalIgnoreCase))
                return null;

            int maxOffers = ActiveRule?.MaxActiveOffersPerSurvivor ?? 5;
            int activeCount = _state.Offers.Count(o =>
                string.Equals(o.OffererId, offererId, StringComparison.OrdinalIgnoreCase) &&
                o.Status == BarterOfferStatus.Pending);

            if (activeCount >= maxOffers)
                return null;

            var offeredList = offeredItems?.ToList() ?? new List<string>();
            var requestedList = requestedItems?.ToList() ?? new List<string>();

            // Validate that offerer actually possesses offered items if hook is wired
            if (HasPersonalItem != null)
            {
                foreach (var itemId in offeredList)
                {
                    if (!HasPersonalItem(offererId, itemId))
                        return null;
                }
            }

            int expirationDays = ActiveRule?.OfferExpirationDays ?? 5;

            var offer = new BarterOffer
            {
                OfferId = $"off_{_state.NextSequence++}",
                OffererId = offererId.Trim(),
                TargetSurvivorId = targetId.Trim(),
                OfferedItemIds = offeredList,
                RequestedItemIds = requestedList,
                OfferedFavor = offeredFavor,
                RequestedFavor = requestedFavor,
                CreatedDay = currentDay,
                ExpiresDay = currentDay + expirationDays,
                Status = BarterOfferStatus.Pending
            };

            _state.Offers.Add(offer);
            OnOfferCreated?.Invoke(offer);
            OnStateChanged?.Invoke();
            return offer;
        }

        public CompletedBarterTrade? AcceptOffer(string offerId, int currentDay = 1)
        {
            var offer = _state.Offers.FirstOrDefault(o => o.OfferId == offerId);
            if (offer == null || offer.Status != BarterOfferStatus.Pending)
                return null;

            // Atomic validation: revalidate item ownership before executing trade
            if (HasPersonalItem != null)
            {
                foreach (var item in offer.OfferedItemIds)
                {
                    if (!HasPersonalItem(offer.OffererId, item))
                        return null;
                }
                foreach (var item in offer.RequestedItemIds)
                {
                    if (!HasPersonalItem(offer.TargetSurvivorId, item))
                        return null;
                }
            }

            // Transfer items
            if (TransferPersonalItem != null)
            {
                foreach (var item in offer.OfferedItemIds)
                {
                    TransferPersonalItem(offer.OffererId, offer.TargetSurvivorId, item);
                }
                foreach (var item in offer.RequestedItemIds)
                {
                    TransferPersonalItem(offer.TargetSurvivorId, offer.OffererId, item);
                }
            }

            // Record favor obligations if any
            int favorDeadline = ActiveRule?.FavorDeadlineDays ?? 14;
            if (offer.OfferedFavor.HasValue)
            {
                _state.Favors.Add(new FavorObligation
                {
                    FavorId = $"fvr_{_state.NextSequence++}",
                    DebtorId = offer.OffererId,
                    CreditorId = offer.TargetSurvivorId,
                    Favor = offer.OfferedFavor.Value,
                    Description = $"Promise of {offer.OfferedFavor.Value} in barter exchange",
                    CreatedDay = currentDay,
                    DueDay = currentDay + favorDeadline,
                    IsFulfilled = false
                });
            }
            if (offer.RequestedFavor.HasValue)
            {
                _state.Favors.Add(new FavorObligation
                {
                    FavorId = $"fvr_{_state.NextSequence++}",
                    DebtorId = offer.TargetSurvivorId,
                    CreditorId = offer.OffererId,
                    Favor = offer.RequestedFavor.Value,
                    Description = $"Promise of {offer.RequestedFavor.Value} in barter exchange",
                    CreatedDay = currentDay,
                    DueDay = currentDay + favorDeadline,
                    IsFulfilled = false
                });
            }

            // Determine trade type
            TradeType tradeType;
            bool hasItems = offer.OfferedItemIds.Count > 0 || offer.RequestedItemIds.Count > 0;
            bool hasFavors = offer.OfferedFavor.HasValue || offer.RequestedFavor.HasValue;
            if (hasItems && hasFavors) tradeType = TradeType.MixedTrade;
            else if (hasFavors) tradeType = TradeType.FavorExchange;
            else if (offer.RequestedItemIds.Count == 0 && offer.OfferedItemIds.Count > 0) tradeType = TradeType.Gift;
            else tradeType = TradeType.ItemExchange;

            var trade = new CompletedBarterTrade
            {
                TradeId = $"trd_{_state.NextSequence++}",
                OfferId = offer.OfferId,
                TraderAId = offer.OffererId,
                TraderBId = offer.TargetSurvivorId,
                TradeType = tradeType,
                ItemsFromA = new List<string>(offer.OfferedItemIds),
                ItemsFromB = new List<string>(offer.RequestedItemIds),
                TradeDay = currentDay
            };

            offer.Status = BarterOfferStatus.Accepted;
            _state.CompletedTrades.Add(trade);

            // Update pairwise reputation
            var rep = GetOrCreateReputation(offer.OffererId, offer.TargetSurvivorId);
            float trustGain = ActiveRule?.BaseTrustGainPerTrade ?? 5f;
            rep.TrustLevel = Math.Clamp(rep.TrustLevel + trustGain, 0f, 100f);
            rep.TradesCompleted++;

            OnRelationshipTrustModified?.Invoke(offer.OffererId, offer.TargetSurvivorId, trustGain);
            OnOfferAccepted?.Invoke(offer, trade);
            OnStateChanged?.Invoke();
            return trade;
        }

        public bool RejectOffer(string offerId)
        {
            var offer = _state.Offers.FirstOrDefault(o => o.OfferId == offerId);
            if (offer == null || offer.Status != BarterOfferStatus.Pending)
                return false;

            offer.Status = BarterOfferStatus.Rejected;
            OnOfferRejected?.Invoke(offer);
            OnStateChanged?.Invoke();
            return true;
        }

        public bool FulfillFavor(string favorId, int currentDay = 1)
        {
            var favor = _state.Favors.FirstOrDefault(f => f.FavorId == favorId);
            if (favor == null || favor.IsFulfilled)
                return false;

            favor.IsFulfilled = true;
            favor.FulfilledDay = currentDay;

            var rep = GetOrCreateReputation(favor.DebtorId, favor.CreditorId);
            rep.TrustLevel = Math.Clamp(rep.TrustLevel + 4f, 0f, 100f);

            OnFavorFulfilled?.Invoke(favor);
            OnStateChanged?.Invoke();
            return true;
        }

        public bool RaiseDispute(string tradeId, string complainantId)
        {
            var trade = _state.CompletedTrades.FirstOrDefault(t => t.TradeId == tradeId);
            if (trade == null) return false;

            string otherTrader = string.Equals(trade.TraderAId, complainantId, StringComparison.OrdinalIgnoreCase)
                ? trade.TraderBId
                : trade.TraderAId;

            var rep = GetOrCreateReputation(complainantId, otherTrader);
            float penalty = ActiveRule?.DisputeTrustPenalty ?? 20f;
            rep.TrustLevel = Math.Clamp(rep.TrustLevel - penalty, 0f, 100f);
            rep.DisputesCount++;

            OnTradeDisputed?.Invoke(tradeId, complainantId);
            OnStateChanged?.Invoke();
            return true;
        }

        public TradeReputation GetReputation(string survivorA, string survivorB)
        {
            return GetOrCreateReputation(survivorA, survivorB);
        }

        public IReadOnlyList<BarterOffer> GetActiveOffers(string survivorId)
        {
            if (string.IsNullOrWhiteSpace(survivorId)) return Array.Empty<BarterOffer>();
            return _state.Offers
                .Where(o => (string.Equals(o.OffererId, survivorId, StringComparison.OrdinalIgnoreCase) ||
                             string.Equals(o.TargetSurvivorId, survivorId, StringComparison.OrdinalIgnoreCase)) &&
                            o.Status == BarterOfferStatus.Pending)
                .ToList();
        }

        public IReadOnlyList<FavorObligation> GetPendingFavors(string survivorId)
        {
            if (string.IsNullOrWhiteSpace(survivorId)) return Array.Empty<FavorObligation>();
            return _state.Favors
                .Where(f => (string.Equals(f.DebtorId, survivorId, StringComparison.OrdinalIgnoreCase) ||
                             string.Equals(f.CreditorId, survivorId, StringComparison.OrdinalIgnoreCase)) &&
                            !f.IsFulfilled)
                .ToList();
        }

        public void TickDay(int currentDay)
        {
            bool changed = false;
            foreach (var offer in _state.Offers)
            {
                if (offer.Status == BarterOfferStatus.Pending && currentDay >= offer.ExpiresDay)
                {
                    offer.Status = BarterOfferStatus.Expired;
                    changed = true;
                }
            }

            // Check overdue favors and penalize trust
            foreach (var favor in _state.Favors)
            {
                if (!favor.IsFulfilled && currentDay > favor.DueDay)
                {
                    var rep = GetOrCreateReputation(favor.DebtorId, favor.CreditorId);
                    if (rep.TrustLevel > 0f)
                    {
                        rep.TrustLevel = Math.Clamp(rep.TrustLevel - 0.5f, 0f, 100f);
                        changed = true;
                    }
                }
            }

            if (changed)
            {
                OnStateChanged?.Invoke();
            }
        }

        public SurvivorBarterSaveState CaptureState()
        {
            var capture = new SurvivorBarterSaveState
            {
                SchemaVersion = _state.SchemaVersion,
                NextSequence = _state.NextSequence,
                ActiveRuleId = _state.ActiveRuleId,
                Offers = new List<BarterOffer>(_state.Offers.Count),
                CompletedTrades = new List<CompletedBarterTrade>(_state.CompletedTrades.Count),
                Reputations = new List<TradeReputation>(_state.Reputations.Count),
                Favors = new List<FavorObligation>(_state.Favors.Count)
            };

            foreach (var o in _state.Offers)
            {
                capture.Offers.Add(new BarterOffer
                {
                    OfferId = o.OfferId,
                    OffererId = o.OffererId,
                    TargetSurvivorId = o.TargetSurvivorId,
                    OfferedItemIds = new List<string>(o.OfferedItemIds),
                    RequestedItemIds = new List<string>(o.RequestedItemIds),
                    OfferedFavor = o.OfferedFavor,
                    RequestedFavor = o.RequestedFavor,
                    CreatedDay = o.CreatedDay,
                    ExpiresDay = o.ExpiresDay,
                    Status = o.Status
                });
            }

            foreach (var t in _state.CompletedTrades)
            {
                capture.CompletedTrades.Add(new CompletedBarterTrade
                {
                    TradeId = t.TradeId,
                    OfferId = t.OfferId,
                    TraderAId = t.TraderAId,
                    TraderBId = t.TraderBId,
                    TradeType = t.TradeType,
                    ItemsFromA = new List<string>(t.ItemsFromA),
                    ItemsFromB = new List<string>(t.ItemsFromB),
                    TradeDay = t.TradeDay
                });
            }

            foreach (var r in _state.Reputations)
            {
                capture.Reputations.Add(new TradeReputation
                {
                    TraderAId = r.TraderAId,
                    TraderBId = r.TraderBId,
                    TrustLevel = r.TrustLevel,
                    TradesCompleted = r.TradesCompleted,
                    DisputesCount = r.DisputesCount
                });
            }

            foreach (var f in _state.Favors)
            {
                capture.Favors.Add(new FavorObligation
                {
                    FavorId = f.FavorId,
                    DebtorId = f.DebtorId,
                    CreditorId = f.CreditorId,
                    Favor = f.Favor,
                    Description = f.Description,
                    CreatedDay = f.CreatedDay,
                    DueDay = f.DueDay,
                    IsFulfilled = f.IsFulfilled,
                    FulfilledDay = f.FulfilledDay
                });
            }

            return capture;
        }

        public void RestoreState(SurvivorBarterSaveState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            _state.SchemaVersion = state.SchemaVersion;
            _state.NextSequence = state.NextSequence;
            _state.ActiveRuleId = string.IsNullOrEmpty(state.ActiveRuleId) ? "standard_community_barter" : state.ActiveRuleId;
            _state.Offers.Clear();
            _state.CompletedTrades.Clear();
            _state.Reputations.Clear();
            _state.Favors.Clear();

            if (state.Offers != null)
            {
                foreach (var o in state.Offers)
                {
                    _state.Offers.Add(new BarterOffer
                    {
                        OfferId = o.OfferId,
                        OffererId = o.OffererId,
                        TargetSurvivorId = o.TargetSurvivorId,
                        OfferedItemIds = new List<string>(o.OfferedItemIds ?? new List<string>()),
                        RequestedItemIds = new List<string>(o.RequestedItemIds ?? new List<string>()),
                        OfferedFavor = o.OfferedFavor,
                        RequestedFavor = o.RequestedFavor,
                        CreatedDay = o.CreatedDay,
                        ExpiresDay = o.ExpiresDay,
                        Status = o.Status
                    });
                }
            }

            if (state.CompletedTrades != null)
            {
                foreach (var t in state.CompletedTrades)
                {
                    _state.CompletedTrades.Add(new CompletedBarterTrade
                    {
                        TradeId = t.TradeId,
                        OfferId = t.OfferId,
                        TraderAId = t.TraderAId,
                        TraderBId = t.TraderBId,
                        TradeType = t.TradeType,
                        ItemsFromA = new List<string>(t.ItemsFromA ?? new List<string>()),
                        ItemsFromB = new List<string>(t.ItemsFromB ?? new List<string>()),
                        TradeDay = t.TradeDay
                    });
                }
            }

            if (state.Reputations != null)
            {
                foreach (var r in state.Reputations)
                {
                    _state.Reputations.Add(new TradeReputation
                    {
                        TraderAId = r.TraderAId,
                        TraderBId = r.TraderBId,
                        TrustLevel = r.TrustLevel,
                        TradesCompleted = r.TradesCompleted,
                        DisputesCount = r.DisputesCount
                    });
                }
            }

            if (state.Favors != null)
            {
                foreach (var f in state.Favors)
                {
                    _state.Favors.Add(new FavorObligation
                    {
                        FavorId = f.FavorId,
                        DebtorId = f.DebtorId,
                        CreditorId = f.CreditorId,
                        Favor = f.Favor,
                        Description = f.Description,
                        CreatedDay = f.CreatedDay,
                        DueDay = f.DueDay,
                        IsFulfilled = f.IsFulfilled,
                        FulfilledDay = f.FulfilledDay
                    });
                }
            }

            OnStateChanged?.Invoke();
        }

        private TradeReputation GetOrCreateReputation(string survivorA, string survivorB)
        {
            var rep = _state.Reputations.FirstOrDefault(r =>
                (string.Equals(r.TraderAId, survivorA, StringComparison.OrdinalIgnoreCase) &&
                 string.Equals(r.TraderBId, survivorB, StringComparison.OrdinalIgnoreCase)) ||
                (string.Equals(r.TraderAId, survivorB, StringComparison.OrdinalIgnoreCase) &&
                 string.Equals(r.TraderBId, survivorA, StringComparison.OrdinalIgnoreCase)));

            if (rep == null)
            {
                rep = new TradeReputation
                {
                    TraderAId = survivorA,
                    TraderBId = survivorB,
                    TrustLevel = 50f,
                    TradesCompleted = 0,
                    DisputesCount = 0
                };
                _state.Reputations.Add(rep);
            }
            return rep;
        }
    }
}
