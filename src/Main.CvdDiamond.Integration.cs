// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Combat;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Random;
using Ashfall.Core.Shelter;
using AtomicWar.GodotApp.UI;
using Godot;
using System;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private CvdDiamondHostSession? _cvdDiamond;
        private CvdDiamondPanel? _cvdPanel;
        public CvdDiamondHostSession? CvdDiamondSession => _cvdDiamond;

        private void SetupCvdDiamond()
        {
            if (_cvdDiamond != null) return;

            var catalog = CvdDiamondCatalogLoader.Load(_dataDir, new FileSystemIO());
            var system = new CvdDiamondSynthesisEngine(catalog, new GodotLog());
            if (_campaignDay?.Rng != null)
                system.Rng = _campaignDay.Rng.Fork(CampaignStreamIds.CvdDiamond);

            _cvdDiamond = new CvdDiamondHostSession(system)
            {
                PowerAvailableProvider = () => _powerGrid?.System == null || !_powerGrid.System.IsBrownout,
                CoolingAvailableProvider = () => true,
                FeedstockAvailableProvider = _ => true,
                SubstrateItemAvailableProvider = _ => true,
                OperatorSkillProvider = () => 0f,
                RepairPartsAvailableProvider = () => true
            };
            // Register the authored high-wear consumers (plan §6.8–6.9).
            _cvdDiamond.System.RegisterConsumer("consumer_deep_excavation_cutter");
            _cvdDiamond.System.RegisterConsumer("consumer_precision_lathe_insert");

            var saved = CvdDiamondSaveStore.TryLoad();
            if (saved != null)
            {
                _cvdDiamond.RestoreSave(saved);
                GD.Print("[Ashfall Godot] CVD diamond reactor state restored.");
            }
        }

        private void SaveCvdDiamond()
        {
            if (_cvdDiamond == null) return;
            CaptureSection(
                CvdDiamondSaveStore.SectionName,
                CvdDiamondSaveStore.TryCapturePersisted(_cvdDiamond.CaptureSave()));
        }

        private void OpenCvdDiamondPanel()
        {
            SetupCvdDiamond();
            EnsurePlans122to125Panels();
            if (_cvdPanel != null) { ShowPanelLifecycle(_cvdPanel); _cvdPanel.RefreshView(); }
        }

    }
}
