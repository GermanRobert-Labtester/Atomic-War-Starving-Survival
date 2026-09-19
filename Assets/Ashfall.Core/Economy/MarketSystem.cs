// SPDX-License-Identifier: MIT
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
    /// Plan 212 — persisted per-category market index (v2). The multiplier is
    /// permille-shaped (1000 = 1.0x) stored as a float multiplier; bounds come
    /// from the bound CommodityBaselineCatalog. No baseline bound → clamp to
    /// [0.2, 5.0] on use so a corrupt row cannot produce a runaway price.
    /// </summary>
    [Serializable]
    public sealed class CategoryIndexEntry
    {
        public string categoryId = string.Empty;
        public float multiplier = 1f;
    }

    /// <summary>
    /// Plan 212 — persisted net trade pressure per category (units bought vs
    /// units sold, decayed daily). Pressure is market flow, never raw player
    /// inventory — owning goods elsewhere must not distort this market.
    /// </summary>
    [Serializable]
    public sealed class TradePressureEntry
    {
        public string categoryId = string.Empty;
        public float buyUnits = 0f;
        public float sellUnits = 0f;
    }

    /// <summary>
    /// Plan 212 — one persisted market shock (shortage raises the index,
    /// crash lowers it). Expires deterministically at expiryDay; shocks are
    /// never silently re-rolled after restore.
    /// </summary>
    [Serializable]
    public sealed class MarketShockState
    {
        public string shockId = string.Empty;
        public string categoryId = string.Empty;
        public bool isShortage = true;
        public float severityBp = 2000f;              // 500..3000 bp (5%..30% effect) clamped on apply
        public int startDay = 0;
        public int expiryDay = 1;                // startDay + durationDays >= 1
        public string sourceId = string.Empty;
    }

    /// <summary>
    /// Versioned market state. Bump <see cref="Version"/> on breaking shape
    /// changes; RestoreState migrates older versions and fails loudly on
    /// NEWER ones (a save from the future must not silently corrupt).
    /// </summary>
    [Serializable]
    public class MarketState
    {
        public const int Version = 3;

        public string systemId = MarketSystem.SystemId;
        public int version = Version;
        public int day = 0;
        public long tickCount = 0;
        public List<DemandEntry> demand = new List<DemandEntry>();
        public List<LedgerEntry> ledger = new List<LedgerEntry>();

        // Plan 212 (additive v2 — a v1 save deserializes these as neutral).
        public List<CategoryIndexEntry> categoryIndices = new List<CategoryIndexEntry>();
        public List<TradePressureEntry> tradePressure = new List<TradePressureEntry>();
        public List<MarketShockState> activeShocks = new List<MarketShockState>();

        // Plan 14A (additive v3 — nested embargo decay runtime state; a v1/v2
        // save restores neutral. Rules are data; only decay state persists).
        public TradeEmbargoState? tradeEmbargo;

        // Plan 215 (additive policy state). Quantities remain owned by the
        // inventory/domain consumers; the economy envelope carries only the
        // ration policy so old saves restore the neutral/full default.
        public ResourceRationingState? rationing;
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

    /// <summary>Quote side used by the explanation contract.</summary>
    public enum MarketTransactionSide
    {
        Buy = 0,
        Sell = 1,
        Barter = 2
    }

    /// <summary>Stable, typed categories in the authoritative price path.</summary>
    public enum PriceFactorKind
    {
        Demand = 0,
        FloorClamp = 1,
        CeilingClamp = 2,
        /// <summary>Plan 212 — category index multiplier.</summary>
        Category = 3,
        /// <summary>Plan 212 — active market shock multiplier (one record per shock).</summary>
        Shock = 4,
        /// <summary>Plan 14B — persistent regional price-geography multiplier (one record).</summary>
        Regional = 5,
        /// <summary>Plan 14A — temporary embargo shock multiplier incl. decay (one record).</summary>
        Embargo = 6,
        /// <summary>Plan 18C — authored GoodDefinition.regionalSupply provenance factor.</summary>
        RegionalSupply = 7
    }

    /// <summary>
    /// One deterministic price-factor record. Values are numeric on purpose:
    /// hosts translate the typed record into player-facing copy and never
    /// reimplement the pricing formula.
    /// </summary>
    [Serializable]
    public sealed class PriceFactorRecord
    {
        public PriceFactorKind kind;
        public string sourceId = string.Empty;
        public float beforePrice;
        public float afterPrice;
        public float delta;
        public float multiplier = 1f;
        public bool isConstraint;
    }

    /// <summary>
    /// Side-effect-free decomposition of one market quote. The final price is
    /// calculated by the same private path as <see cref="GetPrice"/>.
    /// </summary>
    [Serializable]
    public sealed class PriceExplanation
    {
        public string itemId = string.Empty;
        public MarketTransactionSide side;
        public float basePrice;
        public float demandMultiplier = 1f;
        public float unclampedPrice;
        public float finalPrice;
        public List<PriceFactorRecord> factors = new List<PriceFactorRecord>();

        public float TotalFactorDelta
        {
            get
            {
                float total = 0f;
                for (int i = 0; i < factors.Count; i++) total += factors[i].delta;
                return total;
            }
        }
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

        // Plan 212 — commodity index constants (all deterministic; pricing is
        // formulaic — RNG is used only by the caller's explicit shock events).
        /// <summary>Fraction of net trade pressure retained each day (rest decays).</summary>
        public const float PressureRetainedPerDay = 0.75f;
        /// <summary>Absolute pressure clamp per entry — no unbounded save growth.</summary>
        public const float PressureMaxUnits = 100000f;
        /// <summary>Bounded pressure contribution to the target multiplier (3500bp).</summary>
        public const float PressureComponentCapPermille = 350f;
        /// <summary>Bounded lerp fraction: current += alpha * (target - current).</summary>
        public const float IndexSmoothingAlpha = 0.25f;
        /// <summary>Clamp for a category multiplier when no baseline bounds exist.</summary>
        public const float FallbackIndexFloor = 0.2f;
        public const float FallbackIndexCeiling = 5f;
        /// <summary>Shock product clamp — no single event dominates the price path.</summary>
        public const float ShockProductFloor = 0.4f;
        public const float ShockProductCeiling = 2.5f;
        public const float ShockSeverityMinBp = 500f;
        public const float ShockSeverityMaxBp = 3000f;

        private readonly MarketState _state;
        private GoodsCatalog _catalog;
        private CommodityBaselineCatalog _commodityCatalog;
        // Plan 14A/14B — optional collaborators. Unbound (or null region), the
        // market runs EXACTLY the pre-C1 pricing path: no Regional/Embargo
        // factor rows, byte-identical quotes.
        private RegionalPriceAtlas _regionalAtlas;
        private TradeEmbargoSystem _embargoSystem;
        private string _marketRegion = string.Empty;

        public event Action<string, float> OnDemandAdjusted;     // itemId, delta
        public event Action OnEconomyChanged;                     // any price-relevant change
        public event Action<MarketState> OnStateChanged;
        /// <summary>Plan 212 — raised when a shock becomes active (same tick it is applied or restored-in-flight).</summary>
        public event Action<MarketShockState> OnShockStarted;
        /// <summary>Plan 212 — raised once per shock when it expires during TickDay.</summary>
        public event Action<MarketShockState> OnShockExpired;

        public MarketSystem(MarketState? state = null)
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

        /// <summary>
        /// Loads a goods catalog from the specified data directory and binds it to the market.
        /// </summary>
        public void LoadCatalog(string dataDir, IFileIO? fileIO = null, IJsonSerializer? serializer = null)
        {
            if (string.IsNullOrEmpty(dataDir)) return;
            var io = fileIO ?? new FileSystemIO();
            var json = serializer ?? new SystemTextJsonSerializer();
            var load = GoodsCatalogLoader.Load(dataDir, io, json);
            if (!load.HasErrors)
            {
                BindCatalog(GoodsCatalogLoader.ToCatalog(load));
            }
        }

        public GoodDefinition? FindGood(string itemId) =>
            _catalog != null ? _catalog.Find(itemId) : null;

        /// <summary>
        /// Plan 212 — bind the commodity behavior catalog. Optional: without
        /// it the market behaves exactly as before (no category/shock price
        /// factors), which keeps old save restores and legacy callers on the
        /// untouched v1 pricing path.
        /// </summary>
        public void BindCommodityCatalog(CommodityBaselineCatalog catalog)
        {
            _commodityCatalog = catalog;
        }

        public CommodityBaselineDefinition? FindCommodityBaseline(string categoryId) =>
            _commodityCatalog != null && !string.IsNullOrEmpty(categoryId)
                ? _commodityCatalog.Find(categoryId)
                : null;

        /// <summary>
        /// Plan 14B — bind the regional price atlas and this market's canonical
        /// region. Optional: unbound, regional pricing is neutral and no
        /// Regional factor row is emitted (legacy path). The home market is the
        /// settlement profile — the balanced baseline the source data assigns.
        /// </summary>
        public void BindRegionalPriceAtlas(RegionalPriceAtlas atlas, string marketRegion = "settlement")
        {
            _regionalAtlas = atlas;
            _marketRegion = marketRegion ?? string.Empty;
        }

        /// <summary>
        /// Plan 14A — bind the embargo authority. The market applies its
        /// DECAY-AWARE multiplier as ONE Embargo factor after the regional
        /// factor; it never mutates demand, indices, or shocks itself.
        /// Optional: unbound, no Embargo factor row is emitted (legacy path).
        /// </summary>
        public void BindEmbargoSystem(TradeEmbargoSystem embargoSystem)
        {
            _embargoSystem = embargoSystem;
        }

        // ── Daily tick ────────────────────────────────────────────────

        /// <summary>
        /// Advance one market day: drift each tracked/known good's demand with a
        /// deterministic volatility walk (elasticity-scaled), re-clamp to the
        /// Unity bounds, and record the day. All rolls come from the caller's
        /// ISeededRng (host owns the seed sequence).
        /// Plan 212: before the walk, expired shocks are pruned (typed events),
        /// trade pressure decays, and category indices move a bounded step
        /// toward their formulaic target — no RNG in any of these steps.
        /// </summary>
        public void TickDay(int day, ISeededRng rng)
        {
            if (rng == null) return;
            _state.day = day;
            _state.tickCount++;

            TickDayPlan212State(day);

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

        /// <summary>
        /// Advances the market simulation by the specified number of days using a deterministic seeded RNG.
        /// </summary>
        public void TickDays(int days, ISeededRng? rng = null)
        {
            if (days <= 0) return;
            var seededRng = rng ?? new SeededRng(2026);
            for (int i = 0; i < days; i++)
            {
                TickDay(_state.day + 1, seededRng);
            }
        }

        // ── Plan 212: commodity indices / pressure / shocks ─────────

        /// <summary>
        /// Deterministic daily state update: prune expired shocks, decay
        /// trade pressure, and pull each category index one bounded step
        /// toward its formulaic target. Called exactly once per TickDay.
        /// </summary>
        private void TickDayPlan212State(int day)
        {
            // 1. Prune expired shocks (expiryDay <= day means expired).
            for (int i = _state.activeShocks.Count - 1; i >= 0; i--)
            {
                var shock = _state.activeShocks[i];
                if (shock == null) { _state.activeShocks.RemoveAt(i); continue; }
                if (shock.expiryDay <= day)
                {
                    _state.activeShocks.RemoveAt(i);
                    OnShockExpired?.Invoke(shock);
                }
            }

            // 2. Decay trade pressure toward zero.
            for (int i = 0; i < _state.tradePressure.Count; i++)
            {
                var p = _state.tradePressure[i];
                if (p == null) continue;
                p.buyUnits = Math.Max(0f, p.buyUnits * PressureRetainedPerDay);
                p.sellUnits = Math.Max(0f, p.sellUnits * PressureRetainedPerDay);
            }

            // 3. Update category indices toward their bounded target.
            for (int i = 0; i < _state.categoryIndices.Count; i++)
            {
                var idx = _state.categoryIndices[i];
                if (idx == null || string.IsNullOrEmpty(idx.categoryId)) continue;
                var baseline = FindCommodityBaseline(idx.categoryId);
                float floor = baseline != null ? baseline.scarcity_floor_permille / (float)CommodityBaselineCatalog.PermilleScale : FallbackIndexFloor;
                float ceiling = baseline != null ? baseline.scarcity_ceiling_permille / (float)CommodityBaselineCatalog.PermilleScale : FallbackIndexCeiling;
                float target = ComputeCategoryTarget(idx.categoryId, baseline);
                float current = Math.Clamp(idx.multiplier, floor, ceiling);
                float next = current + IndexSmoothingAlpha * (target - current);
                idx.multiplier = Math.Clamp(next, floor, ceiling);
            }
        }

        /// <summary>Formulaic target multiplier for a category: base × shocks × pressure term.</summary>
        private float ComputeCategoryTarget(string categoryId, CommodityBaselineDefinition? baseline)
        {
            float baseMult = baseline != null
                ? baseline.base_multiplier_permille / (float)CommodityBaselineCatalog.PermilleScale
                : 1f;

            float shockProduct = 1f;
            for (int i = 0; i < _state.activeShocks.Count; i++)
            {
                var shock = _state.activeShocks[i];
                if (shock == null || !string.Equals(shock.categoryId, categoryId, StringComparison.Ordinal)) continue;
                float sev = Math.Clamp(shock.severityBp, ShockSeverityMinBp, ShockSeverityMaxBp) / 10000f;
                shockProduct *= shock.isShortage ? 1f + sev : Math.Max(0f, 1f - sev);
            }
            shockProduct = Math.Clamp(shockProduct, ShockProductFloor, ShockProductCeiling);

            var pressure = FindPressure(categoryId);
            float net = pressure != null ? pressure.buyUnits - pressure.sellUnits : 0f;
            float scale = ElasticityPressureScale(baseline != null ? baseline.elasticity_class : "medium");
            float pressureTerm = Math.Clamp(
                net * scale / (float)CommodityBaselineCatalog.PermilleScale,
                -PressureComponentCapPermille / (float)CommodityBaselineCatalog.PermilleScale,
                PressureComponentCapPermille / (float)CommodityBaselineCatalog.PermilleScale);

            return baseMult * shockProduct * (1f + pressureTerm);
        }

        private static float ElasticityPressureScale(string elasticityClass)
        {
            switch (elasticityClass)
            {
                case "low": return 0.4f;
                case "high": return 1.6f;
                default: return 0.8f;   // medium
            }
        }

        /// <summary>Current category index multiplier (1.0 when untracked or unbound).</summary>
        /// <summary>Current category index multiplier (1.0 when untracked or unbound). Clamped to the authored baseline bounds when a baseline exists.</summary>
        public float GetCategoryMultiplier(string categoryId)
        {
            if (string.IsNullOrEmpty(categoryId)) return 1f;
            for (int i = 0; i < _state.categoryIndices.Count; i++)
            {
                var idx = _state.categoryIndices[i];
                if (idx != null && idx.categoryId == categoryId)
                {
                    var baseline = FindCommodityBaseline(categoryId);
                    float floor = baseline != null ? baseline.scarcity_floor_permille / (float)CommodityBaselineCatalog.PermilleScale : FallbackIndexFloor;
                    float ceiling = baseline != null ? baseline.scarcity_ceiling_permille / (float)CommodityBaselineCatalog.PermilleScale : FallbackIndexCeiling;
                    return Math.Clamp(idx.multiplier, floor, ceiling);
                }
            }
            return 1f;
        }

        /// <summary>Current effective category multiplier for an item (1.0 when the good is unknown).</summary>
        public float GetEffectiveCategoryMultiplierForItem(string itemId)
        {
            var good = FindGood(itemId);
            return good == null ? 1f : GetCategoryMultiplier(good.category);
        }

        /// <summary>
        /// Apply (or refresh) a market shock for a category. Idempotent per
        /// (category, kind, sourceId): an active shock from the same source is
        /// refreshed in place rather than stacked. Returns the shock state.
        /// Severity permille clamps to [500, 3000]; duration days clamps to [1, 60].
        /// </summary>
        public MarketShockState? ApplyShock(
            string categoryId, bool isShortage, float severityBp,
            int startDay, int durationDays, string sourceId)
        {
            if (string.IsNullOrEmpty(categoryId)) return null;
            if (FindCommodityBaseline(categoryId) == null) return null;
            if (durationDays < 1) durationDays = 1;
            if (durationDays > 60) durationDays = 60;
            float severity = Math.Clamp(severityBp, ShockSeverityMinBp, ShockSeverityMaxBp);

            for (int i = 0; i < _state.activeShocks.Count; i++)
            {
                var existing = _state.activeShocks[i];
                if (existing == null) continue;
                if (existing.categoryId == categoryId
                    && existing.isShortage == isShortage
                    && string.Equals(existing.sourceId, sourceId ?? string.Empty, StringComparison.Ordinal))
                {
                    existing.severityBp = severity;
                    existing.startDay = startDay;
                    existing.expiryDay = startDay + durationDays;
                    return existing;
                }
            }

            var shock = new MarketShockState
            {
                shockId = $"shock_{categoryId}_{(isShortage ? "shortage" : "crash")}_{startDay}_{_state.activeShocks.Count + 1}",
                categoryId = categoryId,
                isShortage = isShortage,
                severityBp = severity,
                startDay = startDay,
                expiryDay = startDay + durationDays,
                sourceId = sourceId ?? string.Empty
            };
            _state.activeShocks.Add(shock);
            OnShockStarted?.Invoke(shock);
            OnEconomyChanged?.Invoke();
            RaiseChanged();
            return shock;
        }

        public IReadOnlyList<MarketShockState> ActiveShocks => _state.activeShocks;

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
            return ExplainPrice(itemId).finalPrice;
        }

        /// <summary>
        /// Plan 14B — region-aware quote. A null/empty region falls back to the
        /// bound market region (home market); with no atlas/embargo bound the
        /// result is identical to the legacy quote.
        /// </summary>
        public float GetPrice(string itemId, string? region)
        {
            var good = FindGood(itemId);
            if (good == null) return float.NaN;
            return ExplainPrice(itemId, MarketTransactionSide.Buy, region).finalPrice;
        }

        /// <summary>
        /// Explain the current quote without changing demand, ledger, or any
        /// other market state. This method deliberately reuses the exact
        /// base/demand/clamp path used by <see cref="GetPrice"/> so the final
        /// explained value cannot drift from the quote shown to the player.
        /// Plan 14A/14B — canonical factor order: base × demand → category
        /// index → shocks → REGIONAL baseline → EMBARGO (decay-aware) →
        /// existing floor/ceiling clamps. Each factor applies at most once.
        /// </summary>
        public PriceExplanation ExplainPrice(
            string itemId,
            MarketTransactionSide side = MarketTransactionSide.Buy,
            string? region = null)
        {
            var good = FindGood(itemId);
            var explanation = new PriceExplanation
            {
                itemId = itemId ?? string.Empty,
                side = side
            };
            if (good == null)
            {
                explanation.finalPrice = float.NaN;
                explanation.unclampedPrice = float.NaN;
                return explanation;
            }

            explanation.basePrice = good.basePrice;
            explanation.demandMultiplier = GetDemandMultiplier(itemId);
            float price = good.basePrice;
            float demanded = price * explanation.demandMultiplier;
            explanation.unclampedPrice = demanded;
            explanation.factors.Add(new PriceFactorRecord
            {
                kind = PriceFactorKind.Demand,
                sourceId = itemId ?? string.Empty,
                beforePrice = price,
                afterPrice = demanded,
                delta = demanded - price,
                multiplier = explanation.demandMultiplier,
                isConstraint = false
            });
            price = demanded;

            // ── Plan 212: category index + shock factors (additive). With no
            // commodity catalog bound or no baseline for the category, the
            // multiplier is exactly 1.0 and NO factor rows are emitted — the
            // legacy v1 explanation path is byte-for-byte unchanged.
            // (good is non-null here — the early return above guarantees it.)
            {
                float categoryMult = GetEffectiveCategoryMultiplierForItem(good.id);
                if (Math.Abs(categoryMult - 1f) > 1e-6f)
                {
                    float afterCategory = price * categoryMult;
                    explanation.factors.Add(new PriceFactorRecord
                    {
                        kind = PriceFactorKind.Category,
                        sourceId = "category:" + good.category,
                        beforePrice = price,
                        afterPrice = afterCategory,
                        delta = afterCategory - price,
                        multiplier = categoryMult,
                        isConstraint = false
                    });
                    price = afterCategory;
                }

                for (int i = 0; i < _state.activeShocks.Count; i++)
                {
                    var shock = _state.activeShocks[i];
                    if (shock == null || shock.categoryId != good.category) continue;
                    float sev = Math.Clamp(shock.severityBp, ShockSeverityMinBp, ShockSeverityMaxBp) / 10000f;
                    float shockMult = shock.isShortage ? 1f + sev : Math.Max(0f, 1f - sev);
                    float afterShock = price * shockMult;
                    explanation.factors.Add(new PriceFactorRecord
                    {
                        kind = PriceFactorKind.Shock,
                        sourceId = "shock:" + shock.shockId,
                        beforePrice = price,
                        afterPrice = afterShock,
                        delta = afterShock - price,
                        multiplier = shockMult,
                        isConstraint = false
                    });
                    price = afterShock;
                }
            }

            // ── Plan 14A/14B: regional baseline, then embargo shock. Additive
            // and ordered: each applies at most once, before the existing
            // clamps. No collaborator bound (or no region) → no rows — the
            // pre-C1 quote path is byte-for-byte unchanged.
            string effectiveRegion = !string.IsNullOrEmpty(region) ? region : _marketRegion;
            if (!string.IsNullOrEmpty(effectiveRegion))
            {
                bool atlasProvidedRegionalModifier = false;
                if (_regionalAtlas != null)
                {
                    int regionalPermille = _regionalAtlas.GetModifierPermille(good.id, good.category, effectiveRegion);
                    if (regionalPermille != 1000)
                    {
                        atlasProvidedRegionalModifier = true;
                        float regionalMult = regionalPermille / 1000f;
                        float afterRegional = price * regionalMult;
                        explanation.factors.Add(new PriceFactorRecord
                        {
                            kind = PriceFactorKind.Regional,
                            sourceId = "regional:" + effectiveRegion,
                            beforePrice = price,
                            afterPrice = afterRegional,
                            delta = afterRegional - price,
                            multiplier = regionalMult,
                            isConstraint = false
                        });
                        price = afterRegional;
                    }
                }

                // A catalog-level regional price override is authoritative
                // when present. Otherwise the authored regionalSupply field
                // still changes the quote through the live supply matcher.
                // This keeps the C1 atlas precedence deterministic while
                // preventing regionalSupply from remaining a parsed-only knob.
                if (_regionalAtlas != null &&
                    !atlasProvidedRegionalModifier &&
                    !string.IsNullOrEmpty(good.regionalSupply))
                {
                    float supplyMult = RegionalSupplyRouter.ShortageDemandScale(
                        good.regionalSupply,
                        effectiveRegion);
                    if (Math.Abs(supplyMult - 1f) > 1e-6f)
                    {
                        float afterSupply = price * supplyMult;
                        explanation.factors.Add(new PriceFactorRecord
                        {
                            kind = PriceFactorKind.RegionalSupply,
                            sourceId = "regional_supply:" + good.regionalSupply,
                            beforePrice = price,
                            afterPrice = afterSupply,
                            delta = afterSupply - price,
                            multiplier = supplyMult,
                            isConstraint = false
                        });
                        price = afterSupply;
                    }
                }

                if (_embargoSystem != null)
                {
                    int embargoPermille = _embargoSystem.GetCurrentPriceMultiplierPermille(effectiveRegion, good.id, good.category);
                    if (embargoPermille != 1000)
                    {
                        float embargoMult = embargoPermille / 1000f;
                        float afterEmbargo = price * embargoMult;
                        explanation.factors.Add(new PriceFactorRecord
                        {
                            kind = PriceFactorKind.Embargo,
                            sourceId = "embargo",
                            beforePrice = price,
                            afterPrice = afterEmbargo,
                            delta = afterEmbargo - price,
                            multiplier = embargoMult,
                            isConstraint = false
                        });
                        price = afterEmbargo;
                    }
                }
            }

            float floor = good.basePrice * PriceFloorFraction;
            if (price < floor)
            {
                explanation.factors.Add(new PriceFactorRecord
                {
                    kind = PriceFactorKind.FloorClamp,
                    sourceId = "market_price_floor",
                    beforePrice = price,
                    afterPrice = floor,
                    delta = floor - price,
                    multiplier = PriceFloorFraction,
                    isConstraint = true
                });
                price = floor;
            }

            float ceiling = good.basePrice * PriceCeilingFraction;
            if (price > ceiling)
            {
                explanation.factors.Add(new PriceFactorRecord
                {
                    kind = PriceFactorKind.CeilingClamp,
                    sourceId = "market_price_ceiling",
                    beforePrice = price,
                    afterPrice = ceiling,
                    delta = ceiling - price,
                    multiplier = PriceCeilingFraction,
                    isConstraint = true
                });
                price = ceiling;
            }

            explanation.finalPrice = price;
            return explanation;
        }

        // ── Transactions / barter ──────────────────────────────────────

        /// <summary>
        /// Book a purchase/sale at the current market price. Barter invariant:
        /// the ledger records value in both directions at the same unit price,
        /// so a barter of good A for good B exchanges equal total value.
        /// </summary>
        public TransactionResult Buy(string itemId, int quantity, int day, string counterparty = "market", string? region = null)
        {
            return Transact(itemId, quantity, day, counterparty, isSale: false, region: region);
        }

        public TransactionResult Sell(string itemId, int quantity, int day, string counterparty = "market", string? region = null)
        {
            return Transact(itemId, quantity, day, counterparty, isSale: true, region: region);
        }

        private TransactionResult Transact(string itemId, int quantity, int day, string counterparty, bool isSale, string? region = null)
        {
            var good = FindGood(itemId);
            if (good == null)
                return Rejected(itemId, "unknown good");
            if (quantity <= 0)
                return Rejected(itemId, "quantity must be > 0");

            // Region-aware unit price: null region falls back to the bound
            // market region; unbound collaborators yield the legacy quote.
            float unitPrice = ExplainPrice(itemId, MarketTransactionSide.Buy, region).finalPrice;
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
            RecordTradePressure(good.category, isSale ? quantity : 0, isSale ? 0 : quantity);
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
        public TransactionResult Barter(string giveItemId, int giveQuantity, string takeItemId, int day, string? region = null)
        {
            var giveGood = FindGood(giveItemId);
            var takeGood = FindGood(takeItemId);
            if (giveGood == null) return Rejected(giveItemId, "unknown give good");
            if (takeGood == null) return Rejected(takeItemId, "unknown take good");
            if (giveQuantity <= 0) return Rejected(giveItemId, "quantity must be > 0");

            float givePrice = ExplainPrice(giveItemId, MarketTransactionSide.Buy, region).finalPrice;
            float giveValue = givePrice * giveQuantity;
            if (giveValue <= 0f) return Rejected(giveItemId, "zero value");

            float takePrice = ExplainPrice(takeItemId, MarketTransactionSide.Buy, region).finalPrice;
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
            // Barter legs move market stock too: give = supply (sell pressure),
            // take = demand (buy pressure).
            if (giveGood != null) RecordTradePressure(giveGood.category, giveQuantity, 0);
            if (takeGood != null) RecordTradePressure(takeGood.category, 0, takeQuantity);
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

            // Plan 212 — additive v2 fields: sorted, clamped, deduped.
            var indices = new List<CategoryIndexEntry>(_state.categoryIndices);
            indices.Sort((a, b) => string.CompareOrdinal(a.categoryId, b.categoryId));
            var seenIndices = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < indices.Count; i++)
            {
                var idx = indices[i];
                if (idx == null || string.IsNullOrEmpty(idx.categoryId) || !seenIndices.Add(idx.categoryId)) continue;
                copy.categoryIndices.Add(new CategoryIndexEntry
                {
                    categoryId = idx.categoryId,
                    multiplier = Math.Clamp(idx.multiplier, FallbackIndexFloor, FallbackIndexCeiling)
                });
            }

            var pressures = new List<TradePressureEntry>(_state.tradePressure);
            pressures.Sort((a, b) => string.CompareOrdinal(a.categoryId, b.categoryId));
            var seenPressure = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < pressures.Count; i++)
            {
                var p = pressures[i];
                if (p == null || string.IsNullOrEmpty(p.categoryId) || !seenPressure.Add(p.categoryId)) continue;
                copy.tradePressure.Add(new TradePressureEntry
                {
                    categoryId = p.categoryId,
                    buyUnits = Math.Clamp(p.buyUnits, 0f, PressureMaxUnits),
                    sellUnits = Math.Clamp(p.sellUnits, 0f, PressureMaxUnits)
                });
            }

            var shocks = new List<MarketShockState>(_state.activeShocks);
            shocks.Sort((a, b) =>
            {
                int byDay = a.startDay.CompareTo(b.startDay);
                return byDay != 0 ? byDay : string.CompareOrdinal(a.shockId, b.shockId);
            });
            var seenShocks = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < shocks.Count; i++)
            {
                var s = shocks[i];
                if (s == null || string.IsNullOrEmpty(s.shockId) || string.IsNullOrEmpty(s.categoryId) || !seenShocks.Add(s.shockId)) continue;
                copy.activeShocks.Add(new MarketShockState
                {
                    shockId = s.shockId,
                    categoryId = s.categoryId,
                    isShortage = s.isShortage,
                    severityBp = Math.Clamp(s.severityBp, ShockSeverityMinBp, ShockSeverityMaxBp),
                    startDay = Math.Max(0, s.startDay),
                    expiryDay = Math.Max(s.startDay + 1, s.expiryDay),
                    sourceId = s.sourceId ?? string.Empty
                });
            }
            // Plan 14A — nested embargo decay state (v3). Null when the embargo
            // system is unbound (legacy demo paths never invent state).
            copy.tradeEmbargo = _embargoSystem != null ? _embargoSystem.CaptureState() : null;
            return copy;
        }

        /// <summary>
        /// Restore state. Newer versions fail loudly (throws); equal or older
        /// versions migrate predictably (missing demand rows read as 1.0;
        /// missing Plan 212 fields read as neutral — no indices, no pressure,
        /// no shocks).
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

            // Plan 212 — v2 additive restore. A v1 save (or a save missing the
            // fields) restores as neutral: no indices, no pressure, no shocks.
            _state.categoryIndices.Clear();
            if (saved.categoryIndices != null)
            {
                var seenIndices = new HashSet<string>(StringComparer.Ordinal);
                for (int i = 0; i < saved.categoryIndices.Count; i++)
                {
                    var idx = saved.categoryIndices[i];
                    if (idx == null || string.IsNullOrEmpty(idx.categoryId)) continue;
                    if (!seenIndices.Add(idx.categoryId)) continue; // first-wins dedupe
                    _state.categoryIndices.Add(new CategoryIndexEntry
                    {
                        categoryId = idx.categoryId,
                        multiplier = Math.Clamp(idx.multiplier, FallbackIndexFloor, FallbackIndexCeiling)
                    });
                }
            }

            _state.tradePressure.Clear();
            if (saved.tradePressure != null)
            {
                var seenPressure = new HashSet<string>(StringComparer.Ordinal);
                for (int i = 0; i < saved.tradePressure.Count; i++)
                {
                    var p = saved.tradePressure[i];
                    if (p == null || string.IsNullOrEmpty(p.categoryId)) continue;
                    if (!seenPressure.Add(p.categoryId)) continue;
                    _state.tradePressure.Add(new TradePressureEntry
                    {
                        categoryId = p.categoryId,
                        buyUnits = Math.Clamp(p.buyUnits, 0f, PressureMaxUnits),
                        sellUnits = Math.Clamp(p.sellUnits, 0f, PressureMaxUnits)
                    });
                }
            }

            _state.activeShocks.Clear();
            if (saved.activeShocks != null)
            {
                var seenShocks = new HashSet<string>(StringComparer.Ordinal);
                for (int i = 0; i < saved.activeShocks.Count; i++)
                {
                    var s = saved.activeShocks[i];
                    if (s == null || string.IsNullOrEmpty(s.shockId) || string.IsNullOrEmpty(s.categoryId)) continue;
                    if (!seenShocks.Add(s.shockId)) continue;
                    _state.activeShocks.Add(new MarketShockState
                    {
                        shockId = s.shockId,
                        categoryId = s.categoryId,
                        isShortage = s.isShortage,
                        severityBp = Math.Clamp(s.severityBp, ShockSeverityMinBp, ShockSeverityMaxBp),
                        startDay = Math.Max(0, s.startDay),
                        expiryDay = Math.Max(Math.Max(0, s.startDay) + 1, s.expiryDay),
                        sourceId = s.sourceId ?? string.Empty
                    });
                }
            }
            // Plan 14A — v3 additive restore. A v1/v2 save (or a null field)
            // restores the bound embargo system to NEUTRAL: no in-flight decay.
            // A future save (version > supported) already threw above.
            if (_embargoSystem != null)
                _embargoSystem.RestoreState(saved.tradeEmbargo ?? new TradeEmbargoState());
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

        // ── Plan 212: trade pressure ─────────────────────────────────

        /// <summary>
        /// Record market flow pressure for a category. Buying removes stock
        /// (buy pressure raises the index); selling adds stock (sell pressure
        /// lowers it). Deterministic; clamped per entry.
        /// </summary>
        private void RecordTradePressure(string categoryId, int soldUnits, int boughtUnits)
        {
            if (string.IsNullOrEmpty(categoryId) || (soldUnits <= 0 && boughtUnits <= 0)) return;
            var p = FindPressure(categoryId);
            if (p == null)
            {
                p = new TradePressureEntry { categoryId = categoryId };
                _state.tradePressure.Add(p);
            }
            p.buyUnits = Math.Min(PressureMaxUnits, p.buyUnits + Math.Max(0, boughtUnits));
            p.sellUnits = Math.Min(PressureMaxUnits, p.sellUnits + Math.Max(0, soldUnits));
        }

        private TradePressureEntry? FindPressure(string categoryId)
        {
            for (int i = 0; i < _state.tradePressure.Count; i++)
            {
                var p = _state.tradePressure[i];
                if (p != null && p.categoryId == categoryId) return p;
            }
            return null;
        }

        public IReadOnlyList<TradePressureEntry> TradePressure => _state.tradePressure;

        private void RaiseChanged() => OnStateChanged?.Invoke(_state);
    }
}
