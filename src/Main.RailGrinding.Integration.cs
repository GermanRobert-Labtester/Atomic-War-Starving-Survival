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
        private RailGrindingHostSession? _railGrinding;
        private ISeededRng? _railGrindingRng;
        private RailGrindingPanel? _railGrindingPanel;

        private void SetupRailGrinding()
        {
            if (_railGrinding != null) return;
            SetupRouteInfrastructure();
            SetupCampaignDay();
            var state = RailGrindingSaveStore.TryLoad() ?? new RailGrindingEngineState();
            var engine = new RailGrindingEngine(state);
            LoadRailGrindingCatalogInto(engine);
            _railGrinding = new RailGrindingHostSession(engine);
            _railGrinding.Routes = _routeInfrastructure;
            _railGrindingRng = _campaignDay.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.RailGrinding);
        }

        private void LoadRailGrindingCatalogInto(RailGrindingEngine engine)
        {
            try
            {
                string dataDir = ResolvePlans146DataDir();
                var fileIO = CatalogPath.CreateFileIOForDataDir(dataDir);
                var catalog = RailGrindingCatalogLoader.Load(dataDir, fileIO, new SystemTextJsonSerializer());
                foreach (var def in catalog.ToHeadDefs())
                {
                    engine.RegisterHead(def);
                }
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[Plans149] rail_grinding_catalog.json load failed; engine seeds in force: {ex.Message}");
            }
        }

        private void SaveRailGrinding()
        {
            if (_railGrinding != null)
                CaptureSection("rail_grinding", RailGrindingSaveStore.TryCapturePersisted(_railGrinding.System.CaptureState()));
        }

        private void HandleRailGrindingAction(string action, string param = "")
        {
            SetupRailGrinding();
            EnsurePlans146To149Panels();
            if (_railGrinding == null) return;
            if (_railGrinding.Routes == null)
                _railGrinding.Routes = _routeInfrastructure;

            if (string.Equals(action, "CLOSE", StringComparison.OrdinalIgnoreCase))
            {
                if (_railGrindingPanel != null) _railGrindingPanel.Visible = false;
                return;
            }

            if (string.Equals(action, "OPEN", StringComparison.OrdinalIgnoreCase))
            {
                if (_railGrindingPanel != null)
                {
                    _railGrindingPanel.Visible = true;
                    _railGrindingPanel.RefreshView();
                }
                return;
            }

            if (string.Equals(action, "start_grind", StringComparison.OrdinalIgnoreCase))
            {
                SetupRouteInfrastructure();
                string routeId = "rail_trunk_iron_vein";
                string segmentId = "sector_deep_quarry";
                if (TrySplitPlans146Param(param, out string left, out string right))
                {
                    routeId = left;
                    segmentId = right;
                }
                const float defaultLengthKm = 8f;
                bool ok = _railGrinding.StartGrindingJob(
                    routeId, segmentId, ResolvePlans146OperatorId(),
                    defaultLengthKm, _routeInfrastructure!, out string reason);
                _railGrindingPanel?.ShowFeedback(
                    ok ? $"Grinding started on {routeId}:{segmentId}."
                       : $"Cannot start grinding: {reason}",
                    !ok);
            }
            else if (string.Equals(action, "maintain", StringComparison.OrdinalIgnoreCase))
            {
                string maint = string.IsNullOrEmpty(param) ? "replace_stones" : param;
                _railGrinding.PerformMaintenance(maint);
                _railGrindingPanel?.ShowFeedback($"Grinder serviced: {maint}.", false);
            }

            _railGrindingPanel?.RefreshView();
        }

    }
}
