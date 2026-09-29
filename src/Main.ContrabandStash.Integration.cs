// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Economy;
using Ashfall.Core.Inventory;
using Ashfall.Core.Medical;
using Ashfall.Core.Narrative;
using Godot;
using System;
using System.Collections.Generic;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private ContrabandStashSystem? _contrabandStash;

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

    }
}
