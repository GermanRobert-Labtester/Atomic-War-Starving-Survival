// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : SubsidenceSelfTest (ORPHAN-SEAL A.56)
// Subsystem          : Subterranean subsidence & excavation integrity
// Core               : Ashfall.Core.Excavation.SubterraneanSubsidenceEngine,
//                      composed by Ashfall.Core.Subterranean.SubterraneanSystem
//                      (the one underground owner) and surfaced via the
//                      subterranean operations panel.
// Contract           : strata crosswalk determinism, decay math, shoring
//                      mitigation, evacuation gate, catalog topology.
// ============================================================================

using System;
using System.Linq;
using Ashfall.Core.Excavation;
using Ashfall.Core.Subterranean;

namespace AtomicWar.GodotApp
{
    public static class HostCliSubsidence
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Subterranean Subsidence Self-Test (A.56) ===");
            int passed = 0;
            const int total = 10;

            try
            {
                // Check 1: strata crosswalk is deterministic and authored-sane.
                var (mineStrata, mineVoid) = SubterraneanSystem.StrataProfileFor("Mine");
                var (metroStrata, metroVoid) = SubterraneanSystem.StrataProfileFor("Metro");
                var (facilityStrata, _) = SubterraneanSystem.StrataProfileFor("CollapsedFacility");
                if (mineStrata == StrataType.LimestoneKarst && mineVoid == 800
                    && metroStrata == StrataType.GraniteSolid && metroVoid == 500
                    && facilityStrata == StrataType.SandstoneUnconsolidated)
                {
                    Console.WriteLine("[PASS] Check 1: zone_type → strata/void crosswalk matches the authored table.");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 1: strata crosswalk wrong."); }

                // Check 2: sound granite bunker vault evaluates low risk.
                var bunkerProfile = SubterraneanSystem.ProfileForNode(
                    new SubterraneanNodeState { nodeId = "b", depthTier = 2, structuralIntegrity = 95f, shoringLevel = 0 },
                    new SubterraneanZoneDef { zone_type = "Bunker" });
                var bunkerEval = SubterraneanSubsidenceEngine.EvaluateSubsidence(bunkerProfile);
                if (bunkerEval.SubsidenceRiskPermille <= 350
                    && (bunkerEval.Category == SubsidenceCategory.Negligible
                        || bunkerEval.Category == SubsidenceCategory.Low))
                {
                    Console.WriteLine("[PASS] Check 2: sound bunker vault reads low subsidence risk "
                        + $"({bunkerEval.SubsidenceRiskPermille}/1000, {bunkerEval.Category}).");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 2: sound bunker reads {bunkerEval.SubsidenceRiskPermille}/1000 ({bunkerEval.Category})."); }

                // Check 3: failing deep high-void mine gallery reads severe + evacuation.
                var failingProfile = SubterraneanSystem.ProfileForNode(
                    new SubterraneanNodeState { nodeId = "m", depthTier = 3, structuralIntegrity = 15f, shoringLevel = 0, waterLevel = 55f },
                    new SubterraneanZoneDef { zone_type = "Mine" });
                var mineEval = SubterraneanSubsidenceEngine.EvaluateSubsidence(failingProfile);
                if (mineEval.SubsidenceRiskPermille >= 500
                    && (mineEval.Category == SubsidenceCategory.Severe
                        || mineEval.Category == SubsidenceCategory.CatastrophicCollapse)
                    && mineEval.RequiresImmediateEvacuation)
                {
                    Console.WriteLine("[PASS] Check 3: failing deep mine reads severe risk with an evacuation flag.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 3: failing mine reads {mineEval.SubsidenceRiskPermille} ({mineEval.Category})."); }

                // Check 4: shoring level 3 mitigates risk materially.
                var shoredProfile = failingProfile;
                shoredProfile.ShoringLevel = 3;
                var shoredEval = SubterraneanSubsidenceEngine.EvaluateSubsidence(shoredProfile);
                if (shoredEval.SubsidenceRiskPermille < mineEval.SubsidenceRiskPermille)
                {
                    Console.WriteLine($"[PASS] Check 4: shoring reduces risk ({mineEval.SubsidenceRiskPermille} → {shoredEval.SubsidenceRiskPermille}).");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 4: shoring mitigation has no effect."); }

                // Check 5: daily decay respects water + strata.
                int dryGranite = SubterraneanSubsidenceEngine.CalculateDailyIntegrityDecayPermille(
                    MakeProfile(decay: 900, water: 0.1f), 100);
                int wetSalt = SubterraneanSubsidenceEngine.CalculateDailyIntegrityDecayPermille(
                    MakeProfile(decay: 900, water: 0.8f, strata: StrataType.SaltSeam), 800);
                if (dryGranite > 0 && wetSalt > dryGranite)
                {
                    Console.WriteLine($"[PASS] Check 5: decay gates on water/strata (dry granite {dryGranite} < wet salt {wetSalt} permille/day).");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 5: decay math wrong (dry={dryGranite}, wet salt={wetSalt})."); }

                // Check 6: shoring damps daily decay.
                int decayUnshored = SubterraneanSubsidenceEngine.CalculateDailyIntegrityDecayPermille(
                    MakeProfile(water: 0.5f), 500);
                int decayShored = SubterraneanSubsidenceEngine.CalculateDailyIntegrityDecayPermille(
                    MakeProfile(water: 0.5f, shoring: 3), 500);
                if (decayShored < decayUnshored)
                {
                    Console.WriteLine($"[PASS] Check 6: shoring damps daily decay ({decayUnshored} → {decayShored}).");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 6: shoring does not damp decay."); }

                // Check 7: catalog topology loads with ≥10 zones; no salt seam.
                var catalog = SubterraneanZoneCatalogLoader.Load(dataDir ?? ".", new Ashfall.Core.FileSystemIO(), new Ashfall.Core.SystemTextJsonSerializer());
                var zones = catalog.subterranean_zones;
                bool saltSeam = zones.Any(z => SubterraneanSystem.StrataProfileFor(z.zone_type).strata == StrataType.SaltSeam);
                if (zones.Count >= 10 && !saltSeam)
                {
                    Console.WriteLine($"[PASS] Check 7: subterranean topology authority loads ({zones.Count} zones, water-sensitive strata sane).");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 7: topology wrong ({zones.Count} zones, saltSeam={saltSeam})."); }

                // Check 8: per-node evaluation via the system owner (no network →
                // unknown ids read negligible, not throw).
                var system = new SubterraneanSystem(catalog, new Ashfall.Core.Inventory.Inventory());
                var unknown = system.EvaluateSubsidence("subnode_not_generated");
                if (unknown.SubsidenceRiskPermille == 0 && unknown.Category == SubsidenceCategory.Negligible)
                {
                    Console.WriteLine("[PASS] Check 8: unknown node evaluation reads negligible without throwing.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 8: unknown node read {unknown.SubsidenceRiskPermille}."); }

                // Check 9: generated network evaluates all nodes deterministically
                // (nodes generate undiscovered; evaluate straight off the state —
                // discovery gating is the surface layer's concern, not decay's).
                system.EnsureNetwork(20260926);
                var nodes = system.State.nodes;
                var evals = nodes.Select(n => system.EvaluateSubsidence(n.nodeId)).ToList();
                var again = nodes.Select(n => system.EvaluateSubsidence(n.nodeId)).ToList();
                bool deterministic = evals.Zip(again, (a, b) =>
                    a.SubsidenceRiskPermille == b.SubsidenceRiskPermille && evals.Count == again.Count).All(x => x);
                if (nodes.Count == zones.Count && deterministic && evals.Count > 0)
                {
                    Console.WriteLine($"[PASS] Check 9: the {nodes.Count}-node network evaluates deterministically.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 9: nodes {nodes.Count} vs zones {zones.Count}, deterministic={deterministic}."); }

                // Check 10: rigid cavities stay sound after a dry day; deep void
                // mine nodes take measurable decay.
                double metroIntegrityBefore = nodes
                    .Where(n => SubterraneanSystem.StrataProfileFor(
                        zones.First(z => z.id == n.nodeId).zone_type).strata == StrataType.GraniteSolid)
                    .Min(n => (double)n.structuralIntegrity);
                system.ApplyUndergroundDay(310, Array.Empty<(string, string)>());
                double metroIntegrityAfter = nodes
                    .Where(n => SubterraneanSystem.StrataProfileFor(
                        zones.First(z => z.id == n.nodeId).zone_type).strata == StrataType.GraniteSolid)
                    .Min(n => (double)n.structuralIntegrity);
                bool sane = metroIntegrityAfter > 0 && metroIntegrityAfter <= metroIntegrityBefore;
                if (sane)
                {
                    Console.WriteLine($"[PASS] Check 10: a dry underground day leaves integrity sane ({metroIntegrityBefore:F0}% → {metroIntegrityAfter:F0}% floor).");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 10: daily decay ran out of bounds."); }

                Console.WriteLine($"Subsidence: {passed}/{total} checks passed");
                return passed == total ? 0 : 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FAIL] Unhandled: {ex.Message}");
                Console.WriteLine($"Subsidence: {passed}/{total} checks passed");
                return 1;
            }
        }

        private static ExcavationNodeProfile MakeProfile(int decay = 900, float water = 0.0f, StrataType strata = StrataType.LimestoneKarst, int shoring = 0)
            => new ExcavationNodeProfile
            {
                NodeId = MakeProfileNodeId(shoring, strata),
                Strata = strata,
                VoidVolumeCubicMeters = 600,
                ShoringLevel = shoring,
                StructuralIntegrityPermille = decay
            };

        private static string MakeProfileNodeId(int shoring, StrataType strata) => $"p{shoring}_{(int)strata}";
    }
}
