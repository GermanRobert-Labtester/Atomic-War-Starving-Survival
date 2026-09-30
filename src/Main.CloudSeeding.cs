// SPDX-License-Identifier: MIT
// PLAN-WEATHER-ATMOSPHERE-28 (cloud-seeding package) — host wiring for the
// cloud-seeding instrument. Binds the canonical weather owner + inventory and
// persists install/cooldown state under its own `cloud_seeding` section.

using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private CloudSeedingHostSession? _cloudSeeding;
        private bool _cloudSeedingDirty;

        public CloudSeedingHostSession? CloudSeeding => _cloudSeeding;

        public void SetupCloudSeeding()
        {
            if (_cloudSeeding != null) return;
            _cloudSeeding = new CloudSeedingHostSession();
            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("cloud_seeding") : new Ashfall.Core.SeededRng(28);
            _cloudSeeding.Bind(_weatherSondeHost.System, _inventory?.Inventory, rng);
            var saved = CloudSeedingSaveStore.TryLoad();
            if (saved != null) _cloudSeeding.RestoreState(saved);
            _cloudSeeding.StateChanged += () => _cloudSeedingDirty = true;
        }

        public void SaveCloudSeeding()
        {
            if (_cloudSeeding == null || _cloudSeeding.Engine == null) return;
            var state = _cloudSeeding.CaptureState();
            if (state == null) return;
            if (CaptureSection("cloud_seeding", CloudSeedingSaveStore.TryCapturePersisted(state)))
            {
                _cloudSeedingDirty = false;
            }
        }

        public void FlushCloudSeedingIfDirty()
        {
            if (_cloudSeedingDirty) SaveCloudSeeding();
        }

        public void ResetCloudSeeding()
        {
            _cloudSeeding = null;
            _cloudSeedingDirty = false;
        }

        public void TickCloudSeeding(int day)
        {
            _cloudSeeding?.TickDay(day);
        }
    }
}
