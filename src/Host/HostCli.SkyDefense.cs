// SPDX-License-Identifier: MIT
// ============================================================================
// HostCli Partial : Flagship Task 7 — Sky Defense Battery selftest
// --sky-defense-selftest: telemetry track intake, magazine logistics,
// deterministic volley, heat/hydraulics service, crew claim, save round-trip,
// and player-panel construction/binding over the real Core authority.
// ============================================================================
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.IO;
using Ashfall.Core.SkyDefense;
using Ashfall.Core.Shelter;
using AtomicWar.GodotApp.UI;
using Godot;

namespace AtomicWar.GodotApp
{
    public static partial class HostCli
    {
        private const string SkyDefenseAmmoItem = "ammo_76mm_he_flak";

        public static int RunSkyDefenseSelfTest(string dataDirectory)
        {
            int pass = 0, fail = 0;
            var details = new List<string>();

            void Check(string gate, bool ok, string note = "")
            {
                if (ok) { pass++; details.Add($"  PASS {gate}"); }
                else { fail++; details.Add($"  FAIL {gate}{(note.Length > 0 ? " — " + note : "")}"); }
                GD.Print($"{(ok ? "[PASS]" : "[FAIL]")} sky_defense/{gate}");
            }

            try
            {
                var inventory = new Inventory();
                var telemetry = new OrbitalHarrowTelemetrySystem(new SkyLayerArmorSystem(), new SeededRng(42));
                var defense = new SkyDefenseBatterySystem(42, inventory: inventory, telemetry: telemetry);

                var ordnance = SkyDefenseOrdnanceCatalogLoader.Load(dataDirectory, new FileSystemIO(), new SystemTextJsonSerializer());
                Check("catalog_loaded", ordnance.Count >= 1, $"rows={ordnance.Count}");
                defense.LoadOrdnanceCatalog(ordnance);

                var turret = defense.EnsureDefaultTurret();
                Check("default_turret", turret != null && defense.Turrets.Count >= 1);

                inventory.TryProduce(SkyDefenseAmmoItem, 20);
                inventory.TryProduce(SkyDefenseBatterySystem.ServiceOilItemId, 10);

                // ── Telemetry intake + dedup ────────────────────────────────
                telemetry.ScheduleImpact(30, 4, 12f);
                Check("track_intake", defense.Tracks.Count == 1, $"tracks={defense.Tracks.Count}");
                telemetry.ScheduleImpact(30, 4, 12f);
                Check("track_dedup", defense.Tracks.Count == 1, $"tracks={defense.Tracks.Count}");

                // ── Magazine logistics ──────────────────────────────────────
                var def = defense.GetOrdnance(SkyDefenseAmmoItem);
                int before = inventory.CountById(SkyDefenseAmmoItem);
                var load = defense.TryLoadMagazine(turret!.turret_id, SkyDefenseAmmoItem);
                Check("magazine_loaded", load.IsSuccess && turret.magazine_count == (def?.magazine_units ?? 0),
                    $"mag={turret.magazine_count}");
                Check("magazine_atomic", inventory.CountById(SkyDefenseAmmoItem) == before - (def?.magazine_units ?? 0),
                    $"inv={inventory.CountById(SkyDefenseAmmoItem)}");

                // ── Crew claim ──────────────────────────────────────────────
                var assign = defense.TryAssignCrew(turret.turret_id, "survivor_selftest_gunner");
                Check("crew_assign", assign.IsSuccess && turret.assigned_crew_ids.Contains("survivor_selftest_gunner"));
                var remove = defense.TryRemoveCrew(turret.turret_id, "survivor_selftest_gunner");
                Check("crew_remove", remove.IsSuccess && !turret.assigned_crew_ids.Contains("survivor_selftest_gunner"));

                // ── Deterministic volley ────────────────────────────────────
                int heatBefore = turret.barrel_heat;
                var volley = defense.TryFireVolley(turret.turret_id, "custom_impact");
                Check("volley_fired", volley.IsSuccess && defense.TotalVolleys == 1, $"volleys={defense.TotalVolleys}");
                Check("volley_cost", turret.magazine_count == (def?.magazine_units ?? 0) - 1 && turret.barrel_heat >= heatBefore,
                    $"mag={turret.magazine_count} heat={turret.barrel_heat}");

                // ── Hydraulic service ───────────────────────────────────────
                int oilBefore = inventory.CountById(SkyDefenseBatterySystem.ServiceOilItemId);
                var service = defense.TryServiceHydraulics(turret.turret_id);
                Check("service_ok", service.IsSuccess && turret.volleys_since_service == 0);
                Check("service_cost", inventory.CountById(SkyDefenseBatterySystem.ServiceOilItemId) == oilBefore - 1,
                    $"oil={inventory.CountById(SkyDefenseBatterySystem.ServiceOilItemId)}");

                // ── Daily dissipation + track prune ─────────────────────────
                int preTickHeat = turret.barrel_heat;
                turret.barrel_heat = 100;
                defense.TickDay(31);
                Check("heat_dissipates", turret.barrel_heat < 100, $"heat={turret.barrel_heat}");
                Check("track_pruned", defense.Tracks.Count == 0, $"tracks={defense.Tracks.Count}");

                // ─ Save round-trip ─────────────────────────────────────────
                var saved = defense.CaptureState();
                var restored = new SkyDefenseBatterySystem(42);
                restored.RestoreState(saved);
                var restoredTurret = restored.GetTurret(turret.turret_id);
                Check("save_roundtrip",
                    restored.Turrets.Count == defense.Turrets.Count
                    && restored.TotalVolleys == defense.TotalVolleys
                    && restoredTurret != null
                    && restoredTurret.magazine_count == turret.magazine_count);

                // ── Player panel construction + binding ─────────────────────
                var panel = new SkyDefenseBatteryPanel();
                panel._Ready();
                panel.Bind(
                    defense,
                    () => (IReadOnlyList<string>)new List<string> { "survivor_selftest_gunner" },
                    id => id,
                    id => inventory.CountById(id));
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

            GD.Print("\n[HostCli] sky_defense self-test" + (fail == 0 ? " PASS" : " FAIL"));
            foreach (var line in details) GD.Print(line);
            return EmitSummary("sky_defense_selftest", fail == 0, passedCount: pass, failedCount: fail,
                details: "sky-layer counter-battery catalog/telemetry/magazine/volley/service/save/panel");
        }
    }
}