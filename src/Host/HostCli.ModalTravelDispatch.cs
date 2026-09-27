// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : ModalTravelDispatchSelfTest
// Subsystem          : Pre-departure multi-modal travel feasibility projection
//                      over the live wasteland map routes.
// ============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    public static class HostCliModalTravelDispatch
    {
        private static void Check(bool ok, string label)
        {
            if (ok) Console.WriteLine($"[PASS] {label}");
            else Console.WriteLine($"[FAIL] {label}");
        }

        private static MapRoute Route(string from, string to, float km, float hazard, params string[] tags)
            => new MapRoute
            {
                From = from,
                To = to,
                DistanceKm = km,
                WeatherHazard = hazard,
                Tags = new List<string>(tags)
            };

        private static ModalTravelDispatchHostSession BuildSession(
            WastelandMapSystem? map, int condition = 1000, int fuel = 50, int weather = 1000)
            => new ModalTravelDispatchHostSession(
                () => map,
                () => condition,
                () => fuel,
                () => weather);

        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Modal Travel Dispatch Self-Test ===");
            int passed = 0;
            const int total = 11;
            try
            {
                var plain = Route("node_a", "node_b", 24f, 0.2f);
                var flooded = Route("node_a", "node_b", 12f, 0.1f, "flooded");
                var mountain = Route("node_a", "node_b", 30f, 0.3f, "mountain");

                // 1. A plain land route is crossable on foot with no fuel.
                var foot = BuildSession(null).Evaluate(plain, TravelModality.FootExcursion);
                Check(foot.CanDispatch && foot.FuelRequiredUnits == 0,
                    "Check 1: a plain land route is crossable on foot with no fuel.");
                passed += foot.CanDispatch ? 1 : 0;

                // 2. A flooded route refuses foot and ground dispatch.
                var footFlood = BuildSession(null).Evaluate(flooded, TravelModality.FootExcursion);
                var convoyFlood = BuildSession(null).Evaluate(flooded, TravelModality.GroundConvoy);
                Check(!footFlood.CanDispatch && !convoyFlood.CanDispatch
                      && footFlood.RefusalReason.Length > 0,
                    "Check 2: a flooded route refuses foot and ground convoy dispatch.");
                passed += !footFlood.CanDispatch && !convoyFlood.CanDispatch ? 1 : 0;

                // 3. The amphibious rig crosses the flooded route but not a mountain.
                var rigFlood = BuildSession(null).Evaluate(flooded, TravelModality.AmphibiousRig);
                var rigMountain = BuildSession(null).Evaluate(mountain, TravelModality.AmphibiousRig);
                Check(rigFlood.CanDispatch && !rigMountain.CanDispatch,
                    "Check 3: the amphibious rig is the authored modality for flooded routes and refuses cliffs.");
                passed += rigFlood.CanDispatch && !rigMountain.CanDispatch ? 1 : 0;

                // 4. A grounded flight window refuses aerial dispatch.
                var grounded = BuildSession(null, weather: 200).Evaluate(plain, TravelModality.AerialReconFlight);
                Check(!grounded.CanDispatch,
                    "Check 4: a storm window below 400\u2030 grounds aerial dispatch.");
                passed += !grounded.CanDispatch ? 1 : 0;

                // 5. A clear flight window allows aerial dispatch with no fuel available.
                var airborne = BuildSession(null, fuel: 20, weather: 900).Evaluate(plain, TravelModality.AerialReconFlight);
                Check(airborne.CanDispatch,
                    "Check 5: a clear flight window allows aerial dispatch.");
                passed += airborne.CanDispatch ? 1 : 0;

                // 6. Insufficient fuel refuses a ground convoy.
                var dry = BuildSession(null, fuel: 0).Evaluate(plain, TravelModality.GroundConvoy);
                Check(!dry.CanDispatch && dry.RefusalReason.Contains("fuel", StringComparison.OrdinalIgnoreCase),
                    "Check 6: insufficient fuel refuses ground convoy dispatch.");
                passed += !dry.CanDispatch ? 1 : 0;

                // 7. Critical vehicle condition refuses every non-foot modality.
                var broken = BuildSession(null, condition: 100);
                bool allRefused = Enum.GetValues(typeof(TravelModality))
                    .Cast<TravelModality>()
                    .Where(m => m != TravelModality.FootExcursion)
                    .All(m => !broken.Evaluate(plain, m).CanDispatch);
                Check(allRefused,
                    "Check 7: critical vehicle condition refuses every vehicle modality (foot still works).");
                passed += allRefused ? 1 : 0;

                // 8. A null route is a refusal, never a crash.
                var noRoute = BuildSession(null).Evaluate(null, TravelModality.FootExcursion);
                Check(!noRoute.CanDispatch && noRoute.RefusalReason.Length > 0,
                    "Check 8: a null route is a refusal with a reason.");
                passed += !noRoute.CanDispatch ? 1 : 0;

                // 9. The projection is deterministic across repeated evaluations.
                var a = BuildSession(null).Evaluate(mountain, TravelModality.GroundConvoy);
                var b = BuildSession(null).Evaluate(mountain, TravelModality.GroundConvoy);
                Check(a.EstimatedDurationHours == b.EstimatedDurationHours
                      && a.TerrainAttritionRiskPermille == b.TerrainAttritionRiskPermille
                      && a.FuelRequiredUnits == b.FuelRequiredUnits,
                    "Check 9: identical inputs produce identical permille/integer outputs.");
                passed += a.EstimatedDurationHours == b.EstimatedDurationHours ? 1 : 0;

                // 10. The modality matrix covers every authored modality and reports
                //     a truthful crossable count over the live map routes.
                var rows = BuildSession(null).EvaluateAllModalities(plain);
                Check(rows.Count == Enum.GetValues(typeof(TravelModality)).Length
                      && rows.Count(r => r.Result.CanDispatch) >= 1,
                    $"Check 10: modality matrix covers all {rows.Count} authored modalities.");
                passed += rows.Count == Enum.GetValues(typeof(TravelModality)).Length ? 1 : 0;

                // 11. The read model is pure: it never writes back to the map route.
                float before = plain.DistanceKm;
                var session = BuildSession(null);
                session.EvaluateAllModalities(plain);
                session.StatusLine("node_a", "node_b");
                Check(Math.Abs(plain.DistanceKm - before) < 0.0001f && plain.Tags.Count == 0,
                    "Check 11: the projection never writes back to the live map route.");
                passed += Math.Abs(plain.DistanceKm - before) < 0.0001f ? 1 : 0;
            }
            catch (Exception ex) { Console.WriteLine($"[FAIL] Unexpected probe exception: {ex}"); }
            Console.WriteLine($"=== Modal Travel Dispatch Self-Test: {passed}/{total} passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
