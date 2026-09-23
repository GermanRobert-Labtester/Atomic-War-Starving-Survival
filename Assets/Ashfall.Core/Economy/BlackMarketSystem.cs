// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Factions;

namespace Ashfall.Core.Economy
{
    // ── Serialized state ─────────────────────────────────────────

    /// <summary>Scoped underworld standing (D3 — what faction trust cannot express).</summary>
    [Serializable]
    public sealed class UnderworldLedgerState
    {
        public string syndicateId = string.Empty;
        public bool discovered = false;
        public int discoveredDay = -1;
        public float trust = 0f;          // 0..100 (DebtReliability + standing mirror)
        public float heat = 0f;           // 0..100 (attention from patrols/collectors)
        public int accessTier = 0;        // 0 = unknown, 1..3 after discovery
        public int lastStockRefreshDay = -1;
    }

    /// <summary>One underworld loan (Plan 211 §171.10).</summary>
    [Serializable]
    public sealed class UnderworldDebtRecord
    {
        public const string StatusActive = "active";
        public const string StatusRepaid = "repaid";
        public const string StatusDefaulted = "defaulted";

        public string debtId = string.Empty;
        public string syndicateId = string.Empty;
        public float principalUnits = 0f;
        public float repaidUnits = 0f;
        public int interestBp = 0;        // on top of the outstanding remainder at due day
        public int issuedDay = 0;
        public int dueDay = 1;
        public string status = StatusActive;
        public string reason = string.Empty;
    }

    /// <summary>One persisted daily stock line (no reroll on reopen — §171.7).</summary>
    [Serializable]
    public sealed class BlackMarketStockLine
    {
        public string entryId = string.Empty;
        public int quantity = 0;
        public int generatedDay = 0;
    }

    /// <summary>Versioned black-market save state.</summary>
    [Serializable]
    public sealed class BlackMarketState
    {
        public const int CurrentVersion = 1;

        public int schemaVersion = CurrentVersion;
        public List<UnderworldLedgerState> syndicates = new List<UnderworldLedgerState>();
        public List<UnderworldDebtRecord> debts = new List<UnderworldDebtRecord>();
        public List<BlackMarketStockLine> stock = new List<BlackMarketStockLine>();   // flat snapshot; keyed by (syndicate, entry)
        public List<string> stockOwners = new List<string>();                        // syndicateId per stock line (parallel list)
        public List<string> firedEventKeys = new List<string>();                     // debt-event idempotency
        public int lastTickDay = -1;
        // Plan 211 §171.18 — syndicate bounty persistence. The canonical
        // FactionBountySystem state rides this additive field until a
        // dedicated faction-bounty save section exists (null = legacy clean).
        public Factions.FactionBountySystemState? factionBounties = null;
    }

    /// <summary>Quote for one entry on one syndicate's counter (canonical-derived).</summary>
    public readonly struct BlackMarketQuote
    {
        public readonly bool Valid;
        public readonly string EntryId;
        public readonly int Quantity;
        public readonly float UnitPrice;
        public readonly float CanonicalUnitPrice;
        public readonly float TotalValue;
        public readonly string RejectReason;

        public BlackMarketQuote(bool valid, string entryId, int quantity, float unitPrice,
            float canonicalUnitPrice, float totalValue, string rejectReason)
        {
            Valid = valid;
            EntryId = entryId;
            Quantity = quantity;
            UnitPrice = unitPrice;
            CanonicalUnitPrice = canonicalUnitPrice;
            TotalValue = totalValue;
            RejectReason = rejectReason ?? string.Empty;
        }
    }

    /// <summary>Side-effect-free loan preflight owned by the black-market authority.</summary>
    public readonly struct BlackMarketLoanQuote
    {
        public readonly bool Valid;
        public readonly string SyndicateId;
        public readonly float Units;
        public readonly int DurationDays;
        public readonly int DueDay;
        public readonly int InterestBp;
        public readonly string RejectReason;

        public BlackMarketLoanQuote(bool valid, string syndicateId, float units, int durationDays,
            int dueDay, int interestBp, string rejectReason)
        {
            Valid = valid;
            SyndicateId = syndicateId ?? string.Empty;
            Units = units;
            DurationDays = durationDays;
            DueDay = dueDay;
            InterestBp = interestBp;
            RejectReason = rejectReason ?? string.Empty;
        }
    }

