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
        private MineClearingFlailHostSession? _mineClearingFlail;
        private ISeededRng? _mineFlailRng;
        private MineFlailPanel? _mineFlailPanel;

        private void SetupMineClearingFlail()
        {
            if (_mineClearingFlail != null) return;
            SetupRouteInfrastructure();
            SetupCampaignDay();
            var state = MineClearingFlailSaveStore.TryLoad() ?? new MineClearingFlailState();
            var engine = new MineClearingFlailEngine(state);
            LoadMineFlailCatalogInto(engine);
            _mineClearingFlail = new MineClearingFlailHostSession(engine);
            _mineClearingFlail.Routes = _routeInfrastructure;
            _mineFlailRng = _campaignDay.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.MineClearingFlail);
        }

        private void LoadMineFlailCatalogInto(MineClearingFlailEngine engine)
        {
            try
            {
                string dataDir = ResolvePlans146DataDir();
                var fileIO = CatalogPath.CreateFileIOForDataDir(dataDir);
                var catalog = MineFlailCatalogLoader.Load(dataDir, fileIO, new SystemTextJsonSerializer());
                foreach (var def in catalog.ToModuleDefs())
                {
                    engine.RegisterModule(def);
                }
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[Plans147] mine_flail_catalog.json load failed; engine seeds in force: {ex.Message}");
            }
        }

        private void SaveMineClearingFlail()
        {
            if (_mineClearingFlail != null)
                CaptureSection("mine_clearing_flail", MineClearingFlailSaveStore.TryCapturePersisted(_mineClearingFlail.System.CaptureState()));
        }

        private void HandleMineFlailAction(string action, string param = "")
        {
            SetupMineClearingFlail();
            EnsurePlans146To149Panels();
            if (_mineClearingFlail == null) return;
            if (_mineClearingFlail.Routes == null)
                _mineClearingFlail.Routes = _routeInfrastructure;

            if (string.Equals(action, "CLOSE", StringComparison.OrdinalIgnoreCase))
            {
                if (_mineFlailPanel != null) _mineFlailPanel.Visible = false;
                return;
            }

            if (string.Equals(action, "OPEN", StringComparison.OrdinalIgnoreCase))
            {
                if (_mineFlailPanel != null)
                {
                    _mineFlailPanel.Visible = true;
                    _mineFlailPanel.RefreshView();
                }
                return;
            }

            if (string.Equals(action, "start_breach", StringComparison.OrdinalIgnoreCase))
            {
                SetupRouteInfrastructure();
                string routeId = "expedition_corridor_north";
                string segmentId = "seg_mine_gap";
                if (TrySplitPlans146Param(param, out string left, out string right))
                {
                    routeId = left;
                    segmentId = right;
                }
                const float defaultLengthMeters = 1200f;
                bool ok = _mineClearingFlail.StartBreach(
                    routeId, segmentId, ResolvePlans146OperatorId(),
                    _simDay > 0 ? _simDay : 1, defaultLengthMeters, _routeInfrastructure!, out string reason);
                _mineFlailPanel?.ShowFeedback(
                    ok ? $"Breach started on {routeId}:{segmentId}."
                       : $"Cannot start breach: {reason}",
                    !ok);
            }
            else if (string.Equals(action, "maintain", StringComparison.OrdinalIgnoreCase))
            {
                string maint = string.IsNullOrEmpty(param) ? "replace_chains" : param;
                _mineClearingFlail.PerformMaintenance(maint);
                _mineFlailPanel?.ShowFeedback($"Flail serviced: {maint}.", false);
            }

            _mineFlailPanel?.RefreshView();
        }

    }
}
