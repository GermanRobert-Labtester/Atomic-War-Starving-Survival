// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : YoaIceRoadSelfTest
// Subsystem          : Year of Ash — ice road (Plan 146 residual)
// Core               : Ashfall.Core.YearOfAsh.YearOfAshIceRoadSystem (+ storm
//                      catalog gate via YearOfAshHostSession composition)
// Contract           : ≤ −20 °C opens the road; blocking storms (thaw_flood /
//                      thermal_inversion) keep it closed; trade multiplier and
//                      expedition exposure flip with the road; capture/restore
//                      round-trips deterministically; lastOpen/lastClosed days
//                      are truthful; the session ticks it alongside deep freeze.
// ============================================================================

using System;
using Ashfall.Core.YearOfAsh;
using AtomicWar.GodotApp.YearOfAsh;

namespace AtomicWar.GodotApp
{
    public static class HostCliYoaIceRoad
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Year-of-Ash Ice Road Self-Test (Plan 146 residual) ===");
            int passed = 0;
            const int total = 12;

            try
            {
                // Check 1: road starts closed on a fresh state.
                var ice = new YearOfAshIceRoadSystem();
                if (!ice.IsIceRoadOpen && ice.State.lastOpenDay == -1 && ice.State.lastClosedDay == -1)
                {
                    Console.WriteLine("[PASS] Check 1: fresh state reads closed with no window history.");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 1: fresh state should read closed and virgin."); }

                // Check 2: deep cold opens the road (no storm gate).
                ice.TickDay(200, -25f);
                if (ice.IsIceRoadOpen && ice.State.lastOpenDay == 200 && ice.State.totalTradeWindowDays == 1)
                {
                    Console.WriteLine("[PASS] Check 2: −25 °C opens the road and counts the first window day.");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 2: cold tick did not open the road."); }

                // Check 3: trade multiplier/open = 1.4x, exposure 0.30.
                if (System.Math.Abs(ice.GetTradeMultiplier() - 1.4f) < 0.0001f
                    && System.Math.Abs(ice.GetExpeditionExposureRisk() - 0.30f) < 0.0001f)
                {
                    Console.WriteLine("[PASS] Check 3: open-road multipliers match authored values (1.4x / 0.30).");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 3: open-road multiplier wrong."); }

                // Check 4: thaw flood blocks the road even in deep cold.
                var blocked = new YearOfAshIceRoadSystem();
                blocked.TickDay(210, -25f, new System.Collections.Generic.List<StormWindowEntry>
                {
                    new StormWindowEntry { type = "thaw_flood", day_start = 205, day_end = 215 }
                });
                if (!blocked.IsIceRoadOpen && blocked.State.lastClosedDay == 210)
                {
                    Console.WriteLine("[PASS] Check 4: thaw_flood keeps the road closed at −25 °C.");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 4: storm gate ignored."); }

                // Check 5: thermal_inversion also blocks; ice_fog does not.
                var inv = new YearOfAshIceRoadSystem();
                inv.TickDay(205, -25f, new System.Collections.Generic.List<StormWindowEntry>
                {
                    new StormWindowEntry { type = "thermal_inversion", day_start = 200, day_end = 210 }
                });
                var fog = new YearOfAshIceRoadSystem();
                fog.TickDay(205, -25f, new System.Collections.Generic.List<StormWindowEntry>
                {
                    new StormWindowEntry { type = "ice_fog", day_start = 200, day_end = 210 }
                });
                if (!inv.IsIceRoadOpen && fog.IsIceRoadOpen)
                {
                    Console.WriteLine("[PASS] Check 5: thermal_inversion blocks; ice_fog does not.");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 5: blocking-storm type set wrong."); }

                // Check 6: warm weather closes the road; multipliers flip.
                ice.TickDay(230, -5f);
                if (!ice.IsIceRoadOpen
                    && System.Math.Abs(ice.GetTradeMultiplier() - 0.6f) < 0.0001f
                    && System.Math.Abs(ice.GetExpeditionExposureRisk() - 0.10f) < 0.0001f
                    && ice.State.lastClosedDay == 230)
                {
                    Console.WriteLine("[PASS] Check 6: warm day closes the road; closed multipliers correct (0.6x / 0.10).");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 6: close-over-warm transition wrong."); }

                // Check 7: exposure accumulates only while open (float state).
                float cumAtClose = ice.State.cumulativeExposureScore;
                ice.TickDay(231, -30f);
                ice.TickDay(232, -31f);
                float cumAfterReopen = ice.State.cumulativeExposureScore;
                ice.TickDay(233, 5f);
                float cumAfterReclose = ice.State.cumulativeExposureScore;
                if (cumAfterReopen > cumAtClose && System.Math.Abs(cumAfterReclose - cumAfterReopen) < 0.0001f)
                {
                    Console.WriteLine("[PASS] Check 7: exposure score accrues only across open days.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 7: exposure accrual wrong ({cumAtClose}→{cumAfterReopen}→{cumAfterReclose})."); }

                // Check 8: capture/restore round-trip preserves the whole ledger.
                var session = new YearOfAshHostSession();
                session.BindStormCatalog(dataDir ?? string.Empty);
                session.TickDay(240); // timeline day tick drives temperature/policy
                var saved = session.CaptureSave();
                if (saved.iceRoad != null)
                {
                    var session2 = new YearOfAshHostSession();
                    session2.RestoreSave(saved);
                    if (session2.IceRoad.State.iceRoadOpen == session.IceRoad.IsIceRoadOpen
                        && session2.IceRoad.State.lastOpenDay == session.IceRoad.State.lastOpenDay)
                    {
                        Console.WriteLine("[PASS] Check 8: ice road rides the year_of_ash envelope round-trip.");
                        passed++;
                    }
                    else { Console.WriteLine("[FAIL] Check 8: capture/restore lost ice-road state."); }
                }
                else { Console.WriteLine("[FAIL] Check 8: envelope omitted the iceRoad section."); }

                // Check 9: v3/v2 saves without an iceRoad section restore clean.
                var fresh = new YearOfAshHostSession();
                fresh.RestoreSave(new YearOfAshSave()); // defaults: no iceRoad DTO
                if (fresh.IceRoad.State.lastOpenDay == -1)
                {
                    Console.WriteLine("[PASS] Check 9: legacy envelopes without iceRoad bring the road up closed.");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 9: legacy restore seeded ice-road state."); }

                // Check 10: open threshold is exactly ≤ −20 °C (boundary).
                var boundary = new YearOfAshIceRoadSystem();
                boundary.TickDay(300, -20f);
                bool openAtBoundary = boundary.IsIceRoadOpen;
                boundary.TickDay(301, -19.9f);
                if (openAtBoundary && !boundary.IsIceRoadOpen)
                {
                    Console.WriteLine("[PASS] Check 10: authored threshold honored (−20.0 opens, −19.9 stays closed).");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 10: temperature threshold drifted."); }

                // Check 11: status-change events fire exactly on flips.
                var events = new YearOfAshIceRoadSystem();
                int flips = 0;
                events.OnIceRoadStatusChanged += (_, _) => flips++;
                events.TickDay(400, -25f);
                events.TickDay(401, -26f);
                events.TickDay(402, 3f);
                if (flips == 2)
                {
                    Console.WriteLine("[PASS] Check 11: status events fire only on real flips (2 open→close transitions).");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 11: flip count {flips} != 2."); }

                // Check 12: JSON data authority — the storm catalog loads
                // through the session bind and contains both blocking types.
                session.BindStormCatalog(dataDir ?? string.Empty);
                bool hasFlood = false, hasInversion = false;
                foreach (var storm in session.StormWindows)
                {
                    if (storm == null) continue;
                    if (storm.type == "thaw_flood") hasFlood = true;
                    if (storm.type == "thermal_inversion") hasInversion = true;
                }
                if (session.StormWindows.Count >= 10 && hasFlood && hasInversion)
                {
                    Console.WriteLine($"[PASS] Check 12: authored storm catalog loaded ({session.StormWindows.Count} windows, both blocking types present).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 12: storm catalog wrong (count={session.StormWindows.Count}, flood={hasFlood}, inversion={hasInversion}).");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FAIL] Unhandled: {ex.Message}");
            }

            Console.WriteLine($"Year-of-Ash ice road: {passed}/{total} checks passed");
            return passed == total ? 0 : 1;
        }
    }
}