    /// <summary>Side-effect-free active-debt repayment preflight.</summary>
    public readonly struct BlackMarketRepayQuote
    {
        public readonly bool Valid;
        public readonly string DebtId;
        public readonly float RequestedUnits;
        public readonly float AppliedUnits;
        public readonly float OutstandingUnits;
        public readonly bool CompletesDebt;
        public readonly string RejectReason;

        public BlackMarketRepayQuote(bool valid, string debtId, float requestedUnits,
            float appliedUnits, float outstandingUnits, bool completesDebt, string rejectReason)
        {
            Valid = valid;
            DebtId = debtId ?? string.Empty;
            RequestedUnits = requestedUnits;
            AppliedUnits = appliedUnits;
            OutstandingUnits = outstandingUnits;
            CompletesDebt = completesDebt;
            RejectReason = rejectReason ?? string.Empty;
        }
    }

    /// <summary>
    /// Plan 211 — BLACK MARKET &amp; UNDERWORLD SYNDICATES authority.
    ///
    /// Coordinates black-market access, syndicate stock, loans/debt, heat and
    /// underworld reputation. Uses (never duplicates):
    /// <list type="bullet">
    /// <item><see cref="MarketSystem"/> — the canonical item value; black-market
    /// pricing = dynamic value × risk premium × reputation modifier. No static
    /// duplicate prices, no second price authority.</item>
    /// <item><see cref="FactionBountySystem"/> — bounty escalation is delegated
    /// (no second bounty ledger).</item>
    /// <item><see cref="FactionStandingIdResolver"/> — syndicates are real
    /// faction ids (D4); trust/heat here is the scoped dimension normal
    /// standing cannot express (D3).</item>
    /// </list>
    ///
    /// Determinism: stock snapshots are generated per day from the seeded RNG
    /// and PERSISTED — reopening the counter never rerolls stock. Debt
    /// collection events fire exactly once per debt via the fired-key ledger.
    /// </summary>
    public sealed class BlackMarketSystem
    {
        public const string SystemId = "black_market";

        /// <summary>Trust modifier envelope: up to −20% buy price at trust 100.</summary>
        public const float TrustDiscountAtMax = 0.20f;
        /// <summary>Broker-precedent floor: illicit buy premium can never undercut the contraband broker's 1.25×.</summary>
        public const int MinEffectivePremiumBp = 12500 - 10000;   // ≥ +25% over canonical
        public const float HeatMax = 100f;
        public const float TrustMax = 100f;

        private readonly BlackMarketState _state;
        private BlackMarketInventoryCatalog _catalog = new BlackMarketInventoryCatalog();
        private MarketSystem? _market;
        private FactionBountySystem? _bounties;
        private Func<string, string>? _factionResolver = id =>
            Ashfall.Core.Factions.FactionStandingIdResolver.ToSystemsId(id);

        public event Action<string>? OnContactDiscovered;          // syndicateId
        public event Action<string, int, int>? OnStockRefreshed;   // syndicateId, day, lineCount
        public event Action<UnderworldDebtRecord>? OnDebtIssued;
        public event Action<UnderworldDebtRecord, float>? OnDebtRepaid;   // debt, amountRepaid
        public event Action<UnderworldDebtRecord>? OnDebtOverdue;
        public event Action<UnderworldDebtRecord>? OnBountyPlaced;
        public event Action<BlackMarketState>? OnStateChanged;

        public BlackMarketSystem(BlackMarketState? state = null)
        {
            _state = state ?? new BlackMarketState();
            NormalizeState();
        }

        private void NormalizeState()
        {
            if (_state.schemaVersion < 1 || _state.schemaVersion > BlackMarketState.CurrentVersion)
                _state.schemaVersion = BlackMarketState.CurrentVersion;
            _state.syndicates ??= new List<UnderworldLedgerState>();
            _state.debts ??= new List<UnderworldDebtRecord>();
            _state.stock ??= new List<BlackMarketStockLine>();
            _state.stockOwners ??= new List<string>();
            _state.firedEventKeys ??= new List<string>();
        }

        public BlackMarketState State => _state;

        // ── Binding ─────────────────────────────────────────────────

        public void BindCatalog(BlackMarketInventoryCatalog catalog)
        {
            if (catalog != null) _catalog = catalog;
        }

        /// <summary>
        /// Bind the canonical market. REQUIRED before any pricing query — the
        /// black market never computes its own item values.
        /// </summary>
        public void BindMarket(MarketSystem market)
        {
            if (market != null)
            {
                _market = market;
                ValidateEntryItems();
            }
        }

        public void BindFactionBountySystem(FactionBountySystem bounties)
        {
            if (bounties != null) _bounties = bounties;
        }

        /// <summary>Item-id cross-reference (bind-time): unknown items cannot become stock.</summary>
        public IReadOnlyList<string> ValidationErrors { get; private set; } = Array.Empty<string>();

