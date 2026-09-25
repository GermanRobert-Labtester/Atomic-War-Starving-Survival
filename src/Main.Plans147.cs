// SPDX-License-Identifier: MIT
// ============================================================================
// Main Partial : Plan 147 Host Wire & Orchestration
// Subsystem    : Bunker Contraband Barter — stash discovery layer
// Contract     : The Core ContrabandStashSystem is the only executable bridge
//                between the contraband catalog and canonical inventory. No
//                ContrabandMechanics field is read here; feedback flows through
//                the journal (the game's single feedback strip). No UI panel.
// ============================================================================
using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Ashfall.Core.Inventory;
using Ashfall.Core.Medical;
using Ashfall.Core.Narrative;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private ContrabandStashSystem? _contrabandStash;
        private bool _dependencyConsumeHookWired;
        private Dictionary<string, ChemicalDependencyKind>? _dependencyKindsByItemId;

        // ── Narcotics slice: canonical consumption → dependency authority ──

        /// <summary>
        /// Loads chemical_dependency_items.json (item_id → dependency_kind).
        /// Parse failure leaves the map null — consumption then simply carries
        /// no dependency routing (fail closed, no invented kinds).
        /// </summary>
        private Dictionary<string, ChemicalDependencyKind>? EnsureDependencyKindMap()
        {
            if (_dependencyKindsByItemId != null) return _dependencyKindsByItemId;

            string catalogPath = CatalogPath.ResolveCatalog("chemical_dependency_items.json");
            var _catalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
            if (!_catalogIo.FileExists(catalogPath)) return null;
            string _depJson = _catalogIo.ReadAllText(catalogPath);

            try
            {
                using var doc = System.Text.Json.JsonDocument.Parse(_depJson);
                var map = new Dictionary<string, ChemicalDependencyKind>(StringComparer.Ordinal);
                foreach (var it in doc.RootElement.GetProperty("items").EnumerateArray())
                {
                    if (!it.TryGetProperty("item_id", out var idEl)) continue;
                    if (!it.TryGetProperty("dependency_kind", out var kindEl)) continue;
                    if (map.ContainsKey(idEl.GetString() ?? string.Empty)) continue;
                    map[idEl.GetString()!] = kindEl.GetString()?.ToLowerInvariant() switch
                    {
                        "opioid" => ChemicalDependencyKind.Opioid,
                        "alcohol" => ChemicalDependencyKind.Alcohol,
                        "stimulant" => ChemicalDependencyKind.Stimulant,
                        "sedative" => ChemicalDependencyKind.Sedative,
                        _ => ChemicalDependencyKind.Opioid // catalog rows are all dependency substances
                    };
                }
                _dependencyKindsByItemId = map;
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[Main.Contraband] failed to parse dependency catalog: {ex.Message}");
            }
            return _dependencyKindsByItemId;
        }

        /// <summary>
        /// Routes committed inventory consumptions of dependency-catalog items
        /// (morphine, alcohol, …) into the ChemicalDependencySystem — exactly
        /// once per committed consumption, because InventoryHostSession.
        /// OnConsumed fires exactly once after the transaction commits. The
        /// dependency system owns dose math and events; nothing here reads any
        /// contraband mechanics field.
        /// </summary>
        private void WireDependencyConsumeHook()
        {
            if (_dependencyConsumeHookWired || _inventory == null) return;
            _dependencyConsumeHookWired = true;

            _inventory.OnConsumed += (survivorId, itemId) =>
            {
                if (_chemicalDependency == null || string.IsNullOrEmpty(itemId)) return;
                var kinds = EnsureDependencyKindMap();
                if (kinds == null || !kinds.TryGetValue(itemId, out var kind)) return;

                _chemicalDependency.System.OnSubstanceConsumed(survivorId ?? string.Empty, itemId, kind);
            };
        }

        // ── Setup ────────────────────────────────────────────────────────

        public ContrabandStashSystem EnsureContrabandStash()
        {
            if (_contrabandStash != null) return _contrabandStash;

            var inv = _inventory?.Inventory ?? new Inventory();
            var itemCatalog = _inventory?.Catalog;

            // Fail closed: an invalid catalog (unknown mechanics key, bad
            // tier/price, NaN, duplicate ids) leaves the contraband layer
            // inert rather than partially trusted.
            var catalog = LoadContrabandCatalogForHost();

            _contrabandStash = new ContrabandStashSystem(catalog, inv, id => itemCatalog?.Get(id), new GodotLog());

            foreach (var activation in ContrabandStashSystem.DefaultActivations())
            {
                if (!_contrabandStash.TryRegisterActivation(activation))
                    GD.PrintErr($"[Main.Contraband] activation registration failed: {activation.entryId}");
            }

            var saved = ContrabandSaveStore.TryLoad();
            if (saved != null)
            {
                _contrabandStash.RestoreState(saved);
            }

            // Journal is the single feedback strip for claims (deduped per entry).
            _contrabandStash.OnStashClaimed += (entryId, day) =>
            {
                if (_contrabandStash == null) return;
                var activation = _contrabandStash.GetActivation(entryId);
                var entry = catalog.GetById(entryId);
                string title = entry?.Title ?? entryId;
                string itemText = activation?.canonicalItemId ?? "goods";
                if (itemCatalog != null && activation != null)
                {
                    var def = itemCatalog.Get(activation.canonicalItemId);
                    if (def != null) itemText = $"{activation.grantQuantity}× {def.displayName}";
                }
                _journal?.TryAddRawEntry(
                    $"contraband_stash_claimed_{entryId}",
                    $"Cache found and emptied: {title}. Recovered: {itemText}. Keep it quiet.",
                    null!, day);
            };

            return _contrabandStash;
        }

        private void SetupContrabandStash()
        {
            EnsureContrabandStash();
            // Narcotics slice: committed consumptions of dependency-catalog
            // items (morphine, alcohol, …) route to the ChemicalDependencySystem
            // exactly once per event. Safe to call repeatedly (guarded).
            WireDependencyConsumeHook();
        }

        // ── Daily tick: discovery rumors (pure reads + deduped journal) ──

        private void TickContrabandStashDay(int day)
        {
            if (_contrabandStash == null) return;

            // ListDiscoverable is a pure read; TryAddRawEntry dedupes per
            // knowledge key, so this stays once-per-entry for the campaign.
            foreach (var entry in _contrabandStash.ListDiscoverable(day))
            {
                string where = entry.HiddenStashLocation.Replace('_', ' ');
                _journal?.TryAddRawEntry(
                    $"contraband_stash_rumor_{entry.Id}",
                    $"Word travels through the bunker: {entry.Title} is cached {where}. Worth a look — quietly.",
                    null!, day);
            }
        }

        // ── Player command (UI/CLI route; no panel) ──────────────────────

        /// <summary>
        /// Player-facing claim command for an activated, currently discoverable
        /// stash. Feedback arrives via the journal OnStashClaimed entry; the
        /// ActionResult carries the failure code for UI branching.
        /// </summary>
        public ActionResult ClaimContrabandStash(string entryId)
        {
            if (_contrabandStash == null)
                return ActionResult.Failed("contraband_not_ready", "contraband.unknown_entry");
            return _contrabandStash.TryClaimStash(entryId, _simDay);
        }

        // ── Save ─────────────────────────────────────────────────────────

        private void SaveContrabandStash()
        {
            if (_contrabandStash != null)
            {
                CaptureSection(
                    "contraband_stash",
                    ContrabandSaveStore.TryCapturePersisted(_contrabandStash.CaptureState()));
            }
        }

        // ── Barter acquisition route: ShelterBarterSystem (Plan 54 core, Plan 147 host wire) ──

        private ShelterBarterSystem? _shelterBarter;
        private UI.ShelterBarterPanel? _shelterBarterPanel;

        public UI.ShelterBarterPanel EnsureShelterBarterPanel()
        {
            if (_shelterBarterPanel != null) return _shelterBarterPanel;

            _shelterBarterPanel = new UI.ShelterBarterPanel();
            _shelterBarterPanel.Visible = false;
            _shelterBarterPanel.OnClose += () => _shelterBarterPanel.Visible = false;
            AddChild(_shelterBarterPanel);
            return _shelterBarterPanel;
        }

        public void OpenShelterBarterPanel()
        {
            var panel = EnsureShelterBarterPanel();
            var barter = EnsureShelterBarter();
            SetupInventory();
            SetupJournal();
            panel.Bind(
                barter,
                _inventory?.Inventory ?? new Inventory(),
                _journal,
                id => _inventory?.Catalog?.Get(id));
            // Wire appraisal skill. The survivor skill system (SkillProgressionState)
            // is not yet connected to the barter panel; default to 0 until the
            // trade-discipline skill lookup is wired through SurvivorsHostSession.
            panel.SetAppraisalSkill(0);
            panel.Open();
        }

        /// <summary>
        /// The shelter barter host: four legacy Plan-54 caravans plus the
        /// contraband broker (built from the contraband activation map — the
        /// same gate authority as the stash route). Caravan arrivals and
        /// departures surface through the journal; stock is pinned per
        /// arrival and persisted, so reopening any surface cannot reroll it.
        /// </summary>
        public ShelterBarterSystem EnsureShelterBarter()
        {
            if (_shelterBarter != null) return _shelterBarter;

            // Barter trades through the campaign inventory authority; compose it
            // instead of binding a fabricated empty inventory (INV-16.3).
            SetupInventory();
            // D19a determinism contract: When _campaignDay is active, fork deterministically
            // from the campaign RNG; in pre-campaign/offline setup, use a fixed seed (147)
            // so stock generation remains deterministic and never uses unseeded System.Random.
            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("shelter_barter") : new SeededRng(147);
            var inv = _inventory!.Inventory;
            var itemCatalog = _inventory!.Catalog;

            _shelterBarter = new ShelterBarterSystem(
                rng, inv,
                thermalSystem: null, // airlock-freeze gating is a Plan-54 refinement; the broker route does not depend on it
                log: new GodotLog(),
                itemLookup: id => itemCatalog?.Get(id));

            // The contraband broker — registration precedes state restore.
            _shelterBarter.RegisterCaravan(ContrabandBrokerCaravan.Build(
                LoadContrabandCatalogForHost(), ContrabandStashSystem.DefaultActivations()));

            var saved = ShelterBarterSaveStore.TryLoad();
            if (saved != null)
            {
                _shelterBarter.RestoreState(saved);
            }

            _shelterBarter.OnCaravanArrived += caravan =>
            {
                _journal?.TryAddRawEntry(
                    $"shelter_barter_arrival_{caravan.caravan_id}",
                    $"{caravan.name} is at the airlock. {caravan.description}",
                    null!, _simDay);
            };
            _shelterBarter.OnCaravanDeparted += caravan =>
            {
                _journal?.TryAddRawEntry(
                    $"shelter_barter_departure_{caravan.caravan_id}",
                    $"{caravan.name} moved on.",
                    null!, _simDay);
            };

            return _shelterBarter;
        }

        private BunkerContrabandCatalog LoadContrabandCatalogForHost()
        {
            var catalog = new BunkerContrabandCatalog();
            string catalogPath = CatalogPath.ResolveSub("narrative", "bunker_contraband_barter.json");
            var _catalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
            if (_catalogIo.FileExists(catalogPath))
            {
                string json = _catalogIo.ReadAllText(catalogPath);
                {
                    var report = ContrabandCatalogValidator.ValidateJson(json);
                    if (report.IsValid)
                        catalog = BunkerContrabandCatalog.LoadFromJson(json);
                    else
                        GD.PrintErr("[Main.Contraband] catalog validation failed — broker stocks nothing: "
                            + string.Join("; ", report.Errors));
                }
            }
            return catalog;
        }

        private void SetupShelterBarter()
        {
            EnsureShelterBarter();
        }

        private void SaveShelterBarter()
        {
            if (_shelterBarter != null)
            {
                CaptureSection(
                    "shelter_barter",
                    ShelterBarterSaveStore.TryCapturePersisted(_shelterBarter.CaptureState()));
            }
        }

        /// <summary>Daily barter tick: caravan arrivals, departures, restocks (day gates evaluated here).</summary>
        private void TickShelterBarterDay(int day)
        {
            _shelterBarter?.TickDay(day);
        }
    }
}
