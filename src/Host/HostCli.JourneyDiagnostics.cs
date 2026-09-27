// SPDX-License-Identifier: MIT
using System;
using System.Text.Json;

namespace AtomicWar.GodotApp
{
    public static class HostCliJourneyDiagnostics
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Journey Diagnostics Self-Test (PLAN-JOURNEY-CONTEXT-TRUTH-156) ===");
            int passed = 0; const int total = 6;
            try
            {
                var session = new JourneyDiagnosticsHostSession();
                if (!session.Active) { Console.WriteLine("[PASS] Check 1: Context is inactive before Begin."); passed++; }
                else Console.WriteLine("[FAIL] Check 1: context active prematurely.");

                session.Begin("smoke_playthrough", 12345UL);
                if (session.Active && session.JourneyName == "smoke_playthrough") { Console.WriteLine("[PASS] Check 2: Begin establishes the journey context."); passed++; }
                else Console.WriteLine("[FAIL] Check 2: Begin failed.");

                session.Navigate("shelter", "open_panel");
                session.AdvanceDay(2);
                if (session.StepIndex == 2 && session.Day == 2 && session.CurrentRoute == "shelter")
                { Console.WriteLine("[PASS] Check 3: Navigate/AdvanceDay update route and day."); passed++; }
                else Console.WriteLine("[FAIL] Check 3: context did not update.");

                string line = session.DescribeFailure("panel missing");
                if (line.Contains("[JOURNEY_FAILURE]") && line.Contains("seed=12345") && line.Contains("day=2"))
                { Console.WriteLine("[PASS] Check 4: Failure diagnostic carries seed/day/route."); passed++; }
                else Console.WriteLine($"[FAIL] Check 4: diagnostic malformed ({line}).");

                string json = session.FailureJson("panel missing");
                bool ok = false;
                string? parseError = null;
                try
                {
                    using var doc = JsonDocument.Parse(json);
                    ok = doc.RootElement.TryGetProperty("status", out var status)
                        && status.ValueKind == JsonValueKind.String
                        && string.Equals(status.GetString(), "FAILED", StringComparison.Ordinal);
                }
                catch (JsonException ex)
                {
                    parseError = ex.Message;
                }
                if (ok) { Console.WriteLine("[PASS] Check 5: Failure JSON is machine-readable."); passed++; }
                else if (parseError != null)
                    Console.WriteLine($"[FAIL] Check 5: Failure JSON malformed ({parseError}).");
                else Console.WriteLine("[FAIL] Check 5: JSON is missing the expected FAILED status.");

                session.End();
                if (!session.Active) { Console.WriteLine("[PASS] Check 6: End releases the context."); passed++; }
                else Console.WriteLine("[FAIL] Check 6: End failed.");
            }
            catch (Exception ex) { Console.WriteLine($"[FAIL] Unexpected probe exception: {ex.Message}"); }
            Console.WriteLine($"=== Journey Diagnostics Self-Test: {passed}/{total} passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
