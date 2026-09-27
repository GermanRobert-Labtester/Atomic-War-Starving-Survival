// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : StoreCapabilitySelfTest
// Core Authority     : Ashfall.Core.Launch.StoreCapabilityManifest (Plan 57)
// Purpose            : store claims cannot run ahead of shipped systems/gates
// ============================================================================

using System;
using System.Collections.Generic;
using Ashfall.Core.Launch;

namespace AtomicWar.GodotApp
{
    public static class HostCliStoreCapability
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Store Capability Manifest Self-Test (Plan 57 / Plan 48 release craft) ===");
            int passed = 0;
            const int total = 8;

            try
            {
                var data = new StoreCapabilityManifestData
                {
                    ManifestId = "store_selftest",
                    ProductTitle = "ASHFALL",
                    Claims = new List<StoreCapabilityClaim>
                    {
                        new StoreCapabilityClaim { ClaimId = "claim_ok", BackingSystem = "System.A", VerificationGate = "gate.ok" },
                        new StoreCapabilityClaim { ClaimId = "claim_no_system", BackingSystem = "", VerificationGate = "gate.ok" },
                        new StoreCapabilityClaim { ClaimId = "claim_missing_system", BackingSystem = "System.Missing", VerificationGate = "gate.ok" },
                        new StoreCapabilityClaim { ClaimId = "claim_failing_gate", BackingSystem = "System.A", VerificationGate = "gate.fail" }
                    }
                };

                int substantiated = 0;
                var manifest = new StoreCapabilityManifest(data);
                manifest.OnUnsubstantiatedClaimDetectedSeam += (_, _) => substantiated++;

                bool System(string id) => id == "System.A";
                bool Gate(string id) => id == "gate.ok";

                var noSystem = manifest.ValidateClaim(data.Claims[1], System, Gate);
                if (!noSystem.Passed && noSystem.Message.Contains("no declared backing system"))
                {
                    Console.WriteLine("[PASS] Check 1: a claim without a backing system is rejected.");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 1: unbacked claim passed."); }

                var missing = manifest.ValidateClaim(data.Claims[2], System, Gate);
                if (!missing.Passed && missing.Message.Contains("not available"))
                {
                    Console.WriteLine("[PASS] Check 2: a claim backed by an unavailable system is rejected.");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 2: missing-system claim passed."); }

                var failing = manifest.ValidateClaim(data.Claims[3], System, Gate);
                if (!failing.Passed && failing.Message.Contains("gate.fail"))
                {
                    Console.WriteLine("[PASS] Check 3: a claim whose verification gate is not passing is rejected.");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 3: failing-gate claim passed."); }

                var ok = manifest.ValidateClaim(data.Claims[0], System, Gate);
                if (ok.Passed && ok.Message.Contains("System.A"))
                {
                    Console.WriteLine("[PASS] Check 4: a substantiated claim with a passing gate is verified.");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 4: valid claim rejected."); }

                var nullClaim = manifest.ValidateClaim(null!, System, Gate);
                if (!nullClaim.Passed && nullClaim.ClaimId == "null")
                {
                    Console.WriteLine("[PASS] Check 5: a null claim reference fails closed.");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 5: null claim not refused."); }

                int substantiatedBefore = substantiated;
                var report = manifest.AuditAllClaims(System, Gate);
                if (report.TotalClaims == 4 && report.VerifiedClaims == 1 && report.RejectedClaims == 3)
                {
                    Console.WriteLine($"[PASS] Check 6: audit tallies exactly one verified / three rejected ({report.VerifiedClaims}/{report.RejectedClaims}).");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 6: audit tally {report.VerifiedClaims}/{report.RejectedClaims} of {report.TotalClaims}."); }

                if (!report.AllClaimsVerified && substantiated > substantiatedBefore)
                {
                    Console.WriteLine("[PASS] Check 7: the unsubstantiated-claim seam fires for every rejection and the report refuses blanket verification.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 7: seam fired {substantiated - substantiatedBefore} times; AllClaimsVerified={report.AllClaimsVerified}."); }

                var empty = new StoreCapabilityManifest(new StoreCapabilityManifestData { ManifestId = "empty" });
                var emptyReport = empty.AuditAllClaims(System, Gate);
                if (!emptyReport.AllClaimsVerified && emptyReport.TotalClaims == 0)
                {
                    Console.WriteLine("[PASS] Check 8: an empty manifest never claims blanket verification.");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 8: empty manifest verified."); }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FAIL] Unhandled: {ex.Message}");
            }

            Console.WriteLine($"Store capability manifest: {passed}/{total} checks passed");
            return passed == total ? 0 : 1;
        }
    }
}
