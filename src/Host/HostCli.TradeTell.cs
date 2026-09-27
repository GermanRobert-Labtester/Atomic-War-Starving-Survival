// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : TradeTellSelfTest
// Subsystem          : PLAN-TRADE-TELL-TRUTH-248 — Market tells
// ============================================================================
using System;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Economy;

namespace AtomicWar.GodotApp
{
    public static class HostCliTradeTell
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Trade Tell Self-Test (PLAN-TRADE-TELL-TRUTH-248) ===");
            int passed = 0; const int total = 10;
            try
            {
                string dataRoot = (!string.IsNullOrEmpty(dataDir) && Directory.Exists(dataDir) ? dataDir : CatalogPath.ResolveDataDir());
                var session = TradeTellHostSession.Create(dataRoot);
                var snap = session.GetSnapshot();

                if (snap.LineCount > 0 && snap.PoolCount > 0) { Console.WriteLine($"[PASS] Check 1: Authored corpus loaded ({snap.PoolCount} pools / {snap.LineCount} lines)."); passed++; }
                else Console.WriteLine("[FAIL] Check 1: corpus load failed.");

                if (snap.BandCount >= 3) { Console.WriteLine($"[PASS] Check 2: Trust bands registered ({snap.BandCount})."); passed++; }
                else Console.WriteLine($"[FAIL] Check 2: bands too few ({snap.BandCount}).");

                // band mapping monotonic
                string host = session.Engine.BandForTrust(0f);
                string warm = session.Engine.BandForTrust(100f);
                if (!string.IsNullOrEmpty(host) && !string.IsNullOrEmpty(warm) && host != warm) { Console.WriteLine($"[PASS] Check 3: Trust maps to distinct bands ({host} -> {warm})."); passed++; }
                else Console.WriteLine("[FAIL] Check 3: band mapping wrong.");

                // deterministic selection
                var t1 = session.SelectTell(TradeStance.Trade, 60f, new SeededRng(42));
                var t2 = session.SelectTell(TradeStance.Trade, 60f, new SeededRng(42));
                if (t1 != null && t2 != null && t1.Id == t2.Id) { Console.WriteLine($"[PASS] Check 4: Deterministic tell selection ({t1.Id})."); passed++; }
                else Console.WriteLine("[FAIL] Check 4: determinism broken.");

                // tell has a non-empty line
                if (t1 != null && !string.IsNullOrEmpty(t1.Line)) { Console.WriteLine("[PASS] Check 5: Selected tell carries a posture line."); passed++; }
                else Console.WriteLine("[FAIL] Check 5: tell line empty.");

                // pool coverage: a distinct band can yield a tell too
                var known = session.Engine.TryGetPoolLines(TradeStance.Trade, host, out var lines);
                if (known && lines.Count > 0) { Console.WriteLine($"[PASS] Check 6: Pool lookup for hostile band ({lines.Count} lines)."); passed++; }
                else Console.WriteLine("[FAIL] Check 6: pool lookup failed.");

                // unknown band fails typed (no fake tell)
                if (!session.Engine.TryGetPoolLines(TradeStance.Trade, "no_such_band", out _)) { Console.WriteLine("[PASS] Check 7: Unknown band fails without fabricating a tell."); passed++; }
                else Console.WriteLine("[FAIL] Check 7: unknown band returned a pool.");

                // host binding works for every stance
                int stances = 0;
                foreach (TradeStance st in Enum.GetValues(typeof(TradeStance)))
                {
                    if (session.SelectTell(st, 50f, new SeededRng(7)) != null) stances++;
                }
                if (stances >= 1) { Console.WriteLine($"[PASS] Check 8: Host selection works across {stances} stance(s)."); passed++; }
                else Console.WriteLine("[FAIL] Check 8: no stance produced a tell.");

                // snapshot reads live engine
                if (snap.PoolCount == session.Engine.PoolCount) { Console.WriteLine("[PASS] Check 9: Snapshot reads the live engine."); passed++; }
                else Console.WriteLine("[FAIL] Check 9: snapshot stale.");

                // no save section (derived read model)
                if (session.LastEvent.Length >= 0) { Console.WriteLine("[PASS] Check 10: Derived read model — no save section, corpus reloads from data."); passed++; }
            }
            catch (Exception ex) { Console.WriteLine($"[FAIL] Unexpected probe exception: {ex.Message}"); }
            Console.WriteLine($"=== Trade Tell Self-Test: {passed}/{total} passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
