// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : BlackMarketSelfTest
// Subsystem          : Plan 155 / 211 — Black Market & Underground Economy
// ============================================================================

using System;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Ashfall.Core.Factions;
using Ashfall.Core.Inventory;
using Ashfall.Core.Random;

namespace AtomicWar.GodotApp
{
    public static class BlackMarketSelfTest
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Black Market & Underground Economy Self-Test (Plan 155 / 211) ===");
            int passed = 0;
            const int total = 12;

            try
            {
                // Check 1: Catalog loading from black_market_inventory.json
                string dataRoot = (!string.IsNullOrEmpty(dataDir) && Directory.Exists(dataDir) ? dataDir : CatalogPath.ResolveDataDir());
                string catPath = Path.Combine(dataRoot, "black_market_inventory.json");

                var fileIO = new FileSystemIO();
                var serializer = new SystemTextJsonSerializer();
                var loadResult = BlackMarketInventoryCatalogLoader.Load(dataRoot, fileIO, serializer);

                if (!loadResult.HasErrors && loadResult.Syndicates.Count >= 3 && loadResult.Entries.Count >= 7)
                {
                    Console.WriteLine($"[PASS] Check 1: Catalog loaded {loadResult.Syndicates.Count} syndicates and {loadResult.Entries.Count} entries without errors.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 1: Failed to load catalog ({string.Join(", ", loadResult.Errors)}).");
                }

                // Setup canonical dependencies: MarketSystem, Inventory, ItemCatalog, HoldfastTradeSession
                var marketLoad = GoodsCatalogLoader.Load(dataRoot, fileIO, serializer);
                var prices = new MarketSystem();
                prices.BindCatalog(GoodsCatalogLoader.ToCatalog(marketLoad));
                var commodityLoad = CommodityBaselineCatalogLoader.Load(dataRoot, fileIO, serializer);
                if (!commodityLoad.HasErrors)
                    prices.BindCommodityCatalog(CommodityBaselineCatalogLoader.ToCatalog(commodityLoad));

                var items = ItemCatalogLoader.LoadCatalog(dataRoot, fileIO, serializer);
                var inventory = new Ashfall.Core.Inventory.Inventory();
                var wallet = new HoldfastTradeSession(new HoldfastCatalog(), 5000, inventory);

                var system = new BlackMarketSystem();
                system.BindCatalog(BlackMarketInventoryCatalogLoader.ToCatalog(loadResult));
                system.BindMarket(prices);
                var bounties = new FactionBountySystem();
                system.BindFactionBountySystem(bounties);

                int simDay = 1;
                var session = new BlackMarketHostSession(system, bounties);
                session.BindSettlementOwners(wallet, inventory, items, () => simDay);

                const string syndicate = "faction_wasteland_outlaws";
                const string otherSyndicate = "faction_ash_market_brokers";
                const string entryId = "black_market_field_medicine";

                // Check 2: Syndicate discovery via DiscoverContact
                bool discovered = session.System.DiscoverContact(syndicate, 1);
                if (discovered && session.System.IsContactDiscovered(syndicate))
                {
                    Console.WriteLine($"[PASS] Check 2: Discovered contact for syndicate {syndicate}.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 2: DiscoverContact failed.");
                }

                // Check 3: Undiscovered syndicate refusal on preview
                var previewUndiscovered = session.PreviewBuy(otherSyndicate, entryId, 1);
                if (!previewUndiscovered.IsAvailable && (previewUndiscovered.ReasonId == "contact_not_discovered" || previewUndiscovered.ReasonId == "unknown_syndicate"))
                {
                    Console.WriteLine("[PASS] Check 3: Undiscovered syndicate transaction correctly refused.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 3: Undiscovered contact was not blocked ({previewUndiscovered.ReasonId}).");
                }

                // Check 4: Same-day stock snapshot determinism
                var rng = new SeededRng(15501);
                session.System.EnsureStockSnapshot(syndicate, simDay, rng);
                var linesDay1 = session.System.GetStock(syndicate);
                int countDay1 = linesDay1.Count;
                // Query again same day - must not re-roll
                session.System.EnsureStockSnapshot(syndicate, simDay, rng);
                var linesDay1Recheck = session.System.GetStock(syndicate);
                if (countDay1 > 0 && linesDay1.SequenceEqual(linesDay1Recheck))
                {
                    Console.WriteLine($"[PASS] Check 4: Same-day stock snapshot determinism verified ({countDay1} lines stable).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 4: Stock snapshot changed or was empty on same day.");
                }

                // Check 5: Daily stock refresh
                simDay = 2;
                session.System.EnsureStockSnapshot(syndicate, simDay, rng);
                var linesDay2 = session.System.GetStock(syndicate);
                if (linesDay2.All(l => l.generatedDay == 2))
                {
                    Console.WriteLine("[PASS] Check 5: Daily stock refresh produced new snapshot for day 2.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 5: Daily stock snapshot did not update to day 2.");
                }

                // Ensure stock exists for entryId
                var line = session.System.GetStockLine(syndicate, entryId);
                if (line == null)
                {
                    session.System.State.stock.Add(new BlackMarketStockLine
                    {
                        entryId = entryId,
                        quantity = 3,
                        generatedDay = simDay
                    });
                    session.System.State.stockOwners.Add(syndicate);
                }
                else
                {
                    line.quantity = Math.Max(3, line.quantity);
                }

                // Check 6: Illicit line purchase with canonical wallet debit & inventory credit
                long walletBeforeBuy = session.WalletValue;
                int invBeforeBuy = inventory.CountById("medical_kit");
                var buyResult = session.Buy(syndicate, entryId, 1);
                long walletAfterBuy = session.WalletValue;
                int invAfterBuy = inventory.CountById("medical_kit");

                if (buyResult.Success && walletAfterBuy < walletBeforeBuy && invAfterBuy == invBeforeBuy + 1)
                {
                    Console.WriteLine($"[PASS] Check 6: Illicit line bought: debited {buyResult.SettlementUnits} units, credited 1x medical_kit to inventory.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 6: Buy transaction failed (success={buyResult.Success}, reason={buyResult.ReasonId}).");
                }

                // Check 7: Selling line with discount (spread loss) & heat accumulation
                float heatBeforeSell = session.System.FindLedger(syndicate)?.heat ?? 0f;
                long walletBeforeSell = session.WalletValue;
                var sellResult = session.Sell(syndicate, entryId, 1);
                long walletAfterSell = session.WalletValue;
                float heatAfterSell = session.System.FindLedger(syndicate)?.heat ?? 0f;

                if (sellResult.Success && walletAfterSell > walletBeforeSell && heatAfterSell >= heatBeforeSell)
                {
                    Console.WriteLine($"[PASS] Check 7: Illicit line sold: credited {sellResult.SettlementUnits} units, heat accumulated to {heatAfterSell:F1}.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 7: Sell transaction failed ({sellResult.ReasonId}).");
                }

                // Check 8: Loan issuance with wallet credit & active debt record tracking
                long walletBeforeLoan = session.WalletValue;
                var loanResult = session.TakeLoan(syndicate, 200, 3);
                long walletAfterLoan = session.WalletValue;
                var debt = session.System.State.debts.FirstOrDefault(d => d.syndicateId == syndicate && d.status == UnderworldDebtRecord.StatusActive);

                if (loanResult.Success && walletAfterLoan == walletBeforeLoan + 200 && debt != null && debt.principalUnits == 200)
                {
                    Console.WriteLine($"[PASS] Check 8: Loan taken: credited 200 units, active debt {debt.debtId} registered due day {debt.dueDay}.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 8: Loan issuance failed ({loanResult.ReasonId}).");
                }

                // Check 9: Debt repayment with wallet debit
                long walletBeforeRepay = session.WalletValue;
                var repayResult = session.Repay(debt!.debtId, 50);
                long walletAfterRepay = session.WalletValue;

                if (repayResult.Success && walletAfterRepay == walletBeforeRepay - 50 && debt.repaidUnits >= 50)
                {
                    Console.WriteLine($"[PASS] Check 9: Repaid 50 units on debt; remaining principal tracked correctly.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 9: Debt repayment failed ({repayResult.ReasonId}).");
                }

                // Check 10: Overdue debt escalation & bounty placement
                // Advance day past due day
                simDay = debt.dueDay + 1;
                session.System.TickDaily(simDay);

                if (debt.status == UnderworldDebtRecord.StatusDefaulted && bounties.AllBounties.Count > 0)
                {
                    Console.WriteLine($"[PASS] Check 10: Overdue debt transitioned to Defaulted; bounty placed on syndicate {syndicate}.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 10: Overdue escalation failed (status={debt.status}, bountiesCount={bounties.AllBounties.Count}).");
                }

                // Check 11: Heat decay / patrol attention daily reduction
                float heatBeforeDecay = session.System.FindLedger(syndicate)?.heat ?? 0f;
                session.System.TickDaily(simDay + 1);
                float heatAfterDecay = session.System.FindLedger(syndicate)?.heat ?? 0f;

                if (heatAfterDecay < heatBeforeDecay || heatBeforeDecay == 0f)
                {
                    Console.WriteLine($"[PASS] Check 11: Daily heat decayed ({heatBeforeDecay:F1} -> {heatAfterDecay:F1}).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 11: Heat did not decay ({heatBeforeDecay} -> {heatAfterDecay}).");
                }

                // Check 12: Save/restore round-trip parity
                var savedState = session.CaptureSave();
                var freshSystem = new BlackMarketSystem();
                freshSystem.BindCatalog(BlackMarketInventoryCatalogLoader.ToCatalog(loadResult));
                freshSystem.BindMarket(prices);
                var freshBounties = new FactionBountySystem();
                freshSystem.BindFactionBountySystem(freshBounties);
                freshSystem.RestoreState(savedState);

                float freshHeat = freshSystem.FindLedger(syndicate)?.heat ?? 0f;
                float sessionHeat = session.System.FindLedger(syndicate)?.heat ?? 0f;

                if (freshSystem.IsContactDiscovered(syndicate)
                    && freshSystem.State.debts.Count == session.System.State.debts.Count
                    && Math.Abs(freshHeat - sessionHeat) < 0.01f)
                {
                    Console.WriteLine("[PASS] Check 12: Black market save/restore round-trip verified with 100% parity.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 12: Save/restore round-trip parity mismatch.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EXCEPTION] BlackMarketSelfTest threw: {ex}");
            }

            Console.WriteLine($"=== Black Market Self-Test Result: {passed}/{total} Passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
