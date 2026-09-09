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

            string catalogPath = "res://Assets/StreamingAssets/Data/chemical_dependency_items.json";
            if (!Godot.FileAccess.FileExists(catalogPath)) return null;
            using var file = Godot.FileAccess.Open(catalogPath, Godot.FileAccess.ModeFlags.Read);
            if (file == null) return null;

            try
            {
                using var doc = System.Text.Json.JsonDocument.Parse(file.GetAsText());
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

            var catalog = new BunkerContrabandCatalog();
            string catalogPath = "res://Assets/StreamingAssets/Data/narrative/bunker_contraband_barter.json";
            if (Godot.FileAccess.FileExists(catalogPath))
            {
                using var file = Godot.FileAccess.Open(catalogPath, Godot.FileAccess.ModeFlags.Read);
                if (file != null)
                {
                    string json = file.GetAsText();

                    // Fail closed: an invalid catalog (unknown mechanics key,
                    // bad tier/price, NaN, duplicate ids) leaves the contraband
                    // layer inert rather than partially trusted.
                    var report = ContrabandCatalogValidator.ValidateJson(json);
                    if (!report.IsValid)
                    {
                        GD.PrintErr(
                            "[Main.Contraband] catalog validation failed — contraband layer stays inert: "
                            + string.Join("; ", report.Errors));
                    }
                    else
                    {
                        catalog = BunkerContrabandCatalog.LoadFromJson(json);
                    }
                }
            }
            else
            {
                GD.PrintErr("[Main.Contraband] catalog file missing — contraband layer stays inert.");
            }

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
    }
}
