using System.Collections.Generic;

namespace Ashfall.Core.Economy
{
    /// <summary>
    /// Headless verification of the economy core: loads representative goods
    /// JSON, runs deterministic market ticks with a fixed seed, exercises
    /// transactions and barter, then saves, reloads and continues — the
    /// pre-save and post-load trajectories must match exactly. Invoked by
    /// `dotnet test` and by Godot `-- --economy-selftest`.
    /// </summary>
    public static class EconomyHeadlessDemo
    {
        public static HeadlessReport Run(string dataDirectory, ILog? log = null)
        {
            CatalogLocator.UseInvariantCulture();
            log = log ?? NullLog.Instance;
            var report = new HeadlessReport();

            void Check(bool condition, string name)
            {
                report.Checks.Add(new HeadlessCheck { Name = name, Passed = condition });
                if (condition)
                {
                    report.PassedCount++;
                    log.Info("[PASS] " + name);
                }
                else
                {
                    report.FailedCount++;
                    log.Error("[FAIL] " + name);
                }
            }

            log.Info("[EconomyHeadlessDemo] begin");

            // 1. Load representative goods JSON (the authority).
            var load = GoodsCatalogLoader.Load(
                dataDirectory, new FileSystemIO(), new SystemTextJsonSerializer());
            Check(load.HasErrors == false, "goods JSON loads without validation errors");
            Check(load.Goods.Count >= 8, $"representative goods loaded ({load.Goods.Count})");
            var catalog = GoodsCatalogLoader.ToCatalog(load);
            Check(catalog.Find("clean_water") != null && catalog.Find("scrap_metal") != null,
                "known goods resolve");

            // 2. Initialize with a fixed seed and run deterministic ticks.
            var market = new MarketSystem();
            market.BindCatalog(catalog);
            for (int day = 1; day <= 10; day++)
                market.TickDay(day, new SeededRng(4242));
            Check(market.Day == 10, "ten market days elapsed");
            Check(market.GetPrice("clean_water") > 0f, "prices resolve after ticks");

            // 3. Transactions and barter.
            var buy = market.Buy("clean_water", 4, 10, "test_faction");
            Check(buy.Accepted && buy.TotalValue > 0f, "purchase books at market price");
            var barter = market.Barter("scrap_metal", 20, "clean_water", 10);
            Check(barter.Accepted, "barter exchanges goods");
            Check(market.State.ledger.Count >= 3, "ledger records all transactions");

            // 4. Save, reload, continue — trajectories must match exactly.
            var snapshot = market.CaptureState();
            var restored = new MarketSystem();
            restored.RestoreState(snapshot);
            restored.BindCatalog(catalog);
            Check(restored.Day == market.Day && restored.TickCount == market.TickCount,
                "state round-trips");
            bool match = true;
            for (int day = 11; day <= 20; day++)
            {
                market.TickDay(day, new SeededRng(4242));
                restored.TickDay(day, new SeededRng(4242));
                foreach (var good in catalog.All())
                {
                    if (market.GetPrice(good.id) != restored.GetPrice(good.id))
                        match = false;
                }
            }
            Check(match, "pre-save and post-load trajectories match exactly");

            // 5. Corrupt state fails loudly.
            bool threw = false;
            try
            {
                restored.RestoreState(new MarketState { version = MarketState.Version + 1 });
            }
            catch (System.InvalidOperationException)
            {
                threw = true;
            }
            Check(threw, "newer save version fails loudly");

            report.Passed = report.FailedCount == 0;
            report.Summary =
                $"[EconomyHeadlessDemo] {(report.Passed ? "PASS" : "FAIL")} " +
                $"{report.PassedCount}/{report.PassedCount + report.FailedCount}";
            log.Info(report.Summary);
            return report;
        }
    }
}
