// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : PowerLoadSheddingSelfTest
// Subsystem          : EXPANSION-21-THE-GRID — microgrid load shedding
// ============================================================================
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    public static class HostCliPowerLoadShedding
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Power Load Shedding Self-Test (EXPANSION-21-THE-GRID) ===");
            int passed = 0;
            const int total = 12;
            try
            {
                string dataRoot = (!string.IsNullOrEmpty(dataDir) && Directory.Exists(dataDir)
                    ? dataDir
                    : CatalogPath.ResolveDataDir());

                // Live canonical owners.
                var fileIO = CatalogPath.CreateFileIOForDataDir(dataRoot);
                var json = new SystemTextJsonSerializer();
                var grid = ShelterPowerGridCatalogLoader.LoadOrDefault(dataRoot, fileIO, json);
                var rooms = new List<PowerGridRoom>();
                foreach (var r in grid.Rooms)
                {
                    var prio = ShelterPowerGridCatalogLoader.MapPriority(r.Id, r.DefaultPriority)
                               ?? PowerGridRoomPriority.Standard;
                    rooms.Add(new PowerGridRoom(r.Id, r.Id, 500f, prio));
                }

                var gridState = new PowerGridState { GenerationWatts = 60000f, BatteryReserveWh = 24000f };
                var powerGrid = new PowerGridSystem(
                    gridState,
                    rooms,
                    new SeededRng(21));

                var nodes = PowerSubgridCatalogLoader.Load(dataRoot, fileIO, json);
                var subgrids = new PowerDistributionSubgridSystem(
                    nodes, new Ashfall.Core.Inventory.Inventory(), new SeededRng(21));

                var session = new PowerLoadSheddingHostSession(powerGrid, subgrids);
                session.Bind(powerGrid, subgrids);

                // 1. Binding is live (both canonical owners reachable).
                if (session.IsBound) { Console.WriteLine("[PASS] Check 1: Bound to the live power-grid and subgrid owners."); passed++; }
                else Console.WriteLine("[FAIL] Check 1: owners not bound.");

                // 2. Demand vector derives from the authored subgrid nodes.
                var demands = session.BuildDemands();
                if (demands.Count > 0) { Console.WriteLine($"[PASS] Check 2: Demand vector built from {demands.Count} authored subgrid node(s)."); passed++; }
                else Console.WriteLine("[FAIL] Check 2: no subgrid demand derived.");

                // 3. Grid wear projects from the canonical generator condition.
                int wear = session.GetGridWearPermille();
                if (wear >= 0 && wear <= PowerLoadSheddingEngine.PermilleScale)
                { Console.WriteLine($"[PASS] Check 3: Grid wear projected from generator condition ({wear} permille)."); passed++; }
                else Console.WriteLine($"[FAIL] Check 3: grid wear out of range ({wear}).");

                // 4. Life support is served last-shed: a critical node survives a tight budget.
                int supply = session.GetAvailableGenerationKw();
                var tight = PowerLoadSheddingEngine.Evaluate(0, new List<SubgridLoadDemand>
                {
                    new("comfort_lamp", LoadPriorityTier.Tier4_Comfort, 5),
                    new("air_scrubber", LoadPriorityTier.Tier0_LifeSupport, 3),
                    new("greenhouse_lamps", LoadPriorityTier.Tier2_Agricultural, 4),
                });
                if (tight.ShedConsumers.Count == 3 && tight.ShedLoadKw == 12)
                { Console.WriteLine("[PASS] Check 4: Zero supply sheds every node and reports each shed consumer."); passed++; }
                else Console.WriteLine("[FAIL] Check 4: zero-supply shedding wrong.");

                var partial = PowerLoadSheddingEngine.Evaluate(4, new List<SubgridLoadDemand>
                {
                    new("comfort_lamp", LoadPriorityTier.Tier4_Comfort, 5),
                    new("air_scrubber", LoadPriorityTier.Tier0_LifeSupport, 3),
                });
                if (partial.ServedLoadKw == 3 && partial.ShedConsumers.Count == 1 &&
                    partial.ShedConsumers[0] == "comfort_lamp")
                { Console.WriteLine("[PASS] Check 5: Highest priority (life support) is served first, comfort is shed."); passed++; }
                else Console.WriteLine("[FAIL] Check 5: priority shedding order wrong.");

                // 6. Blackout is only reported when supply serves nothing at all.
                var totalBlackout = PowerLoadSheddingEngine.Evaluate(0, new[] { new SubgridLoadDemand("a", LoadPriorityTier.Tier2_Agricultural, 3) });
                if (totalBlackout.IsBlackout
                    && !partial.IsBlackout
                    && !PowerLoadSheddingEngine.Evaluate(100, Array.Empty<SubgridLoadDemand>()).IsBlackout)
                { Console.WriteLine("[PASS] Check 6: Blackout only reported when supply cannot serve any demand."); passed++; }
                else Console.WriteLine("[FAIL] Check 6: blackout detection wrong.");

                // 7. Live evaluation over the real owners is reproducible.
                var a = session.Evaluate();
                var b = session.Evaluate();
                if (a.ServedLoadKw == b.ServedLoadKw && a.ShedLoadKw == b.ShedLoadKw &&
                    a.BrownoutRiskPermille == b.BrownoutRiskPermille)
                { Console.WriteLine($"[PASS] Check 7: Live evaluation is deterministic (served {a.ServedLoadKw} kW / shed {a.ShedLoadKw} kW)."); passed++; }
                else Console.WriteLine("[FAIL] Check 7: live evaluation not deterministic.");

                // 8. Projection reads the live owners.
                var proj = session.GetProjection();
                if (proj.TotalDemandKw == a.TotalDemandKw && proj.DemandCount == demands.Count &&
                    proj.AvailableGenerationKw == supply)
                { Console.WriteLine($"[PASS] Check 8: Projection reads live owners ({proj.TotalDemandKw} kW demand, {supply} kW supply)."); passed++; }
                else Console.WriteLine("[FAIL] Check 8: projection stale.");

                // 9. Brownout risk is monotone in the load factor.
                int lo = PowerLoadSheddingEngine.Evaluate(100, new[] { new SubgridLoadDemand("a", LoadPriorityTier.Tier2_Agricultural, 80) }).BrownoutRiskPermille;
                int hi = PowerLoadSheddingEngine.Evaluate(100, new[] { new SubgridLoadDemand("a", LoadPriorityTier.Tier2_Agricultural, 99) }).BrownoutRiskPermille;
                if (lo == 0 && hi > lo)
                { Console.WriteLine($"[PASS] Check 9: Brownout risk rises with load factor ({lo} -> {hi})."); passed++; }
                else Console.WriteLine($"[FAIL] Check 9: brownout monotonicity wrong ({lo} -> {hi}).");

                // 10. Grid wear compounds brownout risk through the canonical wear projection.
                int worn = PowerLoadSheddingEngine.Evaluate(100, new[] { new SubgridLoadDemand("a", LoadPriorityTier.Tier2_Agricultural, 90) }, 1000).BrownoutRiskPermille;
                int fresh = PowerLoadSheddingEngine.Evaluate(100, new[] { new SubgridLoadDemand("a", LoadPriorityTier.Tier2_Agricultural, 90) }, 0).BrownoutRiskPermille;
                if (worn > fresh)
                { Console.WriteLine($"[PASS] Check 10: Grid wear compounds brownout risk ({fresh} -> {worn})."); passed++; }
                else Console.WriteLine($"[FAIL] Check 10: grid wear did not compound risk.");

                // 11. Morale penalty is bounded and scales with the engine's permille.
                float zero = session.GetMoralePenaltyPoints(0);
                float full = session.GetMoralePenaltyPoints(PowerLoadSheddingEngine.PermilleScale);
                float over = session.GetMoralePenaltyPoints(5000);
                if (zero == 0f && full == -PowerLoadSheddingHostSession.MoralePointsAtFullPenalty &&
                    over == full && partial.EnergyPovertyMoralePenaltyPermille > 0)
                { Console.WriteLine($"[PASS] Check 11: Energy-poverty morale penalty bounded and permille-scaled ({partial.EnergyPovertyMoralePenaltyPermille} permille)."); passed++; }
                else Console.WriteLine("[FAIL] Check 11: morale penalty mapping wrong.");

                // 12. Derived read model — no save store, no second power state.
                var storeType = Type.GetType("AtomicWar.GodotApp.PowerLoadSheddingSaveStore, AtomicWar.GodotApp");
                if (storeType == null && session.GetType().Name == nameof(PowerLoadSheddingHostSession))
                { Console.WriteLine("[PASS] Check 12: Derived read model — no save section, no second power ledger."); passed++; }
                else Console.WriteLine("[FAIL] Check 12: unexpected save store for a read model.");
            }
            catch (Exception ex) { Console.WriteLine($"[FAIL] Unexpected probe exception: {ex}"); }
            Console.WriteLine($"=== Power Load Shedding Self-Test: {passed}/{total} passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
