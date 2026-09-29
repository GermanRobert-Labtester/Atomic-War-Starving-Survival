// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Archaeology;
using Ashfall.Core.Economy;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Farming;
using Ashfall.Core.Medical;
using Ashfall.Core.Narrative;
using Ashfall.Core.Survivors;
using Ashfall.Core.World;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private DesperationSystem? _desperation;

        // ── Plan 187: Cannibalism & Desperation Mechanics ─────────────────

        public DesperationSystem EnsureDesperation()
        {
            if (_desperation != null) return _desperation;

            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("desperation") : new SeededRng(187);
            var inv = _inventory?.Inventory ?? new Ashfall.Core.Inventory.Inventory();
            var needs = _survivors?.Needs ?? new Ashfall.Core.Survivors.NeedsSystem();
            var disease = _disease?.Engine;

            _desperation = new DesperationSystem(rng, inv, needs, disease, new GodotLog());

            string catalogPath = CatalogPath.ResolveCatalog("desperation_events.json");
            var _catalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
            if (_catalogIo.FileExists(catalogPath))
            {
                string json = _catalogIo.ReadAllText(catalogPath);
                {
                    try
                    {
                        var container = System.Text.Json.JsonSerializer.Deserialize<DesperationCatalogContainer>(json);
                        if (container?.events != null)
                        {
                            foreach (var ev in container.events)
                                _desperation.RegisterEvent(ev);
                        }
                    }
                    catch (Exception ex)
                    {
                        GD.PrintErr($"[Main.Desperation] Failed to parse {catalogPath}: {ex.Message}");
                    }
                }
            }

            var saved = DesperationSaveStore.TryLoad();
            if (saved != null)
            {
                _desperation.RestoreState(saved);
            }

            _desperation.OnTabooBroken += (record) =>
            {
                _journal?.TryAddRawEntry("taboo_broken", $"SURVIVAL TABOO BROKEN: Dweller {record.actorId} harvested fallen dweller {record.corpseId}.", null!, _simDay);
            };

            return _desperation;
        }

        private void SetupDesperation()
        {
            EnsureDesperation();
        }

        private void SaveDesperation()
        {
            if (_desperation != null)
            {
                CaptureSection("desperation", DesperationSaveStore.TryCapturePersisted(_desperation.CaptureState()));
            }
        }
        private void CloseDesperationCrisisPanel() { _desperationCrisisPanel?.Visible = false; }

        // ── Plans 186/187: desperation + fallout console commands ──────────

        private void HandleDesperationAction(string action, string param = "")
        {
            if (action == "CLOSE") { CloseDesperationCrisisPanel(); return; }
            if (_desperationCrisisPanel == null || _desperation == null) return;

            switch (action)
            {
                case "harvest_corpse":
                {
                    // Actor resolves from the canonical roster authority.
                    string actorId = _survivors?.RosterState.FirstOrDefault(s => !string.IsNullOrEmpty(s.Id))?.Id ?? string.Empty;
                    var res = _desperation.HarvestCorpse(actorId, param, "desperation_consume_corpse", _simDay);
                    _desperationCrisisPanel.ShowFeedback(
                        res.IsSuccess ? "The unthinkable is done. The shelter eats; nothing is the same."
                                      : "The crisis threshold has not been reached — the act is not yet on the table.",
                        !res.IsSuccess);
                    break;
                }
                case "bury_corpse":
                {
                    var res = _desperation.PerformBurial(param);
                    _desperationCrisisPanel.ShowFeedback(
                        res.IsSuccess ? "The dead are laid to rest with what dignity remains."
                                      : "Nothing to bury.",
                        !res.IsSuccess);
                    break;
                }
            }
            _desperationCrisisPanel.RefreshView();
        }

    }
}
