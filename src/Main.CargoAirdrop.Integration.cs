// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Shelter;
using Ashfall.Core.World;
using Godot;
using System;
using System.Collections.Generic;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private CargoAirdropSystem? _cargoAirdrop;
        private CargoAirdropHostSession _cargoAirdropSession = null!;
        private bool _cargoAirdropDirty;

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

    }
}
