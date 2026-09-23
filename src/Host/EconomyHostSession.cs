// SPDX-License-Identifier: MIT
using System;
#pragma warning disable CS8618
using Ashfall.Core;
using Ashfall.Core.Economy;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Thin Godot-host session for the economy port. Loads the goods catalog,
    /// drives deterministic market days, applies transactions, persists state.
    /// No rules here — hosts only wire and present.
    /// </summary>
    public sealed class EconomyHostSession
    : HostSessionBase
    {
        public const int DemoSeed = 2026;

        public MarketSystem Market { get; }
        /// <summary>
        /// Plan 215 policy projection. Inventory, water, medical, and power
        /// systems still own quantities and effects; this session only owns
        /// the persisted ration policy and routes bounded authorization.
        /// </summary>
        public ResourceRationingSystem Rationing { get; }
        public GoodsCatalog Catalog { get; private set; }
        /// <summary>Plan 212 — bound commodity behavior catalog (null when missing/invalid; market then runs the legacy v1 path).</summary>
        public CommodityBaselineCatalog? CommodityCatalog { get; private set; }
        /// <summary>
        /// Plan 14A — the campaign's ONE embargo authority (rules from
        /// trade_embargoes.json). Null only when the data file is missing or
        /// invalid; the market then runs without embargo factors. Caravans and
        /// the weather bridge share this single instance.
        /// </summary>
        public TradeEmbargoSystem? EmbargoSystem { get; private set; }
        /// <summary>Plan 14B — the regional price atlas (null when missing/invalid; regional pricing then neutral).</summary>
        public RegionalPriceAtlas? RegionalAtlas { get; private set; }

        public string LastEvent { get; private set; } = string.Empty;

        /// <summary>Forwarder for the rationing owner's tier-change event.</summary>
        public Action<RationTarget>? RationTierChangedSeam { get; set; }

        public EconomyHostSession(MarketSystem market = null!, ResourceRationingSystem? rationing = null)
        {
            Market = market ?? new MarketSystem();
            Rationing = rationing ?? new ResourceRationingSystem();
            Market.OnDemandAdjusted += (itemId, delta) =>
            {
                LastEvent = $"Demand {itemId} {delta:+0.00;-0.00}";
                RaiseStateChanged();
            };
            Market.OnEconomyChanged += () => RaiseStateChanged();
            Market.OnStateChanged += _ => RaiseStateChanged();
            // Plan 212 — shocks surface in the session event line like other
            // economy facts (host adapts; Core owns state).
            Market.OnShockStarted += shock =>
            {
                LastEvent = $"{(shock.isShortage ? "Shortage" : "Crash")} {shock.categoryId} (x{shock.severityBp / 10000f:0.00}, until day {shock.expiryDay})";
                RaiseStateChanged();
            };
            Market.OnShockExpired += shock =>
            {
                LastEvent = $"Shock ended {shock.categoryId}";
                RaiseStateChanged();
            };
            Rationing.OnRationingTierChanged += target =>
            {
                LastEvent = $"Rationing {target.ResourceId}: {target.Tier}.";
                RaiseStateChanged();
                // Plan 42 / Plan 46 — forward the rationing owner's canonical
                // tier change to host listeners (voice trigger + session
                // telemetry) instead of duplicating its ration model here.
                RationTierChangedSeam?.Invoke(target);
            };
            Rationing.OnCrisisDeclared += crisis =>
            {
                LastEvent = $"Resource crisis declared: {crisis.Type}.";
                RaiseStateChanged();
            };
            Rationing.OnCrisisResolved += crisis =>
            {
                LastEvent = $"Resource crisis resolved: {crisis.Type}.";
                RaiseStateChanged();
            };
        }

        public static EconomyHostSession Create(string dataDir)
        {
            var session = new EconomyHostSession();
            if (!string.IsNullOrEmpty(dataDir))
            {
                var fileIO = CatalogPath.CreateFileIOForDataDir(dataDir);
                var serializer = new SystemTextJsonSerializer();
                var load = GoodsCatalogLoader.Load(dataDir, fileIO, serializer);
                if (!load.HasErrors)
                {
                    session.Catalog = GoodsCatalogLoader.ToCatalog(load);
                    session.Market.BindCatalog(session.Catalog);
                }
                else
                {
                    session.LastEvent = "Goods catalog failed validation: " + load.Errors[0];
                }

                // Plan 212 — commodity behavior catalog. Optional: a missing or
                // invalid file leaves the market on the legacy v1 path (no
                // category/shock factors), never a hard failure.
                var commodityLoad = CommodityBaselineCatalogLoader.Load(dataDir, fileIO, serializer);
                if (!commodityLoad.HasErrors && commodityLoad.Categories.Count > 0)
                {
                    session.CommodityCatalog = CommodityBaselineCatalogLoader.ToCatalog(commodityLoad);
                    session.Market.BindCommodityCatalog(session.CommodityCatalog);
                }
                else if (commodityLoad.HasErrors)
                {
                    session.LastEvent = "Commodity catalog using legacy path: " + commodityLoad.Errors[0];
                }

                // Plan 14A/14B — embargo rules + regional price geography.
                // Optional collaborators: a missing or invalid file leaves the
                // market on the pre-C1 path (neutral regional, no embargo),
                // never a hard failure. The market region is the settlement
                // profile — the balanced home-market baseline.
                var embargoLoad = TradeEmbargoCatalogLoader.Load(dataDir, fileIO, serializer);
                if (!embargoLoad.HasErrors && embargoLoad.Rules.Count > 0)
                {
                    session.EmbargoSystem = new TradeEmbargoSystem(TradeEmbargoCatalogLoader.ToCatalog(embargoLoad));
                    session.Market.BindEmbargoSystem(session.EmbargoSystem);
                }
                else if (embargoLoad.HasErrors)
                {
                    session.LastEvent = "Embargo rules using neutral path: " + embargoLoad.Errors[0];
                }

                var atlasLoad = RegionalPriceCatalogLoader.Load(dataDir, fileIO, serializer);
                if (!atlasLoad.HasErrors && atlasLoad.Entries.Count > 0)
                {
                    session.RegionalAtlas = new RegionalPriceAtlas(RegionalPriceCatalogLoader.ToCatalog(atlasLoad));
                    session.Market.BindRegionalPriceAtlas(session.RegionalAtlas, marketRegion: "settlement");
                }
                else if (atlasLoad.HasErrors)
                {
                    session.LastEvent = "Regional prices using neutral path: " + atlasLoad.Errors[0];
                }
            }
            var save = EconomySaveStore.TryLoad();
            if (save != null)
            {
                session.Market.RestoreState(save);
                session.Rationing.RestoreState(save.rationing ?? new ResourceRationingState());
                session.LastEvent = "Economy state restored from save.";
            }
            return session;
        }

        public void LoadData(string dataDir)
        {
            Market.LoadCatalog(dataDir);
        }

        /// <summary>Bind the canonical item/goods validator before player policy commands.</summary>
        public void BindRationingResourceValidator(Func<string, bool>? validator)
            => Rationing.BindResourceValidator(validator);

        public RationTarget SetRationTier(string resourceId, RationingTier tier, int currentDay)
            => Rationing.SetRationTier(resourceId, tier, currentDay);

        public ResourceAllocationDecision AuthorizeAllocation(
            string resourceId,
            string consumerId,
            int demandUnits,
            int availableUnits,
            int currentDay)
            => Rationing.AuthorizeAllocation(new ResourceAllocationRequest
            {
                ResourceId = resourceId,
                ConsumerId = consumerId,
                DemandUnits = demandUnits,
                AvailableUnits = availableUnits,
                CurrentDay = currentDay
            });

        public string TickDemo(int days)
        {
            Market.TickDays(days);
            LastEvent = $"Advanced {days} days to Day {Market.Day}.";
            RaiseStateChanged();
            return LastEvent;
        }

        public string BarterDemo(string giveItemId, int giveQuantity, string takeItemId)
        {
            var result = Market.Barter(giveItemId, giveQuantity, takeItemId, Market.Day);
            LastEvent = result.Accepted
                ? $"Bartered {giveQuantity}x {giveItemId} for {result.Quantity}x {takeItemId}."
                : $"Barter rejected: {result.RejectReason}.";
            RaiseStateChanged();
            return LastEvent;
        }

        // ── Production actions ───────────────────────────────────────

        /// <summary>
        /// Advance the market exactly one day. Called by the economy_market
        /// day owner through the campaign coordinator; the RNG comes from the
        /// coordinator's deterministic economy stream.
        /// </summary>
        public void TickDay(int day, ISeededRng rng)
        {
            Market.TickDay(day, rng);
        }

        /// <summary>
        /// Return the Core-owned decomposition used by the quote display. The
        /// host only adapts typed factor records into player-facing copy.
        /// </summary>
        public PriceExplanation ExplainPrice(
            string itemId,
            MarketTransactionSide side = MarketTransactionSide.Buy)
            => Market.ExplainPrice(itemId, side);

        /// <summary>Plan 14B (B1) — region-aware decomposition for the regional
        /// heat map: identical typed factors, evaluated at the atlas's regional
        /// geography. Read-only pass-through; no UI-side math.</summary>
        public PriceExplanation ExplainPrice(
            string itemId,
            MarketTransactionSide side,
            string? region)
            => Market.ExplainPrice(itemId, side, region);

        public string StatusLine()
        {
            var sb = new System.Text.StringBuilder();
            sb.Append($"Economy: day {Market.Day} · {Market.State.ledger.Count} ledger lines · " +
                      $"supplies {(Market.IsSuppliesShort() ? "SHORT" : "normal")}\n");
            sb.Append($"Rationing: {Rationing.TargetCount} policy target(s), {Rationing.ActiveCrisesCount} active crisis(es)\n");
            if (Catalog != null)
            {
                foreach (var good in Catalog.All())
                    sb.Append($"  {good.id}: {Market.GetPrice(good.id):0.00} (demand {Market.GetDemandMultiplier(good.id):0.00})\n");
            }
            return sb.ToString().TrimEnd();
        }

        // ── Save / Load ──────────────────────────────────────────────

        public MarketState CaptureSave()
        {
            var state = Market.CaptureState();
            state.rationing = Rationing.CaptureState();
            return state;
        }

        public void RestoreSave(MarketState state)
        {
            Market.RestoreState(state);
            Rationing.RestoreState(state?.rationing ?? new ResourceRationingState());
        }
    }
}
