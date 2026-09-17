// SPDX-License-Identifier: MIT
// ============================================================================
// HostCli Partial : Plan 50 — Vehicle Garage selftest
// --vehicle-garage-selftest: modification install/uninstall + effects,
// component wear, service, immobilization gate, recovery completion,
// expedition-profile decoration, and player-panel construction.
// ============================================================================
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Inventory;
using Ashfall.Core.IO;
using AtomicWar.GodotApp.UI;
using Godot;

namespace AtomicWar.GodotApp
{
    public static partial class HostCli
    {
        public static int RunVehicleGarageSelfTest(string dataDirectory)
        {
            int pass = 0, fail = 0;
            var details = new List<string>();

            void Check(string gate, bool ok, string note = "")
            {
                if (ok) { pass++; details.Add($"  PASS {gate}"); }
                else { fail++; details.Add($"  FAIL {gate}{(note.Length > 0 ? " — " + note : "")}"); }
                GD.Print($"{(ok ? "[PASS]" : "[FAIL]")} vehicle_garage/{gate}");
            }

            try
            {
                var fileIO = new FileSystemIO();
                var json = new SystemTextJsonSerializer();

                string garagePath = fileIO.Combine(dataDirectory, "vehicle_modifications.json");
                var garageCatalog = VehicleGarageCatalogLoader.Load(fileIO.ReadAllText(garagePath), json);
                Check("catalog_loaded", garageCatalog.modifications.Count >= 1, $"rows={garageCatalog.modifications.Count}");

                var garage = new VehicleGarageSystem(garageCatalog, new SeededRng(50));
                var vehicles = new ExpeditionVehicleSystem(new SeededRng(50));
                vehicles.LoadCatalog(VehicleCatalogLoader.Load(dataDirectory, fileIO, json));

                var inventory = new Inventory();
                const string vehicleId = "vehicle_utility_quad";
                Check("acquire_vehicle", vehicles.AcquireVehicle(vehicleId).IsSuccess);

                // ── Install / effects ───────────────────────────────────────
                var flatbed = FindFirstMod(garage, "cargo");
                Check("catalog_has_cargo_mod", flatbed != null);
                if (flatbed != null)
                {
                    EnsureMaterials(inventory, flatbed);
                    string costItem = flatbed.install_cost[0].item_id;
                    int costAmount = flatbed.install_cost[0].amount;
                    int beforeInstall = inventory.CountById(costItem);
                    bool ok = garage.InstallModification(vehicleId, flatbed.slot_type, flatbed.id, inventory, out string reason);
                    Check("install_mod", ok, reason);
                    Check("installed_slot", garage.GetInstalledSlots(vehicleId).TryGetValue(flatbed.slot_type, out var fitted) && fitted == flatbed.id);
                    Check("install_consumed", inventory.CountById(costItem) == beforeInstall - costAmount,
                        $"{costItem} {beforeInstall}→{inventory.CountById(costItem)}");
                    Check("effective_cargo", Math.Abs(garage.GetEffectiveCargoCapacityDelta(vehicleId) - flatbed.effects.cargo_capacity_delta) < 0.01f,
                        $"cargo={garage.GetEffectiveCargoCapacityDelta(vehicleId)}");

                    // Decoration reaches the expedition profile the sortie uses.
                    var profile = vehicles.CreateExpeditionProfile(vehicleId, 2.5f);
                    Check("profile_built", profile != null);
                    if (profile != null)
                    {
                        float baseCargo = profile.cargoCapacityKg;
                        garage.DecorateProfile(profile);
                        Check("profile_decorated", profile.cargoCapacityKg > baseCargo,
                            $"cargo {baseCargo}→{profile.cargoCapacityKg}");
                    }

                    bool removed = garage.UninstallModification(vehicleId, flatbed.slot_type, inventory, out string removeReason);
                    Check("uninstall_mod", removed, removeReason);
                }

                // ── Service ─────────────────────────────────────────────────
                inventory.AddById("scrap_metal", 50);
                inventory.AddById("mechanical_parts", 50);
                garage.RecordTripWear(vehicleId, 10f);
                var record = garage.GetRecord(vehicleId);
                Check("trip_wear", record != null && record.chassisStressPermille > 0, $"chassis={record?.chassisStressPermille}");
                if (record != null)
                {
                    int before = record.chassisStressPermille;
                    bool serviced = garage.ServiceChassis(vehicleId, inventory, before, out string reason);
                    Check("service_chassis", serviced && record.chassisStressPermille < before, reason);
                }

                // ── Immobilization + recovery ───────────────────────────────
                garage.RecordTripWear(vehicleId, 1000f);
                Check("immobilized", garage.IsImmobilized(vehicleId));
                bool registered = garage.RegisterRecoveryMission(vehicleId, "loc_selftest", 10, out string missionId, out string recReason);
                Check("recovery_registered", registered, recReason);
                garage.AdvanceRecoveries(120);
                var mission = garage.ActiveRecoveries.ContainsKey(missionId) ? garage.ActiveRecoveries[missionId] : null;
                Check("recovery_ready", mission != null && mission.isComplete, $"progress={mission?.progressTicks}");
                bool completed = garage.CompleteRecoveryMission(missionId, inventory, out string compReason);
                Check("recovery_completed", completed && !garage.IsImmobilized(vehicleId), compReason);

                // ── Save round-trip ─────────────────────────────────────────
                var saved = garage.CaptureState();
                var restored = new VehicleGarageSystem(garageCatalog, new SeededRng(50));
                restored.RestoreState(saved);
                Check("save_roundtrip",
                    restored.GetRecord(vehicleId) != null
                    && restored.GetRecord(vehicleId)!.chassisStressPermille == garage.GetRecord(vehicleId)!.chassisStressPermille);

                // ── Player panel construction + binding ─────────────────────
                var panel = new VehicleGaragePanel();
                panel._Ready();
                panel.Bind(garage, vehicles, inventory);
                Check("panel_bound", panel.IsBound);
                panel.RefreshView();
                panel.Unbind();
                Check("panel_unbound", !panel.IsBound);
                panel.Free();
            }
            catch (Exception ex)
            {
                Check("exception", false, ex.Message);
            }

            GD.Print("\n[HostCli] vehicle_garage self-test" + (fail == 0 ? " PASS" : " FAIL"));
            foreach (var line in details) GD.Print(line);
            return EmitSummary("vehicle_garage_selftest", fail == 0, passedCount: pass, failedCount: fail,
                details: "vehicle garage install/service/wear/recovery/save/panel");
        }

        private static VehicleModificationDefinition? FindFirstMod(VehicleGarageSystem garage, string slotType)
        {
            foreach (var kv in garage.GetAllModifications())
            {
                if (kv.Value != null && string.Equals(kv.Value.slot_type, slotType, StringComparison.OrdinalIgnoreCase))
                    return kv.Value;
            }
            return null;
        }

        private static void EnsureMaterials(Inventory inventory, VehicleModificationDefinition mod)
        {
            foreach (var cost in mod.install_cost)
                inventory.AddById(cost.item_id, 100);
        }
    }
}