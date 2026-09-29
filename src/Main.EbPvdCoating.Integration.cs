// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.AdvancedMachinery;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Medical;
using Ashfall.Core.Shelter;
using Ashfall.Core.World;
using AtomicWar.GodotApp.UI;
using Godot;
using System;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private EbPvdCoatingHostSession? _ebPvdCoating;

        private ISeededRng? _ebpvdRng;
        private System.Collections.Generic.Dictionary<string, string>? _ebpvdSubstrateResults;

        private EbPvdCoatingPanel? _ebPvdCoatingPanel;

        private void SetupEbPvdCoating()
        {
            if (_ebPvdCoating != null) return;
            SetupCampaignDay();
            var state = EbPvdCoatingSaveStore.TryLoad() ?? new EbPvdCoatingState();
            var engine = new EbPvdCoatingEngine(state);
            LoadEbPvdCatalogInto(engine);
            _ebPvdCoating = new EbPvdCoatingHostSession(engine);
            _ebpvdRng = _campaignDay.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.EbpvdCoating);
            // A finished coating mints the authored result item into inventory
            // (catalog substrate_classes mapping — engine records stay machine-local).
            _ebPvdCoating.System.OnJobCompleted += record =>
            {
                if (_ebpvdSubstrateResults != null &&
                    _ebpvdSubstrateResults.TryGetValue(record.SubstrateTag, out var resultItemId) &&
                    _inventory != null)
                {
                    _inventory.Inventory.AddById(resultItemId, 1);
                }
            };
        }

        private void LoadEbPvdCatalogInto(EbPvdCoatingEngine engine)
        {
            try
            {
                string dataDir = ResolvePlans146DataDir();
                var fileIO = CatalogPath.CreateFileIOForDataDir(dataDir);
                var catalog = EbPvdCoatingCatalogLoader.Load(dataDir, fileIO, new SystemTextJsonSerializer());
                if (catalog.coatings.Count > 0)
                {
                    foreach (var def in catalog.ToCoatingDefs())
                    {
                        engine.RegisterCoating(def);
                    }
                    engine.SetFailureProfiles(catalog.ToFailureProfiles());
                }
                if (catalog.substrate_classes.Count > 0)
                {
                    _ebpvdSubstrateResults = new System.Collections.Generic.Dictionary<string, string>(StringComparer.Ordinal);
                    foreach (var s in catalog.substrate_classes)
                    {
                        if (!string.IsNullOrEmpty(s.tag) && !string.IsNullOrEmpty(s.result_item_id))
                        {
                            _ebpvdSubstrateResults[s.tag] = s.result_item_id;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[Plans146] ebpvd_coating_catalog.json load failed; engine seeds in force: {ex.Message}");
            }
        }

        private void SaveEbPvdCoating()
        {
            if (_ebPvdCoating != null)
                CaptureSection("ebpvd_coating", EbPvdCoatingSaveStore.TryCapturePersisted(_ebPvdCoating.System.CaptureState()));
        }

        private void HandleEbPvdCoatingAction(string action, string param = "")
        {
            SetupEbPvdCoating();
            EnsurePlans146To149Panels();
            if (_ebPvdCoating == null) return;

            if (string.Equals(action, "CLOSE", StringComparison.OrdinalIgnoreCase))
            {
                if (_ebPvdCoatingPanel != null) _ebPvdCoatingPanel.Visible = false;
                return;
            }

            if (string.Equals(action, "OPEN", StringComparison.OrdinalIgnoreCase))
            {
                if (_ebPvdCoatingPanel != null)
                {
                    ShowPanelLifecycle(_ebPvdCoatingPanel);
                    _ebPvdCoatingPanel.RefreshView();
                }
                return;
            }

            if (string.Equals(action, "start_coating", StringComparison.OrdinalIgnoreCase))
            {
                // param: coatingId|substrateTag[|bond]
                string coatingId = param ?? string.Empty;
                string substrateTag = "superalloy_blade";
                bool applyBond = true;
                if (!string.IsNullOrEmpty(param))
                {
                    var parts = param.Split('|');
                    if (parts.Length >= 1 && !string.IsNullOrEmpty(parts[0])) coatingId = parts[0];
                    if (parts.Length >= 2 && !string.IsNullOrEmpty(parts[1])) substrateTag = parts[1];
                    if (parts.Length >= 3)
                        applyBond = !string.Equals(parts[2], "nobond", StringComparison.OrdinalIgnoreCase);
                }
                if (string.IsNullOrEmpty(coatingId))
                    coatingId = "ebpvd_tbc_yttria_stabilized_zirconia";

                SetupInventory();
                string jobId = $"ebpvd_{_simDay}_{coatingId}";
                bool ok = _ebPvdCoating.StartCoatingJob(
                    jobId, coatingId, substrateTag, ResolvePlans146OperatorId(),
                    _simDay > 0 ? _simDay : 1, applyBond, TryConsumePlans146Demands, out string reason);
                _ebPvdCoatingPanel?.ShowFeedback(
                    ok ? $"Coating started: {coatingId} on {substrateTag}."
                       : $"Cannot start coating: {reason}",
                    !ok);
            }
            else if (string.Equals(action, "install_coated_part", StringComparison.OrdinalIgnoreCase))
            {
                // Plans 146–149 MED: mint ≠ install. Consume inventory then
                // publish watts into PowerGrid under ebpvd_installed.
                string itemId = string.IsNullOrEmpty(param) ? string.Empty : param.Trim();
                if (string.IsNullOrEmpty(itemId))
                {
                    _ebPvdCoatingPanel?.ShowFeedback("Cannot install: missing_item.", true);
                }
                else if (string.IsNullOrEmpty(Ashfall.Core.Shelter.PowerGridSystem.ResolveCoatedPartFamily(itemId)))
                {
                    _ebPvdCoatingPanel?.ShowFeedback($"Cannot install: unsupported_coated_part ({itemId}).", true);
                }
                else
                {
                    SetupInventory();
                    SetupPowerGrid();
                    if (_inventory == null || !_inventory.Inventory.TryConsumeById(itemId, 1))
                    {
                        _ebPvdCoatingPanel?.ShowFeedback($"Cannot install: missing inventory for {itemId}.", true);
                    }
                    else
                    {
                        string installReason = "power_grid_unavailable";
                        bool installed = _powerGrid != null
                            && _powerGrid.TryInstallCoatedPart(itemId, out installReason);
                        if (!installed)
                        {
                            // Refund the consumed part if the grid rejected install.
                            _inventory.Inventory.AddById(itemId, 1);
                            _ebPvdCoatingPanel?.ShowFeedback(
                                $"Cannot install: {installReason}.", true);
                        }
                        else
                        {
                            float watts = Ashfall.Core.Shelter.PowerGridSystem.ResolveCoatedPartWatts(itemId);
                            _ebPvdCoatingPanel?.ShowFeedback(
                                $"Installed {itemId} into generator (+{watts:F0} W).", false);
                        }
                    }
                }
            }
            else if (string.Equals(action, "maintain", StringComparison.OrdinalIgnoreCase))
            {
                string maint = string.IsNullOrEmpty(param) ? "replace_filament" : param;
                _ebPvdCoating.PerformMaintenance(maint);
                _ebPvdCoatingPanel?.ShowFeedback($"Maintenance complete: {maint}.", false);
            }

            _ebPvdCoatingPanel?.RefreshView();
        }

    }
}
