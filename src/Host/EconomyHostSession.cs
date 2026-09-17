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

        public EconomyHostSession(MarketSystem market = null!)
        {
            Market = market ?? new MarketSystem();
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
                session.LastEvent = "Economy state restored from save.";
            }
            return session;
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
            if (Catalog != null)
            {
                foreach (var good in Catalog.All())
                    sb.Append($"  {good.id}: {Market.GetPrice(good.id):0.00} (demand {Market.GetDemandMultiplier(good.id):0.00})\n");
            }
            return sb.ToString().TrimEnd();
        }

        // ── Save / Load ──────────────────────────────────────────────

        public MarketState CaptureSave() => Market.CaptureState();
        public void RestoreSave(MarketState state) => Market.RestoreState(state);
    }
}
