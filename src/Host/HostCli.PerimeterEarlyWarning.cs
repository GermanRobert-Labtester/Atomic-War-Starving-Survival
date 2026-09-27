// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : PerimeterEarlyWarningSelfTest
// Subsystem          : Perimeter Radar (Early Warning)
// ============================================================================

using System;
using Ashfall.Core.Defense;

namespace AtomicWar.GodotApp
{
    public static class HostCliPerimeterEarlyWarning
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Perimeter Radar Self-Test ===");
            int passed = 0;
            const int total = 7;

            try
            {
                var session = PerimeterEarlyWarningHostSession.Create();

                if (session.Engine.Mode == RadarOperationalMode.ActiveScan
                    && session.Engine.CalibrationPermille == PerimeterEarlyWarningEngine.DefaultCalibrationPermille)
                {
                    Console.WriteLine("[PASS] Check 1: Radar starts in ActiveScan at default calibration.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 1: Initial mode/calibration mismatch ({session.Engine.Mode}/{session.Engine.CalibrationPermille}).");
                }

                session.SetMode(RadarOperationalMode.Off);
                var off = session.ProcessScanSweep("gate", 100, true, false, 1, 500);
                if (off == null && session.Engine.PowerDrawWatts == 0)
                {
                    Console.WriteLine("[PASS] Check 2: Radar Off draws no power and reports no contacts.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 2: Radar Off still produced a contact or drew power.");
                }

                session.SetMode(RadarOperationalMode.ActiveScan);
                session.CalibrateSensors(200);
                if (session.Engine.CalibrationPermille == 700)
                {
                    Console.WriteLine("[PASS] Check 3: Sensor calibration advanced to 700 per mille.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 3: Calibration mismatch ({session.Engine.CalibrationPermille}).");
                }

                var hostile = session.ProcessScanSweep("gate", 400, true, false, 2, 0);
                if (hostile != null && hostile.Classification == RadarTargetClassification.HostileIncursion)
                {
                    Console.WriteLine("[PASS] Check 4: Hostile contact classified as incursion.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 4: Hostile classification mismatch ({hostile?.Classification}).");
                }

                var beyondRange = session.ProcessScanSweep("gate", 999999, true, false, 3, 0);
                if (beyondRange == null)
                {
                    Console.WriteLine("[PASS] Check 5: Contacts beyond radar range are ignored.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 5: Out-of-range contact reported.");
                }

                var noise = session.ProcessScanSweep("gate", 300, false, false, 4, 0);
                if (noise != null && noise.Classification != RadarTargetClassification.HostileIncursion)
                {
                    Console.WriteLine($"[PASS] Check 6: Non-hostile contact classified as {noise.Classification}.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 6: Non-hostile classification mismatch ({noise?.Classification}).");
                }

                bool saved = session.TrySave();
                var reloaded = PerimeterEarlyWarningHostSession.Create();
                bool loaded = reloaded.TryLoad();
                if (saved && loaded && reloaded.Engine.CalibrationPermille == 700
                    && reloaded.Engine.Mode == RadarOperationalMode.ActiveScan
                    && PerimeterEarlyWarningSaveStore.SectionName.Equals("perimeter_early_warning", StringComparison.Ordinal)
                    && PerimeterEarlyWarningSaveStore.FileName.Equals("perimeter_early_warning_save.json", StringComparison.Ordinal))
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

            Console.WriteLine($"=== Perimeter Radar Self-Test: {passed}/{total} passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
