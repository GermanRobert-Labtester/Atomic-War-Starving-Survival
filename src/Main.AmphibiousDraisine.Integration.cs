// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.AdvancedMachinery;
using Ashfall.Core.Combat;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Medical;
using Ashfall.Core.Random;
using Ashfall.Core.Shelter;
using Ashfall.Core.World;
using AtomicWar.GodotApp.UI;
using Godot;
using System;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private AmphibiousDraisineHostSession? _amphibiousDraisine;
        private AmphibiousDraisinePanel? _amphibiousPanel;
        public AmphibiousDraisineHostSession? AmphibiousDraisineSession => _amphibiousDraisine;

        private void SetupAmphibiousDraisine()
        {
            if (_amphibiousDraisine != null) return;

            var catalog = AmphibiousDraisineCatalogLoader.Load(_dataDir, new FileSystemIO());
            var system = new AmphibiousDraisineEngine(catalog, new GodotLog());
            if (_campaignDay?.Rng != null)
                system.Rng = _campaignDay.Rng.Fork(CampaignStreamIds.AmphibiousDraisine);

            _amphibiousDraisine = new AmphibiousDraisineHostSession(system)
            {
                VehicleStateProvider = id => (string.Empty, 10000),
                WorkshopAvailableProvider = () => true,
                PartsAvailableProvider = _ => true,
                MechanicSkillProvider = () => 0f,
                PumpPowerAvailableProvider = () => _powerGrid?.System == null || !_powerGrid.System.IsBrownout
            };

            var saved = AmphibiousDraisineSaveStore.TryLoad();
            if (saved != null)
            {
                _amphibiousDraisine.RestoreSave(saved);
                GD.Print("[Ashfall Godot] Amphibious kit state restored.");
            }
        }

        private void SaveAmphibiousDraisine()
        {
            if (_amphibiousDraisine == null) return;
            CaptureSection(
                AmphibiousDraisineSaveStore.SectionName,
                AmphibiousDraisineSaveStore.TryCapturePersisted(_amphibiousDraisine.CaptureSave()));
        }

        private void OpenAmphibiousDraisinePanel()
        {
            SetupAmphibiousDraisine();
            EnsurePlans122to125Panels();
            if (_amphibiousPanel != null) { _amphibiousPanel.Visible = true; _amphibiousPanel.RefreshView(); }
        }


    }
}
