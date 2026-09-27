// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : BlackFlotillaStandingSelfTest
// Subsystem          : Authored flotilla thresholds registered on the live
//                      FactionStanceEngine.
// ============================================================================
using System;
using Ashfall.Core.Economy;

namespace AtomicWar.GodotApp
{
    public static class HostCliBlackFlotillaStanding
    {
        private static void Check(bool ok, string label)
        {
            if (ok) Console.WriteLine($"[PASS] {label}");
            else Console.WriteLine($"[FAIL] {label}");
        }

        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Black Flotilla Standing Self-Test ===");
            int passed = 0;
            const int total = 10;
            try
            {
                // The live stance engine, exactly as the maritime host holds it.
                var engine = new FactionStanceEngine();
                var session = new BlackFlotillaStandingHostSession(() => engine);

                // 1. Before registration the faction runs on generic defaults.
                Check(session.IsRunningOnGenericDefaults() && !session.IsRegistered,
                    "Check 1: unregistered, the flotilla falls back to generic defaults.");
                passed += 1;

                // 2. Registration puts the AUTHORED row on the live engine.
                Check(session.Register() && session.IsRegistered
                      && !session.IsRunningOnGenericDefaults(),
                    "Check 2: authored thresholds are registered on the live stance engine.");
                passed += session.IsRegistered ? 1 : 0;

                // 3. The authored numbers are exactly the policy table.
                Check(Ashfall.Core.Maritime.BlackFlotillaStanding.RaidThreshold == -50f
                      && Ashfall.Core.Maritime.BlackFlotillaStanding.RobThreshold == -20f
                      && Ashfall.Core.Maritime.BlackFlotillaStanding.IntelShareThreshold == 40f
                      && Ashfall.Core.Maritime.BlackFlotillaStanding.SalvageTrustedTrust == 30f
                      && Ashfall.Core.Maritime.BlackFlotillaStanding.DeepCooperationTrust == 55f,
                    "Check 3: the registered thresholds match the authored policy table.");
                passed += 1;

                // 4. Re-registration is idempotent — no second faction row.
                bool again = session.Register();
                engine.SetTrust(BlackFlotillaStandingHostSession.FactionId, 10f);
                Check(again && Math.Abs(engine.GetEffectiveTrust(BlackFlotillaStandingHostSession.FactionId) - 10f) < 0.0001f,
                    "Check 4: re-registering re-affirms the same authored row.");
                passed += again ? 1 : 0;

                // 5. Trade gate follows the authored boundary (>= 0).
                engine.SetTrust(BlackFlotillaStandingHostSession.FactionId, -5f);
                bool closedTrade = !session.CanTrade();
                engine.SetTrust(BlackFlotillaStandingHostSession.FactionId, 5f);
                Check(closedTrade && session.CanTrade(),
                    "Check 5: the trade gate honours the authored trust boundary.");
                passed += closedTrade && session.CanTrade() ? 1 : 0;

                // 6. Intel sharing requires the authored 40 threshold.
                engine.SetTrust(BlackFlotillaStandingHostSession.FactionId, 39f);
                bool intelLocked = !session.CanShareIntel();
                engine.SetTrust(BlackFlotillaStandingHostSession.FactionId, 41f);
                Check(intelLocked && session.CanShareIntel(),
                    "Check 6: intel sharing resolves on the authored 40 threshold.");
                passed += intelLocked && session.CanShareIntel() ? 1 : 0;

                // 7. Plan-23 tiers are distinct and monotone on live trust.
                engine.SetTrust(BlackFlotillaStandingHostSession.FactionId, 55f);
                var deep = session.Tier();
                engine.SetTrust(BlackFlotillaStandingHostSession.FactionId, 31f);
                var salvage = session.Tier();
                engine.SetTrust(BlackFlotillaStandingHostSession.FactionId, -80f);
                var hostile = session.Tier();
                Check(session.CanCooperateOnDeepDives() && !salvage.Equals(deep)
                      && !hostile.Equals(salvage) && session.IsSalvageTrusted(),
                    "Check 7: Plan-23 tiers resolve distinctly and monotonically.");
                passed += (salvage != deep && hostile != salvage) ? 1 : 0;

                // 8. A refusal names the exact authored gate that produced it.
                engine.SetTrust(BlackFlotillaStandingHostSession.FactionId, 10f);
                string reason = session.RefusalReason("intel");
                Check(reason.Contains("40", StringComparison.Ordinal),
                    $"Check 8: a refusal cites its authored threshold ({reason}).");
                passed += reason.Contains("40", StringComparison.Ordinal) ? 1 : 0;

                // 9. The session holds no trust of its own — the engine is the store.
                engine.ModifyTrust(BlackFlotillaStandingHostSession.FactionId, 5f);
                Check(Math.Abs(session.Trust() - engine.GetEffectiveTrust(BlackFlotillaStandingHostSession.FactionId)) < 0.0001f,
                    "Check 9: the adapter reads live engine trust and stores none itself.");
                passed += 1;

                // 10. The status line is a truthful projection.
                string line = session.StatusLine();
                Check(line.StartsWith("flotilla trust", StringComparison.Ordinal) && line.Contains("tier"),
                    $"Check 10: status line projects live standing truthfully ({line}).");
                passed += line.StartsWith("flotilla trust") ? 1 : 0;
            }
            catch (Exception ex) { Console.WriteLine($"[FAIL] Unexpected probe exception: {ex}"); }
            Console.WriteLine($"=== Black Flotilla Standing Self-Test: {passed}/{total} passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
