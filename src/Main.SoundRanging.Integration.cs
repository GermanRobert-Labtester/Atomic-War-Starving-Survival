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
        private SoundRangingHostSession? _soundRanging;
        private SoundRangingPanel? _soundRangingPanel;
        public SoundRangingHostSession? SoundRangingSession => _soundRanging;

        private void SetupSoundRanging()
        {
            if (_soundRanging != null) return;

            var catalog = SoundRangingCatalogLoader.Load(_dataDir, new FileSystemIO());
            var system = new SoundRangingThreatEngine(catalog, new GodotLog());
            if (_campaignDay?.Rng != null)
                system.Rng = _campaignDay.Rng.Fork(CampaignStreamIds.SoundRanging);

            _soundRanging = new SoundRangingHostSession(system)
            {
                AtmosphericProfileProvider = () => "atmos_clear_cold",
                SensorNodeOperationalProvider = _ => true
            };

            var saved = SoundRangingSaveStore.TryLoad();
            if (saved != null)
            {
                _soundRanging.RestoreSave(saved);
                GD.Print("[Ashfall Godot] Sound-ranging station state restored.");
            }
        }

        private void SaveSoundRanging()
        {
            if (_soundRanging == null) return;
            CaptureSection(
                SoundRangingSaveStore.SectionName,
                SoundRangingSaveStore.TryCapturePersisted(_soundRanging.CaptureSave()));
        }

        private void OpenSoundRangingPanel()
        {
            SetupSoundRanging();
            EnsurePlans122to125Panels();
            if (_soundRangingPanel != null) { _soundRangingPanel.Visible = true; _soundRangingPanel.RefreshView(); }
        }

    }
}