        private void ValidateEntryItems()
        {
            var errors = new List<string>();
            if (_market == null) return;
            foreach (var entry in _catalog.EntriesById.Values)
            {
                if (entry == null) continue;
                if (float.IsNaN(_market.GetPrice(entry.item_id)))
                    errors.Add($"black-market entry '{entry.entry_id}' references item '{entry.item_id}' absent from economy_goods.json");
            }
            ValidationErrors = errors;
        }

        public BlackMarketInventoryCatalog Catalog => _catalog;

        // ── Access / discovery ──────────────────────────────────────

        public UnderworldLedgerState EnsureLedger(string syndicateId)
        {
            var found = FindLedger(syndicateId);
            if (found != null) return found;
            var ledger = new UnderworldLedgerState { syndicateId = syndicateId };
            _state.syndicates.Add(ledger);
            return ledger;
        }

        public UnderworldLedgerState? FindLedger(string syndicateId)
        {
            foreach (var s in _state.syndicates)
                if (s != null && s.syndicateId == syndicateId) return s;
            return null;
        }

        /// <summary>Campaign-gate contact discovery — not a clock check.</summary>
        public bool DiscoverContact(string syndicateId, int day)
        {
            if (_catalog.FindSyndicate(syndicateId) == null) return false;
            var ledger = EnsureLedger(syndicateId);
            if (ledger.discovered) return true;
            var profile = _catalog.FindSyndicate(syndicateId)!;
            ledger.discovered = true;
            ledger.discoveredDay = day;
            ledger.accessTier = profile.required_access_tier;
            OnContactDiscovered?.Invoke(syndicateId);
            return true;
        }

        public bool IsContactDiscovered(string syndicateId) =>
            FindLedger(syndicateId)?.discovered == true;

        /// <summary>All discovered syndicate ids (ordinal-sorted for stable UI).</summary>
        public IReadOnlyList<string> DiscoveredContacts =>
            _state.syndicates.Where(s => s != null && s.discovered)
                .Select(s => s.syndicateId).OrderBy(s => s, StringComparer.Ordinal).ToList();

        // ── Stock ────────────────────────────────────────────────────

        /// <summary>
        /// Daily-persistent stock snapshot. Generated once per (syndicate,
        /// day) from the seeded RNG and stored in state; a same-day reopen
        /// returns the persisted lines without rerolling (§171.7 / R4).
        /// </summary>
        public IReadOnlyList<BlackMarketStockLine> EnsureStockSnapshot(string syndicateId, int day, ISeededRng rng)
        {
            var ledger = FindLedger(syndicateId);
            if (ledger == null || !ledger.discovered || rng == null)
                return Array.Empty<BlackMarketStockLine>();

            if (ledger.lastStockRefreshDay == day)
                return GetStock(syndicateId);   // same day: never reroll

            // New day: replace this syndicate's lines deterministically.
            for (int i = _state.stockOwners.Count - 1; i >= 0; i--)
            {
                if (_state.stockOwners[i] == syndicateId)
                {
                    _state.stockOwners.RemoveAt(i);
                    _state.stock.RemoveAt(i);
                }
            }

            var profile = _catalog.FindSyndicate(syndicateId);
            int tier = ledger.accessTier > 0 ? ledger.accessTier : (profile?.required_access_tier ?? 1);
            int lineCount = 0;
            foreach (var entry in _catalog.EntriesFor(syndicateId))
            {
                if (entry == null || entry.access_tier > tier) continue;
                // Inclusion roll: seeded, bounded weight.
                if (rng.Next(0, 100) >= Math.Clamp(entry.stock_weight, 0, 100)) continue;
                int qty = entry.min_stock + (entry.max_stock > entry.min_stock
                    ? rng.Next(0, entry.max_stock - entry.min_stock + 1)
                    : 0);
                if (qty <= 0) continue;
                _state.stock.Add(new BlackMarketStockLine
                {
                    entryId = entry.entry_id,
                    quantity = qty,
                    generatedDay = day
                });
                _state.stockOwners.Add(syndicateId);
                lineCount++;
            }
            ledger.lastStockRefreshDay = day;
            OnStockRefreshed?.Invoke(syndicateId, day, lineCount);
            return GetStock(syndicateId);
        }

        public IReadOnlyList<BlackMarketStockLine> GetStock(string syndicateId)
        {
            var list = new List<BlackMarketStockLine>();
            for (int i = 0; i < _state.stockOwners.Count && i < _state.stock.Count; i++)
            {
                if (_state.stockOwners[i] == syndicateId && _state.stock[i] != null)
                    list.Add(_state.stock[i]);
            }
            list.Sort((a, b) => string.CompareOrdinal(a.entryId, b.entryId));
            return list;
        }

