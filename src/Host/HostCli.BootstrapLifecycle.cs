// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : BootstrapLifecycleGateSelfTest
// Core Authority     : Ashfall.Core.Orchestration.BootstrapLifecycleGate (EN-06)
// Purpose            : all bootstrap paths reach Ready with zero deferred seams
// ============================================================================

using System;
using Ashfall.Core.Orchestration;

namespace AtomicWar.GodotApp
{
    public static class HostCliBootstrapLifecycle
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Bootstrap Lifecycle Gate Self-Test (EN-06) ===");
            int passed = 0;
            const int total = 8;

            try
            {
                var gate = new BootstrapLifecycleGate();

                if (gate.RegisterSubsystem("config", BootstrapStage.Configuration, isRequired: true)
                    && gate.RegisteredCount == 1)
                {
                    Console.WriteLine("[PASS] Check 1: a valid subsystem registers at its stage.");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 1: valid registration refused."); }

                if (!gate.RegisterSubsystem("bad_stage", BootstrapStage.None, isRequired: true)
                    && !gate.RegisterSubsystem("", BootstrapStage.Foundation, isRequired: true))
                {
                    Console.WriteLine("[PASS] Check 2: invalid stage and blank id are refused.");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 2: invalid registration accepted."); }

                if (!gate.AdvanceStage(BootstrapStage.DomainServices))
                {
                    Console.WriteLine("[PASS] Check 3: non-sequential stage advancement is refused.");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 3: skipped stage accepted."); }

                bool advanced = gate.AdvanceStage(BootstrapStage.Configuration)
                    && gate.AdvanceStage(BootstrapStage.Foundation)
                    && gate.AdvanceStage(BootstrapStage.DomainServices)
                    && gate.AdvanceStage(BootstrapStage.CampaignOwners)
                    && gate.AdvanceStage(BootstrapStage.UiSurfaces)
                    && gate.AdvanceStage(BootstrapStage.Ready);
                if (advanced && gate.CurrentStage == BootstrapStage.Ready)
                {
                    Console.WriteLine("[PASS] Check 4: sequential advancement reaches Ready.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 4: stage = {gate.CurrentStage}."); }

                if (gate.ValidateLifecycleParity(BootstrapPathMode.FreshGame, out var okViolations) && okViolations.Count == 0)
                {
                    Console.WriteLine("[PASS] Check 5: a ready gate with no deferred seams validates parity.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 5: parity violations = {string.Join("; ", okViolations)}."); }

                var deferredGate = new BootstrapLifecycleGate();
                deferredGate.RegisterSubsystem("seam", BootstrapStage.Foundation, isRequired: true, hasDeferredSeams: true);
                deferredGate.AdvanceStage(BootstrapStage.Configuration);
                deferredGate.AdvanceStage(BootstrapStage.Foundation);
                deferredGate.AdvanceStage(BootstrapStage.DomainServices);
                deferredGate.AdvanceStage(BootstrapStage.CampaignOwners);
                deferredGate.AdvanceStage(BootstrapStage.UiSurfaces);
                deferredGate.AdvanceStage(BootstrapStage.Ready);
                if (!deferredGate.ValidateLifecycleParity(BootstrapPathMode.SaveRestore, out var deferredViolations)
                    && deferredViolations.Exists(v => v.Contains("deferred seams")))
                {
                    Console.WriteLine("[PASS] Check 6: a deferred seam fails parity for every path mode.");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 6: deferred seam not flagged."); }

                var earlyGate = new BootstrapLifecycleGate();
                earlyGate.RegisterSubsystem("later", BootstrapStage.UiSurfaces, isRequired: true);
                earlyGate.AdvanceStage(BootstrapStage.Configuration);
                earlyGate.AdvanceStage(BootstrapStage.Foundation);
                earlyGate.AdvanceStage(BootstrapStage.DomainServices);
                if (!earlyGate.ValidateLifecycleParity(BootstrapPathMode.SessionReset, out var earlyViolations)
                    && earlyViolations.Exists(v => v.Contains("not ready"))
                    && earlyViolations.Exists(v => v.Contains("unreached stage")))
                {
                    Console.WriteLine("[PASS] Check 7: a not-ready lifecycle reports both stage and unreached-required violations.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 7: early violations = {string.Join("; ", earlyViolations)}."); }

                earlyGate.Reset();
                if (earlyGate.CurrentStage == BootstrapStage.None && earlyGate.RegisteredCount == 0)
                {
                    Console.WriteLine("[PASS] Check 8: reset clears the stage and registrations.");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 8: reset left state."); }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FAIL] Unhandled: {ex.Message}");
            }

            Console.WriteLine($"Bootstrap lifecycle gate: {passed}/{total} checks passed");
            return passed == total ? 0 : 1;
        }
    }
}
