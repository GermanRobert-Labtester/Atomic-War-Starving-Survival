// SPDX-License-Identifier: MIT
// Host CLI self-test probe for Expansion 33 (The Weather — storm forecast readiness).

using System;
using System.IO;
using Godot;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    public static class HostCliStormForecast
    {
        public static int RunSelfTest(string dataDir)
        {
            GD.Print("=== [HostCli] Storm Forecast Readiness Self-Test (Expansion 33) ===");
            int passed = 0;
            int total = 12;

            try
            {
                // Check 1: a weak observation post yields no usable forecast.
                var unknown = StormForecastReadinessEngine.EvaluateForecastReliability(50, 6, StormSeverityClass.AshStorm, 1);
                if (unknown.ConfidenceTier == ForecastConfidenceTier.Unknown && !unknown.IssueWarning)
                {
                    GD.Print("[PASS] Check 1: Weak observation post returns Unknown.");
                    passed++;
                }
                else GD.PrintErr($"[FAIL] Check 1: Weak post returned {unknown.ConfidenceTier}.");

                // Check 2: a strong post at short lead time is high confidence.
                var strong = StormForecastReadinessEngine.EvaluateForecastReliability(1000, 6, StormSeverityClass.AshStorm, 1);
                if (strong.ConfidenceTier == ForecastConfidenceTier.HighConfidence && strong.IssueWarning)
                {
                    GD.Print($"[PASS] Check 2: Strong short-lead forecast {strong.ConfidenceTier} ({strong.ConfidencePermille}\u2030), warning issued.");
                    passed++;
                }
                else GD.PrintErr($"[FAIL] Check 2: Strong forecast {strong.ConfidenceTier} ({strong.ConfidencePermille}\u2030).");

                // Check 3: lead-time beyond onset degrades confidence.
                var shortLead = StormForecastReadinessEngine.EvaluateForecastReliability(1000, 12, StormSeverityClass.AshStorm, 3);
                var longLead = StormForecastReadinessEngine.EvaluateForecastReliability(1000, 36, StormSeverityClass.AshStorm, 3);
                if (longLead.ConfidencePermille < shortLead.ConfidencePermille)
                {
                    GD.Print($"[PASS] Check 3: Lead-time decay {shortLead.ConfidencePermille}\u2030 -> {longLead.ConfidencePermille}\u2030.");
                    passed++;
                }
                else GD.PrintErr("[FAIL] Check 3: Lead time did not degrade confidence.");

                // Check 4: severe weather is harder to forecast.
                var clear = StormForecastReadinessEngine.EvaluateForecastReliability(900, 6, StormSeverityClass.Clear, 4);
                var blackRain = StormForecastReadinessEngine.EvaluateForecastReliability(900, 6, StormSeverityClass.BlackRain, 4);
                if (blackRain.ConfidencePermille < clear.ConfidencePermille)
                {
                    GD.Print($"[PASS] Check 4: Severity penalty applied ({clear.ConfidencePermille}\u2030 clear vs {blackRain.ConfidencePermille}\u2030 black rain).");
                    passed++;
                }
                else GD.PrintErr("[FAIL] Check 4: Severity penalty not applied.");

                // Check 5: warnings are not issued for clear weather even at high confidence.
                if (!clear.IssueWarning)
                {
                    GD.Print("[PASS] Check 5: No warning issued for clear weather.");
                    passed++;
                }
                else GD.PrintErr("[FAIL] Check 5: Warning issued for clear weather.");

                // Check 6: readiness composite is weighted (airlock 30 + filter 30 + medical 25 + drill 15).
                var perfect = StormForecastReadinessEngine.AssessSeasonalReadiness(1000, 1000, 1000, 1000, StormSeverityClass.Clear);
                if (perfect.ReadinessPermille == 1000 && perfect.ReadinessBand == SeasonalReadinessBand.Fortified)
                {
                    GD.Print("[PASS] Check 6: Perfect readiness composites to 1000\u2030 Fortified.");
                    passed++;
                }
                else GD.PrintErr($"[FAIL] Check 6: Composite readiness {perfect.ReadinessPermille}\u2030 {perfect.ReadinessBand}.");

                // Check 7: black-rain absorption requires the 800-permille threshold.
                var strongRain = StormForecastReadinessEngine.AssessSeasonalReadiness(1000, 1000, 1000, 1000, StormSeverityClass.BlackRain);
                var weakRain = StormForecastReadinessEngine.AssessSeasonalReadiness(500, 500, 500, 500, StormSeverityClass.BlackRain);
                if (!weakRain.CanAbsorbBlackRain && strongRain.CanAbsorbBlackRain)
                {
                    GD.Print("[PASS] Check 7: Black-rain absorption gated at 800\u2030.");
                    passed++;
                }
                else GD.PrintErr("[FAIL] Check 7: Black-rain absorption gate incorrect.");

                // Check 8: readiness band and primary gap.
                var badAirlock = StormForecastReadinessEngine.AssessSeasonalReadiness(200, 900, 900, 900, StormSeverityClass.AshStorm);
                if (badAirlock.ReadinessBand < SeasonalReadinessBand.WellPrepared && badAirlock.PrimaryGap.Contains("Airlock"))
                {
                    GD.Print($"[PASS] Check 8: Readiness {badAirlock.ReadinessBand}, primary gap '{badAirlock.PrimaryGap}'.");
                    passed++;
                }
                else GD.PrintErr($"[FAIL] Check 8: Gap identification incorrect ('{badAirlock.PrimaryGap}').");

                // Check 9: better instruments decay slower.
                int basicDecay = StormForecastReadinessEngine.CalculateObservationPostDecay(100);
                int goodDecay = StormForecastReadinessEngine.CalculateObservationPostDecay(900);
                if (goodDecay < basicDecay)
                {
                    GD.Print($"[PASS] Check 9: Instrument decay basic={basicDecay} > good={goodDecay}.");
                    passed++;
                }
                else GD.PrintErr("[FAIL] Check 9: Instrument quality did not slow decay.");

                // Check 10: ledger tick decays skill and drill recency; a drill resets recency.
                var ledger = new StormForecastLedger();
                int skillBefore = ledger.ObservationPostSkillPermille;
                ledger.RunDrill();
                int drillAfterRun = ledger.DrillRecencyPermille;
                ledger.TickDay(1, 100);
                if (ledger.ObservationPostSkillPermille < skillBefore
                    && ledger.DrillRecencyPermille < drillAfterRun && drillAfterRun == 1000)
                {
                    GD.Print($"[PASS] Check 10: Drill reset {drillAfterRun}\u2030; tick decayed skill to {ledger.ObservationPostSkillPermille}\u2030 and drill to {ledger.DrillRecencyPermille}\u2030.");
                    passed++;
                }
                else GD.PrintErr("[FAIL] Check 10: Ledger tick/drill behavior incorrect.");

                // Check 11: capture/restore + schema gate.
                ledger.EvaluateForecast(6, StormSeverityClass.AshStorm, 1);
                ledger.IssueWarning(StormForecastReadinessEngine.EvaluateForecastReliability(1000, 6, StormSeverityClass.AshStorm, 1));
                var state = ledger.CaptureState();
                var restored = new StormForecastLedger();
                restored.RestoreState(state);
                bool roundTrip = restored.WarningsIssued == ledger.WarningsIssued
                    && restored.ObservationPostSkillPermille == ledger.ObservationPostSkillPermille;
                bool newerRejected = false;
                try
                {
                    var newer = ledger.CaptureState();
                    newer.SchemaVersion = 99;
                    restored.RestoreState(newer);
                }
                catch (InvalidOperationException) { newerRejected = true; }
                if (roundTrip && newerRejected)
                {
                    GD.Print("[PASS] Check 11: Forecast ledger round-trips and schema-gates.");
                    passed++;
                }
                else GD.PrintErr($"[FAIL] Check 11: Round-trip/schema gate failed (roundTrip={roundTrip}, newerRejected={newerRejected}).");

                // Check 12: host wiring + save section.
                string main = ReadRepoFile("src", "Main.StormForecast.cs");
                string owners = ReadRepoFile("src", "Main.CampaignOwners.cs");
                string registry = ReadRepoFile("Assets", "Ashfall.Core", "Save", "SaveSectionRegistry.cs");
                if (main.Contains("TickStormForecast") && owners.Contains("StormForecastDayOwner")
                    && registry.Contains("storm_forecast") && registry.Contains("storm_forecast_save.json"))
                {
                    GD.Print("[PASS] Check 12: Host ticks forecast decay and registers its save section.");
                    passed++;
                }
                else GD.PrintErr("[FAIL] Check 12: Storm forecast host wiring or save section missing.");
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[EXCEPTION] Exception during storm forecast self-test: {ex}");
            }

            GD.Print($"=== Storm Forecast Readiness Self-Test Result: {passed}/{total} Passed ===");
            return passed == total ? 0 : 1;
        }

        private static string ReadRepoFile(params string[] parts)
        {
            try
            {
                string dir = AppContext.BaseDirectory;
                for (int i = 0; i < 8 && dir != null; i++)
                {
                    string candidate = Path.Combine(dir, Path.Combine(parts));
                    if (File.Exists(candidate)) return File.ReadAllText(candidate);
                    dir = Directory.GetParent(dir)?.FullName ?? string.Empty;
                }
            }
            catch (Exception) { }
            return string.Empty;
        }
    }
}