        public BlackMarketStockLine? GetStockLine(string syndicateId, string entryId)
        {
            foreach (var line in GetStock(syndicateId))
                if (line.entryId == entryId) return line;
            return null;
        }

        // ── Pricing (canonical-derived) ─────────────────────────────

        /// <summary>
        /// Effective buy price for one unit: canonical market value × (1 +
        /// risk premium + scarcity response) × trust discount. Never below
        /// the canonical value — illicit goods always cost more (no
        /// arbitrage against the ordinary merchants).
        /// </summary>
        public float GetBuyPrice(string syndicateId, BlackMarketEntryDefinition entry)
        {
            if (entry == null || _market == null) return float.NaN;
            var profile = _catalog.FindSyndicate(syndicateId);
            if (profile == null) return float.NaN;

            float canonical = _market.GetPrice(entry.item_id);
            if (float.IsNaN(canonical)) return float.NaN;

            // Scarcity response: category index deviation × sensitivity.
            float categoryIndex = _market.GetEffectiveCategoryMultiplierForItem(entry.item_id);
            float scarcityResponse = 1f
                + Math.Clamp(entry.scarcity_sensitivity, 0, 100) / 100f * (categoryIndex - 1f);

            // Trust discount: better standing, gentler premium.
            var ledger = FindLedger(syndicateId);
            float trust = ledger?.trust ?? 0f;
            float trustFactor = 1f - Math.Clamp(trust, 0f, TrustMax) / TrustMax * TrustDiscountAtMax;

            float premium = 1f + profile.base_premium_bp / 10000f
                + Math.Clamp(entry.risk_premium_bp, 0, BlackMarketInventoryCatalogLoader.MaxPremiumBp) / 10000f;
            float price = canonical * premium * scarcityResponse * trustFactor;

            // Hard floor: never below the canonical value (+25% broker band).
            float floor = canonical * (1f + (MinEffectivePremiumBp - 10000) / 10000f);
            return Math.Max(price, floor);
        }

        /// <summary>
        /// Effective sell price when the syndicate buys FROM the player:
        /// canonical × (1 − sell discount) — dumping contraband always loses
        /// value against the market (arbitrage guard, R6).
        /// </summary>
        public float GetSellPrice(string syndicateId, BlackMarketEntryDefinition entry)
        {
            if (entry == null || _market == null) return float.NaN;
            var profile = _catalog.FindSyndicate(syndicateId);
            if (profile == null) return float.NaN;
            float canonical = _market.GetPrice(entry.item_id);
            if (float.IsNaN(canonical)) return float.NaN;
            float price = canonical * (1f - Math.Clamp(profile.sell_discount_bp, 0, 5000) / 10000f);
            return Math.Max(0f, price);
        }

        // ── Transactions (atomic — §E) ──────────────────────────────

        /// <summary>
        /// Side-effect-free buy preflight. Funds and inventory policy remain
        /// outside this authority and are composed by the settlement owner.
        /// </summary>
        public BlackMarketQuote PreviewBuy(string syndicateId, string entryId, int quantity, int day)
        {
            var entry = _catalog.FindEntry(entryId);
            var ledger = FindLedger(syndicateId);
            if (entry == null) return Reject(entryId, "unknown_entry");
            if (quantity <= 0) return Reject(entryId, "quantity_must_be_positive");
            if (ledger == null || !ledger.discovered) return Reject(entryId, "contact_not_discovered");
            if (entry.access_tier > ledger.accessTier) return Reject(entryId, "access_tier_too_low");
            var line = GetStockLine(syndicateId, entryId);
            if (line == null) return Reject(entryId, "out_of_stock");
            if (line.quantity < quantity) return Reject(entryId, "insufficient_stock");

            float unitPrice = GetBuyPrice(syndicateId, entry);
            if (float.IsNaN(unitPrice)) return Reject(entryId, "unpriced");

            return new BlackMarketQuote(true, entryId, quantity, unitPrice,
                _market!.GetPrice(entry.item_id), unitPrice * quantity, string.Empty);
        }

