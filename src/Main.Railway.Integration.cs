// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Archaeology;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Farming;
using Ashfall.Core.Medical;
using Ashfall.Core.Narrative;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private RailwaySystem? _railway;

        // ── Plan 191: Railways & Armored Trains ──────────────────────────

        public RailwaySystem EnsureRailway()
        {
            if (_railway != null) return _railway;

            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("railway") : new SeededRng(191);
            var inv = _inventory?.Inventory ?? new Ashfall.Core.Inventory.Inventory();

            _railway = new RailwaySystem(rng, inv, new GodotLog());

            string catalogPath = CatalogPath.ResolveCatalog("rail_network.json");
            var _catalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
            if (_catalogIo.FileExists(catalogPath))
            {
                string json = _catalogIo.ReadAllText(catalogPath);
                {
                    try
                    {
                        var catalog = System.Text.Json.JsonSerializer.Deserialize<RailwayNetworkCatalog>(json);
                        if (catalog != null)
                        {
                            _railway.RegisterCatalog(catalog);
                        }
                    }
                    catch (Exception ex)
                    {
                        GD.PrintErr($"[Main.Railway] Failed to parse {catalogPath}: {ex.Message}");
                    }
                }
            }

            string logisticsPath = CatalogPath.ResolveCatalog("rail_logistics_catalog.json");
            if (_catalogIo.FileExists(logisticsPath))
            {
                string json = _catalogIo.ReadAllText(logisticsPath);
                {
                    try
                    {
                        var container = System.Text.Json.JsonSerializer.Deserialize<RailLogisticsCatalogContainer>(json);
                        if (container != null && container.edges != null)
                        {
                            _railway.RegisterLogisticsCatalog(container.edges);
                        }
                    }
                    catch (Exception ex)
                    {
                        GD.PrintErr($"[Main.Railway] Failed to parse {logisticsPath}: {ex.Message}");
                    }
                }
            }

            var saved = RailwaySaveStore.TryLoad();
            if (saved != null)
            {
                _railway.RestoreState(saved);
            }

            _railway.OnTrainDispatched += (trainId, segmentId) =>
            {
                _journal?.TryAddRawEntry("train_dispatched", $"Armored train {trainId} departed onto rail segment {segmentId}.", null!, _simDay);
                RecordRailRunFromTrain(_railway, trainId, segmentId);
            };

            _railway.OnDerailment += (trainId, segmentId) =>
            {
                _journal?.TryAddRawEntry("train_derailment", $"Disaster! Train {trainId} derailed on degraded rail segment {segmentId}!", null!, _simDay);
            };

            _railway.OnTrainAmbushed += (trainId, segmentId) =>
            {
                _journal?.TryAddRawEntry("train_ambush", $"Train {trainId} came under heavy raider fire on segment {segmentId}!", null!, _simDay);
            };

            return _railway;
        }

        private void SetupRailway()
        {
            EnsureRailway();
        }

        private void SaveRailway()
        {
            if (_railway != null)
            {
                CaptureSection("railway", RailwaySaveStore.TryCapturePersisted(_railway.CaptureState()));
            }
        }

        private void HandleRailwayAction(string action, string param = "")
        {
            if (action == "CLOSE") { CloseRailwayTerminalPanel(); return; }
            if (_railwayTerminalPanel == null || _railway == null) return;

            Ashfall.Core.ActionResult? res = action switch
            {
                "repair_track" => _railway.RepairTrack(param, integrityRestored: 0.25f),
                "repair_bridge" => _railway.RepairBridge(param),
                "clear_obstacle" => _railway.ClearTrackObstacle(param),
                "clear_derailment" => _railway.ClearDerailment(param),
                "service" => _railway.ServiceTransmission(param),
                _ => null
            };

            if (res != null)
                _railwayTerminalPanel.ShowFeedback(
                    res.Value.IsSuccess ? "Done. The line is one step closer to running."
                                  : "The crew couldn't do it — check what the terminal says is missing.",
                    !res.Value.IsSuccess);
            _railwayTerminalPanel.RefreshView();
        }
        private void CloseRailwayTerminalPanel() { _railwayTerminalPanel?.Visible = false; }

    }
}
