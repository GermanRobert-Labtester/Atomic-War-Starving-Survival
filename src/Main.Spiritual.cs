// SPDX-License-Identifier: MIT
using Godot;
using Ashfall.Core;
using Ashfall.Core.Spiritual;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Plan 30 spiritual-meaning host: catalog load, death→mourning arcs,
    /// vigil-rite mapping onto MemorialSystem.Mourn, daily stage ticks, save.
    /// Does not add a faith/piety meter.
    /// </summary>
    public partial class Main
    {
        private SpiritualMeaningCoordinator? _spiritual;
        private bool _spiritualDeathWired;

        public SpiritualMeaningCoordinator? SpiritualCoordinator => _spiritual;

        private void SetupSpiritual()
        {
            if (_spiritual != null)
            {
                WireSpiritualDeathFeed();
                return;
            }

            string dataDir = string.IsNullOrEmpty(_dataDir) ? CatalogPath.ResolveDataDir() : _dataDir;
            var catalog = SpiritualCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            _spiritual = new SpiritualMeaningCoordinator(catalog);

            var saved = SpiritualSaveStore.TryLoad();
            if (saved != null)
                _spiritual.RestoreState(saved);

            WireSpiritualDeathFeed();
            GD.Print("[Ashfall Godot] Spiritual meaning coordinator ready.");
        }

        private void WireSpiritualDeathFeed()
        {
            if (_spiritualDeathWired || _spiritual == null) return;
            SetupSurvivorFate();
            if (_spiritualDeathWired || _survivorFate == null) return;

            _survivorFate.OnSurvivorFate += OnSurvivorFateForSpiritual;
            foreach (var fate in _survivorFate.Fates)
            {
                if (fate == null || string.IsNullOrEmpty(fate.survivorId)) continue;
                _spiritual.RegisterDeath(fate.survivorId, fate.day);
            }
            _spiritualDeathWired = true;
        }

        private void OnSurvivorFateForSpiritual(SurvivorFateEvent fate)
        {
            if (_spiritual == null || fate == null || string.IsNullOrEmpty(fate.survivorId)) return;
            _spiritual.RegisterDeath(fate.survivorId, fate.day);
        }

        private void SaveSpiritual()
        {
            if (_spiritual == null) return;
            CaptureSection("spiritual_meaning", SpiritualSaveStore.TryCapturePersisted(_spiritual.CaptureState()));
        }

        public void TickSpiritualDay(int day)
        {
            SetupSpiritual();
            _spiritual?.TickMourning(day);
        }
    }
}