        /// <summary>Side-effect-free sell preflight.</summary>
        public BlackMarketQuote PreviewSell(string syndicateId, string entryId, int quantity, int day)
        {
            var entry = _catalog.FindEntry(entryId);
            var ledger = FindLedger(syndicateId);
            if (entry == null) return Reject(entryId, "unknown_entry");
            if (quantity <= 0) return Reject(entryId, "quantity_must_be_positive");
            if (ledger == null || !ledger.discovered) return Reject(entryId, "contact_not_discovered");
            if (entry.access_tier > ledger.accessTier) return Reject(entryId, "access_tier_too_low");

            float unitPrice = GetSellPrice(syndicateId, entry);
            if (float.IsNaN(unitPrice)) return Reject(entryId, "unpriced_entry");

            return new BlackMarketQuote(true, entryId, quantity, unitPrice,
                _market!.GetPrice(entry.item_id), unitPrice * quantity, string.Empty);
        }

        /// <summary>
        /// Buy from the counter. Black-market stock is staged first so the
        /// supplied funds/goods settlement observes the final domain state.
        /// A rejected or throwing settlement restores stock before returning.
        /// </summary>
        public BlackMarketQuote Buy(string syndicateId, string entryId, int quantity, int day) =>
            Buy(syndicateId, entryId, quantity, day, null);

        public BlackMarketQuote Buy(string syndicateId, string entryId, int quantity, int day,
            Func<BlackMarketQuote, bool>? settle)
        {
            var quote = PreviewBuy(syndicateId, entryId, quantity, day);
            if (!quote.Valid) return quote;

            var line = GetStockLine(syndicateId, entryId)!;
            int previousQuantity = line.quantity;
            line.quantity -= quantity;
            try
            {
                if (settle != null && !settle(quote))
                {
                    line.quantity = previousQuantity;
                    return Reject(entryId, "settlement_failed");
                }
            }
            catch
            {
                line.quantity = previousQuantity;
                throw;
            }

            OnStateChanged?.Invoke(_state);
            return quote;
        }

        /// <summary>
        /// Sell to the counter. Stock growth is staged and rolled back if the
        /// canonical inventory/wallet settlement cannot commit.
        /// </summary>
        public BlackMarketQuote Sell(string syndicateId, string entryId, int quantity, int day) =>
            Sell(syndicateId, entryId, quantity, day, null);

        public BlackMarketQuote Sell(string syndicateId, string entryId, int quantity, int day,
            Func<BlackMarketQuote, bool>? settle)
        {
            var quote = PreviewSell(syndicateId, entryId, quantity, day);
            if (!quote.Valid) return quote;

            var line = GetStockLine(syndicateId, entryId);
            bool createdLine = line == null;
            int previousQuantity = line?.quantity ?? 0;
            if (line != null)
            {
                line.quantity += quantity;
            }
            else
            {
                line = new BlackMarketStockLine { entryId = entryId, quantity = quantity, generatedDay = day };
                _state.stock.Add(line);
                _state.stockOwners.Add(syndicateId);
            }

            try
            {
                if (settle != null && !settle(quote))
                {
                    RollbackSellLine(syndicateId, line, createdLine, previousQuantity);
                    return Reject(entryId, "settlement_failed");
                }
            }
            catch
            {
                RollbackSellLine(syndicateId, line, createdLine, previousQuantity);
                throw;
            }

            OnStateChanged?.Invoke(_state);
            return quote;
        }

        private void RollbackSellLine(string syndicateId, BlackMarketStockLine line,
            bool createdLine, int previousQuantity)
        {
            if (!createdLine)
            {
                line.quantity = previousQuantity;
                return;
            }

            for (int i = _state.stock.Count - 1; i >= 0; i--)
            {
                if (ReferenceEquals(_state.stock[i], line) && i < _state.stockOwners.Count &&
                    _state.stockOwners[i] == syndicateId)
                {
                    _state.stock.RemoveAt(i);
                    _state.stockOwners.RemoveAt(i);
                    return;
                }
            }
        }

        private static BlackMarketQuote Reject(string entryId, string reason) =>
            new BlackMarketQuote(false, entryId, 0, 0f, 0f, 0f, reason);

        // ── Loans / debt (§171.10–171.11) ───────────────────────────

        /// <summary>Side-effect-free loan eligibility and due-date projection.</summary>
        public BlackMarketLoanQuote PreviewLoan(string syndicateId, float units, int day, int durationDays)
        {
            var profile = _catalog.FindSyndicate(syndicateId);
            var ledger = FindLedger(syndicateId);
            if (profile == null) return RejectLoan(syndicateId, "unknown_syndicate");
            if (ledger == null || !ledger.discovered) return RejectLoan(syndicateId, "contact_not_discovered");
            if (units <= 0f) return RejectLoan(syndicateId, "amount_must_be_positive");
            if (durationDays < 1) durationDays = 1;
            if (units > profile.credit_limit_units) return RejectLoan(syndicateId, "credit_limit_exceeded");
            if (_state.debts.Any(d => d != null && d.syndicateId == syndicateId && d.status == UnderworldDebtRecord.StatusActive))
                return RejectLoan(syndicateId, "active_loan_exists");

            return new BlackMarketLoanQuote(true, syndicateId, units, durationDays,
                day + durationDays, profile.loan_interest_bp, string.Empty);
        }

