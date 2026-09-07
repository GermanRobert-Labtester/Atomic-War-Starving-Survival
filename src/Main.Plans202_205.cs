// SPDX-License-Identifier: MIT
// ============================================================================
// Main Partial : Plans 202-205 Host Wire & Orchestration
// Subsystems   : Plan 202 — Plastic Pyrolysis (waste plastic → fuel fractions)
//                (Plans 203-205 land in follow-up waves of this flagship)
// ============================================================================
using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private PlasticPyrolysisSystem? _plasticPyrolysis;
        private PlasticPyrolysisHostSession _plasticPyrolysisSession = null!;

        // ── Plan 202: Plastic Pyrolysis ─────────────────────────────────

        public PlasticPyrolysisHostSession EnsurePlasticPyrolysis()
        {
            if (_plasticPyrolysis != null) return _plasticPyrolysisSession;

            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("plastic_pyrolysis") : new SeededRng(2020);
            _plasticPyrolysis = new PlasticPyrolysisSystem(rng, new GodotLog());

            string catalogPath = "res://Assets/StreamingAssets/Data/plastic_pyrolysis_catalog.json";
            if (Godot.FileAccess.FileExists(catalogPath))
            {
                using var file = Godot.FileAccess.Open(catalogPath, Godot.FileAccess.ModeFlags.Read);
                if (file != null)
                {
                    string json = file.GetAsText();
                    try
                    {
                        var catalog = System.Text.Json.JsonSerializer.Deserialize<PlasticPyrolysisCatalog>(json);
                        if (catalog != null)
                            _plasticPyrolysis.BindCatalog(catalog);
                    }
                    catch (Exception ex)
                    {
                        GD.PrintErr($"[Main.Pyrolysis] Failed to parse {catalogPath}: {ex.Message}");
                    }
                }
            }

            var inv = _inventory?.Inventory ?? new Ashfall.Core.Inventory.Inventory();
            _plasticPyrolysis.BindInventory(
                itemId => inv.CountById(itemId),
                (itemId, amount) => inv.CanAddById(itemId, amount),
                (itemId, amount) => inv.AddById(itemId, amount),
                (itemId, amount) => inv.RemoveById(itemId, amount));

            _plasticPyrolysis.DayProvider = () => _simDay;
            // Duty-roster skill projection is a follow-up; neutral 0.5 until the
            // roster seam is wired (bounded risk reduction, catalog-driven).
            _plasticPyrolysis.OperatorSkillProvider = () => 0.5f;

            var saved = PlasticPyrolysisSaveStore.TryLoad();
            if (saved != null)
                _plasticPyrolysis.RestoreState(saved);

            _plasticPyrolysisSession = new PlasticPyrolysisHostSession(_plasticPyrolysis);

            _plasticPyrolysis.OnBatchCompleted += b =>
            {
                _journal?.TryAddRawEntry("pyro_batch_completed", $"Retort batch {b.batch_id} completed — outputs staged for claim.", null!, _simDay);
            };
            _plasticPyrolysis.OnFireIncident += machineId =>
            {
                // Typed handoff to the hazard authority: the retort bay room takes the fire event.
                _journal?.TryAddRawEntry("pyro_fire", "Fire in the retort bay! Flames lick the draft chamber.", null!, _simDay);
            };
            _plasticPyrolysis.OnGasRelease += severity =>
            {
                // Ventilation authority owns the consequence: this is a load input, not a parallel air state.
                _journal?.TryAddRawEntry("pyro_gas_release", $"Retort off-gas release (severity {severity:P0}) — ventilation strained.", null!, _simDay);
            };
            _plasticPyrolysis.OnIncident += (batch, kind) =>
            {
                if (kind == "machine_damage")
                    _journal?.TryAddRawEntry("pyro_machine_damage", $"Retort damaged during batch {batch.batch_id} — service required.", null!, _simDay);
            };

            // Register the retort as a canonical ventilation source (single air authority).
            // Load fields scale from the catalog's per-day ventilation fraction.
            var ventLoad = _plasticPyrolysis.Catalog.machine.ventilation_load_per_active_day;
            _ventilation?.RegisterSource(new Ashfall.Core.VentilationSource
            {
                sourceId = "pyro_retort",
                roomId = _plasticPyrolysis.Catalog.machine.room_id,
                smokeOutputPerDay = ventLoad * 100f,
                coOutputPerDay = ventLoad * 50f,
                requiresExhaust = true,
                isActive = false
            });

            return _plasticPyrolysisSession;
        }

        private void SetupPlasticPyrolysis()
        {
            EnsurePlasticPyrolysis();
        }

        private void SavePlasticPyrolysis()
        {
            if (_plasticPyrolysis != null)
            {
                CaptureSection("plastic_pyrolysis", PlasticPyrolysisSaveStore.TryCapturePersisted(_plasticPyrolysis.State));
            }
        }

        /// <summary>
        /// Daily tick: project the shelter's real surplus grid power into the retort.
        /// The offgas credit returns to the power grid as fuel units at a pinned
        /// conversion (2 kWh per fuel unit) — bounded by the catalog cap, never
        /// net-positive (roadmap §5.6).
        /// </summary>
        private void TickPlasticPyrolysis(int currentDay)
        {
            if (_plasticPyrolysis == null) return;

            float surplusWatts = Math.Max(0f, _powerGrid?.System.NetWatts ?? 0f);
            float surplusKwh = surplusWatts * 24f / 1000f;

            float creditKwh = _plasticPyrolysis.TickDay(surplusKwh);
            if (creditKwh > 0.001f)
            {
                _powerGrid?.System.AddFuel(creditKwh / 2f);
                _journal?.TryAddRawEntry("pyro_offgas_credit", $"Retort off-gas burned for {creditKwh:F1} kWh of recovered energy.", null!, _simDay);
            }

            // Ventilation source active only while a batch runs (single air authority).
            _ventilation?.SetSourceActive("pyro_retort", _plasticPyrolysis.State.active_batch != null);
        }
    }
}
