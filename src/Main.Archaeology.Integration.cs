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
        private ArchaeologySystem? _archaeology;

        // ── Plan 189: Archaeology & Lore Excavation ───────────────────────

        public ArchaeologySystem EnsureArchaeology()
        {
            if (_archaeology != null) return _archaeology;

            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("archaeology") : new SeededRng(189);
            var inv = _inventory?.Inventory ?? new Ashfall.Core.Inventory.Inventory();
            var research = EnsureSharedResearch();

            _archaeology = new ArchaeologySystem(rng, inv, research, new GodotLog());

            string catalogPath = CatalogPath.ResolveCatalog("lore_archives.json");
            var _catalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
            if (_catalogIo.FileExists(catalogPath))
            {
                string json = _catalogIo.ReadAllText(catalogPath);
                {
                    try
                    {
                        var container = System.Text.Json.JsonSerializer.Deserialize<ArchaeologyCatalogContainer>(json);
                        if (container?.archives != null)
                        {
                            foreach (var a in container.archives)
                                _archaeology.RegisterArchive(a);
                        }
                    }
                    catch (Exception ex)
                    {
                        GD.PrintErr($"[Main.Archaeology] Failed to parse {catalogPath}: {ex.Message}");
                    }
                }
            }

            var saved = ArchaeologySaveStore.TryLoad();
            if (saved != null)
            {
                _archaeology.RestoreState(saved);
            }

            _archaeology.OnLoreUnlocked += (archive, points) =>
            {
                _journal?.TryAddRawEntry("lore_unlocked", $"Pre-war archive '{archive.titleKey}' successfully decrypted (+{points} R&D).", null!, _simDay);
            };

            return _archaeology;
        }

        private void SetupArchaeology()
        {
            EnsureArchaeology();
        }

        private void SaveArchaeology()
        {
            if (_archaeology != null)
            {
                CaptureSection("archaeology", ArchaeologySaveStore.TryCapturePersisted(_archaeology.CaptureState()));
            }
        }

        private void HandleArchaeologyAction(string action, string param = "")
        {
            if (action == "CLOSE") { CloseArchaeologyExcavationPanel(); return; }
            if (_archaeologyExcavationPanel == null || _archaeology == null) return;

            switch (action)
            {
                case "decrypt":
                {
                    var res = _archaeology.ProgressDecryption(param, hours: 8f, engineerSkill: 0.5f, hasPower: true);
                    _archaeologyExcavationPanel.ShowFeedback(
                        res.IsSuccess ? "The decryption shift worked through the cipher layer."
                                      : "The shift made no headway — higher tiers need power or a keycard.",
                        !res.IsSuccess);
                    break;
                }
                case "sell":
                {
                    var res = _archaeology.SellArchiveToBroker(param);
                    _archaeologyExcavationPanel.ShowFeedback(
                        res.IsSuccess ? "The broker took the archive and paid in kind."
                                      : "The broker refused the archive.",
                        !res.IsSuccess);
                    break;
                }
            }
            _archaeologyExcavationPanel.RefreshView();
        }
        private void CloseArchaeologyExcavationPanel() { _archaeologyExcavationPanel?.Visible = false; }

    }
}