        /// <summary>Take a resource loan. Credit limit = profile credit limit; one active loan per syndicate.</summary>
        public UnderworldDebtRecord? TakeLoan(string syndicateId, float units, int day, int durationDays) =>
            TakeLoan(syndicateId, units, day, durationDays, null);

        public UnderworldDebtRecord? TakeLoan(string syndicateId, float units, int day, int durationDays,
            Func<BlackMarketLoanQuote, bool>? settle)
        {
            var preview = PreviewLoan(syndicateId, units, day, durationDays);
            if (!preview.Valid) return null;
            var ledger = FindLedger(syndicateId)!;
            float previousTrust = ledger.trust;

            var debt = new UnderworldDebtRecord
            {
                debtId = $"debt_{syndicateId}_{day}_{_state.debts.Count + 1}",
                syndicateId = syndicateId,
                principalUnits = units,
                interestBp = preview.InterestBp,
                issuedDay = day,
                dueDay = preview.DueDay,
                reason = "underworld_loan"
            };
            _state.debts.Add(debt);
            ledger.trust = Math.Min(TrustMax, ledger.trust + 10f);   // credit extended builds trust

            try
            {
                if (settle != null && !settle(preview))
                {
                    _state.debts.Remove(debt);
                    ledger.trust = previousTrust;
                    return null;
                }
            }
            catch
            {
                _state.debts.Remove(debt);
                ledger.trust = previousTrust;
                throw;
            }

            OnDebtIssued?.Invoke(debt);
            OnStateChanged?.Invoke(_state);
            return debt;
        }

        private static BlackMarketLoanQuote RejectLoan(string syndicateId, string reason) =>
            new BlackMarketLoanQuote(false, syndicateId, 0f, 0, 0, 0, reason);

        public float OutstandingOnDebt(UnderworldDebtRecord debt)
        {
            if (debt == null) return 0f;
            float remaining = debt.principalUnits - debt.repaidUnits;
            if (debt.status == UnderworldDebtRecord.StatusDefaulted) return remaining * (1f + debt.interestBp / 10000f);
            if (debt.status != UnderworldDebtRecord.StatusActive) return 0f;
            return remaining;
        }

        /// <summary>Side-effect-free active-debt repayment preflight.</summary>
        public BlackMarketRepayQuote PreviewRepay(string debtId, float amount, int day)
        {
            if (string.IsNullOrEmpty(debtId)) return RejectRepay(debtId, "unknown_debt");
            if (amount <= 0f) return RejectRepay(debtId, "amount_must_be_positive");
            var debt = _state.debts.FirstOrDefault(d => d != null && d.debtId == debtId);
            if (debt == null) return RejectRepay(debtId, "unknown_debt");
            if (debt.status != UnderworldDebtRecord.StatusActive) return RejectRepay(debtId, "debt_not_active");

            float outstanding = OutstandingOnDebt(debt);
            float applied = Math.Min(amount, outstanding);
            return new BlackMarketRepayQuote(true, debtId, amount, applied, outstanding,
                applied >= outstanding - 1e-4f, string.Empty);
        }

        /// <summary>Repay an active debt. Accepts partial repayment.</summary>
        public bool RepayDebt(string debtId, float amount, int day) =>
            RepayDebt(debtId, amount, day, null);

        public bool RepayDebt(string debtId, float amount, int day,
            Func<BlackMarketRepayQuote, bool>? settle)
        {
            var preview = PreviewRepay(debtId, amount, day);
            if (!preview.Valid) return false;
            var debt = _state.debts.First(d => d != null && d.debtId == debtId)!;
            var ledger = FindLedger(debt.syndicateId);
            float previousRepaid = debt.repaidUnits;
            string previousStatus = debt.status;
            float previousTrust = ledger?.trust ?? 0f;

            debt.repaidUnits += Math.Min(amount, debt.principalUnits - debt.repaidUnits);
            if (debt.repaidUnits >= debt.principalUnits - 1e-4f)
            {
                debt.status = UnderworldDebtRecord.StatusRepaid;
                if (ledger != null) ledger.trust = Math.Min(TrustMax, ledger.trust + 15f);
            }

            try
            {
                if (settle != null && !settle(preview))
                {
                    debt.repaidUnits = previousRepaid;
                    debt.status = previousStatus;
                    if (ledger != null) ledger.trust = previousTrust;
                    return false;
                }
            }
            catch
            {
                debt.repaidUnits = previousRepaid;
                debt.status = previousStatus;
                if (ledger != null) ledger.trust = previousTrust;
                throw;
            }

            OnDebtRepaid?.Invoke(debt, preview.AppliedUnits);
            OnStateChanged?.Invoke(_state);
            return true;
        }

