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
                var armorLoad = VehicleArmorGradeCatalogLoader.Load(dataDirectory, fileIO, json);
                bool armorCatalogLoaded = armorLoad.Catalog != null && !armorLoad.HasErrors
                    && armorLoad.Catalog.grades.Count == 5;
                Check("armor_catalog_loaded", armorCatalogLoaded,
                    armorLoad.HasErrors ? string.Join("; ", armorLoad.Errors) : $"rows={armorLoad.Catalog?.grades.Count}");
                if (armorLoad.Catalog != null && !armorLoad.HasErrors)
                    garage.LoadArmorCatalog(armorLoad.Catalog);
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

                // ── CF-P6 armor grade seam ─────────────────────────────────
                var armorGrade = garage.GetArmorGrade("grade_1_scrap_plate");
                if (armorGrade != null)
                {
                    EnsureArmorMaterials(inventory, armorGrade);
                    bool armorInstalled = garage.InstallArmorGrade(vehicleId, armorGrade.id, inventory, out string armorReason);
                    Check("armor_install_g1", armorInstalled, armorReason);
                    var armorRecord = garage.GetRecord(vehicleId);
                    Check("armor_stamp_neutral_without_foundry",
                        armorRecord != null
                        && armorRecord.armorIntegrityPermille == armorRecord.armorIntegrityMaxPermille
                        && armorRecord.armorIntegrityMaxPermille == armorGrade.integrity_pool_permille
                        && string.IsNullOrEmpty(armorRecord.armorMaterialProfileId),
                        $"integrity={armorRecord?.armorIntegrityPermille}/{armorRecord?.armorIntegrityMaxPermille}");

                    var armorProfile = new ExpeditionVehicleProfile
                    {
                        vehicleId = vehicleId,
                        speedMultiplier = 1f,
                        fuelPerTravelTick = 1f,
                        breakdownChancePerTick = 0.2f
                    };
                    garage.DecorateProfile(armorProfile);
                    Check("armor_mitigation_bounded",
                        armorProfile.breakdownChancePerTick < 0.2f && armorProfile.breakdownChancePerTick > 0f,
                        $"risk={armorProfile.breakdownChancePerTick}");

                    int beforeArmorWear = armorRecord?.chassisStressPermille ?? 0;
                    garage.RecordTripWear(vehicleId, 10f);
                    Check("armor_wear_absorption",
                        armorRecord != null && armorRecord.chassisStressPermille == beforeArmorWear + 16
                        && armorRecord.armorIntegrityPermille == armorGrade.integrity_pool_permille - 4,
                        $"chassis={armorRecord?.chassisStressPermille}, integrity={armorRecord?.armorIntegrityPermille}");

                    if (armorRecord != null) armorRecord.armorIntegrityPermille = 0;
                    bool reforged = garage.ReforgeArmorPlate(vehicleId, inventory, out string reforgeReason);
                    Check("armor_depleted_then_reforge", reforged
                        && armorRecord != null && armorRecord.armorIntegrityPermille == armorRecord.armorIntegrityMaxPermille,
                        reforgeReason);

                    var legacyProbe = new VehicleGarageSystem(garageCatalog, new SeededRng(50));
                    legacyProbe.LoadArmorCatalog(armorLoad.Catalog!);
                    var legacyProfile = new ExpeditionVehicleProfile
                    {
                        vehicleId = "unrecorded_vehicle",
                        speedMultiplier = 1f,
                        fuelPerTravelTick = 1f,
                        breakdownChancePerTick = 0.2f
                    };
                    legacyProbe.DecorateProfile(legacyProfile);
                    Check("armor_legacy_parity",
                        Math.Abs(legacyProfile.speedMultiplier - 1f) < 0.001f
                        && Math.Abs(legacyProfile.fuelPerTravelTick - 1f) < 0.001f
                        && Math.Abs(legacyProfile.breakdownChancePerTick - 0.2f) < 0.001f);

                    var armorSaved = garage.CaptureState();
                    var armorRestored = new VehicleGarageSystem(garageCatalog, new SeededRng(50));
                    armorRestored.LoadArmorCatalog(armorLoad.Catalog!);
                    armorRestored.RestoreState(armorSaved);
                    var restoredArmorRecord = armorRestored.GetRecord(vehicleId);
                    Check("armor_save_roundtrip", restoredArmorRecord != null
                        && restoredArmorRecord.armorGradeId == armorRecord?.armorGradeId
                        && restoredArmorRecord.armorIntegrityPermille == armorRecord?.armorIntegrityPermille
                        && restoredArmorRecord.armorIntegrityMaxPermille == armorRecord?.armorIntegrityMaxPermille);
                }
                else
                {
                    Check("armor_install_g1", false, "grade_1_scrap_plate missing");
                    Check("armor_stamp_neutral_without_foundry", false, "grade_1_scrap_plate missing");
                    Check("armor_mitigation_bounded", false, "grade_1_scrap_plate missing");
                    Check("armor_wear_absorption", false, "grade_1_scrap_plate missing");
                    Check("armor_depleted_then_reforge", false, "grade_1_scrap_plate missing");
                    Check("armor_legacy_parity", false, "grade_1_scrap_plate missing");
                    Check("armor_save_roundtrip", false, "grade_1_scrap_plate missing");
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

        private static void EnsureArmorMaterials(Inventory inventory, VehicleArmorGradeDefinition grade)
        {
            foreach (var cost in grade.install_cost)
                inventory.AddById(cost.item_id, 100);
            foreach (var cost in grade.reforge_cost)
                inventory.AddById(cost.item_id, 100);
        }
    }
}
