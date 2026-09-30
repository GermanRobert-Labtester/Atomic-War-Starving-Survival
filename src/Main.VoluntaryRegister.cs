// SPDX-License-Identifier: MIT
// ============================================================================
// ASHFALL Plan 253 — Voluntary Register Host Wiring.
// Core VoluntaryRegisterSystem is the authority for high-dose volunteer work
// signatures and dose tracking.
// ============================================================================

using System;
using Ashfall.Core;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private VoluntaryRegisterHostSession? _voluntaryRegister;
        private bool _voluntaryRegisterDirty;

        public VoluntaryRegisterHostSession? VoluntaryRegister => _voluntaryRegister;

        public void SetupVoluntaryRegister()
        {
            if (_voluntaryRegister != null) return;

            _voluntaryRegister = new VoluntaryRegisterHostSession();

            var saved = VoluntaryRegisterSaveStore.TryLoad();
            if (saved != null)
            {
                _voluntaryRegister.RestoreState(saved);
            }

            _voluntaryRegister.StateChanged += () => _voluntaryRegisterDirty = true;
        }

        public void SaveVoluntaryRegister()
        {
            if (_voluntaryRegister == null) return;
            var state = _voluntaryRegister.CaptureState();
            if (CaptureSection("voluntary_register", VoluntaryRegisterSaveStore.TryCapturePersisted(state)))
            {
                _voluntaryRegisterDirty = false;
            }
        }

        public void TickVoluntaryRegister(int day)
        {
            if (_voluntaryRegister == null) SetupVoluntaryRegister();
            if (_voluntaryRegister == null) return;

            _voluntaryRegister.Tick(day);
        }

        public void FlushVoluntaryRegisterIfDirty()
        {
            if (_voluntaryRegisterDirty && _voluntaryRegister != null)
            {
                SaveVoluntaryRegister();
            }
        }

        public void ResetVoluntaryRegister()
        {
            _voluntaryRegister?.Reset();
            _voluntaryRegisterDirty = false;
        }
    }
}
