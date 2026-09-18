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
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private PlasticPyrolysisSystem? _plasticPyrolysis;
        private PlasticPyrolysisHostSession _plasticPyrolysisSession = null!;
        private CargoAirdropSystem? _cargoAirdrop;
        private CargoAirdropHostSession _cargoAirdropSession = null!;
        private bool _cargoAirdropDirty;

        // ── Plan 202: Plastic Pyrolysis ─────────────────────────────────

        public PlasticPyrolysisHostSession EnsurePlasticPyrolysis()
        {
            if (_plasticPyrolysis != null) return _plasticPyrolysisSession;

            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("plastic_pyrolysis") : new SeededRng(2020);
            _plasticPyrolysis = new PlasticPyrolysisSystem(rng, new GodotLog());

            string catalogPath = CatalogPath.ResolveCatalog("plastic_pyrolysis_catalog.json");
            var _catalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
            if (_catalogIo.FileExists(catalogPath))
            {
                string json = _catalogIo.ReadAllText(catalogPath);
                {
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
                CaptureSection("plastic_pyrolysis", PlasticPyrolysisSaveStore.TryCapturePersisted(_plasticPyrolysis.CaptureState()));
            }
        }

        // ── Plan 205: Cargo Airdrop & Emergency Supply Recovery ─────────

        public CargoAirdropHostSession EnsureCargoAirdrop()
        {
            if (_cargoAirdrop != null) return _cargoAirdropSession;

            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("cargo_airdrop") : new SeededRng(2050);
            _cargoAirdrop = new CargoAirdropSystem(rng, new GodotLog());

            string catalogPath = CatalogPath.ResolveCatalog("cargo_airdrop_catalog.json");
            var _catalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
            if (_catalogIo.FileExists(catalogPath))
            {
                string json = _catalogIo.ReadAllText(catalogPath);
                {
                    try
                    {
                        var catalog = System.Text.Json.JsonSerializer.Deserialize<CargoAirdropCatalog>(json);
                        if (catalog != null)
                            _cargoAirdrop.BindCatalog(catalog);
                    }
                    catch (Exception ex)
                    {
                        GD.PrintErr($"[Main.Airdrop] Failed to parse {catalogPath}: {ex.Message}");
                    }
                }
            }

            // Live wind projections from the single weather authority.
            _cargoAirdrop.DayProvider = () => _simDay;
            _cargoAirdrop.WindDirectionDeg = () => _world?.Weather.WindDirectionDeg ?? 0f;
            _cargoAirdrop.WindSpeedKph = () => _world?.Weather.WindSpeedKph ?? 0f;

            var saved = CargoAirdropSaveStore.TryLoad();
            if (saved != null)
                _cargoAirdrop.RestoreState(saved);

            _cargoAirdropSession = new CargoAirdropHostSession(_cargoAirdrop);

            _cargoAirdrop.OnDropScheduled += ev =>
            {
                _journal?.TryAddRawEntry("airdrop_scheduled", $"Resolved contact {ev.source_signal_id} arranged a supply canister drop — watch the skies from day {ev.landing_day}.", null!, _simDay);
                _cargoAirdropDirty = true;
            };
            _cargoAirdrop.OnDropLanded += ev =>
            {
                RegisterAirdropDestination(ev);
                _journal?.TryAddRawEntry("airdrop_landed", $"Supply canister down at grid ({ev.landing_x},{ev.landing_y}) — beacon live for {ev.beacon_expires_day - ev.landing_day} days.", null!, _simDay);
                _cargoAirdropDirty = true;
            };
            _cargoAirdrop.OnDropRecovered += ev =>
            {
                _journal?.TryAddRawEntry("airdrop_recovered", $"Supply canister {ev.event_id} recovered.", null!, _simDay);
                _cargoAirdropDirty = true;
            };
            _cargoAirdrop.OnDropIntercepted += ev =>
            {
                _journal?.TryAddRawEntry("airdrop_intercepted", $"Hostiles reached canister {ev.event_id} before the recovery team.", null!, _simDay);
                _cargoAirdropDirty = true;
            };
            _cargoAirdrop.OnDropExpired += ev =>
            {
                _journal?.TryAddRawEntry("airdrop_expired", $"Canister {ev.event_id} was buried by drifting ash — unrecoverable.", null!, _simDay);
                _cargoAirdropDirty = true;
            };

            return _cargoAirdropSession;
        }

        /// <summary>
        /// Plan 205 §7.4: resolved radio contacts may offer scheduled supply
        /// drops. The engine's max-active cap gates flooding; unmatched outcomes
        /// schedule nothing. Deterministic target spread around the shelter grid.
        /// </summary>
        private void AttachAirdropRadioHooks()
        {
            if (_radio == null || _cargoAirdrop == null) return;

            _radio.DistressSystem.OnSignalResolved += (def, state, resolution) =>
            {
                foreach (var profile in _cargoAirdrop.Catalog.drop_profiles)
                {
                    if (!profile.trigger_signal_outcomes.Contains(def.OutcomeType)) continue;

                    int day = _simDay;
                    int targetX = ((day * 13) % 31) - 15;
                    int targetY = ((day * 29) % 27) - 13;
                    var res = _cargoAirdrop.ScheduleDrop(profile.drop_profile_id, def.FrequencyId, targetX, targetY);
                    if (!res.IsSuccess)
                        _journal?.TryAddRawEntry("airdrop_declined", $"A supply drop was offered by contact {def.FrequencyId} but could not be scheduled ({res.FailureCode}).", null!, _simDay);
                    break; // one drop per resolved signal
                }
            };
        }

        private void SetupCargoAirdrop()
        {
            EnsureCargoAirdrop();
        }

        private void SaveCargoAirdrop()
        {
            if (_cargoAirdrop != null)
            {
                CaptureSection("cargo_airdrop", CargoAirdropSaveStore.TryCapturePersisted(_cargoAirdrop.CaptureState()));
            }
        }

        /// <summary>
        /// Registers the drop site as a canonical expedition destination so
        /// recovery uses the standard sortie lifecycle (roadmap §7.11).
        /// </summary>
        private void RegisterAirdropDestination(AirdropEventState ev)
        {
            string locationId = $"airdrop_{ev.event_id}";
            if (Ashfall.Core.Expeditions.ExpeditionDefinitionRegistry.Get(locationId) != null) return;
            Ashfall.Core.Expeditions.ExpeditionDefinitionRegistry.Register(new Ashfall.Core.Expeditions.ExpeditionDefinition
            {
                id = locationId,
                displayName = $"Supply Canister {ev.event_id}",
                distanceTicks = 4,
                dangerLevel = 2,
                encounterChancePerTick = 0.10f,
                lootCategories = new List<string>(),
                scavenging_table_id = string.Empty
            });
        }

        /// <summary>
        /// Daily airdrop tick: descent/landing/interception plus crate collection
        /// for any sortie looting at a drop-site destination (roadmap §7.13 —
        /// capacity enforced by the expedition authority per item).
        /// </summary>
        private void TickCargoAirdrop(int currentDay)
        {
            if (_cargoAirdrop == null) return;

            _cargoAirdrop.TickDay(currentDay);

            // Crate collection: any active sortie in the looting phase at a
            // drop-site destination transfers cargo via TryGrantLoot.
            var active = _expeditions.Engine.Active;
            if (active == null || active.Count == 0) return;

            foreach (var kvp in active)
            {
                var exp = kvp.Value;
                if (exp.phase != (int)Ashfall.Core.Expeditions.ExpeditionPhase.Looting) continue;
                if (!exp.locationId.StartsWith("airdrop_", StringComparison.Ordinal)) continue;

                var drop = _cargoAirdrop.FindLandedAt(exp.locationId);
                if (drop == null) continue;
                if (drop.remaining_contents.Count == 0) continue;

                var expeditionSystem = _expeditions.Engine;
                _cargoAirdrop.CollectCrate(drop.event_id, (itemId, weightPerUnit, quantity) =>
                    expeditionSystem.TryGrantLoot(exp.survivorId, itemId, weightPerUnit, quantity)
                        == Ashfall.Core.Expeditions.ExpeditionSystem.LootGrantStatus.Granted);
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
