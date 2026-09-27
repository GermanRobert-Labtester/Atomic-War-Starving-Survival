// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : BarterSelfTest
// Subsystem          : Plan 213 — Survivor Barter & Informal Economy
// ============================================================================

using System;
using Ashfall.Core.Economy;

namespace AtomicWar.GodotApp
{
    public static class HostCliBarter
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Survivor Barter Self-Test (Plan 213) ===");
            int passed = 0;
            const int total = 7;

            try
            {
                string data = string.IsNullOrWhiteSpace(dataDir) ? CatalogPath.ResolveDataDir() : dataDir;
                var session = SurvivorBarterHostSession.Create();
                bool catalog = session.LoadCatalog(data);

                if (catalog && session.System.GetRule("standard_community_barter") != null)
                {
                    Console.WriteLine("[PASS] Check 1: Barter rule catalog loaded.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 1: Catalog not ready ({catalog}).");
                }

                if (session.System.CaptureState().ActiveRuleId == "standard_community_barter")
                {
                    Console.WriteLine("[PASS] Check 2: Active barter rule set.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 2: Active rule selection failed.");
                }

                var offer = session.CreateOffer("surv_a", "surv_b", 3, new[] { "item_knife" }, new[] { "item_tape" });
                if (offer != null && offer.OffererId == "surv_a" && offer.TargetSurvivorId == "surv_b")
                {
                    Console.WriteLine("[PASS] Check 3: Barter offer created between two survivors.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 3: Offer creation failed.");
                }

                if (session.CreateOffer("surv_a", "surv_a", 3) == null)
                {
                    Console.WriteLine("[PASS] Check 4: Self-directed offer rejected.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 4: Self-directed offer accepted.");
                }

                var trade = session.AcceptOffer(offer.OfferId, 3);
                if (trade != null && trade.OfferId == (offer?.OfferId ?? string.Empty))
                {
                    Console.WriteLine("[PASS] Check 5: Offer accepted and trade recorded.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 5: Offer acceptance failed.");
                }

                var reputation = session.GetReputation("surv_a", "surv_b");
                if (reputation != null && reputation.TraderAId.Length + reputation.TraderBId.Length > 0)
                {
                    Console.WriteLine($"[PASS] Check 6: Pairwise reputation readable (trust {reputation.TrustLevel:0}).");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 6: Reputation lookup failed.");
                }

                bool saved = session.TrySave();
                var reloaded = SurvivorBarterHostSession.Create();
                bool loaded = reloaded.TryLoad();
                if (saved && loaded && reloaded.CompletedTradeCount == 1
                    && SurvivorBarterSaveStore.SectionName.Equals("survivor_barter", StringComparison.Ordinal)
                    && SurvivorBarterSaveStore.FileName.Equals("survivor_barter_save.json", StringComparison.Ordinal))
                {
                    Console.WriteLine("[PASS] Check 7: Save/restore and store contract verified.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 7: Save/restore failed (saved={saved}, loaded={loaded}).");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FAIL] Self-test crashed: {ex.Message}");
            }

            Console.WriteLine($"=== Survivor Barter Self-Test: {passed}/{total} passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
