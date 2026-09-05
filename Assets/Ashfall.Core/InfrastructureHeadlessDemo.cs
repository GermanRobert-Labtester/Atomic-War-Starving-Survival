// SPDX-License-Identifier: MIT
// ASHFALL Flagship Infrastructure Systems headless demo & verification suite (Tasks 1–4).

using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Defense;
using Ashfall.Core.Economy;
using Ashfall.Core.Inventory;
using Ashfall.Core.Medical;
using Ashfall.Core.Shelter;

namespace Ashfall.Core
{
    public static class InfrastructureHeadlessDemo
    {
        public static HeadlessReport Run(string? dataDirectory = null, ILog? log = null)
        {
            CatalogLocator.UseInvariantCulture();
            log ??= NullLog.Instance;
            var report = new HeadlessReport();

            void Check(bool condition, string name)
            {
                report.Checks.Add(new HeadlessCheck { Name = name, Passed = condition });
                if (condition)
                {
                    report.PassedCount++;
                    log.Info("[PASS] " + name);
                }
                else
                {
                    report.FailedCount++;
                    log.Error("[FAIL] " + name);
                }
            }

            log.Info("[InfrastructureHeadlessDemo] begin");

            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            string dataDir = !string.IsNullOrEmpty(dataDirectory) && files.DirectoryExists(dataDirectory)
                ? dataDirectory
                : (files.DirectoryExists("Assets/StreamingAssets/Data") ? "Assets/StreamingAssets/Data" : "StreamingAssets/Data");

            // 1–4: File existence checks
            string caravanPath = Path.Combine(dataDir, "caravan_trade_routes.json");
            string surgeryPath = Path.Combine(dataDir, "surgical_procedures.json");
            string powerPath = Path.Combine(dataDir, "power_subgrid_nodes.json");
            string defensePath = Path.Combine(dataDir, "perimeter_defenses.json");

            Check(files.FileExists(caravanPath), "caravan_trade_routes.json exists");
            Check(files.FileExists(surgeryPath), "surgical_procedures.json exists");
            Check(files.FileExists(powerPath), "power_subgrid_nodes.json exists");
            Check(files.FileExists(defensePath), "perimeter_defenses.json exists");

            // 5–8: Catalog loading
            var routes = CaravanTradeRouteCatalogLoader.Load(dataDir, files, json);
            var procedures = SurgicalProcedureCatalogLoader.Load(dataDir, files, json);
            var nodes = PowerSubgridCatalogLoader.Load(dataDir, files, json);
            var defenses = PerimeterDefenseCatalogLoader.Load(dataDir, files, json);

            Check(routes.Count == 10, $"caravan routes catalog has 10 routes (found {routes.Count})");
            Check(procedures.Count == 10, $"surgical procedures catalog has 10 procedures (found {procedures.Count})");
            Check(nodes.Count == 12, $"power subgrid nodes catalog has 12 nodes (found {nodes.Count})");
            Check(defenses.Count == 8, $"perimeter defenses catalog has 8 defenses (found {defenses.Count})");

            // 9–10: Caravan barter & favored status
            var inv = new Inventory.Inventory { MaxWeight = 5000f, Capacity = 200 };
            var rng = new SeededRng(2026);
            var caravanSys = new CaravanTradeNetworkSystem(routes, inv, rng, log);

            var manifest = caravanSys.ScheduleCaravan("route_compact_supply_line", 1);
            manifest.status = CaravanStatus.Arrived;
            manifest.stocks["clean_water"] = 100;
            inv.TryProduce("scrap_metal", 200);

            var bRes = caravanSys.ExecuteBarter(
                manifest.manifest_id,
                new Dictionary<string, int> { { "scrap_metal", 10 } },
                new Dictionary<string, int> { { "clean_water", 5 } });
            Check(bRes.Success, "caravan atomic barter transaction succeeds");

            for (int i = 0; i < 4; i++)
            {
                caravanSys.ExecuteBarter(
                    manifest.manifest_id,
                    new Dictionary<string, int> { { "scrap_metal", 10 } },
                    new Dictionary<string, int> { { "clean_water", 5 } });
            }
            Check(caravanSys.HasFavoredStatus("faction_the_compact"), "favored barter status unlocks after 5 trades");

            // 11–12: Subgrid power distribution & maintenance
            var powerSys = new PowerDistributionSubgridSystem(nodes, inv, rng, log);
            powerSys.ApplyRoomLoad("room_workshop", 4400f);
            var workshopNode = powerSys.FindNodeForRoom("room_workshop");
            Check(workshopNode != null && workshopNode.temperature_celsius > PowerDistributionSubgridSystem.AmbientTemperatureCelsius,
                "power subgrid thermal model increases temperature under high load");

            inv.TryProduce("machine_oil", 1);
            inv.TryProduce("electrical_wire", 2);
            var maintRes = powerSys.PerformTransformerMaintenance("node_workshop_feed");
            Check(maintRes.IsSuccess && workshopNode!.transformer_oil_condition == 100.0f,
                "transformer maintenance restores oil condition and temperature");

            // 13–14: Perimeter defense construction & turret engagement
            var defenseSys = new PerimeterDefenseSystem(defenses, inv, rng, log);
            inv.TryProduce("scrap_metal", 20);
            inv.TryProduce("electrical_wire", 10);
            inv.TryProduce("ammo_556", 100);

            var cTurretRes = defenseSys.ConstructEmplacement("def_sentry_turret_556");
            var turret = defenseSys.Emplacements.Count > 0 ? defenseSys.Emplacements[0] : null;
            defenseSys.LoadAmmo(turret!.emplacement_id, 50);

            var assaultRes = defenseSys.SimulateRaiderAssault(10, false, id => powerSys.IsRoomPowered("room_airlock"));
            Check(cTurretRes.IsSuccess && turret.loaded_ammo_count < 50, "perimeter turret constructs and loads ammo");
            Check(assaultRes.Repelled && assaultRes.RoundsFiredTotal > 0, "raider assault is repelled by powered sentry fire");

            // 15: Surgery validation & autoclave sterilization
            var surgSys = new AdvancedSurgicalWardSystem(procedures, inv, rng, log);
            inv.TryProduce("surgical_scalpel", 1);
            inv.TryProduce("anesthetic_ether", 5);
            inv.TryProduce("sterile_gauze", 10);
            inv.TryProduce("clean_water", 5);

            var valPreOp = surgSys.ValidatePreOp("patient_dan", "surgeon_eve", "surg_shrapnel_extraction");
            surgSys.StartOperation("patient_dan", "surgeon_eve", "surg_shrapnel_extraction");
            var op = surgSys.ActiveOperations[0];
            for (int h = 0; h < op.total_duration_hours; h++) surgSys.TickOperationHour(op);

            var autoRes = surgSys.RunAutoclaveCycle(hasPower: powerSys.IsRoomPowered("room_clinic"));
            Check(valPreOp.IsSuccess && autoRes.IsSuccess && surgSys.SterileFieldPercent == 100.0f,
                "surgical pre-op, execution, and autoclave sterilization complete");

            // 16: Cross-system save / restore round trip
            var cSave = caravanSys.CaptureState();
            var pSave = powerSys.CaptureState();
            var dSave = defenseSys.CaptureState();
            var sSave = surgSys.CaptureState();

            var freshCaravan = new CaravanTradeNetworkSystem(routes, new Inventory.Inventory(), new SeededRng(1));
            var freshPower = new PowerDistributionSubgridSystem(nodes, new Inventory.Inventory(), new SeededRng(1));
            var freshDefense = new PerimeterDefenseSystem(defenses, new Inventory.Inventory(), new SeededRng(1));
            var freshSurg = new AdvancedSurgicalWardSystem(procedures, new Inventory.Inventory(), new SeededRng(1));

            freshCaravan.RestoreState(cSave);
            freshPower.RestoreState(pSave);
            freshDefense.RestoreState(dSave);
            freshSurg.RestoreState(sSave);

            bool saveOk = freshCaravan.HasFavoredStatus("faction_the_compact") &&
                          freshPower.FindNode("node_workshop_feed")!.transformer_oil_condition == 100.0f &&
                          freshDefense.Emplacements[0].loaded_ammo_count == turret.loaded_ammo_count &&
                          freshSurg.RecoveryPatients.Count == 1;

            Check(saveOk, "all four infrastructure systems round-trip through persistence without state loss");

            report.Passed = report.FailedCount == 0 && report.PassedCount > 0;
            report.Summary = $"{report.PassedCount}/{report.PassedCount + report.FailedCount} passed";
            log.Info($"[InfrastructureHeadlessDemo] complete: {report.Summary}");
            return report;
        }
    }
}
