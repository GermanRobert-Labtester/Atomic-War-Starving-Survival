// SPDX-License-Identifier: MIT
// PLAN-ECONOMY-DATA-FAMILY-TRUTH-270 — economy family host wiring. The two
// stateful owners (route monopoly, syndicate heat) persist under the
// `economy_family` section; contraband/chit engines are stateless consumers.

using Godot;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private EconomyFamilyHostSession? _economyFamily;
        private bool _economyFamilyDirty;

        public EconomyFamilyHostSession? EconomyFamily => _economyFamily;

        public void SetupEconomyFamily()
        {
            if (_economyFamily != null) return;
            _economyFamily = new EconomyFamilyHostSession();
            var saved = EconomyFamilySaveStore.TryLoad();
            if (saved != null) _economyFamily.RestoreState(saved);
            _economyFamily.StateChanged += () => _economyFamilyDirty = true;
        }

        public void SaveEconomyFamily()
        {
            if (_economyFamily == null) return;
            var state = _economyFamily.CaptureState();
            if (CaptureSection("economy_family", EconomyFamilySaveStore.TryCapturePersisted(state)))
            {
                _economyFamilyDirty = false;
            }
        }

        public void FlushEconomyFamilyIfDirty()
        {
            if (_economyFamilyDirty) SaveEconomyFamily();
        }

        public void ResetEconomyFamily()
        {
            _economyFamily = null;
            _economyFamilyDirty = false;
        }

        public void TickEconomyFamily(int day)
        {
            if (_economyFamily == null) return;
            int seed = _campaignDay != null ? _campaignDay.LastAdvancedDay : day;
            _economyFamily.ProcessRouteRecovery(day);
            _economyFamily.TickHeat(day, seed);
        }
    }
}
