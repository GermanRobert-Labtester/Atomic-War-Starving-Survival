// SPDX-License-Identifier: MIT
using System;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Flags;

namespace AtomicWar.GodotApp
{
    public static class HostCliKnockWhitelist
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Knock Whitelist Self-Test (PLAN-KNOCK-WHITELIST-TRUTH-155) ===");
            int passed = 0; const int total = 6;
            try
            {
                string dataRoot = (!string.IsNullOrEmpty(dataDir) && Directory.Exists(dataDir) ? dataDir : CatalogPath.ResolveDataDir());
                var session = new KnockWhitelistHostSession();
                session.Load(dataRoot);

                if (session.Count > 0) { Console.WriteLine($"[PASS] Check 1: Authored whitelist loaded ({session.Count} entries)."); passed++; }
                else Console.WriteLine("[FAIL] Check 1: no entries loaded.");

                if (session.Contains("knock_exp07_vel_vigil")) { Console.WriteLine("[PASS] Check 2: Known knock id present."); passed++; }
                else Console.WriteLine("[FAIL] Check 2: known knock missing.");

                var flags = new InMemoryFlagLedger();
                flags.Set("flag_exp07_vel_vigil_knock", "probe");
                if (session.Validate("door.knock.practiced", flags, out var diag) && diag.Contains("orphan_validated"))
                { Console.WriteLine($"[PASS] Check 3: Gated deliberate knock validated."); passed++; }
                else Console.WriteLine($"[FAIL] Check 3: gated knock rejected ({diag}).");

                var empty = new InMemoryFlagLedger();
                if (!session.Validate("door.knock.practiced", empty, out var diag2) && diag2.Contains("orphan_rejected"))
                { Console.WriteLine("[PASS] Check 4: Ungated knock refused (not silently dropped)."); passed++; }
                else Console.WriteLine($"[FAIL] Check 4: ungated knock not refused ({diag2}).");

                if (!session.Validate("door.knock.unregistered", flags, out _)) { Console.WriteLine("[PASS] Check 5: Unregistered knock refused."); passed++; }
                else Console.WriteLine("[FAIL] Check 5: unregistered knock accepted.");

                if (session.LastEvent.Contains("orphan")) { Console.WriteLine("[PASS] Check 6: Last-event diagnostic is populated."); passed++; }
                else Console.WriteLine("[FAIL] Check 6: no diagnostic.");
            }
            catch (Exception ex) { Console.WriteLine($"[FAIL] Unexpected probe exception: {ex.Message}"); }
            Console.WriteLine($"=== Knock Whitelist Self-Test: {passed}/{total} passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