        private static BlackMarketRepayQuote RejectRepay(string debtId, string reason) =>
            new BlackMarketRepayQuote(false, debtId, 0f, 0f, 0f, false, reason);

        // ── Daily tick ───────────────────────────────────────────────

        /// <summary>
        /// Advance one day: debt due/overdue checks (idempotent per debt via
        /// the fired-key ledger), trust/heat consequences, bounty escalation
        /// through the bound FactionBountySystem, heat decay.
        /// </summary>
        public void TickDaily(int day)
        {
            if (_state.lastTickDay == day) return;
            _state.lastTickDay = day;

            foreach (var debt in _state.debts)
            {
                if (debt == null || debt.status != UnderworldDebtRecord.StatusActive) continue;
                if (day < debt.dueDay) continue;

                string eventKey = $"debt_overdue:{debt.debtId}";
                if (_state.firedEventKeys.Contains(eventKey)) continue;   // idempotent (§171.19 #15)
                _state.firedEventKeys.Add(eventKey);

                debt.status = UnderworldDebtRecord.StatusDefaulted;
                var ledger = FindLedger(debt.syndicateId);
                if (ledger != null)
                {
                    ledger.trust = Math.Max(0f, ledger.trust - 30f);
                    ledger.heat = Math.Min(HeatMax, ledger.heat + 25f);
                }
                OnDebtOverdue?.Invoke(debt);

                // Bounty escalation — delegated to the canonical bounty owner.
                if (_bounties != null)
                {
                    float outstanding = OutstandingOnDebt(debt);
                    int severityDelta = outstanding > 600f ? -20 : outstanding > 300f ? -15 : -10;
                    var bounty = _bounties.IssuePatrolBounty(debt.syndicateId,
                        encounterId: "underworld_debt", choiceId: "defaulted_loan",
                        authoredStandingDelta: severityDelta, day: day);
                    if (bounty != null) OnBountyPlaced?.Invoke(debt);
                }
            }

            // Heat decays daily (bounded).
            foreach (var ledger in _state.syndicates)
            {
                if (ledger == null) continue;
                var profile = _catalog.FindSyndicate(ledger.syndicateId);
                float decay = profile?.heat_decay_per_day ?? 0f;
                ledger.heat = Math.Max(0f, ledger.heat - decay);
            }
        }

        // ── Save / restore ───────────────────────────────────────────

        public BlackMarketState CaptureState()
        {
            var copy = new BlackMarketState
            {
                schemaVersion = BlackMarketState.CurrentVersion,
                lastTickDay = _state.lastTickDay
            };
            foreach (var ledger in _state.syndicates.OrderBy(s => s.syndicateId, StringComparer.Ordinal))
            {
                if (ledger == null) continue;
                copy.syndicates.Add(new UnderworldLedgerState
                {
                    syndicateId = ledger.syndicateId,
                    discovered = ledger.discovered,
                    discoveredDay = ledger.discoveredDay,
                    trust = Math.Clamp(ledger.trust, 0f, TrustMax),
                    heat = Math.Clamp(ledger.heat, 0f, HeatMax),
                    accessTier = Math.Clamp(ledger.accessTier, 0, BlackMarketInventoryCatalogLoader.MaxAccessTier),
                    lastStockRefreshDay = ledger.lastStockRefreshDay
                });
            }
            foreach (var debt in _state.debts.OrderBy(d => d.issuedDay).ThenBy(d => d.debtId, StringComparer.Ordinal))
            {
                if (debt == null) continue;
                copy.debts.Add(new UnderworldDebtRecord
                {
                    debtId = debt.debtId,
                    syndicateId = debt.syndicateId,
                    principalUnits = Math.Max(0f, debt.principalUnits),
                    repaidUnits = Math.Clamp(debt.repaidUnits, 0f, Math.Max(0f, debt.principalUnits)),
                    interestBp = Math.Clamp(debt.interestBp, 0, 5000),
                    issuedDay = Math.Max(0, debt.issuedDay),
                    dueDay = Math.Max(1, debt.dueDay),
                    status = debt.status ?? UnderworldDebtRecord.StatusActive,
                    reason = debt.reason ?? string.Empty
                });
            }
            for (int i = 0; i < _state.stock.Count; i++)
            {
                var line = _state.stock[i];
                var owner = i < _state.stockOwners.Count ? _state.stockOwners[i] : string.Empty;
                if (line == null || string.IsNullOrEmpty(line.entryId)) continue;
                copy.stock.Add(new BlackMarketStockLine
                {
                    entryId = line.entryId,
                    quantity = Math.Max(0, line.quantity),
                    generatedDay = Math.Max(0, line.generatedDay)
                });
                copy.stockOwners.Add(owner ?? string.Empty);
            }
            var seenKeys = new HashSet<string>(StringComparer.Ordinal);
            foreach (var key in _state.firedEventKeys)
            {
                if (!string.IsNullOrEmpty(key) && seenKeys.Add(key)) copy.firedEventKeys.Add(key);
            }
            copy.factionBounties = _bounties?.CaptureState();
            return copy;
        }

