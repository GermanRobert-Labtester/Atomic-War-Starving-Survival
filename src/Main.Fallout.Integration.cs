// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.AdvancedMachinery;
using Ashfall.Core.Archaeology;
using Ashfall.Core.Economy;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Farming;
using Ashfall.Core.Medical;
using Ashfall.Core.Narrative;
using Ashfall.Core.Shelter;
using Ashfall.Core.Survivors;
using Ashfall.Core.World;
using AtomicWar.GodotApp.UI;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {

        private FalloutSystem? _fallout;

        // ── Plan 186: Radioactive Fallout & Wind Dispersal ────────────────

        public FalloutSystem EnsureFallout()
        {
            if (_fallout != null) return _fallout;

            _fallout = new FalloutSystem(new GodotLog());

            string catalogPath = CatalogPath.ResolveCatalog("fallout_patterns.json");
            var _catalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
            if (_catalogIo.FileExists(catalogPath))
            {
                string json = _catalogIo.ReadAllText(catalogPath);
                {
                    try
                    {
                        var container = System.Text.Json.JsonSerializer.Deserialize<FalloutCatalogContainer>(json);
                        if (container?.patterns != null)
                        {
                            foreach (var p in container.patterns)
                                _fallout.RegisterPattern(p);
                        }
                    }
                    catch (Exception ex)
                    {
                        GD.PrintErr($"[Main.Fallout] Failed to parse {catalogPath}: {ex.Message}");
                    }
                }
            }

            var saved = FalloutSaveStore.TryLoad();
            if (saved != null)
            {
                _fallout.RestoreState(saved);
            }

            _fallout.OnFalloutWarning += (cloud, zone, dist) =>
            {
                _journal?.TryAddRawEntry("fallout_warning", $"Radioactive fallout warning: {cloud.patternId} approaching {zone} (Distance: {dist:F1}km).", null!, _simDay);
            };

            _fallout.OnGroundwaterTainted += (zone) =>
            {
                _journal?.TryAddRawEntry("groundwater_taint", $"Prolonged fallout deposit has contaminated water table at {zone}!", null!, _simDay);
            };

            return _fallout;
        }

        private void SetupFallout()
        {
            EnsureFallout();
        }

        private void SaveFallout()
        {
            if (_fallout != null)
            {
                CaptureSection("fallout", FalloutSaveStore.TryCapturePersisted(_fallout.CaptureState()));
            }
        }

        private void HandleFalloutAction(string action, string param = "")
        {
            if (action == "CLOSE") { _falloutPlumePanel.Visible = false; return; }
            if (_falloutPlumePanel == null || _fallout == null) return;

            switch (action)
            {
                case "seal_shelter":
                {
                    float hours = float.TryParse(param, out var h) ? h : 48f;
                    bool sealedOk = _fallout.SealShelter(hours);
                    _falloutPlumePanel.SetFeedback(
                        sealedOk ? "Airlock seal engaged. Outside air stays outside for as long as the seal holds."
                                 : "The seal could not be engaged.",
                        !sealedOk);
                    break;
                }
            }
            _falloutPlumePanel.RefreshView();
        }

    }
}
