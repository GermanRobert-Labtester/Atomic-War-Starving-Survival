// SPDX-License-Identifier: MIT
// PLAN-PRESERVATION-TRUTH-118 — oilseed press host wiring.

using Ashfall.Core.Farming;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private OilseedPressingHostSession? _oilseedPressing;
        private bool _oilseedPressingDirty;

        public OilseedPressingHostSession? OilseedPressing => _oilseedPressing;

        public void SetupOilseedPressing()
        {
            if (_oilseedPressing != null) return;
            _oilseedPressing = new OilseedPressingHostSession();
            var saved = OilseedPressingSaveStore.TryLoad();
            if (saved != null) _oilseedPressing.RestoreState(saved);
            _oilseedPressing.StateChanged += () => _oilseedPressingDirty = true;
        }

        public void SaveOilseedPressing()
        {
            if (_oilseedPressing == null) return;
            var state = _oilseedPressing.CaptureState();
            OilseedPressingSaveStore.TrySave(state);
            if (CaptureSection("oilseed_pressing", OilseedPressingSaveStore.TryCapturePersisted(state)))
            {
                _oilseedPressingDirty = false;
            }
        }

        public void FlushOilseedPressingIfDirty()
        {
            if (_oilseedPressingDirty) SaveOilseedPressing();
        }

        public void ResetOilseedPressing()
        {
            _oilseedPressing = null;
            _oilseedPressingDirty = false;
        }
    }
}
