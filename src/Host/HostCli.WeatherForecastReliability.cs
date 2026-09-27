// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : WeatherForecastReliabilitySelfTest
// Subsystem          : Received-forecast reliability grading
// ============================================================================

using System;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    public static class HostCliWeatherForecastReliability
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Forecast Reliability Self-Test ===");
            int passed = 0;
            const int total = 6;

            try
            {
                var session = WeatherForecastReliabilityHostSession.Create();

                if (session.ForecastCount == 0 && session.CurrentDay == 0)
                {
                    Console.WriteLine("[PASS] Check 1: Forecast ledger starts empty.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 1: Ledger was not empty.");
                }

                // Close station, short horizon, calibrated, clean air -> High confidence.
                var close = session.ReceiveForecast("f_close", 5, 1, 50, 950, 1);
                if (close.grade == ForecastConfidenceGrade.High && close.isReliableForDispatch)
                {
                    Console.WriteLine($"[PASS] Check 2: Close calibrated forecast is High confidence ({close.reliabilityScorePermille} permille).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 2: Grade {close.grade}, reliable {close.isReliableForDispatch}.");
                }

                // Long horizon + heavy interference + far station + poor calibration -> Unusable.
                var poor = session.ReceiveForecast("f_poor", 400, 12, 900, 200, 2);
                if (poor.grade == ForecastConfidenceGrade.Unusable && !poor.isReliableForDispatch)
                {
                    Console.WriteLine("[PASS] Check 3: Distant noisy forecast is Unusable and unsafe for dispatch.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 3: Grade {poor.grade}, reliable {poor.isReliableForDispatch}.");
                }

                // Longer lead time must never increase the reliability score.
                var d1 = WeatherForecastReliabilityEngine.EvaluateReliability(10, 1, 100, 900);
                var d7 = WeatherForecastReliabilityEngine.EvaluateReliability(10, 7, 100, 900);
                if (d7.ReliabilityScorePermille < d1.ReliabilityScorePermille
                    && d7.LeadTimeDays > d1.LeadTimeDays)
                {
                    Console.WriteLine($"[PASS] Check 4: Lead time degrades reliability ({d1.ReliabilityScorePermille} -> {d7.ReliabilityScorePermille}).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 4: {d1.ReliabilityScorePermille} -> {d7.ReliabilityScorePermille}.");
                }

                // Unusable forecasts must never be dispatch-safe regardless of lead time.
                var far = session.ReceiveForecast("f_mid", 250, 3, 400, 500, 3);
                if (!far.isReliableForDispatch && far.grade != ForecastConfidenceGrade.High)
                {
                    Console.WriteLine($"[PASS] Check 5: Mid-range forecast is not High confidence ({far.grade}).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 5: {far.grade} / reliable={far.isReliableForDispatch}.");
                }

                if (session.DispatchableForecastCount == 1 && session.Forecasts.Count == 3
                    && session.Forecasts[0].isReliableForDispatch
                    && !session.Forecasts[2].isReliableForDispatch)
                {
                    Console.WriteLine($"[PASS] Check 6: Dispatchable count derived from the ledger ({session.DispatchableForecastCount}/3).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 6: dispatchable={session.DispatchableForecastCount}, n={session.Forecasts.Count}.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FAIL] Self-test crashed: {ex.Message}");
            }

            Console.WriteLine($"=== Forecast Reliability Self-Test: {passed}/{total} passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
