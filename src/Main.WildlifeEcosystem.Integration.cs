// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Farming;
using Ashfall.Core.Inventory;
using Ashfall.Core.Random;
using Ashfall.Core.Survivors;
using System;
using System.Collections.Generic;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {

        // ==================================================================
        // Plan 165 — WildlifeEcosystemSystem (ecology over migration packs)
        // ==================================================================

        private WildlifeEcosystemHostSession _wildlifeEcosystem = null!;
        private bool _wildlifeEcosystemDirty;
        private UI.BestiaryPanel _bestiaryPanel = null!;
        private bool _bestiaryPanelBound;

        private void SetupWildlifeEcosystem()
        {
            if (_wildlifeEcosystem != null) return;
            SetupWorld();

            var system = new Ashfall.Core.World.WildlifeEcosystemSystem();
            system.LoadCatalog(Ashfall.Core.World.WildlifeEcosystemCatalogLoader.Load(_dataDir));
            var saved = WildlifeEcosystemSaveStore.TryLoad();
            if (saved != null)
                system.RestoreState(saved);

            _wildlifeEcosystem = new WildlifeEcosystemHostSession(system);
            _wildlifeEcosystem.StateChanged += () => _wildlifeEcosystemDirty = true;

            system.OnApexPredatorSpotted += (species, sector) =>
                _journal?.TryAddRawEntry($"apex_{species}_{sector}",
                    $"Something big is working {sector}: {species.Replace("species_", "").Replace('_', ' ')}. Keep the parties armed.",
                    null!, _simDay);
            system.OnLocalExtinction += (species, sector) =>
                _journal?.TryAddRawEntry($"extinct_{species}_{sector}",
                    $"No more {species.Replace("species_", "").Replace('_', ' ')} around {sector}. The traps will come up empty.",
                    null!, _simDay);
            system.OnWildlifeTamed += a =>
                _journal?.TryAddRawEntry($"tamed_{a.animal_id}",
                    $"A {a.species_id.Replace("species_", "").Replace('_', ' ')} was tamed and moved into the pens.",
                    null!, _simDay);
            // Plan 176 — anomaly-driven avoidance migrations journal through the
            // ecology system's own typed event.
            system.OnHazardAvoidanceMigration += (species, fromSector, toSector) =>
                _journal?.TryAddRawEntry($"wildlife_avoid_{species}_{fromSector}",
                    $"{species.Replace("species_", "").Replace('_', ' ')} packs are leaving {fromSector} — something in that sector is wrong.",
                    null!, _simDay);
        }

        private void SaveWildlifeEcosystem()
        {
            if (_wildlifeEcosystem == null) return;
            var payload = WildlifeEcosystemSaveStore.TryCapturePersisted(_wildlifeEcosystem.System.CaptureState());
            if (!string.IsNullOrEmpty(payload))
            {
                CaptureSection(WildlifeEcosystemSaveStore.SectionName, payload);
                _wildlifeEcosystemDirty = false;
            }
        }

        /// <summary>Ecology day — ticks inside EvolvingWorldDayOwner right after
        /// the migration authority moved the packs (single population source).</summary>
        private void TickWildlifeEcosystemDay(int day)
        {
            SetupWildlifeEcosystem();
            var world = _world;
            if (world?.Wildlife == null) return;
            string season = world.Weather?.GetSeasonForDay(day)?.id ?? "any";
            float rad = world.Weather?.OutdoorRadModifier ?? 100f;

            var popRng = _campaignDay != null
                ? _campaignDay.Rng.Fork(CampaignStreamIds.WildlifePopulation, day, 0) : new SeededRng(1650 + day);
            var migRng = _campaignDay != null
                ? _campaignDay.Rng.Fork(CampaignStreamIds.WildlifeMigration, day, 0) : new SeededRng(1651 + day);
            var apexRng = _campaignDay != null
                ? _campaignDay.Rng.Fork(CampaignStreamIds.WildlifeApex, day, 0) : new SeededRng(1652 + day);
            // Plan 176 — anomaly hazard modifiers feed wildlife avoidance through
            // the migration authority; null keeps legacy behavior unchanged.
            var hazardModifiers = BuildSectorHazardModifiers();
            _wildlifeEcosystem.System.TickDay(day, world.Wildlife, rad, season, popRng, migRng, apexRng, hazardModifiers);
            _bestiaryPanel?.RefreshView();
        }

        private void HandleWildlifeAction(string action, string param)
        {
            if (action == "OPEN")
            {
                SetupWildlifeEcosystem();
                if (!_bestiaryPanelBound && _wildlifeEcosystem != null)
                {
                    _bestiaryPanel.Bind(_wildlifeEcosystem, () => _world?.Wildlife);
                    _bestiaryPanelBound = true;
                }
                _bestiaryPanel.SetWorldSector(_world?.ShelterSectorId ?? "");
                _bestiaryPanel.Open();
                return;
            }
            if (action == "CLOSE")
            {
                _bestiaryPanel.Close();
                return;
            }

            if (_wildlifeEcosystem == null) return;
            var world = _world;
            if (world?.Wildlife == null) return;

            switch (action)
            {
                case "TAME": // param "speciesId|sectorId|caretakerId"
                {
                    var parts = (param ?? "").Split('|');
                    if (parts.Length < 3) { _wildlifeEcosystem.MarkDirty("Malformed tame request."); break; }
                    var tamingRng = _campaignDay != null
                        ? _campaignDay.Rng.Fork(CampaignStreamIds.WildlifeTaming, _simDay, 0)
                        : new SeededRng(1653 + _simDay);
                    var animal = _wildlifeEcosystem.System.TryTame(
                        parts[0], parts[1], _simDay, parts[2], world.Wildlife, tamingRng);
                    if (animal == null)
                        _wildlifeEcosystem.MarkDirty($"Could not tame a {parts[0]} here.");
                    else
                    {
                        // Plan 174 — the taming record becomes a persistent
                        // companion under the companion authority (identity =
                        // the wildlife animal_id, never duplicated).
                        SetupCompanionAnimals();
                        _companions?.RegisterCompanion(animal.animal_id, animal.species_id, animal.tamed_day, null);
                    }
                    break;
                }
                case "OBSERVE": // param speciesId — records a sighting from the field
                {
                    if (string.IsNullOrEmpty(param)) break;
                    string sector = _world.ShelterSectorId ?? "";
                    _wildlifeEcosystem.System.RecordObservation(param, sector, _simDay, 1f);
                    _wildlifeEcosystem.MarkDirty($"Observed {param.Replace("species_", "").Replace('_', ' ')} near {sector}.");
                    break;
                }
            }
            _bestiaryPanel?.RefreshView();
        }

    }
}
