using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Economy
{
    /// <summary>One demand multiplier entry (serializable).</summary>
    [Serializable]
    public class DemandEntry
    {
        public string itemId = string.Empty;
        public float multiplier = 1f;
    }

    /// <summary>One ledger line (serializable, immutable after booking).</summary>
    [Serializable]
    public class LedgerEntry
    {
        public int day = 0;
        public string itemId = string.Empty;
        public int quantity = 0;
        public float unitPrice = 0f;
        public float totalValue = 0f;
        public string counterparty = string.Empty; // faction/person or "market"
    }

    /// <summary>
    /// Versioned market state. Bump <see cref="Version"/> on breaking shape
    /// changes; RestoreState migrates older versions and fails loudly on
    /// NEWER ones (a save from the future must not silently corrupt).
    /// </summary>
    [Serializable]
    public class MarketState
    {
        public const int Version = 1;

        public string systemId = MarketSystem.SystemId;
        public int version = Version;
        public int day = 0;
        public long tickCount = 0;
        public List<DemandEntry> demand = new List<DemandEntry>();
        public List<LedgerEntry> ledger = new List<LedgerEntry>();
    }

    /// <summary>Result of a market transaction.</summary>
    public struct TransactionResult
    {
        public bool Accepted;
        public string ItemId;
        public int Quantity;
        public float UnitPrice;
        public float TotalValue;
        public float RemainderValue; // barter only: offered value not exchanged (whole-unit rule)
        public string RejectReason;
    }

    /// <summary>
    /// Data-driven market/price core (port of the Unity economy's demand model:
    /// per-item demand multipliers clamped to [MinDemandMult, MaxDemandMult],
    /// shortage threshold IsSuppliesShort, AdjustDemand nudges). Adds a
    /// deterministic daily volatility walk — every stochastic decision goes
    /// through the ISeededRng passed per tick, so identical seed + identical
    /// tick sequence produce identical trajectories across process runs.
    /// Prices are never silently rebalanced: the demand clamp constants match
    /// the Unity source exactly.
    /// </summary>
    public class MarketSystem
    {
        public const string SystemId = "economy_market_system";
        public const int MarketStateVersion = MarketState.Version;

        // Unity parity constants (DynamicEconomySystem).
        public const float MinDemandMult = 0.25f;
        public const float MaxDemandMult = 4f;
        public const float ShortageThreshold = 1.35f;
        public const float PriceFloorFraction = 0.25f; // price >= base * floor
        public const float PriceCeilingFraction = 4f;  // price <= base * ceiling

        private readonly MarketState _state;
        private GoodsCatalog _catalog;

        public event Action<string, float> OnDemandAdjusted;     // itemId, delta
        public event Action OnEconomyChanged;                     // any price-relevant change
        public event Action<MarketState> OnStateChanged;

        public MarketSystem(MarketState state = null!)
        {
            _state = state ?? new MarketState();
            if (_state.demand == null) _state.demand = new List<DemandEntry>();
            if (_state.ledger == null) _state.ledger = new List<LedgerEntry>();
        }

        public MarketState State => _state;
        public int Day => _state.day;
        public long TickCount => _state.tickCount;

        // ── Catalog binding ───────────────────────────────────────────

        public void BindCatalog(GoodsCatalog catalog)
        {
            _catalog = catalog;
        }

        public GoodDefinition? FindGood(string itemId) =>
            _catalog != null ? _catalog.Find(itemId) : null;

        // ── Daily tick ────────────────────────────────────────────────

        /// <summary>
        /// Advance one market day: drift each tracked/known good's demand with a
        /// deterministic volatility walk (elasticity-scaled), re-clamp to the
        /// Unity bounds, and record the day. All rolls come from the caller's
        /// ISeededRng (host owns the seed sequence).
        /// </summary>
        public void TickDay(int day, ISeededRng rng)
        {
            if (rng == null) return;
            _state.day = day;
            _state.tickCount++;

            if (_catalog != null)
            {
                foreach (var good in _catalog.All())
                {
                    // Deterministic walk: uniform in [-volatility, +volatility],
                    // scaled by elasticity so inelastic goods move less.
                    double noise = (rng.NextDouble() * 2d - 1d) * good.volatility;
                    float delta = (float)noise * good.elasticity;
                    float current = GetDemandMultiplier(good.id);
                    SetDemandRaw(good.id, Math.Clamp(current + delta, MinDemandMult, MaxDemandMult));
                }
            }
            OnEconomyChanged?.Invoke(); // prices moved
            RaiseChanged();
        }

        // ── Demand / pricing ───────────────────────────────────────────

        public float GetDemandMultiplier(string itemId)
        {
            for (int i = 0; i < _state.demand.Count; i++)
                if (_state.demand[i].itemId == itemId)
                    return Math.Clamp(_state.demand[i].multiplier, MinDemandMult, MaxDemandMult);
            return 1f;
        }

        /// <summary>Nudge global demand (scarcity). Positive = more scarce / valuable. Unity parity.</summary>
        public void AdjustDemand(string itemId, float delta)
        {
            if (string.IsNullOrEmpty(itemId)) return;
            if (Math.Abs(delta) < 1e-6f) return;
            float cur = GetDemandMultiplier(itemId);
            SetDemandRaw(itemId, Math.Clamp(cur + delta, MinDemandMult, MaxDemandMult));
            OnDemandAdjusted?.Invoke(itemId, delta);
            OnEconomyChanged?.Invoke();
            RaiseChanged();
        }

        /// <summary>True when average demand pressure is elevated (Unity parity: >= 1.35).</summary>
        public bool IsSuppliesShort()
        {
            if (_state.demand.Count == 0) return false;
            float sum = 0f;
            for (int i = 0; i < _state.demand.Count; i++)
                sum += _state.demand[i].multiplier;
            return (sum / _state.demand.Count) >= ShortageThreshold;
        }

        /// <summary>Effective unit price for an item: base x demand (elasticity-weighted).</summary>
        /// <returns>Price in currency units, or <see cref="float.NaN"/> if the good is unknown.</returns>
        public float GetPrice(string itemId)
        {
            var good = FindGood(itemId);
            if (good == null) return float.NaN;
            float demand = GetDemandMultiplier(itemId);
            float price = good.basePrice * demand;
            return Math.Clamp(price, good.basePrice * PriceFloorFraction, good.basePrice * PriceCeilingFraction);
        }

        // ── Transactions / barter ──────────────────────────────────────

        /// <summary>
        /// Book a purchase/sale at the current market price. Barter invariant:
        /// the ledger records value in both directions at the same unit price,
        /// so a barter of good A for good B exchanges equal total value.
        /// </summary>
        public TransactionResult Buy(string itemId, int quantity, int day, string counterparty = "market")
        {
            return Transact(itemId, quantity, day, counterparty, isSale: false);
        }

        public TransactionResult Sell(string itemId, int quantity, int day, string counterparty = "market")
        {
            return Transact(itemId, quantity, day, counterparty, isSale: true);
        }

        private TransactionResult Transact(string itemId, int quantity, int day, string counterparty, bool isSale)
        {
            var good = FindGood(itemId);
            if (good == null)
                return Rejected(itemId, "unknown good");
            if (quantity <= 0)
                return Rejected(itemId, "quantity must be > 0");

            float unitPrice = GetPrice(itemId);
            float total = unitPrice * quantity;
            var entry = new LedgerEntry
            {
                day = day,
                itemId = itemId,
                quantity = isSale ? -quantity : quantity,
                unitPrice = unitPrice,
                totalValue = total,
                counterparty = counterparty ?? "market"
            };
            _state.ledger.Add(entry);
            OnEconomyChanged?.Invoke();
            RaiseChanged();
            return new TransactionResult
            {
                Accepted = true,
                ItemId = itemId,
                Quantity = quantity,
                UnitPrice = unitPrice,
                TotalValue = total
            };
        }

        /// <summary>
        /// Barter: exchange goods at current prices. Barter is a whole-unit
        /// exchange, so the take leg is floored to whole items; both ledger
        /// legs book the EXCHANGED value (equal), and the unexchanged
        /// remainder is reported explicitly on the result (it stays on the
        /// table — no value silently disappears).
        /// </summary>
        public TransactionResult Barter(string giveItemId, int giveQuantity, string takeItemId, int day)
        {
            var giveGood = FindGood(giveItemId);
            var takeGood = FindGood(takeItemId);
            if (giveGood == null) return Rejected(giveItemId, "unknown give good");
            if (takeGood == null) return Rejected(takeItemId, "unknown take good");
            if (giveQuantity <= 0) return Rejected(giveItemId, "quantity must be > 0");

            float givePrice = GetPrice(giveItemId);
            float giveValue = givePrice * giveQuantity;
            if (giveValue <= 0f) return Rejected(giveItemId, "zero value");

            float takePrice = GetPrice(takeItemId);
            int takeQuantity = (int)Math.Floor(giveValue / takePrice);
            if (takeQuantity <= 0)
                return Rejected(takeItemId, "take good too valuable for the offered amount");

            // Barter invariant: both legs book the same exchanged total value
            // (equal-value exchange); the remainder is explicit, never dropped.
            float exchangedValue = takePrice * takeQuantity;
            float remainder = giveValue - exchangedValue;

            var takeLeg = Transact(takeItemId, takeQuantity, day, "barter", isSale: false);
            // Give leg books the exchanged value at its effective unit price.
            _state.ledger.Add(new LedgerEntry
            {
                day = day,
                itemId = giveItemId,
                quantity = -giveQuantity,
                unitPrice = exchangedValue / giveQuantity,
                totalValue = exchangedValue,
                counterparty = "barter"
            });
            OnEconomyChanged?.Invoke();
            RaiseChanged();

            return new TransactionResult
            {
                Accepted = true,
                ItemId = takeItemId,
                Quantity = takeQuantity,
                UnitPrice = takeLeg.UnitPrice,
                TotalValue = exchangedValue,
                RemainderValue = remainder
            };
        }

        private static TransactionResult Rejected(string itemId, string reason)
        {
            return new TransactionResult
            {
                Accepted = false,
                ItemId = itemId,
                RejectReason = reason
            };
        }

        // ── Save / Load ────────────────────────────────────────────────

        public MarketState CaptureState()
        {
            var copy = new MarketState
            {
                systemId = _state.systemId,
                version = MarketState.Version,
                day = _state.day,
                tickCount = _state.tickCount
            };
            var demand = new List<DemandEntry>(_state.demand);
            demand.Sort((a, b) => string.CompareOrdinal(a.itemId, b.itemId));
            for (int i = 0; i < demand.Count; i++)
                copy.demand.Add(new DemandEntry
                {
                    itemId = demand[i].itemId,
                    multiplier = Math.Clamp(demand[i].multiplier, MinDemandMult, MaxDemandMult)
                });

            var ledger = new List<LedgerEntry>(_state.ledger);
            ledger.Sort((a, b) =>
            {
                int byDay = a.day.CompareTo(b.day);
                return byDay != 0 ? byDay : string.CompareOrdinal(a.itemId, b.itemId);
            });
            for (int i = 0; i < ledger.Count; i++)
            {
                var e = ledger[i];
                copy.ledger.Add(new LedgerEntry
                {
                    day = e.day,
                    itemId = e.itemId,
                    quantity = e.quantity,
                    unitPrice = e.unitPrice,
                    totalValue = e.totalValue,
                    counterparty = e.counterparty
                });
            }
            return copy;
        }

        /// <summary>
        /// Restore state. Newer versions fail loudly (throws); equal or older
        /// versions migrate predictably (missing demand rows read as 1.0).
        /// </summary>
        public void RestoreState(MarketState saved)
        {
            if (saved == null) return;
            if (saved.version > MarketState.Version)
                throw new InvalidOperationException(
                    $"economy save version {saved.version} is newer than supported ({MarketState.Version})");

            _state.systemId = SystemId;
            _state.version = MarketState.Version;
            _state.day = Math.Max(0, saved.day);
            _state.tickCount = Math.Max(0, saved.tickCount);
            _state.demand.Clear();
            if (saved.demand != null)
            {
                var seenDemand = new HashSet<string>(StringComparer.Ordinal);
                for (int i = 0; i < saved.demand.Count; i++)
                {
                    var d = saved.demand[i];
                    if (d == null || string.IsNullOrEmpty(d.itemId)) continue;
                    // First-wins dedupe: a corrupt save with duplicate rows must
                    // not let GetDemandMultiplier and IsSuppliesShort disagree.
                    if (!seenDemand.Add(d.itemId)) continue;
                    _state.demand.Add(new DemandEntry
                    {
                        itemId = d.itemId,
                        multiplier = Math.Clamp(d.multiplier, MinDemandMult, MaxDemandMult)
                    });
                }
            }
            _state.ledger.Clear();
            if (saved.ledger != null)
            {
                for (int i = 0; i < saved.ledger.Count; i++)
                {
                    var e = saved.ledger[i];
                    if (e == null || string.IsNullOrEmpty(e.itemId)) continue;
                    _state.ledger.Add(new LedgerEntry
                    {
                        day = Math.Max(0, e.day),
                        itemId = e.itemId,
                        quantity = e.quantity,
                        unitPrice = Math.Max(0f, e.unitPrice),
                        totalValue = Math.Max(0f, e.totalValue),
                        counterparty = e.counterparty ?? string.Empty
                    });
                }
            }
            RaiseChanged();
        }

        private void SetDemandRaw(string itemId, float multiplier)
        {
            for (int i = 0; i < _state.demand.Count; i++)
            {
                if (_state.demand[i].itemId == itemId)
                {
                    _state.demand[i].multiplier = multiplier;
                    return;
                }
            }
            _state.demand.Add(new DemandEntry { itemId = itemId, multiplier = multiplier });
        }

        private void RaiseChanged() => OnStateChanged?.Invoke(_state);
    }
}
