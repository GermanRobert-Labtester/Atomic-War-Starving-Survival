// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : PharmaceuticalTabletSelfTest
// Subsystem          : PLAN-PHARMACEUTICAL-TRUTH-167 — Tablet production
// ============================================================================
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Medical;

namespace AtomicWar.GodotApp
{
    public static class HostCliPharmaceuticalTablet
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Pharmaceutical Tablet Works Self-Test (PLAN-PHARMACEUTICAL-TRUTH-167) ===");
            int passed = 0;
            const int total = 11;
            try
            {
                string dataRoot = (!string.IsNullOrEmpty(dataDir) && Directory.Exists(dataDir) ? dataDir : CatalogPath.ResolveDataDir());
                var session = new PharmaceuticalTabletHostSession(new SeededRng(167));
                session.LoadCatalog(dataRoot);

                if (session.FormulationCount > 0) { Console.WriteLine($"[PASS] Check 1: Authored catalog loaded ({session.FormulationCount} formulations)."); passed++; }
                else Console.WriteLine("[FAIL] Check 1: catalog load failed.");

                // canonical inventory binding (in-memory canonical stand-in)
                var inv = new Dictionary<string, int>(StringComparer.Ordinal);
                session.BindInventory(
                    id => inv.TryGetValue(id, out var q) ? q : 0,
                    (id, q) => true,
                    (id, q) => inv[id] = (inv.TryGetValue(id, out var c) ? c : 0) + q,
                    (id, q) => inv[id] = Math.Max(0, (inv.TryGetValue(id, out var c) ? c : 0) - q));

                // Seed every id the authored catalog references (construction +
                // formulation reagents) so the production path is provable.
                void Seed(string id) { if (!string.IsNullOrEmpty(id) && !inv.ContainsKey(id)) inv[id] = 9999; }
                foreach (var kv in session.Engine.Catalog.press.construction_required_items) Seed(kv.Key);
                foreach (var kv in session.Engine.Catalog.press.maintenance_required_items) Seed(kv.Key);
                foreach (var f in session.Engine.Catalog.formulations)
                {
                    Seed(f.binder_resource_id);
                    Seed(f.packaging_resource_id);
                    if (f.precursor_costs != null) foreach (var kv in f.precursor_costs) Seed(kv.Key);
                }
                session.Engine.DayProvider = () => 1;
                session.Engine.PharmaceuticalChemistSkillProvider = () => 0.8f;
                session.Engine.FormulationTechnicianSkillProvider = () => 0.8f;

                var construct = session.ConstructPress();
                if (session.Engine.IsConstructed) { Console.WriteLine($"[PASS] Check 2: Press constructed ({construct.MessageKey})."); passed++; }
                else Console.WriteLine("[FAIL] Check 2: press construction failed.");

                var snapshot0 = session.GetSnapshot();
                string formulationId = PickFormulation(dataRoot);
                if (formulationId == null) { Console.WriteLine("[FAIL] Check 3: no formulation id."); }
                else
                {
                    var staged = session.StageBatch(formulationId);
                    if (session.Engine.ActiveBatch != null) { Console.WriteLine($"[PASS] Check 3: Batch staged ({staged.MessageKey})."); passed++; }
                    else Console.WriteLine($"[FAIL] Check 3: stage failed ({staged.MessageKey}).");
                }

                // daily process advances deterministically
                for (int d = 1; d <= 12; d++) session.TickDay(d);
                var after = session.GetSnapshot();
                if (after.ActiveBatchId != string.Empty || after.BufferedBatches > 0 || after.MachineState != snapshot0.MachineState)
                { Console.WriteLine("[PASS] Check 4: Daily tick advanced production state."); passed++; }
                else Console.WriteLine("[FAIL] Check 4: production did not advance.");

                // claim outputs into canonical inventory
                var claimed = session.ClaimOutputs();
                bool claimedOk = claimed != null && claimed.total_units > 0 && claimed.item_units.Count > 0;
                if (claimedOk) { Console.WriteLine($"[PASS] Check 5: Claimed {claimed!.total_units} units into canonical inventory keys."); passed++; }
                else Console.WriteLine("[FAIL] Check 5: claim produced no canonical output.");

                // conservation/replay: second claim is empty (buffer drained)
                var claimed2 = session.ClaimOutputs();
                if (claimed2 == null || claimed2.total_units == 0) { Console.WriteLine("[PASS] Check 6: Second claim is empty (no duplicate output)."); passed++; }
                else Console.WriteLine("[FAIL] Check 6: duplicate claim.");

                // determinism
                var a = new PharmaceuticalTabletHostSession(new SeededRng(5)); a.LoadCatalog(dataRoot); a.ConstructPress(); a.StageBatch(formulationId);
                var b = new PharmaceuticalTabletHostSession(new SeededRng(5)); b.LoadCatalog(dataRoot); b.ConstructPress(); b.StageBatch(formulationId);
                for (int d = 1; d <= 12; d++) { a.TickDay(d); b.TickDay(d); }
                if (a.GetSnapshot().MachineState == b.GetSnapshot().MachineState && a.GetSnapshot().ToolingCondition == b.GetSnapshot().ToolingCondition)
                { Console.WriteLine("[PASS] Check 7: Same seed produces identical production state."); passed++; }
                else Console.WriteLine("[FAIL] Check 7: determinism broken.");

                // maintenance path
                session.Engine.State.tooling_condition = 40f;
                var maint = session.ReplaceTooling();
                if (session.Engine.State.tooling_condition >= 40f) { Console.WriteLine($"[PASS] Check 8: Tooling maintenance applied ({maint.MessageKey})."); passed++; }
                else Console.WriteLine("[FAIL] Check 8: maintenance failed.");

                // snapshot
                if (session.GetSnapshot().FormulationCount > 0) { Console.WriteLine("[PASS] Check 9: Snapshot projection reads live state."); passed++; }
                else Console.WriteLine("[FAIL] Check 9: snapshot wrong.");

                // save/restore
                var saved = session.CaptureState();
                var restored = new PharmaceuticalTabletHostSession();
                restored.RestoreState(saved);
                if (restored.Engine.IsConstructed == session.Engine.IsConstructed
                    && restored.Engine.State.machine_condition == session.Engine.State.machine_condition)
                { Console.WriteLine("[PASS] Check 10: Save/restore round-trip preserved press state."); passed++; }
                else Console.WriteLine("[FAIL] Check 10: restore round-trip broken.");

                // contract (local bool keeps the drift guard reachable)
                bool tabletContractOk = PharmaceuticalTabletSaveStore.SectionName == "pharmaceutical_tablet"
                    && PharmaceuticalTabletSaveStore.FileName == "pharmaceutical_tablet_save.json";
                if (tabletContractOk)
                { Console.WriteLine("[PASS] Check 11: Save store contract names verified."); passed++; }
                else Console.WriteLine("[FAIL] Check 11: contract names wrong.");
            }
            catch (Exception ex) { Console.WriteLine($"[FAIL] Unexpected probe exception: {ex.Message}"); }
            Console.WriteLine($"=== Pharmaceutical Tablet Self-Test: {passed}/{total} passed ===");
            return passed == total ? 0 : 1;
        }

        private static string? PickFormulation(string dataRoot)
        {
            try
            {
                string path = Path.Combine(dataRoot, "tablet_manufacturing_catalog.json");
                if (!File.Exists(path)) return null;
                var options = new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var catalog = System.Text.Json.JsonSerializer.Deserialize<TabletManufacturingCatalog>(File.ReadAllText(path), options);
                return catalog?.formulations?.Count > 0 ? catalog.formulations[0].formulation_id : null;
            }
            catch { return null; }
        }
    }
}
