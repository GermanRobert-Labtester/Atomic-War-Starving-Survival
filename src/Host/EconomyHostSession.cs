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

        public string LastEvent { get; private set; } = string.Empty;

        public event Action StateChanged;

        public EconomyHostSession(MarketSystem market = null!)
        {
            Market = market ?? new MarketSystem();
            Market.OnDemandAdjusted += (itemId, delta) =>
            {
                LastEvent = $"Demand {itemId} {delta:+0.00;-0.00}";
                StateChanged?.Invoke();
            };
            Market.OnEconomyChanged += () => StateChanged?.Invoke();
            Market.OnStateChanged += _ => StateChanged?.Invoke();
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
            }
            var save = EconomySaveStore.TryLoad();
            if (save != null)
            {
                session.Market.RestoreState(save);
                session.LastEvent = "Economy state restored from save.";
            }
            return session;
        }

        // ── Demo actions ─────────────────────────────────────────────

        public string TickDemo(int days)
        {
            for (int i = 0; i < days; i++)
                Market.TickDay(Market.Day + 1, new SeededRng(DemoSeed));
            return $"Ticked {days} day(s): day {Market.Day}.";
        }

        public string BuyDemo(string itemId, int quantity)
        {
            var result = Market.Buy(itemId, quantity, Market.Day);
            return result.Accepted
                ? $"Bought {quantity}x {itemId} at {result.UnitPrice:0.00} each ({result.TotalValue:0.00} total)."
                : $"Buy rejected: {result.RejectReason}.";
        }

        public string BarterDemo(string giveItemId, int giveQuantity, string takeItemId)
        {
            var result = Market.Barter(giveItemId, giveQuantity, takeItemId, Market.Day);
            return result.Accepted
                ? $"Bartered {giveQuantity}x {giveItemId} for {result.Quantity}x {takeItemId}."
                : $"Barter rejected: {result.RejectReason}.";
        }

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