        public void RestoreState(BlackMarketState? saved)
        {
            if (saved == null) return;
            if (saved.schemaVersion > BlackMarketState.CurrentVersion)
                throw new InvalidOperationException(
                    $"black market save version {saved.schemaVersion} is newer than supported ({BlackMarketState.CurrentVersion})");

            _state.schemaVersion = BlackMarketState.CurrentVersion;
            _state.syndicates.Clear();
            if (saved.syndicates != null)
            {
                var seen = new HashSet<string>(StringComparer.Ordinal);
                foreach (var s in saved.syndicates)
                {
                    if (s == null || string.IsNullOrEmpty(s.syndicateId) || !seen.Add(s.syndicateId)) continue;
                    _state.syndicates.Add(new UnderworldLedgerState
                    {
                        syndicateId = s.syndicateId,
                        discovered = s.discovered,
                        discoveredDay = s.discoveredDay,
                        trust = Math.Clamp(s.trust, 0f, TrustMax),
                        heat = Math.Clamp(s.heat, 0f, HeatMax),
                        accessTier = Math.Clamp(s.accessTier, 0, BlackMarketInventoryCatalogLoader.MaxAccessTier),
                        lastStockRefreshDay = s.lastStockRefreshDay
                    });
                }
            }
            _state.debts.Clear();
            if (saved.debts != null)
            {
                var seen = new HashSet<string>(StringComparer.Ordinal);
                foreach (var d in saved.debts)
                {
                    if (d == null || string.IsNullOrEmpty(d.debtId) || !seen.Add(d.debtId)) continue;
                    _state.debts.Add(new UnderworldDebtRecord
                    {
                        debtId = d.debtId,
                        syndicateId = d.syndicateId ?? string.Empty,
                        principalUnits = Math.Max(0f, d.principalUnits),
                        repaidUnits = Math.Clamp(d.repaidUnits, 0f, Math.Max(0f, d.principalUnits)),
                        interestBp = Math.Clamp(d.interestBp, 0, 5000),
                        issuedDay = Math.Max(0, d.issuedDay),
                        dueDay = Math.Max(1, d.dueDay),
                        status = d.status ?? UnderworldDebtRecord.StatusActive,
                        reason = d.reason ?? string.Empty
                    });
                }
            }
            _state.stock.Clear();
            _state.stockOwners.Clear();
            if (saved.stock != null && saved.stockOwners != null)
            {
                int count = Math.Min(saved.stock.Count, saved.stockOwners.Count);
                for (int i = 0; i < count; i++)
                {
                    var line = saved.stock[i];
                    if (line == null || string.IsNullOrEmpty(line.entryId)) continue;
                    _state.stock.Add(new BlackMarketStockLine
                    {
                        entryId = line.entryId,
                        quantity = Math.Max(0, line.quantity),
                        generatedDay = Math.Max(0, line.generatedDay)
                    });
                    _state.stockOwners.Add(saved.stockOwners[i] ?? string.Empty);
                }
            }
            _state.firedEventKeys.Clear();
            if (saved.firedEventKeys != null)
            {
                var seen = new HashSet<string>(StringComparer.Ordinal);
                foreach (var key in saved.firedEventKeys)
                    if (!string.IsNullOrEmpty(key) && seen.Add(key)) _state.firedEventKeys.Add(key);
            }
            _state.lastTickDay = saved.lastTickDay;
            if (saved.factionBounties != null && _bounties != null)
                _bounties.RestoreState(saved.factionBounties);
            OnStateChanged?.Invoke(_state);
        }
    }
}
