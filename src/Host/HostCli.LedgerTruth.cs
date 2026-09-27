// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : LedgerTruthGateSelfTest
// Core Authority     : Ashfall.Core.Orchestration.LedgerTruthIntegrityGate (EN-08)
// Purpose            : decision-register invariants + zero-quarantine truth
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Orchestration;

namespace AtomicWar.GodotApp
{
    public static class HostCliLedgerTruth
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Ledger Truth Integrity Self-Test (EN-08) ===");
            int passed = 0;
            const int total = 8;

            try
            {
                var terminal = new List<DecisionEntry>
                {
                    new DecisionEntry("DEC-1", "Signed decision", "SIGNED"),
                    new DecisionEntry("DEC-2", "Declined decision", "DECLINED"),
                    new DecisionEntry("DEC-3", "Retired decision", "RETIRED")
                };
                var terminalReport = LedgerTruthIntegrityGate.Validate(terminal);
                if (terminalReport.IsValid && terminalReport.TerminalDecisionsCount == 3 && terminalReport.TotalDecisionsEvaluated == 3)
                {
                    Console.WriteLine("[PASS] Check 1: signed/declined/retired entries are terminal and valid.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 1: terminal report invalid ({string.Join("; ", terminalReport.Violations)})."); }

                var deferred = new List<DecisionEntry>
                {
                    new DecisionEntry("DEC-4", "Deferred with condition", "DEFERRED-WITH-CONDITION", "needs F13 signature")
                };
                var deferredReport = LedgerTruthIntegrityGate.Validate(deferred);
                if (deferredReport.IsValid && deferredReport.ValidDeferredCount == 1)
                {
                    Console.WriteLine("[PASS] Check 2: a named-condition deferral is valid.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 2: named deferral rejected ({string.Join("; ", deferredReport.Violations)})."); }

                var open = new List<DecisionEntry> { new DecisionEntry("DEC-5", "Open item", "DEFERRED-WITH-CONDITION") };
                var openReport = LedgerTruthIntegrityGate.Validate(open);
                if (!openReport.IsValid && openReport.Violations.Count == 1)
                {
                    Console.WriteLine("[PASS] Check 3: a deferral without a named blocking condition is flagged.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 3: unnamed deferral not flagged ({openReport.Violations.Count} violations)."); }

                var badVerdict = new List<DecisionEntry> { new DecisionEntry("DEC-6", "Open verdict", "PENDING") };
                var badReport = LedgerTruthIntegrityGate.Validate(badVerdict);
                if (!badReport.IsValid && badReport.Violations.Any(v => v.Contains("PENDING")))
                {
                    Console.WriteLine("[PASS] Check 4: an open non-terminal verdict is a violation.");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 4: open verdict not flagged."); }

                var noId = new List<DecisionEntry> { new DecisionEntry("", "Missing id", "SIGNED") };
                var noIdReport = LedgerTruthIntegrityGate.Validate(noId);
                if (!noIdReport.IsValid && noIdReport.Violations.Any(v => v.Contains("missing DecisionId")))
                {
                    Console.WriteLine("[PASS] Check 5: a decision without an id is a violation.");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 5: missing-id decision not flagged."); }

                var quarantine = LedgerTruthIntegrityGate.Validate(terminal, new[] { "SomeQuarantinedTests" });
                if (!quarantine.IsValid && quarantine.QuarantinedTestCount == 1)
                {
                    Console.WriteLine("[PASS] Check 6: any active quarantine entry is flagged (D21 empty-quarantine truth).");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 6: quarantine entry not flagged."); }

                var emptyQuarantine = LedgerTruthIntegrityGate.Validate(terminal, Array.Empty<string>());
                if (emptyQuarantine.IsValid && emptyQuarantine.QuarantinedTestCount == 0)
                {
                    Console.WriteLine("[PASS] Check 7: an empty quarantine manifest passes.");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 7: empty quarantine rejected."); }

                var mixed = new List<DecisionEntry>
                {
                    new DecisionEntry("DEC-7", "Signed", "SIGNED"),
                    new DecisionEntry("DEC-8", "Deferred", "DEFERRED-WITH-CONDITION", "condition"),
                    new DecisionEntry("DEC-9", "Bad", "OPEN")
                };
                var mixedReport = LedgerTruthIntegrityGate.Validate(mixed);
                if (!mixedReport.IsValid && mixedReport.TotalDecisionsEvaluated == 3
                    && mixedReport.TerminalDecisionsCount == 1 && mixedReport.ValidDeferredCount == 1)
                {
                    Console.WriteLine("[PASS] Check 8: mixed register preserves exact terminal/deferred/violation counts.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 8: mixed counts T={mixedReport.TerminalDecisionsCount} D={mixedReport.ValidDeferredCount} V={mixedReport.Violations.Count}."); }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FAIL] Unhandled: {ex.Message}");
            }

            Console.WriteLine($"Ledger truth integrity: {passed}/{total} checks passed");
            return passed == total ? 0 : 1;
        }
    }
}
